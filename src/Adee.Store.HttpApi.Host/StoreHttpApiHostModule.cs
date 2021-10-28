using Adee.Store.Attributes;
using Adee.Store.EntityFrameworkCore;
using Adee.Store.MultiTenancy;
using Adee.Store.Utils.Filtes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;

namespace Adee.Store
{
    [DependsOn(
        typeof(StoreHttpApiModule),
        typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(StoreApplicationModule),
        typeof(StoreEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpMultiTenancyModule)
    )]
    public class StoreHttpApiHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
            {
                context.Services.Configure<Volo.Abp.AspNetCore.ExceptionHandling.AbpExceptionHandlingOptions>(options =>
                {
                    options.SendExceptionsDetailsToClients = true;
                });
            }

            ConfigureConventionalControllers();
            ConfigureAuthentication(context, configuration, hostingEnvironment);
            ConfigureLocalization();
            ConfigureCache(configuration);
            ConfigureVirtualFileSystem(context);
            ConfigureRedis(context, configuration, hostingEnvironment);
            ConfigureCors(context, configuration);
            ConfigureSwaggerServices(context, configuration);
        }

        private void ConfigureCache(IConfiguration configuration)
        {
            Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "Store:"; });
        }

        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<StoreDomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}Adee.Store.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<StoreDomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}Adee.Store.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<StoreApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}Adee.Store.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<StoreApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}Adee.Store.Application"));
                });
            }
        }

        private void ConfigureConventionalControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(StoreApplicationModule).Assembly, opts =>
                {
                    opts.RootPath = "store";
                });
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = "Store";

                    if (hostingEnvironment.IsDevelopment())
                    {
                        options.RequireHttpsMetadata = false;
                    }
                });
        }

        private void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAbpSwaggerGenWithOAuth(
                configuration["AuthServer:Authority"],
                new Dictionary<string, string>
                {
                    {"Store", "Store API"}
                },
                options =>
                {
                    typeof(ApiGroupType)
                    .GetFields()
                    .Skip(2)
                    .Select(p => new
                    {
                        p.Name,
                        Description = p.GetCustomAttributes(typeof(DescriptionAttribute), false).OfType<DescriptionAttribute>().Select(p => p.Description).FirstOrDefault()
                    })
                    .ForEach(p =>
                    {
                        options.SwaggerDoc(p.Name, new OpenApiInfo { Title = p.Description, Version = "v1" });
                    });
                    options.SwaggerDoc(ApiGroupType.NoGroup.ToString(), new OpenApiInfo { Title = ApiGroupType.NoGroup.GetDescription(), Version = "v1" });

                    options.CustomSchemaIds(type => type.FullName);
                    options.DocumentFilter<SwaggerEnumFilter>();
                    options.OperationFilter<SwaggerEnumOperationFilter>();
                    options.DocInclusionPredicate((docName, apiDesc) =>
                    {
                        var groupTypes = apiDesc
                            .ActionDescriptor
                            .EndpointMetadata
                            .OfType<ApiGroupAttribute>()
                            .Select(p => p.ApiGroupType.ToString())
                            .ToList();

                        if (groupTypes.IsNull() && docName == ApiGroupType.NoGroup.ToString()) return true;

                        return groupTypes.Contains(docName);
                    });

                    Action<string> includeXmlAction = (name) =>
                    {
                        var apiXmlPath = Path.Combine(AppContext.BaseDirectory, name);
                        if (File.Exists(apiXmlPath))
                        {
                            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, name), true);
                        }
                    };
                    includeXmlAction("Adee.Store.Application.xml");
                    includeXmlAction("Adee.Store.Application.Contracts.xml");
                    includeXmlAction("Adee.Store.Domain.Shared.xml");
                    includeXmlAction("Adee.Store.HttpApi.xml");
                });
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            });
        }

        private void ConfigureRedis(
            ServiceConfigurationContext context,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {
            if (!hostingEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                context.Services
                    .AddDataProtection()
                    .PersistKeysToStackExchangeRedis(redis, "Store-Protection-Keys");
            }
        }

        private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            app.UseHttpsRedirection();
            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseAuthorization();

            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                typeof(ApiGroupType)
                    .GetFields()
                    .Skip(2)
                    .Select(p => new
                    {
                        p.Name,
                        Description = p.GetCustomAttributes(typeof(DescriptionAttribute), false).OfType<DescriptionAttribute>().Select(p => p.Description).FirstOrDefault()
                    })
                    .ForEach(p =>
                    {
                        options.SwaggerEndpoint($"/swagger/{p.Name}/swagger.json", p.Description);
                    });
                options.SwaggerEndpoint($"/swagger/{ApiGroupType.NoGroup.ToString()}/swagger.json", ApiGroupType.NoGroup.GetDescription());

                var configuration = context.GetConfiguration();
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
                options.OAuthScopes("Store");
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseUnitOfWork();
            app.UseConfiguredEndpoints();
        }
    }
}

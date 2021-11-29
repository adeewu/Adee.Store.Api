using Adee.Store.Attributes;
using Adee.Store.EntityFrameworkCore;
using Adee.Store.MultiTenancy;
using Adee.Store.Utils.Filtes;
using Hangfire;
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
using Volo.Abp.BackgroundJobs;
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

            context.Services.Configure<AppOptions>(options => options = GetAppOptions(configuration));

            context.Services.Configure<AuthServerOptions>(options => options = GetAuthServerOptions(configuration));

            ConfigureConventionalControllers();
            ConfigureAuthentication(context, configuration, hostingEnvironment);
            ConfigureLocalization();
            ConfigureCache(configuration);
            ConfigureVirtualFileSystem(context);
            ConfigureRedis(context, configuration, hostingEnvironment);
            ConfigureCors(context, configuration);
            ConfigureSwaggerServices(context, configuration);
            ConfigureBackgroudJob(context, hostingEnvironment);
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
                    var authServer = GetAuthServerOptions(configuration);

                    options.Authority = authServer.Authority;
                    options.RequireHttpsMetadata = authServer.RequireHttpsMetadata;
                    options.Audience = "Store";

                    if (hostingEnvironment.IsDevelopment())
                    {
                        options.RequireHttpsMetadata = false;
                    }
                });
        }

        private void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
        {
            var authServer = GetAuthServerOptions(configuration);
            context.Services.AddAbpSwaggerGenWithOAuth(
                authServer.Authority,
                new Dictionary<string, string>
                {
                    {"Store", "Store API"}
                },
                options =>
                {
                    options.SwaggerDoc(ApiGroupType.NoGroup.ToString(), new OpenApiInfo { Title = ApiGroupType.NoGroup.GetDescription(), Version = "v1" });
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

                    options.CustomOperationIds(apiDesc =>
                    {
                        var apiPaths = apiDesc
                            .RelativePath
                            .Replace("{", string.Empty)
                            .Replace("}", string.Empty)
                            .Split(new char[] { '/', '-' });

                        return new List<string> { apiDesc.HttpMethod.ToLower() }
                            .Union(apiPaths)
                            .Select(p => System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(p))
                            .JoinAsString(string.Empty);
                    });

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

                        if (groupTypes.IsNull())
                        {
                            //以Volo.Abp.、Page.Abp.开头归到Adb分组
                            if (apiDesc.ActionDescriptor.DisplayName.StartsWith("Volo.Abp.")
                            || apiDesc.ActionDescriptor.DisplayName.StartsWith("Pages.Abp."))
                            {
                                return docName == ApiGroupType.Abp.ToString();
                            }

                            //没有标记的归到未分组
                            return docName == ApiGroupType.NoGroup.ToString();
                        }

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
                var appOptions = GetAppOptions(configuration);
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(appOptions.CorsOriginArray)
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        private void ConfigureBackgroudJob(ServiceConfigurationContext context, IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                context.Services.Configure<AbpBackgroundJobOptions>(options =>
                {
                    options.IsJobExecutionEnabled = false;
                });
            }
        }

        private AuthServerOptions GetAuthServerOptions(IConfiguration configuration)
        {
            var options = configuration.GetSection("AuthServer").Get<AuthServerOptions>();

            var versionName = configuration.GetValue("VERSION_NAME", string.Empty);
            if (versionName.IsNullOrWhiteSpace() == false)
            {
                options.Authority = $"https://{versionName}-ids4.adee.huobsj.com";
            }

            return options;
        }

        private AppOptions GetAppOptions(IConfiguration configuration)
        {
            var options = configuration.GetSection("App").Get<AppOptions>();

            var versionName = configuration.GetValue("VERSION_NAME", string.Empty);
            if (versionName.IsNullOrWhiteSpace() == false)
            {
                options.SelfUrl = $"https://{versionName}-api.adee.huobsj.com";
                options.CorsOrigins = $"{options.CorsOrigins},https://{versionName}-vue.adee.huobsj.com";
            }

            return options;
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

            app.UseHangfireDashboard();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{ApiGroupType.NoGroup.ToString()}/swagger.json", ApiGroupType.NoGroup.GetDescription());
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

                var authServerOptions = GetAuthServerOptions(context.GetConfiguration());
                options.OAuthClientId(authServerOptions.SwaggerClientId);
                options.OAuthClientSecret(authServerOptions.SwaggerClientSecret);
                options.OAuthScopes("Store");
                options.EnableDeepLinking();
                options.DisplayOperationId();
                options.DisplayRequestDuration();
                options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseUnitOfWork();
            app.UseConfiguredEndpoints();
        }
    }
}

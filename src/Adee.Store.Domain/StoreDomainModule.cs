using Adee.Store.Domain.Pays.TianQue;
using Adee.Store.Domain.Pays.TianQue.Models;
using Adee.Store.Domain.Tenants;
using Adee.Store.MultiTenancy;
using Adee.Store.Pays;
using Adee.Store.Pays.Utils.Helpers;
using Adee.Store.Wechats.Components;
using Flurl.Http;
using Hangfire;
using Hangfire.MySql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net;
using System.Net.Http;
using System.Transactions;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.IdentityServer;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace Adee.Store
{
    [DependsOn(
        typeof(StoreDomainSharedModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpBackgroundJobsDomainModule),
        typeof(AbpFeatureManagementDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpIdentityServerDomainModule),
        typeof(AbpPermissionManagementDomainIdentityServerModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpTenantManagementDomainModule),
        typeof(AbpEmailingModule),
        typeof(AbpBackgroundJobsHangfireModule)
    )]
    public class StoreDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });

#if DEBUG
            context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif

#if DEBUG
            FlurlHttp.Configure(settings =>
            {
                settings.HttpClientFactory = new ProxyHttpClientFactory("proxy.adee.huobsj.com", 8892);
            });
#endif

            context.Services.AddHttpClient();
            context.Services.AddTransient<ICurrentTenantExt, CurrentTenantExt>();

            var config = context.Services.GetConfiguration();
            Configure<TianQueOptions>(config.GetSection($"PayConfig:{TianQueOptions.Name}"));

            Configure<PayOptions>(options =>
            {
                options.AddPayProviders<TianQuePay>();
            });

            context.Services.AddHttpClient<ICommonClient, CommonClient>()
#if DEBUG
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var httpclientHandler = new HttpClientHandler();
                    httpclientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true;
                    httpclientHandler.Proxy = new WebProxy("proxy.adee.huobsj.com", 8892);
                    //httpclientHandler.Proxy.Credentials = new NetworkCredential("decerp", "decerp2020");

                    return httpclientHandler;
                })
#endif
                ;

            context.Services.AddHangfire(globalConfig =>
            {
                var connectionString = config["ConnectionStrings:Default"];
                if (connectionString.ToLower().Contains("Allow User Variables".ToLower()) == false)
                {
                    connectionString += connectionString.EnsureEndsWith(';') + "Allow User Variables=True;";
                }

                globalConfig.UseStorage(new MySqlStorage(
                    connectionString,
                    new MySqlStorageOptions
                    {
                        TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                        QueuePollInterval = TimeSpan.FromSeconds(15),
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        PrepareSchemaIfNecessary = true,
                        DashboardJobListLimit = 50000,
                        TransactionTimeout = TimeSpan.FromMinutes(1),
                        TablesPrefix = "Hangfire"
                    }));
            });
        }
    }
}

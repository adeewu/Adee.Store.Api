
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Pays.Jobs
{
    /// <summary>
    /// 租户后台任务
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    public abstract class StoreTenantBackgroundJob<TArgs> : AsyncBackgroundJob<TArgs> where TArgs : IMultiTenant
    {
        internal readonly ICurrentTenant _currentTenant;

        public StoreTenantBackgroundJob(ICurrentTenant currentTenant)
        {
            _currentTenant = currentTenant;
        }

        public sealed override async Task ExecuteAsync(TArgs args)
        {
            try
            {
                using (_currentTenant.Change(args.TenantId))
                {
                    await ToExecuteAsync(args);
                }
            }
            catch (Exception ex)
            {
                await ExceptionAsync(ex);
            }
        }

        public abstract Task ToExecuteAsync(TArgs args);

        public virtual async Task ExceptionAsync(Exception exception)
        {
            Logger.LogError(exception, exception.Message);

            await Task.CompletedTask;
        }
    }
}

using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.MultiTenancy;

/// <summary>
/// 租户后台任务
/// </summary>
/// <typeparam name="TArgs"></typeparam>
public abstract class StoreTenantBackgroundJob<TArgs> : BackgroundJob<TArgs> where TArgs : IMultiTenant
{
    internal readonly ICurrentTenant _currentTenant;

    public StoreTenantBackgroundJob(ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
    }

    public sealed override void Execute(TArgs args)
    {
        try
        {
            using (_currentTenant.Change(args.TenantId))
            {
                var task = ExecuteAsync(args);
                task.Wait();
            }
        }
        catch (Exception ex)
        {
            var task = ExceptionAsync(ex);
            task.Wait();
        }
    }

    public abstract Task ExecuteAsync(TArgs args);

    public virtual async Task ExceptionAsync(Exception exception)
    {
        Logger.LogError(exception, exception.Message);

        await Task.CompletedTask;
    }
}
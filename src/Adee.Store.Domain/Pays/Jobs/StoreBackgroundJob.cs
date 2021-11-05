
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;

namespace Adee.Store.Pays.Jobs
{
    /// <summary>
    /// 后台任务
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    public abstract class StoreBackgroundJob<TArgs> : AsyncBackgroundJob<TArgs>
    {
        public sealed override async Task ExecuteAsync(TArgs args)
        {
            Logger.LogDebug($"开始执行{typeof(TArgs).Name}任务");

            try
            {
                await ToExecuteAsync(args);
            }
            catch (Exception ex)
            {
                await ExceptionAsync(args, ex);
            }

            Logger.LogDebug($"结束执行{typeof(TArgs).Name}任务");
        }

        public abstract Task ToExecuteAsync(TArgs args);

        public virtual async Task ExceptionAsync(TArgs args, Exception exception)
        {
            Logger.LogError(exception, exception.Message);

            await Task.CompletedTask;
        }
    }
}
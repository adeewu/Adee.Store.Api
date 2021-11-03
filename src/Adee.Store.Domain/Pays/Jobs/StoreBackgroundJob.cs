
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
    public abstract class StoreBackgroundJob<TArgs> : BackgroundJob<TArgs>
    {
        public sealed override void Execute(TArgs args)
        {
            try
            {
                var task = ExecuteAsync(args);
                task.Wait();
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
}
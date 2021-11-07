
using Adee.Store.Pays.Jobs;
using Adee.Store.Wechats.Components.Models;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Adee.Store.Wechats.Components.Jobs.UpdateAccessToken
{
    /// <summary>
    /// 更新令牌
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    public class UpdateAccessTokenBackgroundJob : StoreBackgroundJob<UpdateAccessTokenArgs>, ITransientDependency
    {
        private readonly IWechatComponentManager _wechatComponentManager;
        private readonly IDistributedCache<UpdateAccessTokenArgs> _cache;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public UpdateAccessTokenBackgroundJob(
            IWechatComponentManager wechatComponentManager,
            IDistributedCache<UpdateAccessTokenArgs> cache,
            IBackgroundJobManager backgroundJobManager)
        {
            _wechatComponentManager = wechatComponentManager;
            _cache = cache;
            _backgroundJobManager = backgroundJobManager;
        }

        [UnitOfWork(isTransactional: false)]
        public override async Task ToExecuteAsync(UpdateAccessTokenArgs args)
        {
            var argsOfCache = await _cache.GetAsync(args.AppId);
            if (argsOfCache.IsNotNull())
            {
                CheckHelper.IsTrue(args.UpdateTime >= argsOfCache.UpdateTime, $"已产生新任务，旧任务放弃，{nameof(args.AppId)}:{args.AppId}");
            }

            var componentAccessTokenCacheItem = await _wechatComponentManager.UpdateAccessToken(args.AppId);
            CheckHelper.IsNotNull(componentAccessTokenCacheItem, name: nameof(componentAccessTokenCacheItem));

            args.LastDelay = componentAccessTokenCacheItem.ExpiresIn - WechatComponentConsts.ForwardUpdateAccessToken;
            await _backgroundJobManager.EnqueueAsync(args, delay: TimeSpan.FromSeconds(args.LastDelay));
        }

        public override async Task ExceptionAsync(UpdateAccessTokenArgs args, Exception exception)
        {
            await base.ExceptionAsync(args, exception);

            await _backgroundJobManager.EnqueueAsync(args, delay: TimeSpan.FromSeconds(args.LastDelay));
        }
    }
}
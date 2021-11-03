
using Adee.Store.Pays.Jobs;
using Adee.Store.Wechats.Components.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Wechats.Components.Jobs.UpdateAccessToken
{
    /// <summary>
    /// 更新第三方平台令牌
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    public abstract class UpdateComponentAccessTokenBackgroundJob : StoreBackgroundJob<UpdateComponentAccessTokenArgs>
    {
        private readonly WechatComponentManager _wechatComponentManager;
        private readonly IDistributedCache<UpdateComponentAccessTokenArgs> _cache;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public UpdateComponentAccessTokenBackgroundJob(
            WechatComponentManager wechatComponentManager,
            IDistributedCache<UpdateComponentAccessTokenArgs> cache,
            IBackgroundJobManager backgroundJobManager)
        {
            _wechatComponentManager = wechatComponentManager;
            _cache = cache;
            _backgroundJobManager = backgroundJobManager;
        }

        public override async Task ExecuteAsync(UpdateComponentAccessTokenArgs args)
        {
            var argsOfCache = await _cache.GetAsync(args.ComponentAppId);
            if (argsOfCache.IsNotNull())
            {
                CheckHelper.IsTrue(args.UpdateTime >= argsOfCache.UpdateTime, $"已产生新任务，旧任务放弃，{nameof(args.ComponentAppId)}:{args.ComponentAppId}");
            }

            var componentAccessTokenCacheItem = await _wechatComponentManager.UpdateComponentAccessToken(args.ComponentAppId);
            CheckHelper.IsNotNull(componentAccessTokenCacheItem, name: nameof(componentAccessTokenCacheItem));

            await _backgroundJobManager.EnqueueAsync(new UpdateComponentAccessTokenArgs
            {
                ComponentAppId = args.ComponentAppId
            }, delay: TimeSpan.FromSeconds(componentAccessTokenCacheItem.ExpiresIn - 60 * 10));
        }
    }
}
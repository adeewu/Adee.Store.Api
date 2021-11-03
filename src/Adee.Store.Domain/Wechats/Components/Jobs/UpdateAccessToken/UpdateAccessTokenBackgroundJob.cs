
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
    /// 租户后台任务
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    public abstract class UpdateAccessTokenBackgroundJob : StoreBackgroundJob<UpdateAccessTokenArgs>
    {
        private readonly WechatComponentManager _wechatComponentManager;
        private readonly IDistributedCache<UpdateAccessTokenArgs> _cache;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public UpdateAccessTokenBackgroundJob(
            WechatComponentManager wechatComponentManager,
            IDistributedCache<UpdateAccessTokenArgs> cache,
            IBackgroundJobManager backgroundJobManager)
        {
            _wechatComponentManager = wechatComponentManager;
            _cache = cache;
            _backgroundJobManager = backgroundJobManager;
        }

        public override async Task ExecuteAsync(UpdateAccessTokenArgs args)
        {
            var argsOfCache = await _cache.GetAsync(args.AppId);
            if (argsOfCache.IsNotNull())
            {
                CheckHelper.IsTrue(args.UpdateTime >= argsOfCache.UpdateTime, $"已产生新任务，旧任务放弃，{nameof(args.AppId)}:{args.AppId}");
            }

            var componentAccessTokenCacheItem = await _wechatComponentManager.UpdateComponentAccessToken(args.ComponentAppId);
            CheckHelper.IsNotNull(componentAccessTokenCacheItem, name: nameof(componentAccessTokenCacheItem));

            await _backgroundJobManager.EnqueueAsync(new UpdateComponentAccessTokenArgs
            {
                ComponentAppId = args.ComponentAppId
            }, delay: TimeSpan.FromSeconds(componentAccessTokenCacheItem.ExpiresIn - 60));
        }
    }
}
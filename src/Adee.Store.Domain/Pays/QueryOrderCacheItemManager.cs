using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Adee.Store.Pays
{
    public class QueryOrderCacheItemManager : ITransientDependency
    {
        private readonly IDistributedCache<QueryOrderCacheItem> _distributedCache;

        public QueryOrderCacheItemManager(IDistributedCache<QueryOrderCacheItem> distributedCache)
        {
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<QueryOrderCacheItem> GetAsync(string orderId)
        {
            return await _distributedCache.GetAsync(orderId);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task SetAsync(QueryOrderCacheItem item)
        {
            var keys = GetNotNullKeys(item);

            await _distributedCache.SetManyAsync(keys.Select(p => new KeyValuePair<string, QueryOrderCacheItem>(p, item)));
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task RemoveAsync(QueryOrderCacheItem item)
        {
            var keys = GetNotNullKeys(item);

            await _distributedCache.RemoveManyAsync(keys);
        }

        private List<string> GetNotNullKeys(QueryOrderCacheItem item)
        {
            var keys = new List<string>();
            if (item.PayOrganizationOrderId.IsNullOrWhiteSpace() == false)
            {
                keys.Add(item.PayOrganizationOrderId);
            }
            if (item.BusinessOrderId.IsNullOrWhiteSpace() == false)
            {
                keys.Add(item.BusinessOrderId);
            }
            if (item.PayOrderId.IsNullOrWhiteSpace() == false)
            {
                keys.Add(item.PayOrderId);
            }

            CheckHelper.IsNotNull(keys, "数据错误，未能正确设置缓存");

            return keys;
        }
    }
}

using Adee.Store.ElasticSearchs;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adee.Store.ElasticsearchRepository
{
    public class ElasticSearchLogRepository : ElasticsearchBaseRepository, IElasticSearchLogRepository
    {
        private readonly IConfiguration _configuration;

        public ElasticSearchLogRepository(
            IElasticsearchProvider elasticsearchProvider,
            IConfiguration configuration) : base(elasticsearchProvider)
        {
            _configuration = configuration;
        }

        public async Task<PageResultModel<ElasticSearchLogOutput>> GetAsync(ElasticSearchLogInput input)
        {
            var IndexName = _configuration.GetValue<string>("ElasticSearch:SearchIndexFormat");

            // 默认查询当天
            input.StartTime = input.StartTime?.AddMilliseconds(-1) ?? DateTime.Now.Date.AddMilliseconds(-1);
            input.EndTime = input.EndTime?.AddDays(1).AddMilliseconds(-1) ?? DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
            var mustFilters = new List<Func<QueryContainerDescriptor<ElasticSearchLogOutput>, QueryContainer>>
            {
                t => t.DateRange(f => f.Field(fd => fd.CreationTime).TimeZone("Asia/Shanghai").GreaterThanOrEquals(input.StartTime.Value)),
                t => t.DateRange(f => f.Field(fd => fd.CreationTime).TimeZone("Asia/Shanghai").LessThanOrEquals(input.EndTime.Value))
            };

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                mustFilters.Add(t => t.MatchPhrase(f => f.Field(fd => fd.Message).Query(input.Filter.Trim())));
            }

            var result = await Client.SearchAsync<ElasticSearchLogOutput>(e => e
                .Index(IndexName)
                .From(input.GetSkipCount())
                .Size(input.PageSize)
                .Sort(s => s.Descending(sd => sd.CreationTime))
                .Query(q => q.Bool(qb => qb.Filter(mustFilters))));

            if (result.HitsMetadata != null)
            {
                return new PageResultModel<ElasticSearchLogOutput>(result.HitsMetadata.Total.Value, result.Documents.ToList());
            }

            return null;
        }
    }
}
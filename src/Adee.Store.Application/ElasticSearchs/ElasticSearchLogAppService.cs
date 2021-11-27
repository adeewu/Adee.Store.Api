using Adee.Store.Attributes;
using Adee.Store.ElasticsearchRepository;
using System.Threading.Tasks;

namespace Adee.Store.ElasticSearchs
{
    /// <summary>
    /// ��־ES
    /// </summary>
    [ApiGroup(ApiGroupType.ElasticSearch)]
    public class ElasticSearchLogAppService : StoreAppService, IElasticSearchLogAppService
    {
        private readonly IElasticSearchLogRepository _elasticSearchLogRepository;

        public ElasticSearchLogAppService(IElasticSearchLogRepository elasticSearchLogRepository)
        {
            _elasticSearchLogRepository = elasticSearchLogRepository;
        }

        /// <summary>
        /// ��ҳ��־
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PageResultModel<ElasticSearchLogOutput>> GetListAsync(ElasticSearchLogInput input)
        {
            return _elasticSearchLogRepository.GetAsync(input);
        }
    }
}
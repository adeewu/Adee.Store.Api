using Nest;
using Volo.Abp.Domain.Services;

namespace Adee.Store.ElasticsearchRepository
{
    public abstract class ElasticsearchBaseRepository : DomainService
    {
        private readonly IElasticsearchProvider _elasticsearchProvider;

        public ElasticsearchBaseRepository(IElasticsearchProvider elasticsearchProvider)
        {
            _elasticsearchProvider = elasticsearchProvider;
        }

        protected IElasticClient Client => _elasticsearchProvider.GetElasticClient();
    }
}
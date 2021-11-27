using Nest;
using Volo.Abp.DependencyInjection;

namespace Adee.Store.ElasticsearchRepository
{
    public interface IElasticsearchProvider : ISingletonDependency
    {
        IElasticClient GetElasticClient();
    }
}
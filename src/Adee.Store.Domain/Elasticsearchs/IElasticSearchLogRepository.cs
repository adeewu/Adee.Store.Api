using Adee.Store.ElasticSearchs;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Adee.Store.ElasticsearchRepository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IElasticSearchLogRepository : ITransientDependency
    {
        /// <summary>
        /// 分页查询es日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageResultModel<ElasticSearchLogOutput>> GetAsync(ElasticSearchLogInput input);
    }
}
using System.Threading.Tasks;

namespace Adee.Store.ElasticSearchs
{
    /// <summary>
    /// 日志ES
    /// </summary>
    public interface IElasticSearchLogAppService
    {
        /// <summary>
        /// 分页日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageResultModel<ElasticSearchLogOutput>> GetListAsync(ElasticSearchLogInput input);
    }
}
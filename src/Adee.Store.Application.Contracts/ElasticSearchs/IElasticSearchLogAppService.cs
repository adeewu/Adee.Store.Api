using System.Threading.Tasks;

namespace Adee.Store.ElasticSearchs
{
    /// <summary>
    /// ��־ES
    /// </summary>
    public interface IElasticSearchLogAppService
    {
        /// <summary>
        /// ��ҳ��־
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageResultModel<ElasticSearchLogOutput>> GetListAsync(ElasticSearchLogInput input);
    }
}
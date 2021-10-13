using Adee.Store.Pays;
using System.Threading.Tasks;

namespace Adee.Store.Domain.Pays
{
    /// <summary>
    /// 支付业务
    /// </summary>
    public interface IBusinessTypeProvider
    {
        Task<TPayResponse> Excute<TPayRequest, TPayResponse>(TPayRequest request) where TPayRequest : PayTaskRequest where TPayResponse : PayResponse;
    }
}

using System.Threading.Tasks;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付业务
    /// </summary>
    public interface IPayProvider
    {
        Task<TPayResponse> Excute<TPayRequest, TPayResponse>(TPayRequest request) where TPayRequest : PayTaskRequest where TPayResponse : PayResponse;
    }
}

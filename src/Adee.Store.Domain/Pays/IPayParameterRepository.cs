using System;
using System.Threading.Tasks;
using Adee.Store.Pays;
using Volo.Abp.Domain.Repositories;

namespace Adee.Store.Pays
{
    public interface IPayParameterRepository : IRepository<PayParameter, Guid>
    {
        /// <summary>
        /// 获取支付参数
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="paymentType"></param>
        /// <param name="version">默认最新版本</param>
        /// <returns></returns>
        Task<PayParameter> GetPayParameter(PaymentType paymentType, long? version = null);
    }
}

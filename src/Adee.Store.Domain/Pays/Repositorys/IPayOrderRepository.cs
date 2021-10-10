using System;
using System.Threading.Tasks;
using Adee.Store.Pays;
using Volo.Abp.Domain.Repositories;

namespace Adee.Store.Pays
{
    public interface IPayOrderRepository : IRepository<PayOrder, Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessOrderId"></param>
        /// <returns></returns>
        Task<bool> ExistBusinessOrderId(string businessOrderId);
    }
}

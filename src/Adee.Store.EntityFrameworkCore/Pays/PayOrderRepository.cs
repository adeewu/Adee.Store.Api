using Adee.Store.Pays;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Adee.Store.EntityFrameworkCore.Pays
{
    public class PayOrderRepository : EfCoreRepository<StoreDbContext, PayOrder, Guid>, IPayOrderRepository
    {
        public PayOrderRepository(IDbContextProvider<StoreDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task<bool> ExistBusinessOrderId(string businessOrderId)
        {
            var dbSet = await GetDbSetAsync();
            var count = await dbSet.Where(p => p.BusinessOrderId == businessOrderId).CountAsync();

            return count > 0;
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Adee.Store.Pays;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Adee.Store.EntityFrameworkCore.Pays
{
    public class PayParameterRepository : EfCoreRepository<StoreDbContext, PayParameter, Guid>, IPayParameterRepository
    {
        public PayParameterRepository(IDbContextProvider<StoreDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task<PayParameter> GetPayParameter(PaymentType paymentType, long? version = null)
        {
            var dbSet = await GetDbSetAsync();

            var query = dbSet.Where(p => (p.PaymentType.HasValue && p.PaymentType.Value == paymentType) || p.PaymentType.HasValue == false);
            if (version.HasValue)
            {
                query = query.Where(p => p.Version == version);
            }

            var result = await query.SingleOrDefaultAsync();
            Check.NotNull(result, nameof(result));

            if (result.PaymentType.HasValue == false)
            {
                result.PaymentType = paymentType;
            }

            return result;
        }
    }
}

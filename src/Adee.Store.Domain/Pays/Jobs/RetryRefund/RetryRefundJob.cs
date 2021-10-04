using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Pays
{
    public class RetryRefundJob : StoreTenantBackgroundJob<RetryRefundArgs>, ITransientDependency
    {
        private readonly PayManager _payManager;

        public RetryRefundJob(
            PayManager payManager,
            ICurrentTenant currentTenant
            ) : base(currentTenant)
        {
            _payManager = payManager;
        }

        public override async Task ExecuteAsync(RetryRefundArgs args)
        {
            await _payManager.Refund(args);
        }
    }
}

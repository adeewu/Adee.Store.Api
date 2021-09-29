using System;
using System.Threading.Tasks;
using Adee.Store.Domain.Pays;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Pays
{
    public class RetryRefundJob : BackgroundJob<RetryRefundArgs>, ITransientDependency
    {
        private readonly PayManager _payManager;
        private readonly ICurrentTenant _currentTenant;

        public RetryRefundJob(
            PayManager payManager,
            ICurrentTenant currentTenant
            )
        {
            _payManager = payManager;
            _currentTenant = currentTenant;
        }

        public override void Execute(RetryRefundArgs args)
        {
            _currentTenant.Change(args.TenantId);

            var executeTask = ExecuteAsync(args);
            executeTask.Wait();
        }

        private async Task ExecuteAsync(RetryRefundArgs args)
        {
            await _payManager.Refund(args);
        }
    }
}

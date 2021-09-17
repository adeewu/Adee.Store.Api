using System;
using System.Threading.Tasks;
using Adee.Store.Domain.Pays;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Pays
{
    public class QueryPayStatusJob : BackgroundJob<QueryPayStatusArgs>, ITransientDependency
    {
        private readonly PayManager _payManager;
        private readonly ICurrentTenant _currentTenant;

        public QueryPayStatusJob(
            PayManager payManager,
            ICurrentTenant currentTenant
            )
        {
            _payManager = payManager;
            _currentTenant = currentTenant;
        }

        public override void Execute(QueryPayStatusArgs args)
        {
            _currentTenant.Change(args.TenantId);

            var executeTask = ExecuteAsync(args, PayOrderId.Create(args.PayOrderId));
            executeTask.Wait();
        }

        private async Task ExecuteAsync(QueryPayStatusArgs args, PayOrderId payOrderId)
        {
            await _payManager.Query(args);
        }
    }
}

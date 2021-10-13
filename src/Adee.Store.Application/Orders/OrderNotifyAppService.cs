using Adee.Store.Attributes;
using Adee.Store.Pays;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Adee.Store.Orders
{
    /// <summary>
    /// 订单支付成功回调
    /// </summary>
    [ApiGroup(ApiGroupType.Order)]
    public class OrderNotifyAppService : StoreAppService, ITransientDependency
    {
        public OrderNotifyAppService()
        {

        }

        public async Task Notify(OrderResult result)
        {
            Logger.LogDebug($"通知内容：{result.ToJsonString()}");

            if (result.BusinessType == BusinessType.NoCodePay)
            {
                await NoCodePay(result);
            }

            if (result.BusinessType == BusinessType.WebCheckout)
            {
                await WebCheckout(result);
            }
        }

        private Task NoCodePay(OrderResult result)
        {
            return Task.CompletedTask;
        }

        private Task WebCheckout(OrderResult result)
        {
            return Task.CompletedTask;
        }
    }
}

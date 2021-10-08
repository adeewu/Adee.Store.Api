using Adee.Store.Domain.Shared.Utils.Helpers;
using Adee.Store.Pays;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Adee.Store.Orders
{
    /// <summary>
    /// 订单支付成功回调
    /// </summary>
    public class OrderNotifyAppService : StoreAppService, ITransientDependency
    {
        public OrderNotifyAppService()
        {

        }

        public async Task Notify(PayTaskOrderResult result)
        {
            if (result.BusinessType == BusinessType.NoCodePay)
            {
                await NoCodePay(result);
            }

            if (result.BusinessType == BusinessType.WebCheckout)
            {
                await WebCheckout(result);
            }
        }

        private Task NoCodePay(PayTaskOrderResult result)
        {
            throw new NotImplementedException();
        }

        private Task WebCheckout(PayTaskOrderResult result)
        {
            throw new NotImplementedException();
        }
    }
}

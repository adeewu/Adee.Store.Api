using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adee.Store.Pays;
using Volo.Abp.DependencyInjection;

namespace Adee.Store.Domain.Pays
{
    /// <summary>
    /// 支付业务
    /// </summary>
    public interface IBusinessTypeProvider
    {
        Task<TPayResponse> Excute<TPayRequest, TPayResponse>(TPayRequest request) where TPayRequest : PayRequest where TPayResponse : PayResponse;
    }
}

using System;
using System.Collections.Generic;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付配置
    /// </summary>
    public class PayOptions
    {
        public PayOptions()
        {
            PayProviderTypes = new List<Type>();
        }

        /// <summary>
        /// 支付提供器
        /// </summary>
        public List<Type> PayProviderTypes { get; private set; }

        /// <summary>
        /// 新增支付提供器
        /// </summary>
        /// <typeparam name="TPayProvider"></typeparam>
        public void AddPayProviders<TPayProvider>() where TPayProvider : IPayProvider
        {
            PayProviderTypes.Add(typeof(TPayProvider));
        }
    }
}

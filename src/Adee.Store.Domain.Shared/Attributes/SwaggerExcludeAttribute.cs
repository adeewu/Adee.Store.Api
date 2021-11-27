using System;
using System.ComponentModel;

namespace Adee.Store.Attributes
{
    /// <summary>
    /// 例外不展示
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SwaggerExcludeAttribute : Attribute
    {
    }

    /// <summary>
    /// 接口分组标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiGroupAttribute : Attribute
    {
        /// <summary>
        /// 分组类型
        /// </summary>
        public ApiGroupType ApiGroupType { get; set; }

        public ApiGroupAttribute(ApiGroupType type)
        {
            ApiGroupType = type;
        }
    }

    /// <summary>
    /// 接口分组
    /// </summary>
    public enum ApiGroupType
    {
        /// <summary>
        /// 未分组
        /// </summary>
        [Description("未分组")]
        NoGroup,

        /// <summary>
        /// 支付接口
        /// </summary>
        [Description("支付接口")]
        Pay,

        /// <summary>
        /// 商品接口
        /// </summary>
        [Description("商品接口")]
        Product,

        /// <summary>
        /// 订单接口
        /// </summary>
        [Description("订单接口")]
        Order,

        /// <summary>
        /// 第三方平台接口
        /// </summary>
        [Description("第三方平台接口")]
        WechatComponent,

        /// <summary>
        /// Identity Server接口
        /// </summary>
        [Description("Identity Server接口")]
        IdentityServer,

        /// <summary>
        /// ElasticSearchs接口
        /// </summary>
        [Description("ElasticSearchs接口")]
        ElasticSearch,

        /// <summary>
        /// 系统基础接口
        /// </summary>
        [Description("系统基础接口")]
        SystemBase,
    }
}

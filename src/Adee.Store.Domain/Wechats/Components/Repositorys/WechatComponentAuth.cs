using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Wechats.Components.Repositorys
{
    /// <summary>
    /// 第三方平台授权
    /// </summary>
    public class WechatComponentAuth : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public WechatComponentAuth(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 租户Id
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 第三方平台AppId
        /// </summary>
        public string ComponentAppId { get; set; }

        /// <summary>
        /// 授权AppId
        /// </summary>
        public string AuthAppId { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string AuthorizationCode { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string AuthorizerRefreshToken { get; set; }

        /// <summary>
        /// 授权权限集
        /// </summary>
        public string FuncInfo { get; set; }
    }
}

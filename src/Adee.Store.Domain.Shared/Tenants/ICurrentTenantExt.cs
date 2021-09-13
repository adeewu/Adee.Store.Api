using System;
using System.Threading.Tasks;

namespace Adee.Store.Domain.Shared.Tenants
{
    /// <summary>
    /// 租户扩展信息
    /// </summary>
    public interface ICurrentTenantExt
    {
        /// <summary>
        /// 软件编号
        /// </summary>
        Task<string> GetSoftwareCodeAsync();
    }
}

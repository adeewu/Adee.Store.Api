using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement
{
    public static class UserSettingManagerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingManager"></param>
        /// <param name="name"></param>
        /// <param name="userId"></param>
        /// <param name="keyParam"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static Task<string> GetOrNullForUserAsync(this ISettingManager settingManager, [NotNull] string name, Guid userId, [NotNull] object keyParam, bool fallback = true)
        {
            return settingManager.GetOrNullAsync(name, UserSettingValueProvider.ProviderName, GetKey(keyParam, userId), fallback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingManager"></param>
        /// <param name="name"></param>
        /// <param name="keyParam"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static Task<string> GetOrNullForCurrentUserAsync(this ISettingManager settingManager, [NotNull] string name, [NotNull] object keyParam, bool fallback = true)
        {
            return settingManager.GetOrNullAsync(name, UserSettingValueProvider.ProviderName, GetKey(keyParam), fallback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingManager"></param>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="keyParam"></param>
        /// <param name="forceToSet"></param>
        /// <returns></returns>
        public static Task SetForUserAsync(this ISettingManager settingManager, Guid userId, [NotNull] string name, [CanBeNull] string value, [NotNull] object keyParam, bool forceToSet = false)
        {
            return settingManager.SetAsync(name, value, UserSettingValueProvider.ProviderName, GetKey(keyParam, userId), forceToSet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingManager"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="keyParam"></param>
        /// <param name="forceToSet"></param>
        /// <returns></returns>
        public static Task SetForCurrentUserAsync(this ISettingManager settingManager, [NotNull] string name, [CanBeNull] string value, [NotNull] object keyParam, bool forceToSet = false)
        {
            return settingManager.SetAsync(name, value, UserSettingValueProvider.ProviderName, GetKey(keyParam), forceToSet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyParam"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetKey(object keyParam, object key = null)
        {
            if (keyParam.IsNull())
            {
                return key == null ? null : key.ToString();
            }

            var keys = keyParam.GetType().GetProperties().Select(p => $"{p.Name}:{p.GetValue(keyParam).ToString()}").ToList();
            if (key != null)
            {
                keys.AddFirst(key.ToString());
            }

            return keys.JoinAsString();
        }
    }

    public static class TenantSettingManagerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingManager"></param>
        /// <param name="name"></param>
        /// <param name="tenantId"></param>
        /// <param name="keyParam"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static Task<string> GetOrNullForTenantAsync(this ISettingManager settingManager, [NotNull] string name, Guid tenantId, [NotNull] object keyParam, bool fallback = true)
        {
            return settingManager.GetOrNullAsync(name, TenantSettingValueProvider.ProviderName, UserSettingManagerExtensions.GetKey(keyParam, tenantId), fallback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingManager"></param>
        /// <param name="name"></param>
        /// <param name="keyParam"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static Task<string> GetOrNullForCurrentTenantAsync(this ISettingManager settingManager, [NotNull] string name, [NotNull] object keyParam, bool fallback = true)
        {
            return settingManager.GetOrNullAsync(name, TenantSettingValueProvider.ProviderName, UserSettingManagerExtensions.GetKey(keyParam), fallback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingManager"></param>
        /// <param name="tenantId"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="keyParam"></param>
        /// <param name="forceToSet"></param>
        /// <returns></returns>
        public static Task SetForTenantAsync(this ISettingManager settingManager, Guid tenantId, [NotNull] string name, [CanBeNull] string value, [NotNull] object keyParam, bool forceToSet = false)
        {
            return settingManager.SetAsync(name, value, TenantSettingValueProvider.ProviderName, UserSettingManagerExtensions.GetKey(keyParam, tenantId), forceToSet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingManager"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="keyParam"></param>
        /// <param name="forceToSet"></param>
        /// <returns></returns>
        public static Task SetForCurrentTenantAsync(this ISettingManager settingManager, [NotNull] string name, [CanBeNull] string value, [NotNull] object keyParam, bool forceToSet = false)
        {
            return settingManager.SetAsync(name, value, TenantSettingValueProvider.ProviderName, UserSettingManagerExtensions.GetKey(keyParam), forceToSet);
        }
    }

    public static class GlobalSettingManagerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingManager"></param>
        /// <param name="name"></param>
        /// <param name="keyParam"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static Task<string> GetOrNullGlobalAsync(this ISettingManager settingManager, [NotNull] string name, [NotNull] object keyParam, bool fallback = true)
        {
            return settingManager.GetOrNullAsync(name, GlobalSettingValueProvider.ProviderName, UserSettingManagerExtensions.GetKey(keyParam), fallback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingManager"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="keyParam"></param>
        /// <returns></returns>
        public static Task SetGlobalAsync(this ISettingManager settingManager, [NotNull] string name, [CanBeNull] string value, [NotNull] object keyParam)
        {
            return settingManager.SetAsync(name, value, GlobalSettingValueProvider.ProviderName, UserSettingManagerExtensions.GetKey(keyParam));
        }
    }

    public static class DefaultValueSettingManagerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingManager"></param>
        /// <param name="name"></param>
        /// <param name="keyParam"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static Task<string> GetOrNullDefaultAsync(this ISettingManager settingManager, [NotNull] string name, [NotNull] object keyParam, bool fallback = true)
        {
            return settingManager.GetOrNullAsync(name, DefaultValueSettingValueProvider.ProviderName, UserSettingManagerExtensions.GetKey(keyParam), fallback);
        }
    }

    public static class ConfigurationValueSettingManagerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingManager"></param>
        /// <param name="name"></param>
        /// <param name="keyParam"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static Task<string> GetOrNullConfigurationAsync(this ISettingManager settingManager, [NotNull] string name, [NotNull] object keyParam, bool fallback = true)
        {
            return settingManager.GetOrNullAsync(name, ConfigurationSettingValueProvider.ProviderName, UserSettingManagerExtensions.GetKey(keyParam), fallback);
        }
    }
}

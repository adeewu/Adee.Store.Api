using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Authorization.Permissions
{
    /// <summary>
    /// 
    /// </summary>
    public static class LocalizationExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResource"></typeparam>
        /// <param name="permissionDefinition"></param>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="multiTenancySides"></param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        public static PermissionDefinition AddChild<TResource>(this PermissionDefinition permissionDefinition, string name, string displayName, MultiTenancySides multiTenancySides = MultiTenancySides.Both, bool isEnable = true)
        {
            if (string.IsNullOrWhiteSpace(displayName)) displayName = name;
            return permissionDefinition.AddChild(name, LocalizableString.Create<TResource>(displayName), multiTenancySides, isEnable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResource"></typeparam>
        /// <param name="permissionGroupDefinition"></param>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="multiTenancySide"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        public static PermissionDefinition AddPermission<TResource>(this PermissionGroupDefinition permissionGroupDefinition, string name, string displayName, MultiTenancySides multiTenancySide = MultiTenancySides.Both, bool isEnabled = true)
        {
            if (string.IsNullOrWhiteSpace(displayName)) displayName = name;
            return permissionGroupDefinition.AddPermission(name, LocalizableString.Create<TResource>(displayName), multiTenancySide, isEnabled);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResource"></typeparam>
        /// <param name="permissionDefinitionContext"></param>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="multiTenancySide"></param>
        /// <returns></returns>
        public static PermissionGroupDefinition AddGroup<TResource>(this IPermissionDefinitionContext permissionDefinitionContext, string name, string displayName = null, MultiTenancySides multiTenancySide = MultiTenancySides.Both)
        {
            if (string.IsNullOrWhiteSpace(displayName)) displayName = name;
            return permissionDefinitionContext.AddGroup(name, LocalizableString.Create<TResource>(displayName), multiTenancySide);
        }
    }
}

using Adee.Store.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Adee.Store.Permissions
{
    public class StorePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            StoreDefind(context);
            SystemManagementDefind(context);
            IdentityServerManagementDefind(context);
        }

        private void StoreDefind(IPermissionDefinitionContext context)
        {
            var storeGroup = context.AddGroup<StoreResource>(StorePermissions.GroupName, "收银管理");

            var productPermission = storeGroup.AddPermission<StoreResource>(StorePermissions.Products.Default, "商品列表");
            productPermission.AddChild<StoreResource>(StorePermissions.Products.Create, "新增商品");
            productPermission.AddChild<StoreResource>(StorePermissions.Products.Update, "编辑商品");
            productPermission.AddChild<StoreResource>(StorePermissions.Products.Delete, "删除商品");
            productPermission.AddChild<StoreResource>(StorePermissions.Products.Stock, "库存情况");
            productPermission.AddChild<StoreResource>(StorePermissions.Products.Sale, "上架情况");

            var catalogPermission = storeGroup.AddPermission<StoreResource>(StorePermissions.ProductCatalogs.Default, "商品分类");
            catalogPermission.AddChild<StoreResource>(StorePermissions.ProductCatalogs.Create, "新增商品分类");
            catalogPermission.AddChild<StoreResource>(StorePermissions.ProductCatalogs.Update, "编辑商品分类");
            catalogPermission.AddChild<StoreResource>(StorePermissions.ProductCatalogs.Delete, "删除商品分类");

            var stockPermission = storeGroup.AddPermission<StoreResource>(StorePermissions.ProductStocks.Default, "商品库存");
            stockPermission.AddChild<StoreResource>(StorePermissions.ProductStocks.Create, "新增商品库存");
            stockPermission.AddChild<StoreResource>(StorePermissions.ProductStocks.Update, "编辑商品库存");
            stockPermission.AddChild<StoreResource>(StorePermissions.ProductStocks.Delete, "删除商品库存");

            var stockOrderPermission = storeGroup.AddPermission<StoreResource>(StorePermissions.ProductStockOrders.Default, "商品入库");
            stockOrderPermission.AddChild<StoreResource>(StorePermissions.ProductStockOrders.Create, "新增入库单");

            var salePermission = storeGroup.AddPermission<StoreResource>(StorePermissions.ProductSales.Default, "商品销售");
            salePermission.AddChild<StoreResource>(StorePermissions.ProductSales.Create, "上架商品");
            salePermission.AddChild<StoreResource>(StorePermissions.ProductSales.Update, "编辑商品销售");
            salePermission.AddChild<StoreResource>(StorePermissions.ProductSales.OffSale, "下架商品");
        }

        private void SystemManagementDefind(IPermissionDefinitionContext context)
        {
            var abpIdentityGroup = context.AddGroup<StoreResource>(SystemManagement.Default, "系统管理");

            abpIdentityGroup.AddPermission<StoreResource>(SystemManagement.AuditLog, "审计日志");
            abpIdentityGroup.AddPermission<StoreResource>(SystemManagement.ES, "ES日志");
            abpIdentityGroup.AddPermission<StoreResource>(SystemManagement.Setting, "设置管理");
        }

        private void IdentityServerManagementDefind(IPermissionDefinitionContext context)
        {
            // multiTenancySide: MultiTenancySides.Host 只有host租户才有权限
            var identityServerManagementGroup = context.AddGroup<StoreResource>(IdentityServer.IdentityServerManagement, "IdentityServer", multiTenancySide: MultiTenancySides.Host);

            var clientManagment = identityServerManagementGroup.AddPermission<StoreResource>(IdentityServer.Client.Default, "客户端", multiTenancySide: MultiTenancySides.Host);
            clientManagment.AddChild<StoreResource>(IdentityServer.Client.Create, "新增", multiTenancySides: MultiTenancySides.Host);
            clientManagment.AddChild<StoreResource>(IdentityServer.Client.Update, "编辑", multiTenancySides: MultiTenancySides.Host);
            clientManagment.AddChild<StoreResource>(IdentityServer.Client.Delete, "删除", multiTenancySides: MultiTenancySides.Host);
            clientManagment.AddChild<StoreResource>(IdentityServer.Client.Enable, "启用|禁用", multiTenancySides: MultiTenancySides.Host);

            var apiResourceManagment = identityServerManagementGroup.AddPermission<StoreResource>(IdentityServer.ApiResource.Default, "Api资源", multiTenancySide: MultiTenancySides.Host);
            apiResourceManagment.AddChild<StoreResource>(IdentityServer.ApiResource.Create, "新增", multiTenancySides: MultiTenancySides.Host);
            apiResourceManagment.AddChild<StoreResource>(IdentityServer.ApiResource.Update, "编辑", multiTenancySides: MultiTenancySides.Host);
            apiResourceManagment.AddChild<StoreResource>(IdentityServer.ApiResource.Delete, "删除", multiTenancySides: MultiTenancySides.Host);

            var apiScopeManagment = identityServerManagementGroup.AddPermission<StoreResource>(IdentityServer.ApiScope.Default, "ApiScope", multiTenancySide: MultiTenancySides.Host);
            apiScopeManagment.AddChild<StoreResource>(IdentityServer.ApiScope.Create, "新增", multiTenancySides: MultiTenancySides.Host);
            apiScopeManagment.AddChild<StoreResource>(IdentityServer.ApiScope.Update, "编辑", multiTenancySides: MultiTenancySides.Host);
            apiScopeManagment.AddChild<StoreResource>(IdentityServer.ApiScope.Delete, "删除", multiTenancySides: MultiTenancySides.Host);

            var identityResourcesManagment = identityServerManagementGroup.AddPermission<StoreResource>(IdentityServer.IdentityResources.Default, "Identity资源", multiTenancySide: MultiTenancySides.Host);
            identityResourcesManagment.AddChild<StoreResource>(IdentityServer.IdentityResources.Create, "新增", multiTenancySides: MultiTenancySides.Host);
            identityResourcesManagment.AddChild<StoreResource>(IdentityServer.IdentityResources.Update, "编辑", multiTenancySides: MultiTenancySides.Host);
            identityResourcesManagment.AddChild<StoreResource>(IdentityServer.IdentityResources.Delete, "删除", multiTenancySides: MultiTenancySides.Host);
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<StoreResource>(name);
        }
    }
}

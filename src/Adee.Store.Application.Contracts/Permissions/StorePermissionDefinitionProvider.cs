using Adee.Store.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Adee.Store.Permissions
{
    public class StorePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
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

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<StoreResource>(name);
        }
    }
}

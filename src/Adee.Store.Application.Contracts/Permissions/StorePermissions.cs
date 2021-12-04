namespace Adee.Store.Permissions
{
    public static class StorePermissions
    {
        public const string GroupName = "Store";

        /// <summary>
        /// 商品权限
        /// </summary>
        public static class Products
        {
            public const string Default = GroupName + "." + nameof(Products);
            public const string Create = Default + "." + nameof(Create);
            public const string Update = Default + "." + nameof(Update);
            public const string Delete = Default + "." + nameof(Delete);
            public const string Stock = Default + "." + nameof(Stock);
            public const string Sale = Default + "." + nameof(Sale);
        }

        /// <summary>
        /// 商品分类权限
        /// </summary>
        public static class ProductCatalogs
        {
            public const string Default = GroupName + "." + nameof(ProductCatalogs);
            public const string Create = Default + "." + nameof(Create);
            public const string Update = Default + "." + nameof(Update);
            public const string Delete = Default + "." + nameof(Delete);
        }

        /// <summary>
        /// 商品库存权限
        /// </summary>
        public static class ProductStocks
        {
            public const string Default = GroupName + "." + nameof(ProductStocks);
            public const string Create = Default + "." + nameof(Create);
            public const string Update = Default + "." + nameof(Update);
            public const string Delete = Default + "." + nameof(Delete);
        }

        /// <summary>
        /// 商品库存订单权限
        /// </summary>
        public static class ProductStockOrders
        {
            public const string Default = GroupName + "." + nameof(ProductStockOrders);
            public const string Create = Default + "." + nameof(Create);
        }

        /// <summary>
        /// 商品销售权限
        /// </summary>
        public static class ProductSales
        {
            public const string Default = GroupName + "." + nameof(ProductSales);
            public const string Create = Default + "." + nameof(Create);
            public const string Update = Default + "." + nameof(Update);
            public const string OffSale = Default + "." + nameof(OffSale);
        }

        /// <summary>
        /// 支付权限
        /// </summary>
        public static class Pays
        {
            public const string Default = GroupName + "." + nameof(Pays);
        }
    }

    /// <summary>
    /// 认证服务权限
    /// </summary>
    public static class IdentityServer
    {
        public const string IdentityServerManagement = "IdentityServerManagement";

        public static class Client
        {
            public const string Default = IdentityServerManagement + "." + nameof(Client);
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Enable = Default + ".Enable";
        }


        public static class ApiResource
        {
            public const string Default = IdentityServerManagement + "." + nameof(ApiResource);
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static class ApiScope
        {
            public const string Default = IdentityServerManagement + "." + nameof(ApiScope);
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static class IdentityResources
        {
            public const string Default = IdentityServerManagement + "." + nameof(IdentityResources);
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
    }

    /// <summary>
    /// 系统管理扩展权限
    /// </summary>
    public static class SystemManagement
    {
        public const string Default = "System";
        public const string UserEnable = Default + ".Users.Enable";
        public const string AuditLog = Default + ".AuditLog";
        public const string ES = Default + ".ES";
        public const string Setting = Default + ".Setting";
    }
}
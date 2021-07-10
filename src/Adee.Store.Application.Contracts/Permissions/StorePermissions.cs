namespace Adee.Store.Permissions
{
    public static class StorePermissions
    {
        public const string GroupName = "Store";

        public static class Products
        {
            public const string Default = GroupName + "." + nameof(Products);
            public const string Create = Default + "." + nameof(Create);
            public const string Update = Default + "." + nameof(Update);
            public const string Delete = Default + "." + nameof(Delete);
            public const string Stock = Default + "." + nameof(Stock);
            public const string Sale = Default + "." + nameof(Sale);
        }

        public static class ProductCatalogs
        {
            public const string Default = GroupName + "." + nameof(ProductCatalogs);
            public const string Create = Default + "." + nameof(Create);
            public const string Update = Default + "." + nameof(Update);
            public const string Delete = Default + "." + nameof(Delete);
        }

        public static class ProductStocks
        {
            public const string Default = GroupName + "." + nameof(ProductStocks);
            public const string Create = Default + "." + nameof(Create);
            public const string Update = Default + "." + nameof(Update);
            public const string Delete = Default + "." + nameof(Delete);
        }

        public static class ProductStockOrders
        {
            public const string Default = GroupName + "." + nameof(ProductStockOrders);
            public const string Create = Default + "." + nameof(Create);
        }

        public static class ProductSales
        {
            public const string Default = GroupName + "."+ nameof(ProductSales);
            public const string Create = Default + "." + nameof(Create);
            public const string Update = Default + "." + nameof(Update);
            public const string OffSale = Default + "." + nameof(OffSale);
        }

        public static class Pays
        {
            public const string Default = GroupName + "." + nameof(Pays);
        }
    }
}
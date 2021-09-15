namespace Adee.Store.Settings
{
    public static class StoreSettings
    {
        private const string Prefix = "Store";

        /// <summary>
        /// 租户设置的支付参数版本，特定版本格式：版本|付款方式
        /// </summary>
        public const string PayParameterVersionKey = Prefix + ".Pay.PayParameter.Version";
    }
}
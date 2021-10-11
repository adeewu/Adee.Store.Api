using System.ComponentModel;

namespace Adee.Store.Products
{
    /// <summary>
    /// 业务类型
    /// </summary>
    public enum TerminalType
    {
        /// <summary>
        /// IOSApp
        /// </summary>
        [Description("IOSApp")]
        IOSApp = 1,

        /// <summary>
        /// AndroidApp
        /// </summary>
        [Description("AndroidApp")]
        AndroidApp = 2,

        /// <summary>
        /// WebApp
        /// </summary>
        [Description("WebApp")]
        WebApp = 3,

        /// <summary>
        /// H5App
        /// </summary>
        [Description("H5App")]
        H5App = 4,
    }
}

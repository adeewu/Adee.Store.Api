namespace Adee.Store
{
    /// <summary>
    /// 认证服务
    /// </summary>
    public class AuthServerOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RequireHttpsMetadata { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SwaggerClientId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SwaggerClientSecret { get; set; }
    }
}
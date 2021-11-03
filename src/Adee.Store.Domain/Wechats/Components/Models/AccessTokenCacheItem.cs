namespace Adee.Store.Wechats.Components.Models
{
    public class AccessTokenCacheItem
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// 刷新令牌，授权的小程序、公众号才会有，第三方平台没有
        /// </summary>
        public string RefreshToken { get; set; }
    }
}

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
    }
}

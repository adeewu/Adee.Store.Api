namespace Adee.Store.Wechats.Components.Jobs.UpdateAccessToken
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateAccessTokenArgs
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public long UpdateTime { get; set; }

        /// <summary>
        /// 上次等待时间
        /// </summary>
        public int LastDelay { get; set; } = 7000;
    }
}

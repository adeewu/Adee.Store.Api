using System.ComponentModel;

namespace Adee.Store.Wechats.Components.Models
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public class NotifyMessageType
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        public const string Text = "text";

        /// <summary>
        /// 图片消息
        /// </summary>
        public const string Image = "image";

        /// <summary>
        /// 语音消息
        /// </summary>
        public const string Voice = "voice";

        /// <summary>
        /// 视频消息
        /// </summary>
        public const string Video = "video";

        /// <summary>
        /// 小视频
        /// </summary>
        public const string ShortVideo = "shortvideo";

        /// <summary>
        /// 地理位置消息
        /// </summary>
        public const string Location = "location";

        /// <summary>
        /// 链接消息
        /// </summary>
        public const string Link = "link";
    }
}

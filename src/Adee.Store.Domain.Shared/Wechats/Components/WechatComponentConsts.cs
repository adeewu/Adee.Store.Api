using System.Collections.Generic;

namespace Adee.Store.Wechats.Components.Models
{
    public static class WechatComponentConsts
    {
        /// <summary>
        /// 提前更新令牌时间
        /// </summary>
        public static int ForwardUpdateAccessToken = 1 * 60;

        /// <summary>
        /// 全网发布测试账号
        /// </summary>
        public static Dictionary<string, string> ValidPublicAccount = new Dictionary<string, string>
        {
            { "wx570bc396a51b8ff8", "gh_3c884a361561" },
            { "wx9252c5e0bb1836fc", "gh_c0f28a78b318" },
            { "wx8e1097c5bc82cde9", "gh_3f222ed8d140" },
            { "wx14550af28c71a144", "gh_26128078e9ab" },
            { "wxa35b9c23cfe664eb", "gh_2b3713f184a6" },
            { "wxd101a85aa106f53e", "gh_8dad206e9538" },
            { "wxc39235c15087f6f3", "gh_905ae9d01059" },
            { "wx7720d01d4b2a4500", "gh_393666f1fdf4" },
            { "wx05d483572dcd5d8b", "gh_39abb5d4e1b7" },
            { "wx5910277cae6fd970", "gh_7818dcb60240" },
        };
    }
}

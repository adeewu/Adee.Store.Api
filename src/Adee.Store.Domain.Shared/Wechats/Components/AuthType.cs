using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adee.Store.Wechats.Components.Models
{
    public enum AuthType
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("公众号")]
        MP = 1,

        /// <summary>
        /// 
        /// </summary>
        [Description("小程序")]
        MiniProgram = 2,

        /// <summary>
        /// 
        /// </summary>
        [Description("所有类型")]
        ALL = 3
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// String扩展
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatString(this string str, params object[] args)
        {
            return string.Format(str, args);
        }
    }
}

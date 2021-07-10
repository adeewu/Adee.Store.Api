using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// IEnumerable扩展
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>
        /// 循环遍历
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <param name="lists"></param>
        /// <param name="func">循环回调函数</param>
        public static void ForEach<T>(this IEnumerable<T> lists, Action<T> func)
        {
            if (func == null) return;

            foreach (var item in lists)
            {
                func(item);
            }
        }        
    }
}

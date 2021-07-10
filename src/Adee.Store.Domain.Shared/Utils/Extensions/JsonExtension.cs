using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// json扩展
    /// </summary>
    public static class JsonExtension
    {
        /// <summary>
        /// 序列化设置
        /// </summary>
        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatString = "yyyy-MM-dd HH:mm:ss",
        };

        /// <summary>
        /// 对象序列化为Json字符
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string ToJsonString(this object obj, JsonSerializerSettings settings = null)
        {
            if (obj == null) return string.Empty;

            return JsonConvert.SerializeObject(obj, settings ?? JsonSerializerSettings);
        }

        /// <summary>
        /// 将obj对象通过json反序列化转换为目标对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">obj对象</param>
        /// <param name="settings">序列化设定</param>
        /// <returns></returns>
        public static T AsObject<T>(this object obj, JsonSerializerSettings settings = null) where T : class
        {
            if (obj == null) return default(T);

            var json = string.Empty;
            if (obj is string)
            {
                json = (string)obj;
            }
            else
            {
                json = obj.ToJsonString();
            }

            return JsonConvert.DeserializeObject<T>(json, settings ?? JsonSerializerSettings);
        }

        /// <summary>
        /// Json字符反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="anonymousTypeObject">目标类型数据模型</param>
        /// <param name="settings">序列化设定</param>
        /// <returns></returns>
        public static T AsAnonymousObject<T>(this string obj, T anonymousTypeObject, JsonSerializerSettings settings = null) where T : class
        {
            if (string.IsNullOrWhiteSpace(obj)) obj = "{}";
            return JsonConvert.DeserializeAnonymousType(obj, anonymousTypeObject, settings ?? JsonSerializerSettings);
        }
    }
}

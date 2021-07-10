using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Object扩展
    /// </summary>
    public static class ObjectExtension
    {
        public static T To<T>(this object obj, T defaultValue) where T : struct
        {
            try
            {
                T value = obj.To<T>();
                return value;
            }
            catch { }

            return defaultValue;
        }

        public static int To(this decimal obj, MidpointRounding mode)
        {
            return Math.Round(obj, 0, mode).To<int>();
        }

        public static int To(this double obj, MidpointRounding mode)
        {
            return Math.Round(obj, 0, mode).To<int>();
        }

        public static bool IsNull(this object obj)
        {
            return !obj.IsNotNull();
        }

        public static bool IsNotNull(this object obj)
        {
            if (obj == null) return false;

            var objType = obj.GetType();

            if (objType.IsArray
                || objType.Name == typeof(List<>).Name
                || objType.Name == typeof(IEnumerable<>).Name
                || objType.Name == typeof(Dictionary<,>).Name)
            {
                var anyMehtod = typeof(Enumerable).GetMethods().Where(p => p.Name == "Any").Where(p => p.GetParameters().Length == 1).FirstOrDefault();
                if (anyMehtod == null) return false;

                return (bool)anyMehtod.MakeGenericMethod(objType.GetGenericArguments()).Invoke(null, new object[] { obj });                 
            }

            return true;
        }
    }
}

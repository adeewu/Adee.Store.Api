using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public class AssertHelper
    {
        public static void IsNull(object obj, string name = "", string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new Exception($"${nameof(name)}、{nameof(message)}必须提供一项");
                }

                message = $"{name}需要为空";
            }
            if (obj != null)
            {
                var objType = obj.GetType();
                if (objType.IsArray())
                {
                    var anyMethod = typeof(Enumerable).GetMethod("Any");
                    anyMethod.MakeGenericMethod(objType.GetGenericArguments());

                    var anyResult = (bool)anyMethod.Invoke(null, new object[] { obj });
                    if (!anyResult) return;
                }

                throw new Exception(message);
            }
        }

        public static void IsNotNull(object obj, string name = "", string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new Exception($"${nameof(name)}、{nameof(message)}必须提供一项");
                }

                message = $"{name}不能为空";
            }
            if (obj == null) throw new Exception(message);

            var objType = obj.GetType();

            if (objType == typeof(string) && string.IsNullOrWhiteSpace(obj.ToString()))
            {
                throw new Exception(message);
            }

            if (objType.IsArray())
            {
                var anyMethod = typeof(Enumerable).GetMethod("Any");
                anyMethod.MakeGenericMethod(objType.GetGenericArguments());

                var anyResult = (bool) anyMethod.Invoke(null, new object[] { obj });
                if(!anyResult) throw new Exception(message);
            }
        }

        public static void IsTrue(bool condition, string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = $"条件为真";
            }
            if (condition != true) throw new Exception(message);
        }

        public static void IsFalse(bool condition, string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = $"条件为假";
            }
            if (condition != false) throw new Exception(message);
        }

        public static void AreEqual<T>(T t1, T t2, string t1Name = "", string t2Name = "", string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                if (string.IsNullOrWhiteSpace(t1Name) && string.IsNullOrWhiteSpace(t2Name))
                {
                    throw new Exception($"name、{nameof(message)}必须提供一项");
                }

                if (string.IsNullOrWhiteSpace(t1Name))
                {
                    throw new Exception($"请提供{nameof(t1Name)}参数");
                }

                if (string.IsNullOrWhiteSpace(t2Name))
                {
                    throw new Exception($"请提供{nameof(t2Name)}参数");
                }

                message = $"{t1Name}和{t2Name}的值需要相等";
            }
            if (!t1.Equals(t2)) throw new Exception(message);
        }

        public static void AreNotEqual<T>(T t1, T t2, string t1Name = "", string t2Name = "", string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                if (string.IsNullOrWhiteSpace(t1Name) && string.IsNullOrWhiteSpace(t2Name))
                {
                    throw new Exception($"name、{nameof(message)}必须提供一项");
                }

                if (string.IsNullOrWhiteSpace(t1Name))
                {
                    throw new Exception($"请提供{nameof(t1Name)}参数");
                }

                if (string.IsNullOrWhiteSpace(t2Name))
                {
                    throw new Exception($"请提供{nameof(t2Name)}参数");
                }

                message = $"{t1Name}和{t2Name}的值不能相等";
            }
            if (t1.Equals(t2)) throw new Exception(message);
        }
    }
}


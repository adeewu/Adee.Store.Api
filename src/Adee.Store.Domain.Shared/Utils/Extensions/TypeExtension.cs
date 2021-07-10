using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public static class TypeExtension
    {
        public static bool IsArray(this Type type)
        {
            return type.GetInterfaces().Where(p => p == typeof(ICollection)).Any();
        }
    }
}
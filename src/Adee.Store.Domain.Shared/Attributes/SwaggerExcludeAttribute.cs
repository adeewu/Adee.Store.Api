using System;
using System.Collections.Generic;
using System.Text;

namespace Adee.Store.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SwaggerExcludeAttribute : Attribute
    {
    }
}

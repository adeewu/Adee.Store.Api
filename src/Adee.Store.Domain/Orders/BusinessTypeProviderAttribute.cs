using Adee.Store.Pays;
using System;

namespace Adee.Store.Domain.Pays
{
    public class BusinessTypeProviderAttribute : Attribute
    {
        public BusinessType BusinessType { get; set; }

        public BusinessTypeProviderAttribute(BusinessType businessType)
        {
            BusinessType = businessType;
        }
    }
}
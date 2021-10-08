using System;
using Adee.Store.Pays;

namespace Adee.Store.Pays
{
    public class PayProviderAttribute : Attribute
    {
        public PayOrganizationType PayOrganizationType { get; set; }

        public PayProviderAttribute(PayOrganizationType payOrganizationType)
        {
            PayOrganizationType = payOrganizationType;
        }
    }
}
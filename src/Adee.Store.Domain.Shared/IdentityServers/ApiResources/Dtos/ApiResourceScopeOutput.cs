using System;

namespace Adee.Store.IdentityServers.Dtos
{
    public class ApiResourceScopeOutput
    {
        public Guid ApiResourceId { get; set; }

        public string Scope { get; set; }
    }
}
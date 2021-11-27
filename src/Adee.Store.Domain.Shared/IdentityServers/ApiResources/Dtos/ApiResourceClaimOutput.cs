using System;

namespace Adee.Store.IdentityServers.Dtos
{
    public class ApiResourceClaimOutput
    {
        public Guid ApiResourceId { get; set; }

        public string Type { get; set; }
    }
}
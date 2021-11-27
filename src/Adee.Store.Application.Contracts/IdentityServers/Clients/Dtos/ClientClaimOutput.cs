using System;

namespace Adee.Store.IdentityServers.Clients
{
    public class ClientClaimOutput
    {
        public Guid ClientId { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }
}
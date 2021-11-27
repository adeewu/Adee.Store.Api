using System;

namespace Adee.Store.IdentityServers.Clients
{
    public class ClientGrantTypeOutput
    {
        public Guid ClientId { get; set; }

        public string GrantType { get; set; }
    }
}
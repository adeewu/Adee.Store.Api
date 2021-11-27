using System;

namespace Adee.Store.IdentityServers.Clients
{
    public class ClientScopeOutput
    {
        public Guid ClientId { get; set; }

        public string Scope { get; set; }
    }
}
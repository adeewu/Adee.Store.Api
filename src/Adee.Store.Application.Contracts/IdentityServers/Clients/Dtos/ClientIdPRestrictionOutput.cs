using System;

namespace Adee.Store.IdentityServers.Clients
{
    public class ClientIdPRestrictionOutput
    {
        public Guid ClientId { get; set; }

        public string Provider { get; set; }
    }
}
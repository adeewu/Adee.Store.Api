using System;
namespace Adee.Store.IdentityServers.Clients
{
    public class ClientRedirectUriOutput
    {
        public virtual Guid ClientId { get; set; }

        public virtual string RedirectUri { get; set; }
    }
}
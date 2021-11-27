using System.ComponentModel.DataAnnotations;

namespace Adee.Store.IdentityServers.Clients
{
    public class AddRedirectUriInput
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string Uri { get; set; }
    }
}
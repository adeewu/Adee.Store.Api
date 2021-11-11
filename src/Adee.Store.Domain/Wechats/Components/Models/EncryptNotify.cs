namespace Adee.Store.Wechats.Components.Models
{
    public class EncryptNotify
    {
        public string signature { get; set; }

        public string timestamp { get; set; }

        public string nonce { get; set; }

        public string encrypt_type { get; set; }

        public string msg_signature { get; set; }
    }
}

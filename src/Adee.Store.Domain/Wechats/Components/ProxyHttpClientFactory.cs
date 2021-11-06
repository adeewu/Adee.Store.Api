using Flurl.Http.Configuration;
using System.Net;
using System.Net.Http;

namespace Adee.Store.Wechats.Components
{
    public class ProxyHttpClientFactory : DefaultHttpClientFactory
    {
        private readonly string _address;
        private readonly int _port;

        public ProxyHttpClientFactory(string address, int port)
        {
            _address = address;
            _port = port;
        }

        public override HttpMessageHandler CreateMessageHandler()
        {
            return new HttpClientHandler
            {
                Proxy = new WebProxy(_address, _port),
                UseProxy = true
            };
        }
    }
}

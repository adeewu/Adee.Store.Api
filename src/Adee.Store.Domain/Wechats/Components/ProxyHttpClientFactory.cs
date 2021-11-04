using Flurl.Http.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

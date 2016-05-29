using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using OwinServer.Auth;

namespace OwinServer.Test
{
    internal class HttpClientProvider : IHttpClientProvider
    {
        private readonly ICertificateRepository _certificationProvider;

        public HttpClientProvider(ICertificateRepository certificationProvider)
        {
            _certificationProvider = certificationProvider;
        }

        public bool SkipSslValidationCheck { get; set; }    

        public async Task<HttpClient> GetHttpClientAsync(string url)
        {
            var requestHandler = new WebRequestHandler();

            if (_certificationProvider != null)
            {
                var certificates = await _certificationProvider.GetAllCertificatesAsync();

                if (certificates != null)
                {
                    foreach (var certificate in certificates)
                    {
                        requestHandler.ClientCertificates.Add(certificate);
                    }
                }
            }

            if (!String.IsNullOrWhiteSpace(WebProxyUrl))
            {
                requestHandler.Proxy = new WebProxy(WebProxyUrl);
            }

            if(SkipSslValidationCheck)
            {
                requestHandler.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            }

            return new HttpClient(requestHandler) { BaseAddress = new Uri(url)};
        }

        public string WebProxyUrl { get; set; }
    }
}
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OwinServer.Test
{
    public class AppAuthorizedClient : IDisposable
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private Uri _baseUri;

        public AppAuthorizedClient(Uri baseUri, IHttpClientProvider httpClientProvider)
        {
            _baseUri = baseUri;
            _httpClientProvider = httpClientProvider;
        }

        public void Dispose()
        {
        }

        public async Task<HttpResponseMessage> GetAsync(string resource)
        {
            var client = await _httpClientProvider.GetHttpClientAsync(_baseUri.ToString());
            return await client.GetAsync(resource);
        }
    }
}
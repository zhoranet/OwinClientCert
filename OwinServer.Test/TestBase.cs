using System;
using OwinServer.Auth;
using OwinServer.Test.Properties;

namespace OwinServer.Test
{
    public abstract class TestBase
    {
        protected TestBase()
        {
            ServerBaseUrl = Settings.Default.ServerBaseUrl;
            Thumbprints = Settings.Default.Thumbprints;

            var cachedCertificateRepository = new CachedCertificateRepository(Thumbprints?.Split(new[] {','},
                StringSplitOptions.RemoveEmptyEntries));
            HttpClientProvider = new HttpClientProvider(cachedCertificateRepository)
            {
                WebProxyUrl = Settings.Default.WebProxyUrl,
                SkipSslValidationCheck = true
            };
        }

        public string ServerBaseUrl { get; set; }

        public string Thumbprints { get; set; }

        public IHttpClientProvider HttpClientProvider { get; set; }

        public AppAuthorizedClient CreateTestClient()
        {
            return new AppAuthorizedClient(new Uri(ServerBaseUrl), HttpClientProvider);
        }
    }
}
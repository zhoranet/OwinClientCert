using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace OwinServer.Auth
{
    public class CachedCertificateRepository : ICertificateRepository
    {
        private readonly ConcurrentDictionary<string, X509Certificate2> _certificates;

        public CachedCertificateRepository(IEnumerable<string> thumbprints)
        {
            var thumbprintList = thumbprints.ToList();
            _certificates = new ConcurrentDictionary<string, X509Certificate2>(
                thumbprintList.Select(x => new KeyValuePair<string, X509Certificate2>(x, null)));
        }

        public async Task<IEnumerable<X509Certificate2>> GetAllCertificatesAsync()
        {
            await LoadAllAsync();
            return _certificates.Select(x => x.Value);
        }

        public async Task<X509Certificate2> GetCertificate(string thumbrint)
        {
            await LoadAllAsync();
            X509Certificate2 certificate;
            _certificates.TryGetValue(thumbrint, out certificate);
            return certificate;
        }

        private Task LoadAllAsync()
        {
            var notLoadedKeys = _certificates.Where(x => x.Value == null).Select(x => x.Key);

            X509Certificate2 certificate = null;
            foreach (var key in notLoadedKeys)
            {
                if (TryFindCertificateAsync(key, out certificate))
                {
                    _certificates.AddOrUpdate(key, certificate, (s, cert) => certificate);
                }
            }

            return Task.FromResult(1);
        }

        internal static bool TryFindCertificateAsync(string thumbprint, out X509Certificate2 certificate)
        {
            return TryFindCertificateAsync(thumbprint, StoreLocation.LocalMachine, out certificate) || 
                TryFindCertificateAsync(thumbprint, StoreLocation.CurrentUser, out certificate);
        }

        internal static bool TryFindCertificateAsync(string thumbprint, StoreLocation storeLocation,
            out X509Certificate2 certificate)
        {
            certificate = FindCertificateAsync(thumbprint, storeLocation).Result;
            return certificate != null;
        }

        internal static async Task<X509Certificate2> FindCertificateAsync(string thumbprint, StoreLocation storeLocation)
        {
            X509Store store = null;
            X509Certificate2 cert = null;
            try
            {
                store = new X509Store(StoreName.My, storeLocation);
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly );
                var result = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                if (result.Count > 0)
                {
                    cert = result[0];
                }
            }
            finally
            {
                store?.Close();
            }

            return await Task.FromResult(cert);
        }
    }
}
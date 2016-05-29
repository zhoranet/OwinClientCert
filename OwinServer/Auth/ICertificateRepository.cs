using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace OwinServer.Auth
{
    public interface ICertificateRepository
    {
        Task<IEnumerable<X509Certificate2>> GetAllCertificatesAsync();
    }
}
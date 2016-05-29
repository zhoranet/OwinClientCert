using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace OwinServer.Auth
{
    public interface IClientCertificateValidator
    {
        Task<bool> ValidateAsync(X509Certificate2 userCertificate, bool skipVerification);
    }
}
using System.Security.Cryptography.X509Certificates;

namespace OwinServer
{
    public interface IClientCertificateValidator
    {
        ClientCertificateValidationResult Validate(X509Certificate2 userCertificate);
    }
}
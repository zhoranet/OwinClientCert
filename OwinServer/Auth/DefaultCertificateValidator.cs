using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace OwinServer.Auth
{
    public class DefaultCertificateValidator : IClientCertificateValidator
    {
        private readonly ICertificateRepository _allowedCerts;

        
        public DefaultCertificateValidator(ICertificateRepository allowedCerts = null)
        {
            _allowedCerts = allowedCerts;
        }

        public async Task<bool> ValidateAsync(X509Certificate2 userCertificate, bool skipVerification = false)
        {
            var isValid = false;
            var exceptions = new List<string>();

            try
            {
                if (skipVerification || userCertificate.Verify())
                {
                    if (_allowedCerts == null)
                    {
                        isValid = true;
                    }
                    else
                    {
                        foreach (var allowedCertificate in await _allowedCerts.GetAllCertificatesAsync())
                        {
                            isValid |= userCertificate.Equals(allowedCertificate);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                exceptions.Add(ex.Message);
            }

            return isValid;
        }
    }
}
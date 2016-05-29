using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace OwinServer
{
    public class DefaultClientCertificateValidator : IClientCertificateValidator
    {
        private readonly IEnumerable<X509Certificate2> _allowedCerts;

        
        public DefaultClientCertificateValidator(IEnumerable<X509Certificate2> allowedCerts = null)
        {
            _allowedCerts = allowedCerts;
        }

        public ClientCertificateValidationResult Validate(X509Certificate2 userCertificate)
        {
            var isValid = false;
            var exceptions = new List<string>();

            try
            {
                var chain = new X509Chain
                {
                    ChainPolicy = new X509ChainPolicy()
                    {
                        RevocationMode = X509RevocationMode.Offline,
                        RevocationFlag = X509RevocationFlag.EntireChain
                    }
                };

                if (chain.Build(userCertificate))
                {
                    if (_allowedCerts == null)
                    {
                        isValid = true;
                    }
                    else
                    {
                        foreach (var allowedCertificate in _allowedCerts)
                        {
                            isValid |= MatchCertificate(userCertificate, allowedCertificate);
                        }
                    }
                }
                else
                {
                    foreach (var chainElement in chain.ChainElements)
                    {
                        exceptions.AddRange(chainElement.ChainElementStatus.Select(s => s.StatusInformation));
                    }
                }
            }
            catch (Exception ex)
            {
                exceptions.Add(ex.Message);
            }

            var res = new ClientCertificateValidationResult(isValid);

            if (exceptions.Any())
            {
                res.AddValidationExceptions(exceptions);
            }

            return res;
        }

        private bool MatchCertificate(X509Certificate2 userCertificate, X509Certificate2 allowedCErtificate)
        {
            return userCertificate != null && userCertificate.Equals(allowedCErtificate)
                   && userCertificate.GetPublicKeyString() == allowedCErtificate.GetPublicKeyString()
                   && userCertificate.Thumbprint == allowedCErtificate.Thumbprint;
        }
    }
}
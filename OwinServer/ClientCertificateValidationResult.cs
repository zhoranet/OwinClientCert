using System.Collections.Generic;
using System.Linq;

namespace OwinServer
{
    public class ClientCertificateValidationResult
    {
        private readonly IList<string> _validationExceptions;

        public ClientCertificateValidationResult(bool certificateValid)
        {
            CertificateValid = certificateValid;
            _validationExceptions = new List<string>();
        }

        public void AddValidationExceptions(IEnumerable<string> validationExceptions)
        {
            if (validationExceptions != null) _validationExceptions.Concat(validationExceptions);
        }

        public void AddValidationException(string validationException)
        {
            if (validationException != null) _validationExceptions.Add(validationException);
        }

        public IEnumerable<string> ValidationExceptions => _validationExceptions;

        public bool CertificateValid { get; }
    }
}
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace OwinServer
{
    public class CertificateAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private static readonly string[] Thumbprints =
        {
            //"02AD149E2A51239B3EC8465CAA07669A919DB6F1",
            "1F0CEC52CFD95F273D02D6A3DB2065E203263A4D"
        };

        public IClientCertificateValidator Validator { get; set; }

        public CertificateAuthorizeAttribute()
        {
            Validator = new DefaultClientCertificateValidator(CertificateHelper.FindCertificates(Thumbprints));
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var clientCertificate = actionContext.Request?.GetClientCertificate();

            if (clientCertificate != null)
            {
                var result = Validator.Validate(clientCertificate);

                if (result.CertificateValid)
                {
                    actionContext.RequestContext.Principal = new GenericPrincipal(
                        new GenericIdentity("Certificate"), new[] {"CertificateUser"});
                }
            }

            base.OnAuthorization(actionContext);
        }
    }
}
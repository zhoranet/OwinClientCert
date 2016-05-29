using System;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace OwinServer.Auth
{
    public class CertificateAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private readonly Type _certificateValidatorType;
        private readonly bool _skipVerification;

        public CertificateAuthorizeAttribute(Type certificateValidatorType, bool skipVerification)
        {
            _certificateValidatorType = certificateValidatorType;
            _skipVerification = skipVerification;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var validator =
                (IClientCertificateValidator)
                    actionContext.Request?.GetDependencyScope()?.GetService(_certificateValidatorType);

            var clientCertificate = actionContext.Request?.GetClientCertificate();

            if (clientCertificate != null)
            {
                if (validator != null && validator.ValidateAsync(clientCertificate, _skipVerification).Result)
                {
                    actionContext.RequestContext.Principal = new GenericPrincipal(
                        new GenericIdentity("Certificate"), new[] { "CertificateUser" });
                }
            }

            base.OnAuthorization(actionContext);
        }
    }
}
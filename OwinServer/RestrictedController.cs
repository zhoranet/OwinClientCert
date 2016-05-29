using System.Web.Http;

namespace OwinServer
{
    [CertificateAuthorize]
    public class RestrictedController : ApiController
    {
        [HttpGet]
        //[Authorize(Roles = "CertificateUser", Users = null)]
        [Authorize]
        public string Get()
        {
            return "OK";
        }
    }
}
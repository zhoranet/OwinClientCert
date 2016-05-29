using System.Web.Http;
using OwinServer.Auth;

namespace OwinServer.Controllers
{
    [CertificateAuthorize(typeof (DefaultCertificateValidator), true)]
    public class RestrictedController : ApiController
    {
        [HttpGet]
        [Authorize]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}
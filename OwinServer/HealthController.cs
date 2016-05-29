using System.Web.Http;

namespace OwinServer
{
    public class HealthController : ApiController
    {
        [Authorize]
        [HttpGet]
        public string Get()
        {
            return "OK";
        }
    }
}
using System.Web.Http;

namespace OwinServer.Controllers
{
    public class PublicController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}
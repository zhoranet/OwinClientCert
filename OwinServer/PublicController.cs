using System.Web.Http;

namespace OwinServer
{
    public class PublicController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            return "OK";
        }
    }
}
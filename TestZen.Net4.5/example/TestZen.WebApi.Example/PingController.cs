using System.Web.Http;

namespace TestZen.WebApi.Example
{
    public class PingController : ApiController 
    {
        [Route("ping")]
        [HttpGet]
        public IHttpActionResult Ping()
        {
            return Ok();
        }

    }
}

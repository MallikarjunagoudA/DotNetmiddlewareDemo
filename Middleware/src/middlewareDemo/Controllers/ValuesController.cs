using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace middlewareDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IHttpContextAccessor _httpContextAccessor;

        public ValuesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor =  httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Get()
        
        {
            throw new Exception("test exception");
        }

        //this to pass the co-realtionid to next api call or lower environments. 
        [HttpGet]
        public IActionResult GetCorelationid()
        {
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("x-Correlationid-Id", out StringValues correlationidHeaders);
            string correlationidHeader = correlationidHeaders.ElementAt(0);
            throw new Exception("test exception");
        }
    }
}

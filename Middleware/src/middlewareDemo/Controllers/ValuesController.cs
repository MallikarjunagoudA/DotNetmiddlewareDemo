using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace middlewareDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        public ValuesController()
        {

        }

        [HttpGet]
        public IActionResult Get()
        {
            throw new Exception("test exception");
        }

    }
}

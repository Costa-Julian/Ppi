using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdenController : ControllerBase
    {
        public readonly 
        [HttpGet("ping")]
        public IActionResult Ping() => Ok(new { ok = true });

    }
}

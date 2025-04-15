using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DataWarehouseController : ControllerBase
    {
        /// <summary>
        /// Simple protected endpoint to verify authentication.
        /// </summary>
        [Authorize]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Method executed correctly");
        }
    }
}

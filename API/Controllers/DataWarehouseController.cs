using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DataWarehouseController : ControllerBase
    {

        private static readonly object _lock = new();
        private static bool _isRunning = false;
        /// <summary>
        /// Simple protected endpoint to verify authentication.
        /// </summary>
        [Authorize]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Method executed correctly");
        }


        [Authorize]
        [HttpGet("ReadDataFromUniconta")]
        public IActionResult ReadDataFromUniconta()
        {
            lock (_lock)
            {
                if (_isRunning)
                {
                    return Conflict("Process is already running. Please wait for it to finish before trying again. The data retriever method can only run in a single instance to avoid data corruption in the database");
                }

                _isRunning = true;
            }

            try
            {
                // Run data retrieving here
                Thread.Sleep(3000);

                return Ok("Method executed correctly. All data is up to date.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
            finally
            {
                lock (_lock)
                {
                    _isRunning = false;
                }
            }
        }
    }
}

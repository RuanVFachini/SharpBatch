using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueueController : ControllerBase
    {

        private readonly ILogger<QueueController> _logger;

        public QueueController(ILogger<QueueController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Enqueue([FromQuery] int taskCount, [FromQuery] int seconds)
        {
            return Ok();
        }
    }

}

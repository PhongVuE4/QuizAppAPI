using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quiz_Common;

namespace QuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetHealthStatus()
        {
            return Ok(new
            {
                status = "Healthy",
                timestamp = TimeHelper.GetVietnamCurrentTime()
            });
        }
    }
}

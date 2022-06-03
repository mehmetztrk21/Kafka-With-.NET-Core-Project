using Microsoft.AspNetCore.Mvc;
using Notifier.Api.Models.Notification;

namespace Notifier.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ILogger<NotificationController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        [Route("[action]")]
        public PushOneResponse PushOne(PushOneRequest request)
        {
            var response = new PushOneResponse();

            return response;
        }
    }
}
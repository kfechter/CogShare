using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CogShare.Controllers
{
    public class RequestController : Controller
    {
        private readonly ILogger<RequestController> _logger;

        public RequestController(ILogger<RequestController> logger)
        {
            _logger = logger;
        }

        public IActionResult Requests()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Api.Controllers
{
    public class BaseController : ControllerBase
    {
        private ILogger _logger;
        protected ILogger Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger>();
    }
}

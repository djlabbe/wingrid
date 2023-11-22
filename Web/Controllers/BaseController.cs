using Microsoft.AspNetCore.Mvc;
using Wingrid.Web.Models.Exceptions;

namespace Wingrid.Web.Controllers
{
    [ApiController]
    public abstract class BaseController<T> : ControllerBase where T : BaseController<T>
    {
        protected ILogger<T>? _logger;
        protected ILogger<T>? Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>();

        protected async Task<IActionResult> ExecuteActionAsync(Func<Task<IActionResult>> func)
        {
            try
            {
                try
                {
                    return await func();
                } 
                catch (Exception e)
                {
                    Logger?.LogError(e, "Error Executing");
                    throw;
                }
            } catch (NotFoundException e) {
                return NotFound(e.Message);
            }
        }
    }
}
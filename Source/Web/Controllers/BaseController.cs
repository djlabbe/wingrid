using Microsoft.AspNetCore.Mvc;
using Wingrid.Models.Dto;

namespace Wingrid.Controllers;

[ApiController]
public abstract class BaseController<T> : ControllerBase where T : BaseController<T>
{
    protected ILogger<T>? _logger;
    protected ILogger<T>? Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>();

    protected async Task<ResponseDto> ExecuteActionAsync(Func<Task<object>> func)
    {
        try
        {
            return new ResponseDto()
            {
                Result = await func()
            };
        }
        catch (Exception e)
        {
            Logger?.LogError(e, "Error Executing");
            return new ResponseDto()
            {
                IsSuccess = false,
                Message = e.Message,
            };
        }
    }
}

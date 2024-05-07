using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Broxel.Logger.Filters
{
    public class FiltersLogger : IAsyncActionFilter, IExceptionFilter
    {
        private readonly ILogger<FiltersLogger> _logger;
        //private readonly IHttpClient _httpClient;
        
        public FiltersLogger(ILogger<FiltersLogger> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("FilterLogger LogInformation antes del método");            

            await next();

            _logger.LogInformation("FilterLogger LogInformation después del método");
        }
        

        public class Error
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogInformation("FilterLogger LogTrace exepción");
            _logger.LogWarning("FilterLogger LogWarning excepción");
            _logger.LogError("FilterLogger LogError excepción");
            

            var error = new Error
            {
                StatusCode = 500,
                Message = context.Exception.Message
            };
            context.Result = new JsonResult(error) { StatusCode = 500 };
        }
    }
}

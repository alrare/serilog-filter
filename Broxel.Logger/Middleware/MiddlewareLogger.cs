using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.WebEncoders.Testing;
using Serilog.Context;
using System.Linq;
using System.Text.RegularExpressions;

namespace Broxel.Logger.Middleware
{
    public class MiddlewareLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddlewareLogger> _logger;

        public MiddlewareLogger(RequestDelegate next, ILogger<MiddlewareLogger> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                context.Request.Headers.TryGetValue("CorrelationHeader", out StringValues correlationids);
                  var CorrelationId = correlationids.FirstOrDefault() ?? Guid.NewGuid().ToString();

                using (LogContext.PushProperty("CorrelationId", correlationids))
                {
                    await _next(context);
                }  
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}

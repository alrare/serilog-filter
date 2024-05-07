using Broxel.Logger.Filters;
using Broxel.Logger.Services;
using Microsoft.AspNetCore.Mvc;

namespace Broxel.Logger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionController : ControllerBase
    {

        private readonly ILogger<ExceptionController> _logger;
        public ExceptionController(ILogger<ExceptionController> logger)
        {
            _logger = logger;
        }

        [TypeFilter(typeof(FiltersLogger))]
        [HttpPost(Name = "Exception")]
        public void Exception()
        {
            _logger.LogInformation("Incia proceso Exception  ============>>");
            throw new NotImplementedException();
        }
    }
}

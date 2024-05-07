using Broxel.Logger.Filters;
using Broxel.Logger.Services;
using Microsoft.AspNetCore.Mvc;

namespace Broxel.Logger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorrelationIdController : ControllerBase
    {

        private readonly ILogger<CorrelationIdController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public CorrelationIdController(ILogger<CorrelationIdController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [TypeFilter(typeof(FiltersLogger))]
        [HttpGet(Name = "CorrelationId")]
        public async Task<string> CorrelationId([FromHeader(Name="CorrelationId")] string CorrelationId)
        {
            _logger.LogInformation("Incia proceso CorrelationId  ============>>");

            await CorrelationIdInvocaServicio2();
            return "CorrelationId invoca a Servicio2";
        }

        private async Task CorrelationIdInvocaServicio2()
        {
            var httpClient = _httpClientFactory.CreateClient();
            foreach (var header in Request.Headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value.ToString());
            }
            _ = await httpClient.GetStringAsync("http://localhost:5192/WeatherForecast");
        }
    }
}

using FakeStoreDBAPI.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FakeStoreDBAPI.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetDashboardMetrics(CancellationToken cancellationToken)
        {
            var dashboardMetrics = await _dashboardService.GetDashboardMetrics(cancellationToken);
            return Ok(dashboardMetrics);
        }

    }
}

using FakeStoreDBAPI.Host.DTO.Dashboard;

namespace FakeStoreDBAPI.Host.Services.Interfaces
{
    public interface IDashboardService
    {
        public Task<DashboardMetricsDto> GetDashboardMetrics(CancellationToken cancellationToken);
    }
}

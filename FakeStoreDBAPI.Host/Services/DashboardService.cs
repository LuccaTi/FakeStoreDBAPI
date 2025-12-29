using AutoMapper;
using FakeStoreDBAPI.Host.Data;
using FakeStoreDBAPI.Host.DTO.Dashboard;
using FakeStoreDBAPI.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FakeStoreDBAPI.Host.Services
{
    public class DashboardService : IDashboardService
    {
        private const string _className = "DashboardService";
        private readonly FakeStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(FakeStoreDbContext context, IMapper mapper, ILogger<DashboardService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DashboardMetricsDto> GetDashboardMetrics(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - GetDashboardMetrics - Attempting to obtain dashboard metrics");
            
            var activeOrders = await _context.Orders.CountAsync(o => o.IsActive == true);
            _logger.LogDebug($"{_className} - GetDashboardMetrics - Active orders: {activeOrders}");

            var cancelledOrders = await _context.Orders.CountAsync(o => o.IsActive == false);
            _logger.LogDebug($"{_className} - GetDashboardMetrics - Cancelled orders: {cancelledOrders}");

            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var deliveredToday = await _context.Orders
                .CountAsync(o => o.DeliveredDate >= today && o.DeliveredDate < tomorrow);
            _logger.LogDebug($"{_className} - GetDashboardMetrics - Orders delivered today ({DateTime.UtcNow.Date.ToShortDateString()}): {deliveredToday}");

            var dashboardMetricsDto = new DashboardMetricsDto()
            {
                ActiveOrders = activeOrders,
                CancelledOrders = cancelledOrders,
                DeliveredToday = deliveredToday
            };

            return dashboardMetricsDto;
        }
    }
}

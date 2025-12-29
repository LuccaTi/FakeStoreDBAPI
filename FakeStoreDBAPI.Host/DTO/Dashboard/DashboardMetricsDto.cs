namespace FakeStoreDBAPI.Host.DTO.Dashboard
{
    public class DashboardMetricsDto
    {
        public int ActiveOrders { get; set; }
        public int CancelledOrders { get; set; }
        public int DeliveredToday { get; set; }
    }
}

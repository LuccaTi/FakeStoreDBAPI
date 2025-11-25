namespace FakeStoreDBAPI.Host.Models.Interfaces
{
    public interface IAuditable
    {
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
    }
}

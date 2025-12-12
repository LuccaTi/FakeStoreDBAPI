using FakeStoreDBAPI.Host.DTO.ProcessedFileLog;

namespace FakeStoreDBAPI.Host.Services.Interfaces
{
    public interface IProcessedFileLogService
    {
        public Task<ProcessedFileLogDto?> GetByFileNameAsync(string fileName);
        public Task<ProcessedFileLogDto> PostAsync(CreateProcessedFileLogDto createProcessedFileLogDto);
    }
}

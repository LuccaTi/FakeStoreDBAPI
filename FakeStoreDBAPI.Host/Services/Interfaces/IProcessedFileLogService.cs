using FakeStoreDBAPI.Host.DTO.ProcessedFileLog;

namespace FakeStoreDBAPI.Host.Services.Interfaces
{
    public interface IProcessedFileLogService
    {
        public Task<IEnumerable<ProcessedFileLogDto>> GetAllAsync();
        public Task<ProcessedFileLogDto?> GetByIdAsync(long id); 
        public Task<ProcessedFileLogDto?> GetByFileNameAsync(string fileName);
        public Task<ProcessedFileLogDto> PostAsync(CreateProcessedFileLogDto processedFileLogDto);
        public Task PatchAsync(UpdateProcessedFileLogDto processedFileLogDto, long id);
    }
}

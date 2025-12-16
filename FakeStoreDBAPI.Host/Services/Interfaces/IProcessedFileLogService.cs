using FakeStoreDBAPI.Host.DTO.ProcessedFileLog;

namespace FakeStoreDBAPI.Host.Services.Interfaces
{
    public interface IProcessedFileLogService
    {
        public Task<IEnumerable<ProcessedFileLogDto>> GetAllAsync(CancellationToken cancellationToken);
        public Task<ProcessedFileLogDto?> GetByIdAsync(long id, CancellationToken cancellationToken); 
        public Task<ProcessedFileLogDto?> GetByFileNameAsync(string fileName, CancellationToken cancellationToken);
        public Task<ProcessedFileLogDto> PostAsync(CreateProcessedFileLogDto processedFileLogDto, CancellationToken cancellationToken);
        public Task PatchAsync(UpdateProcessedFileLogDto processedFileLogDto, long id, CancellationToken cancellationToken);
    }
}

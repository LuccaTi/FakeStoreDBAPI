using AutoMapper;
using FakeStoreDBAPI.Host.Data;
using FakeStoreDBAPI.Host.DTO.Customer;
using FakeStoreDBAPI.Host.DTO.ProcessedFileLog;
using FakeStoreDBAPI.Host.Exceptions;
using FakeStoreDBAPI.Host.Models;
using FakeStoreDBAPI.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FakeStoreDBAPI.Host.Services
{
    public class ProcessedFileLogService : IProcessedFileLogService
    {
        private const string _className = "ProcessedFileLogService";
        private readonly FakeStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressService> _logger;

        public ProcessedFileLogService(FakeStoreDbContext context, IMapper mapper, ILogger<AddressService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProcessedFileLogDto>> GetAllAsync()
        {
            _logger.LogDebug($"Attempting to obtain all processed file log records...");
            var processedFileLogs = await _context.ProcessedFileLogs.ToListAsync();

            if (processedFileLogs.Count != 0)
            {
                _logger.LogDebug($"{_className} - GetAllAsync - Records obtained: {processedFileLogs.Count}");
            }
            else
            {
                _logger.LogWarning($"{_className} - GetAllAsync - List of processed file logs is empty");
            }

            return _mapper.Map<IEnumerable<ProcessedFileLogDto>>(processedFileLogs);
        }

        public async Task<ProcessedFileLogDto?> GetByIdAsync(long id)
        {
            _logger.LogDebug($"{_className} - GetByIdAsync - Attempting to find processed file log with ID: {id}");
            if (id == 0)
                throw new InvalidIdException($"Processed file log  ID cannot be zero!");

            var processedFileLog = await _context.ProcessedFileLogs.FindAsync(id);
            if (processedFileLog == null)
                throw new NotFoundException($"Processed file log  ID with ID: {id} was not found!");

            _logger.LogDebug($"{_className} - GetByIdAsync - Found processed file log  ID with ID: {id}");
            return _mapper.Map<ProcessedFileLogDto>(processedFileLog);
        }

        public async Task<ProcessedFileLogDto?> GetByFileNameAsync(string fileName)
        {
            _logger.LogDebug($"{_className} - GetByFileNameAsync - Attempting to obtain processed file log with filename: {fileName}");
            if (string.IsNullOrEmpty(fileName))
                throw new InvalidResourceException("Filename provided cannot be null or empty!");

            var processedFileLog = await _context.ProcessedFileLogs.FirstOrDefaultAsync(p => p.FileName == fileName);
            if (processedFileLog == null)
                throw new NotFoundException($"Resource with name: {fileName} was not found!");

            _logger.LogDebug($"{_className} - GetByFileNameAsync - Found processed file log with filename: {fileName}");
            return _mapper.Map<ProcessedFileLogDto>(processedFileLog);
        }

        public async Task<ProcessedFileLogDto> PostAsync(CreateProcessedFileLogDto processedFileLogDto)
        {
            _logger.LogDebug($"{_className} - PostAsync - Attempting to post processed file log with filename: {processedFileLogDto.FileName}");
            if (string.IsNullOrEmpty(processedFileLogDto.FileName))
                throw new InvalidResourceException("Filename provided cannot be null or empty!");

            var processedFileToPost = _mapper.Map<ProcessedFileLog>(processedFileLogDto);
            _context.ProcessedFileLogs.Add(processedFileToPost);
            await _context.SaveChangesAsync();

            var postedProcessedFile = _mapper.Map<ProcessedFileLogDto>(processedFileToPost);
            _logger.LogDebug($"{_className} - PostAsync - Posted processed file with ID: {postedProcessedFile.Id}");
            return postedProcessedFile;
        }

        public async Task PatchAsync(UpdateProcessedFileLogDto processedFileLogDto, long id)
        {
            _logger.LogDebug($"{_className} - PatchAsync - Attempting to patch customer with ID: {id}");
            if (string.IsNullOrEmpty(processedFileLogDto.FileName))
                throw new InvalidResourceException("Processed file log filename cannot be null or empty!");

            var processedFileToUpdate = await _context.ProcessedFileLogs.FindAsync(id);
            if (processedFileToUpdate == null)
                throw new NotFoundException($"Processed file log with ID: {id} was not found!");

            _mapper.Map(processedFileLogDto, processedFileToUpdate);

            await _context.SaveChangesAsync();
            _logger.LogDebug($"{_className} - PatchAsync - Patched processed file log with ID: {id}");
        }
    }
}

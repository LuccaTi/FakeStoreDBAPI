using AutoMapper;
using FakeStoreDBAPI.Host.Data;
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

        public async Task<ProcessedFileLogDto> PostAsync(CreateProcessedFileLogDto createProcessedFileLogDto)
        {
            _logger.LogDebug($"{_className} - PostAsync - Attempting to post processed file log with filename: {createProcessedFileLogDto.FileName}");
            if (string.IsNullOrEmpty(createProcessedFileLogDto.FileName))
                throw new InvalidResourceException("Filename provided cannot be null or empty!");

            var processedFileToPost = _mapper.Map<ProcessedFileLog>(createProcessedFileLogDto);
            _context.ProcessedFileLogs.Add(processedFileToPost);
            await _context.SaveChangesAsync();

            var postedProcessedFile = _mapper.Map<ProcessedFileLogDto>(processedFileToPost);
            _logger.LogDebug($"{_className} - PostAsync - Posted processed file with ID: {postedProcessedFile.Id}");
            return postedProcessedFile;
        }
    }
}

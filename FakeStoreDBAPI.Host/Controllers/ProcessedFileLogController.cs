using FakeStoreDBAPI.Host.DTO.ProcessedFileLog;
using FakeStoreDBAPI.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FakeStoreDBAPI.Host.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcessedFileLogController : ControllerBase
    {
        private readonly IProcessedFileLogService _processedFileService;
        
        public ProcessedFileLogController(IProcessedFileLogService processedFileLogService)
        {
            _processedFileService = processedFileLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var processedFileLogs = await _processedFileService.GetAllAsync(cancellationToken);
            return Ok(processedFileLogs);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            var processedFile = await _processedFileService.GetByIdAsync(id, cancellationToken);
            return Ok(processedFile);
        }

        [HttpGet("{fileName}", Name = "GetFileByFileName")]
        public async Task<IActionResult> GetByFileNameAsync([FromRoute] string fileName, CancellationToken cancellationToken)
        {
            var processedFile = await _processedFileService.GetByFileNameAsync(fileName, cancellationToken);
            return Ok(processedFile);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateProcessedFileLogDto createProcessedFileLogDto, CancellationToken cancellationToken)
        {
            var createdProcessedFile = await _processedFileService.PostAsync(createProcessedFileLogDto, cancellationToken);
            return CreatedAtRoute("GetFileByFileName", new { fileName = createdProcessedFile.FileName }, createdProcessedFile);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync([FromBody] UpdateProcessedFileLogDto processedFileDto, [FromRoute] long id, CancellationToken cancellationToken)
        {
            await _processedFileService.PatchAsync(processedFileDto, id, cancellationToken);
            return NoContent();
        }
    }
}

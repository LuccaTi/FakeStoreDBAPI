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

        [HttpGet("{fileName}", Name = "GetFileByFileName")]
        public async Task<IActionResult> GetByFileNameAsync([FromRoute] string fileName)
        {
            var processedFile = await _processedFileService.GetByFileNameAsync(fileName);
            return Ok(processedFile);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateProcessedFileLogDto createProcessedFileLogDto)
        {
            var createdProcessedFile = await _processedFileService.PostAsync(createProcessedFileLogDto);
            return CreatedAtRoute("GetFileByFileName", new { id = createdProcessedFile.Id }, createdProcessedFile);
        }
    }
}

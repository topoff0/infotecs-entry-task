using Application.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class CsvFileProcessingController : ControllerBase
    {
        private readonly IFileProcessingService _fileProcessingService;
        private readonly ILogger<CsvFileProcessingController> _logger;

        public CsvFileProcessingController(IFileProcessingService fileService, ILogger<CsvFileProcessingController> logger)
        {
            _fileProcessingService = fileService;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            try
            {
                if (file is null || file.Length == 0)
                    return BadRequest("File wasn't uploaded or is empty");

                using var stream = file.OpenReadStream();
                await _fileProcessingService.ProcessCsvAsync(file.FileName, stream);

                return Ok("Successful upload");
            }
            catch (InvalidOperationException expectedException)
            {
                return BadRequest(
                    $"Validation error has occurred while trying to upload data from file: {expectedException.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"An unexpected error in `Upload` endpoint: {ex.Message}");
                return BadRequest("Internal server error. Please try again later");
            }
        }
    }
}
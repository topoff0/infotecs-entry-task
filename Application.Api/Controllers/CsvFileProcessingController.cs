using Application.Api.Dtos;
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

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "Health", timestamp = DateTime.UtcNow});
        }


        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] CsvFileRequest request)
        {
            try
            {
                if (request.File is null || request.File.Length == 0)
                    return BadRequest("File wasn't uploaded or is empty");

                using var stream = request.File.OpenReadStream();
                await _fileProcessingService.ProcessCsvAsync(request.File.FileName, stream);

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
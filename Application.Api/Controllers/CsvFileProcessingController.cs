using Application.Api.Dtos;
using Application.Core.Dtos;
using Application.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Api.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class CsvFileProcessingController : ControllerBase
    {
        private readonly IFileProcessingService _fileProcessingService;
        private readonly IResultsProcessingService _resultsProcessingService;

        private readonly ILogger<CsvFileProcessingController> _logger;

        public CsvFileProcessingController(IFileProcessingService fileService,
                                           IResultsProcessingService resultsProcessingService,
                                           ILogger<CsvFileProcessingController> logger)
        {
            _fileProcessingService = fileService;
            _resultsProcessingService = resultsProcessingService;
            _logger = logger;
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "Health", timestamp = DateTime.UtcNow });
        }


        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] CsvFileRequest request)
        {
            if (request.File is null || request.File.Length == 0)
                return BadRequest("File wasn't uploaded or is empty");

            using var stream = request.File.OpenReadStream();
            await _fileProcessingService.ProcessCsvAsync(request.File.FileName, stream);

            return Ok("Successful upload");
        }

        [HttpGet("results")]
        public async Task<IActionResult> GetResults([FromQuery] ResultsFilters filters)
        {
            var query = _resultsProcessingService.GetFilteredResult(filters);

            var list = await query.ToListAsync();
            return Ok(list);
        }
    }
}
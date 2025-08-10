using Application.Api.Dtos;
using Application.Core.Dtos;
using Application.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Application.Api.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class CsvFileProcessingController : ControllerBase
    {
        private readonly ILogger<CsvFileProcessingController> _logger;

        private readonly IFileProcessingService _fileProcessingService;
        private readonly IResultsProcessingService _resultsProcessingService;
        private readonly IValuesProcessingService _valueProcessingService;

        public CsvFileProcessingController(ILogger<CsvFileProcessingController> logger,
                                           IFileProcessingService fileService,
                                           IResultsProcessingService resultsProcessingService,
                                           IValuesProcessingService valueProcessingService)
        {
            _logger = logger;
            _fileProcessingService = fileService;
            _resultsProcessingService = resultsProcessingService;
            _valueProcessingService = valueProcessingService;
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

        [HttpGet("last-values")]
        public async Task<IActionResult> GetLastValues([FromQuery] LastValuesDto request)
        {
            if (request.FileName is null)
                return BadRequest("Incorrect filename");

            var query = _valueProcessingService.GetLastSortedValues(request.FileName);
            var result = await query.ToListAsync();

            return Ok(result);
        }
    }
}
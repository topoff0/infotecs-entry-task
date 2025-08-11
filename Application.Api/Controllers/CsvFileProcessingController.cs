using Application.Api.Dtos;
using Application.Core.Dtos;
using Application.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Api.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class CsvFileProcessingController : ControllerBase
    {
        private readonly IFileProcessingService _fileProcessingService;
        
        private readonly IResultService _resultService;
        private readonly IMetricService _metricService;

        public CsvFileProcessingController(IFileProcessingService fileService,
                                           IResultService resultService,
                                           IMetricService metricService)
        {
            _fileProcessingService = fileService;
            _resultService = resultService;
            _metricService = metricService;
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
            var query = _resultService.GetFiltered(filters);

            var list = await query.ToListAsync();
            return Ok(list);
        }

        [HttpGet("last-values")]
        public async Task<IActionResult> GetLastValues([FromQuery] LastValuesDto request)
        {
            if (request.FileName is null)
                return BadRequest("Incorrect filename");

            var query = _metricService.GetLastSorted(request.FileName);
            var result = await query.ToListAsync();

            return Ok(result);
        }
    }
}
#if TEST
using Application.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers.Tests
{
    [ApiController]
    [Route("api/test/files")]
    public class TestCsvFileController : ControllerBase
    {
        [HttpGet("throw-validation")]
        public IActionResult ThrowValidation() => throw new CustomValidationException("Test validation error");

        [HttpGet("throw-exception")]
        public IActionResult ThrowException() => throw new Exception("Test server error");
    }
}
#endif
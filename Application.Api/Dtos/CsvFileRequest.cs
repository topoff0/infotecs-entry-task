 namespace Application.Api.Dtos
{
    public class CsvFileRequest
    {
        public IFormFile File { get; set; } = default!;
    }
}
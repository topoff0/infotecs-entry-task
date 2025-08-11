namespace Application.Core.Interfaces.Services
{
    public interface IFileProcessingService
    {
        Task ProcessCsvAsync(string fileName, Stream csvStream);
    }
}
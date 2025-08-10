namespace Application.Core.Interfaces
{
    public interface IFileProcessingService
    {
        Task ProcessCsvAsync(string fileName, Stream csvStream);
    }
}
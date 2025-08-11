using Application.Core.Interfaces.Calculations;
using Application.Core.Interfaces.Parsers;
using Application.Core.Interfaces.Services;
using Application.Data.Data;

namespace Application.Services.FileProcessing
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly ApplicationDbContext _db;
        private readonly ICsvMetricsParser _csvMetricsParser;
        private readonly IMetricCalculator _metricCalculator;

        public FileProcessingService(ApplicationDbContext dbContext, IMetricCalculator metricCalculator, ICsvMetricsParser csvMetricsParser)
        {
            _db = dbContext;
            _csvMetricsParser = csvMetricsParser;
            _metricCalculator = metricCalculator;
        }

        public async Task ProcessCsvAsync(string fileName, Stream csvStream)
        {
            var metrics = await _csvMetricsParser.ParseAsync(fileName, csvStream);

            await using var transaction = await _db.Database.BeginTransactionAsync();

            var oldValues = _db.Metrics.Where(m => m.FileName == fileName);
            _db.Metrics.RemoveRange(oldValues);

            var oldResults = _db.Results.Where(r => r.FileName == fileName);
            _db.Results.RemoveRange(oldResults);

            await _db.SaveChangesAsync();

            _db.Metrics.AddRange(metrics);
            await _db.SaveChangesAsync();

            var result = _metricCalculator.Calculate(fileName, metrics);

            await _db.Results.AddAsync(result);
            await _db.SaveChangesAsync();

            await _db.Database.CommitTransactionAsync();
        }
    }
}
using System.Globalization;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Data.Data;
using CsvHelper;
using CsvHelper.Configuration;

namespace Application.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly ApplicationDbContext _db;

        public FileProcessingService(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task ProcessCsvAsync(string fileName, Stream csvStream)
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true
            });

            var records = new List<Metric>();

            while (await csv.ReadAsync())
            {
                if (csv.Parser.Record?.Length != 3)
                    throw new InvalidOperationException("Wrong .csv file format");

                if (!DateTime.TryParse(csv.GetField(0), out var date))
                    throw new InvalidOperationException($"Wrong date format: {date}");

                if (!double.TryParse(csv.GetField(1),
                    NumberStyles.Float, CultureInfo.InvariantCulture, out var execTime))
                    throw new InvalidOperationException($"Wrong execution time: ${execTime}");

                if (!double.TryParse(csv.GetField(2),
                    NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
                    throw new InvalidOperationException($"Wrong value: ${value}");


                if (date < new DateTime(2000, 1, 1, 0, 0, 0) || date > DateTime.UtcNow)
                    throw new InvalidOperationException("Date is out of range");

                if (execTime < 0)
                    throw new InvalidOperationException("Execution time must be positive");

                if (value < 0)
                    throw new InvalidOperationException("Value must be positive");

                records.AddRange(new Metric
                {
                    FileName = fileName,
                    DateStart = date,
                    ExecutionTime = execTime,
                    Value = value
                });
            }

            if (records.Count < 1 || records.Count > 10_000)
                throw new InvalidOperationException("Records count is out of range");

            await using var transaction = await _db.Database.BeginTransactionAsync();

            var oldValues = _db.Metrics.Where(m => m.FileName == fileName);
            _db.Metrics.RemoveRange(oldValues);

            var oldResults = _db.Results.Where(r => r.FileName == fileName);
            _db.Results.RemoveRange(oldResults);

            await _db.SaveChangesAsync();

            var minDate = records.Min(r => r.DateStart);
            var maxDate = records.Max(r => r.DateStart);
            var deltaSeconds = (maxDate - minDate).TotalSeconds;
            var avgExecutionTime = records.Average(r => r.ExecutionTime);
            var avgValue = records.Average(r => r.Value);
            var sortedValues = records.OrderBy(r => r.Value).ToList();
            var medianValue = sortedValues.Count % 2 == 0
                ? (sortedValues[sortedValues.Count / 2 - 1].Value + sortedValues[sortedValues.Count / 2].Value) / 2.0
                : sortedValues[sortedValues.Count / 2].Value;
            var maxValue = sortedValues[^1].Value;
            var minValue = sortedValues[0].Value;

            var resultEntity = new Result
            {
                FileName = fileName,
                MinDate = minDate,
                TimeDeltaSeconds = deltaSeconds,
                AvgExecutionTime = avgExecutionTime,
                AvgValue = avgValue,
                MedianValue = medianValue,
                MaxValue = maxValue,
                MinValue = minValue
            };

            await _db.Results.AddAsync(resultEntity);
            await _db.SaveChangesAsync();

            await _db.Database.CommitTransactionAsync();
        }
    }
}
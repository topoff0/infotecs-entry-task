using System.Globalization;
using Application.Core.Entities;
using Application.Core.Exceptions;
using Application.Core.Interfaces.Parsers;
using Application.Core.Interfaces.Validations;
using Application.Services.Validations;
using CsvHelper;
using CsvHelper.Configuration;

namespace Application.Services.Parsers
{
    public class CsvMetricsParser : ICsvMetricsParser
    {
        private readonly IMetricValidator _metricValidator;

        public CsvMetricsParser(IMetricValidator metricValidator)
        {
            _metricValidator = metricValidator;
        }

        public async Task<List<Metric>> ParseAsync(string fileName, Stream stream)
        {
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true
            });

            var records = new List<Metric>();

            while (await csv.ReadAsync())
            {
                var date = ParseDate(csv.GetField(0));
                var execTime = ParseDouble(csv.GetField(1), "execution time");
                var value = ParseDouble(csv.GetField(2), "value");

                var metric = new Metric
                {
                    FileName = fileName,
                    DateStart = DateTime.SpecifyKind(date, DateTimeKind.Local).ToUniversalTime(),
                    ExecutionTime = execTime,
                    Value = value
                };

                _metricValidator.Validate(metric);

                records.Add(metric);
            }

            _metricValidator.ValidateBatch(records);

            return records;
        }

        private static DateTime ParseDate(string? field)
        {
            if (!DateTime.TryParse(field, out var date))
                throw new CustomValidationException($"Wrong date format: {field}");
            return date;
        }

        private static double ParseDouble(string? field, string name)
        {
            if (!double.TryParse(field, NumberStyles.Float, CultureInfo.InvariantCulture, out var number))
                throw new CustomValidationException($"Wrong {name}: {field}");
            return number;
        }
    }
}
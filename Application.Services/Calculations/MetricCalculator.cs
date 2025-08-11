using Application.Core.Entities;
using Application.Core.Interfaces.Calculations;

namespace Application.Services.Calculations
{
    public class MetricCalculator : IMetricCalculator
    {
        public Result Calculate(string fileName, List<Metric> records)
        {
            var minDate = records.Min(r => r.DateStart);
            var maxDate = records.Max(r => r.DateStart);
            var sorted = records.OrderBy(r => r.Value).ToList();

            return new Result
            {
                FileName = fileName,
                MinDate = minDate,
                TimeDeltaSeconds = (maxDate - minDate).TotalSeconds,
                AvgExecutionTime = records.Average(r => r.ExecutionTime),
                AvgValue = records.Average(r => r.Value),
                MedianValue = sorted.Count % 2 == 0
                    ? (sorted[sorted.Count / 2 - 1].Value + sorted[sorted.Count / 2].Value) / 2.0
                    : sorted[sorted.Count / 2].Value,
                MaxValue = sorted[^1].Value,
                MinValue = sorted[0].Value
            };
        }
    }
}
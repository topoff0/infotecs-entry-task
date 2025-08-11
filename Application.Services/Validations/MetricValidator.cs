using Application.Core.Entities;
using Application.Core.Exceptions;
using Application.Core.Interfaces.Validations;
using Application.Shared;

namespace Application.Services.Validations
{
    public class MetricValidator : IMetricValidator
    {
        public void Validate(Metric metric)
        {
            if (metric.DateStart < AppConstants.MinDate || metric.DateStart > DateTime.UtcNow)
                throw new CustomValidationException("Date is out of range");

            if (metric.ExecutionTime < 0 || metric.Value < 0)
                throw new CustomValidationException("Values must be positive");
        }

        public void ValidateBatch(List<Metric> metrics)
        {
            if (metrics.Count is < 1 or > 10_000)
                throw new CustomValidationException("Records count is out of range");
        }
    }
}
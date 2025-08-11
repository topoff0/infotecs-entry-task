using Application.Core.Dtos;
using Application.Core.Interfaces.Data;
using Application.Core.Interfaces.Services;
using Application.Shared;

namespace Application.Services.DataProcessing
{
    public class MetricService : IMetricService
    {
        private readonly IMetricRepository _metricRepository;

        public MetricService(IMetricRepository metricRepository)
        {
            _metricRepository = metricRepository;
        }

        public IQueryable<MetricDto> GetLastSorted(string fileName)
        {
            return _metricRepository.Query()
            .Where(m => m.FileName == fileName)
            .OrderByDescending(m => m.DateStart)
            .Take(AppConstants.LastValuesCount)
            .OrderBy(m => m.DateStart)
            .Select(m => new MetricDto(m.DateStart, m.ExecutionTime, m.Value));
        }
    }
}
using Application.Core.Dtos;

namespace Application.Core.Interfaces.Services
{
    public interface IMetricService
    {
        public IQueryable<MetricDto> GetLastSorted(string fileName);
    }
}
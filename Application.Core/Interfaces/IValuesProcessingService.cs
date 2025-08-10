using Application.Core.Dtos;

namespace Application.Core.Interfaces
{
    public interface IValuesProcessingService
    {
        public IQueryable<MetricDto> GetLastSortedValues(string fileName);
    }
}
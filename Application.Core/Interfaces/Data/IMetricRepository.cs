using Application.Core.Entities;

namespace Application.Core.Interfaces.Data
{
    public interface IMetricRepository
    {
        public IQueryable<Metric> Query();
        public Task RemoveByFileNameAsync(string filename);
        public Task AddRangeAsync(IEnumerable<Metric> metrics);
    }
}
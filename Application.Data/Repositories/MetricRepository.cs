using Application.Core.Entities;
using Application.Core.Interfaces.Data;
using Application.Data.Data;

namespace Application.Data.Repositories
{
    public class MetricRepository : IMetricRepository
    {
        private readonly ApplicationDbContext _db;

        public MetricRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }


        public IQueryable<Metric> Query() => _db.Metrics;

        public async Task AddRangeAsync(IEnumerable<Metric> metrics)
        {
            await _db.AddRangeAsync(metrics);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveByFileNameAsync(string filename)
        {
            var metrics = _db.Metrics.Where(m => m.FileName == filename);
            _db.Metrics.RemoveRange(metrics);
            await _db.SaveChangesAsync();
        }
    }
}
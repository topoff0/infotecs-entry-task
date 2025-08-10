using Application.Core.Dtos;
using Application.Core.Exceptions;
using Application.Core.Interfaces;
using Application.Data.Data;
using Application.Shared;

namespace Application.Services.DataProcessing
{
    public class ValuesProcessingService : IValuesProcessingService
    {
        private readonly ApplicationDbContext _db;

        public ValuesProcessingService(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IQueryable<MetricDto> GetLastSortedValues(string fileName)
        {
            return _db.Metrics
                    .Where(m => m.FileName == fileName)
                    .OrderByDescending(m => m.DateStart)
                    .Take(AppConstants.LastValuesCount)
                    .OrderBy(m => m.DateStart)
                    .Select(m => new MetricDto(m.DateStart,
                                               m.ExecutionTime,
                                               m.Value));
        }
    }
}
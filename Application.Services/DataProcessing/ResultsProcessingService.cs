using Application.Core.Dtos;
using Application.Core.Entities;
using Application.Core.Exceptions;
using Application.Core.Interfaces;
using Application.Data.Data;

namespace Application.Services.DataProcessing
{
    public class ResultsProcessingService : IResultsProcessingService
    {
        private readonly ApplicationDbContext _db;

        public ResultsProcessingService(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IQueryable<Result> GetFilteredResult(ResultsFilters filters)
        {
            ValidateFilters(filters);

            var query = _db.Results.AsQueryable();

            if (!string.IsNullOrEmpty(filters.FileName))
            {
                query = query.Where(r => r.FileName == filters.FileName);
            }

            if (filters.MinDateFrom.HasValue)
            {
                filters.MinDateFrom = DateTime.SpecifyKind(filters.MinDateFrom.Value, DateTimeKind.Utc); 
                query = query.Where(r => r.MinDate >= filters.MinDateFrom.Value);
            }

            if (filters.MinDateTo.HasValue)
            {
                filters.MinDateTo = DateTime.SpecifyKind(filters.MinDateTo.Value, DateTimeKind.Utc);
                query = query.Where(r => r.MinDate <= filters.MinDateTo);
            }

            if (filters.AvgValueFrom.HasValue)
            {
                query = query.Where(r => r.AvgValue >= filters.AvgValueFrom);
            }

            if (filters.AvgValueTo.HasValue)
            {
                query = query.Where(r => r.AvgValue <= filters.AvgValueTo);
            }

            if (filters.AvgExecTimeFrom.HasValue)
            {
                query = query.Where(r => r.AvgExecutionTime >= filters.AvgExecTimeFrom);
            }

            if (filters.AvgExecTimeTo.HasValue)
            {
                query = query.Where(r => r.AvgExecutionTime <= filters.AvgExecTimeTo);
            }

            return query;
        }

            private void ValidateFilters(ResultsFilters filters)
            {
                if (filters.MinDateFrom.HasValue && filters.MinDateTo.HasValue && filters.MinDateFrom > filters.MinDateTo)
                throw new CustomValidationException("MinDateFrom cannot be greater than MinDateTo.");

                if (filters.AvgValueFrom.HasValue && filters.AvgValueTo.HasValue && filters.AvgValueFrom > filters.AvgValueTo)
                    throw new CustomValidationException("AvgValueFrom cannot be greater than AvgValueTo.");

                if (filters.AvgExecTimeFrom.HasValue && filters.AvgExecTimeTo.HasValue && filters.AvgExecTimeFrom > filters.AvgExecTimeTo)
                    throw new CustomValidationException("AvgExecTimeFrom cannot be greater than AvgExecTimeTo.");
            }
    }
}
using Application.Core.Dtos;
using Application.Core.Entities;
using Application.Core.Interfaces.Data;
using Application.Core.Interfaces.Services;
using Application.Core.Interfaces.Validations;

namespace Application.Services.DataProcessing
{
    public class ResultService : IResultService
    {
        private readonly IResultRepository _resultRepository;
        private readonly IResultValidator _resultValidator;

        public ResultService(IResultRepository resultRepository, IResultValidator resultValidator)
        {
            _resultRepository = resultRepository;
            _resultValidator = resultValidator;
        }

        public IQueryable<Result> GetFiltered(ResultsFilters filters)
        {
            _resultValidator.ValidateFilters(filters);

            var query = _resultRepository.Query();

            if (!string.IsNullOrEmpty(filters.FileName))
                query = query.Where(r => r.FileName == filters.FileName);

            if (filters.MinDateFrom.HasValue)
            {
                var date = DateTime.SpecifyKind(filters.MinDateFrom.Value, DateTimeKind.Utc);
                query = query.Where(r => r.MinDate >= date);
            }

            if (filters.MinDateTo.HasValue)
            {
                var date = DateTime.SpecifyKind(filters.MinDateTo.Value, DateTimeKind.Utc);
                query = query.Where(r => r.MinDate <= date);
            }

            if (filters.AvgValueFrom.HasValue)
                query = query.Where(r => r.AvgValue >= filters.AvgValueFrom.Value);

            if (filters.AvgValueTo.HasValue)
                query = query.Where(r => r.AvgValue <= filters.AvgValueTo.Value);

            if (filters.AvgExecTimeFrom.HasValue)
                query = query.Where(r => r.AvgExecutionTime >= filters.AvgExecTimeFrom.Value);

            if (filters.AvgExecTimeTo.HasValue)
                query = query.Where(r => r.AvgExecutionTime <= filters.AvgExecTimeTo.Value);

            return query;
        }
    }
}
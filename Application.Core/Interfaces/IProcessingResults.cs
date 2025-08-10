using Application.Core.Dtos;
using Application.Core.Entities;

namespace Application.Core.Interfaces
{
    public interface IResultsProcessingService
    {
        public IQueryable<Result> GetFilteredResult(ResultsFilters filters);
    }
}
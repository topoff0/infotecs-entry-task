using Application.Core.Dtos;
using Application.Core.Entities;

namespace Application.Core.Interfaces.Services
{
    public interface IResultService
    {
        public IQueryable<Result> GetFiltered(ResultsFilters filters);
    }
}
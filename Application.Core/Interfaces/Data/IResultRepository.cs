using Application.Core.Entities;

namespace Application.Core.Interfaces.Data
{
    public interface IResultRepository
    {
        public IQueryable<Result> Query();
        Task RemoveByFileNameAsync(string fileName);
        Task AddAsync(Result result);
    }
}
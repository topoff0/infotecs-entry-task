using Application.Core.Entities;
using Application.Core.Interfaces.Data;
using Application.Data.Data;

namespace Application.Data.Repositories
{
    public class ResultRepository : IResultRepository
    {
        private readonly ApplicationDbContext _db;

        public ResultRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task AddAsync(Result result)
        {
            await _db.AddAsync(result);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Result> Query()
        {
            return _db.Results;
        }

        public async Task RemoveByFileNameAsync(string fileName)
        {
            var results = _db.Results.Where(r => r.FileName == fileName);
            _db.RemoveRange(results);
            await _db.SaveChangesAsync();
        }
    }
}
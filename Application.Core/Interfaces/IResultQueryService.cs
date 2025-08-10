using Application.Core.Entities;

namespace Application.Core.Interfaces
{
    public interface IResultQueryService
    {
            Task<IEnumerable<Result>> GetResultsAsync(string? fileName, DateTime? minStart, DateTime? maxStart,
                                              double? avgValueMin, double? avgValueMax,
                                              double? avgExecTimeMin, double? avgExecTimeMax);

            Task<IEnumerable<Metric>> GetLast10ValuesAsync(string fileName);
    }
}
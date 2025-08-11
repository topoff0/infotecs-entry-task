using Application.Core.Entities;

namespace Application.Core.Interfaces.Parsers
{
    public interface ICsvMetricsParser
    {
        public Task<List<Metric>> ParseAsync(string fileName, Stream stream); 
    }
}
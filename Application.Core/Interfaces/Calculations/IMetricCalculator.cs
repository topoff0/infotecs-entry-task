using Application.Core.Entities;

namespace Application.Core.Interfaces.Calculations
{
    public interface IMetricCalculator
    {
        Result Calculate(string fileName, List<Metric> records);
    }
}
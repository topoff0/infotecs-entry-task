using Application.Core.Entities;

namespace Application.Core.Interfaces.Validations
{
    public interface IMetricValidator
    {
        public void Validate(Metric metric);
        public void ValidateBatch(List<Metric> metrics);
    }
}
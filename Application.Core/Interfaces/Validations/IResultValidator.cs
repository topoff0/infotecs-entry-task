using Application.Core.Dtos;

namespace Application.Core.Interfaces.Validations
{
    public interface IResultValidator
    {
        public void ValidateFilters(ResultsFilters filters);
    }
}
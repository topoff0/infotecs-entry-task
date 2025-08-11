using Application.Core.Dtos;
using Application.Core.Exceptions;
using Application.Core.Interfaces.Validations;

namespace Application.Services.Validations
{
    public class ResultValidator : IResultValidator
    {
        public void ValidateFilters(ResultsFilters filters)
        {
            if (filters.MinDateFrom.HasValue && filters.MinDateTo.HasValue && filters.MinDateFrom > filters.MinDateTo)
                throw new CustomValidationException("MinDateFrom cannot be greater than MinDateTo.");

            if (filters.AvgValueFrom.HasValue && filters.AvgValueTo.HasValue && filters.AvgValueFrom > filters.AvgValueTo)
                throw new CustomValidationException("AvgValueFrom cannot be greater than AvgValueTo.");

            if (filters.AvgExecTimeFrom.HasValue && filters.AvgExecTimeTo.HasValue && filters.AvgExecTimeFrom > filters.AvgExecTimeTo)
                throw new CustomValidationException("AvgExecTimeFrom cannot be greater than AvgExecTimeTo.");
        }
    }
}
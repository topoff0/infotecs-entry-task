namespace Application.Core.Dtos
{
    public class ResultsFilters
    {
        public string? FileName { get; set; }

        public DateTime? MinDateFrom { get; set; }
        public DateTime? MinDateTo { get; set; }

        public double? AvgValueFrom { get; set; }
        public double? AvgValueTo { get; set; }

        public double? AvgExecTimeFrom { get; set; }
        public double? AvgExecTimeTo { get; set; }
    }
}
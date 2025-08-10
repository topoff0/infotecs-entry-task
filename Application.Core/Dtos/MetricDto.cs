namespace Application.Core.Dtos
{
    public class MetricDto(DateTime date, double executionTime, double value)
    {
        public DateTime DateStart { get; set; } = date;
        public double ExecutionTime { get; set; } = executionTime;
        public double Value { get; set; } = value;
    }
}
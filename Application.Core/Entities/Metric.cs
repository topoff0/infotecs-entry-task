namespace Application.Core.Entities
{
    public class Metric
    {
        public Guid Id { get; set; }

        public DateTime DateStart { get; set; }
        public double ExecutionTime { get; set; }
        public double Value { get; set; }

        public string FileName { get; set; } = default!;
    }
}
using Application.Core.Dtos;
using Application.Core.Entities;
using Application.Core.Interfaces.Data;
using Application.Services.DataProcessing;
using Application.Shared;
using Moq;

namespace Application.UnitTests
{
    public class MetricServiceTests
    {
        private readonly Mock<IMetricRepository> _metricRepoMock = new();

        private MetricService CreateService(IEnumerable<Metric> seedData)
        {
            _metricRepoMock.Setup(r => r.Query()).Returns(seedData.AsQueryable);
            return new MetricService(_metricRepoMock.Object);
        }

        [Fact]
        public void GetLastSorted_FilteredByFileName()
        {
            // Given
            var testMetrics = new[]
            {
                new Metric { FileName = "test1.csv", Value = 10.0 },
                new Metric { FileName = "test2.csv", Value = 11.0 }
            };
            var service = CreateService(testMetrics);

            // When
            var results = service.GetLastSorted("test1.csv").ToList();

            // Then
            Assert.Single(results);
            Assert.Equal(10.0, results[0].Value);
        }
        
        [Fact]
        public void GetLastSorted_RecordsLimit()
        {
            // Given
            string mainFileName = "tes1.csv";
            var testMetrics = new[]
            {
                new Metric {FileName = mainFileName, DateStart = DateTime.UtcNow.AddDays(-11)},
                new Metric {FileName = mainFileName, DateStart = DateTime.UtcNow.AddDays(-10)},
                new Metric {FileName = mainFileName, DateStart = DateTime.UtcNow.AddDays(-9)},
                new Metric {FileName = mainFileName, DateStart = DateTime.UtcNow.AddDays(-8)},
                new Metric {FileName = mainFileName, DateStart = DateTime.UtcNow.AddDays(-7)},
                new Metric {FileName = mainFileName, DateStart = DateTime.UtcNow.AddDays(-6)},
                new Metric {FileName = mainFileName, DateStart = DateTime.UtcNow.AddDays(-5)},
                new Metric {FileName = mainFileName, DateStart = DateTime.UtcNow.AddDays(-4)},
                new Metric {FileName = mainFileName, DateStart = DateTime.UtcNow.AddDays(-3)},
                new Metric {FileName = mainFileName, DateStart = DateTime.UtcNow.AddDays(-2)},
                new Metric {FileName = mainFileName, DateStart = DateTime.UtcNow.AddDays(-1)},
                new Metric {FileName = mainFileName, DateStart = DateTime.UtcNow},
                new Metric {FileName = "tes2.csv", DateStart = DateTime.UtcNow},
            };
            var service = CreateService(testMetrics);

            // When
            var metricDtos = service.GetLastSorted(mainFileName).ToList();

            // Then
            var expectedMetricsDto = testMetrics
                .Where(m => m.FileName == mainFileName)
                .OrderByDescending(m => m.DateStart)
                .Take(AppConstants.LastValuesCount)
                .OrderBy(m => m.DateStart)
                .Select(m => new MetricDto(m.DateStart, m.ExecutionTime, m.Value)).ToList();


            Assert.Equal(expectedMetricsDto.Count, metricDtos.Count);
            
            for (int i = 0; i < expectedMetricsDto.Count; i++)
            {
                Assert.Equal(expectedMetricsDto[i].DateStart, metricDtos[i].DateStart);
                Assert.Equal(expectedMetricsDto[i].ExecutionTime, metricDtos[i].ExecutionTime);
                Assert.Equal(expectedMetricsDto[i].Value, metricDtos[i].Value);
            }
        }
    }
}
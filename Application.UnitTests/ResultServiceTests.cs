using Application.Core.Dtos;
using Application.Core.Entities;
using Application.Core.Interfaces.Data;
using Application.Core.Interfaces.Validations;
using Application.Services.DataProcessing;
using Moq;

namespace Application.UnitTests
{
    public class ResultServiceTests
    {
        private readonly Mock<IResultRepository> _resultRepoMock = new();
        private readonly Mock<IResultValidator> _validatorMock = new();

        private ResultService CreateService(IEnumerable<Result> seedData)
        {
            _resultRepoMock.Setup(r => r.Query()).Returns(seedData.AsQueryable());
            return new ResultService(_resultRepoMock.Object, _validatorMock.Object);
        }

        [Fact]
        public void GetFiltered_CallsValidator()
        {
            // Given
            var service = CreateService(Array.Empty<Result>());

            var filters = new ResultsFilters();

            // When
            _ = service.GetFiltered(filters).ToList();

            // Then
            _validatorMock.Verify(v => v.ValidateFilters(filters), Times.Once);
        }

        [Fact]
        public void GetFiltered_FilteredByFileName()
        {
            // Given
            var testResults = new[]
            {
                new Result { FileName = "test1.csv"},
                new Result { FileName = "test2.csv"}
            };
            var service = CreateService(testResults);

            var filters = new ResultsFilters() { FileName = "test1.csv" };

            // When
            var results = service.GetFiltered(filters).ToList();

            // Then
            Assert.Single(results);
            Assert.Equal("test1.csv", results[0].FileName);
        }

        [Fact]
        public void GetFiltered_FilteredByDateRange()
        {
            // Given
            var date1 = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            var date2 = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-1), DateTimeKind.Utc);
            var date3 = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-2), DateTimeKind.Utc);

            var testResults = new[]
            {
                new Result { MinDate = date1},
                new Result { MinDate = date2},
                new Result {MinDate = date3 }
            };
            var service = CreateService(testResults);

            var filters = new ResultsFilters() { MinDateFrom = date3.AddHours(1), MinDateTo = date1.AddHours(-1) };

            // When
            var results = service.GetFiltered(filters).ToList();

            // Then
            Assert.Single(results);
            Assert.Equal(date2, results[0].MinDate);
        }

        [Fact]
        public void GetFiltered_FilterByAvgValuesRange()
        {
            // Given
            var testResults = new[]
{
                new Result { AvgValue = 8.0 },
                new Result { AvgValue = 9.0 },
                new Result {AvgValue = 10.0 }
            };
            var service = CreateService(testResults);

            var filters = new ResultsFilters() { AvgValueFrom = 8.5, AvgValueTo = 9.5 };

            // When
            var results = service.GetFiltered(filters).ToList();

            // Then
            Assert.Single(results);
            Assert.Equal(9.0, results[0].AvgValue);
        }

        [Fact]
        public void GetFiltered_FilterByAvgExecTimeRange()
        {
            // Given
            var testResults = new[]
{
                new Result { AvgExecutionTime = 8.0 },
                new Result { AvgExecutionTime = 9.0 },
                new Result {AvgExecutionTime = 10.0 }
            };
            var service = CreateService(testResults);

            var filters = new ResultsFilters() { AvgExecTimeFrom = 8.5, AvgExecTimeTo = 9.5 };

            // When
            var results = service.GetFiltered(filters).ToList();

            // Then
            Assert.Single(results);
            Assert.Equal(9.0, results[0].AvgExecutionTime);
        }

        [Fact]
        public void GetFiltered_NoFilters_ReturnsAll()
        {
            // Given
            var testResults = new[]
            {
                new Result(),
                new Result(),
                new Result()
            };
            var service = CreateService(testResults);

            var filters = new ResultsFilters();

            // When
            var results = service.GetFiltered(filters).ToList();

            // Then
            Assert.Equal(3, results.Count);
        }
    }
}
using DataExtractor.Business.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO;
using Xunit;

namespace DataExtractor.Business.Tests
{
    public class DataExtractorServiceTest
    {
        [Fact]
        public async void TestCsvExtractionService()
        {
            var logger = new Mock<ILogger<DataExtractorService>>();
            var service = new DataExtractorService(logger.Object);
            var projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var path = Path.Combine(projectFolder, @"Resources\", "Test.csv");

            byte[] b = await File.ReadAllBytesAsync(path);
            var fileStream = File.OpenRead(path);

            IFormFile formFile = new FormFile(fileStream, 0, fileStream.Length, "Test", "Test.csv");
            var resp = await service.GetExtractedCsvAsync(formFile);
            Assert.NotNull(resp);
        }
    }
}

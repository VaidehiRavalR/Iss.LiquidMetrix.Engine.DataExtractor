using DataExtractor.Business.Interface;
using DataExtractor.Controllers.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace DataExtractor.Integrations.Tests
{
    public class DataExtractorControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private const string RouteV1 = "/data-extractor/uploadfile/";
        public DataExtractorControllerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task TestDataExtractor()
        {
            //Setup mock file using a memory stream
            var projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var path = Path.Combine(projectFolder, @"Resources\", "dummy.csv");

            byte[] b = await File.ReadAllBytesAsync(path);
            var fileStream = File.OpenRead(path);

            //Mocking service
            var service = new Mock<IDataExtractor>();
            var logger = new Mock<ILogger<DataExtractorController>>();

            IFormFile formFile = new FormFile(fileStream, 0, fileStream.Length, "dummy", "dummy.csv");

            //Arrange
            service.Setup(s => s.GetExtractedCsvAsync(formFile)).ReturnsAsync(b);
            DataExtractorController controller = new DataExtractorController(logger.Object, service.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";
            controller.ControllerContext.HttpContext.Response.ContentType = "application/excel";

            var res = controller.GetExtractedCsvFile(formFile);

            //Assert
            Assert.NotNull(res.FileContents);
        }
    }
}

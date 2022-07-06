using Ardalis.GuardClauses;
using DataExtractor.Business.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DataExtractor.Controllers.v1
{
    /// <summary>  DatExtractor targets to provide clean csv file with intented data extraction from Bank provided stock market transactions csv file </summary>
    /// <response code="403"> You do not have rights to access this endpoint </response>
    /// <response code="500"> An unexpected error occured. See logs for more details </response>
    [Produces("application/json")]
    [ApiController]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public class DataExtractorController : Controller
    {
        //Injected logger object for exception logging
        private readonly ILogger<DataExtractorController> _logger;

        //Injected services are readonly to avoid swapping instances
        private readonly IDataExtractor _dataExtractor;

        /// <summary>  
        /// Constructor
        /// </summary>
        /// <param name="logger"> the <see cref="ILogger"/>instance used</param>
        /// <param name="dataExtractor"> the <see cref="IDataExtractor"/>instance used</param>
        /// <exception cref="ArgumentException">if any of the method parameter is <see langword="null"/>.</exception>
        public DataExtractorController(ILogger<DataExtractorController> logger, IDataExtractor dataExtractor)
        {
            Guard.Against.Null(logger, nameof(logger), "DataExtractorContoller logger object can not be null");
            Guard.Against.Null(dataExtractor, nameof(dataExtractor), "IDataExtractor object can not be null");
            _logger = logger;
            _dataExtractor = dataExtractor;
        }

        /// <summary>  
        /// Get extracted file from uploaded csv 
        /// </summary>
        /// <param name="fromFile">Bank provided stock market transactions csv file</param>
        /// <returns></returns>
        [HttpPost("data-extractor/uploadfile/")]
        public FileContentResult GetExtractedCsvFile(IFormFile formFile)
        {
            Guard.Against.Null(formFile, nameof(formFile), "Please upload csv file");
            var bytes = new byte[0];
            try
            {
                if (formFile.Length > 0)
                {
                    bytes = _dataExtractor.GetExtractedCsvAsync(formFile).Result;
                    HttpContext.Response.ContentType = "application/excel";
                    HttpContext.Response.Headers.Add("Content-Disposition", "inline;swaggerDownload=\"attachment\";filename=\"DataExtractor_Output.csv");
                }
                else
                    _logger.LogWarning("User provided file is empty");

            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in data extraction", ex.Message);
                throw ex;
            }
            return File(bytes, Response.ContentType);
        }
    }
}


using Ardalis.GuardClauses;
using CsvHelper;
using DataExtractor.Business.Interface;
using DataExtractor.DataModel.Extractor;
using DataExtractor.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;

namespace DataExtractor.Business.Implementation
{
    /// <summary>  
    /// Class <code>DataExtractorService</code> is the implementation 
    /// of interface <see cref="IDataExtractor"/>.
    /// </summary>
    public class DataExtractorService : IDataExtractor
    {
        //Injected logger object for exception logging
        private readonly ILogger<DataExtractorService> _logger;

        /// <summary>  
        /// Constructor
        /// </summary>
        /// <param name="logger"> the <see cref="ILogger"/>instance used</param>
        /// <exception cref="ArgumentException">if any of the method parameter is <see langword="null"/>.</exception>
        public DataExtractorService(ILogger<DataExtractorService> logger)
        {
            Guard.Against.Null(logger, nameof(logger), "DataExtractorContoller logger object can not be null");
            _logger = logger;
        }

        /// <summary>  
        /// Extracts the complex csv file to get needed data and generates clean csv file
        /// </summary>
        /// <param name="formFile">csv file</param>
        public async Task<byte[]> GetExtractedCsvAsync(IFormFile formFile)
        {
            _logger.LogInformation("[DataExtractor]Start point to file verification and extraction..");

            //creates temp file and pass for extraction
            var filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var csvFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            filePath = Path.ChangeExtension(filePath, Path.GetExtension(formFile.FileName));

            await using (var stream = System.IO.File.Create(filePath))
            {
                await formFile.CopyToAsync(stream);
            }

            //Code to convert .xlsx file in csv format
            if (Path.GetExtension(formFile.FileName).ToLower() == ".xlsx")
            {
                csvFilePath = Path.ChangeExtension(csvFilePath, ".xlsx");
                await DataExtractorHelper.ConvertToCsv(filePath, csvFilePath);
                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Exception occured during releasing file instance", ex.Message);
                }
                filePath = csvFilePath;
            }
            byte[] bytes = FieldExtractionMapping(filePath);

            _logger.LogInformation("[DataExtractor]End point to file verification and extraction..");
            return bytes;
        }

        private byte[] FieldExtractionMapping(string filePath)
        {
            var bytes = new byte[0];
            try
            {
                List<ResponseExtractor> responseExtractors = new List<ResponseExtractor>();
                CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
                ExtractorMapper extractorMapper = new ExtractorMapper();

                CsvParser<RequestExtractor> csvParser = new CsvParser<RequestExtractor>(csvParserOptions, extractorMapper);
                var result = csvParser.ReadFromFile(filePath, Encoding.ASCII).ToList();

                var resultData = result.Where(x => x.Result != null).ToList();
                if (resultData?.Count > 0)
                {
                    foreach (var item in resultData.Skip(1).ToList()) //skiping the first record as it's header
                    {
                        responseExtractors.Add(new ResponseExtractor
                        {
                            Isin = item.Result?.Isin,
                            Venue = item.Result?.Venue,
                            CfiCode = item.Result?.CfiCode,
                            ContractSize = item.Result?.AlgoParams?.Contains(DataExtractorConstant.PriceMultiplier) == true ?
                                            item.Result?.AlgoParams?.Split('|')[4]?.Split(':')[1] : string.Empty
                        });
                    }
                    using (var write = new StreamWriter(filePath))
                    using (var csv = new CsvWriter(write, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(responseExtractors);
                    }
                    bytes = File.ReadAllBytes(filePath);
                    File.Delete(filePath);
                }
                else
                    _logger.LogWarning("No data found after extraction");

            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured during fields mapping", ex.Message);
                throw ex;
            }
            return bytes;
        }
    }
}

using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DataExtractor.Business.Interface
{
    /// <summary>  
    /// Interface <see cref="IDataExtractor"/>.
    /// for class <code>DataExtractorService</code>
    /// </summary>
    public interface IDataExtractor
    {
        /// <summary>  
        /// Get extracted clean csv file from complex csv file 
        /// </summary>
        Task<byte[]> GetExtractedCsvAsync(IFormFile formFile);
    }
}

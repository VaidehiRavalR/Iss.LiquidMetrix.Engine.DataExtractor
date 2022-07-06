using System.Text.Json.Serialization;

namespace DataExtractor.DataModel.Extractor
{
    /// <summary>  
    /// Class <code>RequestExtractor</code> represents intended fields to be considered from uploaded file
    /// </summary>
    public class RequestExtractor
    {
        // TODO : Enter fields explanation in summary for user understanding

        /// <summary>  
        /// ISIN Code
        /// </summary>
        public string Isin { get; set; }
        /// <summary>  
        /// CFCI Code
        /// </summary>
        public string CfiCode { get; set; }
        /// <summary>  
        /// Venue
        /// </summary>
        public string Venue { get; set; }
        /// <summary>  
        /// Algorithm paramteres
        /// </summary>
        public string AlgoParams { get; set; }
    }
}

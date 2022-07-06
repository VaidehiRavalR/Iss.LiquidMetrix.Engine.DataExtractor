using System.Text.Json.Serialization;

namespace DataExtractor.DataModel.Extractor
{
    public class ResponseExtractor
    {
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
        /// Value of PriceMultiplier
        /// </summary>
        public string ContractSize { get; set; }
    }
}

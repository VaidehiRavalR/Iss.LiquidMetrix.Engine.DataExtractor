using DataExtractor.DataModel.Extractor;
using TinyCsvParser.Mapping;

namespace DataExtractor.Utils
{
    /// <summary>  
    /// Class <code>ExtractorMapper</code>
    /// perfomrs mapping of fields with extracted fields
    /// </summary>
    public class ExtractorMapper : CsvMapping<RequestExtractor>
    {
        public ExtractorMapper() : base()
        {
            MapProperty(1, x => x.Isin);
            MapProperty(3, x => x.Venue);
            MapProperty(6, x => x.CfiCode);
            MapProperty(35, x => x.AlgoParams);
        }

    }
}

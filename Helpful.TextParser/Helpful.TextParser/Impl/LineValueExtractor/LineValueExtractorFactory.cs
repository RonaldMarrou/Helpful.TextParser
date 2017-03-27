using System.Linq;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Impl.LineValueExtractor
{
    public class LineValueExtractorFactory : ILineValueExtractorFactory
    {
        private readonly ILineValueExtractor[] _lineValueExtractors;

        public LineValueExtractorFactory(ILineValueExtractor[] lineValueExtractors)
        {
            _lineValueExtractors = lineValueExtractors;
        }

        public ILineValueExtractor Get(LineValueExtractorType lineValueExtractorType)
        {
            return _lineValueExtractors.First(x => x.Type == lineValueExtractorType);
        }
    }
}

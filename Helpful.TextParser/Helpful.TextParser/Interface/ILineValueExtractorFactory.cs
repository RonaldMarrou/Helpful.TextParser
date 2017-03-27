using Helpful.TextParser.Model;

namespace Helpful.TextParser.Interface
{
    public interface ILineValueExtractorFactory
    {
        ILineValueExtractor Get(LineValueExtractorType lineValueExtractorType);
    }
}

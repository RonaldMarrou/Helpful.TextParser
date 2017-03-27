using Helpful.TextParser.Model;

namespace Helpful.TextParser.Interface
{
    public interface ILineValueExtractor
    {
        LineValue Extract(string line, Element element, Element parentElement = null);

        LineValueExtractorType Type { get; }
    }
}

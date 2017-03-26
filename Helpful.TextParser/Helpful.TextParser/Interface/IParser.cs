using Helpful.TextParser.Model;

namespace Helpful.TextParser.Interface
{
    public interface IParser
    {
        void Parse<T>(DelimitedElement element, string[] lines);
    }
}

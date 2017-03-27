using Helpful.TextParser.Model;

namespace Helpful.TextParser.Interface
{
    public interface IParser
    {
        void Parse<T>(Element element, string[] lines);
    }
}

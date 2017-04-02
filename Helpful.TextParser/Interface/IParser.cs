using Helpful.TextParser.Model;

namespace Helpful.TextParser.Interface
{
    public interface IParser
    {
        Result<T> Parse<T>(Element element, string[] lines);
    }
}

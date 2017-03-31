using Helpful.TextParser.Fluent.Interface;

namespace Helpful.TextParser.Interface
{
    public interface IFluentParser
    {
        IDelimitedDescriptor Delimited(string delimitedBy);

        IPositionDescriptor Position();
    }
}

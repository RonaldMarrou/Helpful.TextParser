using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Fluent.Interface.Delimited;

namespace Helpful.TextParser.Interface
{
    public interface IFluentParser
    {
        IDelimitedDescriptor Delimited(string delimitedBy);

        IPositionDescriptor Position();
    }
}

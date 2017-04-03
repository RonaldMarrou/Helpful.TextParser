using Helpful.TextParser.Interface;
using Helpful.TextParser.Fluent.Impl;
using Helpful.TextParser.Fluent.Interface;

namespace Helpful.TextParser.Impl
{
    public class FluentParser : IFluentParser
    {
        private readonly IParser _parser;

        public FluentParser(IParser parser)
        {
            _parser = parser;
        }

        public IDelimitedDescriptor Delimited(string delimitationString)
        {
            return new DelimitedDescriptor(delimitationString, _parser);
        }

        public IPositionedDescriptor Positioned()
        {
            return new PositionedDescriptor(_parser);
        }
    }
}

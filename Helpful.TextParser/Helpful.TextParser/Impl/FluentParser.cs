using Helpful.TextParser.Interface;
using System;
using Helpful.TextParser.Fluent.Impl.Delimited;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Fluent.Interface.Delimited;

namespace Helpful.TextParser.Impl
{
    public class FluentParser : IFluentParser
    {
        public IDelimitedDescriptor Delimited(string delimitationCharacter)
        {
            return new DelimitedDescriptor(delimitationCharacter);
        }

        public IPositionDescriptor Position()
        {
            throw new NotImplementedException();
        }
    }
}

using Helpful.TextParser.Interface;
using System;
using Helpful.TextParser.Fluent.Interface;
namespace Helpful.TextParser.Impl
{
    public class FluentParser : IFluentParser
    {
        public IDelimitedDescriptor Delimited(string delimitationCharacter)
        {
            throw new NotImplementedException();
        }

        public IPositionDescriptor Position()
        {
            throw new NotImplementedException();
        }
    }
}

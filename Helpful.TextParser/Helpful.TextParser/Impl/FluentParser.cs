﻿using Helpful.TextParser.Interface;
using Helpful.TextParser.Fluent.Impl;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Impl
{
    public class FluentParser : IFluentParser
    {
        private readonly IParser _parser;

        public FluentParser(IParser parser)
        {
            _parser = parser;
        }

        public IDelimitedDescriptor Delimited(string delimitationCharacter)
        {
            return new DelimitedDescriptor(new Element(), _parser);
        }

        public IPositionedDescriptor Positioned()
        {
            return new PositionedDescriptor(new Element(), _parser);
        }
    }
}

using System;
using Helpful.TextParser.Fluent.Interface.Delimited;
using Helpful.TextParser.Fluent.Interface.Delimited.WithoutChildren;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl.Delimited.WithoutChildren
{
    public class DelimitedWithoutChildrenPropertiesDescriptor<TClass> : IDelimitedWithoutChildrenPropertiesDescriptor<TClass>, IDelimitedParseDescriptor
    {
        private readonly DelimitedElement _element;

        private Action<IDelimitedWithoutChildrenPropertyDescriptor<TClass>> _action;

        private readonly IParser _parser;

        public DelimitedWithoutChildrenPropertiesDescriptor(DelimitedElement element, IParser parser)
        {
            _element = element;
            _parser = parser;
        }

        public IDelimitedParseDescriptor Properties(Action<IDelimitedWithoutChildrenPropertyDescriptor<TClass>> properties)
        {
            _action = properties;

            return this;
        }

        public void Parse(string[] lines)
        {
            var delimitedWithoutChildrenPropertyDescriptor = new DelimitedWithoutChildrenPropertyDescriptor<TClass>(_element.Children);

            _action(delimitedWithoutChildrenPropertyDescriptor);

            _parser.Parse<TClass>(_element, lines);
        }

        public void Parse()
        {
            throw new NotImplementedException();
        }
    }
}

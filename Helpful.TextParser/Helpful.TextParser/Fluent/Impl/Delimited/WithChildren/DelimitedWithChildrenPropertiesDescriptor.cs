using System;
using Helpful.TextParser.Fluent.Interface.Delimited;
using Helpful.TextParser.Fluent.Interface.Delimited.WithChildren;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl.Delimited.WithChildren
{
    public class DelimitedWithChildrenPropertiesDescriptor<TClass> : IDelimitedWithChildrenPropertiesDescriptor<TClass>, IDelimitedParseDescriptor
    {
        private readonly DelimitedElement _element;

        private Action<IDelimitedWithChildrenPropertyDescriptor<TClass>> _action;

        private readonly IParser _parser;

        public DelimitedWithChildrenPropertiesDescriptor(DelimitedElement element, IParser parser)
        {
            _element = element;
            _parser = parser;
        }

        public IDelimitedParseDescriptor Properties(Action<IDelimitedWithChildrenPropertyDescriptor<TClass>> properties)
        {
            _action = properties;

            return this;
        }

        public void Parse(string[] lines)
        {
            var delimitedWithChildrenPropertyDescriptor = new DelimitedWithChildrenPropertyDescriptor<TClass>(_element.Children);

            _action(delimitedWithChildrenPropertyDescriptor);

            _parser.Parse<TClass>(_element, lines);
        }

        public void Parse()
        {
            throw new NotImplementedException();
        }
    }
}

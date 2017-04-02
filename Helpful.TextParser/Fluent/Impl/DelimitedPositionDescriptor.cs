using System;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class DelimitedPositionDescriptor<TClass> : IDelimitedPositionDescriptor<TClass>, IDelimitedPropertiesDescriptor<TClass>, IParseDescriptor<TClass> where TClass : class
    {
        private readonly Element _element;

        private Action<IDelimitedPropertyDescriptor<TClass>> _action;
        private readonly IParser _parser;

        public DelimitedPositionDescriptor(Element element, IParser parser)
        {
            _element = element;
            _parser = parser;
        }

        public IDelimitedPropertiesDescriptor<TClass> Position(int position)
        {
            _element.Positions.Add("Position", position);

            return this;
        }

        public IParseDescriptor<TClass> Properties(Action<IDelimitedPropertyDescriptor<TClass>> properties)
        {
            _action = properties;

            var delimitedPropertyDescriptor = new DelimitedPropertyDescriptor<TClass>(_element);

            _action(delimitedPropertyDescriptor);

            return this;
        }

        public Result<TClass> Parse(string[] content)
        {
            return _parser.Parse<TClass>(_element, content);
        }

        public Result<TClass> Parse(Func<string[]> content)
        {
            return _parser.Parse<TClass>(_element, content());
        }
    }
}

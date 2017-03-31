using System;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class PositionedPropertiesOnlyDescriptor<TClass> : IPositionedPropertiesOnlyDescriptor<TClass>, IParseDescriptor<TClass> where TClass : class
    {
        private readonly Element _element;

        private readonly IParser _parser;

        private Action<IPositionedPropertyOnlyDescriptor<TClass>> _action;

        public PositionedPropertiesOnlyDescriptor(Element element, IParser parser)
        {
            _element = element;
            _parser = parser;
        }

        public IParseDescriptor<TClass> Properties(Action<IPositionedPropertyOnlyDescriptor<TClass>> properties)
        {
            _action = properties;

            var positionedPropertyOnlyDescriptor = new PositionedPropertyOnlyDescriptor<TClass>(_element.Elements);

            _action(positionedPropertyOnlyDescriptor);

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

using System;
using System.Linq;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class PositionedPositionDescriptor<TClass> : IPositionedPositionDescriptor<TClass>, IPositionedPropertiesDescriptor<TClass>, IParseDescriptor<TClass> where TClass : class
    {
        private readonly Element _element;

        private Action<IPositionedPropertyDescriptor<TClass>> _action;

        private readonly IParser _parser;

        public PositionedPositionDescriptor(Element element, IParser parser)
        {
            _element = element;
            _parser = parser;
        }

        public IPositionedPropertiesDescriptor<TClass> Position(int startPosition, int endPosition)
        {
            if (startPosition >= endPosition)
            {
                throw new ArgumentException($"Start Position {startPosition} is greather or equal than {endPosition} for {typeof(TClass).FullName}");
            }

            _element.Positions.Add("StartPosition", startPosition);
            _element.Positions.Add("EndPosition", endPosition);

            return this;
        }

        public IParseDescriptor<TClass> Properties(Action<IPositionedPropertyDescriptor<TClass>> properties)
        {
            _action = properties;

            var delimitedPropertyDescriptor = new PositionedPropertyDescriptor<TClass>(_element.Elements);

            _action(delimitedPropertyDescriptor);

            foreach (var tagElements in _element.Elements.Where(x => x.ElementType == ElementType.Tag))
            {
                tagElements.Custom.Add("DelimitationString", _element.Custom["DelimitationString"]);
            }

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

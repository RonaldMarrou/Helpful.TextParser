using System;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class PositionedPropertyPositionDescriptor<TClass> : IPositionedPropertyPositionDescriptor<TClass>, IPositionedPropertiesDescriptor<TClass> where TClass : class
    {
        private readonly Element _element;

        private Action<IPositionedPropertyDescriptor<TClass>> _action;

        public PositionedPropertyPositionDescriptor(Element element)
        {
            _element = element;
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

        public void Properties(Action<IPositionedPropertyDescriptor<TClass>> properties)
        {
            _action = properties;

            var delimitedPropertyDescriptor = new PositionedPropertyDescriptor<TClass>(_element.Elements);

            _action(delimitedPropertyDescriptor);
        }
    }
}

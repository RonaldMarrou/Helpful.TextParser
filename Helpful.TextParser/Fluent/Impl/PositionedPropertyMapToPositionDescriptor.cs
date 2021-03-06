﻿using System;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class PositionedPropertyMapToPositionDescriptor<TClass> : IPositionedPropertyMapToPositionDescriptor<TClass>, IPositionedPropertyMapToPropertiesDescriptor<TClass> where TClass : class
    {
        private readonly Element _element;

        private Action<IPositionedPropertyDescriptor<TClass>> _action;

        public PositionedPropertyMapToPositionDescriptor(Element element)
        {
            _element = element;
        }

        public IPositionedPropertyMapToPropertiesDescriptor<TClass> Position(int startPosition, int endPosition)
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

            var delimitedPropertyDescriptor = new PositionedPropertyDescriptor<TClass>(_element);

            _action(delimitedPropertyDescriptor);
        }
    }
}

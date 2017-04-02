using System;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class DelimitedPropertyPositionDescriptor<TClass> : IDelimitedPropertyPositionDescriptor<TClass>, IDelimitedPropertyPropertiesDescriptor<TClass> where TClass : class
    {
        private readonly Element _element;

        private Action<IDelimitedPropertyDescriptor<TClass>> _action;

        public DelimitedPropertyPositionDescriptor(Element element)
        {
            _element = element;
        }

        public IDelimitedPropertyPropertiesDescriptor<TClass> Position(int position)
        {
            _element.Positions.Add("Position", position);

            return this;
        }

        public void Properties(Action<IDelimitedPropertyDescriptor<TClass>> properties)
        {
            _action = properties;

            var delimitedPropertyDescriptor = new DelimitedPropertyDescriptor<TClass>(_element.Elements);

            _action(delimitedPropertyDescriptor);
        }
    }
}

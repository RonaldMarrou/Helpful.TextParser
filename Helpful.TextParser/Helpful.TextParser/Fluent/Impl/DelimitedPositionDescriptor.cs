using System;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class DelimitedPositionDescriptor<TClass> : IDelimitedPositionDescriptor<TClass>, IDelimitedPropertiesDescriptor<TClass> where TClass : class
    {
        private readonly Element _element;

        private Action<IDelimitedPropertyDescriptor<TClass>> _action;

        public DelimitedPositionDescriptor(Element element)
        {
            _element = element;
        }

        public IDelimitedPropertiesDescriptor<TClass> Position(int position)
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

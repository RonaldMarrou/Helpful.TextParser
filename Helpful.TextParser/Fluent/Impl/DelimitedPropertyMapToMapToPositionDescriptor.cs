using System;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class DelimitedPropertyMapToMapToPositionDescriptor<TClass> : IDelimitedPropertyMapToPositionDescriptor<TClass>, IDelimitedPropertyMapToPropertiesDescriptor<TClass> where TClass : class
    {
        private readonly Element _element;

        private Action<IDelimitedPropertyDescriptor<TClass>> _action;

        public DelimitedPropertyMapToMapToPositionDescriptor(Element element)
        {
            _element = element;
        }

        public IDelimitedPropertyMapToPropertiesDescriptor<TClass> Position(int position)
        {
            _element.Positions.Add("Position", position);

            return this;
        }

        public void Properties(Action<IDelimitedPropertyDescriptor<TClass>> properties)
        {
            _action = properties;

            var delimitedPropertyDescriptor = new DelimitedPropertyDescriptor<TClass>(_element);

            _action(delimitedPropertyDescriptor);
        }
    }
}

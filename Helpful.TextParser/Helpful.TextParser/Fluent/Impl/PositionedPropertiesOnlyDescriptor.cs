using System;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class PositionedPropertiesOnlyDescriptor<TClass> : IPositionedPropertiesOnlyDescriptor<TClass> where TClass : class
    {
        private readonly Element _element;

        private Action<IPositionedPropertyOnlyDescriptor<TClass>> _action;

        public PositionedPropertiesOnlyDescriptor(Element element)
        {
            _element = element;
        }

        public void Properties(Action<IPositionedPropertyOnlyDescriptor<TClass>> properties)
        {
            _action = properties;
        }
    }
}

using System;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class DelimitedPropertiesOnlyDescriptor<TClass> : IDelimitedPropertiesOnlyDescriptor<TClass> where TClass : class
    {
        private readonly Element _element;

        private Action<IDelimitedPropertyOnlyDescriptor<TClass>> _action;

        public DelimitedPropertiesOnlyDescriptor(Element element)
        {
            _element = element;
        }

        public void Properties(Action<IDelimitedPropertyOnlyDescriptor<TClass>> properties)
        {
            _action = properties;
        }
    }
}

using System;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IDelimitedPropertyPropertiesDescriptor<TClass> where TClass : class
    {
        void Properties(Action<IDelimitedPropertyDescriptor<TClass>> properties);
    }
}
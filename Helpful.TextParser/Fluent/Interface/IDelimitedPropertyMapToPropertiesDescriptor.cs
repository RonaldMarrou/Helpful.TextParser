using System;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IDelimitedPropertyMapToPropertiesDescriptor<TClass> where TClass : class
    {
        void Properties(Action<IDelimitedPropertyDescriptor<TClass>> properties);
    }
}
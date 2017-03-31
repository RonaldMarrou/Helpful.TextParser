using System;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IDelimitedPropertiesOnlyDescriptor<TClass> where TClass : class
    {
        void Properties(Action<IDelimitedPropertyOnlyDescriptor<TClass>> properties);
    }
}
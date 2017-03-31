using System;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionedPropertyPropertiesDescriptor<TClass> where TClass : class
    {
        void Properties(Action<IPositionedPropertyDescriptor<TClass>> properties);
    }
}
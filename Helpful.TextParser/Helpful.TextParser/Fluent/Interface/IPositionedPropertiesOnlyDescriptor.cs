using System;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionedPropertiesOnlyDescriptor<TClass> where TClass : class
    {
        void Properties(Action<IPositionedPropertyOnlyDescriptor<TClass>> properties);
    }
}
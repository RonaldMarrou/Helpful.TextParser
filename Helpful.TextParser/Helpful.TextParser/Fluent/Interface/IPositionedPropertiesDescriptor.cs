using System;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionedPropertiesDescriptor<TClass> where TClass : class
    {
        IParseDescriptor<TClass> Properties(Action<IPositionedPropertyDescriptor<TClass>> properties);
    }
}
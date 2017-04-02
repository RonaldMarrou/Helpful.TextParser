using System;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionedPropertiesOnlyDescriptor<TClass> where TClass : class
    {
        IParseDescriptor<TClass> Properties(Action<IPositionedPropertyOnlyDescriptor<TClass>> properties);
    }
}
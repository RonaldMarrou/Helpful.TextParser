using System;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IDelimitedPropertiesDescriptor<TClass> where TClass : class
    {
        IParseDescriptor<TClass> Properties(Action<IDelimitedPropertyDescriptor<TClass>> properties);
    }
}
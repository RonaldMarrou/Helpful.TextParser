using System;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IDelimitedPropertiesOnlyDescriptor<TClass> where TClass : class
    {
        IParseDescriptor<TClass> Properties(Action<IDelimitedPropertyOnlyDescriptor<TClass>> properties);
    }
}
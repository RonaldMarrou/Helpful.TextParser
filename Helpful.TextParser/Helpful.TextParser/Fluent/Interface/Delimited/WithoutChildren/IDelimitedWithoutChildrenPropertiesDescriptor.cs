using System;

namespace Helpful.TextParser.Fluent.Interface.Delimited.WithoutChildren
{
    public interface IDelimitedWithoutChildrenPropertiesDescriptor<TClass>
    {
        IDelimitedParseDescriptor Properties(Action<IDelimitedWithoutChildrenPropertyDescriptor<TClass>> properties);
    }
}

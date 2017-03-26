using System;

namespace Helpful.TextParser.Fluent.Interface.Delimited.WithChildren
{
    public interface IDelimitedWithChildrenPropertiesDescriptor<TClass>
    {
        IDelimitedParseDescriptor Properties(Action<IDelimitedWithChildrenPropertyDescriptor<TClass>> properties);
    }
}

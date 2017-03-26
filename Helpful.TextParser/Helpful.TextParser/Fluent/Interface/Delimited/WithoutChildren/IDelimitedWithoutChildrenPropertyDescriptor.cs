using System;
using System.Linq.Expressions;

namespace Helpful.TextParser.Fluent.Interface.Delimited.WithoutChildren
{
    public interface IDelimitedWithoutChildrenPropertyDescriptor<TClass>
    {
        IDelimitedWithoutChildrenPropertyPositionDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property);
    }
}

using System;
using System.Linq.Expressions;

namespace Helpful.TextParser.Fluent.Interface.Delimited.WithChildren
{
    public interface IDelimitedWithChildrenPropertyDescriptor<TClass>
    {
        IDelimitedWithChildrenPropertyWithChildrenDescriptor<TProperty> Property<TProperty>(Expression<Func<TClass, TProperty>> property);
    }
}

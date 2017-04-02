using System;
using System.Linq.Expressions;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IDelimitedPropertyOnlyDescriptor<TClass> where TClass : class
    {
        IDelimitedPropertyOnlyPositionDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property);
    }
}
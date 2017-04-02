using System;
using System.Linq.Expressions;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionedPropertyOnlyDescriptor<TClass> where TClass : class
    {
        IPositionedPropertyOnlyPositionDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property);
    }
}
using System;
using System.Linq.Expressions;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionedPropertyDescriptor<TClass> where TClass : class
    {
        IPositionedPropertyMapToDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property);
    }
}
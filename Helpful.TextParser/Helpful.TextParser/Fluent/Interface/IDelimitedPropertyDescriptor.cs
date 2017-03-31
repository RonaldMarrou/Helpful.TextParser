using System;
using System.Linq.Expressions;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IDelimitedPropertyDescriptor<TClass> where TClass : class
    {
        IDelimitedPropertyMapToDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property);
    }
}
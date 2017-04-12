using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IDelimitedPropertyDescriptor<TClass> where TClass : class
    {
        IDelimitedPropertyPositionDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property);

        IDelimitedPropertyMapToPositionDescriptor<TChildClass> MapTo<TChildClass>(Expression<Func<TClass, List<TChildClass>>> child, string tag) where TChildClass : class;
    }
}
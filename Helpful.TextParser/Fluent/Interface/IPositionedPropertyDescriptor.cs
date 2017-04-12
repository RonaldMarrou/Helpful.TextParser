using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IPositionedPropertyDescriptor<TClass> where TClass : class
    {
        IPositionedPropertyPositionDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property);

        IPositionedPropertyMapToPositionDescriptor<TChildClass> MapTo<TChildClass>(Expression<Func<TClass, List<TChildClass>>> child, string tag) where TChildClass : class;
    }
}
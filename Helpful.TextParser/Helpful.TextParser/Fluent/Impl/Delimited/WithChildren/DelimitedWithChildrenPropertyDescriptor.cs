using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Helpful.TextParser.Fluent.Interface.Delimited.WithChildren;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl.Delimited.WithChildren
{
    public class DelimitedWithChildrenPropertyDescriptor<TClass> : IDelimitedWithChildrenPropertyDescriptor<TClass>
    {
        private readonly List<DelimitedElement> _elements;

        public DelimitedWithChildrenPropertyDescriptor(List<DelimitedElement> elements)
        {
            _elements = elements;
        }

        public IDelimitedWithChildrenPropertyWithChildrenDescriptor<TProperty> Property<TProperty>(Expression<Func<TClass, TProperty>> property)
        {
            var type = typeof(TClass);

            var member = property.Body as MemberExpression;

            if (member == null)
            {
                throw new ArgumentException($"Expression {property} refers to a method, not a property.");
            }

            var propInfo = member.Member as PropertyInfo;

            if (propInfo == null)
            {
                throw new ArgumentException($"Expression {property} refers to a field, not a property.");
            }

            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
            {
                throw new ArgumentException($"Expresion {property} refers to a property that is not from type {type}.");
            }

            var element = new DelimitedElement()
            {
                Name = propInfo.Name
            };

            _elements.Add(element);

            return new DelimitedWithChildrenPropertyWithChildrenDescriptor<TProperty>(element);
        }
    }
}

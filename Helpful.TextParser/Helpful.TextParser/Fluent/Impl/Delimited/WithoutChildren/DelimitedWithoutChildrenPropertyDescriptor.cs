using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Fluent.Interface.Delimited.WithoutChildren;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl.Delimited.WithoutChildren
{
    public class DelimitedWithoutChildrenPropertyDescriptor<TClass> : IDelimitedWithoutChildrenPropertyDescriptor<TClass>
    {
        private readonly List<DelimitedElement> _elements;

        public DelimitedWithoutChildrenPropertyDescriptor(List<DelimitedElement> elements)
        {
            _elements = elements;
        }

        public IDelimitedWithoutChildrenPropertyPositionDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property)
        {
            Type type = typeof(TClass);

            MemberExpression member = property.Body as MemberExpression;

            if (member == null)
            {
                throw new ArgumentException($"Expression {property.ToString()} refers to a method, not a property.");
            }

            PropertyInfo propInfo = member.Member as PropertyInfo;

            if (propInfo == null)
            {
                throw new ArgumentException($"Expression {property.ToString()} refers to a field, not a property.");
            }

            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
            {
                throw new ArgumentException($"Expresion {property.ToString()} refers to a property that is not from type {type}.");
            }

            var element = new DelimitedElement()
            {
                Name = propInfo.Name
            };

            _elements.Add(element);

            return new DelimitedWithoutChildrenPropertyPositionDescriptor(element);
        }
    }
}

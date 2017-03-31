using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class DelimitedPropertyOnlyDescriptor<TClass> : IDelimitedPropertyOnlyDescriptor<TClass>, IDelimitedPropertyOnlyPositionDescriptor, IDelimitedPropertyOnlyRequiredDescriptor where TClass : class
    {
        private readonly List<Element> _elements;

        public DelimitedPropertyOnlyDescriptor(List<Element> elements)
        {
            _elements = elements;
        }

        public IDelimitedPropertyOnlyPositionDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property)
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

            var element = new Element()
            {
                Name = propInfo.Name
            };

            _elements.Add(element);

            return this;
        }

        public IDelimitedPropertyOnlyRequiredDescriptor Position(int position)
        {
            var element = _elements.Last();

            element.Positions.Add("Position", position);

            return this;
        }

        public void Required()
        {
            var element = _elements.Last();

            element.Required = true;
        }

        public void NotRequired()
        {
            var element = _elements.Last();

            element.Required = false;
        }
    }
}

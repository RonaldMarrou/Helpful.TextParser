using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class PositionedPropertyOnlyDescriptor<TClass> : IPositionedPropertyOnlyDescriptor<TClass>, IPositionedPropertyOnlyPositionDescriptor, IPositionedPropertyOnlyRequiredDescriptor where TClass : class
    {
        private readonly Element _parentElement;

        public PositionedPropertyOnlyDescriptor(Element parentElement)
        {
            _parentElement = parentElement;
        }

        public IPositionedPropertyOnlyPositionDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property)
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

            _parentElement.Elements.Add(element);

            return this;
        }

        public IPositionedPropertyOnlyRequiredDescriptor Position(int startPosition, int endPosition)
        {
            if (startPosition >= endPosition)
            {
                throw new ArgumentException($"Start Position {startPosition} is greather or equal than {endPosition} for {typeof(TClass).FullName}");
            }

            var element = _parentElement.Elements.Last();

            element.ElementType = ElementType.Property;
            element.Positions.Add("StartPosition", startPosition);
            element.Positions.Add("EndPosition", endPosition);

            return this;
        }

        public void Required()
        {
            var element = _parentElement.Elements.Last();

            element.Required = true;
        }

        public void NotRequired()
        {
            var element = _parentElement.Elements.Last();

            element.Required = false;
        }
    }
}

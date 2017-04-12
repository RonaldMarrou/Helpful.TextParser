using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class PositionedPropertyDescriptor<TClass> : IPositionedPropertyDescriptor<TClass>, IPositionedPropertyPositionDescriptor, IPositionedPropertyRequiredDescriptor where TClass : class
    {
        private readonly Element _parentElement;

        public PositionedPropertyDescriptor(Element parentElement)
        {
            _parentElement = parentElement;
        }
        
        public IPositionedPropertyPositionDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property)
        {
            var member = property.Body as MemberExpression;

            var propInfo = member.Member as PropertyInfo;

            var element = new Element()
            {
                Name = propInfo.Name
            };

            _parentElement.Elements.Add(element);

            return this;
        }

        public IPositionedPropertyMapToPositionDescriptor<TChildClass> MapTo<TChildClass>(Expression<Func<TClass, List<TChildClass>>> child, string tag) where TChildClass : class
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException($"Tag cannot be empty for {typeof(TChildClass).FullName}");
            }

            var member = child.Body as MemberExpression;

            var propInfo = member.Member as PropertyInfo;

            var element = new Element
            {
                Name = propInfo.Name,
                LineValueExtractorType = LineValueExtractorType.Positioned,
                ElementType = ElementType.Tag,
                Tag = tag,
                Type = typeof(TChildClass)
            };

            _parentElement.Elements.Add(element);

            return new PositionedPropertyMapToPositionDescriptor<TChildClass>(element);
        }

        public IPositionedPropertyRequiredDescriptor Position(int startPosition, int endPosition)
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

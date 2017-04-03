using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class PositionedPropertyDescriptor<TClass> : IPositionedPropertyDescriptor<TClass>, IPositionedPropertyMapToDescriptor, IPositionedPropertyRequiredDescriptor where TClass : class
    {
        private readonly Element _parentElement;

        public PositionedPropertyDescriptor(Element parentElement)
        {
            _parentElement = parentElement;
        }
        
        public IPositionedPropertyMapToDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property)
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

        public IPositionedPropertyPositionDescriptor<TChildClass> MapTo<TChildClass>(string tag) where TChildClass : class
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentException($"Tag cannot be empty for {typeof(TChildClass).FullName}");
            }

            var element = _parentElement.Elements.Last();

            element.LineValueExtractorType = LineValueExtractorType.Positioned;
            element.ElementType = ElementType.Tag;
            element.Tag = tag;
            element.Type = typeof(TChildClass);

            return new PositionedPropertyPositionDescriptor<TChildClass>(element);
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

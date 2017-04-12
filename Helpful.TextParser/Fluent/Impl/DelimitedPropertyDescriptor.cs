using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class DelimitedPropertyDescriptor<TClass> : IDelimitedPropertyDescriptor<TClass>, IDelimitedPropertyPositionDescriptor, IDelimitedPropertyRequiredDescriptor where TClass : class
    {
        private readonly Element _parentElement;

        public DelimitedPropertyDescriptor(Element parentElement)
        {
            _parentElement = parentElement;
        }
        
        public IDelimitedPropertyPositionDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property)
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

        public IDelimitedPropertyMapToPositionDescriptor<TChildClass> MapTo<TChildClass>(Expression<Func<TClass, List<TChildClass>>> child, string tag) where TChildClass : class
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
                LineValueExtractorType = LineValueExtractorType.DelimitedByString,
                ElementType = ElementType.Tag,
                Tag = tag,
                Type = typeof(TChildClass)
            };

            element.Custom.Add("DelimitationString", _parentElement.Custom["DelimitationString"]);

            _parentElement.Elements.Add(element);

            return new DelimitedPropertyMapToPositionDescriptor<TChildClass>(element);
        }

        public IDelimitedPropertyRequiredDescriptor Position(int position)
        {
            var element = _parentElement.Elements.Last();

            element.ElementType = ElementType.Property;
            element.Positions.Add("Position", position);

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

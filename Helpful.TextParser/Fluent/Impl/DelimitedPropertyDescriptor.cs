using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class DelimitedPropertyDescriptor<TClass> : IDelimitedPropertyDescriptor<TClass>, IDelimitedPropertyMapToDescriptor, IDelimitedPropertyRequiredDescriptor where TClass : class
    {
        private readonly Element _parentElement;

        public DelimitedPropertyDescriptor(Element parentElement)
        {
            _parentElement = parentElement;
        }
        
        public IDelimitedPropertyMapToDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property)
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

        public IDelimitedPropertyPositionDescriptor<TChildClass> MapTo<TChildClass>(string tag) where TChildClass : class
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException($"Tag cannot be empty for {typeof(TChildClass).FullName}");
            }

            var element = _parentElement.Elements.Last();

            element.LineValueExtractorType = LineValueExtractorType.DelimitedByString;
            element.ElementType = ElementType.Tag;
            element.Tag = tag;
            element.Custom.Add("DelimitationString", _parentElement.Custom["DelimitationString"]);
            element.Type = typeof(TChildClass);

            return new DelimitedPropertyPositionDescriptor<TChildClass>(element);
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

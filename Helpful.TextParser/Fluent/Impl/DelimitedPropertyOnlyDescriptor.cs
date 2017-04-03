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
        private readonly Element _parentElement;

        public DelimitedPropertyOnlyDescriptor(Element parentElement)
        {
            _parentElement = parentElement;
        }

        public IDelimitedPropertyOnlyPositionDescriptor Property<TProperty>(Expression<Func<TClass, TProperty>> property)
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

        public IDelimitedPropertyOnlyRequiredDescriptor Position(int position)
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

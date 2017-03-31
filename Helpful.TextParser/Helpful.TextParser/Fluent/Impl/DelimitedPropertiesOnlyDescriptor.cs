using System;
using System.Linq;
using Helpful.TextParser.Fluent.Interface;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Impl
{
    public class DelimitedPropertiesOnlyDescriptor<TClass> : IDelimitedPropertiesOnlyDescriptor<TClass>, IParseDescriptor<TClass> where TClass : class
    {
        private readonly Element _element;
        private readonly IParser _parser;

        private Action<IDelimitedPropertyOnlyDescriptor<TClass>> _action;

        public DelimitedPropertiesOnlyDescriptor(Element element, IParser parser)
        {
            _element = element;
            _parser = parser;
        }

        public IParseDescriptor<TClass> Properties(Action<IDelimitedPropertyOnlyDescriptor<TClass>> properties)
        {
            _action = properties;

            var delimitedPropertyDescriptor = new DelimitedPropertyOnlyDescriptor<TClass>(_element.Elements);

            _action(delimitedPropertyDescriptor);

            return this;
        }

        public Result<TClass> Parse(string[] content)
        {
            return _parser.Parse<TClass>(_element, content);
        }

        public Result<TClass> Parse(Func<string[]> content)
        {
            return _parser.Parse<TClass>(_element, content());
        }
    }
}

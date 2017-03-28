using System;
using System.Linq;
using System.Reflection;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Impl
{
    public class Parser : IParser
    {
        private readonly ILineValueExtractorFactory _lineValueExtractorFactory;
        private readonly IValueSetter _valueSetter;

        public Parser(ILineValueExtractorFactory lineValueExtractorFactory, IValueSetter valueSetter)
        {
            _lineValueExtractorFactory = lineValueExtractorFactory;
            _valueSetter = valueSetter;
        }

        public void Parse<T>(Element element, string[] lines)
        {
            var result = new Result<T>();

            if (element.ElementType == ElementType.PropertyCollection)
            {
                ParseWithoutTag<T>(element, result, lines);
            }
            else
            {
                ParseWithTag<T>(element, result, lines, 0);
            }
        }

        private void ParseWithTag<T>(Element element, Result<T> result , string[] lines, int linePosition)
        {
            var lineValueExtractor = _lineValueExtractorFactory.Get(element.LineValueExtractorType);

            var continueParsing = true;

            for (var i = linePosition; i < lines.Length; i++)
            {
                var tagValue = lineValueExtractor.Extract(lines[i], element);

                if (!tagValue.IsFound || string.IsNullOrEmpty(tagValue.Value))
                {
                    result.Errors.Add($"Line {i} does not contain any valid tag.");

                    return;
                }

                if (!tagValue.Value.Equals(element.Tag))
                {
                    var tagElement = (from childTag in element.Elements.Where(x => x.ElementType == ElementType.Tag)
                                      let childTagValue = lineValueExtractor.Extract(lines[i], childTag)
                                      where childTagValue.IsFound && childTagValue.Value.Equals(childTag.Tag)
                                      select childTag).FirstOrDefault();

                    if (tagElement != null)
                    {
                        ParseWithTag(element, tagElement, result.Content.Last(), result, lines, lineValueExtractor, ref i, ref continueParsing);

                        if (!continueParsing)
                        {
                            return;
                        }

                        continue;
                    }

                    result.Errors.Add($"Line {i} does not contain any valid tag.");

                    return;
                }

                var newObject = (T)Activator.CreateInstance(typeof(T));

                result.Content.Add(newObject);

                var propertyElements = element.Elements.Where(child => child.ElementType == ElementType.Property).ToList();

                foreach (var propertyElement in propertyElements)
                {
                    var propertyValue = lineValueExtractor.Extract(lines[i], propertyElement, element);

                    if (!propertyValue.IsFound || string.IsNullOrEmpty(propertyValue.Value) && propertyElement.Required)
                    {
                        result.Errors.Add($"Property {propertyElement.Name} is missing at Line {i}.");
                    }
                    else
                    {
                        var propertyInfo = typeof(T).GetProperty(propertyElement.Name, BindingFlags.Public | BindingFlags.Instance);

                        var isSet = _valueSetter.Set(propertyInfo, propertyValue.Value, newObject);

                        if (!isSet)
                        {
                            result.Errors.Add($"Value of Property {propertyElement.Name} is not valid at Line {i}.");
                        }
                    }
                }
            }
        }

        private void ParseWithTag<T>(Element parentElement, Element element, object parentInstance, Result<T> result, string[] lines, ILineValueExtractor lineValueExtractor, ref int linePosition, ref bool continueParsing)
        {
            for (var i = linePosition; i < lines.Length; i++)
            {
                linePosition = i;

                var parentElementTagValue = lineValueExtractor.Extract(lines[i], parentElement);

                if (parentElementTagValue.IsFound && parentElementTagValue.Value.Equals(parentElement.Tag))
                {
                    linePosition--;

                    return;
                }

                var tagValue = lineValueExtractor.Extract(lines[i], element);

                if (!tagValue.IsFound || string.IsNullOrEmpty(tagValue.Value))
                {
                    result.Errors.Add($"Line {i} does not contain any valid tag.");

                    continueParsing = false;

                    return;
                }

                if (!tagValue.Value.Equals(element.Tag))
                {
                    var tagElement = (from childTag in element.Elements.Where(x => x.ElementType == ElementType.Tag)
                                      let childTagValue = lineValueExtractor.Extract(lines[i], childTag)
                                      where childTagValue.IsFound && childTagValue.Value.Equals(childTag.Tag)
                                      select childTag).FirstOrDefault();

                    if (tagElement != null)
                    {
                        ParseWithTag(element, tagElement, result.Content.Last(), result, lines, lineValueExtractor, ref linePosition, ref continueParsing);
                    }

                    result.Errors.Add($"Line {i} does not contain any valid tag.");

                    continueParsing = false;

                    return;
                }

                var newObject = Activator.CreateInstance(element.Type);

                var listPropertyInfo = parentInstance.GetType().GetProperty(element.Name);

                if (listPropertyInfo.GetValue(parentInstance) == null)
                {
                    listPropertyInfo.SetValue(parentInstance, Activator.CreateInstance(listPropertyInfo.PropertyType));
                }

                listPropertyInfo.PropertyType.GetMethod("Add").Invoke(listPropertyInfo.GetValue(parentInstance), new[] { newObject });

                var propertyElements = element.Elements.Where(child => child.ElementType == ElementType.Property).ToList();

                foreach (var propertyElement in propertyElements)
                {
                    var propertyValue = lineValueExtractor.Extract(lines[i], propertyElement, element);

                    if (!propertyValue.IsFound || string.IsNullOrEmpty(propertyValue.Value) && propertyElement.Required)
                    {
                        result.Errors.Add($"Property {propertyElement.Name} is missing at Line {i}.");
                    }

                    var propertyInfo = element.Type.GetProperty(propertyElement.Name, BindingFlags.Public | BindingFlags.Instance);

                    var isSet = _valueSetter.Set(propertyInfo, propertyValue.Value, newObject);

                    if (!isSet)
                    {
                        result.Errors.Add($"Value of Property {propertyElement.Name} is not valid at Line {i}.");
                    }
                }
            }
        }

        private void ParseWithoutTag<T>(Element element, Result<T> result , string[] lines)
        {
            var lineValueExtractor = _lineValueExtractorFactory.Get(element.LineValueExtractorType);

            for (var i = 0; i < lines.Length; i++)
            {
                var newObject = (T)Activator.CreateInstance(typeof(T));

                result.Content.Add(newObject);

                var propertyElements = element.Elements.Where(child => child.ElementType == ElementType.Property).ToList();

                foreach (var propertyElement in propertyElements)
                {
                    var propertyValue = lineValueExtractor.Extract(lines[i], propertyElement, element);

                    if (!propertyValue.IsFound || string.IsNullOrEmpty(propertyValue.Value) && propertyElement.Required)
                    {
                        result.Errors.Add($"Property {propertyElement.Name} is missing at Line {i}.");
                    }
                    else
                    {
                        var propertyInfo = typeof(T).GetProperty(propertyElement.Name, BindingFlags.Public | BindingFlags.Instance);

                        var isSet = _valueSetter.Set(propertyInfo, propertyValue.Value, newObject);

                        if (!isSet)
                        {
                            result.Errors.Add($"Value of Property {propertyElement.Name} is not valid at Line {i}.");
                        }
                    }
                }
            }
        }
    }
}

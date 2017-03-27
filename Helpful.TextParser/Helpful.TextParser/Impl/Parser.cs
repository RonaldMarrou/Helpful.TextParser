using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Impl
{
    public class Parser : IParser
    {
        private readonly ILineValueExtractorFactory _lineValueExtractorFactory;

        public Parser(ILineValueExtractorFactory lineValueExtractorFactory)
        {
            _lineValueExtractorFactory = lineValueExtractorFactory;
        }

        public void Parse<T>(Element element, string[] lines)
        {
            var result = new Result<T>();

            if (string.IsNullOrEmpty(element.Tag))
            {
                ParseWithoutTag(element, lines);
            }
            else
            {
                ParseWithTag<T>(element, result, lines, 0);
            }
        }

        private void ParseWithTag<T>(Element element, Result<T> result , string[] lines, int linePosition)
        {
            var lineValueExtractor = _lineValueExtractorFactory.Get(element.LineValueExtractorType);

            for (var i = linePosition; i < lines.Length; i++)
            {
                var tagValue = lineValueExtractor.Extract(lines[i], element);

                if (!tagValue.IsFound || string.IsNullOrEmpty(tagValue.Value))
                {
                    //TODO: Returns Error: Tag not found
                }

                if (!tagValue.Value.Equals(element.Tag))
                {
                    var tagElement = (from childTag in element.Elements.Where(x => x.ElementType == ElementType.Tag)
                                      let childTagValue = lineValueExtractor.Extract(lines[i], childTag)
                                      where childTagValue.IsFound && childTagValue.Value.Equals(childTag.Tag)
                                      select childTag).FirstOrDefault();

                    if (tagElement != null)
                    {
                        var propertyInfo = typeof(T).GetProperty(tagElement.Name);

                        if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            ParseChildrenWithTag(element, tagElement, result.Content.Last(), result, lines, lineValueExtractor, ref i);

                            continue;
                        }

                        result.Errors.Add($"Property {tagElement.Name} is not typed as List.");

                        return;
                    }
                    else
                    {
                        //TODO: Returns Error: Tag is not List
                    }
                }

                var newObject = (T)Activator.CreateInstance(typeof(T));

                result.Content.Add(newObject);

                var propertyElements = element.Elements.Where(child => child.ElementType == ElementType.Property).ToList();

                foreach (var propertyElement in propertyElements)
                {
                    var propertyValue = lineValueExtractor.Extract(lines[i], propertyElement, element);

                    if (!propertyValue.IsFound || string.IsNullOrEmpty(propertyValue.Value) && propertyElement.Required)
                    {
                        //TODO: Returns Error: Required Property Not Found
                    }

                    var propertyInfo = typeof(T).GetProperty(propertyElement.Name, BindingFlags.Public | BindingFlags.Instance);

                    propertyInfo.SetValue(newObject, Convert.ChangeType(propertyValue.Value, propertyInfo.PropertyType));
                }
            }
        }

        private void ParseChildrenWithTag<T>(Element parentElement, Element element, object parentInstance, Result<T> result, string[] lines, ILineValueExtractor lineValueExtractor, ref int linePosition)
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
                    //TODO: Returns Error: Tag not found
                }

                if (!tagValue.Value.Equals(element.Tag))
                {
                    var tagElement = (from childTag in element.Elements.Where(x => x.ElementType == ElementType.Tag)
                                      let childTagValue = lineValueExtractor.Extract(lines[i], childTag)
                                      where childTagValue.IsFound && childTagValue.Value.Equals(childTag.Tag)
                                      select childTag).FirstOrDefault();

                    if (tagElement != null)
                    {
                        var propertyInfo = typeof(T).GetProperty(tagElement.Name);

                        if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))
                        {
                            ParseChildrenWithTag(element, tagElement, result.Content.Last(), result, lines, lineValueExtractor, ref linePosition);
                        }
                        else
                        {
                            //TODO: Returns Error: Tag is not List
                        }
                    }
                    else
                    {
                        //TODO: Returns Error: Tag is not List
                    }
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
                        //TODO: Returns Error: Required Property Not Found
                    }

                    var propertyInfo = element.Type.GetProperty(propertyElement.Name, BindingFlags.Public | BindingFlags.Instance);

                    propertyInfo.SetValue(newObject, Convert.ChangeType(propertyValue.Value, propertyInfo.PropertyType));
                }
            }
        }

        private void ParseWithoutTag(Element element, string[] lines)
        {
            foreach (var child in element.Elements)
            {

            }
        }
    }
}

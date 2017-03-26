using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Impl
{
    public class DelimitedParser : IParser
    {
        public void Parse<T>(DelimitedElement element, string[] lines)
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

        private void ParseWithTag<T>(DelimitedElement element, Result<T> result , string[] lines, int linePosition)
        {
            for (var i = linePosition; i < lines.Length; i++)
            {
                var lineParts = lines[i].Split(new[] {element.DelimitationCharacter}, StringSplitOptions.None);

                if (element.Position >= lineParts.Length || string.IsNullOrEmpty(lineParts[element.Position]))
                {
                    //TODO: Returns Error: Tag not found
                }

                if (!lineParts[element.Position].Equals(element.Tag))
                {
                    var tags = element.Children.Where(child => child.ElementType == ElementType.Tag).ToList();

                    if (tags.Any(tag => tag.Tag.Equals(lineParts[element.Position])))
                    {
                        var tag = tags.First(x => x.Tag.Equals(lineParts[element.Position]));

                        var propertyInfo = typeof(T).GetProperty(tag.Name);

                        if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            ParseChildrenWithTag(element, tag, result.Content.Last(), result, lines, ref i);

                            continue;
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

                var newObject = (T)Activator.CreateInstance(typeof(T));

                result.Content.Add(newObject);

                var properties = element.Children.Where(child => child.ElementType == ElementType.Property).ToList();

                foreach (var property in properties)
                {
                    if ((property.Position >= lineParts.Length || string.IsNullOrEmpty(lineParts[element.Position])) && property.Required)
                    {
                        //TODO: Returns Error: Required Property Not Found
                    }

                    var propertyInfo = typeof(T).GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);

                    propertyInfo.SetValue(newObject, Convert.ChangeType(lineParts[property.Position], propertyInfo.PropertyType));
                }
            }
        }

        private void ParseChildrenWithTag<T>(DelimitedElement parentElement, DelimitedElement element, object parentInstance, Result<T> result, string[] lines, ref int linePosition)
        {
            for (var i = linePosition; i < lines.Length; i++)
            {
                linePosition = i;

                var linePartsParentElement = lines[i].Split(new[] { parentElement.DelimitationCharacter }, StringSplitOptions.None);

                if (parentElement.Position < linePartsParentElement.Length && !string.IsNullOrEmpty(linePartsParentElement[parentElement.Position]) && linePartsParentElement[parentElement.Position].Equals(parentElement.Tag))
                {
                    linePosition--;

                    return;
                }

                var lineParts = lines[i].Split(new[] { element.DelimitationCharacter }, StringSplitOptions.None);

                if (element.Position >= lineParts.Length || string.IsNullOrEmpty(lineParts[element.Position]))
                {
                    //TODO: Returns Error: Tag not found
                }

                if (!lineParts[element.Position].Equals(element.Tag))
                {
                    var tags = element.Children.Where(child => child.ElementType == ElementType.Property).ToList();

                    if (tags.Any(tag => tag.Tag.Equals(lineParts[element.Position])))
                    {
                        var tag = tags.First(x => x.Tag.Equals(lineParts[element.Position]));

                        var propertyInfo = typeof(T).GetProperty(tag.Name);

                        if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))
                        {
                            ParseChildrenWithTag(element, tag, result.Content.Last(), result, lines, ref linePosition);
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

                listPropertyInfo.PropertyType.GetMethod("Add").Invoke(listPropertyInfo.GetValue(parentInstance), new[] { (object)newObject });

                var properties = element.Children.Where(child => child.ElementType == ElementType.Property).ToList();

                foreach (var property in properties)
                {
                    if ((property.Position >= lineParts.Length || string.IsNullOrEmpty(lineParts[element.Position])) && property.Required)
                    {
                        //TODO: Returns Error: Required Property Not Found
                    }

                    var propertyInfo = element.Type.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);

                    propertyInfo.SetValue(newObject, Convert.ChangeType(lineParts[property.Position], propertyInfo.PropertyType));
                }
            }
        }

        private void ParseWithoutTag(DelimitedElement element, string[] lines)
        {
            foreach (var child in element.Children)
            {

            }
        }
    }
}

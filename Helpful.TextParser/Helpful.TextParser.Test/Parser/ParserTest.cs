using System;
using System.Collections.Generic;
using Helpful.TextParser.Impl;
using Helpful.TextParser.Impl.LineValueExtractor;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;
using NUnit.Framework;

namespace Helpful.TextParser.Test.Parser
{
    public class ParserTest
    {
        [Test]
        public void Parse_WithTagAndOnlyHeader_ReturnsResult()
        {
            var lineValueExtractorFactory = new LineValueExtractorFactory(new ILineValueExtractor[] {new DelimitedLineValueExtractor()});

            var sut = new Impl.Parser(lineValueExtractorFactory);

            var delimitedElement = new DelimitedElement()
            {
                Custom = new Dictionary<string, string>()
                {
                    { "DelimitationString", "," }
                },
                Tag = "HEADER",
                LineValueExtractorType = LineValueExtractorType.DelimitedByString,
                ElementType = ElementType.Tag,
                Positions = new Dictionary<string, int>()
                        {
                            { "Position", 0 }
                        },
                Children = new List<DelimitedElement>()
                {
                    new DelimitedElement()
                    {
                        Name = "Code",
                        ElementType = ElementType.Property,
                        Positions = new Dictionary<string, int>()
                        {
                            { "Position", 1 }
                        }
                    },
                    new DelimitedElement()
                    {
                        Name = "FirstName",
                        ElementType = ElementType.Property,
                        Positions = new Dictionary<string, int>()
                        {
                            { "Position", 2 }
                        }
                    },
                    new DelimitedElement()
                    {
                        Name = "LastName",
                        ElementType = ElementType.Property,
                        Positions = new Dictionary<string, int>()
                        {
                            { "Position", 3 }
                        }
                    },
                    new DelimitedElement()
                    {
                        Name = "City",
                        ElementType = ElementType.Property,
                        Positions = new Dictionary<string, int>()
                        {
                            { "Position", 4 }
                        }
                    },
                    new DelimitedElement()
                    {
                        Name = "Country",
                        ElementType = ElementType.Property,
                        Positions = new Dictionary<string, int>()
                        {
                            { "Position", 5 }
                        }
                    },
                    new DelimitedElement()
                    {
                        Name = "Age",
                        ElementType = ElementType.Property,
                        Positions = new Dictionary<string, int>()
                        {
                            { "Position", 6 }
                        }
                    },
                    new DelimitedElement()
                    {
                        Name = "DateOfBirth",
                        ElementType = ElementType.Property,
                        Positions = new Dictionary<string, int>()
                        {
                            { "Position", 7 }
                        }
                    },
                    new DelimitedElement()
                    {
                        Name = "Income",
                        ElementType = ElementType.Property,
                        Positions = new Dictionary<string, int>()
                        {
                            { "Position", 8 }
                        }
                    }
                }
            };

            var lines = ParserTestResources.DelimitedParserOnlyOneLevel.Split(new[] {"\n"}, StringSplitOptions.None);

            sut.Parse<Person>(delimitedElement, lines);
        }

        [Test]
        public void Parse_WithTagAndChildren_ReturnsResult()
        {
            var lineValueExtractorFactory = new LineValueExtractorFactory(new ILineValueExtractor[] { new DelimitedLineValueExtractor() });

            var sut = new Impl.Parser(lineValueExtractorFactory);

            var delimitedElement = new Element()
            {
                Custom = new Dictionary<string, string>()
                {
                    { "DelimitationString", "," }
                },
                Tag = "HEADER",
                LineValueExtractorType = LineValueExtractorType.DelimitedByString,
                ElementType = ElementType.Tag,
                Positions = new Dictionary<string, int>()
                        {
                            { "Position", 0 }
                        },
                Elements = new List<Element>()
                {
                    new Element()
                    {
                        Name = "Code",
                        ElementType = ElementType.Property,
                        Positions = new Dictionary<string, int>()
                        {
                            { "Position", 1 }
                        }
                    },
                    new Element()
                    {
                        Name = "Supplier",
                        ElementType = ElementType.Property,
                        Positions = new Dictionary<string, int>()
                        {
                            { "Position", 2 }
                        }
                    },
                    new Element()
                    {
                        Name = "IssueDate",
                        ElementType = ElementType.Property,
                        Positions = new Dictionary<string, int>()
                        {
                            { "Position", 3 }
                        }
                    },
                    new Element()
                    {
                        Custom = new Dictionary<string, string>()
                        {
                            { "DelimitationString", "," }
                        },
                        Name = "Details",
                        Tag = "DETAIL",
                        ElementType = ElementType.Tag,
                        LineValueExtractorType = LineValueExtractorType.DelimitedByString,
                        Positions = new Dictionary<string, int>()
                        {
                            { "Position", 0 }
                        },
                        Type = typeof(PurchaseOrderDetail),
                        Elements = new List<Element>()
                        {
                            new Element()
                            {
                                Name = "Code",
                                ElementType = ElementType.Property,
                                Positions = new Dictionary<string, int>()
                                {
                                    { "Position", 1 }
                                }
                            },
                            new Element()
                            {
                                Name = "Description",
                                ElementType = ElementType.Property,
                                Positions = new Dictionary<string, int>()
                                {
                                    { "Position", 2 }
                                }
                            },
                            new Element()
                            {
                                Name = "Quantity",
                                ElementType = ElementType.Property,
                                Positions = new Dictionary<string, int>()
                                {
                                    { "Position", 3 }
                                }
                            },
                            new Element()
                            {
                                Name = "Price",
                                ElementType = ElementType.Property,
                                Positions = new Dictionary<string, int>()
                                {
                                    { "Position", 4 }
                                }
                            },
                            new Element()
                            {
                                Name = "Currency",
                                ElementType = ElementType.Property,
                                Positions = new Dictionary<string, int>()
                                {
                                    { "Position", 5 }
                                }
                            }
                        }
                    }
                }
            };

            var lines = ParserTestResources.DelimitedParserWithChildren.Split(new[] { "\n" }, StringSplitOptions.None);

            sut.Parse<PurchaseOrder>(delimitedElement, lines);
        }
    }
}

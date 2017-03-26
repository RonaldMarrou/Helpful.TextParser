using System;
using System.Collections.Generic;
using Helpful.TextParser.Impl;
using Helpful.TextParser.Model;
using NUnit.Framework;

namespace Helpful.TextParser.Test.Parser
{
    public class DelimitedParserTest
    {
        [Test]
        public void Parse_WithTagAndOnlyHeader_ReturnsResult()
        {
            var sut = new DelimitedParser();

            var delimitedElement = new DelimitedElement()
            {
                DelimitationCharacter = ",",
                Tag = "HEADER",
                ElementType = ElementType.Tag,
                Position = 0,
                Children = new List<DelimitedElement>()
                {
                    new DelimitedElement()
                    {
                        Name = "Code",
                        ElementType = ElementType.Property,
                        Position = 1
                    },
                    new DelimitedElement()
                    {
                        Name = "FirstName",
                        ElementType = ElementType.Property,
                        Position = 2
                    },
                    new DelimitedElement()
                    {
                        Name = "LastName",
                        ElementType = ElementType.Property,
                        Position = 3
                    },
                    new DelimitedElement()
                    {
                        Name = "City",
                        ElementType = ElementType.Property,
                        Position = 4
                    },
                    new DelimitedElement()
                    {
                        Name = "Country",
                        ElementType = ElementType.Property,
                        Position = 5
                    },
                    new DelimitedElement()
                    {
                        Name = "Age",
                        ElementType = ElementType.Property,
                        Position = 6
                    },
                    new DelimitedElement()
                    {
                        Name = "DateOfBirth",
                        ElementType = ElementType.Property,
                        Position = 7
                    },
                    new DelimitedElement()
                    {
                        Name = "Income",
                        ElementType = ElementType.Property,
                        Position = 8
                    }
                }
            };

            var lines = ParserTestResources.DelimitedParserOnlyOneLevel.Split(new[] {"\n"}, StringSplitOptions.None);

            sut.Parse<Person>(delimitedElement, lines);
        }

        [Test]
        public void Parse_WithTagAndChildren_ReturnsResult()
        {
            var sut = new DelimitedParser();

            var delimitedElement = new DelimitedElement()
            {
                DelimitationCharacter = ",",
                Tag = "HEADER",
                ElementType = ElementType.Tag,
                Position = 0,
                Children = new List<DelimitedElement>()
                {
                    new DelimitedElement()
                    {
                        Name = "Code",
                        ElementType = ElementType.Property,
                        Position = 1
                    },
                    new DelimitedElement()
                    {
                        Name = "Supplier",
                        ElementType = ElementType.Property,
                        Position = 2
                    },
                    new DelimitedElement()
                    {
                        Name = "IssueDate",
                        ElementType = ElementType.Property,
                        Position = 3
                    },
                    new DelimitedElement()
                    {
                        DelimitationCharacter = ",",
                        Name = "Details",
                        Tag = "DETAIL",
                        ElementType = ElementType.Tag,
                        Position = 0,
                        Type = typeof(PurchaseOrderDetail),
                        Children = new List<DelimitedElement>()
                        {
                            new DelimitedElement()
                            {
                                Name = "Code",
                                ElementType = ElementType.Property,
                                Position = 1
                            },
                            new DelimitedElement()
                            {
                                Name = "Description",
                                ElementType = ElementType.Property,
                                Position = 2
                            },
                            new DelimitedElement()
                            {
                                Name = "Quantity",
                                ElementType = ElementType.Property,
                                Position = 3
                            },
                            new DelimitedElement()
                            {
                                Name = "Price",
                                ElementType = ElementType.Property,
                                Position = 4
                            },
                            new DelimitedElement()
                            {
                                Name = "Currency",
                                ElementType = ElementType.Property,
                                Position = 5
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

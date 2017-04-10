using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Helpful.TextParser.Fluent.Impl;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Helpful.TextParser.Test.Fluent
{
    public class DelimitedDescriptorTest
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void DelimitedDescriptor_TagIsEmpty_ReturnSexception(string tag)
        {
            var sut = new TestDelimitedDescriptor(It.IsAny<string>(), It.IsAny<IParser>());

            Should.Throw<ArgumentNullException>(() => sut.MapTo<DelimitedFooClass>(tag));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void DelimitedDescriptor_PropertyTagIsEmpty_ReturnSexception(string tag)
        {
            var sut = new TestDelimitedDescriptor(It.IsAny<string>(), It.IsAny<IParser>());

            Should.Throw<ArgumentNullException>(() => sut.MapTo<DelimitedFooClass>("NOTEMPTY").Position(0).Properties(property => property.Property(x => x.FooProperty1).MapTo<DelimitedChildFooClass>(tag)));
        }

        [Test]
        [TestCase("FOO1", "FOO1")]
        [TestCase("FOO2", "FOO2")]
        [TestCase("FOO3", "FOO3")]
        public void DelimitedDescriptor_Constructor_InitializeComponentCorrectly(string delimitationString, string expectedValue)
        {
            var sut = new TestDelimitedDescriptor(delimitationString, It.IsAny<IParser>());

           sut.Element.Custom["DelimitationString"].ShouldBe(expectedValue);
           sut.Element.LineValueExtractorType.ShouldBe(LineValueExtractorType.DelimitedByString);
        }

        [Test]
        public void DelimitedDescriptor_WithoutTagProperties_RightQuantityOfProperties()
        {
            var sut = new TestDelimitedDescriptor(It.IsAny<string>(), It.IsAny<IParser>());

            sut.MapTo<DelimitedFooClass>();

            sut.Element.ElementType.ShouldBe(ElementType.PropertyCollection);
            sut.Element.Type.ShouldBe(typeof(DelimitedFooClass));
        }

        [Test]
        public void DelimitedDescriptor_MapToWithoutTag_ElementIsPropertyCollection()
        {
            var sut = new TestDelimitedDescriptor(It.IsAny<string>(), It.IsAny<IParser>());

            sut.MapTo<DelimitedFooClass>().Properties(
                properties =>
                {
                    properties.Property(x => x.FooProperty1).Position(0).Required();
                    properties.Property(x => x.FooProperty2).Position(1).NotRequired();
                }); ;

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").Positions["Position"].ShouldBe(0);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").Positions["Position"].ShouldBe(1);

            sut.Element.Elements.Count(x => x.ElementType == ElementType.Property).ShouldBe(sut.Element.Elements.Count);
        }

        [Test]
        [TestCase("FOO1", "FOO1")]
        [TestCase("FOO2", "FOO2")]
        [TestCase("FOO3", "FOO3")]
        public void DelimitedDescriptor_MapToWithTag_HasRightTag(string tag, string expectedResult)
        {
            var sut = new TestDelimitedDescriptor(It.IsAny<string>(), It.IsAny<IParser>());

            sut.MapTo<DelimitedFooClass>(tag);

            sut.Element.ElementType.ShouldBe(ElementType.Tag);
            sut.Element.Tag.ShouldBe(expectedResult);
            sut.Element.Type.ShouldBe(typeof(DelimitedFooClass));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void DelimitedDescriptor_TagIsEmpty_ThrowsArgumentException(string tag)
        {
            var sut = new TestDelimitedDescriptor(It.IsAny<string>(), It.IsAny<IParser>());

            Should.Throw<ArgumentException>(() =>  sut.MapTo<DelimitedFooClass>(tag));
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        public void DelimitedDescriptor_Position_HasRightPosition(int position, int expectedValue)
        {
            var sut = new TestDelimitedDescriptor(It.IsAny<string>(), It.IsAny<IParser>());

            sut.MapTo<DelimitedFooClass>("FOO").Position(position);

            sut.Element.Positions["Position"].ShouldBe(expectedValue);
        }

        [Test]
        public void DelimitedDescriptor_WithTagAddProperties_AllTagsHaveParentDelimitationString()
        {
            var sut = new TestDelimitedDescriptor("FOODELIMITATIONSTRING", It.IsAny<IParser>());

            sut.MapTo<DelimitedFooClass>("FOOTAG").Position(0).Properties(
                properties =>
                {
                    properties.Property(x => x.FooProperty1).Position(1).Required();
                    properties.Property(x => x.FooProperty2).Position(2).NotRequired();
                    properties.Property(x => x.FooProperty3).MapTo<DelimitedChildFooClass>("FOODETAILTAG1").Position(0);
                    properties.Property(x => x.FooProperty4).MapTo<DelimitedChildFooClass>("FOODETAILTAG2").Position(0).Properties(
                        childProperties =>
                        {
                            childProperties.Property(x => x.FooProperty5).Position(1).Required();
                            childProperties.Property(x => x.FooProperty6).Position(2).NotRequired();

                            childProperties.Property(x => x.FooProperty7).MapTo<DelimitedGrandChildFooClass>("FOOSUBDETAILTAG1").Position(0).Properties(
                                grandChildProperties =>
                                {
                                    grandChildProperties.Property(x => x.FooProperty8).Position(1).NotRequired();
                                });
                        });
                });

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").Positions["Position"].ShouldBe(1);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").Positions["Position"].ShouldBe(2);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty3").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty3").ElementType.ShouldBe(ElementType.Tag);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty3").Positions["Position"].ShouldBe(0);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty3").Custom["DelimitationString"].ShouldBe("FOODELIMITATIONSTRING");
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty3").Tag.ShouldBe("FOODETAILTAG1");

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4").ElementType.ShouldBe(ElementType.Tag);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4").Positions["Position"].ShouldBe(0);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4").Custom["DelimitationString"].ShouldBe("FOODELIMITATIONSTRING");
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4").Tag.ShouldBe("FOODETAILTAG2");
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4").Elements.Count.ShouldBe(3);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                .Elements.FirstOrDefault(x => x.Name == "FooProperty5").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                .Elements.FirstOrDefault(x => x.Name == "FooProperty5").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty5").Positions["Position"].ShouldBe(1);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                .Elements.FirstOrDefault(x => x.Name == "FooProperty6").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                .Elements.FirstOrDefault(x => x.Name == "FooProperty6").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty6").Positions["Position"].ShouldBe(2);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty7").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty7").ElementType.ShouldBe(ElementType.Tag);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty7").Positions["Position"].ShouldBe(0);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty7").Custom["DelimitationString"].ShouldBe("FOODELIMITATIONSTRING");
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty7").Tag.ShouldBe("FOOSUBDETAILTAG1");
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty7").Elements.Count.ShouldBe(1);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty7")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty8").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty7")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty8").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty7")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty8").Positions["Position"].ShouldBe(1);
        }
    }

    public class DelimitedFooClass
    {
        public DelimitedFooClass()
        {
            FooProperty1 = false;
            FooProperty2 = 0;
            FooProperty3 = null;
            FooProperty4 = null;
        }

        public bool FooProperty1 { get; set; }

        public decimal FooProperty2 { get; set; }

        public List<DelimitedChildFooClass> FooProperty3 { get; set; }

        public List<DelimitedChildFooClass> FooProperty4 { get; set; }
    }

    public class DelimitedChildFooClass
    {
        public DelimitedChildFooClass()
        {
            FooProperty5 = null;
            FooProperty6 = null;
            FooProperty7 = null;
        }

        public string FooProperty5 { get; set; }

        public string FooProperty6 { get; set; }

        public List<DelimitedGrandChildFooClass> FooProperty7 { get; set; }
    }

    public class DelimitedGrandChildFooClass
    {
        public DelimitedGrandChildFooClass()
        {
            FooProperty8 = null;
        }

        public string FooProperty8 { get; set; }
    }

    public class TestDelimitedDescriptor : DelimitedDescriptor
    {
        public TestDelimitedDescriptor(string delimitationString, IParser parser) : base(delimitationString, parser)
        {
        }

        public Element Element => _element;
    }
}

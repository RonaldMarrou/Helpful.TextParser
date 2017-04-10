using System;
using System.Linq;
using Helpful.TextParser.Fluent.Impl;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;
using Helpful.TextParser.Test.Dummy;
using Helpful.TextParser.Test.Parser;
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

            Should.Throw<ArgumentNullException>(() => sut.MapTo<DummyFooClass1>(tag));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void DelimitedDescriptor_PropertyTagIsEmpty_ReturnSexception(string tag)
        {
            var sut = new TestDelimitedDescriptor(It.IsAny<string>(), It.IsAny<IParser>());

            Should.Throw<ArgumentNullException>(() => sut.MapTo<DummyFooClass1>("NOTEMPTY").Position(0).Properties(property => property.Property(x => x.Property1).MapTo<DummyFooClass1>(tag)));
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

            sut.MapTo<DummyFooClass1>();

            sut.Element.ElementType.ShouldBe(ElementType.PropertyCollection);
            sut.Element.Type.ShouldBe(typeof(DummyFooClass1));
        }

        [Test]
        public void DelimitedDescriptor_MapToWithoutTag_ElementIsPropertyCollection()
        {
            var sut = new TestDelimitedDescriptor(It.IsAny<string>(), It.IsAny<IParser>());

            sut.MapTo<DummyFooClass1>().Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(0).Required();
                    properties.Property(x => x.Property2).Position(1).NotRequired();
                }); ;

            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").Positions["Position"].ShouldBe(0);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").Positions["Position"].ShouldBe(1);

            sut.Element.Elements.Count(x => x.ElementType == ElementType.Property).ShouldBe(sut.Element.Elements.Count);
        }

        [Test]
        [TestCase("FOO1", "FOO1")]
        [TestCase("FOO2", "FOO2")]
        [TestCase("FOO3", "FOO3")]
        public void DelimitedDescriptor_MapToWithTag_HasRightTag(string tag, string expectedResult)
        {
            var sut = new TestDelimitedDescriptor(It.IsAny<string>(), It.IsAny<IParser>());

            sut.MapTo<DummyFooClass1>(tag);

            sut.Element.ElementType.ShouldBe(ElementType.Tag);
            sut.Element.Tag.ShouldBe(expectedResult);
            sut.Element.Type.ShouldBe(typeof(DummyFooClass1));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void DelimitedDescriptor_TagIsEmpty_ThrowsArgumentException(string tag)
        {
            var sut = new TestDelimitedDescriptor(It.IsAny<string>(), It.IsAny<IParser>());

            Should.Throw<ArgumentException>(() =>  sut.MapTo<DummyFooClass1>(tag));
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        public void DelimitedDescriptor_Position_HasRightPosition(int position, int expectedValue)
        {
            var sut = new TestDelimitedDescriptor(It.IsAny<string>(), It.IsAny<IParser>());

            sut.MapTo<DummyFooClass1>("FOO").Position(position);

            sut.Element.Positions["Position"].ShouldBe(expectedValue);
        }

        [Test]
        public void DelimitedDescriptor_WithTagAddProperties_AllTagsHaveParentDelimitationString()
        {
            var sut = new TestDelimitedDescriptor("FOODELIMITATIONSTRING", It.IsAny<IParser>());

            sut.MapTo<DummyFooClass1>("FOOTAG").Position(0).Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(1).Required();
                    properties.Property(x => x.Property2).Position(2).NotRequired();
                    properties.Property(x => x.Property7).MapTo<DummyFooClass2>("FOODETAILTAG2").Position(0).Properties(
                        childProperties =>
                        {
                            childProperties.Property(x => x.Property1).Position(1).Required();
                            childProperties.Property(x => x.Property2).Position(2).NotRequired();

                            childProperties.Property(x => x.Property7).MapTo<DummyFooClass3>("FOOSUBDETAILTAG1").Position(0).Properties(
                                grandChildProperties =>
                                {
                                    grandChildProperties.Property(x => x.Property1).Position(1).NotRequired();
                                });
                        });
                });

            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").Positions["Position"].ShouldBe(1);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").Positions["Position"].ShouldBe(2);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7").ElementType.ShouldBe(ElementType.Tag);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7").Positions["Position"].ShouldBe(0);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7").Custom["DelimitationString"].ShouldBe("FOODELIMITATIONSTRING");
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7").Tag.ShouldBe("FOODETAILTAG2");
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7").Elements.Count.ShouldBe(3);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                .Elements.FirstOrDefault(x => x.Name == "Property1").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                .Elements.FirstOrDefault(x => x.Name == "Property1").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property1").Positions["Position"].ShouldBe(1);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                .Elements.FirstOrDefault(x => x.Name == "Property2").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                .Elements.FirstOrDefault(x => x.Name == "Property2").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property2").Positions["Position"].ShouldBe(2);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property7").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property7").ElementType.ShouldBe(ElementType.Tag);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property7").Positions["Position"].ShouldBe(0);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property7").Custom["DelimitationString"].ShouldBe("FOODELIMITATIONSTRING");
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property7").Tag.ShouldBe("FOOSUBDETAILTAG1");
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property7").Elements.Count.ShouldBe(1);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property1").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property1").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property1").Positions["Position"].ShouldBe(1);
        }
    }

    public class TestDelimitedDescriptor : DelimitedDescriptor
    {
        public TestDelimitedDescriptor(string delimitationString, IParser parser) : base(delimitationString, parser)
        {
        }

        public Element Element => _element;
    }
}

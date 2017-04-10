using System;
using System.Collections.Generic;
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
    public class PositionedDescriptorTest
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void PositioneddDescriptor_TagIsEmpty_ReturnsException(string tag)
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            Should.Throw<ArgumentNullException>(() => sut.MapTo<DummyFooClass1>(tag));
        }

        [Test]
        [TestCase(2, 1)]
        [TestCase(3, 3)]
        [TestCase(25, 4)]
        public void PositioneddDescriptor_PropertyPosition_ReturnsException(int startPosition, int endPosition)
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            Should.Throw<ArgumentException>(() => sut.MapTo<DummyFooClass1>().Properties(property => property.Property(x => x.Property1).Position(startPosition, endPosition)));
        }

        [Test]
        [TestCase(2, 1)]
        [TestCase(3, 3)]
        [TestCase(25, 4)]
        public void PositioneddDescriptor_NotValidTagPosition_ReturnsException(int startPosition, int endPosition)
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            Should.Throw<ArgumentException>(() => sut.MapTo<DummyFooClass1>("NOTEMPTY").Position(startPosition, endPosition));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void PositionedDescriptor_PropertyTagIsEmpty_ReturnsException(string tag)
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            Should.Throw<ArgumentNullException>(() => sut.MapTo<DummyFooClass1>("NOTEMPTY").Position(0, 1).Properties(property => property.Property(x => x.Property1).MapTo<DummyFooClass2>(tag)));
        }

        [Test]
        [TestCase(2, 1)]
        [TestCase(3, 3)]
        [TestCase(25, 4)]
        public void PositioneddDescriptor_NotValidPropertyTagPosition_ReturnsException(int startPosition, int endPosition)
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            Should.Throw<ArgumentException>(() => sut.MapTo<DummyFooClass1>("NOTEMPTY").Position(0, 1).Properties(property => property.Property(x => x.Property1).MapTo<DummyFooClass2>("NOTEMPTY").Position(startPosition, endPosition)));
        }

        [Test]
        [TestCase(2, 1)]
        [TestCase(3, 3)]
        [TestCase(25, 4)]
        public void PositioneddDescriptor_TagPropertyPosition_ReturnsException(int startPosition, int endPosition)
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            Should.Throw<ArgumentException>(() => sut.MapTo<DummyFooClass1>("NOTEMPTY").Position(0, 1).Properties(property => property.Property(x => x.Property1).Position(startPosition, endPosition)));
        }

        [Test]
        public void PositionedDescriptor_Constructor_InitializeComponentCorrectly()
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            sut.Element.LineValueExtractorType.ShouldBe(LineValueExtractorType.Positioned);
        }

        [Test]
        public void PositionedDescriptor_WithoutTagProperties_RightQuantityOfProperties()
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            sut.MapTo<DummyFooClass1>();

            sut.Element.ElementType.ShouldBe(ElementType.PropertyCollection);
            sut.Element.Type.ShouldBe(typeof(DummyFooClass1));
        }

        [Test]
        public void PositionedDescriptor_MapToWithoutTag_ElementIsPropertyCollection()
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            sut.MapTo<DummyFooClass1>().Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(0, 1).Required();
                    properties.Property(x => x.Property2).Position(1, 2).NotRequired();
                }); ;

            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").Positions["StartPosition"].ShouldBe(0);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").Positions["EndPosition"].ShouldBe(1);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").Positions["StartPosition"].ShouldBeLessThan(sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").Positions["EndPosition"]);
            
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").Positions["StartPosition"].ShouldBe(1);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").Positions["EndPosition"].ShouldBe(2);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").Positions["StartPosition"].ShouldBeLessThan(sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").Positions["EndPosition"]);

            sut.Element.Elements.Count(x => x.ElementType == ElementType.Property).ShouldBe(sut.Element.Elements.Count);
        }

        [Test]
        [TestCase("FOO1", "FOO1")]
        [TestCase("FOO2", "FOO2")]
        [TestCase("FOO3", "FOO3")]
        public void PositionedDescriptor_MapToWithTag_HasRightTag(string tag, string expectedResult)
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            sut.MapTo<DummyFooClass1>(tag);

            sut.Element.ElementType.ShouldBe(ElementType.Tag);
            sut.Element.Tag.ShouldBe(expectedResult);
            sut.Element.Type.ShouldBe(typeof(DummyFooClass1));
        }

        [Test]
        [TestCase(0, 1, 0, 1)]
        [TestCase(1, 2, 1, 2)]
        [TestCase(2, 3, 2, 3)]
        public void PositionedDescriptor_Position_HasRightPosition(int startPosition, int endPosition, int expectedStartPosition, int expectedEndPosition)
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            sut.MapTo<DummyFooClass1>("FOO").Position(startPosition, endPosition);

            sut.Element.Positions["StartPosition"].ShouldBe(expectedStartPosition);
            sut.Element.Positions["EndPosition"].ShouldBe(expectedEndPosition);
            sut.Element.Positions["StartPosition"].ShouldBeLessThan(sut.Element.Positions["EndPosition"]);
        }

        [Test]
        public void PositionedDescriptor_WithTagAddProperties_AllTagsHaveParentDelimitationString()
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            sut.MapTo<DummyFooClass1>("FOOTAG").Position(0, 1).Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(1, 2).Required();
                    properties.Property(x => x.Property2).Position(2, 3).NotRequired();
                    properties.Property(x => x.Property7).MapTo<DummyFooClass2>("FOODETAILTAG2").Position(0, 1).Properties(
                        childProperties =>
                        {
                            childProperties.Property(x => x.Property1).Position(1, 2).Required();
                            childProperties.Property(x => x.Property2).Position(2, 3).NotRequired();

                            childProperties.Property(x => x.Property7).MapTo<DummyFooClass3>("FOOSUBDETAILTAG1").Position(0, 1).Properties(
                                grandChildProperties =>
                                {
                                    grandChildProperties.Property(x => x.Property1).Position(1, 2).NotRequired();
                                });
                        });
                });

            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").Positions["StartPosition"].ShouldBe(1);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property1").Positions["EndPosition"].ShouldBe(2);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").Positions["StartPosition"].ShouldBe(2);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property2").Positions["EndPosition"].ShouldBe(3);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7").ElementType.ShouldBe(ElementType.Tag);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7").Positions["StartPosition"].ShouldBe(0);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7").Positions["EndPosition"].ShouldBe(1);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7").Tag.ShouldBe("FOODETAILTAG2");
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7").Elements.Count.ShouldBe(3);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                .Elements.FirstOrDefault(x => x.Name == "Property1").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                .Elements.FirstOrDefault(x => x.Name == "Property1").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property1").Positions["StartPosition"].ShouldBe(1);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property1").Positions["EndPosition"].ShouldBe(2);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                .Elements.FirstOrDefault(x => x.Name == "Property2").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                .Elements.FirstOrDefault(x => x.Name == "Property2").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property2").Positions["StartPosition"].ShouldBe(2);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property2").Positions["EndPosition"].ShouldBe(3);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property7").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property7").ElementType.ShouldBe(ElementType.Tag);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property7").Positions["StartPosition"].ShouldBe(0);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property7").Positions["EndPosition"].ShouldBe(1);
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
                 .Elements.FirstOrDefault(x => x.Name == "Property1").Positions["StartPosition"].ShouldBe(1);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property7")
                 .Elements.FirstOrDefault(x => x.Name == "Property1").Positions["EndPosition"].ShouldBe(2);
        }
    }

    public class TestPositionedDescriptor : PositionedDescriptor
    {
        public TestPositionedDescriptor(IParser parser) : base(parser)
        {
        }

        public Element Element => _element;
    }
}

using System.Collections.Generic;
using System.Linq;
using Helpful.TextParser.Fluent.Impl;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Helpful.TextParser.Test.Fluent
{
    public class PositionedDescriptorTest
    {
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

            sut.MapTo<PositionedFooClass>();

            sut.Element.ElementType.ShouldBe(ElementType.PropertyCollection);
            sut.Element.Type.ShouldBe(typeof(PositionedFooClass));
        }

        [Test]
        public void PositionedDescriptor_MapToWithoutTag_ElementIsPropertyCollection()
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            sut.MapTo<PositionedFooClass>().Properties(
                properties =>
                {
                    properties.Property(x => x.FooProperty1).Position(0, 1).Required();
                    properties.Property(x => x.FooProperty2).Position(1, 2).NotRequired();
                }); ;

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").Positions["StartPosition"].ShouldBe(0);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").Positions["EndPosition"].ShouldBe(1);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").Positions["StartPosition"].ShouldBeLessThan(sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").Positions["EndPosition"]);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").Positions["StartPosition"].ShouldBe(1);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").Positions["EndPosition"].ShouldBe(2);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").Positions["StartPosition"].ShouldBeLessThan(sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").Positions["EndPosition"]);

            sut.Element.Elements.Count(x => x.ElementType == ElementType.Property).ShouldBe(sut.Element.Elements.Count);
        }

        [Test]
        [TestCase("FOO1", "FOO1")]
        [TestCase("FOO2", "FOO2")]
        [TestCase("FOO3", "FOO3")]
        public void PositionedDescriptor_MapToWithTag_HasRightTag(string tag, string expectedResult)
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            sut.MapTo<PositionedFooClass>(tag);

            sut.Element.ElementType.ShouldBe(ElementType.Tag);
            sut.Element.Tag.ShouldBe(expectedResult);
            sut.Element.Type.ShouldBe(typeof(PositionedFooClass));
        }

        [Test]
        [TestCase(0, 1, 0, 1)]
        [TestCase(1, 2, 1, 2)]
        [TestCase(2, 3, 2, 3)]
        public void PositionedDescriptor_Position_HasRightPosition(int startPosition, int endPosition, int expectedStartPosition, int expectedEndPosition)
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            sut.MapTo<PositionedFooClass>("FOO").Position(startPosition, endPosition);

            sut.Element.Positions["StartPosition"].ShouldBe(expectedStartPosition);
            sut.Element.Positions["EndPosition"].ShouldBe(expectedEndPosition);
            sut.Element.Positions["StartPosition"].ShouldBeLessThan(sut.Element.Positions["EndPosition"]);
        }

        [Test]
        public void PositionedDescriptor_WithTagAddProperties_AllTagsHaveParentDelimitationString()
        {
            var sut = new TestPositionedDescriptor(It.IsAny<IParser>());

            sut.MapTo<PositionedFooClass>("FOOTAG").Position(0, 1).Properties(
                properties =>
                {
                    properties.Property(x => x.FooProperty1).Position(1, 2).Required();
                    properties.Property(x => x.FooProperty2).Position(2, 3).NotRequired();
                    properties.Property(x => x.FooProperty3).MapTo<PositionedChildFooClass>("FOODETAILTAG1").Position(0, 1);
                    properties.Property(x => x.FooProperty4).MapTo<PositionedChildFooClass>("FOODETAILTAG2").Position(0, 1).Properties(
                        childProperties =>
                        {
                            childProperties.Property(x => x.FooProperty5).Position(1, 2).Required();
                            childProperties.Property(x => x.FooProperty6).Position(2, 3).NotRequired();

                            childProperties.Property(x => x.FooProperty7).MapTo<PositionedGrandChildFooClass>("FOOSUBDETAILTAG1").Position(0, 1).Properties(
                                grandChildProperties =>
                                {
                                    grandChildProperties.Property(x => x.FooProperty8).Position(1, 2).NotRequired();
                                });
                        });
                });

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").Positions["StartPosition"].ShouldBe(1);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty1").Positions["EndPosition"].ShouldBe(2);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").Positions["StartPosition"].ShouldBe(2);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty2").Positions["EndPosition"].ShouldBe(3);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty3").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty3").ElementType.ShouldBe(ElementType.Tag);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty3").Positions["StartPosition"].ShouldBe(0);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty3").Positions["EndPosition"].ShouldBe(1);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty3").Tag.ShouldBe("FOODETAILTAG1");

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4").ElementType.ShouldBe(ElementType.Tag);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4").Positions["StartPosition"].ShouldBe(0);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4").Positions["EndPosition"].ShouldBe(1);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4").Tag.ShouldBe("FOODETAILTAG2");
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4").Elements.Count.ShouldBe(3);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                .Elements.FirstOrDefault(x => x.Name == "FooProperty5").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                .Elements.FirstOrDefault(x => x.Name == "FooProperty5").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty5").Positions["StartPosition"].ShouldBe(1);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty5").Positions["EndPosition"].ShouldBe(2);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                .Elements.FirstOrDefault(x => x.Name == "FooProperty6").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                .Elements.FirstOrDefault(x => x.Name == "FooProperty6").ElementType.ShouldBe(ElementType.Property);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty6").Positions["StartPosition"].ShouldBe(2);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty6").Positions["EndPosition"].ShouldBe(3);

            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty7").ShouldNotBeNull();
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty7").ElementType.ShouldBe(ElementType.Tag);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty7").Positions["StartPosition"].ShouldBe(0);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty7").Positions["EndPosition"].ShouldBe(1);
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
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty8").Positions["StartPosition"].ShouldBe(1);
            sut.Element.Elements.FirstOrDefault(x => x.Name == "FooProperty4")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty7")
                 .Elements.FirstOrDefault(x => x.Name == "FooProperty8").Positions["EndPosition"].ShouldBe(2);
        }
    }

    public class PositionedFooClass
    {
        public bool FooProperty1 { get; set; }

        public decimal FooProperty2 { get; set; }

        public List<PositionedChildFooClass> FooProperty3 { get; set; }

        public List<PositionedChildFooClass> FooProperty4 { get; set; }
    }

    public class PositionedChildFooClass
    {
        public string FooProperty5 { get; set; }

        public string FooProperty6 { get; set; }

        public List<PositionedGrandChildFooClass> FooProperty7 { get; set; }
    }

    public class PositionedGrandChildFooClass
    {
        public string FooProperty8 { get; set; }
    }

    public class TestPositionedDescriptor : PositionedDescriptor
    {
        public TestPositionedDescriptor(IParser parser) : base(parser)
        {
        }

        public Element Element => _element;
    }
}

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
    public class DelimitedDescriptorTest
    {
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
                    properties.Property(x => x.FooProperty3).Position(0).Required();
                    properties.Property(x => x.FooProperty4).Position(1).NotRequired();
                }); ;

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
                    properties.Property(x => x.FooProperty3).MapTo<DelimitedChildFooClass>("FOODETAILTAG1").Position(0);
                    properties.Property(x => x.FooProperty4).MapTo<DelimitedChildFooClass>("FOODETAILTAG2").Position(0);
                });

            sut.Element.Elements.Count(x => x.Custom["DelimitationString"].Equals("FOODELIMITATIONSTRING"))
                .ShouldBe(sut.Element.Elements.Count(x => x.ElementType == ElementType.Tag));
        }
    }

    public class DelimitedFooClass
    {
        public bool FooProperty1 { get; set; }

        public decimal FooProperty2 { get; set; }

        public List<DelimitedChildFooClass> FooProperty3 { get; set; }

        public List<DelimitedChildFooClass> FooProperty4 { get; set; }
    }

    public class DelimitedChildFooClass
    {
        
    }

    public class TestDelimitedDescriptor : DelimitedDescriptor
    {
        public TestDelimitedDescriptor(string delimitationString, IParser parser) : base(delimitationString, parser)
        {
        }

        public Element Element => _element;
    }
}

using Helpful.TextParser.Impl.LineValueExtractor;
using Helpful.TextParser.Model;
using NUnit.Framework;
using Shouldly;

namespace Helpful.TextParser.Test.LineValueExtractor
{
    public class DelimitedLineValueExtractorTest
    {
        [Test]
        [TestCase("POSITION0,POSITION1,POSITION2", ",", 2, true, "POSITION2")]
        [TestCase("POSITION0,POSITION1,POSITION2", ",", 1, true, "POSITION1")]
        [TestCase("POSITION0,POSITION1,POSITION2", ",", 4, false, null)]
        [TestCase("POSITION0,POSITION1,POSITION2,,", ",", 4, true, "")]
        public void Extract_WithLines_ReturnsResult(string line, string delimitationString, int position, bool isFound, string expectedValue)
        {
            var element = new Element();

            element.Positions.Add("Positioned", position);
            element.Custom.Add("DelimitationString", delimitationString);

            var sut = new DelimitedLineValueExtractor();

            var result = sut.Extract(line, element);

            result.IsFound.ShouldBe(isFound);
            result.Value.ShouldBe(expectedValue);
        }

        [Test]
        [TestCase("POSITION0,POSITION1,POSITION2", ",", 2, true, "POSITION2")]
        [TestCase("POSITION0,POSITION1,POSITION2", ",", 1, true, "POSITION1")]
        [TestCase("POSITION0,POSITION1,POSITION2", ",", 4, false, null)]
        [TestCase("POSITION0,POSITION1,POSITION2,,", ",", 4, true, "")]
        public void Extract_WithLinesAndParentElement_ReturnsResult(string line, string delimitationString, int position, bool isFound, string expectedValue)
        {
            var element = new Element();

            element.Positions.Add("Positioned", position);

            var parentElement = new Element();

            parentElement.Custom.Add("DelimitationString", delimitationString);

            var sut = new DelimitedLineValueExtractor();

            var result = sut.Extract(line, element, parentElement);

            result.IsFound.ShouldBe(isFound);
            result.Value.ShouldBe(expectedValue);
        }
    }
}

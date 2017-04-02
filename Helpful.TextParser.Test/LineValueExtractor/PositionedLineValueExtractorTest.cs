using Helpful.TextParser.Impl.LineValueExtractor;
using Helpful.TextParser.Model;
using NUnit.Framework;
using Shouldly;

namespace Helpful.TextParser.Test.LineValueExtractor
{
    public class PositionedLineValueExtractorTest
    {
        [Test]
        [TestCase("VALUE1VALUE2VALUE3", 6, 12, true, "VALUE2")]
        [TestCase("VALUE1VALUE2VALUE3VAL4", 18, 24, true, "VAL4")]
        [TestCase("VALUE1", 7, 14, false, null)]
        [TestCase("VAL1     VAL3", 4, 9, true, "     ")]
        public void Extract_WithLines_ReturnsResult(string line, int startPosition, int endPosition, bool isFound, string expectedValue)
        {
            var element = new Element();

            element.Positions.Add("StartPosition", startPosition);
            element.Positions.Add("EndPosition", endPosition);

            var sut = new PositionedLineValueExtractor();

            var result = sut.Extract(line, element);

            result.IsFound.ShouldBe(isFound);
            result.Value.ShouldBe(expectedValue);
        }
    }
}

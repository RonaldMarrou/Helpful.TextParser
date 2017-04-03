using Helpful.TextParser.Impl.LineValueExtractor;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;
using NUnit.Framework;
using Moq;
using Shouldly;

namespace Helpful.TextParser.Test.LineValueExtractor
{
    public class LineValueExtractorFactoryTest
    {
        [Test]
        [TestCase(LineValueExtractorType.Positioned)]
        [TestCase(LineValueExtractorType.DelimitedByString)]
        public void Get_ObtainsExtractor_Returns(LineValueExtractorType lineValueExtractorType)
        {
            var extractor1 = new Mock<ILineValueExtractor>();
            extractor1.Setup(x => x.Type).Returns(LineValueExtractorType.Positioned);

            var extractor2 = new Mock<ILineValueExtractor>();
            extractor2.Setup(x => x.Type).Returns(LineValueExtractorType.DelimitedByString);

            var sut = new LineValueExtractorFactory(new[] { extractor1.Object, extractor2.Object });

            sut.Get(lineValueExtractorType).Type.ShouldBe(lineValueExtractorType);
        }
    }
}

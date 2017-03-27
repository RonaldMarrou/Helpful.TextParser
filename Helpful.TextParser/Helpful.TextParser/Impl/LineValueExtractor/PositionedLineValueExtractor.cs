using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Impl.LineValueExtractor
{
    public class PositionedLineValueExtractor : ILineValueExtractor
    {
        public LineValue Extract(string line, Element element, Element parentElement = null)
        {
            var startPosition = element.Positions["StartPosition"];

            var endPosition = element.Positions["EndPosition"];

            if (startPosition >= line.Length)
            {
                return new LineValue()
                {
                    IsFound = false,
                    Value = null
                };
            }

            if (endPosition >= line.Length)
            {
                return new LineValue()
                {
                    IsFound = true,
                    Value = line.Substring(startPosition)
                };
            }

            return new LineValue()
            {
                IsFound = true,
                Value = line.Substring(startPosition, endPosition - startPosition)
            };
        }

        public LineValueExtractorType Type => LineValueExtractorType.Positioned;
    }
}

using System;
using Helpful.TextParser.Interface;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Impl.LineValueExtractor
{
    public class DelimitedLineValueExtractor : ILineValueExtractor
    {
        public LineValue Extract(string line, Element element, Element parentElement = null)
        {
            var position = element.Positions["Position"];

            var delimitationString = parentElement == null ? element.Custom["DelimitationString"] : parentElement?.Custom["DelimitationString"];

            var lineParts = line.Split(new[] {delimitationString}, StringSplitOptions.None);

            if (position >= lineParts.Length)
            {
                return new LineValue()
                {
                    IsFound = false,
                    Value = null
                };
            }

            return new LineValue()
            {
                IsFound = true,
                Value = lineParts[position]
            };
        }

        public LineValueExtractorType Type => LineValueExtractorType.DelimitedByString;
    }
}

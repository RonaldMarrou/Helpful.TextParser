using System;
using System.Collections.Generic;

namespace Helpful.TextParser.Model
{
    public class Element
    {
        public Element()
        {
            Positions = new Dictionary<string, int>();

            Custom = new Dictionary<string, string>();

            Elements = new List<Element>();
        }

        public LineValueExtractorType LineValueExtractorType { get; set; }

        public ElementType ElementType { get; set; }

        public Type Type { get; set; }

        public string Tag { get; set; }

        public string Name { get; set; }

        public bool Required { get; set; }

        public Dictionary<string, int> Positions { get; set; }

        public Dictionary<string, string> Custom { get; set; }

        public List<Element> Elements { get; set; }
    }
}

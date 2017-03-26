using System;

namespace Helpful.TextParser.Model
{
    public class Element
    {
        public ElementType ElementType { get; set; }

        public Type Type { get; set; }

        public string Tag { get; set; }

        public string Name { get; set; }

        public bool Required { get; set; }
    }
}

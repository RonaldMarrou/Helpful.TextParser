using System.Collections.Generic;

namespace Helpful.TextParser.Model
{
    public class DelimitedElement : Element
    {
        public DelimitedElement()
        {
            Children = new List<DelimitedElement>();
        }

        public string DelimitationCharacter { get; set; }

        public int Position { get; set; }

        public List<DelimitedElement> Children { get; set; }
    }
}

using System.Collections.Generic;

namespace Helpful.TextParser.Model
{
    public class Result<T>
    {
        public Result()
        {
            Content = new List<T>();

            Errors = new List<string>();
        }

        public List<T> Content { get; set; }

        public List<string> Errors { get; set; }
    }
}

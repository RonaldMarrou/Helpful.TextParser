using System;
using Helpful.TextParser.Model;

namespace Helpful.TextParser.Fluent.Interface
{
    public interface IParseDescriptor<TClass> where TClass : class
    {
        Result<TClass> Parse(string[] content);

        Result<TClass> Parse(Func<string[]> content);
    }
}

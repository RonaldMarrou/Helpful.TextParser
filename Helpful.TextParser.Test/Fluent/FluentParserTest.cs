using System;
using Helpful.TextParser.Impl;
using Helpful.TextParser.Interface;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Helpful.TextParser.Test.Fluent
{
    public class FluentParserTest
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void DelimitedDescriptor_DelimitationStringIsEmpty_ReturnSexception(string delimitationString)
        {
            var sut = new FluentParser(It.IsAny<IParser>());

            Should.Throw<ArgumentNullException>(() => sut.Delimited(delimitationString));
        }
    }
}

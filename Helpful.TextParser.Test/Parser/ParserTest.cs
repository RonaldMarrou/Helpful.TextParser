using System;
using Helpful.TextParser.Impl;
using Helpful.TextParser.Impl.LineValueExtractor;
using Helpful.TextParser.Interface;
using NUnit.Framework;
using Shouldly;

namespace Helpful.TextParser.Test.Parser
{
    public class ParserTest
    {
        [Test]
        public void Parser_DelimitedWithoutTagParse_WithErrors()
        {
            var sut = new FluentParser(new Impl.Parser(
                new LineValueExtractorFactory(
                    new ILineValueExtractor[]
                    {
                        new DelimitedLineValueExtractor(), 
                        new PositionedLineValueExtractor() 
                    }), new Impl.ValueSetter()
                ));

            var parser = sut.Delimited(",").MapTo<ParserFooClass1>().Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(0).Required();
                    properties.Property(x => x.Property2).Position(1).Required();
                    properties.Property(x => x.Property3).Position(2).Required();
                });
            
            var result = parser.Parse(new []
                    {
                        "PROPERTY1,,25654.34",
                        "PROPERTY1,PROPERTY2,43.643",
                        "PROPERTY1,PROPERTY2,",
                        "PROPERTY1,PROPERTY2,35646.22",
                        "PROPERTY1,",
                        "PROPERTY1,PROPERTY2,PROPERTY3"
                    });

            result.Errors.Count.ShouldBe(5);

            result.Errors[0].ShouldBe("Property Property2 is missing at Line 0.");
            result.Errors[1].ShouldBe("Property Property3 is missing at Line 2.");
            result.Errors[2].ShouldBe("Property Property2 is missing at Line 4.");
            result.Errors[3].ShouldBe("Property Property3 is missing at Line 4.");
            result.Errors[4].ShouldBe("Value of Property Property3 is not valid at Line 5.");
        }

        [Test]
        public void Parser_DelimitedWithoutTagParse_Success()
        {
            var sut = new FluentParser(new Impl.Parser(
                new LineValueExtractorFactory(
                    new ILineValueExtractor[]
                    {
                        new DelimitedLineValueExtractor(),
                        new PositionedLineValueExtractor()
                    }), new Impl.ValueSetter()
                ));

            var parser = sut.Delimited(",").MapTo<ParserFooClass1>().Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(0).Required();
                    properties.Property(x => x.Property2).Position(1).NotRequired();
                    properties.Property(x => x.Property3).Position(2).Required();
                    properties.Property(x => x.Property4).Position(3).Required();
                    properties.Property(x => x.Property5).Position(4).NotRequired();
                    properties.Property(x => x.Property6).Position(5).NotRequired();
                });

            var result = parser.Parse(new[]
                    {
                        "LINE1PROP1,LINE1PROP2,435.573,2017-04-02,LINE1PROP5",
                        "LINE2PROP1,,885.49,2015-01-08,LINE2PROP5",
                        "LINE3PROP1,LINE3PROP2,59873.494,2011-12-08,",
                        "LINE4PROP1,LINE4PROP2,11.12,1941-11-01,LINE4PROP5,33"
                    });

            result.Errors.Count.ShouldBe(0);

            result.Content.Count.ShouldBe(4);

            result.Content[0].Property1.ShouldBe("LINE1PROP1");
            result.Content[0].Property2.ShouldBe("LINE1PROP2");
            result.Content[0].Property3.ShouldBe((decimal)435.573);
            result.Content[0].Property4.ShouldBe(new DateTime(2017, 04, 02));
            result.Content[0].Property5.ShouldBe("LINE1PROP5");
            result.Content[0].Property6.ShouldBeNull();

            result.Content[1].Property1.ShouldBe("LINE2PROP1");
            result.Content[1].Property2.ShouldBeNullOrEmpty();
            result.Content[1].Property3.ShouldBe((decimal)885.49);
            result.Content[1].Property4.ShouldBe(new DateTime(2015, 01, 08));
            result.Content[1].Property5.ShouldBe("LINE2PROP5");
            result.Content[1].Property6.ShouldBeNull();

            result.Content[2].Property1.ShouldBe("LINE3PROP1");
            result.Content[2].Property2.ShouldBe("LINE3PROP2");
            result.Content[2].Property3.ShouldBe((decimal)59873.494);
            result.Content[2].Property4.ShouldBe(new DateTime(2011, 12, 08));
            result.Content[2].Property5.ShouldBeNullOrEmpty();
            result.Content[2].Property6.ShouldBeNull();

            result.Content[3].Property1.ShouldBe("LINE4PROP1");
            result.Content[3].Property2.ShouldBe("LINE4PROP2");
            result.Content[3].Property3.ShouldBe((decimal)11.12);
            result.Content[3].Property4.ShouldBe(new DateTime(1941, 11, 01));
            result.Content[3].Property5.ShouldBe("LINE4PROP5");
            result.Content[3].Property6.ShouldBe(33);
        }

        [Test]
        public void Parser_PositionedWithoutTagParse_WithErrors()
        {
            var sut = new FluentParser(new Impl.Parser(
                new LineValueExtractorFactory(
                    new ILineValueExtractor[]
                    {
                        new DelimitedLineValueExtractor(),
                        new PositionedLineValueExtractor()
                    }), new Impl.ValueSetter()
                ));

            var parser = sut.Positioned().MapTo<ParserFooClass1>().Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(0, 9).Required();
                    properties.Property(x => x.Property2).Position(9, 18).Required();
                    properties.Property(x => x.Property3).Position(18, 27).Required();
                });

            var result = parser.Parse(new[]
                    {
                        "PROPERTY1         25654.304",
                        "PROPERTY1         00043.643",
                        "PROPERTY1PROPERTY2         ",
                        "PROPERTY1PROPERTY235646.22",
                        "PROPERTY1",
                        "PROPERTY1PROPERTY2PROPERTY3"
                    });

            result.Errors.Count.ShouldBe(4);
            
            result.Errors[0].ShouldBe("Value of Property Property3 is not valid at Line 2.");
            result.Errors[1].ShouldBe("Property Property2 is missing at Line 4.");
            result.Errors[2].ShouldBe("Property Property3 is missing at Line 4.");
            result.Errors[3].ShouldBe("Value of Property Property3 is not valid at Line 5.");
        }

        [Test]
        public void Parser_PositioneddWithoutTagParse_Success()
        {
            var sut = new FluentParser(new Impl.Parser(
                new LineValueExtractorFactory(
                    new ILineValueExtractor[]
                    {
                        new DelimitedLineValueExtractor(),
                        new PositionedLineValueExtractor()
                    }), new Impl.ValueSetter()
                ));

            var parser = sut.Positioned().MapTo<ParserFooClass1>().Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(0, 10).Required();
                    properties.Property(x => x.Property2).Position(10, 20).NotRequired();
                    properties.Property(x => x.Property3).Position(20, 30).Required();
                    properties.Property(x => x.Property4).Position(30, 40).Required();
                    properties.Property(x => x.Property5).Position(40, 50).NotRequired();
                    properties.Property(x => x.Property6).Position(50, 55).NotRequired();
                });

            var result = parser.Parse(new[]
                    {
                        "LINE1PROP1LINE1PROP2100435.5732017-04-02LINE1PROP5",
                        "LINE2PROP1          000885.4902015-01-08LINE2PROP5",
                        "LINE3PROP1LINE3PROP2059873.4942011-12-08",
                        "LINE4PROP1LINE4PROP2000011.1201941-11-01LINE4PROP500033",
                        "LINE5PROP1LINE5PROP2000012.0101987-01-01LI",
                    });

            result.Errors.Count.ShouldBe(0);

            result.Content.Count.ShouldBe(5);

            result.Content[0].Property1.ShouldBe("LINE1PROP1");
            result.Content[0].Property2.ShouldBe("LINE1PROP2");
            result.Content[0].Property3.ShouldBe((decimal)100435.573);
            result.Content[0].Property4.ShouldBe(new DateTime(2017, 04, 02));
            result.Content[0].Property5.ShouldBe("LINE1PROP5");
            result.Content[0].Property6.ShouldBeNull();

            result.Content[1].Property1.ShouldBe("LINE2PROP1");
            result.Content[1].Property2.ShouldBe("          ");
            result.Content[1].Property3.ShouldBe((decimal)885.49);
            result.Content[1].Property4.ShouldBe(new DateTime(2015, 01, 08));
            result.Content[1].Property5.ShouldBe("LINE2PROP5");
            result.Content[1].Property6.ShouldBeNull();

            result.Content[2].Property1.ShouldBe("LINE3PROP1");
            result.Content[2].Property2.ShouldBe("LINE3PROP2");
            result.Content[2].Property3.ShouldBe((decimal)59873.494);
            result.Content[2].Property4.ShouldBe(new DateTime(2011, 12, 08));
            result.Content[2].Property5.ShouldBeNullOrEmpty();
            result.Content[2].Property6.ShouldBeNull();

            result.Content[3].Property1.ShouldBe("LINE4PROP1");
            result.Content[3].Property2.ShouldBe("LINE4PROP2");
            result.Content[3].Property3.ShouldBe((decimal)11.12);
            result.Content[3].Property4.ShouldBe(new DateTime(1941, 11, 01));
            result.Content[3].Property5.ShouldBe("LINE4PROP5");
            result.Content[3].Property6.ShouldBe(33);

            result.Content[4].Property1.ShouldBe("LINE5PROP1");
            result.Content[4].Property2.ShouldBe("LINE5PROP2");
            result.Content[4].Property3.ShouldBe((decimal)12.01);
            result.Content[4].Property4.ShouldBe(new DateTime(1987, 01, 01));
            result.Content[4].Property5.ShouldBe("LI");
            result.Content[4].Property6.ShouldBeNull();
        }
    }

    public class ParserFooClass1
    {
        public string Property1 { get; set; }

        public string Property2 { get; set; }

        public decimal Property3 { get; set; }

        public DateTime Property4 { get; set; }

        public string Property5 { get; set; }

        public int? Property6 { get; set; }
    }
}

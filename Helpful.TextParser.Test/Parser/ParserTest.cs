using System;
using System.Collections.Generic;
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
        [TestCase(0,"")]
        [TestCase(1, "NOTVALID,PROPERTY1,PROPERTY2,43.643")]
        [TestCase(2, "ANOTHERNOTVALID,PROPERTY1,PROPERTY2,43.643")]
        [TestCase(3, ",PROPERTY1,PROPERTY2,43.643")]
        public void Parser_DelimitedWithTagParse_TagNotValid(int lineToSet, string value)
        {
            var sut = new FluentParser(new Impl.Parser(
                new LineValueExtractorFactory(
                    new ILineValueExtractor[]
                    {
                        new DelimitedLineValueExtractor(),
                        new PositionedLineValueExtractor()
                    }), new Impl.ValueSetter()
                ));

            var parser = sut.Delimited(",").MapTo<ParserFooClass1>("HEADER").Position(0).Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(1).Required();
                    properties.Property(x => x.Property2).Position(2).Required();
                    properties.Property(x => x.Property3).Position(3).Required();
                });

            var lines = new[]
            {
                "HEADER,PROPERTY1,PROPERTY2,43.643",
                "HEADER,PROPERTY1,PROPERTY2,43.643",
                "HEADER,PROPERTY1,PROPERTY2,43.643",
                "HEADER,PROPERTY1,PROPERTY2,43.643",
            };

            lines[lineToSet] = value;

            var result = parser.Parse(lines);

            result.Errors.Count.ShouldBe(1);

            result.Errors[0].ShouldBe($"Line {lineToSet} does not contain any valid tag.");
        }

        [Test]
        [TestCase(1, "")]
        [TestCase(5, "NOTVALID,PROPERTY1,PROPERTY2,43.643")]
        [TestCase(7, "ANOTHERNOTVALID,PROPERTY1,PROPERTY2,43.643")]
        [TestCase(8, ",PROPERTY1,PROPERTY2,43.643")]
        public void Parser_DelimitedWithTagParse_ChildrenTagNotValid(int lineToSet, string value)
        {
            var sut = new FluentParser(new Impl.Parser(
                new LineValueExtractorFactory(
                    new ILineValueExtractor[]
                    {
                        new DelimitedLineValueExtractor(),
                        new PositionedLineValueExtractor()
                    }), new Impl.ValueSetter()
                ));

            var parser = sut.Delimited(",").MapTo<ParserFooClass1>("HEADER").Position(0).Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(1).Required();
                    properties.Property(x => x.Property2).Position(2).Required();
                    properties.Property(x => x.Property3).Position(3).Required();

                    properties.Property(x => x.Property7).MapTo<ParserFooClass2>("DETAIL").Position(0).Properties(
                        childProperties =>
                        {
                            childProperties.Property(x => x.Property1).Position(1).Required();
                            childProperties.Property(x => x.Property2).Position(2).Required();
                            childProperties.Property(x => x.Property3).Position(3).Required();
                        });
                });

            var lines = new[]
            {
                "HEADER,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
                "HEADER,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
                "HEADER,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
            };

            lines[lineToSet] = value;

            var result = parser.Parse(lines);

            result.Errors.Count.ShouldBe(1);

            result.Errors[0].ShouldBe($"Line {lineToSet} does not contain any valid tag.");
        }

        [Test]
        [TestCase(3, "")]
        [TestCase(5, "NOTVALID,PROPERTY1,PROPERTY2,43.643")]
        [TestCase(6, "ANOTHERNOTVALID,PROPERTY1,PROPERTY2,43.643")]
        [TestCase(17, ",PROPERTY1,PROPERTY2,43.643")]
        public void Parser_DelimitedWithTagParse_GrandChildrenTagNotValid(int lineToSet, string value)
        {
            var sut = new FluentParser(new Impl.Parser(
                new LineValueExtractorFactory(
                    new ILineValueExtractor[]
                    {
                        new DelimitedLineValueExtractor(),
                        new PositionedLineValueExtractor()
                    }), new Impl.ValueSetter()
                ));

            var parser = sut.Delimited(",").MapTo<ParserFooClass1>("HEADER").Position(0).Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(1).Required();
                    properties.Property(x => x.Property2).Position(2).Required();
                    properties.Property(x => x.Property3).Position(3).Required();

                    properties.Property(x => x.Property7).MapTo<ParserFooClass2>("DETAIL").Position(0).Properties(
                        childProperties =>
                        {
                            childProperties.Property(x => x.Property1).Position(1).Required();
                            childProperties.Property(x => x.Property2).Position(2).Required();
                            childProperties.Property(x => x.Property3).Position(3).Required();

                            childProperties.Property(x => x.Property7).MapTo<ParserFooClass3>("SUBDETAIL").Position(0).Properties(
                                grandChildProperties =>
                                {
                                    grandChildProperties.Property(x => x.Property1).Position(1).Required();
                                    grandChildProperties.Property(x => x.Property2).Position(2).Required();
                                    grandChildProperties.Property(x => x.Property3).Position(3).Required();
                                });
                        });
                });

            var lines = new[]
            {
                "HEADER,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "HEADER,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "HEADER,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
            };

            lines[lineToSet] = value;

            var result = parser.Parse(lines);

            result.Errors.Count.ShouldBe(1);

            result.Errors[0].ShouldBe($"Line {lineToSet} does not contain any valid tag.");
        }

        [Test]
        public void Parser_DelimitedWithTagParse_WithErrors()
        {
            var sut = new FluentParser(new Impl.Parser(
                new LineValueExtractorFactory(
                    new ILineValueExtractor[]
                    {
                        new DelimitedLineValueExtractor(),
                        new PositionedLineValueExtractor()
                    }), new Impl.ValueSetter()
                ));

            var parser = sut.Delimited(",").MapTo<ParserFooClass1>("HEADER").Position(0).Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(1).Required();
                    properties.Property(x => x.Property2).Position(2).Required();
                    properties.Property(x => x.Property3).Position(3).Required();

                    properties.Property(x => x.Property7).MapTo<ParserFooClass2>("DETAIL").Position(0).Properties(
                        childProperties =>
                        {
                            childProperties.Property(x => x.Property1).Position(1).Required();
                            childProperties.Property(x => x.Property2).Position(2).Required();
                            childProperties.Property(x => x.Property3).Position(3).Required();

                            childProperties.Property(x => x.Property7).MapTo<ParserFooClass3>("SUBDETAIL").Position(0).Properties(
                                grandChildProperties =>
                                {
                                    grandChildProperties.Property(x => x.Property1).Position(1).Required();
                                    grandChildProperties.Property(x => x.Property2).Position(2).Required();
                                    grandChildProperties.Property(x => x.Property3).Position(3).Required();
                                });
                        });
                });

            var lines = new[]
            {
                "HEADER,,PROPERTY2,ABC",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "HEADER,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,,PROPERTY2,NOTASTRING",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,SURELY NOT A STRING",
                "HEADER,,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "DETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
                "SUBDETAIL,PROPERTY1,PROPERTY2,43.643",
            };

            var result = parser.Parse(lines);

            result.Errors.Count.ShouldBe(8);

            result.Errors[0].ShouldBe("Property Property1 is missing at Line 0.");
            result.Errors[1].ShouldBe("Value of Property Property3 is not valid at Line 0.");
            result.Errors[2].ShouldBe("Property Property2 is missing at Line 3.");
            result.Errors[3].ShouldBe("Property Property1 is missing at Line 8.");
            result.Errors[4].ShouldBe("Value of Property Property3 is not valid at Line 8.");
            result.Errors[5].ShouldBe("Value of Property Property3 is not valid at Line 10.");
            result.Errors[6].ShouldBe("Property Property1 is missing at Line 11.");
            result.Errors[7].ShouldBe("Property Property2 is missing at Line 13.");
        }

        [Test]
        public void Parser_DelimitedWithTagParse_Success()
        {
            var sut = new FluentParser(new Impl.Parser(
                new LineValueExtractorFactory(
                    new ILineValueExtractor[]
                    {
                        new DelimitedLineValueExtractor(),
                        new PositionedLineValueExtractor()
                    }), new Impl.ValueSetter()
                ));

            var parser = sut.Delimited(",").MapTo<ParserFooClass1>("HEADER").Position(0).Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(1).Required();
                    properties.Property(x => x.Property2).Position(2).Required();
                    properties.Property(x => x.Property3).Position(3).Required();
                    properties.Property(x => x.Property4).Position(4).Required();
                    properties.Property(x => x.Property5).Position(5).Required();
                    properties.Property(x => x.Property6).Position(6).NotRequired();

                    properties.Property(x => x.Property7).MapTo<ParserFooClass2>("DETAIL").Position(0).Properties(
                        childProperties =>
                        {
                            childProperties.Property(x => x.Property1).Position(1).Required();
                            childProperties.Property(x => x.Property2).Position(2).Required();
                            childProperties.Property(x => x.Property3).Position(3).Required();
                            childProperties.Property(x => x.Property4).Position(4).Required();
                            childProperties.Property(x => x.Property5).Position(5).Required();
                            childProperties.Property(x => x.Property6).Position(6).NotRequired();

                            childProperties.Property(x => x.Property7).MapTo<ParserFooClass3>("SUBDETAIL").Position(0).Properties(
                                grandChildProperties =>
                                {
                                    grandChildProperties.Property(x => x.Property1).Position(1).Required();
                                    grandChildProperties.Property(x => x.Property2).Position(2).Required();
                                    grandChildProperties.Property(x => x.Property3).Position(3).Required();
                                    grandChildProperties.Property(x => x.Property4).Position(4).Required();
                                    grandChildProperties.Property(x => x.Property5).Position(5).Required();
                                    grandChildProperties.Property(x => x.Property6).Position(6).NotRequired();
                                });
                        });
                });

            var lines = new[]
            {
                "HEADER,HEAD11,HEAD12,80.50,2017-04-02,HEAD14,11",
                "HEADER,HEAD21,HEAD22,4005.50,2017-04-02,HEAD24,",
                "DETAIL,DETAIL211,DETAIL221,00505.50,2011-04-01,DETAIL241,55",
                "DETAIL,DETAIL212,DETAIL222,0484005.500,2012-03-02,DETAIL242,47",
                "HEADER,HEAD31,HEAD32,105.50,1997-04-02,HEAD34,",
                "DETAIL,DETAIL311,DETAIL321,5899.88,1911-04-01,DETAIL341,99",
                "SUBDETAIL,SUBDETAIL3111,SUBDETAIL3211,14409.011,1985-12-03,SUBDETAIL3411,80",
                "SUBDETAIL,SUBDETAIL3112,SUBDETAIL3212,9.1,1991-11-08,SUBDETAIL3412,80"
            };

            var result = parser.Parse(lines);

            result.Errors.Count.ShouldBe(0);

            result.Content[0].Property7.ShouldBeNull();
            result.Content[0].Property1.ShouldBe("HEAD11");
            result.Content[0].Property2.ShouldBe("HEAD12");
            result.Content[0].Property3.ShouldBe((decimal)80.5);
            result.Content[0].Property4.ShouldBe(new DateTime(2017, 04, 02));
            result.Content[0].Property5.ShouldBe("HEAD14");
            result.Content[0].Property6.ShouldBe(11);

            result.Content[1].Property7.Count.ShouldBe(2);
            result.Content[1].Property1.ShouldBe("HEAD21");
            result.Content[1].Property2.ShouldBe("HEAD22");
            result.Content[1].Property3.ShouldBe((decimal)4005.50);
            result.Content[1].Property4.ShouldBe(new DateTime(2017, 04, 02));
            result.Content[1].Property5.ShouldBe("HEAD24");
            result.Content[1].Property6.ShouldBeNull();

            result.Content[1].Property7[0].Property7.ShouldBeNull();
            result.Content[1].Property7[0].Property1.ShouldBe("DETAIL211");
            result.Content[1].Property7[0].Property2.ShouldBe("DETAIL221");
            result.Content[1].Property7[0].Property3.ShouldBe((decimal)505.5);
            result.Content[1].Property7[0].Property4.ShouldBe(new DateTime(2011, 04, 01));
            result.Content[1].Property7[0].Property5.ShouldBe("DETAIL241");
            result.Content[1].Property7[0].Property6.ShouldBe(55);

            result.Content[1].Property7[1].Property7.ShouldBeNull();
            result.Content[1].Property7[1].Property1.ShouldBe("DETAIL212");
            result.Content[1].Property7[1].Property2.ShouldBe("DETAIL222");
            result.Content[1].Property7[1].Property3.ShouldBe((decimal)484005.5);
            result.Content[1].Property7[1].Property4.ShouldBe(new DateTime(2012, 03, 02));
            result.Content[1].Property7[1].Property5.ShouldBe("DETAIL242");
            result.Content[1].Property7[1].Property6.ShouldBe(47);

            result.Content[2].Property7.Count.ShouldBe(1);
            result.Content[2].Property1.ShouldBe("HEAD31");
            result.Content[2].Property2.ShouldBe("HEAD32");
            result.Content[2].Property3.ShouldBe((decimal)105.50);
            result.Content[2].Property4.ShouldBe(new DateTime(1997, 04, 02));
            result.Content[2].Property5.ShouldBe("HEAD34");
            result.Content[2].Property6.ShouldBeNull();

            result.Content[2].Property7[0].Property7.Count.ShouldBe(2);
            result.Content[2].Property7[0].Property1.ShouldBe("DETAIL311");
            result.Content[2].Property7[0].Property2.ShouldBe("DETAIL321");
            result.Content[2].Property7[0].Property3.ShouldBe((decimal)5899.88);
            result.Content[2].Property7[0].Property4.ShouldBe(new DateTime(1911, 04, 01));
            result.Content[2].Property7[0].Property5.ShouldBe("DETAIL341");
            result.Content[2].Property7[0].Property6.ShouldBe(99);

            result.Content[2].Property7[0].Property7[0].Property1.ShouldBe("SUBDETAIL3111");
            result.Content[2].Property7[0].Property7[0].Property2.ShouldBe("SUBDETAIL3211");
            result.Content[2].Property7[0].Property7[0].Property3.ShouldBe((decimal)14409.011);
            result.Content[2].Property7[0].Property7[0].Property4.ShouldBe(new DateTime(1985, 12, 03));
            result.Content[2].Property7[0].Property7[0].Property5.ShouldBe("SUBDETAIL3411");
            result.Content[2].Property7[0].Property7[0].Property6.ShouldBe(80);

            result.Content[2].Property7[0].Property7[1].Property1.ShouldBe("SUBDETAIL3112");
            result.Content[2].Property7[0].Property7[1].Property2.ShouldBe("SUBDETAIL3212");
            result.Content[2].Property7[0].Property7[1].Property3.ShouldBe((decimal)9.1);
            result.Content[2].Property7[0].Property7[1].Property4.ShouldBe(new DateTime(1991, 11, 08));
            result.Content[2].Property7[0].Property7[1].Property5.ShouldBe("SUBDETAIL3412");
            result.Content[2].Property7[0].Property7[1].Property6.ShouldBe(80);
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

        [Test]
        [TestCase(0, "")]
        [TestCase(1, "AAPROPERTY1PROPERTY243.643")]
        [TestCase(2, "BBPROPERTY1PROPERTY243.643")]
        [TestCase(3, "  PROPERTY1PROPERTY243.643")]
        public void Parser_PositionedWithTagParse_TagNotValid(int lineToSet, string value)
        {
            var sut = new FluentParser(new Impl.Parser(
                new LineValueExtractorFactory(
                    new ILineValueExtractor[]
                    {
                        new DelimitedLineValueExtractor(),
                        new PositionedLineValueExtractor()
                    }), new Impl.ValueSetter()
                ));

            var parser = sut.Positioned().MapTo<ParserFooClass1>("HH").Position(0, 2).Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(2, 11).Required();
                    properties.Property(x => x.Property2).Position(11, 19).Required();
                    properties.Property(x => x.Property3).Position(19, 26).Required();
                });

            var lines = new[]
            {
                "HHPROPERTY1PROPERTY243.643",
                "HHPROPERTY1PROPERTY243.643",
                "HHPROPERTY1PROPERTY243.643",
                "HHPROPERTY1PROPERTY243.643",
            };

            lines[lineToSet] = value;

            var result = parser.Parse(lines);

            result.Errors.Count.ShouldBe(1);

            result.Errors[0].ShouldBe($"Line {lineToSet} does not contain any valid tag.");
        }

        [Test]
        [TestCase(1, "")]
        [TestCase(5, "AAPROPERTY1PROPERTY243.643")]
        [TestCase(7, "BBPROPERTY1PROPERTY243.643")]
        [TestCase(8, "  PROPERTY1PROPERTY243.643")]
        public void Parser_PositionedWithTagParse_ChildrenTagNotValid(int lineToSet, string value)
        {
            var sut = new FluentParser(new Impl.Parser(
                new LineValueExtractorFactory(
                    new ILineValueExtractor[]
                    {
                        new DelimitedLineValueExtractor(),
                        new PositionedLineValueExtractor()
                    }), new Impl.ValueSetter()
                ));

            var parser = sut.Positioned().MapTo<ParserFooClass1>("HH").Position(0, 2).Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(2, 11).Required();
                    properties.Property(x => x.Property2).Position(11, 19).Required();
                    properties.Property(x => x.Property3).Position(19, 26).Required();

                    properties.Property(x => x.Property7).MapTo<ParserFooClass2>("DD").Position(0, 2).Properties(
                        childProperties =>
                        {
                            childProperties.Property(x => x.Property1).Position(2, 11).Required();
                            childProperties.Property(x => x.Property2).Position(11, 19).Required();
                            childProperties.Property(x => x.Property3).Position(19, 26).Required();
                        });
                });

            var lines = new[]
            {
                "HHPROPERTY1PROPERTY243.643",
                "DDPROPERTY1PROPERTY243.643",
                "DDPROPERTY1PROPERTY243.643",
                "DDPROPERTY1PROPERTY243.643",
                "DDPROPERTY1PROPERTY243.643",
                "DDPROPERTY1PROPERTY243.643",
                "HHPROPERTY1PROPERTY243.643",
                "DDPROPERTY1PROPERTY243.643",
                "DDPROPERTY1PROPERTY243.643",
            };

            lines[lineToSet] = value;

            var result = parser.Parse(lines);

            result.Errors.Count.ShouldBe(1);

            result.Errors[0].ShouldBe($"Line {lineToSet} does not contain any valid tag.");
        }

        [Test]
        [TestCase(3, "")]
        [TestCase(5, "AAPROPERTY1PROPERTY243.643")]
        [TestCase(6, "CCPROPERTY1PROPERTY243.643")]
        [TestCase(17, "  PROPERTY1PROPERTY243.643")]
        public void Parser_PositionedWithTagParse_GrandChildrenTagNotValid(int lineToSet, string value)
        {
            var sut = new FluentParser(new Impl.Parser(
                new LineValueExtractorFactory(
                    new ILineValueExtractor[]
                    {
                        new DelimitedLineValueExtractor(),
                        new PositionedLineValueExtractor()
                    }), new Impl.ValueSetter()
                ));

            var parser = sut.Positioned().MapTo<ParserFooClass1>("HH").Position(0, 2).Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(2, 11).Required();
                    properties.Property(x => x.Property2).Position(11, 19).Required();
                    properties.Property(x => x.Property3).Position(19, 26).Required();

                    properties.Property(x => x.Property7).MapTo<ParserFooClass2>("DD").Position(0, 2).Properties(
                        childProperties =>
                        {
                            childProperties.Property(x => x.Property1).Position(2, 11).Required();
                            childProperties.Property(x => x.Property2).Position(11, 19).Required();
                            childProperties.Property(x => x.Property3).Position(19, 26).Required();

                            childProperties.Property(x => x.Property7).MapTo<ParserFooClass3>("SD").Position(0, 2).Properties(
                                grandChildProperties =>
                                {
                                    grandChildProperties.Property(x => x.Property1).Position(2, 11).Required();
                                    grandChildProperties.Property(x => x.Property2).Position(11, 19).Required();
                                    grandChildProperties.Property(x => x.Property3).Position(19, 26).Required();
                                });
                        });
                });

            var lines = new[]
            {
                "HHPROPERTY1PROPERTY243.643",
                "DDPROPERTY1PROPERTY243.643",
                "SDPROPERTY1PROPERTY243.643",
                "DDPROPERTY1PROPERTY243.643",
                "SDPROPERTY1PROPERTY243.643",
                "SDPROPERTY1PROPERTY243.643",
                "HHPROPERTY1PROPERTY243.643",
                "DDPROPERTY1PROPERTY243.643",
                "DDPROPERTY1PROPERTY243.643",
                "SDPROPERTY1PROPERTY243.643",
                "SDPROPERTY1PROPERTY243.643",
                "HHPROPERTY1PROPERTY243.643",
                "DDPROPERTY1PROPERTY243.643",
                "SDPROPERTY1PROPERTY243.643",
                "SDPROPERTY1PROPERTY243.643",
                "SDPROPERTY1PROPERTY243.643",
                "DDPROPERTY1PROPERTY243.643",
                "SDPROPERTY1PROPERTY243.643",
                "SDPROPERTY1PROPERTY243.643",
            };

            lines[lineToSet] = value;

            var result = parser.Parse(lines);

            result.Errors.Count.ShouldBe(1);

            result.Errors[0].ShouldBe($"Line {lineToSet} does not contain any valid tag.");
        }

        [Test]
        public void Parser_PositionedWithTagParse_WithErrors()
        {
            var sut = new FluentParser(new Impl.Parser(
                new LineValueExtractorFactory(
                    new ILineValueExtractor[]
                    {
                        new DelimitedLineValueExtractor(),
                        new PositionedLineValueExtractor()
                    }), new Impl.ValueSetter()
                ));

            var parser = sut.Positioned().MapTo<ParserFooClass1>("HH").Position(0, 2).Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(2, 11).Required();
                    properties.Property(x => x.Property2).Position(11, 19).Required();
                    properties.Property(x => x.Property3).Position(19, 26).Required();

                    properties.Property(x => x.Property7).MapTo<ParserFooClass2>("DD").Position(0, 2).Properties(
                        childProperties =>
                        {
                            childProperties.Property(x => x.Property1).Position(2, 11).Required();
                            childProperties.Property(x => x.Property2).Position(11, 19).Required();
                            childProperties.Property(x => x.Property3).Position(19, 26).Required();

                            childProperties.Property(x => x.Property7).MapTo<ParserFooClass3>("SD").Position(0, 2).Properties(
                                grandChildProperties =>
                                {
                                    grandChildProperties.Property(x => x.Property1).Position(2, 11).Required();
                                    grandChildProperties.Property(x => x.Property2).Position(11, 19).Required();
                                    grandChildProperties.Property(x => x.Property3).Position(19, 26).Required();
                                });
                        });
                });

            var lines = new[]
            {
                "HH         PROPERTY2ABC",
                "DDPROPERTY1PROPERTY243.643",
                "SDPROPERTY1PROPERTY243.643",
                "DDPROPERTY1         43.643",
                "SDPROPERTY1PROPERTY243.643",
                "SDPROPERTY1PROPERTY243.643",
                "HHPROPERTY1PROPERTY243.643",
                "DDPROPERTY1PROPERTY243.643",
                "DD,PROPERTY         AABBC",
                "SDPROPERTY1PROPERTY243.643",
                "SDPROPERTY1PROPERTY2NOTAST",
                "HH,PROPERTY         43.643",
                "DDPROPERTY1PROPERTY243.643",
                "SDPROPERTY1         43.643",
                "SDPROPERTY1PROPERTY2,43.643",
                "SDPROPERTY1PROPERTY2,43.643",
                "DDPROPERTY1PROPERTY2,43.643",
                "SDPROPERTY1PROPERTY2,43.643",
                "SDPROPERTY1PROPERTY2,43.643",
            };

            var result = parser.Parse(lines);

            result.Errors.Count.ShouldBe(3);

            result.Errors[0].ShouldBe("Value of Property Property3 is not valid at Line 0.");
            result.Errors[1].ShouldBe("Value of Property Property3 is not valid at Line 8.");
            result.Errors[2].ShouldBe("Value of Property Property3 is not valid at Line 10.");
        }

        [Test]
        public void Parser_PositionedWithTagParse_Success()
        {
            var sut = new FluentParser(new Impl.Parser(
                new LineValueExtractorFactory(
                    new ILineValueExtractor[]
                    {
                        new DelimitedLineValueExtractor(),
                        new PositionedLineValueExtractor()
                    }), new Impl.ValueSetter()
                ));

            var parser = sut.Positioned().MapTo<ParserFooClass1>("HH").Position(0, 2).Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(2, 8).Required();
                    properties.Property(x => x.Property2).Position(8, 14).Required();
                    properties.Property(x => x.Property3).Position(14, 21).Required();
                    properties.Property(x => x.Property4).Position(21, 31).Required();
                    properties.Property(x => x.Property5).Position(31, 37).Required();
                    properties.Property(x => x.Property6).Position(37, 39).NotRequired();

                    properties.Property(x => x.Property7).MapTo<ParserFooClass2>("DD").Position(0, 2).Properties(
                        childProperties =>
                        {
                            childProperties.Property(x => x.Property1).Position(2, 11).Required();
                            childProperties.Property(x => x.Property2).Position(11, 20).Required();
                            childProperties.Property(x => x.Property3).Position(20, 31).Required();
                            childProperties.Property(x => x.Property4).Position(31, 41).Required();
                            childProperties.Property(x => x.Property5).Position(41, 50).Required();
                            childProperties.Property(x => x.Property6).Position(50, 52).NotRequired();

                            childProperties.Property(x => x.Property7).MapTo<ParserFooClass3>("SD").Position(0, 2).Properties(
                                grandChildProperties =>
                                {
                                    grandChildProperties.Property(x => x.Property1).Position(2, 15).Required();
                                    grandChildProperties.Property(x => x.Property2).Position(15, 28).Required();
                                    grandChildProperties.Property(x => x.Property3).Position(28, 37).Required();
                                    grandChildProperties.Property(x => x.Property4).Position(37, 47).Required();
                                    grandChildProperties.Property(x => x.Property5).Position(47, 60).Required();
                                    grandChildProperties.Property(x => x.Property6).Position(60, 62).NotRequired();
                                });
                        });
                });

            var lines = new[]
            {
                "HHHEAD11HEAD120080.502017-04-02HEAD1411",
                "HHHEAD21HEAD224005.502017-04-02HEAD24",
                "DDDETAIL211DETAIL2210000505.5002011-04-01DETAIL24155",
                "DDDETAIL212DETAIL2220484005.5002012-03-02DETAIL24247",
                "HHHEAD31HEAD320105.501997-04-02HEAD34",
                "DDDETAIL311DETAIL3210005899.8811911-04-01DETAIL34199",
                "SDSUBDETAIL3111SUBDETAIL321114409.0111985-12-03SUBDETAIL341180",
                "SDSUBDETAIL3112SUBDETAIL321200009.1001991-11-08SUBDETAIL341280"
            };

            var result = parser.Parse(lines);

            result.Errors.Count.ShouldBe(0);

            result.Content[0].Property7.ShouldBeNull();
            result.Content[0].Property1.ShouldBe("HEAD11");
            result.Content[0].Property2.ShouldBe("HEAD12");
            result.Content[0].Property3.ShouldBe((decimal)80.5);
            result.Content[0].Property4.ShouldBe(new DateTime(2017, 04, 02));
            result.Content[0].Property5.ShouldBe("HEAD14");
            result.Content[0].Property6.ShouldBe(11);

            result.Content[1].Property7.Count.ShouldBe(2);
            result.Content[1].Property1.ShouldBe("HEAD21");
            result.Content[1].Property2.ShouldBe("HEAD22");
            result.Content[1].Property3.ShouldBe((decimal)4005.50);
            result.Content[1].Property4.ShouldBe(new DateTime(2017, 04, 02));
            result.Content[1].Property5.ShouldBe("HEAD24");
            result.Content[1].Property6.ShouldBeNull();

            result.Content[1].Property7[0].Property7.ShouldBeNull();
            result.Content[1].Property7[0].Property1.ShouldBe("DETAIL211");
            result.Content[1].Property7[0].Property2.ShouldBe("DETAIL221");
            result.Content[1].Property7[0].Property3.ShouldBe((decimal)505.5);
            result.Content[1].Property7[0].Property4.ShouldBe(new DateTime(2011, 04, 01));
            result.Content[1].Property7[0].Property5.ShouldBe("DETAIL241");
            result.Content[1].Property7[0].Property6.ShouldBe(55);

            result.Content[1].Property7[1].Property7.ShouldBeNull();
            result.Content[1].Property7[1].Property1.ShouldBe("DETAIL212");
            result.Content[1].Property7[1].Property2.ShouldBe("DETAIL222");
            result.Content[1].Property7[1].Property3.ShouldBe((decimal)484005.5);
            result.Content[1].Property7[1].Property4.ShouldBe(new DateTime(2012, 03, 02));
            result.Content[1].Property7[1].Property5.ShouldBe("DETAIL242");
            result.Content[1].Property7[1].Property6.ShouldBe(47);

            result.Content[2].Property7.Count.ShouldBe(1);
            result.Content[2].Property1.ShouldBe("HEAD31");
            result.Content[2].Property2.ShouldBe("HEAD32");
            result.Content[2].Property3.ShouldBe((decimal)105.50);
            result.Content[2].Property4.ShouldBe(new DateTime(1997, 04, 02));
            result.Content[2].Property5.ShouldBe("HEAD34");
            result.Content[2].Property6.ShouldBeNull();

            result.Content[2].Property7[0].Property7.Count.ShouldBe(2);
            result.Content[2].Property7[0].Property1.ShouldBe("DETAIL311");
            result.Content[2].Property7[0].Property2.ShouldBe("DETAIL321");
            result.Content[2].Property7[0].Property3.ShouldBe((decimal)5899.881);
            result.Content[2].Property7[0].Property4.ShouldBe(new DateTime(1911, 04, 01));
            result.Content[2].Property7[0].Property5.ShouldBe("DETAIL341");
            result.Content[2].Property7[0].Property6.ShouldBe(99);

            result.Content[2].Property7[0].Property7[0].Property1.ShouldBe("SUBDETAIL3111");
            result.Content[2].Property7[0].Property7[0].Property2.ShouldBe("SUBDETAIL3211");
            result.Content[2].Property7[0].Property7[0].Property3.ShouldBe((decimal)14409.011);
            result.Content[2].Property7[0].Property7[0].Property4.ShouldBe(new DateTime(1985, 12, 03));
            result.Content[2].Property7[0].Property7[0].Property5.ShouldBe("SUBDETAIL3411");
            result.Content[2].Property7[0].Property7[0].Property6.ShouldBe(80);

            result.Content[2].Property7[0].Property7[1].Property1.ShouldBe("SUBDETAIL3112");
            result.Content[2].Property7[0].Property7[1].Property2.ShouldBe("SUBDETAIL3212");
            result.Content[2].Property7[0].Property7[1].Property3.ShouldBe((decimal)9.1);
            result.Content[2].Property7[0].Property7[1].Property4.ShouldBe(new DateTime(1991, 11, 08));
            result.Content[2].Property7[0].Property7[1].Property5.ShouldBe("SUBDETAIL3412");
            result.Content[2].Property7[0].Property7[1].Property6.ShouldBe(80);
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

        public List<ParserFooClass2> Property7 { get; set; }
    }

    public class ParserFooClass2
    {
        public string Property1 { get; set; }

        public string Property2 { get; set; }

        public decimal Property3 { get; set; }

        public DateTime Property4 { get; set; }

        public string Property5 { get; set; }

        public int? Property6 { get; set; }

        public List<ParserFooClass3> Property7 { get; set; }
    }

    public class ParserFooClass3
    {
        public string Property1 { get; set; }

        public string Property2 { get; set; }

        public decimal Property3 { get; set; }

        public DateTime Property4 { get; set; }

        public string Property5 { get; set; }

        public int? Property6 { get; set; }
    }
}

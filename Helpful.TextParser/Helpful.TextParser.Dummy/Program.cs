using Helpful.TextParser.Impl;
using System.Collections.Generic;

namespace Helpful.TextParser.Dummy
{
    class Program
    {
        static void Main(string[] args)
        {
            var parserDelimitedWithTag = new FluentParser();

            parserDelimitedWithTag.Delimited(",").WithChildren().Tag("HEADER").Position(0).MapTo<PurchaseOrder>()
                .Properties(properties =>
                {
                    properties.Property(x => x.Number).WithoutChildren().Position(1).Required();
                    properties.Property(x => x.Supplier).WithoutChildren().Position(2).Required();

                    properties.Property(x => x.Details).WithChildren().Tag("DETAIL").Position(0).MapTo<Detail>()
                    .Properties(childProperties =>
                    {
                        childProperties.Property(x => x.ItemCode).WithoutChildren().Position(1).Required();
                        childProperties.Property(x => x.Description).WithoutChildren().Position(2).Required();
                    });
                })
                .Parse();

            //var parserPositionWithTag = new Parser();

            //parserPositionWithTag.Position().WithChildren().Tag("HEADER").StartPosition(0).EndPosition(2).MapTo<PurchaseOrder>()
            //    .Properties(properties =>
            //    {
            //        properties.Property(x => x.Number).StartPosition(0).EndPosition(1).Required();
            //        properties.Property(x => x.Supplier).StartPosition(1).EndPosition(2).Required();

            //        properties.Property(x => x.Details).Child().Tag("DETAIL").StartPosition(0).EndPosition(1).MapTo<Detail>()
            //        .Properties(childProperties =>
            //        {
            //            childProperties.Property(x => x.ItemCode).StartPosition(0).EndPosition(1).Required();
            //            childProperties.Property(x => x.Description).StartPosition(1).EndPosition(2).Required();
            //        });
            //    });

            //var parserDelimitedWithoutTag = new Parser();

            //parserDelimitedWithoutTag.Delimited(",").WithoutChildren().MapTo<PurchaseOrder>()
            //    .Properties(properties =>
            //    {
            //        properties.Property(x => x.Number).Position(1).Required();
            //        properties.Property(x => x.Supplier).Position(2).Required();
            //    });

            //var parserPositionWithoutTag = new Parser();

            //parserPositionWithoutTag.Position().WithoutChildren().MapTo<PurchaseOrder>()
            //    .Properties(properties =>
            //    {
            //        properties.Property(x => x.Number).StartPosition(0).EndPosition(1).Required();
            //        properties.Property(x => x.Supplier).StartPosition(1).EndPosition(2).Required();
            //    });
        }
    }

    public class PurchaseOrder
    {
        public string Number { get; set; }

        public int Supplier { get; set; }

        public List<Detail> Details { get; set; }
    }

    public class Detail
    {
        public string ItemCode { get; set; }

        public string Description { get; set; }
    }
}

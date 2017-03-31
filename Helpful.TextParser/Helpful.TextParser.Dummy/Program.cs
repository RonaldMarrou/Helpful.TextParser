using Helpful.TextParser.Impl;
using System.Collections.Generic;

namespace Helpful.TextParser.Dummy
{
    class Program
    {
        static void Main(string[] args)
        {
            var parserDelimitedWithTag = new FluentParser();

            parserDelimitedWithTag.Delimited(",").MapTo<PurchaseOrder>("HEADER").Position(0).Properties(properties =>
            {
                properties.Property(x => x.Number).Position(1).Required();
                properties.Property(x => x.Supplier).Position(2).Required();

                properties.Property(x => x.Details).MapTo<Detail>("DETAIL").Position(0).Properties(childProperties =>
                {
                    childProperties.Property(x => x.ItemCode).Position(1).Required();
                    childProperties.Property(x => x.Description).Position(2).Required();
                });
            });

            parserDelimitedWithTag.Delimited(",").MapTo<PurchaseOrder>().Properties(properties =>
            {
                properties.Property(x => x.Number).Position(1).Required();
                properties.Property(x => x.Supplier).Position(2).Required();
            });

            var parserPositionWithTag = new FluentParser();

            parserPositionWithTag.Positioned().MapTo<PurchaseOrder>("HEADER").Position(0,1).Properties(properties =>
                {
                    properties.Property(x => x.Number).Position(1,2).Required();
                    properties.Property(x => x.Supplier).Position(2,3).Required();

                    properties.Property(x => x.Details).MapTo<Detail>("DETAIL").Position(0,1).Properties(childProperties =>
                    {
                        childProperties.Property(x => x.ItemCode).Position(1,2).Required();
                        childProperties.Property(x => x.Description).Position(2,3).Required();
                    });
                });

            parserPositionWithTag.Positioned().MapTo<PurchaseOrder>().Properties(properties =>
            {
                properties.Property(x => x.Number).Position(0,1).Required();
                properties.Property(x => x.Supplier).Position(1,2).Required();
            });

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

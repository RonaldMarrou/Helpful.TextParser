using System;
using System.Collections.Generic;

namespace Helpful.TextParser.Test.Parser
{
    public class PurchaseOrder
    {
        public string Code { get; set; }

        public string Supplier { get; set; }

        public DateTime IssueDate { get; set; }

        public List<PurchaseOrderDetail> Details { get; set; }
    }
}

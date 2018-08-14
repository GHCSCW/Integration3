using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ghc.Utility;

namespace FixedWidthSerializerSample
{
    public class ResponsePriorAuthTestObject
    {
        [FlatFile(1, 3, Padding.Right, "")]
        public string RecordType { get; set; }

        [FlatFile(2, 10, Padding.Left, "")]
        public int RecordNumber { get; set; }

        [FlatFile(3, 10, Padding.Right, "")]
        public string MemberID { get; set; }

        [FlatFile(4, 10, Padding.Right, "")]
        public string BillingProviderID { get; set; }

        [FlatFile(5, 20, Padding.Right, "")]
        public string PriorAuthNumber { get; set; }

        [FlatFile(6, 8, Padding.Right, "MM/dd/yyyy")]
        public DateTime DateReceived { get; set; }

        [FlatFile(7, 8, Padding.Right, "MM/dd/yyyy")]
        public DateTime DateFinalized { get; set; }

        [FlatFile(8, 10, Padding.Left, "")]
        public int RenderingProviderID { get; set; }

        [FlatFile(9, 16, Padding.Left, "")]
        public decimal AuthorizedUnits { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ghc.Utility;
using System.Configuration;

namespace FixedWidthSerializerSample
{
    public class PriorAuthTestObject
    {
        [FlatFile(1, 3, Padding.Right, "")]
        public string RecordType
        {
            get { return ConfigurationManager.AppSettings["DetailRecordType"].ToString(); ; }
        }

        [FlatFile(2, 10, Padding.Left, "PADZERO")]
        public int RecordNumber { get; set; }

        [FlatFile(3, 10, Padding.Right, "")]
        public string MemberID { get; set; }

        [FlatFile(4, 10, Padding.Right, "")]
        public string BillingProviderID { get; set; }

        [FlatFile(5, 20, Padding.Right, "")]
        public string PriorAuthNumber { get; set; }

        [FlatFile(6, 8, Padding.Right, "yyyyMMdd")]
        public DateTime DateReceived { get; set; }

        [FlatFile(7, 8, Padding.Right, "yyyyMMdd")]
        public DateTime DateFinalized { get; set; }
        
        [FlatFile(8, 10, Padding.Left, "PADZERO")]
        public int RenderingProviderID { get; set; }
        
        [FlatFile(9, 16, Padding.Left, "REMOVEDECIMAL")]
        public decimal AuthorizedUnits { get; set; }
    }
}

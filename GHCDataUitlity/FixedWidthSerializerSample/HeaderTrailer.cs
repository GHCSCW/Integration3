using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ghc.Utility;
using System.Configuration;

namespace FixedWidthSerializerSample
{
    public class Header
    {
        [FlatFile(1, 3, Padding.Right, "")]
        public string RecordType
        {
            get { return ConfigurationManager.AppSettings["HeaderRecordType"].ToString(); }
        }

        [FlatFile(2, 8, Padding.Right, "yyyyMMdd")]
        public DateTime CreationDate { get; set; }

        [FlatFile(3, 6, Padding.Right, "HHmmss")]
        public DateTime CreationTime { get; set; }
    }

    public class Trailer
    {
        [FlatFile(1, 3, Padding.Right, "")]
        public string RecordType
        {
            get { return ConfigurationManager.AppSettings["TrailerRecordType"].ToString(); }
        }

        [FlatFile(2, 10, Padding.Left, "PADZERO")]
        public int DetailRecordCount { get; set; }

    }
}


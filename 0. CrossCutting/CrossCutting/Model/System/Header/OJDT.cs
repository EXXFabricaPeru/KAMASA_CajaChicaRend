using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail;
using VersionDLL.FlagElements.Attributes;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header
{
    [SystemTable(@"OJDT", FormType = "dummy")]
    public class OJDT
    {
        [SAPColumn(@"JdtNum")]
        public string JdtNum { get; set; }

        [SAPColumn(@"ReferenceDate")]
        public DateTime ReferenceDate { get; set; }
 
        [SAPColumn(@"DueDate")]
        public DateTime DueDate { get; set; }

        [SAPColumn(@"TaxDate")]
        public DateTime TaxDate { get; set; }

        [SAPColumn("TransCode")]
        public string TransactionCode { get; set; }

        [SAPColumn("Memo")]
        public string Memo { get; set; }

        [SAPColumn("ProjectCode")]
        public string ProjectCode { get; set; }


        [SAPColumn("Reference")]
        public string Reference { get; set; }

        [SAPColumn("Reference2")]
        public string Reference2 { get; set; }

        [SAPColumn("Reference3")]
        public string Reference3 { get; set; }


        [SAPColumn("JournalEntryLines")]
        public List<JDT1> JournalEntryLines { get; set; }
    }
}

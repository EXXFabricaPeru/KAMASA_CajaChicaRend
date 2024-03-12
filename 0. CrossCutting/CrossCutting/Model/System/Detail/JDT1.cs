using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes;
using System;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail
{
    [SystemTable("JDT1", FormType = "dummy")]
    public class JDT1
    {
        [SAPColumn("Account")]
        public string AccountCode { get; set; }

        [SAPColumn("Line_ID")]
        public int Line { get; set; }

        [SAPColumn("Debit")]
        public double Debit { get; set; }

        [SAPColumn("Credit")]
        public double Credit { get; set; }

        [SAPColumn("FCDebit")]
        public double FcDebit { get; set; }

        [SAPColumn("FCCredit")]
        public double FcCredit { get; set; }


        [SAPColumn("CreditSys")]
        public double? CreditSys { get; set; }


        [SAPColumn("FCCurrency")]
        public string FcCurrency { get; set; }

        [SAPColumn("DueDate")]
        public DateTime DueDate { get; set; }

        [SAPColumn("ShortName")]
        public string ShortName { get; set; }

        [SAPColumn("LineMemo")]
        public string LineMemo { get; set; }

        [SAPColumn("Reference1")]
        public string Reference1 { get; set; }

        [SAPColumn("Reference2")]
        public string Reference2 { get; set; }

        [SAPColumn("AdditionalReference")]
        public string AdditionalReference { get; set; }

        [SAPColumn("ProjectCode")]
        public string ProjectCode { get; set; }

        [SAPColumn("CostingCode")]
        public string CostingCode { get; set; }
        public int BPLID { get; set; }
    }
}

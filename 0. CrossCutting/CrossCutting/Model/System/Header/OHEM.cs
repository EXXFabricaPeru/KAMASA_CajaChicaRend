using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes;
using VersionDLL.FlagElements.Attributes;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header
{
    [SystemTable("OHEM", FormType = "dummy")]
    public class OHEM : BaseSAPTable
    {
        [SAPColumn("Code")]
        public string Code { get; set; }

        [SAPColumn("firstName")]
        public string FirstName { get; set; }

        [SAPColumn("lastName")]
        public string LastName { get; set; }
    }
}
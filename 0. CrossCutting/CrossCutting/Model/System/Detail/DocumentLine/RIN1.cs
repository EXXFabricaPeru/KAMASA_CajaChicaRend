using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine
{
    public class RIN1 : SAPDocumentLine
    {
        public int DocumentTypeReferenced { get; set; }
        public int DocumentEntryReferenced { get; set; }
        public int LineNumberReferenced { get; set; }
    }
}
using System;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document
{
    [Serializable]
    [SAPObject(BoObjectTypes.oCreditNotes)]
    public class ORIN : SAPDocument<RIN1>
    {
        public int ReferenceInvoiceDocumentEntry { get; set; }

        public bool IsInvoiceOrigin => ReferenceInvoiceDocumentEntry != default(int);
    }
}
using System;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document
{
    [Serializable]
    [SAPObject(BoObjectTypes.oPurchaseOrders)]
    public class OPOR : SAPDocument<POR1>
    {
        public bool IsItem { get; set; }

        public bool IsService { get; set; }

        public string DocumentTypeDescription { get; set; }

        public static class DocumentType
        {
            public const string BILL = "Boleta";
            public const string INVOICE = "Factura";
        }

    }
}
﻿// ReSharper disable InconsistentNaming

using System;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document
{
    [Serializable]
    [SAPObject(BoObjectTypes.oOrders)]
    public class ORDR : SAPDocument<RDR1>
    {
        public bool IsItem { get; set; }

        public bool IsService { get; set; }

        public string DocumentTypeDescription { get; set; }
        public string SaleChannel { get; set; }
        public string Region { get; set; }

        public static class DocumentType
        {
            public const string BILL = "Boleta";
            public const string INVOICE = "Factura";
        }
    }
}
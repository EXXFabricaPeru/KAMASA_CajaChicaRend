// ReSharper disable InconsistentNaming

using System;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document
{
    [Serializable]
    [SAPObject(BoObjectTypes.oInventoryTransferRequest)]
    public class OWTQ : SAPDocument<WTQ1>
    {
        [SAPColumn("Filler")]
        public string FromWareHouseCode { get; set; }

        [SAPColumn("ToWhsCode")]
        public string ToWareHouseCode { get; set; }
    }
}
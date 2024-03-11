using System;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document
{
    [Serializable]
    [SAPObject(BoObjectTypes.oPurchaseDeliveryNotes)]
    public class OPDN : SAPDocument<PDN1>
    {
        public object Naturaleza { get; set; }
        public object TipoDocumento { get; set; }
        public object SerieDocumento { get; set; }
        public object CorrelativoDocumento { get; set; }
    }
}
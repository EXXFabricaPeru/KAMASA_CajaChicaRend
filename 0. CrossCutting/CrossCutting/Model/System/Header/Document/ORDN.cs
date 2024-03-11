// ReSharper disable InconsistentNaming

using System;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document
{
    [Serializable]
    [SAPObject(BoObjectTypes.oReturns)]
    public class ORDN : SAPDocument<RDN1>
    {
        public object MotivoTrasladoSoraya { get; set; }
        public object TipoDocumento { get; set; }
    }
}
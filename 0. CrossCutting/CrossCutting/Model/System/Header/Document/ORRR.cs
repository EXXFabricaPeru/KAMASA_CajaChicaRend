// ReSharper disable InconsistentNaming

using System;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document
{
    [Serializable]
    [SAPObject(BoObjectTypes.oReturnRequest)]
    public class ORRR : SAPDocument<RRR1>
    {
    }
}
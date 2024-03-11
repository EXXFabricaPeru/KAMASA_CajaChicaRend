// ReSharper disable InconsistentNaming
using System;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine
{
    [Serializable]
    public class PRQ1 : SAPDocumentLine
    {
        public string ProviderCode { get; set; }
    }
}
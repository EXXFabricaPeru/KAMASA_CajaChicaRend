using System;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models
{
    [Serializable]
    public class SAPRelatedDocument
    {
        public int DocumentEntry { get; set; }
        public int SAPType { get; set; }
    }
}
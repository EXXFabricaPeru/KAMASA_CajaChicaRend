﻿// ReSharper disable InconsistentNaming
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header
{

    public class OADM : BaseSAPTable
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
    
        public ADM1 detailCompany { get; set; }

        public class ADM1
        {
            public string Ubigeo { get; set; }
        }
    }
}
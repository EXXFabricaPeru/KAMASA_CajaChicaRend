using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header
{
    [SystemTable(nameof(OVPM), FormType = "dummy")]
    public class OVPM : BaseSAPTable
    {
        public string TransId { get; set; }
        public string CardCode { get; set; }
        public string BpAct { get; set; }
        public string DocEntry { get; set; }
        public string DocTotal { get; set; }

        public string TrsfrAcct { get; set; }

        public string NroRendicion { get; set; }

        public DateTime DocDate { get; set; }

        public int BPLID { get; set; }

        //[SAPColumn(@"U_EXX_MPTRABAN", false)]
        public string MedioPagoTrans { get; set; }

        public string Comments { get; set; }

        public string CodFlujo { get; set; }

    }
}

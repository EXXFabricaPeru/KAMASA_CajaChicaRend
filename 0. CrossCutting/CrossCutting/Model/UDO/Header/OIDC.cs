using System;
using System.Collections.Generic;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Detail;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes;
using SAPbobsCOM;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header
{

    
    [Serializable]
    [SystemTable(@"OIDC", FormType = "dummy")]
    public class OIDC : BaseSAPTable
    {


        [SAPColumn(@"EXX_ORCR_VAL", false)]
        [FieldNoRelated("EXX_ORCR_VAL", "Addon RRCC", BoDbTypes.Alpha, Size = 2, Default = "N")]
        [Val("N", "No")]
        [Val("Y", "Yes")]
        public string ValidAddon { get; set; }

    }
}

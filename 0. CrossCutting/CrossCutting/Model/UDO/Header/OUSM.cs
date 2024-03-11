using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OUSM : BaseUDO
    {
        public const string ID = "VS_PD_OUSM";
        public const string DESCRIPTION = "Usuarios Moviles";

        [FormColumn(0, Description = "Identificador"), ColumnProperty("Code")]
        public string Code { get; set; }

        [FormColumn(1, Visible = false), ColumnProperty("Name")]
        public string Name { get; set; }

        [FormColumn(2), FieldNoRelated("U_EXK_ACTV", "Activo", BoDbTypes.Alpha, Size = 1)]
        [Val("Y", "Si"), Val("N", "No")]
        public string Active { get; set; }


        public class Status
        {
            public const string ACTIVE = "Y";
            public const string NO_ACTIVE = "N";
        }
    }
}

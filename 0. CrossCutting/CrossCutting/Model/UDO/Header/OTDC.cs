using System;
using System.Collections.Generic;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Detail;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;


namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = false, CanCancel = false)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]

    public class OTDC : BaseUDO
    {
        public const string ID = "EXX_RCCR_TDOC";
        public const string DESCRIPTION = "EXX_RCCR Tipo de Documento.";


        [FormColumn(0, Description = "Código"), ColumnProperty("Code"), FindColumn]
        public string SupplierId { get; set; }

        [FormColumn(1, Description = "Descripción"), ColumnProperty("Name"), FindColumn]
        public string Description { get; set; }

    }
}

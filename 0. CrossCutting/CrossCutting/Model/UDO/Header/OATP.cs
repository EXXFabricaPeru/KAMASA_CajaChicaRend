using System;
using System.Collections.Generic;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Detail;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_Document, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OATP : BaseUDO
    {
        public const string ID = "VS_PD_OATP";
        public const string DESCRIPTION = "Tarifario_Transportistas";

        [FormColumn(0, Description = "Código"), ColumnProperty("DocEntry"), FindColumn]
        public int DocumentEntry { get; set; }

        [FormColumn(1, Description = "Número"), ColumnProperty("DocNum")]
        public int DocumentNumber { get; set; }

        [FormColumn(2), FieldNoRelated("U_EXK_CDPR", "Código Transportista", BoDbTypes.Alpha, Size = 50), FindColumn]
        public string SupplierId { get; set; }

        [FormColumn(3), FieldNoRelated("U_EXK_DSPR", "Razón Social Transportista", BoDbTypes.Alpha, Size = 254)]
        public string SupplierName { get; set; }

        [ChildProperty(ATP1.ID)] public List<ATP1> RelatedTariffs { get; set; }

        [ChildProperty(ATP2.ID)] public List<ATP2> RelatedPenalties { get; set; }
    }
}
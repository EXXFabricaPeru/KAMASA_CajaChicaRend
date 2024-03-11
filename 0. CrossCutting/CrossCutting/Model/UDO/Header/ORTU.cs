using DisposableSAPBO.RuntimeMapper.Attributes;
using System;
using System.Collections.Generic;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Detail;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_Document, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = false, CanCancel = false)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class ORTU : BaseUDO
    {
        public const string ID = "VS_PD_ORTU";
        public const string DESCRIPTION = "Carga de Ordenes de Traslado";

        public ORTU()
        {
            ColumnDetail = new List<RTU1>();
        }

        [EnhancedColumn(1), FieldNoRelated("U_EXK_FILE", "Archivo de carga", BoDbTypes.Alpha, Size = 254)]
        public int FileName { get; set; }
        [EnhancedColumn(2), FieldNoRelated("U_EXK_DATE", "Fecha de carga", BoDbTypes.Date)]
        public int DocDate { get; set; }
        [EnhancedColumn(3), FieldNoRelated("U_EXK_FUPL", "Archivo Anexo", BoDbTypes.Link)]
        public string Archivo { get; set; }

        [ChildProperty(RTU1.ID)] public List<RTU1> ColumnDetail { get; set; }
    }
}
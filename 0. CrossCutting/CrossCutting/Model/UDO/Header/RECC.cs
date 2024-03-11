using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header
{
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = false, CanManageSeries = true, CanCancel = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class RECC : BaseUDO
    {
        public const string ID = "EXX_REDCAJA";
        public const string DESCRIPTION = "Rendiciones y Caja Chica";

        public RECC()
        {
            DetalleRendicion = new List<REC1>();
        }

        [FormColumn(0, Description = "Code"), ColumnProperty("Code"), FindColumn]
        public string SupplierId { get; set; }

        [FormColumn(1, Visible = false), ColumnProperty("Name"), FindColumn]
        public string Description { get; set; }

        [FormColumn(2), FieldNoRelated(@"U_EXX_TIPO", @"Tipo", BoDbTypes.Alpha, Size = 50)]
        public string Tipo { get; set; }


        [ChildProperty(REC1.ID)]
        public List<REC1> DetalleRendicion { get;  set; }
    }
}

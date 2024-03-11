using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;
namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [UDOFatherReference(ORCR.ID, 1)]
    public class RCR1
    {
        public const string ID = "EXX_RCCR_RCR1";
        public const string DESCRIPTION = "Detalle de comprobantes";

        [EnhancedColumn(Visible = false), ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn(1), FieldNoRelated("U_EXX_RCR1_CODP", "Cód. Proveedor", BoDbTypes.Alpha, Size = 150)]
        public string CodigoProveedor { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_EXX_RCR1_NOMP", "Nombre Proveedor", BoDbTypes.Alpha, Size = 254)]
        public string NombreProveedor { get; set; }

        [EnhancedColumn(3), FieldNoRelated("U_EXX_RCR1_FEDO", "Fecha Doc", BoDbTypes.Date, Size = 10)]
        public DateTime FechaDoc { get; set; }

        [EnhancedColumn(4), FieldNoRelated("U_EXX_RCR1_FECO", "Fecha Reg. Contable", BoDbTypes.Date, Size = 10)]
        public DateTime FechaRegContable { get; set; }

        [EnhancedColumn(5), FieldNoRelated("U_EXX_RCR1_FEVE", "Fecha Vencimiento", BoDbTypes.Date, Size = 10)]
        public DateTime FechaVencimiento { get; set; }

        [EnhancedColumn(6), FieldNoRelated("U_EXX_RCR1_COND", "Cond. Pago", BoDbTypes.Alpha, Size = 150)]
        public string CondicionPago { get; set; }

        [EnhancedColumn(7), FieldNoRelated("U_EXX_RCR1_TIDO", "Tipo Documento", BoDbTypes.Alpha, Size = 10)]
        public string TipoDocumento { get; set; }

        [EnhancedColumn(8), FieldNoRelated("U_EXX_RCR1_SERI", "Nro. Serie", BoDbTypes.Alpha, Size = 50)]
        public string NroSerie { get; set; }

        [EnhancedColumn(9), FieldNoRelated("U_EXX_RCR1_FOLI", "Nro. Folio", BoDbTypes.Alpha, Size = 50)]
        public string NroFolio { get; set; }

        [EnhancedColumn(10), FieldNoRelated("U_EXX_RCR1_CONC", "Concepto", BoDbTypes.Alpha, Size = 254)]
        public string Concepto { get; set; }

        [EnhancedColumn(11), FieldNoRelated("U_EXX_RCR1_CCTA", "Cuenta", BoDbTypes.Alpha, Size = 150)]
        public string Cuenta { get; set; }

        [EnhancedColumn(12), FieldNoRelated("U_EXX_RCR1_DIM1", "Dim 1 (Canal)", BoDbTypes.Alpha, Size = 150)]
        public string Dim1 { get; set; }

        [EnhancedColumn(13), FieldNoRelated("U_EXX_RCR1_DIM3", "Dim 3 (Centro de Costo)", BoDbTypes.Alpha, Size = 150)]
        public string Dim3 { get; set; }

        [EnhancedColumn(14), FieldNoRelated("U_EXX_RCR1_MONE", "Moneda", BoDbTypes.Alpha, Size = 10)]
        public string Moneda { get; set; }

        [EnhancedColumn(15), FieldNoRelated("U_EXX_RCR1_UNIT", "Valor Unit(Sin IGV)", BoDbTypes.Amount)]
        public string ValorUnitario { get; set; }

        [EnhancedColumn(16), FieldNoRelated("U_EXX_RCR1_IMPT", "Impuesto", BoDbTypes.Alpha, Size = 20)]
        public string Impuesto { get; set; }

        [EnhancedColumn(17), FieldNoRelated("U_EXX_RCR1_OIGV", "IGV", BoDbTypes.Amount)]
        public string IGV { get; set; }

        [EnhancedColumn(18), FieldNoRelated("U_EXX_RCR1_TOTA", "TOTAL", BoDbTypes.Alpha, Size = 20)]
        public string Total { get; set; }

        [EnhancedColumn(19), FieldNoRelated("U_EXX_RCR1_MIGR", "Migrado", BoDbTypes.Alpha, Size = 20)]
        public string Migrado { get; set; }

        [EnhancedColumn(20), FieldNoRelated("U_EXX_RCR1_DOCE", "Doc. Entry SAP", BoDbTypes.Alpha, Size = 20)]
        public string DocEntry { get; set; }
    }
}

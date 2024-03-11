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
    [UDOFatherReference(RECC.ID, 1)]
    public class REC1
    {
        public const string ID = "EXX_REDCAJA1";
        public const string DESCRIPTION = "Detalle";

        [EnhancedColumn(Visible = false), ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn(1), FieldNoRelated("U_EXX_CODIGO", "CodigoRendicion", BoDbTypes.Alpha, Size = 20)]
        public string CodigoRendicion { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_EXX_NRODOE", "NroDocEmpleado", BoDbTypes.Alpha, Size = 20)]
        public string NroDocEmpleado { get; set; }

        [EnhancedColumn(3), FieldNoRelated("U_EXX_NOMEMP", "NombreEmpleado", BoDbTypes.Date, Size = 254)]
        public string NombreEmpleado { get; set; }

        [EnhancedColumn(4), FieldNoRelated("U_EXX_ESTADO", "Inactivo", BoDbTypes.Alpha, Size = 10)]
        public string Inactivo { get; set; }

        [EnhancedColumn(5), FieldNoRelated("U_EXX_CODCUE", "CuentaContable", BoDbTypes.Alpha, Size = 20)]
        public string CuentaContable { get; set; }

        [EnhancedColumn(6), FieldNoRelated("U_EXX_NOMCUE", "NombreCuenta", BoDbTypes.Alpha, Size = 254)]
        public string NombreCuenta { get; set; }

        [EnhancedColumn(7), FieldNoRelated("U_EXX_DESCRIPCION", "Descripcion", BoDbTypes.Alpha, Size = 254)]
        public string Descripcion { get; set; }

        [EnhancedColumn(8), FieldNoRelated("U_EXX_FINICIO", "FechaInicio", BoDbTypes.Date, Size = 10)]
        public DateTime FechaInicio { get; set; }

        [EnhancedColumn(9), FieldNoRelated("U_EXX_FFIN", "FechaFin", BoDbTypes.Date, Size = 10)]
        public DateTime FechaFin { get; set; }

        [EnhancedColumn(10), FieldNoRelated("U_EXX_MONEDA", "Moneda", BoDbTypes.Alpha, Size = 10)]
        public string Moneda { get; set; }

        [EnhancedColumn(11), FieldNoRelated("U_EXX_MONCAJ", "Monto", BoDbTypes.Amount)]
        public double Monto { get; set; }


    }
}

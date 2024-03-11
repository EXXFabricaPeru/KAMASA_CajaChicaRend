using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class REN1
    {
        public const string ID = "VS_PD_REN1";
        public const string DESCRIPTION = "Vehículo Relacionado";

        [EnhancedColumn(Visible = false), ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_VHRF", "Codigo", BoDbTypes.Alpha, Size = 12)]
        public string ReferenceVehicle { get; set; }
    }
}

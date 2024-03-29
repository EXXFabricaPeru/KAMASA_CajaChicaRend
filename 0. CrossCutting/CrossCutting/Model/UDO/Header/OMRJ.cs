﻿using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header
{
    [UserDefinedTable(ID, DESCRIPTION)]
    public class OMRJ : BaseSAPTable
    {
        public const string ID = "VS_PD_OMRJ";
        public const string DESCRIPTION = "Tabla de código de rechazo";

        [EnhancedColumn(0), ColumnProperty("Code", ColumnName = "Código")]
        public string Code { get; set; }

        [EnhancedColumn(1), ColumnProperty("Name", ColumnName = "Descripción")]
        public string Name { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_EXK_DESC", "Descripción de rechazo", BoDbTypes.Alpha, Size = 254)]
        public string Descripcion { get; set; }
    }
}

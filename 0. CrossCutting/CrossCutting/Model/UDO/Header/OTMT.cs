﻿// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

using System.Reflection;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Constant;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Utilities;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header
{
    [UserDefinedTable(ID, DESCRIPTION)]
    public class OTMT : BaseSAPTable
    {
        public const string ID = "VS_PD_OTMT";
        public const string DESCRIPTION = "Tabla de motivos Tarifario";

        [EnhancedColumn(0), ColumnProperty(@"Code")]
        public string Code { get; set; }

        [EnhancedColumn(1), ColumnProperty(@"Name")]
        public string Name { get; set; }

        [EnhancedColumn(2), FieldNoRelated(@"U_EXK_TIPO", @"Tipo Motivo", BoDbTypes.Alpha, Size = 2)]
        [Val(MotiveType.EXTRA_CODE, MotiveType.EXTRA_DESCRIPTION)]
        [Val(MotiveType.PENALTY_CODE, MotiveType.PENALTY_DESCRIPTION)]
        public string Type { get; set; }

        public string TypeDescription
        {
            get
            {
                PropertyInfo property = GenericHelper.GetPropertyForExpression<OTMT, string>(t => t.Type);
                return GenericHelper.GetDescriptionFromValue(property, Type);
            }
        }
    }
}
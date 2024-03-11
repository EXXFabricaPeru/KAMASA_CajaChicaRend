// ReSharper disable InconsistentNaming
// ReSharper disable ExplicitCallerInfoArgument
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantArgumentDefaultValue

using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header
{
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OMTT : BaseUDO
    {
        public const string ID = "VS_PD_OMTT";
        public const string DESCRIPTION = "Codificacion_Motivos";

        [FormColumn(0, Description = @"Código del Motivo"), ColumnProperty("Code")]
        public string Code { get; set; }

        [FormColumn(1, Description = @"Descripción del Motivo"), ColumnProperty("Name")]
        public string Name { get; set; }

        [FormColumn(3), FieldNoRelated(@"U_EXK_COTP", @"Tipo de Codificación", BoDbTypes.Alpha, Size = 2)]
        [Val(Types.APPROVED, @"Aprobación"), Val(Types.DISAPPROVED, @"Desaprobación"), Val(Types.POSTPONED, @"Pospuesto"), Val(Types.OPENING, @"Apertura")]
        public string Type { get; set; }

        public static class Types
        {
            public const string APPROVED = "AP";
            public const string DISAPPROVED = "DS";
            public const string POSTPONED = "PS";
            public const string OPENING = "OP";
        }
    }
}

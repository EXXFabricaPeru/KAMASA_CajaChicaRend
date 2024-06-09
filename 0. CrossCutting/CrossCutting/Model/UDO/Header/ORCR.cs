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
    public class ORCR : BaseUDO
    {
        public const string ID = "EXX_RCCR_ORCR"; 
        public const string DESCRIPTION = "REGISTRO_COMPROBANTES_CC_RR";

        public ORCR()
        {
            DetalleComprobantes = new List<RCR1>();
        }

        [FormColumn(0, Description = "Doc."), ColumnProperty("Code"), FindColumn]
        public string SupplierId { get; set; }

        [FormColumn(1, Visible = false), ColumnProperty("Name"), FindColumn]
        public string Description { get; set; }

        [FormColumn(2), FieldNoRelated(@"U_EXX_ORCR_TIPO", @"Tipo", BoDbTypes.Alpha, Size = 50)]
        public string Tipo { get; set; }

        [FormColumn(3), FieldNoRelated(@"U_EXX_ORCR_NRCR", @"Nro. CC/REND", BoDbTypes.Alpha, Size = 100)]
        public string NroCCREND { get; set; }

        [FormColumn(4), FieldNoRelated(@"U_EXX_ORCR_DESC", @"Desc. Rendición", BoDbTypes.Alpha, Size = 200)]
        public string Descripcion { get; set; }

        [FormColumn(5), FieldNoRelated(@"U_EXX_ORCR_EMPL", @"Empleado", BoDbTypes.Alpha, Size = 200)]
        public string Empleado { get; set; }

        [FormColumn(6), FieldNoRelated(@"U_EXX_ORCR_MONT", @"Monto Rendición / CC", BoDbTypes.Amount)]
        public double MontoRendicionCC { get; set; }

        [FormColumn(7), FieldNoRelated(@"U_EXX_ORCR_TOGA", @"Total Gastos Registrados", BoDbTypes.Amount)]
        public double TotalGastosRegistrados { get; set; }

        [FormColumn(8), FieldNoRelated(@"U_EXX_ORCR_SALD", @"Saldo", BoDbTypes.Amount)]
        public double Saldo { get; set; }

        [FormColumn(9), FieldNoRelated(@"U_EXX_ORCR_SUCR", @"Sucursal", BoDbTypes.Alpha, Size = 150)]
        public string Sucursal { get; set; }

        [FormColumn(10), FieldNoRelated(@"U_EXX_ORCR_STAD", @"Estado", BoDbTypes.Alpha, Size = 2, Default = "O")]
        [Val("O", "Abierto")]
        [Val("L", "Liquidado")]
        [Val("C", "Cancelado")]
        public string Estado { get; set; }

        [FormColumn(11), FieldNoRelated(@"U_EXX_ORCR_MEDP", @"Medio Pago", BoDbTypes.Alpha, Size = 100)]
        public string MedioPago { get; set; }

        [FormColumn(12), FieldNoRelated(@"U_EXX_ORCR_CTAC", @"Cta. Contable", BoDbTypes.Alpha, Size = 150)]
        public string CuentaContable { get; set; }

        [FormColumn(13), FieldNoRelated(@"U_EXX_ORCR_FECH", @"Fecha", BoDbTypes.Date, Size = 10)]
        public DateTime Fecha { get; set; }

        [FormColumn(14), FieldNoRelated(@"U_EXX_ORCR_REFE", @"Referencia", BoDbTypes.Alpha, Size = 200)]
        public string Referencia { get; set; }

        [FormColumn(15), FieldNoRelated(@"U_EXX_ORCR_DEVO", @"Devolución" , BoDbTypes.Amount)]
        public string Devolucion { get; set; }

        [FormColumn(16), FieldNoRelated(@"U_EXX_ORCR_CLIQ", @"Codigo Liquidación", BoDbTypes.Alpha, Size = 100)]
        public string CodLiquidacion { get; set; }

        [FormColumn(17), FieldNoRelated(@"U_EXX_ORCR_CDEV", @"Código Devolución", BoDbTypes.Alpha, Size = 100)]
        public string CodDevolucion { get; set; }

        [FormColumn(18), FieldNoRelated(@"U_EXX_ORCR_CREM", @"Código Reembolso", BoDbTypes.Alpha, Size = 100)]
        public string CodReembolso { get; set; }

        [FormColumn(19), FieldNoRelated(@"U_EXX_ORCR_REEM", @"Reembolso", BoDbTypes.Amount)]
        public string Reembolso { get; set; }

        [FormColumn(20), FieldNoRelated(@"U_EXX_ORCR_CRCC", @"Código Reembolso CC", BoDbTypes.Alpha, Size = 100)]
        public string CodReembolsoCajaChica { get; set; }

        [FormColumn(21), FieldNoRelated(@"U_EXX_ORCR_CCSG", @"CCH Siguiente", BoDbTypes.Alpha, Size = 120)]
        public string CodCajaChicaSiguiente { get; set; }

        [FormColumn(22), FieldNoRelated(@"U_EXX_ORCR_FLJC", @"Flujo de Caja", BoDbTypes.Alpha, Size = 120)]
        public string CodFlujodeCaja { get; set; }

        [ChildProperty(RCR1.ID)]
        public List<RCR1> DetalleComprobantes { get; set; }
    }
}

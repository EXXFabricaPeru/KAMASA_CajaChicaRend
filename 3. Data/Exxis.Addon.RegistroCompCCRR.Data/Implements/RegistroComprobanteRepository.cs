using B1SLayer;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.Local;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.ServiceLayer.Detail;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.ServiceLayer.Header;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Utilities;
using Exxis.Addon.RegistroCompCCRR.Data.Code;
using Exxis.Addon.RegistroCompCCRR.Data.Repository;
using Sap.Data.Hana;
using System.IO;
using System.Diagnostics;
using System.Data;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail;

namespace Exxis.Addon.RegistroCompCCRR.Data.Implements
{
    public class RegistroComprobanteRepository : BaseRegistroComprobanteRepository
    {
        public SLConnection Login;
        public RegistroComprobanteRepository(Company company) : base(company)
        {

            ServiceLayerHelper serviceLayerHelper = new ServiceLayerHelper();

            //var _company = company.CompanyDB;
            //var user = company.UserName;
            //var pass = "B1Admin$$";
            //Login = serviceLayerHelper.Login(company, _company, user, pass);

        }



        public override void ActualizarEstadoLinea(string code, string line, string estado, string docentry)
        {
            try
            {

                //var list= TiendasList(Login);
                var list = line.Split('-');

                //var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                //var query = " UPDATE\"@EXX_RCCR_RCR1\" SET \"U_EXX_RCR1_MIGR\"='{0}', \"U_EXX_RCR1_DOCE\"='{1}'  " +
                //    "WHERE \"Code\"='{2}' AND \"U_EXX_RCR1_CODP\"='{3}' AND \"U_EXX_RCR1_SERI\"='{4}' AND \"U_EXX_RCR1_FOLI\"='{5}' ";

                //var update = string.Format(query, estado, docentry, code, list[0], list[1], list[2]);
                //recordSet.DoQuery(update);


                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(ORCR.ID);
                var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("Code", code);
                GeneralData generalData = generalService.GetByParams(generalDataParams);

                GeneralDataCollection dataCollection = generalData.Child(RCR1.ID);

                for (int i = 0; i < dataCollection.Count; i++)
                {
                    var childLine = dataCollection.Item(i);
                    var codProv = childLine.GetProperty("U_EXX_RCR1_CODP").ToString();
                    var serie = childLine.GetProperty("U_EXX_RCR1_SERI").ToString();
                    var folio = childLine.GetProperty("U_EXX_RCR1_FOLI").ToString();
                    if (codProv == list[0] && serie == list[1] && folio == list[2])
                    {
                        childLine.SetProperty("U_EXX_RCR1_MIGR", estado);
                        childLine.SetProperty("U_EXX_RCR1_DOCE", docentry);
                    }

                }

                generalService.Update(generalData);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
            }
            //catch (Exception exception)
            //{
            //    if (Company.InTransaction)
            //        Company.EndTransaction(BoWfTransOpt.wf_RollBack);

            //    //return Tuple.Create(false, exception.Message);
            //}
            finally
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override Tuple<bool, string, OJDT> GenerarAsiento(OJDT asientoRecon)
        {
            OJDT asiento = null;
            try
            {

                Company.StartTransaction();
                var document = Company.GetBusinessObject(BoObjectTypes.oJournalEntries).To<JournalEntries>();

                document.TransactionCode = asientoRecon.TransactionCode;
                document.DueDate = asientoRecon.DueDate;
                document.ReferenceDate = asientoRecon.ReferenceDate;
                document.TaxDate = asientoRecon.TaxDate;
                document.Memo = asientoRecon.Memo;
                document.Reference2 = asientoRecon.Reference2;

                JournalEntries_Lines documentLines = document.Lines;
                asientoRecon.JournalEntryLines.ForEach((line, index, lastIteration) =>
                {
                    documentLines.AccountCode = line.AccountCode;
                    documentLines.BPLID = line.BPLID;
                    documentLines.ShortName = line.ShortName;
                    documentLines.Credit = line.Credit;
                    documentLines.Debit = line.Debit;
                    documentLines.LineMemo = line.LineMemo;
                    documentLines.Reference2 = line.Reference2;

                    if (line.idflujo > 0)
                    {
                        documentLines.PrimaryFormItems.CashFlowLineItemID = line.idflujo;
                        documentLines.PrimaryFormItems.AmountLC = line.Debit > 0 ? line.Debit : line.Credit;
                    }
                      


                    lastIteration.IfFalse(() => documentLines.Add());
                });




                int operationResult = document.Add();
                if (operationResult.IsDefault())
                {
                    string key;
                    Company.GetNewObjectCode(out key);
                    if (Company.InTransaction)
                        Company.EndTransaction(BoWfTransOpt.wf_Commit);

                    asiento = RetriveAsientoByCode(key, asientoRecon);
                    return Tuple.Create(true, key, asiento);
                }

                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                return Tuple.Create(false, Company.GetLastErrorDescription(), asiento);
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message, asiento);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public OJDT RetriveAsientoByCode(string code, OJDT asiento)
        {
            try
            {

                //var list= TiendasList(Login);
                OJDT refAsiento = new OJDT();

                refAsiento.TransactionCode = asiento.TransactionCode;
                refAsiento.TransId = code;

                List<JDT1> lines = new List<JDT1>();
                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = " select * from JDT1 where \"TransId\"={0} ";
                recordSet.DoQuery(string.Format(query, code));

                while (!recordSet.EoF)
                {
                    JDT1 line = new JDT1();
                    line.AccountCode = recordSet.GetColumnValue("Account").ToString();
                    line.Line = recordSet.GetColumnValue("Line_ID").ToString().ToInt32();
                    line.Debit = recordSet.GetColumnValue("Debit").ToString().ToDouble();
                    line.Credit = recordSet.GetColumnValue("Credit").ToString().ToDouble();
                    line.ShortName = recordSet.GetColumnValue("ShortName").ToString();
                    line.LineMemo = recordSet.GetColumnValue("LineMemo").ToString();

                    lines.Add(line);
                    recordSet.MoveNext();
                }
                refAsiento.JournalEntryLines = lines;

                return refAsiento;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override Tuple<bool, string> GenerarReconciliacion(OITR reconciliacion)
        {
            try
            {
                Company.StartTransaction();
                //var document = Company.GetBusinessObject(BoObjectTypes.oAccountSegmentationCategories).To<Documents>();
                InternalReconciliationsService service = (InternalReconciliationsService)Company.GetCompanyService().GetBusinessService(ServiceTypes.InternalReconciliationsService);
                InternalReconciliationOpenTrans openTrans = (InternalReconciliationOpenTrans)service.GetDataInterface(InternalReconciliationsServiceDataInterfaces.irsInternalReconciliationOpenTrans);
                InternalReconciliationParams reconParams = (InternalReconciliationParams)service.GetDataInterface(InternalReconciliationsServiceDataInterfaces.irsInternalReconciliationParams);

                openTrans.CardOrAccount = reconciliacion.CardOrAccount == "C" ? CardOrAccountEnum.coaCard : CardOrAccountEnum.coaAccount;
                openTrans.ReconDate = reconciliacion.ReconDate;

                int cont = 0;

                foreach (var item in reconciliacion.InternalReconciliationOpenTransRows)
                {
                    openTrans.InternalReconciliationOpenTransRows.Add();
                    openTrans.InternalReconciliationOpenTransRows.Item(cont).Selected = BoYesNoEnum.tYES;
                    openTrans.InternalReconciliationOpenTransRows.Item(cont).TransId = item.TransId.ToInt32();
                    openTrans.InternalReconciliationOpenTransRows.Item(cont).TransRowId = item.TransRowId.ToInt32();
                    openTrans.InternalReconciliationOpenTransRows.Item(cont).ReconcileAmount = item.ReconcileAmount.ToDouble();
                    cont++;
                }
                //openTrans.InternalReconciliationOpenTransRows.Add();
                //openTrans.InternalReconciliationOpenTransRows.Item(0).Selected = BoYesNoEnum.tYES;
                //openTrans.InternalReconciliationOpenTransRows.Item(0).TransId = 41;
                //openTrans.InternalReconciliationOpenTransRows.Item(0).TransRowId = 1;
                //openTrans.InternalReconciliationOpenTransRows.Item(0).ReconcileAmount = 10;
                //openTrans.InternalReconciliationOpenTransRows.Add();
                //openTrans.InternalReconciliationOpenTransRows.Item(1).Selected = BoYesNoEnum.tYES;
                //openTrans.InternalReconciliationOpenTransRows.Item(1).TransId = 43;
                //openTrans.InternalReconciliationOpenTransRows.Item(1).TransRowId = 0;
                //openTrans.InternalReconciliationOpenTransRows.Item(1).ReconcileAmount = 10;



                reconParams = service.Add(openTrans);

                if (reconParams.ReconNum != 0)
                {
                    if (Company.InTransaction)
                        Company.EndTransaction(BoWfTransOpt.wf_Commit);

                    return Tuple.Create(true, reconParams.ReconNum.ToString());
                }
                else
                {
                    return Tuple.Create(false, "Error al generar la reonciliación");
                }




                return Tuple.Create(false, "");
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }
        public override void ActualizarEstadoRegistroRendicion(string codigo, string estado)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(ORCR.ID);
                var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("Code", codigo);
                GeneralData generalData = generalService.GetByParams(generalDataParams);

                generalData.SetProperty("U_EXX_ORCR_STAD", estado);


                generalService.Update(generalData);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
                //return Tuple.Create(true, string.Empty);
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                //return Tuple.Create(false, exception.Message);
            }
        }

        public override void ActualizarObjectRRCC(string codigo, ORCR document)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(ORCR.ID);
                var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("Code", codigo);
                GeneralData generalData = generalService.GetByParams(generalDataParams);

                if (!string.IsNullOrEmpty(document.CodDevolucion))
                    generalData.SetProperty("U_EXX_ORCR_CDEV", document.CodDevolucion);

                if (!string.IsNullOrEmpty(document.CodReembolsoCajaChica))
                    generalData.SetProperty("U_EXX_ORCR_CRCC", document.CodReembolsoCajaChica);

                if (!string.IsNullOrEmpty(document.CodReembolso))
                    generalData.SetProperty("U_EXX_ORCR_CREM", document.CodReembolso);

                if (!string.IsNullOrEmpty(document.CodLiquidacion))
                    generalData.SetProperty("U_EXX_ORCR_CLIQ", document.CodLiquidacion);


                generalService.Update(generalData);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
                //return Tuple.Create(true, string.Empty);
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                //return Tuple.Create(false, exception.Message);
            }
        }

        public override string RetrieveCodigoGenerado()
        {
            try
            {

                //var list= TiendasList(Login);

                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = " select LPAD(Count(*) + 1, 5, '0') as \"count\" from \"@EXX_RCCR_ORCR\" ";
                recordSet.DoQuery(string.Format(query));

                while (!recordSet.EoF)
                {
                    var Code = DateTime.Now.Year + "-" + recordSet.GetColumnValue("count").ToString();
                    return Code;
                    recordSet.MoveNext();
                }

                return "";
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override Tuple<bool, OVPM> RetrievePagoByRendicion(string rendicion)
        {
            try
            {

                //var list= TiendasList(Login);

                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = "Select * from \"OVPM\" V" +
                          "   where V.\"U_EXX_NUMEREND\" = '{0}' and \"Canceled\" = 'N' ";

                recordSet.DoQuery(string.Format(query, rendicion));
                OVPM pago = new OVPM();

                while (!recordSet.EoF)
                {
                    pago.TransId = recordSet.GetColumnValue("TransId").ToString();
                    pago.DocEntry = recordSet.GetColumnValue("DocEntry").ToString();
                    pago.BpAct = recordSet.GetColumnValue("BpAct").ToString();
                    pago.CardCode = recordSet.GetColumnValue("CardCode").ToString();
                    pago.DocTotal = recordSet.GetColumnValue("DocTotal").ToString();
                    pago.TrsfrAcct = recordSet.GetColumnValue("TrsfrAcct").ToString();
                    pago.DocDate = recordSet.GetColumnValue("DocDate").ToDateTime();
                    pago.NroRendicion = recordSet.GetColumnValue("U_EXX_NUMEREND").ToString();
                    pago.BPLID = recordSet.GetColumnValue("BPLId").ToInt32();

                    return Tuple.Create(true, pago);
                    recordSet.MoveNext();
                }

                return Tuple.Create(false, pago);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override REC1 RetrieveRendicionByCode(string documentEntry)
        {
            REC1 line = new REC1();

            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            var query = "select * from \"@EXX_REDCAJA1\" where  IFNULL(\"U_EXX_CODIGO\",'')='{0}'";
            recordSet.DoQuery(string.Format(query, documentEntry));
            //IList<Tuple<string, string>> result = new List<Tuple<string, string>>();
            while (!recordSet.EoF)
            {

                line.CodigoRendicion = recordSet.GetColumnValue("U_EXX_CODIGO")?.ToString();
                line.NroDocEmpleado = recordSet.GetColumnValue("U_EXX_NRODOE")?.ToString();
                line.Descripcion = recordSet.GetColumnValue("U_EXX_DESCRIPCION")?.ToString();
                line.NombreEmpleado = recordSet.GetColumnValue("U_EXX_NOMEMP")?.ToString();
                line.Monto = recordSet.GetColumnValue("U_EXX_MONCAJ").ToDouble();

                recordSet.MoveNext();
            }

            return line;
        }

        public override IEnumerable<REC1> RetrieveRendicionesActivas(string tipoRendicion)
        {
            List<REC1> list = new List<REC1>();

            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            var query = "";

            query = "select \"U_EXX_ORCR_STAD\", OV.\"DocTotal\" as \"TotalPago\", R1.* from \"@EXX_REDCAJA1\" R1 JOIN \"@EXX_REDCAJA\" R ON R.\"Code\"=R1.\"Code\" " +
                            " LEFT JOIN \"@EXX_RCCR_ORCR\" OC ON OC.\"U_EXX_ORCR_NRCR\"=R1.\"U_EXX_CODIGO\" " +
                            "  JOIN \"OVPM\" OV ON OV.\"U_EXX_NUMEREND\" = R1.\"U_EXX_CODIGO\"    " +
                            " where R1.\"U_EXX_ESTADO\"='N' and IFNULL(R1.\"U_EXX_CODIGO\",'') <>'' and R.\"U_EXX_TIPO\"='{0}'" +
                            " and IFNULL(\"U_EXX_ORCR_STAD\",'')<>'L' and IFNULL(OC.\"Canceled\",'N')='N'  ";





            recordSet.DoQuery(string.Format(query, tipoRendicion));
            IList<Tuple<string, string>> result = new List<Tuple<string, string>>();
            while (!recordSet.EoF)
            {
                REC1 line = new REC1();
                line.CodigoRendicion = recordSet.GetColumnValue("U_EXX_CODIGO").ToString();
                line.NroDocEmpleado = recordSet.GetColumnValue("U_EXX_NRODOE")?.ToString();
                line.Descripcion = recordSet.GetColumnValue("U_EXX_DESCRIPCION")?.ToString();
                line.NombreEmpleado = recordSet.GetColumnValue("U_EXX_NOMEMP")?.ToString();
                line.Monto = recordSet.GetColumnValue("TotalPago").ToDouble();
                line.FechaInicio = recordSet.GetColumnValue("U_EXX_FINICIO").ToDateTime();
                line.FechaFin = recordSet.GetColumnValue("U_EXX_FFIN").ToDateTime();
                list.Add(line);
                recordSet.MoveNext();
            }

            return list;
        }

        public override IEnumerable<Tuple<string, string>> RetrieveSucursales()
        {
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery("select  * from \"OBPL\" where \"Disabled\" = 'N' ");
            IList<Tuple<string, string>> result = new List<Tuple<string, string>>();
            while (!recordSet.EoF)
            {
                var code = recordSet.GetColumnValue("BPLId").ToString();
                var description = recordSet.GetColumnValue("BPLName").ToString();
                result.Add(Tuple.Create(code, description));
                recordSet.MoveNext();
            }

            return result;
        }

        public override OJDT RetriveAsientoByCode(object asientoRecon)
        {
            throw new NotImplementedException();
        }

        public override Tuple<bool, string, string> RetrieveGastoByCode(string codServicioCompra)
        {
            try
            {

                //var list= TiendasList(Login);

                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = " select  GD.\"Code\",GD.\"U_EXX_MONMIN\"  from \"@EXX_SERCOM\" S JOIN \"@EXX_GRUDET\" GD ON GD.\"Code\"=S.\"U_EXX_GRUDET\" where S.\"Code\"='{0}' ";
                recordSet.DoQuery(string.Format(query, codServicioCompra));

                while (!recordSet.EoF)
                {
                    var grupoDet = recordSet.GetColumnValue("Code").ToString();
                    var montoMinimo = recordSet.GetColumnValue("U_EXX_MONMIN").ToString();
                    return Tuple.Create(true, grupoDet, montoMinimo);
                    recordSet.MoveNext();
                }

                return Tuple.Create(false, "No se encontró registro del servicio", "");
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override Tuple<bool, ORCT> GenerarPagoRecibido(OVPM document)
        {
            string DocEntry = "", numeracion = "";
            ORCT pago = new ORCT();
            try
            {
                Payments documentValid;
                documentValid = (SAPbobsCOM.Payments)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);

                documentValid.DocDate = document.DocDate;
                //documentValid.DocDueDate = document.DocDate;
                documentValid.TaxDate = document.DocDate;
                documentValid.CardCode = document.CardCode;
                //documentValid.JournalMemo = document.CardName;
                documentValid.UserFields.Fields.Item("U_EXX_NUMEREND").Value = document.NroRendicion;
                documentValid.UserFields.Fields.Item("U_EXX_MPTRABAN").Value = document.MedioPagoTrans;

                documentValid.Remarks = document.Comments;
                int cont = 0;

                documentValid.TransferSum = document.DocTotal.ToDouble();
                documentValid.TransferAccount = document.TrsfrAcct;
                documentValid.BPLID = document.BPLID;
                documentValid.IsPayToBank = BoYesNoEnum.tYES;
                documentValid.ControlAccount = document.BpAct;
                documentValid.DocType = BoRcptTypes.rSupplier;
                //foreach (var item in document.DocumentLines)
                //{
                //    //documentValid.Lines.ItemCode = item.ItemCode;
                //    documentValid.Lines.ItemDescription = item.ItemDescription;
                //    //documentValid.Lines.BaseEntry = deliveryEntry;
                //    //documentValid.Lines.BaseLine = cont;
                //    documentValid.Lines.Quantity = Decimal.ToDouble(item.Quantity);
                //    //documentValid.Lines.BaseType = 15;
                //    documentValid.Lines.UnitPrice = Decimal.ToDouble(item.UnitPrice);
                //    documentValid.Lines.LineTotal = Decimal.ToDouble(item.TotalPrice);
                //    documentValid.Lines.UserFields.Fields.Item("U_BPP_OPER").Value = "E";
                //    documentValid.Lines.AccountCode = "6311101";
                //    documentValid.Lines.UserFields.Fields.Item("U_tipoOpT12").Value = "99";
                //    documentValid.Lines.UserFields.Fields.Item("U_VS_TIPAFE").Value = "10";
                //    documentValid.Lines.TaxCode = item.TaxCode;
                //    documentValid.Lines.Add();
                //    cont++;
                //}

                //documentValid.UserFields.Fields.Item("U_VS_USRSV").Value = "N";

                int res = documentValid.Add();
                if (res != 0) // Check the result
                {
                    string error;
                    string vm_GetLastErrorDescription_string = Company.GetLastErrorDescription();
                    //return Tuple.Create(false, vm_GetLastErrorDescription_string);
                    throw new Exception(vm_GetLastErrorDescription_string);
                    //Company.GetLastError(out res, out error);                 
                }
                else
                {
                    DocEntry = Company.GetNewObjectKey();
                    pago = RetrievePagoByDocEntry(DocEntry);
                    return Tuple.Create(true, pago);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Tuple.Create(true, pago);
        }

        private ORCT RetrievePagoByDocEntry(string docEntry)
        {
            try
            {

                //var list= TiendasList(Login);

                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = "Select * from \"ORCT\" V" +
                          "   where V.\"DocEntry\" = {0} and \"Canceled\" = 'N' ";

                recordSet.DoQuery(string.Format(query, docEntry));
                ORCT pago = new ORCT();

                while (!recordSet.EoF)
                {
                    pago.TransId = recordSet.GetColumnValue("TransId").ToString();
                    pago.DocEntry = recordSet.GetColumnValue("DocEntry").ToString();
                    pago.BpAct = recordSet.GetColumnValue("BpAct").ToString();
                    pago.CardCode = recordSet.GetColumnValue("CardCode").ToString();
                    pago.DocTotal = recordSet.GetColumnValue("DocTotal").ToString();
                    pago.TrsfrAcct = recordSet.GetColumnValue("TrsfrAcct").ToString();
                    pago.DocDate = recordSet.GetColumnValue("DocDate").ToDateTime();
                    pago.NroRendicion = recordSet.GetColumnValue("U_EXX_NUMEREND").ToString();
                    pago.BPLID = recordSet.GetColumnValue("BPLId").ToInt32();


                    return pago;
                    recordSet.MoveNext();
                }

                return pago;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override Tuple<bool, OVPM> GenerarPagoEfectuado(OVPM document)
        {
            string DocEntry = "", numeracion = "";
            OVPM pago = new OVPM();
            try
            {
                Payments documentValid;
                documentValid = (SAPbobsCOM.Payments)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments);

                documentValid.DocDate = document.DocDate;
                //documentValid.DocDueDate = document.DocDate;
                documentValid.TaxDate = document.DocDate;
                documentValid.CardCode = document.CardCode;
                //documentValid.JournalMemo = document.CardName;
                //documentValid.UserFields.Fields.Item("U_EXX_NUMEREND").Value = document.NroRendicion;
                documentValid.UserFields.Fields.Item("U_EXX_MPTRABAN").Value = document.MedioPagoTrans;

                documentValid.Remarks = document.NroRendicion + "-" + document.Comments;

                int cont = 0;

                documentValid.TransferSum = document.DocTotal.ToDouble();
                documentValid.TransferAccount = document.TrsfrAcct;
                documentValid.BPLID = document.BPLID;
                //documentValid.IsPayToBank = BoYesNoEnum.tYES;
                documentValid.ControlAccount = document.BpAct;
                documentValid.DocType = BoRcptTypes.rSupplier;


                int res = documentValid.Add();
                if (res != 0) // Check the result
                {
                    string error;
                    string vm_GetLastErrorDescription_string = Company.GetLastErrorDescription();
                    //return Tuple.Create(false, vm_GetLastErrorDescription_string);
                    throw new Exception(vm_GetLastErrorDescription_string);
                    //Company.GetLastError(out res, out error);                 
                }
                else
                {
                    DocEntry = Company.GetNewObjectKey();
                    pago = RetrievePagoEfectuadoByDocEntry(DocEntry);
                    return Tuple.Create(true, pago);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Tuple.Create(true, pago);
        }

        private OVPM RetrievePagoEfectuadoByDocEntry(string docEntry)
        {
            try
            {

                //var list= TiendasList(Login);

                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = "Select * from \"OVPM\" V" +
                          "   where V.\"DocEntry\" = {0} and \"Canceled\" = 'N' ";

                recordSet.DoQuery(string.Format(query, docEntry));
                OVPM pago = new OVPM();

                while (!recordSet.EoF)
                {
                    pago.TransId = recordSet.GetColumnValue("TransId").ToString();
                    pago.DocEntry = recordSet.GetColumnValue("DocEntry").ToString();
                    pago.BpAct = recordSet.GetColumnValue("BpAct").ToString();
                    pago.CardCode = recordSet.GetColumnValue("CardCode").ToString();
                    pago.DocTotal = recordSet.GetColumnValue("DocTotal").ToString();
                    pago.TrsfrAcct = recordSet.GetColumnValue("TrsfrAcct").ToString();
                    pago.DocDate = recordSet.GetColumnValue("DocDate").ToDateTime();
                    pago.NroRendicion = recordSet.GetColumnValue("U_EXX_NUMEREND")?.ToString();
                    pago.BPLID = recordSet.GetColumnValue("BPLId").ToInt32();

                    return pago;
                    recordSet.MoveNext();
                }

                return pago;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override double GetTipoCambio(DateTime fecha, string currency)
        {
            try
            {
                SBObob oSBObob = (SBObob)Company.GetBusinessObject(BoObjectTypes.BoBridge);
                if (currency == "SOL")
                    return 1.0;

                DateTime exchangeDate = fecha; // Fecha del tipo de cambio (en este caso, la fecha actual)
                var exchangeRate = oSBObob.GetCurrencyRate(currency, exchangeDate);
                while (!exchangeRate.EoF)
                {

                    var x = exchangeRate.Fields.Item(0);

                    return x.Value.ToDouble();
                    //Console.WriteLine("Tipo de cambio para " + currencyCode + " en la fecha " + exchangeDate.ToShortDateString() + ": " + exchangeRate);
                    exchangeRate.MoveNext();
                }
                return 0;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public override IEnumerable<Tuple<string, string>> RetrieveMedioPago()
        {
            {
                var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
                recordSet.DoQuery("select  * from \"@EXX_MEDPAG\"");
                IList<Tuple<string, string>> result = new List<Tuple<string, string>>();
                while (!recordSet.EoF)
                {
                    var code = recordSet.GetColumnValue("Code").ToString();
                    var description = recordSet.GetColumnValue("Name").ToString();
                    result.Add(Tuple.Create(code, description));
                    recordSet.MoveNext();
                }

                return result;
            }
        }
    }
}

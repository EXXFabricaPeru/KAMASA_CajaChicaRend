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

                JournalEntries_Lines documentLines = document.Lines;
                asientoRecon.JournalEntryLines.ForEach((line, index, lastIteration) =>
                {
                    documentLines.AccountCode = line.AccountCode;
                    documentLines.BPLID = line.BPLID;
                    documentLines.ShortName = line.ShortName;
                    documentLines.Credit = line.Credit;
                    documentLines.Debit = line.Debit;
                    documentLines.LineMemo = line.LineMemo;


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
                recordSet.DoQuery(string.Format(query,code));

                while (!recordSet.EoF)
                {
                    JDT1 line = new JDT1();
                    line.AccountCode=  recordSet.GetColumnValue("Account").ToString();
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

                openTrans.CardOrAccount = reconciliacion.CardOrAccount=="C"? CardOrAccountEnum.coaCard:CardOrAccountEnum.coaAccount;
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

                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_Commit);



                return Tuple.Create(false, "");
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
                OVPM pago = new OVPM()
;                while (!recordSet.EoF)
                {
                    pago.TransId =  recordSet.GetColumnValue("TransId").ToString();
                    pago.DocEntry = recordSet.GetColumnValue("DocEntry").ToString();
                    pago.BpAct = recordSet.GetColumnValue("BpAct").ToString();
                    pago.CardCode = recordSet.GetColumnValue("CardCode").ToString();
                    pago.DocTotal = recordSet.GetColumnValue("DocTotal").ToString();
                    pago.TrsfrAcct = recordSet.GetColumnValue("TrsfrAcct").ToString();


                    return Tuple.Create(true,pago);
                    recordSet.MoveNext();
                }

                return  Tuple.Create(false, pago);
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
            recordSet.DoQuery(string.Format(query,documentEntry));
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
            var query = "select R1.* from \"@EXX_REDCAJA1\" R1 JOIN \"@EXX_REDCAJA\" R ON R.\"Code\"=R1.\"Code\" " +
                "where R1.\"U_EXX_ESTADO\"='N' and IFNULL(R1.\"U_EXX_CODIGO\",'') <>'' and R.\"U_EXX_TIPO\"='{0}' ";
            recordSet.DoQuery(string.Format(query, tipoRendicion));
            IList<Tuple<string, string>> result = new List<Tuple<string, string>>();
            while (!recordSet.EoF)
            {
                REC1 line = new REC1();
                line.CodigoRendicion = recordSet.GetColumnValue("U_EXX_CODIGO").ToString();
                line.NroDocEmpleado = recordSet.GetColumnValue("U_EXX_NRODOE")?.ToString();
                line.Descripcion = recordSet.GetColumnValue("U_EXX_DESCRIPCION")?.ToString();
                line.NombreEmpleado = recordSet.GetColumnValue("U_EXX_NOMEMP")?.ToString();
                line.Monto = recordSet.GetColumnValue("U_EXX_MONCAJ").ToDouble();
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


        //public override IEnumerable<Tuple<string, string>> RetrieveTiendasData()
        //{
        //    try
        //    {
        //        List<Tuple<string, string>> result = new List<Tuple<string, string>>();

        //        //var list= TiendasList(Login);

        //        var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var query = "Select * from \"@CL_CODEMP\"";
        //        recordSet.DoQuery(string.Format(query));

        //        while (!recordSet.EoF)
        //        {
        //            var Code = recordSet.GetColumnValue("Code").ToString();
        //            var Name = recordSet.GetColumnValue("Name").ToString();
        //            result.Add(Tuple.Create(Code, Name));
        //            recordSet.MoveNext();
        //        }

        //        //    foreach (var item in list.Result)
        //        //{
        //        //    result.Add(Tuple.Create(item.Code, item.Name));
        //        //}

        //        return result;
        //    }
        //    finally
        //    {
        //        GenericHelper.ReleaseCOMObjects();
        //    }
        //}


        //private async Task<List<OCEM>> TiendasList(SLConnection login)
        //{


        //    var bpList = await login.Request(OCEM.ID)
        //       //.Filter("startswith(CardCode, 'c')")
        //       .Select("Code, Name")
        //       //.OrderBy("CardName")
        //       .WithPageSize(150)
        //       .WithCaseInsensitive()
        //       .GetAsync<List<OCEM>>();

        //    return bpList;
        //}

        //public override Tuple<string, string> RetrieveCuentaTransitoria(string Pasarela, string Tienda)
        //{
        //    try
        //    {

        //        //var list= TiendasList(Login);

        //        var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var query = "Select * from \"OACT\" where \"U_VS_LT_PASP\"='{0}' and \"U_VS_LT_COTI\"='{1}'";
        //        recordSet.DoQuery(string.Format(query, Pasarela, Tienda));

        //        while (!recordSet.EoF)
        //        {
        //            var Code = recordSet.GetColumnValue("AcctCode").ToString();
        //            var Name = recordSet.GetColumnValue("AcctName").ToString();
        //            return Tuple.Create(Code, Name);
        //            recordSet.MoveNext();
        //        }

        //        return Tuple.Create("", "");
        //    }
        //    finally
        //    {
        //        GenericHelper.ReleaseCOMObjects();
        //    }
        //}

        //public override string RetrieveMonedaPasarela(string Pasarela)
        //{
        //    try
        //    {

        //        //var list= TiendasList(Login);

        //        var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var query = "Select * from \"@VS_LT_OPAP\" where \"Code\"='{0}' ";
        //        recordSet.DoQuery(string.Format(query, Pasarela));

        //        while (!recordSet.EoF)
        //        {
        //            var moneda = recordSet.GetColumnValue("U_VS_LT_MONE").ToString();
        //            return moneda;
        //            recordSet.MoveNext();
        //        }

        //        return "";
        //    }
        //    finally
        //    {
        //        GenericHelper.ReleaseCOMObjects();
        //    }
        //}

        //public override SAPDocument RetrieveDocumentByPasarela(LTR1 result)
        //{
        //    try
        //    {

        //        //var list= TiendasList(Login);
        //        var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var query = "Select R1.* from \"ORCT\" R  JOIN \"RCT2\" R1 on R.\"DocEntry\"=R1.\"DocNum\" where R.\"U_VS_LT_STAD\"<>'Y' AND RIGHT(\"U_NDP_NROTAR\",4)='{0}'  AND \"DocDate\"='{1}' and \"DocTotal\" ={2}";


        //        var _date = result.FechaCobro.ToString("yyyyMMdd");
        //        var _query = string.Format(query,
        //            ObtenerUltimosCuatroDigitos(result.NroTarjeta),
        //            result.FechaCobro.ToString("yyyyMMdd"),
        //            result.ImporteCobradoTarjeta);

        //        recordSet.DoQuery(_query);

        //        int DocEntry = 0;
        //        int DocNum = 0;
        //        int TypeDoc = 0;

        //        while (!recordSet.EoF)
        //        {
        //            DocEntry = recordSet.GetColumnValue("DocEntry").ToInt32();
        //            TypeDoc = recordSet.GetColumnValue("InvType").ToInt32();
        //            DocNum = recordSet.GetColumnValue("DocNum").ToInt32();
        //            recordSet.MoveNext();
        //        }

        //        if (TypeDoc == 13)
        //        {
        //            BaseSAPDocumentRepository<OINV, INV1> documentRepository = new UnitOfWork(Company).InvoiceRepository;
        //            var document = documentRepository.RetrieveDocuments(t => t.DocumentEntry == DocEntry).FirstOrDefault();
        //            document.NroSapCobro = DocNum;

        //            var respuesta = documentRepository.RetriveCobroTransId(DocNum.ToString());

        //            document.NroSapCobroTransId = respuesta.Item1 ? respuesta.Item2.ToInt32() : 0;
        //            return document;

        //        }


        //        return null;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception("RetrieveDocumentByPasarela: " + ex.Message);
        //    }
        //}

        //static string ObtenerUltimosCuatroDigitos(string texto)
        //{
        //    if (texto.Length >= 4)
        //    {
        //        // Utilizar Substring para obtener los últimos 4 caracteres
        //        return texto.Substring(texto.Length - 4);
        //    }
        //    else
        //    {
        //        // El texto es demasiado corto para obtener 4 dígitos
        //        return "Texto demasiado corto";
        //    }
        //}

        //public override Tuple<bool, string, JournalEntrySL> GenerarAsiento(JournalEntrySL asiento)
        //{

        //    JournalEntrySL receive = new JournalEntrySL();

        //    var list = CreateJournalEntrySLAsync(Login, asiento);


        //    while (list.Status == TaskStatus.WaitingForActivation)
        //    {

        //    }
        //    if (list.Status == TaskStatus.RanToCompletion)
        //    {
        //        return Tuple.Create(true, list.Result.JdtNum.ToString(), list.Result);
        //    }
        //    if (list.Status == TaskStatus.Faulted)
        //    {
        //        return Tuple.Create(false, list.Exception.InnerException.Message, receive);
        //    }

        //    return Tuple.Create(true, "", receive);
        //}


        //private async Task<JournalEntrySL> CreateJournalEntrySLAsync(SLConnection login, JournalEntrySL asiento)
        //{


        //    var _journal = await login.Request("JournalEntries")
        //        .PostAsync<JournalEntrySL>(asiento);

        //    return _journal;
        //}

        //public override Tuple<bool, string> cancelarAsiento(string codAs)
        //{
        //    var list = CancelournalEntrySLAsync(Login, codAs);


        //    while (list.Status == TaskStatus.WaitingForActivation)
        //    {

        //    }
        //    if (list.Status == TaskStatus.RanToCompletion)
        //    {
        //        return Tuple.Create(true, "");
        //    }
        //    if (list.Status == TaskStatus.Faulted)
        //    {
        //        return Tuple.Create(false, list.Exception.InnerException.Message);
        //    }

        //    return Tuple.Create(true, "");
        //}

        //private async Task CancelournalEntrySLAsync(SLConnection login, string asiento)
        //{
        //    await login.Request("JournalEntries(" + asiento + ")/Cancel")
        //        .WithReturnNoContent()
        //        .PostAsync()
        //        ;


        //}

        //public override Tuple<bool, string> GenerarReconciliacion(ReconciliationSL reconciliation)
        //{

        //    var list = GenerarReconciliacionSLAsync(Login, reconciliation);

        //    while (list.Status == TaskStatus.WaitingForActivation)
        //    {

        //    }
        //    if (list.Status == TaskStatus.RanToCompletion)
        //    {
        //        return Tuple.Create(true, "Correcto");
        //    }
        //    if (list.Status == TaskStatus.Faulted)
        //    {
        //        return Tuple.Create(false, list.Exception.InnerException.Message);
        //    }

        //    return Tuple.Create(true, "");


        //}

        //private async Task<ReconciliationSL> GenerarReconciliacionSLAsync(SLConnection login, ReconciliationSL reconciliation)
        //{
        //    var _reconciliation = await login.Request("InternalReconciliations")
        //    .PostAsync<ReconciliationSL>(reconciliation);

        //    return _reconciliation;


        //}

        //public override Tuple<bool, string> agregarDetalleLiquidacion(string codLiquidacion, List<LiquidationLineSL> detail)
        //{


        //    LiquidationSL liquidation = new LiquidationSL();
        //    liquidation.VsLtLtr1Collection = detail;
        //    var list = AddDetailLiquidationAsync(Login, liquidation, codLiquidacion);

        //    while (list.Status == TaskStatus.WaitingForActivation)
        //    {

        //    }
        //    if (list.Status == TaskStatus.RanToCompletion)
        //    {
        //        return Tuple.Create(true, "Correcto");
        //    }
        //    if (list.Status == TaskStatus.Faulted)
        //    {
        //        return Tuple.Create(false, list.Exception.InnerException.Message);
        //    }

        //    return Tuple.Create(true, "");
        //}

        //private async Task AddDetailLiquidationAsync(SLConnection login, LiquidationSL liquidation, string codLiquidacion)
        //{
        //    await login.Request("VS_LT_OLTR(" + codLiquidacion + ")")
        //        .WithReplaceCollectionsOnPatch()
        //        .WithReturnNoContent()
        //        .PatchAsync(liquidation);

        //    //return _data;
        //}


        //public override Tuple<bool, string> ActualizaPagoSAP(List<int> listCod)
        //{

        //    foreach (var item in listCod)
        //    {
        //        var list = ActualizaPagoSAPAsync(Login, item);
        //        while (list.Status == TaskStatus.WaitingForActivation)
        //        {

        //        }
        //    }
        //    return Tuple.Create(true, "");
        //}

        //private async Task ActualizaPagoSAPAsync(SLConnection login, int cod)
        //{
        //    var estado = new { U_VS_LT_STAD = "Y" };

        //    await login.Request("IncomingPayments(" + cod + ")")
        //      .WithReturnNoContent()
        //      .PatchAsync(estado);

        //}

        //public override LiquidationSL RetrieveDetalleLiquidacion(string codigo)
        //{
        //    //var list = AddDetailLiquidationAsync(Login, codigo);

        //    var list = Login.Request("VS_LT_OLTR(" + codigo + ")").GetAsync<LiquidationSL>().GetAwaiter().GetResult();
        //    return list;
        //    //while (list.Status == TaskStatus.WaitingForActivation)
        //    //{

        //    //}
        //    //if (list.Status == TaskStatus.RanToCompletion)
        //    //{
        //    //    return list.Result;
        //    //}
        //    //if (list.Status == TaskStatus.Faulted)
        //    //{
        //    //    return null;
        //    //}

        //    return null;
        //}


        //private async Task<LiquidationSL> AddDetailLiquidationAsync(SLConnection login, string codLiquidacion)
        //{
        //    var data = await login.Request("VS_LT_OLTR(" + codLiquidacion + ")")
        //        .GetAsync<LiquidationSL>();

        //    return data;
        //}

        //public override List<LiquidationLineSL> RetrieveDetalleLiquidacion(List<Tuple<string, string>> lista)
        //{
        //    //string whereStatement = QueryHelper.ParseToHANAQuery(expression);

        //    var list = RetrieveLiquidationAsync(Login, lista);

        //    while (list.Status == TaskStatus.WaitingForActivation)
        //    {

        //    }
        //    if (list.Status == TaskStatus.RanToCompletion)
        //    {
        //        List<LiquidationLineSL> details = new List<LiquidationLineSL>();
        //        foreach (var item in list.Result)
        //        {
        //            details.AddRange(item.VsLtLtr1Collection);
        //        }
        //        return details;//list.Result;
        //    }
        //    if (list.Status == TaskStatus.Faulted)
        //    {
        //        return null;
        //    }

        //    return null;//documents_from_query_draft(string.Format(DOCUMENT_QUERY_DRAFT, $"where {whereStatement}"));
        //}

        //private async Task<List<LiquidationSL>> RetrieveLiquidationAsync(SLConnection login, List<Tuple<string, string>> lista)
        //{
        //    var filter = "";
        //    bool isFirst = true;
        //    foreach (var item in lista)
        //    {
        //        if (isFirst)
        //        {
        //            filter = item.Item1 + " eq '" + item.Item2 + "'";
        //            isFirst = false;
        //        }
        //        else
        //        {
        //            filter = filter + " and " + item.Item1 + " eq '" + item.Item2 + "'";
        //        }


        //    }

        //    var data = await login.Request("VS_LT_OLTR")
        //        //.WithHeader("Prefer", "odata.maxpagesize = 500")
        //        .Filter(filter)
        //        .GetAllAsync<LiquidationSL>();

        //    return data.ToList();
        //}

        //public override Tuple<bool, string> RetrieveSocioNegocioPasarela(string pasarela)
        //{
        //    try
        //    {

        //        //var list= TiendasList(Login);

        //        var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var query = "Select * from \"@VS_LT_OPAP\" where \"Code\"='{0}' ";
        //        recordSet.DoQuery(string.Format(query, pasarela));

        //        while (!recordSet.EoF)
        //        {
        //            var Code = recordSet.GetColumnValue("U_VS_LT_CDSN").ToString();
        //            return Tuple.Create(true, Code);
        //            recordSet.MoveNext();
        //        }

        //        return Tuple.Create(false, "");
        //    }
        //    finally
        //    {
        //        GenericHelper.ReleaseCOMObjects();
        //    }
        //}

        //public override Tuple<bool, string> RetrieveCuentaxMotivo(string codAjuste)
        //{
        //    try
        //    {

        //        //var list= TiendasList(Login);

        //        var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var query = "Select * from \"@VS_LT_OMAJ\" where \"Code\"='{0}' ";
        //        recordSet.DoQuery(string.Format(query, codAjuste));

        //        while (!recordSet.EoF)
        //        {
        //            var Code = recordSet.GetColumnValue("U_VS_LT_CCAS").ToString();
        //            return Tuple.Create(true, Code);
        //            recordSet.MoveNext();
        //        }

        //        return Tuple.Create(false, "");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Tuple.Create(false, ex.Message);
        //    }
        //    finally
        //    {
        //        GenericHelper.ReleaseCOMObjects();
        //    }
        //}

        //public override Tuple<bool, LTR1> RetrieveDataPago(string value)
        //{
        //    LTR1 line = new LTR1();
        //    try
        //    {


        //        var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var query = "select OV.\"CardCode\",OV.\"CardName\",R2.\"InvType\", OV.\"NumAtCard\",OV.\"DocDate\", " +
        //            "OV.\"DocTotal\", RC.\"DocEntry\" as \"SapCobro\", RC.\"TransId\" from \"OINV\"  OV JOIN  \"RCT2\"" +
        //            " R2 ON OV.\"DocEntry\"=R2.\"DocEntry\" JOIN ORCT RC ON R2.\"DocNum\"=RC.\"DocEntry\" where OV.\"DocEntry\"='{0}'  and RC.\"Canceled\"='N' and RC.\"U_VS_LT_STAD\"='N' ";
        //        recordSet.DoQuery(string.Format(query, value));

        //        while (!recordSet.EoF)
        //        {


        //            line.CodigoCliente = recordSet.GetColumnValue("CardCode").ToString();
        //            line.TipoDocumento = "01";
        //            line.NombreCliente = recordSet.GetColumnValue("CardName").ToString();
        //            line.NroTicket = recordSet.GetColumnValue("NumAtCard").ToString();
        //            line.FechaDocumento = recordSet.GetColumnValue("DocDate").ToDateTime();
        //            line.ImporteDocumento = recordSet.GetColumnValue("DocTotal").ToDouble();
        //            line.Moneda = "SOL";
        //            line.NroSAPCobro = recordSet.GetColumnValue("SapCobro").ToInt32();
        //            line.CodigoDocumento = recordSet.GetColumnValue("TransId").ToString();

        //            return Tuple.Create(true, line);
        //            recordSet.MoveNext();
        //        }

        //        var recordSet2 = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var query2 = "select OV.\"CardCode\",OV.\"CardName\",R2.\"InvType\", OV.\"NumAtCard\",OV.\"DocDate\", " +
        //            "OV.\"DocTotal\", RC.\"DocEntry\" as \"SapCobro\", RC.\"TransId\" from \"ORIN\"  OV JOIN  \"VPM2\"" +
        //            " R2 ON OV.\"DocEntry\"=R2.\"DocEntry\" JOIN OVPM RC ON R2.\"DocNum\"=RC.\"DocEntry\" where OV.\"DocEntry\"='{0}' and RC.\"Canceled\"='N' ";
        //        recordSet.DoQuery(string.Format(query2, value));

        //        while (!recordSet.EoF)
        //        {


        //            line.CodigoCliente = recordSet.GetColumnValue("CardCode").ToString();
        //            line.TipoDocumento = "07";
        //            line.NombreCliente = recordSet.GetColumnValue("CardName").ToString();
        //            line.NroTicket = recordSet.GetColumnValue("NumAtCard").ToString();
        //            line.FechaDocumento = recordSet.GetColumnValue("DocDate").ToDateTime();
        //            line.ImporteDocumento = recordSet.GetColumnValue("DocTotal").ToDouble();
        //            line.Moneda = "SOL";
        //            line.NroSAPCobro = recordSet.GetColumnValue("SapCobro").ToInt32();
        //            line.CodigoDocumento = recordSet.GetColumnValue("TransId").ToString();

        //            return Tuple.Create(true, line);
        //            recordSet.MoveNext();
        //        }

        //        return Tuple.Create(false, line);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Tuple.Create(false, line);
        //    }
        //    finally
        //    {
        //        GenericHelper.ReleaseCOMObjects();
        //    }
        //}

        //public override Tuple<bool, OHOJ> RetrieveHojaRuta(string value)
        //{
        //    OHOJ hoja = new OHOJ();
        //    try
        //    {
        //        var r1 = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var val = "select* from \"@EX_HR_OHGR\"  where \"U_EXK_COD\" = '{0}'   and \"U_EXK_EST\" in ('O','T') ";
        //        r1.DoQuery(string.Format(val, value));
        //        while (!r1.EoF)
        //        {

        //            return Tuple.Create(false, hoja);
        //            r1.MoveNext();
        //        }





        //        var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var query = "select * from \"@EXK_HOJARUTA\" where \"Code\"='{0}'   ";
        //        recordSet.DoQuery(string.Format(query, value));

        //        while (!recordSet.EoF)
        //        {


        //            hoja.Transportista = recordSet.GetColumnValue("U_EXK_TRANSP").ToString();
        //            hoja.Chofer = recordSet.GetColumnValue("U_EXK_CHOFER").ToString();
        //            hoja.Auxiliar1 = recordSet.GetColumnValue("U_EXK_AUX1")?.ToString();
        //            hoja.Auxiliar2 = recordSet.GetColumnValue("U_EXK_AUX2")?.ToString();
        //            hoja.Auxiliar3 = recordSet.GetColumnValue("U_EXK_AUX3")?.ToString();
        //            hoja.InicioTraslado = recordSet.GetColumnValue("U_EXK_FEINTRAS").ToDateTime();
        //            hoja.FinTraslado = recordSet.GetColumnValue("U_EXK_FEFITRAS").ToDateTime();
        //            hoja.Placa = recordSet.GetColumnValue("U_EXK_PLACA")?.ToString();

        //            recordSet.MoveNext();
        //        }
        //        var recordSetDet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var queryDet = "select * from \"@EXK_DHOJARUTA\" where \"Code\"='{0}'   ";
        //        recordSetDet.DoQuery(string.Format(queryDet, value));

        //        List<HOJ1> list = new List<HOJ1>();
        //        while (!recordSetDet.EoF)
        //        {
        //            HOJ1 line = new HOJ1();

        //            line.ZonaDespacho = recordSetDet.GetColumnValue("U_EXK_ZONDESP").ToString();

        //            list.Add(line);
        //            recordSetDet.MoveNext();
        //        }

        //        hoja.DetalleZonas = list;

        //        return Tuple.Create(true, hoja);
        //    }
        //    catch (Exception)
        //    {
        //        return Tuple.Create(false, hoja);
        //    }
        //    finally
        //    {
        //        GenericHelper.ReleaseCOMObjects();
        //    }
        //}

        //public override Tuple<bool, List<ODLN>> RetrieveGuiasHoja(string desde, string hasta, string programado, string zona)
        //{
        //    List<ODLN> listaGuias = new List<ODLN>();
        //    try
        //    {
        //        var queryprogram = "";
        //        if (!string.IsNullOrEmpty(programado))
        //        {
        //            if (programado != "B")
        //                queryprogram = " and \"U_EXK_HRPROG\"='" + programado + "'";

        //        }
        //        var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var query = "select (select \"Name\" from \"@EXK_ZONAVENTA\" where \"Code\"=D.\"U_EXK_AGENZONA\") as \"ZonaAgencia\", " +
        //            " ZN.\"Name\" , D.*  from \"ODLN\" D join  \"DLN12\" D12 on D12.\"DocEntry\"=D.\"DocEntry\" " +
        //            " LEFT JOIN \"@EXK_ZONAVENTA\" ZN on ZN.\"Code\"=D12.\"U_EXX_TPED_ZONAS\" " +
        //            " where \"FolioPref\" is not null and \"DocDate\">= TO_DATE('{0}', 'YYYYMMDD') and \"DocDate\"<= TO_DATE('{1}', 'YYYYMMDD') {2} ";
        //        recordSet.DoQuery(string.Format(query, desde, hasta, queryprogram));

        //        var ex = string.Format(query, desde, hasta, queryprogram);
        //        while (!recordSet.EoF)
        //        {
        //            ODLN Guias = new ODLN();

        //            Guias.NumberAtCard = recordSet.GetColumnValue("FolioPref").ToString() + "-" + recordSet.GetColumnValue("FolioNum").ToString();
        //            Guias.Peso = recordSet.GetColumnValue("U_EXX_FE_GRPESOTOTAL").ToString();
        //            Guias.CantidadBultos = (recordSet.GetColumnValue("U_EXK_CANTBULTO") == null) ? 0 : recordSet.GetColumnValue("U_EXK_CANTBULTO").ToInt32();
        //            Guias.Programado = recordSet.GetColumnValue("U_EXK_HRPROG").ToString();
        //            var valAgencia = recordSet.GetColumnValue("U_EXK_AGENCOD") != null ? recordSet.GetColumnValue("U_EXK_AGENCOD").ToString() : "";

        //            if (string.IsNullOrEmpty(valAgencia))
        //            {
        //                Guias.Zona = recordSet.GetColumnValue("Name") != null ? recordSet.GetColumnValue("Name").ToString() : "";
        //                Guias.DireccionDespacho = recordSet.GetColumnValue("Address2")?.ToString();

        //            }
        //            else
        //            {
        //                Guias.Zona = recordSet.GetColumnValue("ZonaAgencia") != null ? recordSet.GetColumnValue("ZonaAgencia").ToString() : "";
        //                Guias.DireccionDespacho = recordSet.GetColumnValue("U_EXK_AGENDIREC") != null ? recordSet.GetColumnValue("U_EXK_AGENDIREC").ToString() : "";
        //            }
        //            var dept = recordSet.GetColumnValue("U_EXK_DPTO") != null ? recordSet.GetColumnValue("U_EXK_DPTO").ToString() : "";
        //            var proc = recordSet.GetColumnValue("U_EXK_PROVINCIA") != null ? recordSet.GetColumnValue("U_EXK_PROVINCIA").ToString() : "";
        //            var dist = recordSet.GetColumnValue("U_EXK_DISTRITO") != null ? recordSet.GetColumnValue("U_EXK_DISTRITO").ToString() : "";
        //            Guias.DepProvZona = dept + "-" + proc + "-" + dist;



        //            listaGuias.Add(Guias);
        //            recordSet.MoveNext();
        //        }

        //        return Tuple.Create(true, listaGuias);
        //    }

        //    catch (Exception ex)
        //    {
        //        return Tuple.Create(false, listaGuias);
        //    }
        //    finally
        //    {
        //        GenericHelper.ReleaseCOMObjects();
        //    }
        //}

        //public override string RetrieveCodigoGenerado()
        //{
        //    try
        //    {

        //        //var list= TiendasList(Login);

        //        var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var query = " select LPAD(Count(*) + 1, 3, '0') as \"count\" from \"@EX_HR_OHGR\" ";
        //        recordSet.DoQuery(string.Format(query));

        //        while (!recordSet.EoF)
        //        {
        //            var Code = DateTime.Now.Year + "-" + recordSet.GetColumnValue("count").ToString();
        //            return Code;
        //            recordSet.MoveNext();
        //        }

        //        return "";
        //    }
        //    finally
        //    {
        //        GenericHelper.ReleaseCOMObjects();
        //    }
        //}

        //public override void ActualizarProgramado(string numeracion, string estado, ODLN datosGuia)
        //{
        //    try
        //    {

        //        //var list= TiendasList(Login);
        //        var list = numeracion.Split("-");

        //        BaseSAPDocumentRepository<ODLN, DLN1> documentRepository = new UnitOfWork(Company).DeliveryRepository;
        //        string foliopref = list[0];
        //        int folionum = list[1].ToInt32();
        //        var document = documentRepository.RetrieveDocuments(t => t.FolioPref == foliopref && t.FolioNum == folionum).FirstOrDefault();

        //        document.Programado = estado;
        //        document.FechaInicioTraslado = datosGuia.FechaInicioTraslado;
        //        document.CodigoTransportista = datosGuia.CodigoTransportista;
        //        document.NombreTransportista = datosGuia.NombreTransportista;
        //        document.NombreConductor = datosGuia.NombreConductor;
        //        document.LicenciaConductor = datosGuia.LicenciaConductor;
        //        document.CantidadBultos = datosGuia.CantidadBultos;
        //        document.FechaGuia = document.DocumentDeliveryDate;
        //        document.TipoOperacion = datosGuia.TipoOperacion;
        //        document.MotivoTraslado = datosGuia.MotivoTraslado;
        //        document.FEXModalidadTraslado = datosGuia.FEXModalidadTraslado;
        //        document.HojaRuta = datosGuia.HojaRuta;
        //        document.EstadoEnvioSunat = datosGuia.EstadoEnvioSunat;
        //        var val = documentRepository.UpdateCustomFieldsFromDocument(document);
        //        //var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        //var query = " update ODLN where \"FolioPref\"='{0}' and \"FolioNum\"={1}";
        //        //recordSet.DoQuery(string.Format(query,list[0],list[1]));
        //        if (!val.Item1)
        //        {
        //            throw new Exception(val.Item2);
        //        }
        //        //using (HanaCommand updateCommand = new HanaCommand(query, connection))
        //        //{
        //        //    // Ejecutar la sentencia de actualización
        //        //    int rowsAffected = updateCommand.ExecuteNonQuery();
        //        //    Console.WriteLine($"Filas actualizadas: {rowsAffected}");
        //        //}


        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        GenericHelper.ReleaseCOMObjects();
        //    }
        //}

        //public override void ActualizarEnvioSunat(string sunat)
        //{
        //    try
        //    {

        //        //var list= TiendasList(Login);

        //        var list = sunat.Split("-");
        //        BaseSAPDocumentRepository<ODLN, DLN1> documentRepository = new UnitOfWork(Company).DeliveryRepository;
        //        string foliopref = list[0];
        //        int folionum = list[1].ToInt32();
        //        var document = documentRepository.RetrieveDocuments(t => t.FolioPref == foliopref && t.FolioNum == folionum).FirstOrDefault();

        //        document.EstadoEnvioSunat = "Y";
        //        //document.EstadoSUNAT = "AUT";
        //        documentRepository.UpdateCustomFieldsFromDocument(document);
        //        //var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        //var query = " update ODLN where \"FolioPref\"='{0}' and \"FolioNum\"={1}";
        //        //recordSet.DoQuery(string.Format(query,list[0],list[1]));

        //        //using (HanaCommand updateCommand = new HanaCommand(query, connection))
        //        //{
        //        //    // Ejecutar la sentencia de actualización
        //        //    int rowsAffected = updateCommand.ExecuteNonQuery();
        //        //    Console.WriteLine($"Filas actualizadas: {rowsAffected}");
        //        //}


        //        //var list= TiendasList(Login);

        //        var recordSetIDCompany = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var queryIDCompany = "SELECT \"IdFexCompany\" FROM \"FEX_PE\".FEX_COMPANY WHERE \"dbName\"='{0}' AND IFNULL(\"BranchID\",-1) = {1}";
        //        recordSetIDCompany.DoQuery(string.Format(queryIDCompany, Company.CompanyDB, document.BranchId));

        //        var IdFexCompany = "";
        //        while (!recordSetIDCompany.EoF)
        //        {
        //            IdFexCompany = recordSetIDCompany.GetColumnValue("IdFexCompany").ToString();
        //            recordSetIDCompany.MoveNext();
        //        }


        //        var rvalidar = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var queryvalidar = "SELECT count(*) as \"existe\" FROM \"FEX_PE\".\"FEX_DOCUMENTOS\" where \"IdFexCompany\" = {0} and \"Docentry\" = {1} and \"ObjectType\" = {2}";
        //        rvalidar.DoQuery(string.Format(queryvalidar, IdFexCompany, document.DocumentEntry,15));

        //        int  existe = 0;
        //        while (!rvalidar.EoF)
        //        {
        //            existe = rvalidar.GetColumnValue("existe").ToInt32 ();
        //            rvalidar.MoveNext();
        //        }

        //        if (existe == 0)
        //        {
        //            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //            var query = "Call \"FEX_PE\".\"FEX_ACCION_Guardar\"( " +
        //                "  0," +//  --IN IdAccion bigint.  0 Para Insert.  Valor de IdAccion para actualizar registro existente
        //                IdFexCompany + ", " +//    --IN IdFexCompany int.Valor de Company para documento actual
        //                  "'" + document.Indicator + "'," + //--IN CodigoEntidad varchar(50).
        //                 +document.DocumentEntry + "," +// --IN DocEntry int.
        //                "'" + document.ObjectType + "'," +//   --IN ObjectType varchar(50).
        //                "'" + document.DocSubType + "', " + //   --IN DocSubType varchar(50).
        //             " '100',  " +//  --IN IdTipoAccion int.
        //                            "  'VIG', " +//   --IN Estado varchar(50).
        //             "'" + DateTime.Now.ToString() + "'" + //--IN FechaCreacion varchar(70)
        //             "); ";
        //            recordSet.DoQuery(string.Format(query));
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        GenericHelper.ReleaseCOMObjects();
        //    }
        //}

        //public override void ActualizarEstadoHojaGuia(string estado, string codigo)
        //{
        //    try
        //    {
        //        Company.StartTransaction();
        //        CompanyService companyService = Company.GetCompanyService();
        //        GeneralService generalService = companyService.GetGeneralService(OHRG.ID);
        //        var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
        //        generalDataParams.SetProperty("Code", codigo);
        //        GeneralData generalData = generalService.GetByParams(generalDataParams);

        //        generalData.SetProperty("U_EXK_EST", estado);


        //        generalService.Update(generalData);
        //        Company.EndTransaction(BoWfTransOpt.wf_Commit);
        //        //return Tuple.Create(true, string.Empty);
        //    }
        //    catch (Exception exception)
        //    {
        //        if (Company.InTransaction)
        //            Company.EndTransaction(BoWfTransOpt.wf_RollBack);

        //        //return Tuple.Create(false, exception.Message);
        //    }
        //}

        //public override Tuple<bool, string> GetCargaUtilByPlaca(string placa)
        //{
        //    try
        //    {

        //        //var list= TiendasList(Login);

        //        var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var query = "Select * from \"@EXX_VEHICU\" where \"Code\"='{0}' ";
        //        recordSet.DoQuery(string.Format(query, placa));

        //        while (!recordSet.EoF)
        //        {
        //            var Code = recordSet.GetColumnValue("U_EXK_CARGAUTIL").ToString();
        //            return Tuple.Create(true, Code);
        //            recordSet.MoveNext();
        //        }

        //        return Tuple.Create(false, "");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Tuple.Create(false, ex.Message);
        //    }
        //    finally
        //    {
        //        GenericHelper.ReleaseCOMObjects();
        //    }
        //}
        //static byte[] DecodePdfText(string pdfText)
        //{
        //    // Convertir el texto PDF a bytes
        //    byte[] pdfBytes = System.Text.Encoding.UTF8.GetBytes(pdfText);

        //    // Devolver los bytes del PDF decodificado
        //    return pdfBytes;
        //}
        //public override Tuple<bool, byte[]> ObtenerPDF(string numeracion)
        //{
        //    byte[] res = new byte[0];
        //    try
        //    {

        //        var list = numeracion.Split("-");
        //        BaseSAPDocumentRepository<ODLN, DLN1> documentRepository = new UnitOfWork(Company).DeliveryRepository;
        //        string foliopref = list[0];
        //        int folionum = list[1].ToInt32();
        //        var document = documentRepository.RetrieveDocuments(t => t.FolioPref == foliopref && t.FolioNum == folionum).FirstOrDefault();



        //        BaseOPDSRepository configRepository = new UnitOfWork(Company).SettingsRepository;
        //        var user=configRepository.Setting(OPDS.Codes.DBUSER);
        //        var pass = configRepository.Setting(OPDS.Codes.DBPASS);
        //        var server = configRepository.Setting(OPDS.Codes.SERVER);

        //        var recordSetIDCompany = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //        var queryIDCompany = "SELECT \"IdFexCompany\" FROM \"FEX_PE\".FEX_COMPANY WHERE \"dbName\"='{0}' AND IFNULL(\"BranchID\",-1) = {1}";
        //        recordSetIDCompany.DoQuery(string.Format(queryIDCompany, Company.CompanyDB, document.BranchId));

        //        var IdFexCompany = "";
        //        while (!recordSetIDCompany.EoF)
        //        {
        //            IdFexCompany = recordSetIDCompany.GetColumnValue("IdFexCompany").ToString();
        //            recordSetIDCompany.MoveNext();
        //        }



        //        var connectionString = "Server="+ server.Value + ";CS="+Company.CompanyDB+";UserID="+user.Value+";Password="+pass.Value;

        //        HanaConnectionStringBuilder connBuilder = new HanaConnectionStringBuilder();
        //        using (HanaConnection connection = new HanaConnection(connectionString))
        //        {
        //            try
        //            {
        //                // Abrir la conexión
        //                connection.Open();

        //                // Crear un comando SQL para ejecutar la consulta
        //                string query2 = "SELECT * FROM \"FEX_PE\".\"FEX_DOCUMENTOS\" WHERE \"IdFexCompany\" = "+IdFexCompany+" AND \"ObjectType\" = 15 AND \"Docentry\"="+document.DocumentEntry;

        //                using (HanaCommand command = new HanaCommand(query2, connection))
        //                {
        //                    // Ejecutar la consulta y obtener un lector de datos
        //                    using (HanaDataReader reader = command.ExecuteReader())
        //                    {
        //                        //// Crear un DataTable para almacenar los resultados
        //                        //DataTable dataTable = new DataTable();
        //                        //dataTable.Load(reader); // Cargar los datos del DataReader en el DataTable
        //                        if (reader.Read())
        //                        {
        //                            byte[] pdfBytes = (byte[])reader["PDF"];
        //                            return Tuple.Create(true, pdfBytes); ;
        //                            //string tempFilePath = Path.GetTempFileName() + ".pdf";
        //                            //File.WriteAllBytes(tempFilePath, pdfBytes);

        //                            //// Abrir el archivo PDF en el navegador predeterminado
        //                            //Process.Start(tempFilePath);
        //                        }

        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"Error: {ex.Message}");
        //            }
        //        }



        //        return Tuple.Create(false, res);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Tuple.Create(false, res);
        //    }
        //    finally
        //    {
        //        GenericHelper.ReleaseCOMObjects();
        //    }
        //}

        //public override Tuple<bool, string> ValidarSunat(string sunat)
        //{
        //    try
        //    {
        //        var list = sunat.Split("-");
        //        BaseSAPDocumentRepository<ODLN, DLN1> documentRepository = new UnitOfWork(Company).DeliveryRepository;
        //        string foliopref = list[0];
        //        int folionum = list[1].ToInt32();
        //        var document = documentRepository.RetrieveDocuments(t => t.FolioPref == foliopref && t.FolioNum == folionum).FirstOrDefault();

        //        if (document.EstadoSUNAT == "AUT")
        //        {
        //            return Tuple.Create(true, "validado");
        //        }
        //        else{
        //            return Tuple.Create(false, "sin validar");
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        return Tuple.Create(false, ex.Message);
        //    }
        //    finally
        //    {
        //        GenericHelper.ReleaseCOMObjects();
        //    }
        //}


    }
}

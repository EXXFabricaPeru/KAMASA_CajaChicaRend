using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.RegistroCompCCRR.Data.Repository;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Utilities;

namespace Exxis.Addon.RegistroCompCCRR.Data.Implements.DocumentRepository
{
    public class OPCHDocumentRepository : BaseSAPDocumentRepository<OPCH, PCH1>
    {
        public OPCHDocumentRepository(Company company) : base(company)
        {
        }

        public override IEnumerable<OPCH> RetrieveDocuments(Expression<Func<OPCH, bool>> expression)
        {
            IEnumerable<OPCH> documents = base.RetrieveDocuments(expression)
                .ToList();
            var unitOfWork = new UnitOfWork(Company);
            foreach (OPCH document in documents)
            {
                //PCH1 firstLine = document.DocumentLines.First();
                //OITM item = unitOfWork.ItemsRepository.GetItem(firstLine.ItemCode);
                //document.IsService = item.IsInventory == false;
                //document.IsItem = item.IsInventory;
            }

            return documents;
        }

        public override Tuple<bool, string> RegisterDocument(OPCH entity)
        {
            try
            {
                Company.StartTransaction();
                var document = Company.GetBusinessObject(BoObjectTypes.oPurchaseInvoices).To<Documents>();
                document.CardCode = entity.CardCode;
                document.DocDate = entity.DocumentDate;
                document.DocDueDate = entity.DocumentDeliveryDate;
                document.TaxDate = entity.TaxDate;
                document.BPL_IDAssignedToInvoice = entity.BranchId;
                document.Comments = entity.Comments;
                document.FolioPrefixString = entity.FolioPref;
                document.FolioNumber = entity.FolioNum;
                document.Indicator = entity.Indicator;
                document.GroupNumber = entity.CondicionPago;
                document.NumAtCard = entity.NumberAtCard;
                document.DocCurrency = entity.Currency;
                document.DocType = entity.Type == "S" ? BoDocumentTypes.dDocument_Service : BoDocumentTypes.dDocument_Items;
                Fields userFields = document.UserFields.Fields;
                //userFields.Item("U_VS_AFEDET").Value = "N";
                //userFields.Item("U_VS_NATNUM").Value = entity.Naturaleza;
                //userFields.Item("U_BPP_MDTD").Value = entity.TipoDocumento;
                userFields.Item("U_EXX_NUMEREND").Value = entity.NroRendicion;
                userFields.Item("U_EXX_DESCREND").Value = entity.DescripcionRendicion;
                userFields.Item("U_EXX_EMPLEADO").Value = entity.Empleado;

                Document_Lines documentLines = document.Lines;
                entity.DocumentLines.ForEach((line, index, lastIteration) =>
                {
                    documentLines.ItemDescription = line.ItemDescription;
                   
                   
                    if (line.MontoImpuesto.ToDouble() > 0)
                    {
                        documentLines.PriceAfterVAT = line.TotalConImpuesto.ToDouble();
                        //documentLines.UnitPrice = line.TotalPrice.ToDouble();
                        documentLines.TaxTotal = line.MontoImpuesto.ToDouble();

                        documentLines.TaxJurisdictions.TaxAmount = line.MontoImpuesto.ToDouble();
                        documentLines.TaxJurisdictions.JurisdictionCode = line.TaxCode;
                        documentLines.TaxJurisdictions.JurisdictionType = 1;
                    }
                    else
                    {
                        documentLines.LineTotal = line.TotalPrice.ToDouble();
                    }


                    documentLines.CostingCode = line.CentroCosto;
                    documentLines.CostingCode3 = line.CentroCosto3;

                    BaseInfrastructureRepository infraRepository = new UnitOfWork(Company).InfrastructureRepository;

                    string cuenta = infraRepository.RetrieveAccountCodeByActID(line.Cuenta.Replace("-", ""));
                    if (string.IsNullOrEmpty(cuenta))
                    {
                        throw new Exception("No se ha configurado o no existe la cuenta del servicio");
                    }
                    documentLines.AccountCode = cuenta;//line.Cuenta;
                    documentLines.TaxCode = line.TaxCode;

                    Fields oUserFields = documentLines.UserFields.Fields;

                    oUserFields.Item("U_EXX_SERCOMPR").Value = line.CodServicioCompra;
                    oUserFields.Item("U_EXX_GRUPODET").Value = line.GrupoDetraccion;


                    //documentLines.BaseType = line.BaseType;
                    //documentLines.BaseEntry = line.BaseEntry;
                    //documentLines.BaseLine = line.BaseLine;
                    //documentLines.Quantity = line.Quantity.ToDouble();
                    //documentLines.ItemCode = line.ItemCode;
                    //documentLines.WarehouseCode = line.WarehouseCode;
                    //documentLines.CostingCode2 = line.CentroCosto2;//TODO
                    //BatchNumbers batchNumbers = documentLines.BatchNumbers;
                    //line.SelectedBatches.ForEach((batch, batchIndex, batchLastIteration) =>
                    //{
                    //    batchNumbers.BatchNumber = batch.BatchNumber;
                    //    batchNumbers.Quantity = batch.Quantity.ToDouble();
                    //    batchNumbers.ExpiryDate = batch.Expiry;
                    //    batchLastIteration.IfFalse(() => batchNumbers.Add());
                    //});

                    lastIteration.IfFalse(() => documentLines.Add());
                });

                int operationResult = document.Add();
                if (operationResult.IsDefault())
                {
                    string key;
                    Company.GetNewObjectCode(out key);
                    if (Company.InTransaction)
                        Company.EndTransaction(BoWfTransOpt.wf_Commit);
                    return Tuple.Create(true, key);
                }

                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                return Tuple.Create(false, Company.GetLastErrorDescription());
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }
        }

        //public override void UpdateSystemFieldsFromDocument(OPCH document)
        //{
        //    var documents = Company.GetBusinessObject(BoObjectTypes.oPurchaseOrders).To<Documents>();
        //    documents.GetByKey(document.DocumentEntry);
        //    foreach (var documentLine in document.DocumentLines)
        //    {
        //        documents.Lines.SetCurrentLine(documentLine.LineNumber);
        //        documents.Lines.Quantity = documentLine.Quantity.ToDouble();
        //    }

        //    documents.Update();
        //}

    }
}

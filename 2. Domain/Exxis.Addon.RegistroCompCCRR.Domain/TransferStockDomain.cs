using System;
using System.Collections.Generic;
using System.Linq;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Resources;
using Exxis.Addon.RegistroCompCCRR.Domain.Code;
using Exxis.Addon.RegistroCompCCRR.Domain.Contracts;

namespace Exxis.Addon.RegistroCompCCRR.Domain
{
    public class TransferStockDomain : BaseDomain, ITransferStockDomain
    {
        public TransferStockDomain(Company company) : base(company)
        {
        }

        public OWTR RetrieveRequestTransfeStockByOT(int transferOrderDocEntry)
        {
            return UnitOfWork.TransferItemRepository.RetrieveRequestTransfeStockByOT(t => t.ReferenceTransferOrderEntryDoc == transferOrderDocEntry);
        }

        public OWTR RetrieveTransfeStockByEntry(int documentEntry)
        {
            return UnitOfWork.TransferItemRepository.Retrieve(t => t.DocumentEntry == documentEntry).FirstOrDefault();
            //return UnitOfWork.TransferItemRepository.Register();
        }

        public OWTR RetrieveTransfeStockByOT(int transferOrderDocEntry)
        {
            return UnitOfWork.TransferItemRepository.RetrieveTransfeStockByOT(t=>t.ReferenceTransferOrderEntryDoc==transferOrderDocEntry);
        }

        public Tuple<bool, string> UpdateEntity(OWTR document)
        {
            return UnitOfWork.TransferItemRepository.UpdateBaseEntity(document);
        }

        public Tuple<bool, string> UpdateEntityRequest(OWTR document)
        {
            return UnitOfWork.TransferItemRepository.UpdateBaseEntityRequest(document);
        }

        public Tuple<bool, string> UpdateFolio(OWTR document)
        {
            return UnitOfWork.TransferItemRepository.UpdateFolio(document);
        }

        public Tuple<bool, string> UpdateFolioRequest(OWTR document)
        {
            return UnitOfWork.TransferItemRepository.UpdateFolioRequest(document);
        }
    }
}

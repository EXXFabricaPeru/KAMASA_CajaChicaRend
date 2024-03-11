using System.Collections.Generic;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;
using Exxis.Addon.RegistroCompCCRR.Domain.Code;
using Exxis.Addon.RegistroCompCCRR.Domain.Contracts;

namespace Exxis.Addon.RegistroCompCCRR.Domain
{
    public class BatchDomain : BaseDomain, IBatchDomain
    {
        public BatchDomain(Company company) : base(company)
        {
        }

        public IEnumerable<OIBT> RetrieveBatchesPerItemCode(string itemCode)
        {
            return UnitOfWork.BatchRepository.GetBatches(itemCode);
        }

        public IEnumerable<OIBT> RetrieveBatchesPerItemCodeWO(string itemCode)
        {
            return UnitOfWork.BatchRepository.GetBatchesWithoutCNRV(itemCode);
        }

        public decimal RetrieveActualStock(string itemCode, string warehouse, string batchNumber)
        {
            return UnitOfWork.BatchRepository.RetrieveActualStock(itemCode,  warehouse, batchNumber);
        }

        public decimal RetrieveActualStock(string itemCode, string warehouse)
        {
            return UnitOfWork.BatchRepository.RetrieveActualStock(itemCode, warehouse);
        }
    }
}
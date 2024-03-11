using System.Collections.Generic;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;

namespace Exxis.Addon.RegistroCompCCRR.Data.Repository
{
    // ReSharper disable once InconsistentNaming
    public abstract class BaseOIBTRepository: Code.Repository<OIBT>
    {
        protected BaseOIBTRepository(Company company) : base(company)
        {
        }

        public abstract IEnumerable<OIBT> GetBatches(string itemId);

        public abstract IEnumerable<OIBT> GetBatchesWithoutCNRV(string itemId);

        public abstract OIBT RetrieveBatch(string itemCode, string batchNumber, string warehouseCode);

        public abstract void ReserveQuantity(int batchEntry, decimal quantity);

        public abstract decimal RetrieveActualStock(string itemCode, string warehouseCode, string batchNumber);

        public abstract decimal RetrieveActualStock(string itemCode, string warehouseCode);
    }
}
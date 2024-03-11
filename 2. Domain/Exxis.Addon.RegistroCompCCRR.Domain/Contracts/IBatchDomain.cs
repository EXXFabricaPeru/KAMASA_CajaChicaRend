using System.Collections.Generic;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;

namespace Exxis.Addon.RegistroCompCCRR.Domain.Contracts
{
    public interface IBatchDomain
    {
        IEnumerable<OIBT> RetrieveBatchesPerItemCode(string itemCode);
        IEnumerable<OIBT> RetrieveBatchesPerItemCodeWO(string itemCode);
        decimal RetrieveActualStock(string itemCode, string warehouse, string batchNumber);
        decimal RetrieveActualStock(string itemCode, string warehouse);
    }
}
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;
using Exxis.Addon.RegistroCompCCRR.Domain.Code;
using Exxis.Addon.RegistroCompCCRR.Domain.Contracts;

namespace Exxis.Addon.RegistroCompCCRR.Domain
{
    public class WarehouseDomain : BaseDomain, IWarehouseDomain
    {
        public WarehouseDomain(Company company) : base(company)
        {
        }

        public OWHS RetrieveWarehouse(string code)
        {
            return UnitOfWork.WarehouseRepository.RetrieveWarehouse(code);
        }
    }
}
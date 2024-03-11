using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;

namespace Exxis.Addon.RegistroCompCCRR.Domain.Contracts
{
    public interface IWarehouseDomain
    {
        OWHS RetrieveWarehouse(string code);
    }
}
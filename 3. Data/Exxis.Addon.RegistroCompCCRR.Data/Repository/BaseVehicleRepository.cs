using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.Localization.Header;

namespace Exxis.Addon.RegistroCompCCRR.Data.Repository
{
    public abstract class BaseVehicleRepository : Code.Repository
    {
        protected BaseVehicleRepository(Company company) : base(company)
        {
        }

        public abstract BPP_VEHICU FindByCode(string code);
    }
}
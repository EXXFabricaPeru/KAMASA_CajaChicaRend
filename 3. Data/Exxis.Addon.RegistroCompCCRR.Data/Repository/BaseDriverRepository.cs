using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.Localization.Header;

namespace Exxis.Addon.RegistroCompCCRR.Data.Repository
{
    public abstract class BaseDriverRepository : Code.Repository
    {
        protected BaseDriverRepository(Company company) : base(company)
        {
        }

        public abstract BPP_CONDUC FindByCode(string code);

        public abstract BPP_CONDUC FindByLicense(string code);
    }
}
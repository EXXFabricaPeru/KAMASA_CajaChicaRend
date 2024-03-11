using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;

namespace Exxis.Addon.RegistroCompCCRR.Data.Repository
{
    public abstract class BaseBusinessPartnerRepository : Code.Repository
    {
        protected BaseBusinessPartnerRepository(Company company) : base(company)
        {
        }

        public abstract OCRD FindByCode(string cardCode);
    }
}
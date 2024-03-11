using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;
using Exxis.Addon.RegistroCompCCRR.Data.Code;

namespace Exxis.Addon.RegistroCompCCRR.Data.Repository
{
    // ReSharper disable once InconsistentNaming
    public abstract class BaseOADMRepository : Repository<OADM>
    {
        protected BaseOADMRepository(Company company) : base(company)
        {
        }

        public abstract OADM CurrentCompany { get; }
    }
}
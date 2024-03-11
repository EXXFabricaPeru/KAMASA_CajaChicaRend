using SAPbobsCOM;
using System.Collections.Generic;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;
using Exxis.Addon.RegistroCompCCRR.Data.Code;

namespace Exxis.Addon.RegistroCompCCRR.Data.Repository
{
    // ReSharper disable once InconsistentNaming
    public abstract class BaseOCSTRepository : Repository<OCST>
    {
        protected BaseOCSTRepository(Company company) : base(company)
        {
        }

        public abstract List<OCST> ListDepartments { get; }
    }
}
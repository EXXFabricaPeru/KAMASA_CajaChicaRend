using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;

namespace Exxis.Addon.RegistroCompCCRR.Data.Repository
{
    public abstract class BaseEmployeeRepository : Code.Repository<OHEM>
    {
        protected BaseEmployeeRepository(Company company) : base(company)
        {
        }

        public abstract OHEM FindByCode(string cardCode);

        public abstract IEnumerable<OHEM> Retrieve(Expression<Func<OHEM, bool>> expression = null);
    }
}
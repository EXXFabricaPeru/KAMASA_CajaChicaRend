using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.RegistroCompCCRR.Data.Repository
{
    public abstract class BaseTariffRepository : Code.Repository<OATP>
    {
        protected BaseTariffRepository(Company company) : base(company)
        {
        }

        public abstract IEnumerable<OATP> Retrieve(Expression<Func<OATP, bool>> expression = null);
    }
}
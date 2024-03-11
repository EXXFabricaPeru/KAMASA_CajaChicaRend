using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.RegistroCompCCRR.Data.Repository
{
    public abstract class BaseTariffMotiveRepository : Code.Repository<OTMT>
    {
        protected BaseTariffMotiveRepository(SAPbobsCOM.Company company) 
            : base(company)
        {
        }

        public abstract IEnumerable<OTMT> Retrieve(Expression<Func<OTMT, bool>> expression);
    }
}
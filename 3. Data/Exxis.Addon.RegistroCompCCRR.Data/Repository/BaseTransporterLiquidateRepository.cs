using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.RegistroCompCCRR.Data.Repository
{
    public abstract class BaseTransporterLiquidateRepository : Code.Repository<OLDP>
    {
        protected BaseTransporterLiquidateRepository(Company company)
            : base(company)
        {
        }

        public abstract Tuple<bool, string> Register(OLDP entity);

        public abstract Tuple<bool, string> Update(OLDP entity);

        public abstract Tuple<bool, string> UpdateHeaderFields(OLDP entity);

        public abstract IEnumerable<OLDP> Retrieve(Expression<Func<OLDP, bool>> expression = null);
        public abstract IEnumerable<OLDP> RetrieveByDate(DateTime date,string Transporter);

        public abstract Tuple<bool, string> RemoveChild<T>(int entry) where T : BaseUDO;

        public abstract Tuple<bool, string> RegisterChild<T>(int entry, IEnumerable<T> children) where T : BaseUDO;

        public abstract Tuple<bool, string> ClosePayment(int entry, DateTime closeDate);
    }
}
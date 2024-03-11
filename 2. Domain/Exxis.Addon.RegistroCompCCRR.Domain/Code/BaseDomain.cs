using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Utilities;
using Exxis.Addon.RegistroCompCCRR.Data;

namespace Exxis.Addon.RegistroCompCCRR.Domain.Code
{
    public abstract class BaseDomain : IBaseDomain
    {
        protected Company Company { get; private set; }

        protected UnitOfWork UnitOfWork { get; }

        protected BaseDomain(Company company)
        {
            UnitOfWork = new UnitOfWork(company);
            Company = company;
        }

        protected T SpecificDomain<T>() where T : BaseDomain, IBaseDomain
        {
            return GenericHelper.MakeInstance<T>(Company);
        }
    }
}

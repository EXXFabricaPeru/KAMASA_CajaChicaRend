using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;
using Exxis.Addon.RegistroCompCCRR.Domain.Code;
using Exxis.Addon.RegistroCompCCRR.Domain.Contracts;

namespace Exxis.Addon.RegistroCompCCRR.Domain
{
    public class AuxiliaryDomain : BaseDomain, IAuxiliaryDomain
    {
        public AuxiliaryDomain(Company company)
            : base(company)
        {
        }

        public OHEM FindByCode(string cardCode)
        {
            return UnitOfWork.EmployeeRepository.FindByCode(cardCode);
        }
    }
}
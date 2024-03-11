using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;
using Exxis.Addon.RegistroCompCCRR.Domain.Code;

namespace Exxis.Addon.RegistroCompCCRR.Domain.Contracts
{
    public interface IAuxiliaryDomain : IBaseDomain
    {
        OHEM FindByCode(string cardCode);
    }
}
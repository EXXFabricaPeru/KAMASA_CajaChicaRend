using System;
using System.Collections.Generic;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header;
using Exxis.Addon.RegistroCompCCRR.Domain.Code;

namespace Exxis.Addon.RegistroCompCCRR.Domain.Contracts
{
    public interface ILiquidationTransporterDomain : IBaseDomain
    {
        Tuple<bool, string> ReplaceReferenceRoutes(int entry, IEnumerable<LDP1> routes);

        Tuple<bool, string> ReplaceReferenceMotives(int entry, IEnumerable<LDP2> motives);

        Tuple<bool, string> ClosePayment(int entry, DateTime closeDate);

        Tuple<bool, string> UpdateHeaderFields(OLDP header);
    }
}
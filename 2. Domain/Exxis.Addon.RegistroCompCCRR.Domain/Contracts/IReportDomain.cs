﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.Report;
using Exxis.Addon.RegistroCompCCRR.Domain.Code;

namespace Exxis.Addon.RegistroCompCCRR.Domain.Contracts
{
    public interface IReportDomain : IBaseDomain
    {
        IEnumerable<PickingSheet> RetrievePicking(int docentryRoute, bool isRoute, bool isSheet);
        IEnumerable<PickingSheet> RetrievePickingExcel(int entryRoute, bool isRoute,bool isSheet);
        IEnumerable<ControlCargaModel> RetrieveControlCarga(int docentryRoute);
        IEnumerable<CertificadoCalidadModel> RetrieveCertificadoCalidad(int docentryRoute, bool isRoute = true);
        IEnumerable<LiquidationRouteModel> RetrieveLiquidationRoute(int docentryRoute);
        IEnumerable<GuiaRemisionModel> RetrieveGuiaRoute(int docentryRoute,int codOT,bool isRoute=true);
    }
}

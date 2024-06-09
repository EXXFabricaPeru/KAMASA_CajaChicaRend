using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.ServiceLayer.Detail;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.ServiceLayer.Header;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header;
using Exxis.Addon.RegistroCompCCRR.Domain.Code;

namespace Exxis.Addon.RegistroCompCCRR.Domain.Contracts
{
    public interface IRegistroComprobanteDomain : IBaseDomain
    {
        string RetrieveCodigoGenerado();
        IEnumerable<Tuple<string, string>> RetrieveSucursales();
        IEnumerable<REC1> RetrieveRendicionesActivas(string tipoRendicion);
        REC1 RetrieveRendicionByCode(string documentEntry);
        void ActualizarEstadoLinea(string value, string line, string estado, string docentry);
        Tuple<bool, string> GenerarReconciliacion(OITR reconcilicaion);
        Tuple<bool, string, OJDT> generarAsiento(OJDT asientoRecon);
        Tuple<bool, OVPM> RetrievePagoByRendicion(string value);
        OJDT RetrieveAsiento(string item2);
        void ActualizarEstadoRegistroRendicion(string value, string v);
        Tuple<bool, string,string> RetrieveGastoByCode(string codServicioCompra);
        Tuple<bool, ORCT> GenerarPagoRecibido(OVPM item2);
        Tuple<bool, OVPM> GenerarPagoEfectuado(OVPM item2);
        double GetTipoCambio(DateTime fecha, string currency);
        IEnumerable<Tuple<string, string>> RetrieveMedioPago();
    }
}

using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.ServiceLayer.Detail;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.ServiceLayer.Header;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;

namespace Exxis.Addon.RegistroCompCCRR.Data.Repository
{
    public abstract  class BaseRegistroComprobanteRepository : Code.Repository
    {
        protected BaseRegistroComprobanteRepository(Company company) : base(company)
        {

        }

        public abstract string RetrieveCodigoGenerado();
        public abstract IEnumerable<Tuple<string, string>> RetrieveSucursales();
        public abstract IEnumerable<REC1> RetrieveRendicionesActivas(string tipoRendicion);
        public abstract void ActualizarEstadoLinea(string code, string line, string estado, string docentry);
        public abstract REC1 RetrieveRendicionByCode(string documentEntry,string tipo);
        public abstract Tuple<bool, string> GenerarReconciliacion(OITR reconcilicaion);
        public abstract Tuple<bool, string, OJDT> GenerarAsiento(OJDT asientoRecon);
        public abstract Tuple<bool, OVPM> RetrievePagoByRendicion(string rendicion);
        public abstract OJDT RetriveAsientoByCode(object asientoRecon);
        public abstract void ActualizarEstadoRegistroRendicion(string code, string estado);
        public abstract Tuple<bool, string, string> RetrieveGastoByCode(string codServicioCompra);
        public abstract Tuple<bool, ORCT> GenerarPagoRecibido(OVPM doc);
        public abstract Tuple<bool, OVPM> GenerarPagoEfectuado(OVPM doc);
        public abstract double GetTipoCambio(DateTime fecha, string currency);
        public abstract IEnumerable<Tuple<string, string>> RetrieveMedioPago();
        public abstract void ActualizarObjectRRCC(string value, ORCR regCom);




        //public abstract IEnumerable<Tuple<string, string>> RetrieveTiendasData();

        //public abstract Tuple<string, string> RetrieveCuentaTransitoria(string Pasarela, string Tienda);

        //public abstract string RetrieveMonedaPasarela(string Pasarela);
        //public abstract Tuple<bool, string> ActualizaPagoSAP(List<int> list);
        //public abstract Tuple<bool, string> cancelarAsiento(string codAs);
        //public abstract Tuple<bool, string> agregarDetalleLiquidacion(string codLiquidacion, List<LiquidationLineSL> detail);
        //public abstract SAPDocument RetrieveDocumentByPasarela(LTR1 result);
        //public abstract Tuple<bool, string, JournalEntrySL> GenerarAsiento(JournalEntrySL asiento);
        //public abstract Tuple<bool, string> GenerarReconciliacion(ReconciliationSL reconciliation);
        //public abstract LiquidationSL RetrieveDetalleLiquidacion(string codigo);
        //public abstract List<LiquidationLineSL> RetrieveDetalleLiquidacion(List<Tuple<string, string>> lista);
        //public abstract Tuple<bool, string> RetrieveSocioNegocioPasarela(string pasarela);
        //public abstract Tuple<bool, string> RetrieveCuentaxMotivo(string codAjuste);
        //public abstract Tuple<bool, LTR1> RetrieveDataPago(string value);
        //public abstract Tuple<bool, OHOJ> RetrieveHojaRuta(string value);
        //public abstract Tuple<bool, List<ODLN>> RetrieveGuiasHoja(string desde, string hasta, string programado, string zona);
        //public abstract string RetrieveCodigoGenerado();
        //public abstract void ActualizarProgramado(string numeracion, string estado, ODLN datosGuia);
        //public abstract void ActualizarEnvioSunat(string sunat);
        //public abstract void ActualizarEstadoHojaGuia(string estado,string codigo);
        //public abstract Tuple<bool, string> GetCargaUtilByPlaca(string placa);
        //public abstract Tuple<bool, byte[]> ObtenerPDF(string numeracion);
        //public abstract Tuple<bool, string> ValidarSunat(string placa);
    }
}

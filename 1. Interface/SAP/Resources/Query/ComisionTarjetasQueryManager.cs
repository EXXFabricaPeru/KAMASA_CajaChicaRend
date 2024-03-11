using Exxis.Addon.RegistroCompCCRR.CrossCutting.Utilities;

namespace Exxis.Addon.RegistroCompCCRR.Interface.Resources.Query
{
    [ResourceNamespace("Exxis.Addon.RegistroCompCCRR.Interface.Resources")]
    public class ComisionTarjetasQueryManager : SAPQueryManager
    {
        public ComisionTarjetasQueryManager(QueryType queryType) : base("BPVS - Hoja de Ruta a Guia", queryType)
        {
        }

        public virtual ElementTuple<string> VS_LT_BUSCAPASARELA { get; set; }
        public virtual ElementTuple<string> VS_LT_BUSCACUENTAPASARELA {get; set; }
        public virtual ElementTuple<string> VS_LT_BUSCAMONEDA { get; set; }

        public virtual ElementTuple<string> VS_LT_BUSCATIENDAS { get; set; }
        public virtual ElementTuple<string> VS_LT_CUENTA_BANCO { get; set; }

        public virtual ElementTuple<string> VS_LT_BUSCACODDOCUMENTO { get; set; }
        public virtual ElementTuple<string> VS_LT_MOTIVO_AJUSTE { get;  set; }
    }
}
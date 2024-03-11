using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header;
using Exxis.Addon.RegistroCompCCRR.Interface.Resources.Query;

namespace Exxis.Addon.RegistroCompCCRR.Interface.Startup.Versions
{
    // ReSharper disable once InconsistentNaming
    public class Version_0_0_0_4 : Versioner
    {
        public static Versioner Make => new Version_0_0_0_4();

        public Version_0_0_0_4() : base("0.0.0.4")
        {
        }

        protected override void InitializeTables()
        {
            SyncSystemTable(typeof(ODLN));

        }
        protected override void InitializeFormattedSearch()
        {     
            
        }
    }
}
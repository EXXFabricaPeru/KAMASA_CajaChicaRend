using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.RegistroCompCCRR.Interface.Startup.Versions
{
    // ReSharper disable once InconsistentNaming
    public class Version_0_0_0_1 : Versioner
    {
        public static Versioner Make => new Version_0_0_0_1();

        private Version_0_0_0_1() : base("0.0.0.1")
        {
        }

        protected override void InitializeTables()
        {
           
        }
        
        // protected override void InitializeFormattedSearch()
        // {
        // }
    }
}
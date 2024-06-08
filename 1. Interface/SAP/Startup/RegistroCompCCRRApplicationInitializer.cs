using SAPbouiCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Utilities;
using Exxis.Addon.RegistroCompCCRR.Interface.Customizations;
using Exxis.Addon.RegistroCompCCRR.Interface.Startup.Versions;
using Exxis.Addon.RegistroCompCCRR.Interface.Utilities;
using static Exxis.Addon.RegistroCompCCRR.CrossCutting.Utilities.ElementTuple<string>;
namespace Exxis.Addon.RegistroCompCCRR.Interface.Startup
{
    [ResourceNamespace("Exxis.Addon.RegistroCompCCRR.Interface.Resources")]
    public class RegistroCompCCRRApplicationInitializer : ApplicationInitializer
    {
     
        private RegistroCompCCRRApplicationInitializer(SAPbouiCOM.Framework.Application application)
            : base(MakeTuple(ProductId, ProductDescription),MakeTuple(Constants.Menu.MAIN_APPLICATION, @"EXXIS-Addon Registro Comp. CC y Rend"), application)
        {
        }

        protected override void BuildApplicationVersion(VersionCollection versionCollection)
        {
            versionCollection.Add(Version_0_0_0_1.Make);
            versionCollection.Add(Version_0_0_0_2.Make);
            versionCollection.Add(Version_0_0_0_3.Make);
            versionCollection.Add(Version_0_0_0_4.Make);
            //versionCollection.Add(Version_0_0_0_5.Make);

        }

        protected override void OnClickBaseMenuEvent(ref MenuEvent menuEvent, out bool handleEvent)
        {
            base.OnClickBaseMenuEvent(ref menuEvent, out handleEvent);
            if (handleEvent)
                CatchEvents.OnClickMenu(menuEvent, out handleEvent);
        }

        protected override void OnReferenceEvent(string formId, ref ItemEvent itemEvent, out bool handleEvent)
        {
            CatchEvents.OnGeneralApplicationEvents(formId, itemEvent, out handleEvent);
        }

        public static RegistroCompCCRRApplicationInitializer MakeDevelopmentInitializer()
        {
            var application = new SAPbouiCOM.Framework.Application();
            return new RegistroCompCCRRApplicationInitializer(application);
        }

        public static RegistroCompCCRRApplicationInitializer MakeProductionInitializer(string connectionString)
        {
            var application = new SAPbouiCOM.Framework.Application(connectionString);
            return new RegistroCompCCRRApplicationInitializer(application);
        }
    }
}

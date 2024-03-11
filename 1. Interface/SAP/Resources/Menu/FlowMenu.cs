using System.Collections.Generic;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header;
using Exxis.Addon.RegistroCompCCRR.Interface.Utilities;
using Exxis.Addon.RegistroCompCCRR.Interface.Views;
using Exxis.Addon.RegistroCompCCRR.Interface.Views.UserViews;

namespace Exxis.Addon.RegistroCompCCRR.Interface.Resources.Menu
{
    public class FlowMenu : BaseMenu
    {
        //public FlowMenu() : base(new SAPMenu {Id = Constants.Menu.MAIN_FLOW, Description = "Hoja de Ruta Asignación",TargetUDOClass =  typeof(OHRG) })
        //{
        //    order = 1;
        //}

        public FlowMenu() : base(new SAPMenu { Id = $"EX.{ORCR.ID}", Description = "Registro Comprobantes CC/REND", TargetUDOClass = typeof(ORCR) })
        {
            order = 1;
        }
        public static int order=1;
        
        public override void BuildChildOptions(IList<SAPMenu> childMenuReference)
        {
            //childMenuReference.Add(new SAPMenu { Id = $"PD.{OHRG.ID}", Description = OHRG.DESCRIPTION, TargetUDOClass = typeof(OHRG) });
            //childMenuReference.Add(new SAPMenu { Id = $"PD.{Form1.ID}", Description = Form1.DESCRIPTION, TargetFormClass = typeof(Form1) });
            //childMenuReference.Add(new SAPMenu { Id = $"PD.{ReporteLiquidacion.ID}", Description = ReporteLiquidacion.DESCRIPTION, TargetFormClass = typeof(ReporteLiquidacion) });

        }
    }
}

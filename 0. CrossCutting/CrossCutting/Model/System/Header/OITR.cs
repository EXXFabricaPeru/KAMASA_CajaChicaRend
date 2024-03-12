using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header
{
    public class OITR
    {
        public string CardOrAccount { get; set; }
        public List<ITR1> InternalReconciliationOpenTransRows { get; set; }
        public DateTime ReconDate { get; set; }

    }
}

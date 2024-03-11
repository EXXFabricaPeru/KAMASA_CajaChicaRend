using System;
using System.Collections.Generic;
using System.Linq;
using Exxis.Addon.RegistroCompCCRR.Interface.Startup.Versions;

namespace Exxis.Addon.RegistroCompCCRR.Interface.Startup
{
    public class VersionCollection : List<Versioner>
    {
        public string GetLastVersion()
        {
            Versioner lastVersion = this.LastOrDefault();
            if(lastVersion == null)
                throw new Exception("The application doesn't have versions to implement.");

            return lastVersion.CurrentVersion;
        }
    }
}
﻿using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header;
using Exxis.Addon.RegistroCompCCRR.Interface.Resources.Query;

namespace Exxis.Addon.RegistroCompCCRR.Interface.Startup.Versions
{
    // ReSharper disable once InconsistentNaming
    public class Version_0_0_0_3 : Versioner
    {
        public static Versioner Make => new Version_0_0_0_3();

        public Version_0_0_0_3() : base("0.0.0.3")
        {
        }

        protected override void InitializeTables()
        {
            SyncSystemTable(typeof(OIDC));

        }
        protected override void InitializeFormattedSearch()
        {


        }
    }
}
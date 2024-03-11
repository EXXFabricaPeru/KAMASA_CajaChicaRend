using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.RegistroCompCCRR.Data.Repository;

namespace Exxis.Addon.RegistroCompCCRR.Data.Implements.DocumentRepository
{
    public class OPRQDocumentRepository : BaseSAPDocumentRepository<OPRQ, PRQ1>
    {
        public OPRQDocumentRepository(Company company) : base(company)
        {

        }
    }
}

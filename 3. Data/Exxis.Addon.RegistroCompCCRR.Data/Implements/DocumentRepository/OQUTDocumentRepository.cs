using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.RegistroCompCCRR.Data.Repository;

namespace Exxis.Addon.RegistroCompCCRR.Data.Implements.DocumentRepository
{
    public class OQUTDocumentRepository : BaseSAPDocumentRepository<OQUT, QUT1>
    {
        public OQUTDocumentRepository(Company company) : base(company)
        {
        }
    }
}
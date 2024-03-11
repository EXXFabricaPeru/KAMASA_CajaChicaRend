// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.RegistroCompCCRR.Data.Repository;

namespace Exxis.Addon.RegistroCompCCRR.Data.Implements.DocumentRepository
{
    public class OINVDocumentRepository : BaseSAPDocumentRepository<OINV, INV1>
    {
        public OINVDocumentRepository(Company company) : base(company)
        {
        }
    }
}
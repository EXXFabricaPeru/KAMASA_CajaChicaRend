// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.RegistroCompCCRR.Data.Repository;

namespace Exxis.Addon.RegistroCompCCRR.Data.Implements.DocumentRepository
{
    public class ODPIDocumentRepository : BaseSAPDocumentRepository<ODPI, DPI1>
    {
        public ODPIDocumentRepository(Company company) : base(company)
        {
        }
    }
}
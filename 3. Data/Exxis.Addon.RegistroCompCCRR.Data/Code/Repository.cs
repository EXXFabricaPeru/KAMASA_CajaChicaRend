using System;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;

namespace Exxis.Addon.RegistroCompCCRR.Data.Code
{
    public abstract class Repository
    {
        protected RetrieveFormat RetrieveFormat { get; private set; }

        protected Company Company { get; }

        protected Repository(Company company)
        {
            Company = company;
            RetrieveFormat = RetrieveFormat.Simplify;
        }

        public void SetRetrieveFormat(RetrieveFormat retrieveFormat)
        {
            RetrieveFormat = retrieveFormat;
        }
    }

    public abstract class Repository<T> : Repository where T : BaseSAPTable
    {
        private Type _currentReferenceType;

        protected Type CurrentReferenceType
        {
            get
            {
                if (_currentReferenceType == null) _currentReferenceType = typeof(T);
                return _currentReferenceType;
            }
        }

        protected Repository(Company company) : base(company)
        {
        }
        
    }
}

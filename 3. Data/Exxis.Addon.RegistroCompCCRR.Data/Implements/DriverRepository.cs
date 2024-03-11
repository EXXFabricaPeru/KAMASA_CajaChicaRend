using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAPbobsCOM;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.Localization.Header;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Utilities;
using Exxis.Addon.RegistroCompCCRR.Data.Repository;

namespace Exxis.Addon.RegistroCompCCRR.Data.Implements
{
    public class DriverRepository : BaseDriverRepository
    {
        private const string DRIVER_FIND_QUERY = "select * from \"@BPP_CONDUC\" {0}";

        public DriverRepository(Company company) : base(company)
        {
        }

        public override BPP_CONDUC FindByCode(string code)
        {
            var recordset = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordset.DoQuery(string.Format(DRIVER_FIND_QUERY, $"where \"Code\"='{code}'"));

            var result = new BPP_CONDUC();

            IEnumerable<Tuple<PropertyInfo, string>> tuplesProperties = result.GetType()
                .RetrieveSAPProperties()
                .Select(item =>
                {
                    if (item.IsColumnProperty())
                        return Tuple.Create(item, item.RetrieveColumnProperty().ColumnName);

                    if (item.IsFieldNoRelated())
                        return Tuple.Create(item, item.RetrieveFieldNoRelated().sAliasID);

                    return null;
                })
                .Where(item => item != null);

            foreach (var tupleProperty in tuplesProperties)
            {
                var value = recordset.GetColumnValue(tupleProperty.Item2);
                tupleProperty.Item1.SetValue(result, value);
            }

            return result;
        }

        public override BPP_CONDUC FindByLicense(string code)
        {
            var recordset = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordset.DoQuery(string.Format(DRIVER_FIND_QUERY, $"where \"U_BPP_CHLI\"='{code}'"));

            var result = new BPP_CONDUC();

            IEnumerable<Tuple<PropertyInfo, string>> tuplesProperties = result.GetType()
                .RetrieveSAPProperties()
                .Select(item =>
                {
                    if (item.IsColumnProperty())
                        return Tuple.Create(item, item.RetrieveColumnProperty().ColumnName);

                    if (item.IsFieldNoRelated())
                        return Tuple.Create(item, item.RetrieveFieldNoRelated().sAliasID);

                    return null;
                })
                .Where(item => item != null);

            foreach (var tupleProperty in tuplesProperties)
            {
                var value = recordset.GetColumnValue(tupleProperty.Item2);
                tupleProperty.Item1.SetValue(result, value);
            }

            return result;
        }
    }
}
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header
{
    public class ContactPerson
    {
        [SAPColumn("Cellolar")]
        public string Telefono { get; set; }

        [SAPColumn("Name")]
        public string ID { get; set; }

        [SAPColumn("E_MailL")]
        public string Email { get; set; }

        [SAPColumn("Notes1")]
        public string Licencia { get; set; }

    }
}
namespace Exxis.Addon.RegistroCompCCRR.Interface.Code.UserInterface
{
    public struct SelectItem
    {
        public string Code { get; }
        public string Description { get; }

        public SelectItem(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
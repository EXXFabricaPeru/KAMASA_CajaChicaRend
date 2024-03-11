using System;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UDOFatherReferenceAttribute : Attribute
    {
        public string UDOId { get; }

        public int ChildNumber { get; }

        public UDOFatherReferenceAttribute(string id, int childNumber)
        {
            UDOId = id;
            ChildNumber = childNumber;
        }
    }
}
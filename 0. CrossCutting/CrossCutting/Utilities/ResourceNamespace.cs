﻿using System;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Utilities
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ResourceNamespace : Attribute
    {
        public string NameSpace { get; }

        public ResourceNamespace(string nameSpace)
        {
            NameSpace = nameSpace;
        }
    }
}
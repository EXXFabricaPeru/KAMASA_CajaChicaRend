﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Exxis.Addon.RegistroCompCCRR.CrossCutting.Resources.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Error validación] La orden de traslado no supero el monto minimo para ser distribuida..
        /// </summary>
        public static string NotPassAmountValidation {
            get {
                return ResourceManager.GetString("NotPassAmountValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Error validación] El socio de negocio de la orden de traslado no posee certificados de distribución..
        /// </summary>
        public static string NotPassCertificateValidation {
            get {
                return ResourceManager.GetString("NotPassCertificateValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Error validación] La orden de traslado no posee la cantidad minima de entrega para ser distribuida.
        /// </summary>
        public static string NotPassQuantityValidation {
            get {
                return ResourceManager.GetString("NotPassQuantityValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Error validación] La dirección no tiene asignada una restricción de entrega..
        /// </summary>
        public static string NotPassRestrictionValidation {
            get {
                return ResourceManager.GetString("NotPassRestrictionValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Error validación] La orden de traslado no supero el peso minimo para ser distribuida..
        /// </summary>
        public static string NotPassWeightValidation {
            get {
                return ResourceManager.GetString("NotPassWeightValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Error validación] La dirección no tiene asignada una ventana horaria.
        /// </summary>
        public static string NotPassWindowValidation {
            get {
                return ResourceManager.GetString("NotPassWindowValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El cierre manual de recepción de documentos SAP se realizó de forma exitosa..
        /// </summary>
        public static string SucessfullClosingDocumentReception {
            get {
                return ResourceManager.GetString("SucessfullClosingDocumentReception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Error validación] El almacén de entrega no tiene una altitud asignada..
        /// </summary>
        public static string WarehouseNotPassLatitudeValidation {
            get {
                return ResourceManager.GetString("WarehouseNotPassLatitudeValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Error validación] El almacén de entrega no tiene una longitud asignada..
        /// </summary>
        public static string WarehouseNotPassLongitudeValidation {
            get {
                return ResourceManager.GetString("WarehouseNotPassLongitudeValidation", resourceCulture);
            }
        }
    }
}

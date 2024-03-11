using System;
using System.Collections.Generic;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header;
using Exxis.Addon.RegistroCompCCRR.Interface.Views;
//using Exxis.Addon.RegistroCompCCRR.Interface.Views.Modal;
using Exxis.Addon.RegistroCompCCRR.Interface.Views.UserObjectViews;

// ReSharper disable InconsistentNaming

namespace Exxis.Addon.RegistroCompCCRR.Interface.Utilities
{
    public static class LocalStorage
    {
        public static string FRM_DET_ORTR_TRANSFER_ORDER_ID { get; set; }

        public static ORTR STORAGE_TRANSFER_ORDER_VALIDATION { get; set; }

        public static ORTR FORM_TRANSFER_ORDER_OPENING { get; set; }

        public static Tuple<ORTR, int> FORM_TRANSFER_ORDER_UPDATING { get; set; }

        public static Tuple<int, int> STORE_DOCUMENT_VALIDATING { get; set; }
        public static Tuple<int, string> STORE_DOCUMENT_VALIDATING_STATUS { get; set; }
        public static Tuple<int, string> STORE_DOCUMENT_VALIDATING_REASON { get; set; }

        public static int STORE_ORTR_DOCUMENT_INCLUDE { get; set; }

        //public static TransferOrderValidate FORM_ORTR_DOCUMENT_INCLUDE { get; set; }

        //public static DocumentValidating FORM_DOCUMENT_VALIDATING { get; set; }

        //public static TransferOrderValidating FORM_ORTR_VALIDATION { get; set; }

        //public static OARD STORE_OARD_CLS_ROTE { get; set; }

        ////public static UpdateClosingRoute FORM_CLS_RT_2 { get; set; }

        //public static RouteForm FORM_ROUTE_UDO { get; set; }

        //public static RouteFormModal MODAL_ROUTE_UDO { get; set; }
    }
}
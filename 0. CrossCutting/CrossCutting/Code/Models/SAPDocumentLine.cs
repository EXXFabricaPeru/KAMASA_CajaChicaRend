﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Attributes;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Constant;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Utilities;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;
using static Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models.SAPDocument;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models
{
    [Serializable]
    [SystemTable("RDR1", FormType = "dummy")]
    public class SAPDocumentLine
    {
        [SAPColumn("LineNum")]
        public int LineNumber { get; set; }
        [SAPColumn("VisOrder")]
        public int VisOrder { get; set; }

     

        [SAPColumn("AcctCode")]
        public string Cuenta { get; set; }

        [SAPColumn("U_EXX_SERCOMPR")]
        public string CodServicioCompra { get; set; }

        [SAPColumn("U_EXX_GRUPODET")]
        public string GrupoDetraccion { get; set; }

        [SAPColumn("ItemCode")]
        public string ItemCode { get; set; }
        [SAPColumn("Dscription")]
        public string ItemDescription { get; set; }

        [SAPColumn("Quantity")]
        public decimal Quantity { get; set; }
        [SAPColumn("PriceBefDi")]
        public decimal UnitPrice { get; set; }

        [SAPColumn("LineTotal")]
        public decimal TotalPrice { get; set; }

        [SAPColumn("PriceAfVAT")]
        public decimal TotalConImpuesto { get; set; }

        [SAPColumn("TotalFrgn")]
        public decimal TotalPriceFC { get; set; }


        [SAPColumn("VatSum")]
        public decimal MontoImpuesto { get; set; }
        [SAPColumn("VatSumFrgn")]
        public decimal MontoImpuestoFC { get; set; }


        [SAPColumn("Weight1")]
        public decimal TotalSaleWeight { get; set; }
        [SAPColumn("Volume")]
        public decimal TotalSaleVolume { get; set; }

        [SAPColumn("Price")]
        public decimal Price { get; set; }

        /// <summary>
        [SAPColumn("DocEntryRuta")]
        public int CodeRuta { get; set; }
        [SAPColumn("DocEntryOT")]
        public int CodeOT { get; set; }
        [SAPColumn("DocEntryOrder")]
        public int CodeOrder { get; set; }

        [SAPColumn("TaxCode")]
        public string TaxCode { get; set; }
        [SAPColumn("WhsCode")]
        public string WarehouseCode { get; set; }


        [SAPColumn("unitMsr")]
        public string UnitMeasure { get; set; }

        [SAPColumn("BaseEntry")]
        public int BaseEntry { get; set; }
        [SAPColumn("BaseType")]
        public int BaseType { get; set; }
        [SAPColumn("BaseLine")]
        public int BaseLine { get; set; }


        [SAPColumn("TrgetEntry")]
        public int TargetEntry { get; set; }

        [SAPColumn("TargetType")]
        public int TargetType { get; set; }

        [SAPColumn("OcrCode")]
        public string CentroCosto { get; set; }

        [SAPColumn("OcrCode2")]
        public string CentroCosto2 { get; set; }

        [SAPColumn("OcrCode3")]
        public string CentroCosto3 { get; set; }

        [SAPColumn("OcrCode4")]
        public string CentroCosto4 { get; set; }

        [SAPColumn("CogsOcrCod")]
        public string COGSCostingCode { get; set; }

        [SAPColumn("CogsOcrCo2")]
        public string COGSCostingCode2 { get; set; }

        [SAPColumn("CogsOcrCo3")]
        public string COGSCostingCode3 { get; set; }

        [SAPColumn("CogsOcrCo4")]
        public string COGSCostingCode4 { get; set; }

        [SAPColumn("TaxOnly")]
        public string SoloImpuesto { get; set; }




        /// </summary>
        public IEnumerable<SAPSelectedBatch> SelectedBatches { get; set; }
        
    }
}
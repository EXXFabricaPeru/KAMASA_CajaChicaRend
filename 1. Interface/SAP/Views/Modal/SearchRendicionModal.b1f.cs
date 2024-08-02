using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Utilities;
using Exxis.Addon.RegistroCompCCRR.Domain;
using Exxis.Addon.RegistroCompCCRR.Domain.Contracts;
using Exxis.Addon.RegistroCompCCRR.Interface.Code.UserInterface;
using Exxis.Addon.RegistroCompCCRR.Interface.Utilities;
using Exxis.Addon.RegistroCompCCRR.Interface.Views.UserObjectViews;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exxis.Addon.RegistroCompCCRR.Interface.Views.Modal
{
    [FormAttribute("Exxis.Addon.RegistroCompCCRR.Interface.Views.Modal.SearchRendicionModal", "Views/Modal/SearchRendicionModal.b1f")]
    public class SearchRendicionModal : UserFormBase
    {

        public delegate void OnSelectedRouteHandler(string documentEntry);

        public event SearchRendicionModal.OnSelectedRouteHandler OnSelectedRendicion;

        public const string ID = "MDL_EXX_REN";
        public const string DESCRIPTION = "Lista de Rendiciones/Caja Chica";
        public const string SAP_FORM_ID = "Exxis.Addon.RegistroCompCCRR.Interface.Views.Modal.SearchRendicionModal";


        private class RendicionMatrix:REC1
        {
            public string DocEntryPago { get; set; }

            //public DateTime DeliveryDate { get; set; }

            //public string EndHour { get; set; }
            //public string DeliveryStatus { get; set; }
        }

        private string _selectDocumentEntry;
        private IRegistroComprobanteDomain _registroComprobanteDomain;
        private MatrixBuilder<REC1> _rendicionMatrixBuilder;

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this._rendicionesMatrix = ((SAPbouiCOM.Matrix)(this.GetItem("Item_0").Specific));
            this._seleccionarButton = ((SAPbouiCOM.Button)(this.GetItem("Item_1").Specific));
            this._seleccionarButton.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this._seleccionarButton_ClickBefore);
            this._seleccionarButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._seleccionarButton_ClickAfter);
            this._cancelButton = ((SAPbouiCOM.Button)(this.GetItem("Item_2").Specific));
            this._cancelButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._cancelButton_ClickAfter);
            this._rendicionDataTable = this.UIAPIRawForm.DataSources.DataTables.Item("DT_RE");
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new SAPbouiCOM.Framework.FormBase.ResizeAfterHandler(this.Form_ResizeAfter);
            this.CloseAfter += new CloseAfterHandler(this.Form_CloseAfter);

        }

        private SAPbouiCOM.Matrix _rendicionesMatrix;

        private void OnCustomInitialize()
        {
            _registroComprobanteDomain = FormHelper.GetDomain<RegistroComprobanteDomain>();
            _rendicionMatrixBuilder = new MatrixBuilder<REC1>(_rendicionesMatrix, _rendicionDataTable)
            .ApplySingleRowMode()
            .AddAutoincrementColumn()
            .AddNonEditableEditTextColumn(@"Código", t => t.CodigoRendicion)
            .AddNonEditableEditTextColumn(@"Nro Doc. EMpleado", t => t.NroDocEmpleado)
            .AddNonEditableEditTextColumn(@"Descripción", t => t.Descripcion)
            .AddNonEditableEditTextColumn(@"Monto", t => t.Monto)
            .AddNonEditableEditTextColumn(@"Fecha Inicio", t => t.FechaInicio)
            .AddNonEditableEditTextColumn(@"Fecha Fin", t => t.FechaFin)
            //.AddNonEditableEditTextColumn(@"Pago", t => t.DocEntryPago)
            ;

            //UIAPIRawForm.Maximize();
            UIAPIRawForm.Title = DESCRIPTION;

            populate_matrix();
        }

        private void populate_matrix()
        {
            IEnumerable<REC1> rendiciones = _registroComprobanteDomain
                .RetrieveRendicionesActivas(RegistroComprobante.tipoRendicion)
                //.Select(t => new REC1
                //{
                //    CodigoRendicion = t.CodigoRendicion,
                //    NroDocEmpleado = t.NroDocEmpleado,
                //    Descripcion = t.Descripcion,
                //    Monto=t.Monto,
                //    FechaInicio = t.FechaInicio,
                //    FechaFin = t.FechaFin,
                //    DocEntryPago = t.doc
                //    //OriginFlowDescription = t.OriginFlowDescription,
                //    //DeliveryDate = t.DistributionDate ?? DateTime.Now,
                //    //StartHour = (t.StartHour ?? DateTime.Now).ToString("HH:mm"),
                //    //EndHour = (t.EndHour ?? DateTime.Now).ToString("HH:mm"),
                //    //NroDocEmpleado = t.RouteStatusDescription
                //})
                .OrderByDescending(t => t.CodigoRendicion);
            _rendicionMatrixBuilder.SyncData(rendiciones);
        }

        private SAPbouiCOM.Button _seleccionarButton;
        private SAPbouiCOM.Button _cancelButton;
        private DataTable _rendicionDataTable;
        private string tipoRendicion;
       

        private void _cancelButton_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            UIAPIRawForm.Close();

        }

        private void _seleccionarButton_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            int selectedRow = _rendicionesMatrix.GetNextSelectedRow();
            var editText = _rendicionesMatrix.GetCellSpecific(1, selectedRow).To<SAPbouiCOM.EditText>();
            _selectDocumentEntry = editText.Value;
            GenericHelper.ReleaseCOMObjects(editText);
            UIAPIRawForm.Close();

        }

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {
            _rendicionMatrixBuilder.AutoResizeColumns();

        }

        private void Form_CloseAfter(SBOItemEventArg pVal)
        {
            if (_selectDocumentEntry != null)
            {
                _selectDocumentEntry.IsNotDefaultThen(i =>
                {
                    var list = _rendicionMatrixBuilder.SyncedData.Where(t => t.CodigoRendicion == _selectDocumentEntry).ToList();
                    if (list.Count > 1)
                    {
                        throw new Exception("El código se encuentra en más de un pago");
                    }
                    

                    REC1 selectedRendicion = _rendicionMatrixBuilder.SyncedData.Single(t => t.CodigoRendicion == _selectDocumentEntry);
                    //OnSelectedRendicion?.Invoke(selectedRoute.DocumentEntry);
                    OnSelectedRendicion?.Invoke(_selectDocumentEntry);
                });
            }
           

        }

        private void _seleccionarButton_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            int selectedRow = _rendicionesMatrix.GetNextSelectedRow();
            if (selectedRow == -1)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage("[Error] Se debe seleccionar una fila.", SAPbouiCOM.BoMessageTime.bmt_Short);
                BubbleEvent = false;
            }
            else
            {
                BubbleEvent = true;
            }

        }
    }
}

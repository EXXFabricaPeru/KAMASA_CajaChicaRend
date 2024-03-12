﻿using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Utilities;
using Exxis.Addon.RegistroCompCCRR.Domain;
using Exxis.Addon.RegistroCompCCRR.Domain.Contracts;
using Exxis.Addon.RegistroCompCCRR.Interface.Utilities;
using Exxis.Addon.RegistroCompCCRR.Interface.Views.Modal;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exxis.Addon.RegistroCompCCRR.Interface.Views.UserObjectViews
{
    [FormAttribute("UDO_FT_EXX_RCCR_ORCR")]
    public class RegistroComprobante : UDOFormBase
    {
        public RegistroComprobante()
        {
        }

        private IInfrastructureDomain _infrastructureDomain;
        private IRegistroComprobanteDomain _registroComprobanteDomain;
        private ISettingsDomain _settingsDomain;
        private IMarketingDocumentDomain _marketingDocumentDomain;
        public static RegistroComprobante Instance { get; private set; }

        public const string SAP_FORM_TYPE = "UDO_FT_EXX_RCCR_ORCR";

        private REC1 _selectedRendicion;

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this._codeEditText = ((SAPbouiCOM.EditText)(this.GetItem("0_U_E").Specific));
            this._registrarButton = ((SAPbouiCOM.Button)(this.GetItem("Item_0").Specific));
            this._registrarButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._registrarButton_ClickAfter);
            this._liquidarButton = ((SAPbouiCOM.Button)(this.GetItem("Item_1").Specific));
            this._liquidarButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._liquidarButton_ClickAfter);
            this._tipoComboBox = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_2").Specific));
            this._detailMatrix = ((SAPbouiCOM.Matrix)(this.GetItem("0_U_G").Specific));
            this._detailMatrix.KeyDownAfter += new SAPbouiCOM._IMatrixEvents_KeyDownAfterEventHandler(this._detailMatrix_KeyDownAfter);
            this._detailMatrix.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this._detailMatrix_ChooseFromListAfter);
            this._detailMatrix.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this._detailMatrix_ValidateAfter);
            this._detailMatrix.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this._detailMatrix_ValidateBefore);
            this._detailMatrix.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this._detailMatrix_ClickBefore);
            this._detailMatrix.ChooseFromListBefore += new SAPbouiCOM._IMatrixEvents_ChooseFromListBeforeEventHandler(this._detailMatrix_ChooseFromListBefore);
            this._sucursalComboBox = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_3").Specific));
            this._searchRendicionButton = ((SAPbouiCOM.Button)(this.GetItem("Item_4").Specific));
            this._nroRendicionEditText = ((SAPbouiCOM.EditText)(this.GetItem("14_U_E").Specific));
            this._descripcionEditText = ((SAPbouiCOM.EditText)(this.GetItem("15_U_E").Specific));
            this._empleadoEditText = ((SAPbouiCOM.EditText)(this.GetItem("16_U_E").Specific));
            this._montoEditText = ((SAPbouiCOM.EditText)(this.GetItem("17_U_E").Specific));
            this._totalGastoEditText = ((SAPbouiCOM.EditText)(this.GetItem("18_U_E").Specific));
            this._saldoEditText = ((SAPbouiCOM.EditText)(this.GetItem("19_U_E").Specific));
            this._searchRendicionButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._searchRendicionButton_ClickAfter);
            this._crearButton = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataLoadAfter += new DataLoadAfterHandler(this.Form_DataLoadAfter);

        }

        private EditText _codeEditText;
        private SAPbouiCOM.Button _registrarButton;

        private void OnCustomInitialize()
        {
            try
            {
                _infrastructureDomain = FormHelper.GetDomain<InfrastructureDomain>();
                _registroComprobanteDomain = FormHelper.GetDomain<RegistroComprobanteDomain>();
                _settingsDomain = FormHelper.GetDomain<SettingsDomain>();
                _marketingDocumentDomain = FormHelper.GetDomain<MarketingDocumentDomain>();

                Instance = this;



                if (UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE)
                {
                    _codeEditText.Value = _registroComprobanteDomain.RetrieveCodigoGenerado(); //DateTime.Now.Year.ToString();
                }
                else
                {
                    _codeEditText.Item.Enabled = false;
                    _tipoComboBox.Item.Enabled = false;
                }

                FillComboBox();
                loadSapFields(null);

                _detailMatrix.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage("Iniciar formulario: " + ex.Message);
            }
            _detailMatrix.FlushToDataSource();

        }

        public void ClearFormToAddMode()
        {
            SAPbouiCOM.CommonSetting commonSetting = null;
            SAPbouiCOM.Item item = null;
            try
            {
                _codeEditText.Value = _registroComprobanteDomain.RetrieveCodigoGenerado();
            }
            catch
            {

            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(commonSetting, item);
            }
        }

        private void FillComboBox()
        {
            ValidValues tipoRendicionValidValues = null;
            ValidValues sucursalesValidValues = null;
            try
            {
                IEnumerable<Tuple<string, string>> tipoRendicion = _infrastructureDomain.RetrieveValidValues<RECC, string>(t => t.Tipo);
                tipoRendicionValidValues = _tipoComboBox.ValidValues;

                foreach (Tuple<string, string> flow in tipoRendicion.Where(t => t.Item2.ToUpper().Contains("RENDI") || t.Item2.ToUpper().Contains("CAJA")).ToList())
                    tipoRendicionValidValues.Add(flow.Item1, flow.Item2);


                IEnumerable<Tuple<string, string>> sucursales = _registroComprobanteDomain.RetrieveSucursales();
                sucursalesValidValues = _sucursalComboBox.ValidValues;

                foreach (Tuple<string, string> flow in sucursales)
                    sucursalesValidValues.Add(flow.Item1, flow.Item2);
            }
            catch (Exception ex)
            {

                ApplicationInterfaceHelper.ShowErrorStatusBarMessage("FillComboBox: " + ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(tipoRendicionValidValues);
            }



        }
        private SAPbouiCOM.Button _liquidarButton;
        private ComboBox _tipoComboBox;
        private Matrix _detailMatrix;
        private ComboBox _sucursalComboBox;
        private Button _searchRendicionButton;
        private EditText _nroRendicionEditText;
        private EditText _descripcionEditText;
        private EditText _empleadoEditText;
        private EditText _montoEditText;
        private EditText _totalGastoEditText;
        private EditText _saldoEditText;
        private static SearchRendicionModal _modal1;
        public static string tipoRendicion = "";

        private void loadSapFields(SBOItemEventArg pVal)
        {
            SAPbouiCOM.ComboBox comboBox = null;
            SAPbouiCOM.ValidValues validValues = null;
            int row = 0;
            if (pVal == null)
            {
                row = 1;
            }
            else
            {
                row = pVal.Row;
            }
            comboBox = _detailMatrix.GetCellSpecific(ColumnaCondicionPago, row).To<SAPbouiCOM.ComboBox>();
            validValues = comboBox.ValidValues;
            validValues.RemoveValidValues();
            validValues.Add(string.Empty, string.Empty);

            _infrastructureDomain.RetrievePaymentGroup()
                .ForEach(t => validValues.Add(t.Item1, t.Item2));
        }

        private void _searchRendicionButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (_tipoComboBox.Selected == null)
                {
                    ApplicationInterfaceHelper.ShowErrorStatusBarMessage("Debe elegir un tipo primero");
                    return;
                }
                tipoRendicion = _tipoComboBox.Selected.Value;
                _modal1 = new SearchRendicionModal();
                _modal1.OnSelectedRendicion += delegate (string documentEntry) { _nroRendicionEditText.Value = documentEntry.ToString(); };
                _modal1.CloseAfter += delegate (SBOItemEventArg eventArg)
                {
                    _modal1 = null;
                    sync_headers_from_rendicion_entry_edit_text();
                };

                _modal1.Show();
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage("searchRendicionButton: " + ex.Message);
            }

        }

        private void sync_headers_from_rendicion_entry_edit_text()
        {
            try
            {
                if (string.IsNullOrEmpty(_nroRendicionEditText.Value))
                    return;

                var documentEntry = _nroRendicionEditText.Value;
                _selectedRendicion = _registroComprobanteDomain.RetrieveRendicionByCode(documentEntry);
                if (_selectedRendicion != null)
                {
                    populate_header();

                }
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage("sync_headers_from_rendicion_entry_edit_text: " + ex.Message);
            }
        }

        private void populate_header()
        {
            _descripcionEditText.Value = _selectedRendicion.Descripcion;
            _empleadoEditText.Value = _selectedRendicion.NombreEmpleado;
            _montoEditText.Value = _selectedRendicion.Monto.ToString();
            _saldoEditText.Value = _selectedRendicion.Monto.ToString();
        }

        private static string ColumnaCodigoProveedor = "C_0_1";
        private static string ColumnaNombreProveedor = "C_0_2";
        private static string ColumnaFechaDoc = "C_0_3";
        private static string ColumnaFechaContable = "C_0_4";
        private static string ColumnaFechaVencimiento = "C_0_5";
        private static string ColumnaCondicionPago = "C_0_6";
        private static string ColumnaTipoDocumento = "C_0_7";
        private static string ColumnaSerie= "C_0_8";
        private static string ColumnaNumFolio = "C_0_9";
        private static string ColumnaCodigoGasto = "C_0_10";
        private static string ColumnaCuentaGasto = "C_0_11";
        private static string ColumnaDimension1 = "C_0_12";
        private static string ColumnaDimension3 = "C_0_13";
        private static string ColumnaMoneda= "C_0_14";
        private static string ColumnaValorUnitario = "C_0_15";
        private static string ColumnaImpuesto = "C_0_16";
        private static string ColumnaImpuestoPorcentaje = "C_0_17";
        private static string ColumnaTotal = "C_0_18";
        private static string ColumnaEstado = "C_0_19";
        private static string ColumnaDocEntry = "C_0_20";
        private static string ColumnaDescripcionGasto = "Col_0";


        RCR1 lineGrilla = new RCR1();
        private Button _crearButton;

        private void _detailMatrix_ChooseFromListBefore(object sboObject, SBOItemEventArg eventArgs, out bool BubbleEvent)
        {
            BubbleEvent = true;

            SAPbouiCOM.ChooseFromList ChooseFromListConditions = null;
            SAPbouiCOM.Conditions conditions = null;
            SAPbouiCOM.Condition condition = null;
            lineGrilla = new RCR1();
            try
            {
                if (eventArgs.ColUID == ColumnaCodigoProveedor)//Codigo proveedor
                {
                    conditions = ApplicationInterfaceHelper.ApplicationInstance
                    .CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                    .To<SAPbouiCOM.Conditions>();

                    condition = conditions.Add();
                    condition.Alias = @"CardType";
                    condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    condition.CondVal = "S";
                    //condition.Relationship = BoConditionRelationship.cr_OR;

                    //condition = conditions.Add();
                    //condition.Alias = @"U_VS_LT_PROV";
                    //condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    //condition.CondVal = "Y";


                    ChooseFromListConditions = UIAPIRawForm.ChooseFromLists.Item(@"CFL_SN");
                    ChooseFromListConditions.SetConditions(conditions);

                    //var fechaDoc = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaFechaDoc).Cells.Item(eventArgs.Row).Specific;
                    //lineGrilla.FechaDoc = fechaDoc.GetDateTimeValue();
                }
                else if (eventArgs.ColUID == ColumnaTipoDocumento)
                {
                    conditions = ApplicationInterfaceHelper.ApplicationInstance
                  .CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                  .To<SAPbouiCOM.Conditions>();

                    condition = conditions.Add();
                    condition.Alias = @"Code";
                    condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    condition.CondVal = "01";
                    condition.Relationship = BoConditionRelationship.cr_OR;

                    condition = conditions.Add();
                    condition.Alias = @"Code";
                    condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    condition.CondVal = "03";


                    ChooseFromListConditions = UIAPIRawForm.ChooseFromLists.Item(@"CFL_TD");
                    ChooseFromListConditions.SetConditions(conditions);
                }
                else if (eventArgs.ColUID == ColumnaDimension1)
                {
                    conditions = ApplicationInterfaceHelper.ApplicationInstance
                  .CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                  .To<SAPbouiCOM.Conditions>();

                    condition = conditions.Add();
                    condition.Alias = @"DimCode";
                    condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    condition.CondVal = "1";

                    ChooseFromListConditions = UIAPIRawForm.ChooseFromLists.Item(@"CFL_D1");
                    ChooseFromListConditions.SetConditions(conditions);
                }
                else if (eventArgs.ColUID == ColumnaDimension3)
                {
                    conditions = ApplicationInterfaceHelper.ApplicationInstance
                  .CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                  .To<SAPbouiCOM.Conditions>();

                    condition = conditions.Add();
                    condition.Alias = @"DimCode";
                    condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    condition.CondVal = "3";

                    ChooseFromListConditions = UIAPIRawForm.ChooseFromLists.Item(@"CFL_D3");
                    ChooseFromListConditions.SetConditions(conditions);
                }

            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
                //throw;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(ChooseFromListConditions, condition, conditions);
            }

        }

        private void _detailMatrix_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            _detailMatrix.FlushToDataSource();

        }

        private void _detailMatrix_ValidateBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            _detailMatrix.FlushToDataSource();

        }

        private void _detailMatrix_ValidateAfter(object sboObject, SBOItemEventArg eventArgs)
        {
            try
            {
                //var codigoSocio = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaCodigoProveedor).Cells.Item(eventArgs.Row).Specific;
                //if (!string.IsNullOrEmpty(codigoSocio.Value))
                //{

                //    var _item = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaNombreProveedor).Cells.Item(eventArgs.Row).Specific;
                //    if (string.IsNullOrEmpty(_item.Value))
                //    {
                //        var _socio = _infrastructureDomain.RetrieveBusinessPartner(codigoSocio.Value.ToString());
                //        _item.Value = _socio.CardName;
                //    }
                //}



                ActualizarSaldo();
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }

            _detailMatrix.FlushToDataSource();

        }

        private void ActualizarSaldo()
        {
            ActualizarTotalGasto();
            _saldoEditText.Value = (_montoEditText.Value.ToDouble() - _totalGastoEditText.Value.ToDouble()).ToString();

        }
        private void ActualizarTotalGasto()
        {
            double total = 0;
            for (int i = 1; i <= _detailMatrix.RowCount; i++)
            {
                var totalLinea = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaTotal).Cells.Item(i).Specific;
                if (!string.IsNullOrEmpty(totalLinea.Value))
                {
                    total += totalLinea.Value.ToDouble();
                }

            }

            _totalGastoEditText.Value = total.ToString();

        }

        private void _detailMatrix_ChooseFromListAfter(object sboObject, SBOItemEventArg eventArgs)
        {
            try
            {
                if (eventArgs.ColUID == ColumnaCodigoProveedor)
                {
                    SAPbouiCOM.DataTable selectedObjects = eventArgs.To<SAPbouiCOM.ISBOChooseFromListEventArg>().SelectedObjects;
                    if (selectedObjects == null)
                        return;
                    object value = selectedObjects.GetValue("CardCode", 0);
                    string nameProveedor = selectedObjects.GetValue("CardName", 0).ToString();


                    var _item = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaNombreProveedor).Cells.Item(eventArgs.Row).Specific;
                    _item.Value = nameProveedor;
                    _detailMatrix.FlushToDataSource();
                    _detailMatrix.AutoResizeColumns();
                    GenericHelper.ReleaseCOMObjects(selectedObjects);

                }
                else if (eventArgs.ColUID == ColumnaCodigoGasto)
                {
                    SAPbouiCOM.DataTable selectedObjects = eventArgs.To<SAPbouiCOM.ISBOChooseFromListEventArg>().SelectedObjects;
                    if (selectedObjects == null)
                        return;
                    //object value = selectedObjects.GetValue("CardCode", 0);
                    string cuentacontable = selectedObjects.GetValue("U_EXX_CUECON", 0).ToString();
                    string descripcion = selectedObjects.GetValue("Name", 0).ToString();


                    var _item = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaCuentaGasto).Cells.Item(eventArgs.Row).Specific;
                    _item.Value = cuentacontable;
                    _detailMatrix.FlushToDataSource();

                    var _desc = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaDescripcionGasto).Cells.Item(eventArgs.Row).Specific;
                    _desc.Value = descripcion;
                    _detailMatrix.FlushToDataSource();

                    _detailMatrix.AutoResizeColumns();
                    GenericHelper.ReleaseCOMObjects(selectedObjects);

                }
                else if (eventArgs.ColUID == ColumnaImpuesto)
                {
                    SAPbouiCOM.DataTable selectedObjects = eventArgs.To<SAPbouiCOM.ISBOChooseFromListEventArg>().SelectedObjects;
                    if (selectedObjects == null)
                        return;
                    //object value = selectedObjects.GetValue("CardCode", 0);
                    string porcentaje = selectedObjects.GetValue("Rate", 0).ToString();


                    var item = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaImpuestoPorcentaje).Cells.Item(eventArgs.Row).Specific;
                    item.Value = porcentaje;

                    var valor = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaValorUnitario).Cells.Item(eventArgs.Row).Specific;
                    var total = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaTotal).Cells.Item(eventArgs.Row).Specific;

                    var impuesto = (valor.Value.ToDouble() * porcentaje.ToDouble() / 100);
                    total.Value = (valor.Value.ToDouble() + impuesto).ToString();

                    _detailMatrix.FlushToDataSource();
                    ActualizarSaldo();
                    _detailMatrix.AutoResizeColumns();
                    GenericHelper.ReleaseCOMObjects(selectedObjects);

                }
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }

        }

        private void _detailMatrix_KeyDownAfter(object sboObject, SBOItemEventArg eventArgs)
        {
            try
            {
                if (eventArgs.ColUID == ColumnaValorUnitario || eventArgs.ColUID == ColumnaImpuestoPorcentaje)
                {


                    var valor = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaValorUnitario).Cells.Item(eventArgs.Row).Specific;
                    var porcentaje = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaImpuestoPorcentaje).Cells.Item(eventArgs.Row).Specific;
                    var total = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaTotal).Cells.Item(eventArgs.Row).Specific;

                    var impuesto = (valor.Value.ToDouble() * porcentaje.Value.ToDouble() / 100);
                    total.Value = (valor.Value.ToDouble() + impuesto).ToString();


                    _detailMatrix.FlushToDataSource();
                    ActualizarSaldo();
                    //_detailMatrix.AutoResizeColumns();
                    GenericHelper.ReleaseCOMObjects();

                }
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }

        }

        private void _registrarButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (UIAPIRawForm.IsAddMode())
                    throw new Exception("Primero debe crear el registro");

                if (string.IsNullOrEmpty(_nroRendicionEditText.Value))
                    throw new Exception("Debe seleccionar primero una rendición/caja chica");

                if (string.IsNullOrEmpty(_sucursalComboBox.Value))
                    throw new Exception("Debe seleccionar una sucursal");

                ApplicationInterfaceHelper.ShowDialogMessageBox("¿Está seguro de registrar los comprobantes?",
                    () =>
                    {
                        _registrarComprobantes();
                    },
                    null);
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }

        }

        private void _registrarComprobantes()
        {
            try
            {
                for (int i = 1; i <= _detailMatrix.RowCount; i++)
                {
                    var estado = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaEstado).Cells.Item(i).Specific;
                    try
                    {
                        if (estado.Value != "Si")
                        {
                            OPCH factura = new OPCH();

                            factura.CardCode = ((SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaCodigoProveedor).Cells.Item(i).Specific).Value;
                            factura.DocumentDate = ((SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaFechaContable).Cells.Item(i).Specific).GetDateTimeValue();
                            factura.TaxDate = ((SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaFechaDoc).Cells.Item(i).Specific).GetDateTimeValue();
                            factura.DocumentDeliveryDate = ((SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaFechaVencimiento).Cells.Item(i).Specific).GetDateTimeValue();
                            factura.FolioPref = ((SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaSerie).Cells.Item(i).Specific).Value;
                            factura.FolioNum = ((SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaNumFolio).Cells.Item(i).Specific).Value.ToInt32();
                            factura.Indicator = ((SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaTipoDocumento).Cells.Item(i).Specific).Value;
                            factura.CondicionPago = ((SAPbouiCOM.ComboBox)_detailMatrix.Columns.Item(ColumnaCondicionPago).Cells.Item(i).Specific).Value.ToInt32();
                            factura.BranchId = _sucursalComboBox.Value.ToInt32();
                            factura.NumberAtCard = factura.FolioPref + "-" + factura.FolioNum;
                            factura.Comments = "Factura generada por el Addon de Registro Comprobante Rend/CC - Nro " + _nroRendicionEditText.Value;
                            factura.Currency = ((SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaMoneda).Cells.Item(i).Specific).Value;
                            factura.Type = "S";

                            List<PCH1> lines = new List<PCH1>();
                            PCH1 line = new PCH1();


                            line.ItemDescription = ((SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaDescripcionGasto).Cells.Item(i).Specific).Value;
                            line.TotalPrice = ((SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaValorUnitario).Cells.Item(i).Specific).Value.ToDecimal();
                            line.TaxCode = ((SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaImpuesto).Cells.Item(i).Specific).Value;
                            line.Cuenta = ((SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaCuentaGasto).Cells.Item(i).Specific).Value;
                            line.CodServicioCompra = ((SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaCodigoGasto).Cells.Item(i).Specific).Value;
                            line.GrupoDetraccion = "999";
                            line.CentroCosto = ((SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaDimension1).Cells.Item(i).Specific).Value;
                            line.CentroCosto3 = ((SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaDimension3).Cells.Item(i).Specific).Value;
                            lines.Add(line);

                            factura.DocumentLines = lines;

                            var respuesta = _marketingDocumentDomain.RegisterPurchaseInvoice(factura);

                            if (respuesta.Item1)
                            {

                                estado.Value = "Si";
                                _detailMatrix.FlushToDataSource();

                                var docentry = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaDocEntry).Cells.Item(i).Specific;
                                docentry.Value = respuesta.Item2;
                                _detailMatrix.FlushToDataSource();

                                ActualizarEstado(factura.CardCode+"-"+factura.FolioPref+"-"+factura.FolioNum, "Si", respuesta.Item2);
                            }
                            else
                            {
                                estado.Value = "No";
                                _detailMatrix.FlushToDataSource();
                                ActualizarEstado(factura.CardCode + "-" + factura.FolioPref + "-" + factura.FolioNum, "No", "");
                                ApplicationInterfaceHelper.ShowErrorStatusBarMessage("Error al registrar línea " + i + " - " + factura.FolioPref + "-" + factura.FolioPref + ": " + respuesta.Item2);
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        estado.Value = "No";
                        _detailMatrix.FlushToDataSource();
                        //ActualizarEstado(i, "No", "");
                        ApplicationInterfaceHelper.ShowErrorStatusBarMessage("Error al registrar línea " + i + " - "+ ex.Message);
                    }
                   
                }

                if (_crearButton.Caption != "OK")
                {
                    _crearButton.Item.Click();
                    
                }
                  

            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        private void ActualizarEstado(string line,string estado, string docentry)
        {
            try
            {
                _registroComprobanteDomain.ActualizarEstadoLinea(_codeEditText.Value, line, estado, docentry);
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        private void _liquidarButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (UIAPIRawForm.IsAddMode())
                    throw new Exception("Primero debe crear el registro");

                if (string.IsNullOrEmpty(_nroRendicionEditText.Value))
                    throw new Exception("Debe seleccionar primero una rendición/caja chica");

                if (string.IsNullOrEmpty(_sucursalComboBox.Value))
                    throw new Exception("Debe seleccionar una sucursal");



                ApplicationInterfaceHelper.ShowDialogMessageBox("¿Está seguro de liquidar la rendición/caja chica?, Luego de Liquidar no podrá revertir los cambios por el addon",
                   () =>
                   {
                       _liquidarRendicion();
                   },
                   null);
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }

        }

        private void _liquidarRendicion()
        {
            try
            {

                OITR reconcilicaion = new OITR();
                OJDT asientoRecon = new OJDT();
                asientoRecon.TransactionCode = "ASM";
                asientoRecon.TaxDate = DateTime.Now;
                asientoRecon.DueDate = DateTime.Now;
                asientoRecon.ReferenceDate = DateTime.Now;
                asientoRecon.Memo = "Asiento de reconciliación";



                List<JDT1> lines = new List<JDT1>();
                for (int i = 1; i <= _detailMatrix.RowCount; i++)
                {
                    JDT1 line = new JDT1();

                    line.ShortName = "E00045853455";
                    line.AccountCode = "_SYS00000001366";
                    line.Credit = 550;

                    lines.Add(line);

                }
                //JDT1 line = new JDT1();

                //line.ShortName = "E00045853455";
                //line.AccountCode = "_SYS00000001366";
                //line.Credit = 550;
                //line.BPLID = _sucursalComboBox.Value.ToInt32();

                //lines.Add(line);

                //JDT1 line2 = new JDT1();

                //line2.ShortName = "P10088735051";
                //line2.AccountCode = "_SYS00000001366";
                //line2.Debit = 550;
                //line2.BPLID = _sucursalComboBox.Value.ToInt32();

                //lines.Add(line2);

                asientoRecon.JournalEntryLines = lines;
                var asiento = _registroComprobanteDomain.generarAsiento(asientoRecon);

                //var respuesta = _registroComprobanteDomain.GenerarReconciliacion(reconcilicaion);
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }


        private void Form_DataLoadAfter(ref BusinessObjectInfo pVal)
        {
            _detailMatrix.AutoResizeColumns();
        }
        //var fechaDoc = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item(ColumnaFechaDoc).Cells.Item(eventArgs.Row).Specific;
        //fechaDoc.Value = lineGrilla.FechaDoc.ToString("yyyyMMdd");


    }
}

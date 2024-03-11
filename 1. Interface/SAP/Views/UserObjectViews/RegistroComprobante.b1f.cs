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
            this._liquidarButton = ((SAPbouiCOM.Button)(this.GetItem("Item_1").Specific));
            this._tipoComboBox = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_2").Specific));
            this._detailMatrix = ((SAPbouiCOM.Matrix)(this.GetItem("0_U_G").Specific));
            this._detailMatrix.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this._detailMatrix_ChooseFromListAfter);
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
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
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

                _detailMatrix.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage("Iniciar formulario: " + ex.Message);
            }
            
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
            ValidValues tipoRendicionValidValues=null;
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
        public static string tipoRendicion="";
        private void _searchRendicionButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if(_tipoComboBox.Selected==null)
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
            catch (Exception ex )
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

        private void _detailMatrix_ChooseFromListBefore(object sboObject, SBOItemEventArg eventArgs, out bool BubbleEvent)
        {
            BubbleEvent = true;

            SAPbouiCOM.ChooseFromList socioTextChooseFromList = null;
            SAPbouiCOM.Conditions conditions = null;
            SAPbouiCOM.Condition condition = null;
            try
            {
                if (eventArgs.ColUID == "C_0_1")//Codigo proveedor
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


                    socioTextChooseFromList = UIAPIRawForm.ChooseFromLists.Item(@"CFL_SN");
                    socioTextChooseFromList.SetConditions(conditions);
                }
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
                //throw;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(socioTextChooseFromList, condition, conditions);
            }

        }

        private void _detailMatrix_ChooseFromListAfter(object sboObject, SBOItemEventArg eventArgs)
        {
            try
            {
                //if (eventArgs.ColUID == "C_0_1")
                //{
                //    SAPbouiCOM.DataTable selectedObjects = eventArgs.To<SAPbouiCOM.ISBOChooseFromListEventArg>().SelectedObjects;
                //    if (selectedObjects == null)
                //        return;
                //    object value = selectedObjects.GetValue("CardCode", 0);


                //    var _socio = _infrastructureDomain.RetrieveBusinessPartner(value.ToString());
                //    var _item = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item("C_0_2").Cells.Item(eventArgs.Row).Specific;
                //    _item.Value = _socio.CardName;


                    
                //    var codigoSocio = (SAPbouiCOM.EditText)_detailMatrix.Columns.Item("C_0_1").Cells.Item(eventArgs.Row).Specific;
                //    codigoSocio.Value = value.ToString();
                //}

            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }

        }
    }
}

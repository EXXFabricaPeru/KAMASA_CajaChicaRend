﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Code.Models;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Constant;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Model.UDO.Header;
using Exxis.Addon.RegistroCompCCRR.CrossCutting.Utilities;
using Exxis.Addon.RegistroCompCCRR.Data.Code;
using Exxis.Addon.RegistroCompCCRR.Data.Repository;
using Exxis.Addon.RegistroCompCCRR.Domain.Code;
using Exxis.Addon.RegistroCompCCRR.Domain.Contracts;

namespace Exxis.Addon.RegistroCompCCRR.Domain
{
    public class InfrastructureDomain : BaseDomain, IInfrastructureDomain
    {
        public InfrastructureDomain(SAPbobsCOM.Company company) : base(company)
        {
        }

        public IEnumerable<OMTT> RetrieveApprovalReasons()
        {
            return RetrieveReasons().Where(t => t.Type == OMTT.Types.APPROVED);
        }

        public IEnumerable<OMTT> RetrieveDisapprovalReasons()
        {
            return RetrieveReasons().Where(t => t.Type == OMTT.Types.DISAPPROVED);
        }

        public IEnumerable<OMTT> RetrieveReasons()
        {
            return UnitOfWork.InfrastructureRepository.RetrieveReasons();
        }

        public OMTT RetrieveDisapprovalReasonByCode(string code)
        {
            return UnitOfWork.InfrastructureRepository.RetrieveDisapprovalReasonByCode(code);
        }

        public ODSP RetrieveLoadPriorityByLoadType(string loadType)
        {
            return UnitOfWork.LoadChargePriorityRepository.RetrieveAll().Single(t => t.ItemLoadType == loadType);
        }

        public string RetrieveDescriptionOfValidValueCode(string field, string code)
        {
            return UnitOfWork.InfrastructureRepository.RetrieveDescriptionOfValidValueCode(field, code);
        }

        public string RetrieveNextDistributionStatus(string code)
        {
            BaseOTDIRepository mappingRepository = UnitOfWork.MappingTableRepository;
            return mappingRepository.MappingValue(OTMI.EntityType.DOCUMENT_DISTRIBUTION_FLOW_STATUS)
                .Single(item => item.OriginValue == code)
                .TargetValue;
        }

        public string RetrieveOppositeDistributionStatus(string code)
        {
            BaseOTDIRepository mappingRepository = UnitOfWork.MappingTableRepository;
            return mappingRepository.MappingValue(OTMI.EntityType.DOCUMENT_DISTRIBUTION_STATUS)
                .Single(item => item.OriginValue == code)
                .TargetValue;
        }

        public string RetrieveOppositeDistributionTransferOrderStatus(string code)
        {
            return UnitOfWork.MappingTableRepository.MappingValue(OTMI.EntityType.TRANSFER_ORDER_STATUS)
                .Single(item => item.OriginValue == code)
                .TargetValue;
        }

        public IEnumerable<Tuple<string, string>> RetrieveSaleChannels()
        {
            return UnitOfWork.InfrastructureRepository.RetrieveSaleChannels();
        }

        public IEnumerable<OTMI> RetrieveSAPDocumentMappingStatus()
        {
            return UnitOfWork.MappingTableRepository
                .MappingValue(OTMI.EntityType.SAP_DOCUMENT_STATUS);
        }

        public CrossCutting.Model.System.Header.OCRD RetrieveBusinessPartner(string cardCode)
        {
            BaseBusinessPartnerRepository businessPartnerRepository = UnitOfWork.BusinessPartnerRepository;
            businessPartnerRepository.SetRetrieveFormat(RetrieveFormat.Complete);
            return businessPartnerRepository.FindByCode(cardCode);
        }

        public IEnumerable<OTMT> RetrievePenaltyMotives()
        {
            return UnitOfWork.TariffMotiveRepository.Retrieve(t => t.Type == MotiveType.PENALTY_CODE);
        }

        public IEnumerable<OTMT> RetrieveExtraMotives()
        {
            return UnitOfWork.TariffMotiveRepository.Retrieve(t => t.Type == MotiveType.EXTRA_CODE);
        }

        public IEnumerable<Tuple<string, string>> RetrieveValidValues<TEntity, TKProperty>(Expression<Func<TEntity, TKProperty>> propertyExpression)
            where TEntity : BaseUDO
        {
            return UnitOfWork.InfrastructureRepository.RetrieveValidValues(propertyExpression);
        }

        public IEnumerable<OEIT> RetrieveMappingValuesByTemplate(string templateCode)
            => UnitOfWork.InfrastructureRepository.RetrieveMappingValuesByTemplate(templateCode);

        public string RetrieveLastNumberBySerie(string Serie, string tipo)
        {
            return UnitOfWork.InfrastructureRepository.RetrieveLastNumberBySerie(Serie, tipo);
        }

        public void SendAlertMessageBySaleOrderCashPayment(int documentEntry)
        {
            SAPbobsCOM.Messages messages = null;
            SAPbobsCOM.Recipients recipients = null;
            try
            {
                IEnumerable<string> users = UnitOfWork.SettingsRepository
                    .Setting(OPDS.Codes.ALERT_USERS_SALE_ORDER_CASH_PAYMENT)
                    .Value
                    .Split(',');

                if (!users.Any())
                    return;

                messages = Company.MakeMessage();
                recipients = messages.Recipients;
                messages.Priority = SAPbobsCOM.BoMsgPriorities.pr_High;
                messages.Subject = @"Alerta de Ordenes de Venta Al Contado";
                messages.MessageText = $@"Se ha creado/modificado la orden de venta  N° {documentEntry} de tipo Al Contado";



                users.ForEach((item, index, lastIteration) =>
                    {
                        recipients.SetCurrentLine(index);
                        recipients.UserCode = item;
                        recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES;
                        if (!lastIteration) recipients.Add();
                    });

                if (messages.Add() == default(int))
                    return;




                SAPError sapError = Company.RetrieveSAPError();
                throw new Exception($"{sapError.Code} - {sapError.Message}");
            }
            catch (Exception)
            {

            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(messages, recipients);
            }
        }

        public List<Tuple<string, string>> RetriveMotiveTransferSoraya()
        {
            return UnitOfWork.InfrastructureRepository.RetriveMotiveTransferSoraya();
        }

        public List<Tuple<string, string>> RetriveMotiveTransferSunat()
        {
            return UnitOfWork.InfrastructureRepository.RetriveMotiveTransferSunat();
        }

        public string RetrievePaymentGroupNumDescription(int code)
        {
            return UnitOfWork.InfrastructureRepository.RetrievePaymentGroupNumDescription(code);
        }

        public void validarTipoCambio(DateTime fchTrn)
        {
            string moneda = "USD";
            var sboBob = (SAPbobsCOM.SBObob)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);

            try
            {
                var tc = sboBob.GetCurrencyRate(moneda, fchTrn);
                if (Convert.ToDouble(tc.Fields.Item(0).Value) <= 0)
                {
                    throw new Exception(
                               $"[Error] Error Orden de Traslado - Generación de Entrega: ACTUALICE TIPO DE CAMBIO");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                               $"[Error] Error Orden de Traslado - Generación de Entrega: ACTUALICE TIPO DE CAMBIO");
                throw;
            }
           
        }

        public decimal obtenerTipoCambio(DateTime fecha)
        {
            string moneda = "USD";
            var sboBob = (SAPbobsCOM.SBObob)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);

            try
            {
                var tc = sboBob.GetCurrencyRate(moneda, fecha);
                var tipoCambio = Convert.ToDecimal(tc.Fields.Item(0).Value);
                if (tipoCambio <= 0)
                {
                    throw new Exception(
                               $"[Error] Error Tipo de Cambio:  ACTUALICE TIPO DE CAMBIO");
                }
                return tipoCambio;
            }
            catch (Exception ex)
            {
                throw new Exception(
                               $"[Error] Error Tipo de Cambio: ACTUALICE TIPO DE CAMBIO");
                throw;
            }
        }

        public IEnumerable<Tuple<string, string>> RetrieveTipoFlujos()
        {
            return UnitOfWork.InfrastructureRepository.RetrieveTipoFlujos();
        }

        public IEnumerable<Tuple<string, string>> RetrievePaymentGroup()
        {
            return UnitOfWork.InfrastructureRepository.RetrievePaymentGroup();
        }

        public string RetrieveAccountByFormatCode(string code)
        {
            return UnitOfWork.InfrastructureRepository.RetrieveAccountCodeByActID(code);
        }
    }
}
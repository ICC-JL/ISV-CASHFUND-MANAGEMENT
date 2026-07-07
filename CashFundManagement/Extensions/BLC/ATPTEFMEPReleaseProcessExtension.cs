using System;
using System.Collections.Generic;
using System.Linq;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.CA;
using PX.Objects.CM;
using PX.Objects.Common.Extensions;
using PX.Objects.Common.GraphExtensions.Abstract;
using PX.Objects.Common.GraphExtensions.Abstract.DAC;
using PX.Objects.Common.GraphExtensions.Abstract.Mapping;
using PX.Objects.CR;
using PX.Objects.CT;
using PX.Objects.GL;
using PX.Objects.GL.FinPeriods;
using PX.Objects.PM;
using PX.Objects.TX;
using PX.Objects.EP;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.Objects.CN.Common.Extensions;
using static PX.Objects.AP.APReleaseProcess;
using static PX.Objects.EP.EPReleaseProcess;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Attributes;
using CashFundManagement.DAC.Setup;
using CashFundManagement.DAC;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using CashFundManagement.Helper;

namespace CashFundManagement.Extensions.BLC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// 2024-11-06 : Change the implementation of populating necessary fields using event handler. {RRS} <br/>
    ///     Reason : The previously copied code is from 23R2, multiple changes were made on the succeeding Acumatica versions.
    /// 20204-12-18: 009293 - [PROD] Time & Expense Preferences - Require Approval in RFP Bill (New implementation causing EC approval issues)
    /// </remarks>
    public class ATPTEFMEPReleaseProcess_Extension : PXGraphExtension<EPReleaseProcess>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif
        #region Views
        public PXSetup<ATPTEFMCASetup> ATPTEFMPreferences;
        public PXSetup<ATPTEFMCASetup> ATPTEFMCAPreference;
        #endregion


        #region New Implementation
        /// <remarks>
        /// 2025-09-11: CFM Expense Claims - Error in releasing RFP CASE:013442 {JLTG} <br/>
        /// </remarks>
        public delegate void ReleaseDocProcDelegate(EPExpenseClaim claim);
        [PXOverride]
        public void ReleaseDocProc(EPExpenseClaim claim, ReleaseDocProcDelegate baseMethod)
        {
            ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();
            var transactionType = claimExt.UsrATPTEFMTranType;
            Location location = transactionType == ATPTEFMExpenseTypeAttribute.RequestforPayment ? 
                SelectFrom<Location>.Where<Location.bAccountID.IsEqual<@P.AsInt>
                .And<Location.isActive.IsEqual<True>>.And<Location.isDefault.IsEqual<True>>>.View.Select(Base, claimExt.UsrATPTVendorID) :
                SelectFrom<Location>.Where<Location.bAccountID.IsEqual<@P.AsInt>.And<Location.locationID.IsEqual<@P.AsInt>>>
                .View.Select(Base, claim.EmployeeID, claim.LocationID);

            InvoiceGraphHandler(claim, claimExt, transactionType, location); //Event Listener when invoice graph is created on baseMethod.

            baseMethod(claim);

            EPSetup epsetup = PXSelectReadonly<EPSetup>.Select(Base);

            PXResultset<APInvoice> invoices = PXSelect<APInvoice,
                Where<APInvoice.origModule, Equal<BatchModule.moduleEP>,
                    And<APInvoice.origRefNbr, Equal<Required<APInvoice.origRefNbr>>>>>
                .Select(Base, claim.RefNbr);

            if (invoices != null && invoices.Count > 0)
            {
                ATPTEFMEPSetupExtension aTPTEFMEPSetup = epsetup.GetExtension<ATPTEFMEPSetupExtension>();
                APInvoiceEntry invoiceEntry = PXGraph.CreateInstance<APInvoiceEntry>();

                foreach (APInvoice invoice in invoices)
                {
                    invoiceEntry.Clear();
                    invoiceEntry.Document.Current = invoice;

                    if (transactionType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                    {
                        if ((aTPTEFMEPSetup?.UsrATPTEFMIsRequireApprovalRFPBill ?? false))
                        {
                            invoiceEntry.Approval.SuppressApproval = false;
                            invoice.Hold = true;
                            invoice.Status = "H";
                            invoiceEntry.Document.Update(invoice);
                            invoiceEntry.Actions.PressSave();
                        }
                    }
                    else if (transactionType == ATPTEFMExpenseTypeAttribute.Liquidation)
                    {
                        if ((ATPTEFMPreferences?.Current?.IsRequireApprovalLiquidationBill ?? false))
                        {
                            invoiceEntry.Approval.SuppressApproval = false;
                            invoice.Hold = true;
                            invoice.Status = "H";
                            invoiceEntry.Document.Update(invoice);
                            invoiceEntry.Actions.PressSave();
                        }
                    }
                }
            }

            if (epsetup.AutomaticReleaseAP != true)
            {
                List<EPExpenseClaim> list = new List<EPExpenseClaim>();
                list.Add(claim);
                ATPTEFMApplyPPT(list);
            }
        }


        /// <summary>
        /// Event listener when APInvoiceEntry graph is created from baseMethod <see cref="EPReleaseProcess.ReleaseDocProc(EPExpenseClaim)"/> 
        /// Force change of status on RowPersisted listener so the AP Document does not go under any approval
        /// </summary>
        /// <remarks>
        /// 2025-01-13 : GL ACCOUNT CAPTURED BY THE SYSTEM IN GENERATED ACCOUNTING ENTRY IS GL ACCOUNT SETUP TO EMPLOYEE INSTEAD GL ACCOUNT SETUP TO VENDOR. Case: 009583 {JLTG} <br/>
        /// 2025-02-03 : 009917 - CFM 2024R1: RFP Bills Terms. {CRS} <br/>
        /// 2025-02-28---Use vendor cash account and payment method---010530---RFS <br/>
        /// </remarks>
        private void InvoiceGraphHandler(EPExpenseClaim claim, ATPTEFMEPExpenseClaimExt claimExt, string transactionType, Location location)
        {
            PXGraph.InstanceCreated.AddHandler<APInvoiceEntry>(graph =>
            {
                var countBill = 1;
                //Modify header values
                graph.RowInserted.AddHandler<APInvoice>((cache, args) =>
                {
                    var invoice = (APInvoice)args.Row;
                    var vendorID = transactionType == ATPTEFMExpenseTypeAttribute.RequestforPayment ? claimExt.UsrATPTVendorID : claim.EmployeeID;
                    invoice.InvoiceNbr = (graph.APSetup.Current.RaiseErrorOnDoubleInvoiceNbr ?? false) ? String.Format("{0}-{1}", claim.RefNbr, countBill) : claim.RefNbr;
                    invoice.VendorID = vendorID;
                    invoice.VendorLocationID = transactionType == ATPTEFMExpenseTypeAttribute.RequestforPayment ? location.LocationID : claim.LocationID;
                    invoice.APAccountID = location.VAPAccountID;
                    invoice.APSubID = location.VAPSubID;
                    invoice.PayLocationID = transactionType == ATPTEFMExpenseTypeAttribute.RequestforPayment ? location.LocationID : claim.LocationID;
                    invoice.SuppliedByVendorID = transactionType == ATPTEFMExpenseTypeAttribute.RequestforPayment ? claimExt.UsrATPTVendorID : claim.EmployeeID;
                    invoice.SuppliedByVendorLocationID = transactionType == ATPTEFMExpenseTypeAttribute.RequestforPayment ? location.LocationID : claim.LocationID;
                    invoice.TermsID = Vendor.PK.Find(Base, vendorID).TermsID;
                    invoice.PayTypeID = location.VPaymentMethodID;
                    invoice.PayAccountID = location.VCashAccountID;

                    ATPTEFMAPRegisterExt invoiceExt = invoice.GetExtension<ATPTEFMAPRegisterExt>();

                    invoiceExt.UsrATPTEFMTranType = claimExt.UsrATPTEFMTranType;
                    invoiceExt.UsrATPTEFMReqType = claimExt.UsrATPTEFMReqType;
                    if (claimExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                    {
                        invoiceExt.UsrATPTEFMLiqNbr = claimExt.UsrATPTEFMRFPReqRef;
                        DoAdditionalCreateApBillProcess(invoice, ATPTEFMExpenseTypeAttribute.RequestforPayment);
                    }
                    else
                    {
                        invoiceExt.UsrATPTEFMLiqNbr = claimExt.UsrATPTEFMLiqNbr;
                        DoAdditionalCreateApBillProcess(invoice, ATPTEFMExpenseTypeAttribute.Liquidation);
                    }

                    invoiceExt.UsrATPTEFMIsFromCA = invoiceExt.UsrATPTEFMTranType != null && invoiceExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation;
                    graph.Document.SetValueExt<APInvoice.docDate>(invoice, Base.Accessinfo.BusinessDate);
                    countBill++;
                });

                //graph.RowPersisted.AddHandler<APInvoice>((cache, args) =>
                //{
                //    var invoice = (APInvoice)args.Row;
                //    #region Approval setup
                //    EPSetup epsetup = PXSelectReadonly<EPSetup>.Select(graph);
                //    ATPTEFMEPSetupExtension aTPTEFMEPSetup = epsetup.GetExtension<ATPTEFMEPSetupExtension>();
                //    APInvoiceEntry.APInvoiceEntryDocumentExtension apDocumentGraphExtension = graph.FindImplementation<APInvoiceEntry.APInvoiceEntryDocumentExtension>();
                //    if (transactionType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                //    {
                //        if ((aTPTEFMEPSetup?.UsrATPTEFMIsRequireApprovalRFPBill ?? false) == false)
                //        {
                //            invoice.Hold = false;
                //            graph.Approval.SuppressApproval = true;
                //            invoice.Status = "B";
                //        }
                //    }
                //    if (transactionType == ATPTEFMExpenseTypeAttribute.Liquidation)
                //    {
                //        if ((ATPTEFMPreferences?.Current?.IsRequireApprovalLiquidationBill ?? false) == false)
                //        {
                //            invoice.Hold = false;
                //            graph.Approval.SuppressApproval = true;
                //            invoice.Status = "B";
                //        }
                //    }
                //    #endregion
                //});
                ////Follow approval setup defined by CFM fields
                //graph.FieldUpdated.AddHandler<APInvoice.hold>((cache, args) =>
                //{
                //    var invoice = (APInvoice)args.Row;

                //    EPSetup epsetup = PXSelectReadonly<EPSetup>.Select(graph);
                //    ATPTEFMEPSetupExtension aTPTEFMEPSetup = epsetup.GetExtension<ATPTEFMEPSetupExtension>();
                //    APInvoiceEntry.APInvoiceEntryDocumentExtension apDocumentGraphExtension = graph.FindImplementation<APInvoiceEntry.APInvoiceEntryDocumentExtension>();
                //    if (transactionType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                //    {
                //        if ((aTPTEFMEPSetup?.UsrATPTEFMIsRequireApprovalRFPBill ?? false) == false)
                //        {
                //            invoice.Hold = false;
                //            apDocumentGraphExtension.SuppressApproval();
                //            return;
                //        }
                //    }
                //    if (transactionType == ATPTEFMExpenseTypeAttribute.Liquidation)
                //    {
                //        if ((ATPTEFMPreferences?.Current?.IsRequireApprovalLiquidationBill ?? false) == false)
                //        {
                //            invoice.Hold = false;
                //            apDocumentGraphExtension.SuppressApproval();
                //            return;
                //        }
                //    }
                //    //default 
                //    invoice.Hold = true;
                //    graph.Approval.SuppressApproval = false;
                //});
            });
        }

        public delegate InvoiceTran UpdateInvoiceTransactionDelegate(PXCache InvoiceTrans, InvoiceTran tran, InvoiceTranContext invoiceTranTipInsertionContext);
        [PXOverride]
        public InvoiceTran UpdateInvoiceTransaction(PXCache InvoiceTrans, InvoiceTran tran, InvoiceTranContext invoiceTranTipInsertionContext, UpdateInvoiceTransactionDelegate del)
        {
            InvoiceTrans.Graph.RowUpdated.AddHandler<APTran>((cache, args) =>
            {
                var docGraph = cache.Graph;
                if ((invoiceTranTipInsertionContext.EPClaimDetails.CuryTipAmt ?? 0) != 0)
                {
                    EPSetup epsetup = PXSelectReadonly<EPSetup>.Select(docGraph);
                    InventoryItem tipItem = InventoryItem.PK.Find(docGraph, epsetup.NonTaxableTipItem);

                    if (tipItem == null)
                    {
                        string fieldname = PXUIFieldAttribute.GetDisplayName<EPSetup.nonTaxableTipItem>(docGraph.Caches[typeof(EPSetup)]);
                        throw new PXException(ErrorMessages.ValueDoesntExistOrNoRights, fieldname, epsetup.NonTaxableTipItem);
                    }
                }
            });
            return del(InvoiceTrans, tran, invoiceTranTipInsertionContext);
        }
        #endregion

        #region Actions
        /*
        public delegate void ReleaseDocProcDelegate(EPExpenseClaim claim);
        [PXOverride]
        public void ReleaseDocProc(EPExpenseClaim claim, ReleaseDocProcDelegate baseMethod)
        {
            PXTrace.WriteInformation("CFM Execution");

            ExpenseClaimEntry expenseClaimGraph = PXGraph.CreateInstance<ExpenseClaimEntry>();
            expenseClaimGraph.SelectTimeStamp();

            ATPTEFMEPExpenseClaimExt extDAC = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();

            EPExpenseClaim checkClaim = PXSelectReadonly<EPExpenseClaim, Where<EPExpenseClaim.refNbr, Equal<Required<EPExpenseClaim.refNbr>>>>.Select(expenseClaimGraph, claim.RefNbr);

            if (checkClaim.Released == true)
            {
                throw new PXException(PX.Objects.EP.Messages.AlreadyReleased);
            }
            else if (checkClaim.Approved == false)
            {
                throw new PXException(PX.Objects.EP.Messages.UnapprovedWhenRelease);
            }

            var receipts = PXSelect<
                EPExpenseClaimDetails,
                Where<EPExpenseClaimDetails.refNbr, Equal<Required<EPExpenseClaim.refNbr>>,
                    And<EPExpenseClaimDetails.released, Equal<False>>>>
                .Select(expenseClaimGraph, claim.RefNbr)
                .RowCast<EPExpenseClaimDetails>()
                .ToArray();

            // Acuminator disable once PX1076 CallToInternalApi [Use to validate FinPeriodID]
            IFinPeriodUtils finPeriodUtils = expenseClaimGraph.GetService<IFinPeriodUtils>();

            if (claim.FinPeriodID != null)
            {
                // Acuminator disable once PX1076 CallToInternalApi [Use to validate FinPeriodID]
                finPeriodUtils.ValidateFinPeriod<EPExpenseClaimDetails>(receipts, m => claim.FinPeriodID, m => m.BranchID.SingleToArray());
            }

            List<APRegister> apDocs = new List<APRegister>();

            using (var ts = new PXTransactionScope())
            {
                if (receipts.Any())
                {
                    var receiptsByPaidWithType = receipts.GroupBy(receipt => receipt.PaidWith);

                    foreach (var receiptGroup in receiptsByPaidWithType)
                    {
                        List<APRegister> res = null;

                        if (receiptGroup.Key == EPExpenseClaimDetails.paidWith.PersonalAccount)
                        {
                            res = ReleaseClaimDetails<
                                Invoice, InvoiceMapping, APInvoiceEntry, APInvoiceEntry.APInvoiceEntryDocumentExtension>(
                                expenseClaimGraph, claim, receiptGroup, receiptGroup.Key, extDAC.UsrATPTEFMTranType);
                        }
                        else if (receiptGroup.Key == EPExpenseClaimDetails.paidWith.CardCompanyExpense)
                        {
                            res = ReleaseClaimDetails<
                                PaidInvoice, PaidInvoiceMapping, APQuickCheckEntry, APQuickCheckEntry.APQuickCheckEntryDocumentExtension>(
                                expenseClaimGraph, claim, receiptGroup, receiptGroup.Key, extDAC.UsrATPTEFMTranType);
                        }
                        else if (receiptGroup.Key == EPExpenseClaimDetails.paidWith.CardPersonalExpense)
                        {
                            res = ReleaseClaimDetails<
                                Invoice, InvoiceMapping, APInvoiceEntry, APInvoiceEntry.APInvoiceEntryDocumentExtension>(
                                expenseClaimGraph, claim, receiptGroup, receiptGroup.Key, extDAC.UsrATPTEFMTranType);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }

                        apDocs.AddRange(res);

                          #region EFMChanges
                          //if (extDAC.UsrATPTEFMTranType == Attributes.ATPTEFMExpenseTypeAttribute.Liquidation)
                          //{
                          //    ATPTEFMExpenseClaimEntry_Extension expenseClaimGraphExt = expenseClaimGraph.GetExtension<ATPTEFMExpenseClaimEntry_Extension>();
                          //    List<EPExpenseClaim> list = new List<EPExpenseClaim>();
                          //    list.Add(claim);
                          //    expenseClaimGraphExt.ATPTEFMBillRefUpdate(list);
                          //}
                          #endregion
    }
                }
                else
                {
                    apDocs = ReleaseClaimDetails<Invoice, InvoiceMapping, APInvoiceEntry, APInvoiceEntry.APInvoiceEntryDocumentExtension>(
                        expenseClaimGraph, claim, new EPExpenseClaimDetails[0], EPExpenseClaimDetails.paidWith.PersonalAccount, extDAC.UsrATPTEFMTranType);
                }

                foreach (APRegister result in apDocs)
                {
                    if (result.DocType == APDocType.Invoice)
                    {
                        //APInvoice inv = APInvoice.PK.Find(Base, APDocType.Invoice, result.RefNbr);

                        APInvoiceEntry grp = PXGraph.CreateInstance<APInvoiceEntry>();
                        grp.Clear();

                        grp.Document.Current = (APInvoice)result;
                        ATPTEFMAPRegisterExt invoiceExt = result.GetExtension<ATPTEFMAPRegisterExt>();

                        invoiceExt.UsrATPTEFMTranType = extDAC.UsrATPTEFMTranType;
                        invoiceExt.UsrATPTEFMReqType = extDAC.UsrATPTEFMReqType;
                        if (extDAC.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                        {
                            invoiceExt.UsrATPTEFMLiqNbr = extDAC.UsrATPTEFMRFPReqRef;
                        }
                        else
                        {
                            invoiceExt.UsrATPTEFMLiqNbr = extDAC.UsrATPTEFMLiqNbr;
                            DoAdditionalCreateApBillProcess(grp.Document.Current);
                        }
                        invoiceExt.UsrATPTEFMIsFromCA = invoiceExt.UsrATPTEFMTranType != null && invoiceExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation;
                        grp.Document.Current.DocDate = null;
                        grp.Document.SetValueExt<APInvoice.docDate>((APInvoice)result, Base.Accessinfo.BusinessDate);
                        grp.Document.Update((APInvoice)result);
                        grp.Save.Press();
                    }
                }

                ts.Complete();
            }

            EPSetup epsetup = PXSelectReadonly<EPSetup>.Select(Base);

            if (epsetup.AutomaticReleaseAP == true)
            {
                APDocumentRelease.ReleaseDoc(apDocs, false);

                 #region EFMChanges
                 //List<EPExpenseClaim> list = new List<EPExpenseClaim>();
                 //list.Add(claim);
                 //ATPTEFMAutoApplyPPT(list);
                 #endregion
            }
            else
            {
                #region EFMChanges
                List<EPExpenseClaim> list = new List<EPExpenseClaim>();
                list.Add(claim);
                ATPTEFMApplyPPT(list);
                #endregion
            }
            //baseMethod(claim);

        }
        */
        #endregion

        #region Methods
        /// <remarks>
        /// This method is not needed anymore implementation is handled on Philtax.
        /// </remarks>
        public virtual void DoAdditionalOperation(APTran apTran, EPExpenseClaimDetails claimdetail) { }  
        public virtual APInvoice DoAdditionalCreateApBillProcess(APInvoice row, string type) { return row; }
        /*
        private List<APRegister> ReleaseClaimDetails
            <TAPDocument, TInvoiceMapping, TGraph, TAPDocumentGraphExtension>
            (ExpenseClaimEntry expenseClaimGraph, EPExpenseClaim claim, IEnumerable<EPExpenseClaimDetails> receipts, string receiptGroupPaidWithType, string transactionType)
            where TGraph : PXGraph, new()
            where TAPDocument : InvoiceBase, new()
            where TInvoiceMapping : IBqlMapping
            where TAPDocumentGraphExtension : PX.Objects.Common.GraphExtensions.Abstract.InvoiceBaseGraphExtension<TGraph, TAPDocument, TInvoiceMapping>
        {
            #region prepare required variable

            var docgraph = PXGraph.CreateInstance<TGraph>();

            APInvoiceEntry invoiceGraph = PXGraph.CreateInstance<APInvoiceEntry>();

            ATPTEFMEPExpenseClaimExt extDAC = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();

            EPSetup epsetup = PXSelectReadonly<EPSetup>.Select(docgraph);
            ATPTEFMEPSetupExtension aTPTEFMEPSetup = epsetup.GetExtension<ATPTEFMEPSetupExtension>();

            TAPDocumentGraphExtension apDocumentGraphExtension = docgraph.FindImplementation<TAPDocumentGraphExtension>();

            List<List<EPExpenseClaimDetails>> receiptsForDocument = new List<List<EPExpenseClaimDetails>>();

            if (receiptGroupPaidWithType == EPExpenseClaimDetails.paidWith.PersonalAccount)
            {
                receiptsForDocument = receipts.GroupBy(item => new { item.TaxZoneID, item.TaxCalcMode })
                                                .Select(group => group.ToList())
                                                .ToList();

            }
            else if (receiptGroupPaidWithType == EPExpenseClaimDetails.paidWith.CardCompanyExpense)
            {
                if (epsetup.PostSummarizedCorpCardExpenseReceipts == true)
                {
                    receiptsForDocument = receipts.GroupBy(item =>
                            new
                            {
                                item.TaxZoneID,
                                item.TaxCalcMode,
                                item.CorpCardID,
                                item.ExpenseDate,
                                item.ExpenseRefNbr
                            })
                        .Select(group => group.ToList())
                        .ToList();
                }
                else
                {
                    receiptsForDocument = receipts.Select(receipt => receipt.SingleToList()).ToList();
                }
            }
            else if (receiptGroupPaidWithType == EPExpenseClaimDetails.paidWith.CardPersonalExpense)
            {
                receiptsForDocument = new List<List<EPExpenseClaimDetails>>() { receipts.ToList() };
            }
            else
            {
                throw new NotImplementedException();
            }

            if (!receiptsForDocument.Any())
            {
                receiptsForDocument.Add(new List<EPExpenseClaimDetails>());
            }

            APSetup apsetup = PXSelectReadonly<APSetup>.Select(docgraph);

            EPEmployee employee = PXSelect<
                EPEmployee,
                Where<EPEmployee.bAccountID, Equal<Required<EPExpenseClaim.employeeID>>>>
                .Select(docgraph, claim.EmployeeID);

            Vendor vendor = PXSelect<
                Vendor,
                Where<Vendor.bAccountID, Equal<Required<ATPTEFMEPExpenseClaimExt.usrATPTVendorID>>>>
                .Select(docgraph, extDAC.UsrATPTVendorID);

            Location location = transactionType == ATPTEFMExpenseTypeAttribute.RequestforPayment ? PXSelect<
                Location,
                Where<Location.bAccountID, Equal<Required<ATPTEFMEPExpenseClaimExt.usrATPTVendorID>>,
                    And<Location.isActive, Equal<True>>>>
                .Select(docgraph, extDAC.UsrATPTVendorID) : PXSelect<
              Location,
              Where<Location.bAccountID, Equal<Required<EPExpenseClaim.employeeID>>,
                  And<Location.locationID, Equal<Required<EPExpenseClaim.locationID>>>>>
              .Select(docgraph, claim.EmployeeID, claim.LocationID);

            List<APRegister> doclist = new List<APRegister>();

            expenseClaimGraph.SelectTimeStamp();


            if (claim.FinPeriodID != null)
            {
                // Acuminator disable once PX1076 CallToInternalApi [Use to validate FinPeriodID]
                Base.FinPeriodUtils.ValidateFinPeriod(claim.SingleToArray());
            }
            #endregion
            int countBill = 1;

            foreach (var receiptGroup in receiptsForDocument)
            {
                if (receiptGroupPaidWithType == EPExpenseClaimDetails.paidWith.CardCompanyExpense && receiptGroup.Count > 1)
                {
                    EPExpenseClaimDetails[] matchedReceipts = receiptGroup.Where(receipt => receipt.BankTranDate != null).Take(11).ToArray();

                    if (matchedReceipts.Any())
                    {
                        PXResult<EPExpenseClaimDetails, CABankTranMatch, CABankTran>[] rows =
                            PXSelectJoin<EPExpenseClaimDetails,
                                    InnerJoin<CABankTranMatch,
                                        On<CABankTranMatch.docModule, Equal<BatchModule.moduleEP>,
                                            And<CABankTranMatch.docType, Equal<EPExpenseClaimDetails.docType>,
                                            And<CABankTranMatch.docRefNbr, Equal<EPExpenseClaimDetails.claimDetailCD>>>>,
                                    InnerJoin<CABankTran,
                                        On<CABankTran.tranID, Equal<CABankTranMatch.tranID>>>>,
                                    Where<EPExpenseClaimDetails.claimDetailCD, In<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                                .Select(expenseClaimGraph, matchedReceipts.Select(receipt => receipt.ClaimDetailCD).ToArray())
                                .Cast<PXResult<EPExpenseClaimDetails, CABankTranMatch, CABankTran>>()
                                .ToArray();

                        throw new PXException(PX.Objects.EP.Messages.ExpenseReceiptCannotBeSummarized,
                            rows.Select(row => String.Concat(PXMessages.LocalizeNoPrefix(PX.Objects.EP.Messages.Receipt),
                                                            " ",
                                                            ((EPExpenseClaimDetails)row).ClaimDetailCD,
                                                            " - ",
                                                            ((CABankTran)row).GetFriendlyKeyImage(Base.Caches[typeof(CABankTran)])))
                                .ToArray()
                                .JoinIntoStringForMessageNoQuotes(maxCount: 10));
                    }
                }

                docgraph.Clear(PXClearOption.ClearAll);
                docgraph.SelectTimeStamp();
                apDocumentGraphExtension.Contragent.Current = apDocumentGraphExtension.Contragent.Cache.GetExtension<Contragent>(employee);
                apDocumentGraphExtension.Location.Current = location;

                CurrencyInfo infoOriginal = PXSelect<CurrencyInfo,
                    Where<CurrencyInfo.curyInfoID, Equal<Required<EPExpenseClaim.curyInfoID>>>>.Select(docgraph, claim.CuryInfoID);

                if (infoOriginal != null && infoOriginal.CuryPrecision == null)
                {
                    docgraph.Caches<CurrencyInfo>().RaiseFieldDefaulting<CurrencyInfo.curyPrecision>(infoOriginal, out object newValue);
                    infoOriginal.CuryPrecision = newValue as short?;
                }

                CurrencyInfo info = PXCache<CurrencyInfo>.CreateCopy(infoOriginal);

                info.CuryInfoID = null;
                info = apDocumentGraphExtension.CurrencyInfo.Insert(PX.Objects.CM.Extensions.CurrencyInfo.GetEX(info)).GetCM();

                #region CreateInvoiceHeader

                var invoice = new TAPDocument();

                CABankTranMatch bankTranMatch = null;

                if (receiptGroupPaidWithType == EPExpenseClaimDetails.paidWith.PersonalAccount)
                {
                    invoice.DocType = receiptGroup.Sum(_ => _.ClaimCuryTranAmtWithTaxes) < 0
                        ? APInvoiceType.DebitAdj
                        : APInvoiceType.Invoice;
                }
                else if (receiptGroupPaidWithType == EPExpenseClaimDetails.paidWith.CardCompanyExpense)
                {
                    EPExpenseClaimDetails receipt = receiptGroup.First();

                    invoice.DocType = APDocType.QuickCheck;

                    CACorpCard card = CACorpCard.PK.Find(Base, receipt.CorpCardID);

                    PaymentMethodAccount paymentMethodAccount = PXSelect<PaymentMethodAccount,
                                                                            Where<PaymentMethodAccount.cashAccountID, Equal<Required<PaymentMethodAccount.cashAccountID>>>>
                                                                            .Select(Base, card.CashAccountID);

                    invoice.CashAccountID = card.CashAccountID;
                    invoice.PaymentMethodID = paymentMethodAccount.PaymentMethodID;
                    invoice.ExtRefNbr = receipt.ExpenseRefNbr;

                    if (receiptGroup.Count == 1)
                    {
                        bankTranMatch =
                            PXSelect<CABankTranMatch,
                                    Where<CABankTranMatch.docModule, Equal<BatchModule.moduleEP>,
                                        And<CABankTranMatch.docType, Equal<EPExpenseClaimDetails.docType>,
                                        And<CABankTranMatch.docRefNbr, Equal<Required<CABankTranMatch.docRefNbr>>>>>>
                                    .Select(expenseClaimGraph, receipt.ClaimDetailCD);

                        if (bankTranMatch != null)
                        {
                            CABankTran bankTran = CABankTran.PK.Find(expenseClaimGraph, bankTranMatch.TranID);

                            invoice.ClearDate = bankTran.ClearDate;
                            invoice.Cleared = true;
                        }
                    }
                }
                else if (receiptGroupPaidWithType == EPExpenseClaimDetails.paidWith.CardPersonalExpense)
                {
                    invoice.DocType = APDocType.DebitAdj;
                }
                else
                {
                    throw new NotImplementedException();
                }

                invoice.CuryInfoID = info.CuryInfoID;
                invoice.Hold = true;
                invoice.Released = false;
                invoice.Printed = invoice.DocType == APDocType.QuickCheck;
                invoice.OpenDoc = true;
                invoice.HeaderDocDate = Base.Accessinfo.BusinessDate; //claim.DocDate; //#mod
                invoice.FinPeriodID = claim.FinPeriodID;
                invoice.InvoiceNbr = (apsetup.RaiseErrorOnDoubleInvoiceNbr ?? false) ? String.Format("{0}-{1}", claim.RefNbr, countBill) : claim.RefNbr; //#mod create countBill variable on override increment on insert.
                invoice.DocDesc = claim.DocDesc;
                invoice.ContragentID = transactionType == ATPTEFMExpenseTypeAttribute.RequestforPayment ? extDAC.UsrATPTVendorID : claim.EmployeeID; //claim.EmployeeID; //#mod
                invoice.CuryID = info.CuryID;
                invoice.ContragentLocationID = transactionType == ATPTEFMExpenseTypeAttribute.RequestforPayment ? location.LocationID : claim.LocationID; //claim.LocationID; //#mod implement new location select.
                invoice.ModuleAccountID = location != null ? location.APAccountID : null;
                invoice.ModuleSubID = location != null ? location.APSubID : null;
                invoice.TaxCalcMode = receiptGroup.Any() ? receiptGroup.First().TaxCalcMode : claim.TaxCalcMode;
                invoice.BranchID = claim.BranchID;
                invoice.OrigModule = BatchModule.EP;

                if (receiptGroupPaidWithType == EPExpenseClaimDetails.paidWith.CardCompanyExpense
                    && receiptGroup.Count == 1)
                {
                    invoice.OrigDocType = EPExpenseClaimDetails.DocType;
                    invoice.OrigRefNbr = receiptGroup.Single().ClaimDetailCD;
                }
                else
                {
                    invoice.OrigDocType = EPExpenseClaim.DocType;
                    invoice.OrigRefNbr = claim.RefNbr;
                }

                bool reversedDocument = invoice.DocType == APInvoiceType.DebitAdj && receiptGroupPaidWithType == EPExpenseClaimDetails.paidWith.PersonalAccount;

                decimal signOperation = reversedDocument ? -1 : 1;

                invoice.HeaderTranPeriodID = claim.FinPeriodID;
                invoice.HeaderFinPeriodID = claim.FinPeriodID;
                
                invoice = apDocumentGraphExtension.Documents.Insert(invoice);
                HandleInvoiceInMultiBaseCurrency(docgraph, invoice);

                (apDocumentGraphExtension.Documents.Cache as PXModelExtension<TAPDocument>)?.UpdateExtensionMapping(invoice, MappingSyncDirection.BaseToExtension);

                invoice.BranchID = claim.BranchID;
                invoice.TaxZoneID = receiptGroup.Any() ? receiptGroup.First().TaxZoneID : claim.TaxZoneID;

                invoice = apDocumentGraphExtension.Documents.Update(invoice);

                PXCache<CurrencyInfo>.RestoreCopy(info, infoOriginal);
                info.CuryInfoID = invoice.CuryInfoID;

                PXCache claimcache = docgraph.Caches[typeof(EPExpenseClaim)];
                PXCache claimdetailcache = docgraph.Caches[typeof(EPExpenseClaimDetails)];

                PXNoteAttribute.CopyNoteAndFiles(claimcache, claim, apDocumentGraphExtension.Documents.Cache, invoice, epsetup.GetCopyNoteSettings<PXModule.ap>());
                #endregion

                TaxAttribute.SetTaxCalc<InvoiceTran.taxCategoryID>(apDocumentGraphExtension.InvoiceTrans.Cache, null, TaxCalc.ManualCalc);

                decimal? claimCuryTaxRoundDiff = 0m;
                decimal? claimTaxRoundDiff = 0m;
                foreach (EPExpenseClaimDetails claimdetail in receiptGroup)
                {
                    #region AddDetails

                    decimal tipQty;
                    if (reversedDocument == claimdetail.ClaimCuryTranAmtWithTaxes < 0)
                    {
                        tipQty = 1;
                    }
                    else
                    {
                        tipQty = -1;
                    }
                    Contract contract = PXSelect<Contract, Where<Contract.contractID, Equal<Required<EPExpenseClaimDetails.contractID>>>>.SelectSingleBound(docgraph, null, claimdetail.ContractID);
                    InventoryItem inventory = PXSelect<InventoryItem, Where<InventoryItem.inventoryID, Equal<Required<EPExpenseClaimDetails.inventoryID>>>>.SelectSingleBound(docgraph, null, claimdetail.InventoryID);

                    if (claimdetail.TaskID != null)
                    {
                        PMTask task = PMTask.PK.FindDirty(expenseClaimGraph, claimdetail.ContractID, claimdetail.TaskID);
                        if (task != null && !(bool)task.VisibleInAP)
                            throw new PXException(PX.Objects.PM.Messages.TaskInvisibleInModule, task.TaskCD, BatchModule.AP);
                    }

                    InvoiceTran tran = new InvoiceTran();
                    tran.InventoryID = claimdetail.InventoryID;
                    tran.TranDesc = claimdetail.TranDesc;
                    decimal unitCost;
                    decimal amount;
                    decimal taxableAmt;
                    decimal taxAmt;
                    if (CurrencyHelper.IsSameCury(expenseClaimGraph, claimdetail.CuryInfoID, claimdetail.ClaimCuryInfoID))
                    {
                        unitCost = claimdetail.CuryUnitCost ?? 0m;
                        amount = claimdetail.CuryTaxableAmt ?? 0m;
                        taxableAmt = claimdetail.CuryTaxableAmtFromTax ?? 0m;
                        taxAmt = claimdetail.CuryTaxAmt ?? 0m;
                    }
                    else
                    {
                        if (claimdetail.CuryUnitCost == null || claimdetail.CuryUnitCost == 0m)
                        {
                            unitCost = 0m;
                        }
                        else
                        {
                            PXCurrencyAttribute.CuryConvCury<EPExpenseClaimDetails.claimCuryInfoID>(expenseClaimGraph.ExpenseClaimDetails.Cache, claimdetail, (decimal)claimdetail.UnitCost, out unitCost);
                        }
                        if (claimdetail.CuryTaxableAmt == null || claimdetail.CuryTaxableAmt == 0m)
                        {
                            amount = 0m;
                        }
                        else
                        {
                            PXCurrencyAttribute.CuryConvCury<EPExpenseClaimDetails.claimCuryInfoID>(expenseClaimGraph.ExpenseClaimDetails.Cache, claimdetail, (decimal)claimdetail.TaxableAmt, out amount);
                        }
                        if (claimdetail.CuryTaxableAmtFromTax == null || claimdetail.CuryTaxableAmtFromTax == 0m)
                        {
                            taxableAmt = 0m;
                        }
                        else
                        {
                            PXCurrencyAttribute.CuryConvCury<EPExpenseClaimDetails.claimCuryInfoID>(expenseClaimGraph.ExpenseClaimDetails.Cache, claimdetail, (decimal)claimdetail.TaxableAmtFromTax, out taxableAmt);
                        }
                        if (claimdetail.CuryTaxAmt == null || claimdetail.CuryTaxAmt == 0m)
                        {
                            taxAmt = 0m;
                        }
                        else
                        {
                            PXCurrencyAttribute.CuryConvCury<EPExpenseClaimDetails.claimCuryInfoID>(expenseClaimGraph.ExpenseClaimDetails.Cache, claimdetail, (decimal)claimdetail.TaxAmt, out taxAmt);
                        }
                    }

                    tran.ManualPrice = true;
                    tran.CuryUnitCost = unitCost;
                    tran.Qty = claimdetail.Qty * signOperation;
                    tran.UOM = claimdetail.UOM;
                    tran.NonBillable = claimdetail.Billable != true;
                    claimCuryTaxRoundDiff += (claimdetail.ClaimCuryTaxRoundDiff ?? 0m) * signOperation;
                    claimTaxRoundDiff += (claimdetail.ClaimTaxRoundDiff ?? 0m) * signOperation;
                    tran.Date = claimdetail.ExpenseDate;

                    if (contract.BaseType == PX.Objects.CT.CTPRType.Project)
                    {
                        tran.ProjectID = claimdetail.ContractID;
                    }
                    else
                    {
                        tran.ProjectID = ProjectDefaultAttribute.NonProject();
                    }

                    tran.TaskID = claimdetail.TaskID;
                    tran.CostCodeID = claimdetail.CostCodeID;

                    if (receiptGroupPaidWithType == EPExpenseClaimDetails.paidWith.CardPersonalExpense)
                    {
                        CACorpCard card = CACorpCard.PK.Find(Base, claimdetail.CorpCardID);
                        CashAccount cashAccount = CashAccount.PK.Find(Base, card.CashAccountID);

                        tran.AccountID = cashAccount.AccountID;
                        tran.SubID = cashAccount.SubID;
                    }
                    else
                    {
                        tran.AccountID = claimdetail.ExpenseAccountID;
                        tran.SubID = claimdetail.ExpenseSubID;
                    }

                    tran.BranchID = claimdetail.BranchID;

                    tran = Base.InsertInvoiceTransaction(apDocumentGraphExtension.InvoiceTrans.Cache, tran,
                                        new InvoiceTranContext { EPClaim = claim, EPClaimDetails = claimdetail });

                    if (claimdetail.PaidWith == EPExpenseClaimDetails.paidWith.CardPersonalExpense)
                    {
                        claimdetail.APLineNbr = tran.LineNbr;
                    }

                    if (inventory.StkItem == null || inventory.StkItem == false)
                    {
                        ((APTran)tran.Base).AccrueCost = false;
                    }
                    tran.CuryLineAmt = amount * signOperation;
                    tran.CuryTaxAmt = 0;
                    tran.CuryTaxableAmt = taxableAmt * signOperation;
                    tran.CuryTaxAmt = taxAmt * signOperation;
                    tran.TaxCategoryID = claimdetail.TaxCategoryID;

                    DoAdditionalOperation((APTran)apDocumentGraphExtension.InvoiceTrans.Current.Base, claimdetail);

                    tran = Base.UpdateInvoiceTransaction(apDocumentGraphExtension.InvoiceTrans.Cache, tran,
                                    new InvoiceTranContext { EPClaim = claim, EPClaimDetails = claimdetail });

                    if ((claimdetail.CuryTipAmt ?? 0) != 0)
                    {
                        InvoiceTran tranTip = new InvoiceTran();
                        if (epsetup.NonTaxableTipItem == null)
                        {
                            throw new PXException(PX.Objects.EP.Messages.TipItemIsNotDefined);
                        }
                        InventoryItem tipItem = PXSelect<InventoryItem,
                            Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>.Select(docgraph, epsetup.NonTaxableTipItem);

                        if (tipItem == null)
                        {
                            //#mod identify how to modify error message on override.
                            string fieldname = PXUIFieldAttribute.GetDisplayName<EPSetup.nonTaxableTipItem>(docgraph.Caches[typeof(EPSetup)]);
                            throw new PXException(ErrorMessages.ValueDoesntExistOrNoRights, fieldname, epsetup.NonTaxableTipItem);
                        }
                        tranTip.InventoryID = epsetup.NonTaxableTipItem;
                        tranTip.TranDesc = tipItem.Descr;
                        if (CurrencyHelper.IsSameCury(expenseClaimGraph, claimdetail.CuryInfoID, claimdetail.ClaimCuryInfoID))
                        {
                            tranTip.CuryUnitCost = Math.Abs(claimdetail.CuryTipAmt ?? 0m);
                            tranTip.CuryTranAmt = claimdetail.CuryTipAmt * signOperation;
                        }
                        else
                        {
                            decimal tipAmt;
                            PXCurrencyAttribute.CuryConvCury<EPExpenseClaimDetails.claimCuryInfoID>(expenseClaimGraph.ExpenseClaimDetails.Cache, claimdetail, (decimal)claimdetail.TipAmt, out tipAmt);
                            tranTip.CuryUnitCost = Math.Abs(tipAmt);
                            tranTip.CuryTranAmt = tipAmt * signOperation;
                        }
                        tranTip.Qty = tipQty;
                        tranTip.UOM = tipItem.BaseUnit;
                        tranTip.NonBillable = claimdetail.Billable != true;
                        tranTip.Date = claimdetail.ExpenseDate;

                        tranTip.BranchID = claimdetail.BranchID;

                        tranTip = Base.InsertInvoiceTipTransaction(apDocumentGraphExtension.InvoiceTrans.Cache, tranTip,
                                    new InvoiceTranContext { EPClaim = claim, EPClaimDetails = claimdetail });


                        if (epsetup.UseReceiptAccountForTips == true)
                        {
                            tranTip.AccountID = claimdetail.ExpenseAccountID;
                            tranTip.SubID = claimdetail.ExpenseSubID;
                        }
                        else
                        {
                            tranTip.AccountID = tipItem.COGSAcctID;
                            Location companyloc = (Location)PXSelectJoin<Location,
                                                                        InnerJoin<BAccountR, On<Location.bAccountID, Equal<BAccountR.bAccountID>,
                                                                                            And<Location.locationID, Equal<BAccountR.defLocationID>>>,
                                                                        InnerJoin<Branch, On<BAccountR.bAccountID, Equal<Branch.bAccountID>>>>,
                                                            Where<Branch.branchID, Equal<Current<APInvoice.branchID>>>>.Select(docgraph);
                            PMTask task = PXSelect<PMTask,
                                                    Where<PMTask.projectID, Equal<Required<PMTask.projectID>>,
                                                    And<PMTask.taskID, Equal<Required<PMTask.taskID>>>>>.Select(docgraph, claimdetail.ContractID, claimdetail.TaskID);

                            Location customerLocation = (Location)PXSelectorAttribute.Select<EPExpenseClaimDetails.customerLocationID>(claimdetailcache, claimdetail);

                            int? employee_SubID = (int?)docgraph.Caches[typeof(EPEmployee)].GetValue<EPEmployee.expenseSubID>(employee);
                            int? item_SubID = (int?)docgraph.Caches[typeof(InventoryItem)].GetValue<InventoryItem.cOGSSubID>(tipItem);
                            int? company_SubID = (int?)docgraph.Caches[typeof(Location)].GetValue<Location.cMPExpenseSubID>(companyloc);
                            int? project_SubID = (int?)docgraph.Caches[typeof(Contract)].GetValue<Contract.defaultExpenseSubID>(contract);
                            int? task_SubID = (int?)docgraph.Caches[typeof(PMTask)].GetValue<PMTask.defaultExpenseSubID>(task);
                            int? location_SubID = (int?)docgraph.Caches[typeof(Location)].GetValue<Location.cSalesSubID>(customerLocation);

                            object value = PX.Objects.EP.SubAccountMaskAttribute.MakeSub<EPSetup.expenseSubMask>(docgraph, epsetup.ExpenseSubMask,
                                new object[] { employee_SubID, item_SubID, company_SubID, project_SubID, task_SubID, location_SubID },
                                new Type[] { typeof(EPEmployee.expenseSubID), typeof(InventoryItem.cOGSSubID), typeof(Location.cMPExpenseSubID), typeof(Contract.defaultExpenseSubID), typeof(PMTask.defaultExpenseSubID), typeof(Location.cSalesSubID) });

                            docgraph.Caches[typeof(APTran)].RaiseFieldUpdating<APTran.subID>(tranTip, ref value);
                            tranTip.SubID = (int?)value;
                        }

                        tranTip = Base.UpdateInvoiceTipTransactionAccounts(apDocumentGraphExtension.InvoiceTrans.Cache, tranTip,
                                        new InvoiceTranContext { EPClaim = claim, EPClaimDetails = claimdetail });

                        tranTip.TaxCategoryID = tipItem.TaxCategoryID;
                        tranTip.ProjectID = tran.ProjectID;
                        tranTip.TaskID = tran.TaskID;
                        tranTip.CostCodeID = tran.CostCodeID;
                        tranTip = AddTaxes<TAPDocument, TInvoiceMapping, TGraph, TAPDocumentGraphExtension>(apDocumentGraphExtension, docgraph, expenseClaimGraph, invoice, signOperation, claimdetail, tranTip, true);

                        tranTip = Base.UpdateInvoiceTipTransactionTaxesAndProject(apDocumentGraphExtension.InvoiceTrans.Cache, tranTip,
                                    new InvoiceTranContext { EPClaim = claim, EPClaimDetails = claimdetail });
                    }

                    PXNoteAttribute.CopyNoteAndFiles(claimdetailcache, claimdetail, apDocumentGraphExtension.InvoiceTrans.Cache, tran, epsetup.GetCopyNoteSettings<PXModule.ap>());
                    claimdetail.Released = true;
                    expenseClaimGraph.ExpenseClaimDetails.Cache.MarkUpdated(claimdetail);
                    #endregion

                    if (receiptGroupPaidWithType != EPExpenseClaimDetails.paidWith.CardPersonalExpense)
                    {
                        tran = AddTaxes<TAPDocument, TInvoiceMapping, TGraph, TAPDocumentGraphExtension>(apDocumentGraphExtension, docgraph, expenseClaimGraph, invoice, signOperation, claimdetail, tran, false);
                    }
                }

                #region legacy taxes
                foreach (EPTaxAggregate tax in PXSelectReadonly<EPTaxAggregate,
                    Where<EPTaxAggregate.refNbr, Equal<Required<EPExpenseClaim.refNbr>>>>.Select(docgraph, claim.RefNbr))
                {
                    #region Add taxes
                    GenericTaxTran new_aptax = apDocumentGraphExtension.TaxTrans.Search<GenericTaxTran.taxID>(tax.TaxID);

                    if (new_aptax == null)
                    {
                        new_aptax = new GenericTaxTran();
                        new_aptax.TaxID = tax.TaxID;
                        new_aptax = apDocumentGraphExtension.TaxTrans.Insert(new_aptax);
                        if (new_aptax != null)
                        {
                            new_aptax = (GenericTaxTran)apDocumentGraphExtension.TaxTrans.Cache.CreateCopy(new_aptax);
                            new_aptax.CuryTaxableAmt = 0m;
                            new_aptax.CuryTaxAmt = 0m;
                            new_aptax.CuryExpenseAmt = 0m;
                            new_aptax = apDocumentGraphExtension.TaxTrans.Update(new_aptax);
                        }
                    }

                    if (new_aptax != null)
                    {
                        new_aptax = (GenericTaxTran)apDocumentGraphExtension.TaxTrans.Cache.CreateCopy(new_aptax);
                        new_aptax.TaxRate = tax.TaxRate;
                        new_aptax.CuryTaxableAmt = (new_aptax.CuryTaxableAmt ?? 0m) + tax.CuryTaxableAmt * signOperation;
                        new_aptax.CuryTaxAmt = (new_aptax.CuryTaxAmt ?? 0m) + tax.CuryTaxAmt * signOperation;
                        new_aptax.CuryExpenseAmt = (new_aptax.CuryExpenseAmt ?? 0m) + tax.CuryExpenseAmt * signOperation;
                        new_aptax = apDocumentGraphExtension.TaxTrans.Update(new_aptax);
                    }
                    #endregion
                }
                #endregion

                invoice.CuryOrigDocAmt = invoice.CuryDocBal;
                invoice.CuryTaxAmt = invoice.CuryTaxTotal;
                //#mod try this on fieldupdated of hold and modify Base.Approval.SuppressApproval accordingly.
                if (transactionType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                {
                    if ((aTPTEFMEPSetup?.UsrATPTEFMIsRequireApprovalRFPBill ?? false) == false)
                    {
                        invoice.Hold = false;
                        apDocumentGraphExtension.SuppressApproval();
                    }

                }
                if (transactionType == ATPTEFMExpenseTypeAttribute.Liquidation)
                {
                    if ((ATPTEFMPreferences?.Current?.IsRequireApprovalLiquidationBill ?? false) == false)
                    {
                        invoice.Hold = false;
                        apDocumentGraphExtension.SuppressApproval();
                    }

                }
                apDocumentGraphExtension.Documents.Update(invoice);

                if (receiptGroupPaidWithType != EPExpenseClaimDetails.paidWith.CardPersonalExpense)
                {
                    invoice.CuryTaxRoundDiff = invoice.CuryRoundDiff = invoice.CuryRoundDiff = claimCuryTaxRoundDiff;
                    invoice.TaxRoundDiff = invoice.RoundDiff = claimTaxRoundDiff;
                    bool inclusive = PXSelectJoin<APTaxTran, InnerJoin<Tax,
                            On<APTaxTran.taxID, Equal<Tax.taxID>>>,
                            Where<APTaxTran.refNbr, Equal<Required<APInvoice.refNbr>>,
                            And<APTaxTran.tranType, Equal<Required<APInvoice.docType>>,
                            And<Tax.taxCalcLevel, Equal<CSTaxCalcLevel.inclusive>>>>>
                            .Select(docgraph, invoice.RefNbr, invoice.DocType).Count > 0;
                    if ((invoice.TaxCalcMode == TaxCalculationMode.Gross
                        && PXSelectJoin<APTaxTran, InnerJoin<Tax,
                            On<APTaxTran.taxID, Equal<Tax.taxID>>>,
                            Where<APTaxTran.refNbr, Equal<Required<APInvoice.refNbr>>,
                            And<APTaxTran.tranType, Equal<Required<APInvoice.docType>>,
                            And<Tax.taxCalcLevel, Equal<CSTaxCalcLevel.calcOnItemAmt>>>>>
                            .Select(docgraph, invoice.RefNbr, invoice.DocType).Count > 0)
                            || inclusive)
                    {

                        decimal curyAdditionalDiff = -(invoice.CuryTaxRoundDiff ?? 0m) + (invoice.CuryTaxAmt ?? 0m) - (invoice.CuryDocBal ?? 0m);
                        decimal additionalDiff = -(invoice.TaxRoundDiff ?? 0m) + (invoice.TaxAmt ?? 0m) - (invoice.DocBal ?? 0m);
                        foreach (InvoiceTran line in apDocumentGraphExtension.InvoiceTrans.Select())
                        {
                            curyAdditionalDiff += (line.CuryTaxableAmt ?? 0m) == 0m ? (line.CuryTranAmt ?? 0m) : (line.CuryTaxableAmt ?? 0m);
                            additionalDiff += (line.TaxableAmt ?? 0m) == 0m ? (line.TranAmt ?? 0m) : (line.TaxableAmt ?? 0m);
                        }

                        invoice.CuryTaxRoundDiff += curyAdditionalDiff;
                        invoice.TaxRoundDiff += additionalDiff;

                    }
                }

                invoice = apDocumentGraphExtension.Documents.Update(invoice);
                docgraph.Actions.PressSave();

                if (receiptGroupPaidWithType == EPExpenseClaimDetails.paidWith.CardCompanyExpense
                    && receiptGroup.Count == 1
                    && bankTranMatch != null)
                {
                    CABankTransactionsMaint.RematchFromExpenseReceipt(Base, bankTranMatch, invoice.CATranID, invoice.ContragentID, receiptGroup.Single());
                }

                foreach (EPExpenseClaimDetails claimdetail in receiptGroup)
                {
                    claimdetail.APDocType = invoice.DocType;
                    claimdetail.APRefNbr = invoice.RefNbr;
                    expenseClaimGraph.ExpenseClaimDetails.Cache.MarkUpdated(claimdetail);
                }

                claim.Status = EPExpenseClaimStatus.ReleasedStatus;
                claim.Released = true;

                expenseClaimGraph.ExpenseClaim.Update(claim);

                #region EP History Update
                EPHistory hist = new EPHistory();
                hist.EmployeeID = invoice.ContragentID;
                hist.FinPeriodID = invoice.FinPeriodID;
                hist = (EPHistory)expenseClaimGraph.Caches[typeof(EPHistory)].Insert(hist);

                hist.FinPtdClaimed += invoice.DocBal;
                hist.FinYtdClaimed += invoice.DocBal;
                if (invoice.FinPeriodID == invoice.HeaderTranPeriodID)
                {
                    hist.TranPtdClaimed += invoice.DocBal;
                    hist.TranYtdClaimed += invoice.DocBal;
                }
                else
                {
                    EPHistory tranhist = new EPHistory();
                    tranhist.EmployeeID = invoice.ContragentID;
                    tranhist.FinPeriodID = invoice.HeaderTranPeriodID;
                    tranhist = (EPHistory)expenseClaimGraph.Caches[typeof(EPHistory)].Insert(tranhist);
                    tranhist.TranPtdClaimed += invoice.DocBal;
                    tranhist.TranYtdClaimed += invoice.DocBal;
                }
                expenseClaimGraph.Views.Caches.Add(typeof(EPHistory));
                #endregion

                expenseClaimGraph.Save.Press();

                Base.Actions.PressSave();

                doclist.Add((APRegister)apDocumentGraphExtension.Documents.Current.Base);
                countBill++;
            }

            return doclist;
        }

        private InvoiceTran AddTaxes<TAPDocument, TInvoiceMapping, TGraph, TAPDocumentGraphExtension>
        (TAPDocumentGraphExtension apDocumentGraphExtension,
            TGraph docgraph,
            ExpenseClaimEntry expenseClaimGraph,
            TAPDocument invoice,
            decimal signOperation,
            EPExpenseClaimDetails claimdetail,
            InvoiceTran tran,
            bool isTipTran)
            where TGraph : PXGraph, new()
            where TAPDocument : InvoiceBase, new()
            where TInvoiceMapping : IBqlMapping
            where TAPDocumentGraphExtension : PX.Objects.Common.GraphExtensions.Abstract.InvoiceBaseGraphExtension<TGraph, TAPDocument, TInvoiceMapping>

        {
            var cmdEPTaxTran = new PXSelect<EPTaxTran,
                Where<EPTaxTran.claimDetailID, Equal<Required<EPTaxTran.claimDetailID>>>>(docgraph);

            var cmdEPTax = new PXSelect<EPTax,
                                Where<EPTax.claimDetailID, Equal<Required<EPTax.claimDetailID>>,
                                And<EPTax.taxID, Equal<Required<EPTax.taxID>>>>>(docgraph);
            if (isTipTran)
            {
                cmdEPTaxTran.WhereAnd<Where<EPTaxTran.isTipTax, Equal<True>>>();
                cmdEPTax.WhereAnd<Where<EPTax.isTipTax, Equal<True>>>();
            }
            else
            {
                cmdEPTaxTran.WhereAnd<Where<EPTaxTran.isTipTax, Equal<False>>>();
                cmdEPTax.WhereAnd<Where<EPTax.isTipTax, Equal<False>>>();
            }

            CurrencyInfo expenseCuriInfo = PXSelect<CurrencyInfo,
                Where<CurrencyInfo.curyInfoID, Equal<Required<EPExpenseClaimDetails.curyInfoID>>>>.SelectSingleBound(docgraph, null, claimdetail.CuryInfoID);

            CurrencyInfo currencyinfo = PXSelect<CurrencyInfo,
                Where<CurrencyInfo.curyInfoID, Equal<Required<EPExpenseClaimDetails.curyInfoID>>>>.SelectSingleBound(docgraph, null, claimdetail.ClaimCuryInfoID);

            foreach (EPTaxTran epTaxTran in cmdEPTaxTran.Select(claimdetail.ClaimDetailID))
            {
                #region Add taxes
                GenericTaxTran new_aptax = apDocumentGraphExtension.TaxTrans.Search<GenericTaxTran.taxID>(epTaxTran.TaxID);

                if (new_aptax == null)
                {
                    new_aptax = new GenericTaxTran();
                    new_aptax.TaxID = epTaxTran.TaxID;
                    TaxAttribute.SetTaxCalc<InvoiceTran.taxCategoryID>(apDocumentGraphExtension.InvoiceTrans.Cache, null, TaxCalc.NoCalc);
                    new_aptax = apDocumentGraphExtension.TaxTrans.Insert(new_aptax);
                    if (new_aptax != null)
                    {
                        new_aptax = (GenericTaxTran)apDocumentGraphExtension.TaxTrans.Cache.CreateCopy(new_aptax);
                        new_aptax.CuryTaxableAmt = 0m;
                        new_aptax.CuryTaxAmt = 0m;
                        new_aptax.CuryExpenseAmt = 0m;
                        new_aptax = apDocumentGraphExtension.TaxTrans.Update(new_aptax);
                    }
                }

                if (new_aptax != null)
                {
                    EPTax epTax = cmdEPTax.Select(claimdetail.ClaimDetailID, new_aptax.TaxID);
                    new_aptax = (GenericTaxTran)apDocumentGraphExtension.TaxTrans.Cache.CreateCopy(new_aptax);
                    new_aptax.TaxRate = epTaxTran.TaxRate;
                    new_aptax.CuryTaxableAmt = (new_aptax.CuryTaxableAmt ?? 0m) + epTaxTran.ClaimCuryTaxableAmt * signOperation;
                    new_aptax.CuryTaxAmt = (new_aptax.CuryTaxAmt ?? 0m) + epTaxTran.ClaimCuryTaxAmt * signOperation;
                    new_aptax.CuryTaxAmtSumm = new_aptax.CuryTaxAmt;
                    new_aptax.CuryExpenseAmt = (new_aptax.CuryExpenseAmt ?? 0m) + epTaxTran.ClaimCuryExpenseAmt * signOperation;
                    new_aptax.NonDeductibleTaxRate = epTaxTran.NonDeductibleTaxRate;
                    TaxAttribute.SetTaxCalc<InvoiceTran.taxCategoryID>(apDocumentGraphExtension.InvoiceTrans.Cache, null, TaxCalc.ManualCalc);
                    new_aptax = apDocumentGraphExtension.TaxTrans.Update(new_aptax);
                    //On first inserting APTaxTran APTax line will be created automatically. 
                    //However, new APTax will not be inserted on APTaxTran line update, even if we already have more lines.
                    //So, we have to do it manually.
                    LineTax aptax = apDocumentGraphExtension.LineTaxes.Search<LineTax.lineNbr, LineTax.taxID>(tran.LineNbr, new_aptax.TaxID);
                    if (aptax == null)
                    {
                        decimal ClaimCuryTaxableAmt = 0m;
                        decimal ClaimCuryTaxAmt = 0m;
                        decimal ClaimCuryExpenseAmt = 0m;

                        if (CurrencyHelper.IsSameCury(claimdetail.CuryInfoID, claimdetail.ClaimCuryInfoID, expenseCuriInfo, currencyinfo))
                        {
                            ClaimCuryTaxableAmt = epTax.CuryTaxableAmt ?? 0m;
                            ClaimCuryTaxAmt = epTax.CuryTaxAmt ?? 0m;
                            ClaimCuryExpenseAmt = epTax.CuryExpenseAmt ?? 0m;
                        }
                        else if (currencyinfo?.CuryRate != null)
                        {
                            PXCurrencyAttribute.CuryConvCury<EPExpenseClaimDetails.claimCuryInfoID>(expenseClaimGraph.ExpenseClaimDetails.Cache, claimdetail, epTax.TaxableAmt ?? 0m, out ClaimCuryTaxableAmt);
                            PXCurrencyAttribute.CuryConvCury<EPExpenseClaimDetails.claimCuryInfoID>(expenseClaimGraph.ExpenseClaimDetails.Cache, claimdetail, epTax.TaxAmt ?? 0m, out ClaimCuryTaxAmt);
                            PXCurrencyAttribute.CuryConvCury<EPExpenseClaimDetails.claimCuryInfoID>(expenseClaimGraph.ExpenseClaimDetails.Cache, claimdetail, epTax.ExpenseAmt ?? 0m, out ClaimCuryExpenseAmt);
                        }
                        aptax = apDocumentGraphExtension.LineTaxes.Insert(new LineTax()
                        {
                            LineNbr = tran.LineNbr,
                            TaxID = new_aptax.TaxID,
                            TaxRate = epTax.TaxRate,
                            CuryTaxableAmt = ClaimCuryTaxableAmt * signOperation,
                            CuryTaxAmt = ClaimCuryTaxAmt * signOperation,
                            CuryExpenseAmt = ClaimCuryExpenseAmt * signOperation
                        });
                    }
                    Tax taxRow = PXSelect<Tax, Where<Tax.taxID, Equal<Required<Tax.taxID>>>>.Select(docgraph, new_aptax.TaxID);
                    if ((taxRow.TaxCalcLevel == CSTaxCalcLevel.Inclusive ||
                        (invoice.TaxCalcMode == TaxCalculationMode.Gross && taxRow.TaxCalcLevel == CSTaxCalcLevel.CalcOnItemAmt))
                        && (tran.CuryTaxableAmt == null || tran.CuryTaxableAmt == 0m))
                    {
                        tran.CuryTaxableAmt = epTaxTran.ClaimCuryTaxableAmt * signOperation;
                        tran.CuryTaxAmt = epTaxTran.ClaimCuryTaxAmt * signOperation;
                        tran = apDocumentGraphExtension.InvoiceTrans.Update(tran);
                    }
                }
                #endregion
            }

            return tran;
        }
        public void ATPTEFMAutoApplyPPT(List<EPExpenseClaim> list)
        {
            foreach (EPExpenseClaim claim in list)
            {
                ATPTEFMCASetup setup = ATPTEFMCAPreference.Current;
                if (setup != null && claim != null)
                {
                    if (setup.AutoApplyPPT == true)
                    {
                        ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();

                        if (claimExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
                        {
                            foreach (EPExpenseClaimDetails rct in PXSelect<EPExpenseClaimDetails,
                                                                    Where<EPExpenseClaimDetails.refNbr, Equal<Required<EPExpenseClaimDetails.refNbr>>>>
                                                                            .Select(this.Base, claim.RefNbr))
                            {
                                //EPExpenseClaimDetails rct = PXSelect<EPExpenseClaimDetails,
                                //										Where<EPExpenseClaimDetails.refNbr, Equal<Required<EPExpenseClaimDetails.refNbr>>>>
                                //												.Select(this.Base, claim.RefNbr).FirstOrDefault();

                                //EPExpenseClaimDetails rct = PXSelect<EPExpenseClaimDetails,
                                //										Where<EPExpenseClaimDetails.refNbr, Equal<Required<EPExpenseClaimDetails.refNbr>>>>
                                //												.Select(this.Base, claim.RefNbr);


                                ATPTEFMEPExpenseClaimDetailsExt rctExt = rct.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                                ATPTEFMCashAdvance cadv = PXSelect<ATPTEFMCashAdvance,
                                    Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>.Select(this.Base, rctExt.UsrATPTEFMReqRef);
                                if (cadv != null)
                                {


                                    APPaymentEntry paymentGraph = PXGraph.CreateInstance<APPaymentEntry>();
                                    paymentGraph.Document.Current = paymentGraph.Document.Search<APPayment.refNbr>(cadv.BillRefNbr, cadv.BillType);
                                    if (paymentGraph.Document.Current != null)
                                    {


                                        foreach (APInvoice inv in PXSelectReadonly<APInvoice,
                                                            Where<APInvoice.origModule, Equal<BatchModule.moduleEP>,
                                                                    And<APInvoice.origRefNbr, Equal<Required<APInvoice.origRefNbr>>>>>
                                                            .Select(this.Base, claim.RefNbr))
                                        {

                                            //bool hasRow = false;

                                            if (inv.DocType == APDocType.Invoice)
                                            {
                                                APAdjust newadj = new APAdjust();
                                                newadj.AdjdDocType = inv.DocType;
                                                newadj.AdjdRefNbr = inv.RefNbr;
                                                paymentGraph.Adjustments.Insert(newadj);
                                            }

                                        }
                                        paymentGraph.Actions.PressSave();
                                        paymentGraph.releaseFromHold.Press();
                                        paymentGraph.release.Press();

                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
        */
        public void ATPTEFMApplyPPT(List<EPExpenseClaim> list)
        {
            List<string> appliedBills = new List<string>();

            foreach (EPExpenseClaim claim in list)
            {
                ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();
                if (claimExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
                {
                    foreach (EPExpenseClaimDetails rct in PXSelect<EPExpenseClaimDetails,
                                                            Where<EPExpenseClaimDetails.refNbr, Equal<Required<EPExpenseClaimDetails.refNbr>>>>
                                                                    .Select(this.Base, claim.RefNbr))
                    {
                        ATPTEFMEPExpenseClaimDetailsExt rctExt = rct.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                        ATPTEFMCashAdvance cadv = PXSelect<ATPTEFMCashAdvance,
                            Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>.Select(this.Base, rctExt.UsrATPTEFMReqRef);
                        if (cadv != null)
                        {
                            APPaymentEntry paymentGraph = PXGraph.CreateInstance<APPaymentEntry>();
                            paymentGraph.Document.Current = paymentGraph.Document.Search<APPayment.refNbr>(cadv.BillRefNbr, cadv.BillType);
                            if (paymentGraph.Document.Current != null)
                            {
                                bool hasRow = false;

                                foreach (APInvoice inv in PXSelectReadonly<APInvoice,
                                                    Where<APInvoice.origModule, Equal<BatchModule.moduleEP>,
                                                            And<APInvoice.origRefNbr, Equal<Required<APInvoice.origRefNbr>>>>>
                                                    .Select(this.Base, claim.RefNbr))
                                {
                                    if (appliedBills.Contains(inv.RefNbr)) continue;

                                    if (inv.Status == APDocStatus.Open)
                                    {
                                        if (inv.DocType == APDocType.Invoice)
                                        {
                                            APAdjust newadj = new APAdjust();
                                            newadj.AdjdDocType = inv.DocType;
                                            newadj.AdjdRefNbr = inv.RefNbr;
                                            paymentGraph.Adjustments.Update(newadj);
                                            appliedBills.Add(inv.RefNbr);
                                            hasRow = true;
                                        }
                                    }
                                }
                                if (hasRow == true)
                                {
                                    paymentGraph.Save.Press();
                                    //  paymentGraph.release.Press();
                                }
                            }
                        }
                    }
                }

            }

        }
        /*
        private void HandleInvoiceInMultiBaseCurrency<TAPDocument, TGraph>(TGraph docgraph, TAPDocument invoice)
            where TAPDocument : InvoiceBase, new()
            where TGraph : PXGraph, new()
        {
            if (!PXAccess.FeatureInstalled<FeaturesSet.multipleBaseCurrencies>())
            {
                return;
            }

            if (typeof(TGraph) == typeof(APInvoiceEntry))
            {
                var apInvoiceEntryGraph = docgraph as APInvoiceEntry;
                apInvoiceEntryGraph.Document.Cache.RaiseFieldUpdated<APInvoice.branchID>((APInvoice)invoice.Base, invoice.BranchID);
            }
            else if (typeof(TGraph) == typeof(APQuickCheckEntry))
            {
                var apQuickCheckEntryGraph = docgraph as APQuickCheckEntry;
                apQuickCheckEntryGraph.Document.Cache.RaiseFieldUpdated<PX.Objects.AP.Standalone.APQuickCheck.branchID>((PX.Objects.AP.Standalone.APQuickCheck)invoice.Base, invoice.BranchID);
            }
        }
        */
        #endregion
    }
}
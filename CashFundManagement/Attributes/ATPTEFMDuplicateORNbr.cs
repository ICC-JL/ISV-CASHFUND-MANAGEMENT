using CashFundManagement.DAC;
using CashFundManagement.Extensions.DAC;
using PX.Data;
using PX.Objects.EP;
using PX.Data.BQL;
using CashFundManagement.Messages;
using PX.Api;
using PX.Objects.FA;
using PX.Objects.AP;
using PX.Objects.IN;

namespace CashFundManagement.Attributes
{
    public class ATPTEFMDuplicateORNbr : PXEventSubscriberAttribute, IPXFieldVerifyingSubscriber, IPXRowPersistingSubscriber//, IPXRowUpdatedSubscriber
    {
        public virtual void FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e)
        {
            if (e.NewValue != null)
            {
                if (CheckDuplicateVal(e.Row, e.NewValue.ToString(), sender.Graph))
                    sender.RaiseExceptionHandling(_FieldName, e.Row, e.NewValue, new PXSetPropertyKeepPreviousException(ATPTEFMMessages.RefNbrDuplicate));
            }
        }
        public virtual void RowPersisting(PXCache sender, PXRowPersistingEventArgs e)
        {
            if (CheckDuplicateVal(e.Row, sender.Graph))
                sender.RaiseExceptionHandling(_FieldName, e.Row, null, new PXSetPropertyKeepPreviousException(ATPTEFMMessages.RefNbrDuplicate));
        }
        #region Methods
        private bool CheckDuplicateVal(object row, PXGraph graph)
        {
            bool retValue = false;

            EPSetup expenseSetup = PXSelect<EPSetup>.Select(graph);
            ATPTEFMEPSetupExtension expenseSetupExt = expenseSetup.GetExtension<ATPTEFMEPSetupExtension>();

            if (expenseSetupExt.UsrATPTEFMRaiseErrorOnDuplicateRefNbr ?? false)
            {
                if (row is ATPTEFMCAReceiptDetail caRow)
                {
                    retValue = CAReceiptCheckDuplicateProcess(caRow, graph);
                }
                else if (row is EPExpenseClaimDetails erRow)
                {
                    retValue = ERReceiptCheckDuplicateProcess(erRow, graph);
                }
                else if (row is ATPTEFMFundTransactionReceiptDetail ftRow)
                {
                    retValue = FTReceiptCheckDuplicateProcess(ftRow, graph);
                }
            }
            return retValue;
        }
        private bool CheckDuplicateVal(object row, string refNbr, PXGraph graph)
        {
            bool retValue = false;

            EPSetup expenseSetup = PXSelect<EPSetup>.Select(graph);
            ATPTEFMEPSetupExtension expenseSetupExt = expenseSetup.GetExtension<ATPTEFMEPSetupExtension>();

            if (expenseSetupExt.UsrATPTEFMRaiseErrorOnDuplicateRefNbr ?? false)
            {
                if (row is ATPTEFMCAReceiptDetail caRow)
                {
                    retValue = CAReceiptCheckDuplicateProcess(caRow, refNbr, graph);
                }
                else if (row is EPExpenseClaimDetails erRow)
                {
                    retValue = ERReceiptCheckDuplicateProcess(erRow, refNbr, graph);
                }
                else if (row is ATPTEFMFundTransactionReceiptDetail ftRow)
                {
                    retValue = FTReceiptCheckDuplicateProcess(ftRow, refNbr, graph);
                }
            }
            return retValue;
        }
        private bool CAReceiptCheckDuplicateProcess(ATPTEFMCAReceiptDetail row, PXGraph graph)
        {
            bool retValue = false;

            ATPTEFMCAReceiptDetail dup = null;
            EPExpenseClaimDetails erdup = null;
            //CA Receipt
            if (row.VendID == null && (!string.IsNullOrEmpty(row.VendorName)) && (!string.IsNullOrEmpty(row.VendorTin)))
            {
                dup = PXSelect<
                    ATPTEFMCAReceiptDetail,
                    Where<ATPTEFMCAReceiptDetail.refNbr, Equal<@P.AsString>,
                        And<ATPTEFMCAReceiptDetail.cashAdvanceReceiptDetailIID, NotEqual<@P.AsInt>,
                        And<ATPTEFMCAReceiptDetail.inventoryID, Equal<@P.AsInt>,
                        And<ATPTEFMCAReceiptDetail.vendorName, Equal<@P.AsString>,
                        And<ATPTEFMCAReceiptDetail.vendorTin, Equal<@P.AsString>,
                        And<Where<ATPTEFMCAReceiptDetail.vendID, Equal<Empty>,
                            Or<ATPTEFMCAReceiptDetail.vendID, IsNull>>>>>>>>>
                    .Select(graph, row.RefNbr, row.CashAdvanceReceiptDetailIID, row.InventoryID, row.VendorName, row.VendorTin);
            }
            else if (row.VendID != null)
            {
                dup = PXSelect<
                    ATPTEFMCAReceiptDetail,
                    Where<ATPTEFMCAReceiptDetail.refNbr, Equal<@P.AsString>,
                        And<ATPTEFMCAReceiptDetail.vendID, Equal<@P.AsInt>,
                        And<ATPTEFMCAReceiptDetail.cashAdvanceReceiptDetailIID, NotEqual<@P.AsInt>,
                        And<ATPTEFMCAReceiptDetail.inventoryID, Equal<@P.AsInt>>>>>>
                    .Select(graph, row.RefNbr, row.VendID, row.CashAdvanceReceiptDetailIID, row.InventoryID);
            }

            //ER
            Vendor vendor = PXSelect<Vendor, Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>.Select(graph, row.VendID);
            string vendorCD = vendor != null ? vendor.AcctCD : null;

            if (vendorCD == null && (!string.IsNullOrEmpty(row.VendorName)) && (!string.IsNullOrEmpty(row.VendorTin)))
            {
                erdup = PXSelect<
                    EPExpenseClaimDetails,
                    Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                        And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                        And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN, Equal<@P.AsString>,
                        And<Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, IsNull,
                            Or<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<Empty>>>>>>>>>>>
                    .Select(graph, row.RefNbr, ATPTEFMExpenseTypeAttribute.Liquidation, row.ExpenseReceiptRefNbr, row.InventoryID, row.VendorName, row.VendorTin);
            }
            else if (vendorCD != null)
            {
                erdup = PXSelect<
                    EPExpenseClaimDetails,
                    Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                        And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                        And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>>>>>>>
                    .Select(graph, row.RefNbr, vendorCD, ATPTEFMExpenseTypeAttribute.Liquidation, row.ExpenseReceiptRefNbr, row.InventoryID);
            }

            if (dup != null || erdup != null)
            {
                retValue = true;
            }
            return retValue;
        }
        private bool FTReceiptCheckDuplicateProcess(ATPTEFMFundTransactionReceiptDetail row, PXGraph graph)
        {
            bool retValue = false;

            ATPTEFMFundTransactionReceiptDetail dup = null;
            EPExpenseClaimDetails erdup = null;

            //CA Receipt
            if (row.VendID == null && (!string.IsNullOrEmpty(row.VendorName)) && (!string.IsNullOrEmpty(row.VendorTin)))
            {
                dup = PXSelect<
                    ATPTEFMFundTransactionReceiptDetail,
                    Where<ATPTEFMFundTransactionReceiptDetail.refNbr, Equal<@P.AsString>,
                        And<ATPTEFMFundTransactionReceiptDetail.fundTransactionReceiptDetailID, NotEqual<@P.AsInt>,
                        And<ATPTEFMFundTransactionReceiptDetail.inventoryID, Equal<@P.AsInt>,
                        And<ATPTEFMFundTransactionReceiptDetail.vendorName, Equal<@P.AsString>,
                        And<ATPTEFMFundTransactionReceiptDetail.vendorTin, Equal<@P.AsString>,
                        And<Where<ATPTEFMFundTransactionReceiptDetail.vendID, Equal<Empty>,
                            Or<ATPTEFMFundTransactionReceiptDetail.vendID, IsNull>>>>>>>>>
                    .Select(graph, row.RefNbr, row.FundTransactionReceiptDetailID, row.InventoryID, row.VendorName, row.VendorTin);
            }
            else if (row.VendID != null)
            {
                dup = PXSelect<
                    ATPTEFMFundTransactionReceiptDetail,
                    Where<ATPTEFMFundTransactionReceiptDetail.refNbr, Equal<@P.AsString>,
                        And<ATPTEFMFundTransactionReceiptDetail.vendID, Equal<@P.AsInt>,
                        And<ATPTEFMFundTransactionReceiptDetail.fundTransactionReceiptDetailID, NotEqual<@P.AsInt>,
                        And<ATPTEFMFundTransactionReceiptDetail.inventoryID, Equal<@P.AsInt>>>>>>
                    .Select(graph, row.RefNbr, row.VendID, row.FundTransactionReceiptDetailID, row.InventoryID);
            }

            //ER
            Vendor vendor = PXSelect<Vendor, Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>.Select(graph, row.VendID);
            string vendorCD = vendor != null ? vendor.AcctCD : null;

            if (vendorCD == null && (!string.IsNullOrEmpty(row.VendorName)) && (!string.IsNullOrEmpty(row.VendorTin)))
            {
                erdup = PXSelect<
                    EPExpenseClaimDetails,
                    Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                        And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                        And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN, Equal<@P.AsString>,
                        And<Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, IsNull,
                            Or<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<Empty>>>>>>>>>>>
                    .Select(graph, row.RefNbr, ATPTEFMExpenseTypeAttribute.Liquidation, row.ExpenseReceiptRefNbr, row.InventoryID, row.VendorName, row.VendorTin);
            }
            else if (vendorCD != null)
            {
                erdup = PXSelect<
                    EPExpenseClaimDetails,
                    Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                        And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                        And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>>>>>>>
                    .Select(graph, row.RefNbr, vendorCD, ATPTEFMExpenseTypeAttribute.Liquidation, row.ExpenseReceiptRefNbr, row.InventoryID);
            }

            if (dup != null || erdup != null)
            {
                retValue = true;
            }
            return retValue;
        }
        /// <remarks>
        /// 2025-07-30 : Exclude cancelled ER from validation CASE: 012688 {JLTG} <br/> 
        /// 2025-10-10 : LFC - Expense claim upon releasing error: Updating 'Expense Receipt' record raised at least one error. Please review the errors. Reference Nbr already used. CASEID: 013155 {JLTG}
        /// </remarks>
        private bool ERReceiptCheckDuplicateProcess(EPExpenseClaimDetails row, PXGraph graph)
        {
            bool retValue = false;
            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            if (row.Status.Equals(ATPTEFMExpenseReceiptStatusAttribute.CancelledValue)) return false;

            if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
            {
                ATPTEFMCAReceiptDetail dup = null;
                EPExpenseClaimDetails erdup = null;

                //CA Receipt
                if (string.IsNullOrEmpty(rowExt.UsrATPTVendID) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendName)) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendTIN)))
                {
                    dup = PXSelect<
                        ATPTEFMCAReceiptDetail,
                        Where<ATPTEFMCAReceiptDetail.refNbr, Equal<@P.AsString>,
                            And<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, NotEqual<@P.AsString>,
                            And<ATPTEFMCAReceiptDetail.inventoryID, Equal<@P.AsInt>,
                            And<ATPTEFMCAReceiptDetail.vendorName, Equal<@P.AsString>,
                            And<ATPTEFMCAReceiptDetail.vendorTin, Equal<@P.AsString>,
                            And<Where<ATPTEFMCAReceiptDetail.vendID, Equal<Empty>,
                                Or<ATPTEFMCAReceiptDetail.vendID, IsNull>>>>>>>>>
                        .Select(graph, row.ExpenseRefNbr, row.ClaimDetailCD, row.InventoryID, rowExt.UsrATPTVendName, rowExt.UsrATPTVendTIN);
                }
                else if (!string.IsNullOrEmpty(rowExt.UsrATPTVendID))
                {
                    VendorR vendor = PXSelect<
                        VendorR,
                        Where<VendorR.acctCD, Equal<Required<VendorR.acctCD>>>>
                        .Select(graph, rowExt.UsrATPTVendID);
                    dup = PXSelect<
                        ATPTEFMCAReceiptDetail,
                        Where<ATPTEFMCAReceiptDetail.refNbr, Equal<@P.AsString>,
                            And<ATPTEFMCAReceiptDetail.vendID, Equal<@P.AsInt>,
                            And<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, NotEqual<@P.AsString>,
                            And<ATPTEFMCAReceiptDetail.inventoryID, Equal<@P.AsInt>>>>>>
                        .Select(graph, row.ExpenseRefNbr, vendor.BAccountID, row.ClaimDetailCD, row.InventoryID);
                }

                //ER
                if (string.IsNullOrEmpty(rowExt.UsrATPTVendID) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendName)) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendTIN)))
                {
                    erdup = PXSelect<
                        EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                            And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                            And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN, Equal<@P.AsString>,
                            And<Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, IsNull,
                                Or<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<Empty>>>>>>>>>>>
                        .Select(graph, row.ExpenseRefNbr, ATPTEFMExpenseTypeAttribute.Liquidation, row.ClaimDetailCD, row.InventoryID, rowExt.UsrATPTVendName, rowExt.UsrATPTVendTIN);
                }
                else if (!string.IsNullOrEmpty(rowExt.UsrATPTVendID))
                {
                    erdup = PXSelect<
                        EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                            And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                            And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>>>>>>>
                        .Select(graph, row.ExpenseRefNbr, rowExt.UsrATPTVendID, ATPTEFMExpenseTypeAttribute.Liquidation, row.ClaimDetailCD, row.InventoryID);
                }
                if (dup != null || erdup != null)
                {
                    retValue = true;
                }
            }
            else if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Replenishment)
            {
                ATPTEFMFundTransactionReceiptDetail dup = null;
                EPExpenseClaimDetails erdup = null;

                //FT Receipt
                if (string.IsNullOrEmpty(rowExt.UsrATPTVendID) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendName)) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendTIN)))
                {
                    dup = PXSelect<
                        ATPTEFMFundTransactionReceiptDetail,
                        Where<ATPTEFMFundTransactionReceiptDetail.refNbr, Equal<@P.AsString>,
                            And<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, NotEqual<@P.AsString>,
                            And<ATPTEFMFundTransactionReceiptDetail.inventoryID, Equal<@P.AsInt>,
                            And<ATPTEFMFundTransactionReceiptDetail.vendorName, Equal<@P.AsString>,
                            And<ATPTEFMFundTransactionReceiptDetail.vendorTin, Equal<@P.AsString>,
                            And<Where<ATPTEFMFundTransactionReceiptDetail.vendID, Equal<Empty>,
                                Or<ATPTEFMFundTransactionReceiptDetail.vendID, IsNull>>>>>>>>>
                        .Select(graph, row.ExpenseRefNbr, row.ClaimDetailCD, row.InventoryID, rowExt.UsrATPTVendName, rowExt.UsrATPTVendTIN);
                }
                else if (!string.IsNullOrEmpty(rowExt.UsrATPTVendID))
                {
                    VendorR vendor = PXSelect<
                        VendorR,
                        Where<VendorR.acctCD, Equal<Required<VendorR.acctCD>>>>
                        .Select(graph, rowExt.UsrATPTVendID);
                    dup = PXSelect<
                        ATPTEFMFundTransactionReceiptDetail,
                        Where<ATPTEFMFundTransactionReceiptDetail.refNbr, Equal<@P.AsString>,
                            And<ATPTEFMFundTransactionReceiptDetail.vendID, Equal<@P.AsInt>,
                            And<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, NotEqual<@P.AsString>,
                            And<ATPTEFMFundTransactionReceiptDetail.inventoryID, Equal<@P.AsInt>>>>>>
                        .Select(graph, row.ExpenseRefNbr, vendor.BAccountID, row.ClaimDetailCD, row.InventoryID);
                }

                //ER
                if (string.IsNullOrEmpty(rowExt.UsrATPTVendID) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendName)) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendTIN)))
                {
                    erdup = PXSelect<
                        EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                            And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                            And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN, Equal<@P.AsString>,
                            And<Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, IsNull,
                                Or<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<Empty>>>>>>>>>>>
                        .Select(graph, row.ExpenseRefNbr, ATPTEFMExpenseTypeAttribute.Replenishment, row.ClaimDetailCD, row.InventoryID, rowExt.UsrATPTVendName, rowExt.UsrATPTVendTIN);
                }
                else if (!string.IsNullOrEmpty(rowExt.UsrATPTVendID))
                {
                    erdup = PXSelect<
                        EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                            And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                            And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>>>>>>>
                        .Select(graph, row.ExpenseRefNbr, rowExt.UsrATPTVendID, ATPTEFMExpenseTypeAttribute.Replenishment, row.ClaimDetailCD, row.InventoryID);
                }
                if (dup != null || erdup != null)
                {
                    retValue = true;
                }
            }
            else
            {
                EPExpenseClaimDetails erdup = null;

                if (string.IsNullOrEmpty(rowExt.UsrATPTVendID) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendName)) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendTIN)))
                {
                    erdup = PXSelect<
                        EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                            And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                            And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN, Equal<@P.AsString>,
                            And<Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, IsNull,
                                Or<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<Empty>>>>>>>>>>>
                        .Select(graph, row.ExpenseRefNbr, ATPTEFMExpenseTypeAttribute.RequestforPayment, row.ClaimDetailCD, row.InventoryID, rowExt.UsrATPTVendName, rowExt.UsrATPTVendTIN);
                }
                else if (!string.IsNullOrEmpty(rowExt.UsrATPTVendID))
                {
                    erdup = PXSelect<
                        EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                            And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                            And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>>>>>>>
                        .Select(graph, row.ExpenseRefNbr, rowExt.UsrATPTVendID, ATPTEFMExpenseTypeAttribute.RequestforPayment, row.ClaimDetailCD, row.InventoryID);
                }
                if (erdup != null && !string.IsNullOrEmpty(erdup.ExpenseRefNbr))
                {
                    retValue = true;
                }
            }
            return retValue;
        }
        private bool CAReceiptCheckDuplicateProcess(ATPTEFMCAReceiptDetail row, string refnbr, PXGraph graph)
        {
            bool retValue = false;

            ATPTEFMCAReceiptDetail dup = null;
            EPExpenseClaimDetails erdup = null;
            //CA Receipt
            if (row.VendID == null && (!string.IsNullOrEmpty(row.VendorName)) && (!string.IsNullOrEmpty(row.VendorTin)))
            {
                dup = PXSelect<
                    ATPTEFMCAReceiptDetail,
                    Where<ATPTEFMCAReceiptDetail.refNbr, Equal<@P.AsString>,
                        And<ATPTEFMCAReceiptDetail.cashAdvanceReceiptDetailIID, NotEqual<@P.AsInt>,
                        And<ATPTEFMCAReceiptDetail.inventoryID, Equal<@P.AsInt>,
                        And<ATPTEFMCAReceiptDetail.vendorName, Equal<@P.AsString>,
                        And<ATPTEFMCAReceiptDetail.vendorTin, Equal<@P.AsString>,
                        And<Where<ATPTEFMCAReceiptDetail.vendID, Equal<Empty>,
                            Or<ATPTEFMCAReceiptDetail.vendID, IsNull>>>>>>>>>
                    .Select(graph, refnbr, row.CashAdvanceReceiptDetailIID, row.InventoryID, row.VendorName, row.VendorTin);
            }
            else if (row.VendID != null)
            {
                dup = PXSelect<
                    ATPTEFMCAReceiptDetail,
                    Where<ATPTEFMCAReceiptDetail.refNbr, Equal<@P.AsString>,
                        And<ATPTEFMCAReceiptDetail.vendID, Equal<@P.AsInt>,
                        And<ATPTEFMCAReceiptDetail.cashAdvanceReceiptDetailIID, NotEqual<@P.AsInt>,
                        And<ATPTEFMCAReceiptDetail.inventoryID, Equal<@P.AsInt>>>>>>
                    .Select(graph, refnbr, row.VendID, row.CashAdvanceReceiptDetailIID, row.InventoryID);
            }

            //ER
            Vendor vendor = PXSelect<Vendor, Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>.Select(graph, row.VendID);
            string vendorCD = vendor != null ? vendor.AcctCD : null;

            if (vendorCD == null && (!string.IsNullOrEmpty(row.VendorName)) && (!string.IsNullOrEmpty(row.VendorTin)))
            {
                erdup = PXSelect<
                    EPExpenseClaimDetails,
                    Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                        And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                        And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN, Equal<@P.AsString>,
                        And<Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, IsNull,
                            Or<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<Empty>>>>>>>>>>>
                    .Select(graph, refnbr, ATPTEFMExpenseTypeAttribute.Liquidation, row.ExpenseReceiptRefNbr, row.InventoryID, row.VendorName, row.VendorTin);
            }
            else if (vendorCD != null)
            {
                erdup = PXSelect<
                    EPExpenseClaimDetails,
                    Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                        And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                        And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>>>>>>>
                    .Select(graph, refnbr, vendorCD, ATPTEFMExpenseTypeAttribute.Liquidation, row.ExpenseReceiptRefNbr, row.InventoryID);
            }

            if (dup != null || erdup != null)
            {
                retValue = true;
            }
            return retValue;
        }
        private bool FTReceiptCheckDuplicateProcess(ATPTEFMFundTransactionReceiptDetail row, string refnbr, PXGraph graph)
        {
            bool retValue = false;

            ATPTEFMFundTransactionReceiptDetail dup = null;
            EPExpenseClaimDetails erdup = null;

            //CA Receipt
            if (row.VendID == null && (!string.IsNullOrEmpty(row.VendorName)) && (!string.IsNullOrEmpty(row.VendorTin)))
            {
                dup = PXSelect<
                    ATPTEFMFundTransactionReceiptDetail,
                    Where<ATPTEFMFundTransactionReceiptDetail.refNbr, Equal<@P.AsString>,
                        And<ATPTEFMFundTransactionReceiptDetail.fundTransactionReceiptDetailID, NotEqual<@P.AsInt>,
                        And<ATPTEFMFundTransactionReceiptDetail.inventoryID, Equal<@P.AsInt>,
                        And<ATPTEFMFundTransactionReceiptDetail.vendorName, Equal<@P.AsString>,
                        And<ATPTEFMFundTransactionReceiptDetail.vendorTin, Equal<@P.AsString>,
                        And<Where<ATPTEFMFundTransactionReceiptDetail.vendID, Equal<Empty>,
                            Or<ATPTEFMFundTransactionReceiptDetail.vendID, IsNull>>>>>>>>>
                    .Select(graph, refnbr, row.FundTransactionReceiptDetailID, row.InventoryID, row.VendorName, row.VendorTin);
            }
            else if (row.VendID != null)
            {
                dup = PXSelect<
                    ATPTEFMFundTransactionReceiptDetail,
                    Where<ATPTEFMFundTransactionReceiptDetail.refNbr, Equal<@P.AsString>,
                        And<ATPTEFMFundTransactionReceiptDetail.vendID, Equal<@P.AsInt>,
                        And<ATPTEFMFundTransactionReceiptDetail.fundTransactionReceiptDetailID, NotEqual<@P.AsInt>,
                        And<ATPTEFMFundTransactionReceiptDetail.inventoryID, Equal<@P.AsInt>>>>>>
                    .Select(graph, refnbr, row.VendID, row.FundTransactionReceiptDetailID, row.InventoryID);
            }

            //ER
            Vendor vendor = PXSelect<Vendor, Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>.Select(graph, row.VendID);
            string vendorCD = vendor != null ? vendor.AcctCD : null;

            if (vendorCD == null && (!string.IsNullOrEmpty(row.VendorName)) && (!string.IsNullOrEmpty(row.VendorTin)))
            {
                erdup = PXSelect<
                    EPExpenseClaimDetails,
                    Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                        And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                        And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN, Equal<@P.AsString>,
                        And<Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, IsNull,
                            Or<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<Empty>>>>>>>>>>>
                    .Select(graph, refnbr, ATPTEFMExpenseTypeAttribute.Liquidation, row.ExpenseReceiptRefNbr, row.InventoryID, row.VendorName, row.VendorTin);
            }
            else if (vendorCD != null)
            {
                erdup = PXSelect<
                    EPExpenseClaimDetails,
                    Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                        And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                        And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>>>>>>>
                    .Select(graph, refnbr, vendorCD, ATPTEFMExpenseTypeAttribute.Liquidation, row.ExpenseReceiptRefNbr, row.InventoryID);
            }

            if (dup != null || erdup != null)
            {
                retValue = true;
            }
            return retValue;
        }
        private bool ERReceiptCheckDuplicateProcess(EPExpenseClaimDetails row, string refnbr, PXGraph graph)
        {
            bool retValue = false;
            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
            {
                ATPTEFMCAReceiptDetail dup = null;
                EPExpenseClaimDetails erdup = null;

                //CA Receipt
                if (string.IsNullOrEmpty(rowExt.UsrATPTVendID) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendName)) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendTIN)))
                {
                    dup = PXSelect<
                        ATPTEFMCAReceiptDetail,
                        Where<ATPTEFMCAReceiptDetail.refNbr, Equal<@P.AsString>,
                            And<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, NotEqual<@P.AsString>,
                            And<ATPTEFMCAReceiptDetail.inventoryID, Equal<@P.AsInt>,
                            And<ATPTEFMCAReceiptDetail.vendorName, Equal<@P.AsString>,
                            And<ATPTEFMCAReceiptDetail.vendorTin, Equal<@P.AsString>,
                            And<Where<ATPTEFMCAReceiptDetail.vendID, Equal<Empty>,
                                Or<ATPTEFMCAReceiptDetail.vendID, IsNull>>>>>>>>>
                        .Select(graph, refnbr, row.ClaimDetailCD, row.InventoryID, rowExt.UsrATPTVendName, rowExt.UsrATPTVendTIN);
                }
                else if (!string.IsNullOrEmpty(rowExt.UsrATPTVendID))
                {
                    VendorR vendor = PXSelect<
                        VendorR,
                        Where<VendorR.acctCD, Equal<Required<VendorR.acctCD>>>>
                        .Select(graph, rowExt.UsrATPTVendID);
                    dup = PXSelect<
                        ATPTEFMCAReceiptDetail,
                        Where<ATPTEFMCAReceiptDetail.refNbr, Equal<@P.AsString>,
                            And<ATPTEFMCAReceiptDetail.vendID, Equal<@P.AsInt>,
                            And<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, NotEqual<@P.AsString>,
                            And<ATPTEFMCAReceiptDetail.inventoryID, Equal<@P.AsInt>>>>>>
                        .Select(graph, refnbr, vendor.BAccountID, row.ClaimDetailCD, row.InventoryID);
                }

                //ER
                if (string.IsNullOrEmpty(rowExt.UsrATPTVendID) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendName)) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendTIN)))
                {
                    erdup = PXSelect<
                        EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                            And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                            And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN, Equal<@P.AsString>,
                            And<Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, IsNull,
                                Or<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<Empty>>>>>>>>>>>
                        .Select(graph, refnbr, ATPTEFMExpenseTypeAttribute.Liquidation, row.ClaimDetailCD, row.InventoryID, rowExt.UsrATPTVendName, rowExt.UsrATPTVendTIN);
                }
                else if (!string.IsNullOrEmpty(rowExt.UsrATPTVendID))
                {
                    erdup = PXSelect<
                        EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                            And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                            And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>>>>>>>
                        .Select(graph, refnbr, rowExt.UsrATPTVendID, ATPTEFMExpenseTypeAttribute.Liquidation, row.ClaimDetailCD, row.InventoryID);
                }
                if (dup != null || erdup != null)
                {
                    retValue = true;
                }
            }
            else if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Replenishment)
            {
                ATPTEFMFundTransactionReceiptDetail dup = null;
                EPExpenseClaimDetails erdup = null;

                //FT Receipt
                if (string.IsNullOrEmpty(rowExt.UsrATPTVendID) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendName)) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendTIN)))
                {
                    dup = PXSelect<
                        ATPTEFMFundTransactionReceiptDetail,
                        Where<ATPTEFMFundTransactionReceiptDetail.refNbr, Equal<@P.AsString>,
                            And<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, NotEqual<@P.AsString>,
                            And<ATPTEFMFundTransactionReceiptDetail.inventoryID, Equal<@P.AsInt>,
                            And<ATPTEFMFundTransactionReceiptDetail.vendorName, Equal<@P.AsString>,
                            And<ATPTEFMFundTransactionReceiptDetail.vendorTin, Equal<@P.AsString>,
                            And<Where<ATPTEFMFundTransactionReceiptDetail.vendID, Equal<Empty>,
                                Or<ATPTEFMFundTransactionReceiptDetail.vendID, IsNull>>>>>>>>>
                        .Select(graph, refnbr, row.ClaimDetailCD, row.InventoryID, rowExt.UsrATPTVendName, rowExt.UsrATPTVendTIN);
                }
                else if (!string.IsNullOrEmpty(rowExt.UsrATPTVendID))
                {
                    VendorR vendor = PXSelect<
                        VendorR,
                        Where<VendorR.acctCD, Equal<Required<VendorR.acctCD>>>>
                        .Select(graph, rowExt.UsrATPTVendID);
                    dup = PXSelect<
                        ATPTEFMFundTransactionReceiptDetail,
                        Where<ATPTEFMFundTransactionReceiptDetail.refNbr, Equal<@P.AsString>,
                            And<ATPTEFMFundTransactionReceiptDetail.vendID, Equal<@P.AsInt>,
                            And<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, NotEqual<@P.AsString>,
                            And<ATPTEFMFundTransactionReceiptDetail.inventoryID, Equal<@P.AsInt>>>>>>
                        .Select(graph, refnbr, vendor.BAccountID, row.ClaimDetailCD, row.InventoryID);
                }

                //ER
                if (string.IsNullOrEmpty(rowExt.UsrATPTVendID) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendName)) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendTIN)))
                {
                    erdup = PXSelect<
                        EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                            And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                            And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN, Equal<@P.AsString>,
                            And<Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, IsNull,
                                Or<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<Empty>>>>>>>>>>>
                        .Select(graph, refnbr, ATPTEFMExpenseTypeAttribute.Replenishment, row.ClaimDetailCD, row.InventoryID, rowExt.UsrATPTVendName, rowExt.UsrATPTVendTIN);
                }
                else if (!string.IsNullOrEmpty(rowExt.UsrATPTVendID))
                {
                    erdup = PXSelect<
                        EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                            And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                            And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>>>>>>>
                        .Select(graph, refnbr, rowExt.UsrATPTVendID, ATPTEFMExpenseTypeAttribute.Replenishment, row.ClaimDetailCD, row.InventoryID);
                }
                if (dup != null || erdup != null)
                {
                    retValue = true;
                }
            }
            else
            {
                EPExpenseClaimDetails erdup = null;

                if (string.IsNullOrEmpty(rowExt.UsrATPTVendID) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendName)) && (!string.IsNullOrEmpty(rowExt.UsrATPTVendTIN)))
                {
                    erdup = PXSelect<
                        EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                            And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                            And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN, Equal<@P.AsString>,
                            And<Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, IsNull,
                                Or<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<Empty>>>>>>>>>>>
                        .Select(graph, refnbr, ATPTEFMExpenseTypeAttribute.RequestforPayment, row.ClaimDetailCD, row.InventoryID, rowExt.UsrATPTVendName, rowExt.UsrATPTVendTIN);
                }
                else if (!string.IsNullOrEmpty(rowExt.UsrATPTVendID))
                {
                    erdup = PXSelect<
                        EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Equal<@P.AsString>,
                            And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                            And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                            And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>>>>>>>
                        .Select(graph, refnbr, rowExt.UsrATPTVendID, ATPTEFMExpenseTypeAttribute.RequestforPayment, row.ClaimDetailCD, row.InventoryID);
                }
                if (erdup != null)
                {
                    retValue = true;
                }
            }
            return retValue;
        }
        #endregion
    }
}

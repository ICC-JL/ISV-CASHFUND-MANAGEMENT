<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="ATPT9201.aspx.cs" Inherits="Page_ATPT9201" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:content id="cont1" contentplaceholderid="phDS" runat="Server">
    <px:pxdatasource id="ds" runat="server" visible="True" primaryview="DataFixDocument" suspendunloading="False" typename="CashFundManagement.BLC.ATPTEFMCashFundDataFixMaint">
    </px:pxdatasource>
</asp:content>
<asp:content id="cont2" contentplaceholderid="phF" runat="Server">
    <px:pxformview id="form" runat="server" datasourceid="ds" style="z-index: 100" width="100%" datamember="DataFixDocument" tabindex="2500">
        <template>
            <px:pxformview id="frmCFMDataFix" runat="server" captionvisible="false" datamember="DataFixDocument" datasourceid="ds" tabindex="1000">
                <template>
                    <px:pxtextedit id="edPassword" runat="server" datafield="Password" width="200px" defaultlocale="" isclientcontrol="True" textmode="Password">
                    </px:pxtextedit>
                </template>
                <contentlayout columnswidth="M" labelswidth="SM" />
            </px:pxformview>
            <px:pxlayoutrule runat="server" groupcaption="Group Caption" startcolumn="True" labelswidth="SM" controlsize="M" />

            <px:pxlayoutrule runat="server" startrow="True" groupcaption="Cash Advance Module" startcolumn="True" labelswidth="SM" />
            <px:pxcheckbox id="edCAPendingForLiquidationToClosed" runat="server" alreadylocalized="False" datafield="CAPendingForLiquidationToClosed" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxlabel id="lblUpdateCAStatusToClose" runat="server" alreadylocalized="False">NOTE: Select this if Cash Advance status is pending for liquidation and prepayment status is closed</px:pxlabel>
            <px:pxcheckbox id="edCaPPMBalanceNotEqualToBalance" runat="server" alreadylocalized="False" datafield="CaPPMBalanceNotEqualToBalance" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxlabel id="edSelectCa" runat="server" alreadylocalized="False">NOTE: Select this if Cash Advance Balance is not equal to balance</px:pxlabel>
            <px:pxcheckbox id="edBlankUnitCost" runat="server" alreadylocalized="False" datafield="BlankUnitCost" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="Pxcheckbox1" runat="server" alreadylocalized="False" datafield="BlankReceiptUnitCost" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxmultiselector id="edUnlinkCABillRefNbr" runat="server" autorefresh="true" allowcustomitems="true" datafield="UnlinkCABillRefNbr" commitchanges="True">
            </px:pxmultiselector>
            <px:pxcheckbox id="edCAInitialLiquidationDatafixForOldData" runat="server" alreadylocalized="False" datafield="CAInitialLiquidationDatafixForOldData" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edEFFOverrideReceiptsSetToFalse" runat="server" alreadylocalized="False" datafield="EFFOverrideReceiptsSetToFalse" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edCAwoLineDescriptionDatafix" runat="server" alreadylocalized="False" datafield="CAwoLineDescriptionDatafix" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxmultiselector id="edCloseCAWithBalance" runat="server" autorefresh="true" allowcustomitems="true" datafield="CloseCAWithBalance" commitchanges="True">
            </px:pxmultiselector>
            <px:pxcheckbox id="edCAReceiptAlreadyCancelledButExistInCAReceiptsTab" runat="server" alreadylocalized="False" datafield="CAReceiptAlreadyCancelledButExistInCAReceiptsTab" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxlabel id="Pxlabel6" runat="server" alreadylocalized="False">NOTE: Select this if Processed CA Receipt Already Cancelled But Exist In CA Receipts Tab</px:pxlabel>
            <px:pxselector id="edCAVendorRefundWrongAmt" runat="server" datafield="CAVendorRefundWrongAmt" commitchanges="True"></px:pxselector>

            <px:pxselector id="edCleanUpCAReceiptsIssues" runat="server" datafield="CleanUpCAReceiptsIssues" commitchanges="True"></px:pxselector>
            <px:pxlabel id="Pxlabel7" runat="server" alreadylocalized="False">NOTE: Select CA to clean up duplicate refnbrs and ghost liquidation nbrs</px:pxlabel>

            <px:pxselector id="edDeleteCAReceiptsWithRefs" runat="server" datafield="DeleteCAReceiptsWithRefs" commitchanges="True"></px:pxselector>
            <px:pxlabel id="lblDeleteCAReceiptsWithRefs" runat="server" alreadylocalized="False">NOTE: Delete all CA receipts where CashAdvanceRequestDetailID = 0</px:pxlabel>

            <px:pxselector id="edCAZeroLiquidationAmt" runat="server" datafield="CAZeroLiquidationAmt" commitchanges="True"></px:pxselector>
            <px:pxlabel id="Pxlabel8" runat="server" alreadylocalized="False">NOTE: Set liquidation amount to zero</px:pxlabel>

            <px:pxselector id="edCAStuckRefundFixer" runat="server" datafield="CAStuckRefundFixer" commitchanges="True"></px:pxselector>
            <px:pxlabel id="lblCAStuckRefundFixer" runat="server" alreadylocalized="False">NOTE: Fix stuck CA refund by syncing Documents to Apply with PPM Balance</px:pxlabel>

            <px:pxselector id="edCARecalcLiquidationAmt" runat="server" datafield="CARecalcLiquidationAmt" commitchanges="True"></px:pxselector>
            <px:pxlabel id="lblCARecalcLiquidationAmt" runat="server" alreadylocalized="False">NOTE: Recalculate CA Liquidation amount from non-reversed receipt details</px:pxlabel>

            <px:pxlayoutrule runat="server" groupcaption="Expense Claim Module" labelswidth="SM" controlsize="M" />
            <px:pxmultiselector id="edRefNbr" runat="server" autorefresh="true" allowcustomitems="true" datafield="RefNbr" commitchanges="True">
            </px:pxmultiselector>
            <px:pxdropdown id="edStatus" runat="server" datafield="Status" isclientcontrol="True">
            </px:pxdropdown>
            <px:pxlabel id="PXLabel1" runat="server" alreadylocalized="False">NOTE: This is for an Expense Claim Transaction that has been released, and no bill has been created.</px:pxlabel>

            <px:pxcheckbox id="edECMissingDetailsDataFix" runat="server" alreadylocalized="False" datafield="ECMissingDetailsDataFix" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>

            <px:pxmultiselector id="edECDeleteApprovalsForCancelledStatus" runat="server" autorefresh="true" allowcustomitems="true" datafield="ECDeleteApprovalsForCancelledStatus" commitchanges="True">
            </px:pxmultiselector>
            <px:pxlabel id="PXLabel5" runat="server" alreadylocalized="False">NOTE: Delete all approvals in a cancelled claim.</px:pxlabel>

            <px:pxlayoutrule runat="server" groupcaption="Fund Module" labelswidth="SM" controlsize="M" />
            <px:pxcheckbox id="edUpdateFundClearingAccounts" runat="server" alreadylocalized="False" datafield="UpdateFundClearingAccounts" isclientcontrol="True" alignleft="True" commitchanges="True">
            </px:pxcheckbox>
            <px:pxlabel id="lblUpdateFundClearingAccounts" runat="server" alreadylocalized="False">NOTE: Select this to update funds with empty clearing account/subaccount from Fund Management Preferences.</px:pxlabel>

            <px:pxcheckbox id="edInsertDefaultForOldFundsFundCD" runat="server" alreadylocalized="False" datafield="InsertDefaultForOldFundsFundCD" isclientcontrol="True" alignleft="True" commitchanges="True">
            </px:pxcheckbox>
            <px:pxlabel id="PXLabel3" runat="server" alreadylocalized="False">NOTE: Insert Default value for Old Funds.</px:pxlabel>

            <px:pxmultiselector id="PXMultiSelector1" runat="server" autorefresh="true" allowcustomitems="true" datafield="FundReplenishPoint" commitchanges="True">
            </px:pxmultiselector>
            <px:pxnumberedit id="edReplenishPointPercent" runat="server" alreadylocalized="False" commitchanges="True" datafield="ReplenishPointPercent" isclientcontrol="True">
            </px:pxnumberedit>
            <px:pxlabel id="PXLabel2" runat="server" alreadylocalized="False">NOTE: Allows the user to correct fund replenishment point.</px:pxlabel>

            <px:pxmultiselector id="edQmazFundCD" runat="server" autorefresh="true" allowcustomitems="true" datafield="QmazFundCD" commitchanges="True">
            </px:pxmultiselector>

            <px:pxlayoutrule runat="server" groupcaption="Replenishment Module" labelswidth="SM" controlsize="M" />
            <px:pxmultiselector id="edEmptyReplenishmentDetailReplenishmentRefNbr" runat="server" autorefresh="true" allowcustomitems="true" datafield="EmptyReplenishmentDetailReplenishmentRefNbr" commitchanges="True">
            </px:pxmultiselector>
            <px:pxlabel id="lblEmptyReplenishmentDetailReplenishmentRefNbr" runat="server" alreadylocalized="False">NOTE: This is for Replenishments that has empty details but has summary amount and Taxes.</px:pxlabel>

            <px:pxcheckbox id="edDataFixForFTReceiptswithReplenishmentRefNbrbutalreadyDeletedinReplenishment" runat="server" alreadylocalized="False" datafield="DataFixForFTReceiptswithReplenishmentRefNbrbutalreadyDeletedinReplenishment" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxlabel id="lblDataFixForFTReceiptswithReplenishmentRefNbrbutalreadyDeletedinReplenishment" runat="server" alreadylocalized="False">NOTE: Data Fix For FT Receipts with Replenishment Ref Nbr but already Deleted in Replenishment.</px:pxlabel>

            <px:pxmultiselector id="edNullChangeAmountDataFix" runat="server" autorefresh="true" allowcustomitems="true" datafield="NullChangeAmountDataFix" commitchanges="True">
            </px:pxmultiselector>
            <px:pxlabel id="lblNullChangeAmountDataFix" runat="server" alreadylocalized="False">NOTE: Replenishment Unable to Release due to FT Null Change Amount Field.</px:pxlabel>

            <px:pxmultiselector id="edReplenishmentNrbNeedsToBeOpen" runat="server" autorefresh="true" allowcustomitems="true" datafield="ReplenishmentNrbNeedsToBeOpen" commitchanges="True">
            </px:pxmultiselector>
            <px:pxlabel id="PXLabel4" runat="server" alreadylocalized="False">NOTE: For those replenishment bills that have already been reversed but still have a status of closed that needs to be reopen.</px:pxlabel>

            <px:pxlayoutrule runat="server" groupcaption="Fund Module (IMPORT unbound data to bounded data)" labelswidth="SM" controlsize="M" />
            <px:pxselector id="edFundCD" runat="server" datafield="FundCD">
            </px:pxselector>

            <px:pxlayoutrule runat="server" groupcaption="Fund Module (IMPORT Base Fields to Cury Fields)" labelswidth="SM" controlsize="M" />
            <px:pxselector id="edFundCDCurrency" runat="server" datafield="FundCDCurrency">
            </px:pxselector>

            <px:pxlayoutrule runat="server" groupcaption="Update Budget for Old Data" labelswidth="SM" controlsize="M" />
            <px:pxcheckbox id="edUpdateCABudgetEnabled" runat="server" alreadylocalized="False" datafield="UpdateCABudgetEnabled" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edUpdateFTBudgetEnabled" runat="server" alreadylocalized="False" datafield="UpdateFTBudgetEnabled" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edUpdateRFPBudgetEnabled" runat="server" alreadylocalized="False" datafield="UpdateRFPBudgetEnabled" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edUpdateBillsBudgetEnabled" runat="server" alreadylocalized="False" datafield="UpdateBillsBudgetEnabled" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>

            <px:pxlayoutrule runat="server" groupcaption="Fund Transaction Module" labelswidth="SM" controlsize="M" />
            <px:pxcheckbox id="edFTInitialLiquidationDatafixForOldData" runat="server" alreadylocalized="False" datafield="FTInitialLiquidationDatafixForOldData" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edFTExpectedDateOfUseDatafixForOldData" runat="server" alreadylocalized="False" datafield="FTExpectedDateOfUseDatafixForOldData" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edFTwoLineDescriptionDatafix" runat="server" alreadylocalized="False" datafield="FTwoLineDescriptionDatafix" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edFundManagementPreferencePopulateGLAccountsSetup" runat="server" alreadylocalized="False" datafield="FundManagementPreferencePopulateGLAccountsSetup" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>

            <px:pxlayoutrule runat="server" groupcaption="LEP Setup for CFM Pages with Manual GI" labelswidth="SM" controlsize="M" />
            <px:pxcheckbox id="edLEPSetup" runat="server" alreadylocalized="False" datafield="LEPSetup" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>

            <px:pxlayoutrule runat="server" groupcaption="Recalculate Fund History Balance And Sorting Fixer" labelswidth="SM" controlsize="M" />
            <px:pxselector id="edFundHistoryBalanceAndSortingFixer" runat="server" datafield="FundHistoryBalanceAndSortingFixer">
            </px:pxselector>

            <px:pxlayoutrule runat="server" groupcaption="Recalcualte Fund History Balance Fixer" labelswidth="SM" controlsize="M" />
            <px:pxselector id="edFundHistoryBalanceFixer" runat="server" datafield="FundHistoryBalanceFixer">
            </px:pxselector>

            <px:pxlayoutrule runat="server" groupcaption="Remove Duplicates and Recalcualte Fund History Balance Fixer" labelswidth="SM" controlsize="M" />
            <px:pxselector id="edFundHistoryDuplicateBalanceFixer" runat="server" datafield="FundHistoryDuplicateBalanceFixer">
            </px:pxselector>

            <px:pxcheckbox id="edFTReceiptsBranchIDMigration" runat="server" alreadylocalized="False" datafield="FTReceiptsBranchIDMigration" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>
            <px:pxlayoutrule runat="server" groupcaption="Remove FT Detail Lines from Reimbursement Type FT" labelswidth="SM" controlsize="M" />
            <px:pxmultiselector id="edRemoveReimbursementFTDetails" runat="server" autorefresh="true" allowcustomitems="false" datafield="RemoveReimbursementFTDetails" commitchanges="True">
            </px:pxmultiselector>
            <px:pxlabel id="PXLabel11" runat="server" alreadylocalized="False">Note: Lists Reimbursement-type Fund Transactions that still have request/detail lines; selecting one removes those lines.</px:pxlabel>

            <px:pxlayoutrule runat="server" groupcaption="Time & Expenses" labelswidth="SM" controlsize="M" />
            <px:pxcheckbox id="edDisableAutomaticReleaseAPAndRequireApprovalOnRFPBill" runat="server" alreadylocalized="False" datafield="DisableAutomaticReleaseAPAndRequireApprovalOnRFPBill" isclientcontrol="True" alignleft="True">
            </px:pxcheckbox>

            <px:pxlayoutrule runat="server" groupcaption="Expense Receipt Module" labelswidth="SM" controlsize="M" />
            <px:pxmultiselector id="edFundHistoryErNbr" runat="server" autorefresh="true" allowcustomitems="true" datafield="FundHistoryErNbr" commitchanges="True">
            </px:pxmultiselector>
            <px:pxlabel id="PXLabel9" runat="server" alreadylocalized="False">Note: This is for a receipt in fund history where the status needs to change to liquidated.</px:pxlabel>
            <px:pxmultiselector id="edClaimDetailCD" runat="server" autorefresh="true" allowcustomitems="true" datafield="ClaimDetailCD" commitchanges="True">
            </px:pxmultiselector>
            <px:pxlabel id="PXLabel10" runat="server" alreadylocalized="False">Note: This is for a receipt in fund history where the status needs to change to Cancelled.</px:pxlabel>

        </template>
        <autosize container="Window" enabled="True" minheight="200" />
    </px:pxformview>
</asp:content>

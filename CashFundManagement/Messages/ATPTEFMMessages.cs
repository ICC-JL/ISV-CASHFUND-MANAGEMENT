using PX.Common;
using PX.Data;

namespace CashFundManagement.Messages
{
    //TODO : Separate classes for error messages and labels, this class must be under ATPTEFMMessages
    [PXLocalizable]
    public class ATPTEFMMessages
    {
        public const string BudgetBusinessDateFinPeriodError = "Budget YTD Calculation using Business Date as Fin Period error.";
        public const string LiquidationDateHasBeenManuallyAdjusted = "Liquidation Date has been manually adjusted.";
        public const string EnableCFM = "Enable Cash Fund Management";
        public const string RefNbrDuplicate = "Reference Nbr already used.";
        public const string RaiseErrorOnDuplicateRefNbr = "Raise an Error on Duplicate Reference Nbr";
        public const string AmountRequestedIsGreaterThanRemainingBudget = "Amount Requested is greater than the Remaining Budget";
        public const string PendingForLiquidation = "Pending for Liquidation";
        public const string ActivateFund = "Activate Fund";
        public const string IsOverBudget = "Is Over Budget";
        public const string HasInitialBudget = "Has Initial Budget";
        public const string EmployeeNotUser = "Employee must be a user.";
        public const string CloseFundValidation = "Warning: You are about to close a fund. Once fund is closed, it can no longer be reopened";
        public const string NewKey = " <New>";
        public const string NoFundIDSelected = "No Fund ID selected.";

        public const string MigratedBill = "MIGRATED BILL";
        public const string Release = "Release";
        public const string ReleaseAll = "Release All";
        public const string BatchNbr = "Batch Nbr";
        public const string ReversingBatchNbr = "Reversing Batch Nbr";

        public const string AlreadyReleased = "Record already released.";
        public const string MassProcessReleased = "Record has been successfully released.";

        public const string Distribute = "Distribute";
        public const string PreloadArticles = "Preload Articles";

        public const string InventoryItemIsNotAnExpenseType = "Only inventory items of the Expense type can be selected.";

        public const string LowestFinPeriodID = "200001";
        public const string BillsAndAdjustments = "Bills & Adjustments";
        public const string Requests = "Requests";

        public const string ReleaseBill = "Cash advance request cannot be submitted. \nBill should be released to proceed with the transaction.";
        public const string CheckBudget = "Check for budget exceed in request item.";
        public const string CheckProjectBudget = "Check for project budget exceed in request item.";
        public const string RemainingBudgetMustNotBeLessThanZero = "Remaining budget must not be less than zero.";
        public const string AmountNotEqual = "Distributed Amount is not equal to Amount. The budget article cannot be released.";
        public const string NotInProjectBudget = "Requested Amount is not present in the Project Budget.";
        public const string ExceedsBudgetQuantity = "The Quantity exceeds the Budget.";

        public const string ProjectBudgetDeleteTitle = "Cannot delete Project Budget";
        public const string ProjectBudgetDeleteMessage = "One or more items are released.";
        public const string ProjectBudgetWasDeleteMessage = "GL Error: Budget Articles with non-zero Released amount cannot be deleted.";
        public const string ProjectBudgetCannotBeEdited = "GL Error: Budget Articles with non-zero Released amount cannot be edited.";
        public const string SelectVendorFirst = "Select Vendor first.";
        public const string BudgetPendingChangesTitle = "Pending Changes";
        public const string BudgetPendingChangesMessage = "The budget has pending changes. Review the budget and then save or discard your changes before selecting another budget.";
        public const string ReplenishPointPercentMustBeGreaterThanZero = "Replenish Point Percent must be greater than zero.";
        public const string ReplenishAmountMustBeGreaterThanZero = "Replenish Amount must be greater than zero.";
        public const string QuantityMustBeGreaterThanZero = "Net Quantity must be greater than zero.";
        public const string UnitCostMustBeGreaterThanZero = "Net Unit Cost must be greater than zero.";
        public const string ExceedsRequestAmount = "Unable to proceed, No enough Funds";
        public const string TransactionTypeNotEqual = "Transaction Type both in Summary and in Details are not the same.";
        public const string RequestTypeNotEqual = "Request Type both in Summary and in Details are not the same.";
        public const string RequestClassNotEqual = "Request Class ID both in Summary and in Details are not the same.";
        public const string ReplenishAmountMustBeEqualLowerThanInitialFund = "Replenishment Amount must be lesser or equal to fund amount";
        public const string LiquidationAmountIsGreaterThanOnHandAmount = "Liquidation Amount is GREATER THAN the Fund's On Hand amount";
        public const string TotalAmountIsGreaterThanOnHandAmount = "Total Amount is GREATER THAN the Fund's On Hand amount";

        public const string CashAdvanceEmployeeAllowed = "Maximum cash advance exceeded.";
        public const string CashAdvanceEmployeeExist = "Employee already exists.";
        public const string CashAdvanceReclass = "Reclassify";
        public const string CashAdvanceRemarks = "Remarks";
        public const string FundMinimumBalance = "Minimum Balance";
        public const string InvalidReqClass = "Invalid Request Class";
        public const string DuplicateInventory = "Duplicate Inventory Item is not allowed";
        public const string NotTheSameTypes = "Not all receipts have the same types.";
        public const string NotTheSameVendors = "Not all receipts have the same vendors.";
        public const string EnabledDocumentOverrid = "You still have pending Open CA, cannot proceed";
        public const string EnableRequestAccount = "Enable Request Account";
        public const string EnableRequestSubAccount = "Enable Request Subaccount";
        public const string MaxCAAmount = "Requested amount exceeds maximum allowable CA Amount";
        public const string MaxCAOpen = "Maximum allowable no. of Open CA is reached";
        public const string CASetupComplete = "Cash Advance Preferences not properly setup";
        public const string CAUserError = "You are not authorized to create a Cash Advance request. Please contact system administrator";
        public const string InvalidDocumentStatus = "Document status is invalid for processing.";
        public const string SystemGeneratedEstablishmentFund = "Establishment of {0} {1} {2}";
        public const string SystemGeneratedCloseFund = "Close Fund of {0} {1} {2}";
        public const string LineDescription = "Line Description";
        public const string ReplenishmentAmtLimit = "The request amount exceeds replenishment limit";
        public const string Reversed = "Reversed";
        public const string Type = "Type";
        public const string FinPeriodErr = "The {0} financial period is inactive.";

        public const string ReclassTranDesc = "Outstanding Liquidation";
        private const string _reclassInvoiceNbr = "Reclass: {0}";

        public const string AmountMustBeGreaterThanZero = "Amount must be greater than zero.";
        public static string ReclassInvoiceNbr(string refNbr) => PXLocalizer.LocalizeFormat(_reclassInvoiceNbr, refNbr);

        //RLCashAdvanceSetupApproval

        public const string AssignmentMapID = "Approval Map";
        public const string AssignmentNotificationID = "Pending Approval Notification";
        public const string IsActive = "Active";

        //RLFundTransactionSetupApproval
        public const string FundTransactionType = "Fund Transaction Type";

        //RLSetup 
        public const string CashadvanceRequestApproval = "Require Approval for Cash Advance";
        public const string FundTransactionRequestApproval = "Require Approval for Fund Transaction";
        public const string ReplenishmentRequestApproval = "Require Approval for Replenishment";
        public const string FundsApprovalSetup = "Require Approval for Funds";
        public const string FundsSetupApproval = "Funds Setup Approval";
        public const string MonthEndRequestApproval = "Require Approval for Month-End Transaction";
        public const string MaterialRequestRequestApproval = "Require Approval for Material Request";
        public const string AllowMultipleCA = "Allow multiple CA for single employee";
        public const string NoDaysToLiquidate = "No. of Days to Liquidate";
        public const string ReClassAccountID = "Reclassification Account";
        public const string ReClassSubID = "Reclassification Subaccount";
        public const string NoDaysToLiquidateFund = "No. of Days to Liquidate";
        public const string ReClassAccountIDFund = "Reclassification Account";
        public const string ReClassSubIDFund = "Default Subaccount";
        public const string AllowableCAAmt = "Restrict Allowable CA Amount";
        public const string AllowableOpenCA = "Restrict Allowable Open CA";
        public const string RestrictionValidation = "Restriction Validation";
        public const string RestrictCAEmployees = "Restrict Employees to CA";
        public const string AutoReleaseAP = "Auto Release AP Bill";
        public const string CopyAPNotes = "Copy Notes and Files to AP";
        public const string CopyECNotes = "Copy Notes and Files to Expense Claim";
        public const string RequireExtRef = "Require External Reference Nbr.";
        public const string ErrorDupExtRef = "Raise an Error on Duplicate Ext. Ref. Nbr.";
        public const string DuplicateProjectCombinationEntry = "Duplicate Project Combination Entry";
        public const string CouldNotDeleteTheReplenishmentExpenseReceiptsAlreadyAdded = "Could not delete the Replenishment, Expense Receipts already added";
        public const string LiquidationRule = "Days to Liquidate Rule";
        public const string StandardAllowableDays = "Standard Allowable Days";

#if Version25R1
        public const string AutoApplyPPT = "Auto Apply Liquidation Bill to CA Prepayment";
#else
        public const string AutoApplyPPT = "Auto Apply Prepayment to Invoice";
#endif
        public const string LiquidationDateBasedOnWorkCalendar = "Liquidation Date based on Work Calendar";

#if Version25R1
        public const string AutoApplyCredAdjPPT = "Auto Apply Reclass. Credit Adj. to CA Prepayment";
#else
        public const string AutoApplyCredAdjPPT = "Auto Apply Credit Adj. to Prepayment";
# endif
        public const string AllowableDays = "Allowable Days to Liquidate";
        public const string MaxCAAmt = "Maximum CA Amount";
        public const string NoOpenCA = "No. of Open CA Allowed";
        public const string CAUser = "Cash Advance User";
        public const string PCFAccount = "PCF Account";
        public const string PCFSubaccount = "PCF Subaccount";
        public const string RVFAccount = "RVF Account";
        public const string RVFSubaccount = "RVF Subaccount";
        public const string ClearingAccount = "Clearing Account";
        public const string ClearingSubaccount = "Clearing Subaccount";
        public const string MonthendTransactionAccount = "Month End Transaction Account";
        public const string MonthendTransactionSubaccount = "Month End Transaction Subaccount";
        public const string FundNumberingID = "Fund Numbering ID";
        public const string FundTransactionNumberingID = "Fund Transaction Numbering ID";
        public const string FundTransactionNbr = "Fund Transaction Ref Nbr";
        public const string ReplenishmentNumberingID = "Replenishment Numbering ID";
        public const string FundReturn = "Fund Return (Refund)";
        public const string ActualReturn = "Actual Return (Refund)";
        public const string AmountReceived = "Amount Received";
        public const string PleaseInputActualAmountReceivedFromRequestor = "Please input actual amount received from requestor";
        public const string AmountReleased = "Amount Released";
        public const string AmountReleaseCannotBeGreaterThanFundReturn = "Amount released cannot be greater than  Fund Return";
        public const string PleaseInputActualAmountReleasedToRequestor = "Please input actual amount released to requestor";
        public const string ReclassificationAmt = "Reclassification Amount";
        public const string ModuleType = "Module Type";
        public const string AllowManualReceipts = "Allow Manual Receipts";
        public const string PerDiem = "Per Diem";
        public const string RequireVendorDetails = "Require Vendor Details on Receipts Tab";
        public const string RequireExternalReferenceNbr = "Require External Reference Nbr.";
        public const string AllowSubmissionExcessCA = "Allow Submission of Excess CA using Vendor Refund";
        public const string ReturnExcessCashAdvance = "Return Excess Cash Advance";
        public const string RequireApprovalOfBillsAfterReleaseOfReplenishment = "Require Approval of Bills after Release of Replenishment";
        public const string WarningMessageForAllowManualReceipt = "Allow Manual Receipt cannot be enabled when Budget is enabled in Extended Financial Features";
        public const string WarningMessageForRequireVendorDetails = "Require Vendor Details cannot be disabled when Require Vendor Details setup is enabled in Time and Expenses Preference";
        public const string WarningMessageForEPAutomaticReleaseAP = "Automatically Release AP Documents cannot be enabled when Require Approval on RFP Bill or Require Approval on Liquidation Bill is enabled";
        public const string WarningMessageForEPRequireRFPBillApproval = "Require Approval on RFP Bill cannot be enabled when Automatically Release AP Documents is enabled";
        public const string WarningMessageForCARequireLiquidationBillApproval = "Require Approval on Liquidation Bill cannot be enabled when Automatically Release AP Documents is enabled on Time and Expenses Preference";
        public const string WarningMsgForRequireRefNbr = "Require Ext Reference Nbr cannot be disabled when Require Ext Reference Nbr is enabled in Time and Expenses Preference.";
        public const string AutoReleaseAPIsEnabledInTimeAndExpensesPreference = "Auto-Release AP is enabled in Time & Expenses Preference";
        public const string AutoReleaseAPIsEnabled = "Auto-Release AP is enabled";
        public const string BillRequireApprovalIsEnabled = "Bill Require Approval is Enabled";
        public const string AllReceiptsMustOpenStatus = "FT Error: All receipts included in this transaction must be in the Open status.";
        public const string UnableToProceedBalanceNegative = "Error: Unable to proceed. The Fund Transaction balance amount is negative";
        public const string NoOfDaysToLiquidateShouldBeGreaterThanZero = "No. of Days to Liquidate should be greater than Zero.";
        public const string ErAlreadyAddedInReplenishment = "Unable to cancel transaction because some of the receipts under this document have already been added in replenishment.";
        public const string BudgetIsAlreadyEnabledInTheRequestClass = "Budget is already enabled in the Request Class.";
        public const string BudgetIsAlreadyEnabledInTheExtendedFinancialFeatures = "Budget is already enabled in the Extended Financial Features.";

        //RLSetupEmployeeCA 
        public const string EmployeeID = "Employee";
        public const string AllowedCA = "No. of CA Allowed";
        public const string AdditionalCA = "Additional No. of CA";
        public const string EndDate = "End Date";
        public const string Approval = "Approval";

        //RLBudget 
        public const string AccountID = "Account";
        public const string SubID = "Subaccount";
        public const string AccountGroup = "Account Group";
        public const string CuryID = "Currency";
        public const string DocAmt = "Document Amount";
        public const string RequestAmt = "Request Amount";
        public const string BudgetAmt = "Remaining Budget Amount";
        public const string SpentAmt = "Spent Amount";
        public const string ApprovedAmt = "Approved Amount";
        public const string UnapprovedAmt = "Unapproved Amount";
        public const string ReturnAmt = "Returns";

        //RLPBudget 
        public const string ProjectID = "Project";
        public const string ProjectTaskID = "Project Task";
        public const string CostCodeID = "Cost Code";

        //RLCashAdvance 
        public const string CashAdvanceNbr = "Reference Nbr";
        public const string Status = "Status";
        public const string Hold = "Hold";
        public const string Date = "Date";
        public const string FinPeriodID = "Post Period";
        public const string Descr = "Description";
        public const string RequestedByID = "Requested By";
        public const string Positions = "Position";
        public const string DepartmentID = "Department";
        public const string InvoiceRefNbr = "Invoice Reference Nbr.";
        public const string Branch = "Branch";
        public const string BranchID = "BranchID";
        public const string OwnerID = "Owner";
        public const string WorkgroupID = "Workgroup";
        public const string Approved = "Approved";
        public const string DateOfUse = "Expected Date of Use";
        public const string RequestedAmount = "Total Amount";
        public const string CaAmount = "CA Amount";
        public const string ActualSpentAmount = "Liquidation";
        public const string ChangeAmount = "Balance";
        public const string ReclassifyDate = "Reclassify Date";
        public const string Reclassified = "Reclassified";
        public const string ReclassifiedInvoiceRefNbr = "Reclassified Invoice Reference Nbr.";
        public const string RequestApproval = "Request Approval";
        public const string ApprovalWorkgroupID = "Approval Workgroup ID";
        public const string ApprovalOwnerID = "Approver";
        public const string EmployeeName = "Employee Name";
        public const string CannotBeZero = "Cash Advance cannot be zero.";
        public const string LiquidationDate = "Liquidation Date";
        public const string InitialLiquidationDate = "Initial Liquidation Date";
        public const string ReqClassID = "Request Class ID";
        public const string ViewBill = "View Bill";
        public const string ViewCheck = "View Check";
        public const string ViewPrepayment = "View Prepayment";
        public const string CACancel = "Cancel";
        public const string Refund = "Refund";
        public const string DescCharExceeds = "Cash advance Description field must not be greater than 194 characters long.";
        public const string CAPrepaymentBillDesc = "Cash Advance Request {0} {1}";
        public const string IsReady = "Ready";

        //RLCashAdvanceReceiptDetail 
        public const string RefNbr = "Reference Nbr";
        public const string VendorID = "Vendor ID ";
        public const string TaxZone = "Tax Zone";
        public const string AtcCode = "ATC Code";
        public const string NetAmt = "Net Amount";
        public const string ExpenseReceiptRefNbr = "Expense Receipt Reference";
        public const string NetQty = "Net Qty.";
        public const string NetUnitCost = "Net Unit Cost";
        public const string LiqRef = "Liquidation Ref";
        public const string FailedLiquidation = "Failed to Generate Liquidation Ref.";
        public const string ReleasedExpenseClaimCannotBeDeleted = "Released expense claim, which is a supporting document, cannot be deleted.";
        public const string ReceiptDateNotLessThanCADocumentDate = "Receipt date should not be less than Document date.";
        public const string ReceiptDateNotGreaterThanLiqDate = "Receipt Date should not be greater than Document Liquidation Date.";


        //RLCashAdvanceRequestDetail 
        public const string CashAdvanceNbrRqstID = "Request ID";
        public const string InventoryID = "Inventory ID";
        public const string ReclassificationItem = "Reclassification Item";
        public const string Qty = "Quantity";
        public const string UnitCost = "Unit Cost";
        public const string Amount = "Amount";
        public const string Subaccount = "Subaccount";
        public const string Selected = "Selected";
        public const string Balance = "Balance";
        public const string RunningQty = "Qty";
        public const string RequestAmountMustBeWithinLimitAmt = "Request amount must be within limit amount set.";

        //RLFeatures 
        public const string BudgetModules = "Enable Budget Modules";
        public const string BudgetFeatureSet = "Budget Feature Set";
        public const string BudgetDocumentAmount = "Document Amount";
        public const string BudgetModulesRqstAmt = "Request Amount";
        public const string BudgetSpentAmount = "Amount Spent";
        public const string BudgetBudgetAmount = "Remaining Budget Amount";
        public const string BudgetApprovedAmount = "Approved Amount";
        public const string BudgetUnapprovedAmount = "Unapproved Amount";
        public const string BudgetReturnAmount = "Returns";
        public const string BudgetPOAmount = "PO Amount (Request)";
        public const string BudgetDocumentAmountLabel = "Document Amount";
        public const string BudgetBudgetAmountLabel = "Budget Amount";
        public const string BudgetSpentAmountLabel = "Amount Spent";
        public const string BudgetApprovedAmountLabel = "Approved Amount";
        public const string BudgetUnapprovedAmountLabel = "Unapproved Amount";
        public const string BudgetReturnAmountLabel = "Returns";
        public const string BudgetPOAmountLabel = "PO Amount (Request)";
        public const string ProjectBudgetModules = "Enable Project Budget Modules";
        public const string ProjectBudgetFeatureSet = "Project Budget Feature Set";
        public const string ProjectBudgetDocumentAmount = "Document Amount";
        public const string ProjectBudgetRequestAmount = "Request Amount";
        public const string ProjectBudgetBudgetAmount = "Remaining Project Budget Amount";
        public const string ProjectBudgetSpentAmount = "Amount Spent";
        public const string ProjectBudgetApprovedAmount = "Approved Amount";
        public const string ProjectBudgetUnapprovedAmount = "Unapproved Amount";
        public const string ProjectBudgetReturnAmount = "Returns";
        public const string ProjectBudgetPOAmount = "PO Amount (Request)";
        public const string ProjectBudgetDocumentAmountLabel = "Document Amount";
        public const string ProjectBudgetModulesRqstAmt = "Request Amount";
        public const string ProjectBudgetBudgetAmountLabel = "Budget Amount";
        public const string ProjectBudgetSpentAmountLabel = "Amount Spent";
        public const string ProjectBudgetApprovedAmountLabel = "Approved Amount";
        public const string ProjectBudgetUnapprovedAmountLabel = "Unapproved Amount";
        public const string ProjectBudgetReturnAmountLabel = "Returns";
        public const string ProjectBudgetPOAmountLabel = "PO Amount (Request)";
        public const string TransferAssetCustodian = "Custodian";
        public const string TransferAssetBuilding = "Building";
        public const string TransferAssetFloor = "Floor";
        public const string TransferAssetRoom = "Room";
        public const string LimitValidation = "Limit when budget validation occurs. Cash Advance type only";
        public const string LimitValidationAmt = "Limit Validation Amount";

        //RLFund 
        public const string FundID = "Fund ID";
        public const string FundType = "Fund Type";
        public const string Active = "Active";
        public const string CustodianID = "Custodian";
        public const string CustodianName = "Custodian Name";
        public const string PayeeID = "Payee";
        public const string PayeeName = "Payee Name";
        public const string ReplenishPointPercent = "Replenish Point";
        public const string FundTransactionPointPercent = "Limitation Point";
        public const string ReplenishmentAmount = "Replenishment Amount";
        public const string FundTransactionAmount = "Limitation Amount";
        public const string ReplenishmentRestrictions = "Replenishment Restrictions";
        public const string FundTransactionRestrictions = "Limitation Restrictions";
        public const string Subid = "Subaccount";
        public const string CreditAccountID = "Credit Account";
        public const string Closed = "Closed";
        public const string OnHandBalanceAmount = "On Hand";
        public const string ReceiptBalanceAmount = "Receipts";
        public const string UnliquidatedCashAdvanceBalanceAmount = "Unliquidated Cash Advance";
        public const string MinimumBalanceAmount = "Minimum Balance";
        public const string DocumentDate = "Document Date";
        public const string InitialFund = "Initial Fund";
        public const string InitialBudget = "Initial Budget";
        public const string EstablishmentAPRef = "Establishment AP Ref.";
        public const string CloseFundAPRef = "Close Fund AP Ref.";
        public const string ExpenseBatchNbr = "Expense Batch Nbr.";
        public const string ReplenishmentLimit = "Replenishment Limit";
        public const string FundTransactionLimit = "Fund Transaction Limit";
        public const string Liquidated = "Liquidated";
        public const string Unliquidated = "Unliquidated";
        public const string OnReplenishmentAmt = "On Replenishment";
        public const string ClaimAmount = "Claim Amount";
        public const string WithholdingTax = "Withholding Tax";
        public const string VATAmount = "VAT Amount";
        public const string OverrideTax = "Override Default Tax";
        public const string Establishment = "Establishment";
        public const string FundRequest = "Fund Request";
        public const string Reclassification = "Reclassification";
        public const string FundReimbursment = "Fund Reimbursement";
        public const string Replenishment = "Replenishment";
        public const string CheckAmount = "Checks & Payments Amount.";
        public const string Source = "Source";
        public const string Liquidate = "Liquidate";
        public const string Unliquidate = "Unliquidate";
        public const string UnliquidatedAmtMustBeZero = "Unliquidated Amount must be zero.";
        public const string NoReversalEntry = "No reversal entry has been posted for the month-end transaction.";
        public const string HasOpenReplenishment = "There is at least one Open Replenishment transaction.";
        public const string DecreaseAmtIsGreaterThanBalance = "The amount of decrease in the fund should not be greater than the On Hand amount.";
        public const string ReplenishmentCannotBeRelease = "Replenishment cannot be released. Fund Transaction  of Expense Receipt {0} is still open.";
        public const string CurrencyAccountError = "The currency assigned to the denominated GL account {0} is different from the transaction currency.";

        //RLFundTransaction
        public const string CashAdvanceStatus = "CA Status";
        public const string ChangeAmnt = "Change";
        public const string ReceivedAmount = "Amount Received";
        public const string ReleasedAmount = "Amount Released";
        public const string NoFund = "Reimbursement w/out Fund";
        public const string TotalReceipt = "Total Receipts";
        public const string TotalApprovalAmount = "Approval Amount";
        public const string ShowBudgetValidation = "Show Budget Validation";
        public const string OverbudgetWarning = "The request amount exceeds the budget amount.";

        //RLFundTransactionDetail 
        public const string FundTransactionRefNbr = "Request ID";
        public const string Particulars = "Particulars";
        public const string UnitRecordID = "UOM";
        public const string ProjectTaskid = "Project Task";
        public const string Description = "Description";

        //RLFundTransactionReceiptDetail 
        public const string TaxZoneID = "Tax Zone";
        public const string ReplenishmentRefNbr = "Replenishment Reference";
        public const string VendorNameEmpty = "Error: Vendor Name cannot be empty";
        public const string VendorAddressEmpty = "Error: Vendor Address cannot be empty";
        public const string VendorTinEmpty = "Error: Vendor Address cannot be empty";
        public const string WHT = "WHT";
        public const string NoAccountGroup = "Account {0} is not mapped to any project account group. Either map the account or select a non-project code.";

        //RLMaterialRequest
        public const string IssueRefNbr = "Issue Ref Nbr.";
        public const string RequestRefNbr = "Request Ref Nbr.";

        //RLMaterialRequestDetail 
        public const string UOM = "UOM";
        public const string LocationID = "Location";

        //RLMonthEndDetail 
        public const string ReceiptNbr = "Receipt Number";
        public const string ExpenseItm = "Expense Item";
        public const string ContractID = "Project/Contract";
        public const string LedgerID = "Ledger";
        public const string FinYear = "Financial Year";

        //RLProjectBudgetHistory 
        public const string CuryAmt = "Currency Amount";

        //RLProjectBudgetLine 
        public const string FinPeriodid = "Fin Period";
        public const string ReleasedAmnt = "Released Amount";
        public const string Released = "Released";

        //RLProjectBudgetLineSummary
        public const string DistributedAmount = "Distributed Amount";
        public const string FinPeriod01 = "Jan";
        public const string FinPeriod02 = "Feb";
        public const string FinPeriod03 = "Mar";
        public const string FinPeriod04 = "Apr";
        public const string FinPeriod05 = "May";
        public const string FinPeriod06 = "Jun";
        public const string FinPeriod07 = "Jul";
        public const string FinPeriod08 = "Aug";
        public const string FinPeriod09 = "Sep";
        public const string FinPeriod10 = "Oct";
        public const string FinPeriod11 = "Nov";
        public const string FinPeriod12 = "Dec";

        //RLReplenishment
        public const string ReplenishmentNbr = "Reference Nbr";
        public const string TaxID = "Tax ID";
        public const string TaxType = "Tax Type";
        public const string TaxRate = "Tax Rate";
        public const string TaxableAmount = "Taxable Amount";
        public const string TaxAmount = "Tax Amount";
        public const string ReplenishmentRefNbrDetail = "Replenishment Ref Nbr.";


        //RLReplenishmentDetail 
        public const string ReplenishmentID = "Replenishment ID";
        public const string ExpenseReceiptNbr = "Receipt Nbr.";

        //APInvoiceExtension 
        public const string UsrRLInvoiceNbr = "Invoice Nbr";
        public const string UsrRLInvoiceDate = "Invoice Date";
        public const string UsrRLAmountToWords = "Amount to Word";
        public const string UsrRLPaymentRefNbr = "Payment Nbr.";

        //APSetupExtension
        public const string UsrRLCashAdvanceNumberingID = "Cash Advance Numbering Sequence";
        public const string UsrATPTEFMLiqNumberingID = "Liquidation Numbering Sequence";
        public const string UsrRLBudgetLedgerID = "Budget Ledger";
        public const string UsrRLBudgetCalculation = "Budget Calculation";
        public const string UsrRLDefaultReqClassCD = "Default Request Class";
        public const string UsrRLBudgetValidation = "Budget Validation";
        public const string UsrRLExpenseAccountDefault = "Use Expense Account From";
        public const string UsrRLExpenseSubMask = "Combine Expense Sub. From";
        public const string UsrRLExpenseAcctID = "Expense Account";
        public const string UsrRLExpenseSubID = "Expense Sub";

        //APTranExtension
        public const string UsrRLDepartmentCD = "Department";
        public const string UsrVendID = "Vendor ID";
        public const string UsrVendName = "Vendor Name";
        public const string UsrVendTin = "TIN";
        public const string UsrAddress = "Address";
        public const string UsrDefaultATC = "ATC Code";

        //CASetupExtension 
        public const string UsrRLFundNumberingID = "Fund Management Numbering Sequence";
        public const string UsrRLReplishmentID = "Replenishment Numbering Sequence";
        public const string UsrRLMonthEndNumberingID = "Month-End Numbering ID";
        public const string UsrRLFundAcctName = "Account Name";
        public const string FilteredView = "Filtered View";

#if Version25R1
        public const string RequireApprovalOnReplenishmentBill = "Hold Generated Replenishment Bill";
#else
        public const string RequireApprovalOnReplenishmentBill = "Require Approval on Replenishment Bill";
#endif


#if Version25R1
        public const string RequireApprovalOnCABill = "Hold Generated Cash Advance Prepayment";
#else
        public const string RequireApprovalOnCABill = "Require Approval on Cash Advance Bill";
#endif

#if Version25R1
        public const string RequireApprovalOnFundEstablishment = "Hold Generated Fund Establishment Bill";
#else
        public const string RequireApprovalOnFundEstablishment = "Require Approval on Fund Establishment";
#endif


#if Version25R1
        public const string RequireApprovalOnLiquidationBill = "Hold Generated Liquidation Bill";
#else
        public const string RequireApprovalOnLiquidationBill = "Require Approval on Liquidation Bill";
#endif


#if Version25R1
        public const string RequireApprovalOnFundIncreaseCredAdj = "Hold Generated Credit Adjustment for Fund Increase";
#else
        public const string RequireApprovalOnFundIncreaseCredAdj = "Require Approval on Fund Increase Credit Adj.";
#endif

#if Version25R1
        public const string RequireApprovalOnFundDecreaseDebAdj = "Hold Generated Debit Adjustment for Fund Decrease";
#else
        public const string RequireApprovalOnFundDecreaseDebAdj = "Require Approval on Fund Decrease Debit Adj.";
#endif

#if Version25R1
        public const string RequireApprovalOnRFPBill = "Hold Generated RFP Bill";
#else
        public const string RequireApprovalOnRFPBill = "Require Approval on RFP Bill";
#endif
        public const string UseTaxZoneInExpenseClaimFinancialTabForRFP = "Use Tax Zone in Expense Claim Financial Tab for RFP";

        //CATransferExtension 
        public const string UsrRLCheckNbr = "Checks & Payments Ref. Nbr.";
        public const string UsrRLDestAccountName = "Account Name";

        //DisposalFilterExtension 
        public const string UsrRLItemClassID = "Item Class";
        public const string UsrRLItemDescr = "Item Description";
        public const string UsrRLCustomerID = "Customer ID";

        //EPExpenseClaimDetailsExtension
        public const string FromPhilTax = "Vendor ID";
        public const string ORCRRefNbr = "OR/CR Ref#";
        public const string RequestedItemNotTheSame = "The requested item is not the same with the actual item submitted for liquidation";

        //EPExpenseClaimExtension
        public const string RequestedBy = "Requested by";
        public const string APAccountError = "Employee Financial Settings: AP Account must not be empty.";
        public const string APSubError = "Employee Financial Settings: AP Sub. must not be empty.";
        public const string APAccountAndAPSubError = "Employee Financial Settings: Must Contain AP Account and AP Sub.";
        public const string TermsIsEmpty = "Employee Financial Settings: Terms must not be empty.";
        public const string TaxZoneIsEmpty = "Employee Financial Settings: Tax Zone must not be empty.";
        public const string VendorTermsIsEmpty = "Vendor Financial Settings: Terms must not be empty.";

        //FADetailExtension
        public const string UsrRLPrevDepreciateFromDate = "Orig. Placed-in-Service Date";

        //FASetupExtension
        public const string UsrRLInventoryDisposalMethodID = "FA to Inventory";
        public const string UsrRLCustomerDisposalMethodID = "FA to Customer";
        public const string UsrRLExpenseAccountID = "Expense Account";
        public const string UsrRLExpenseSubid = "Expense Subaccount";

        //FATranExtension 
        public const string UsrRLBuildingID = "Building";
        public const string UsrRLFloor = "Floor";
        public const string UsrRLRoom = "Room";

        //FixedAssetExtension 
        public const string UsrRLPrevUsefulLife = "Orig. Useful Life, Years";
        public const string UsrRLUnitID = "Unit ID";

        //InSetupExtension
        public const string UsrRLMaterialRequestNumberingID = "Material Request Numbering Sequence";
        public const string UsrRLMaterialBudgetValidation = "Material Issuance on Quantity";
        public const string UsrRLMaterialBudgetValidationType = "Material Issuance Validation";


        //InventoryItemExtension 
        public const string UsrDefaultatc = "Default ATC";

        //POLineExtension 
        public const string UsrRLWarranty = "Warranty Date";

        //POReceiptExtension
        public const string UsrRLDRNbr = "DR Number";
        public const string UsrRLInvoiceNumnbr = "Invoice Number";
        public const string UsrRLPackageTotal = "Total Package";
        public const string UsrRLFreighNbr = "Freight Bill Nbr";
        public const string UsrRLDescr = "Remarks";
        public const string UsrRLComplete = "Complete";
        public const string UsrRLQuality = "Good Quality";
        public const string UsrRLTimeLiness = "On - Time";
        public const string UsrRLCustomerService = "Satisfactory";
        public const string UsrRLPOAmount = "PO Amount";

        //RQRequestClassExtension
        public const string UsrRLProjectBudgetValidation = "Budget Validation";
        public const string UsrRLProjectBudgetCalculation = "Budget Calculation";
        public const string UsrRLProjectBudgetCheck = "Checks Project Budget";

        //RQRequestLineExtension
        public const string UsrRLProjectID = "Project";
        public const string UsrRLProjectTaskID = "Task";

        //RQRequisitionLineExtension
        public const string UsrRLProjectTaskid = "Sub Job";

        //TransferFilterExtension
        public const string EmployeeIDFrom = "Custodian";
        public const string BuildingIDFrom = "Building";
        public const string FloorFrom = "Floor";
        public const string RoomFrom = "Room";
        public const string EmployeeIDTo = "Custodian";
        public const string BuildingIDTo = "Building";
        public const string FloorTo = "Floor";
        public const string RoomTo = "Room";

        //RLPMCostBudgetExtension
        public const string UsrRLVarianceQty = "Variance QTY";

        //RLINTranExtension
        public const string UsrRLRemainingQty = "Remaining Budgeted QTY";

        //Graphs 

        //APInvoiceEntryExtension
        public const string PrintAPVoucher = "Print AP Voucher";
        public const string ImportInvoice = "Import";
        public const string CreateImportCAPayment = "CA Payment";
        public const string BudgetFinPeriodID = "Fin. Period";
        public const string Year = "Year";

        //APPaymentEntryExtension 
        public const string PrintCVoucher = "Print Check Voucher";
        public const string DocumentBillCannotBeReleased = "AP Error: Document '{0}' is not paid in full. Document will not be released";


        // APInvoiceEntryExtension 
        public const string ViewFundRefNbr = "View Fund Ref Nbr";
        public const string ViewCashAdvance = "View Cash Advance";

        //AssetMaintExtension
        public const string Nuly = "New Useful Life, Years";
        public const string NPiSD = "New Placed-in-Service Date";

        //CAReconEntryExtension
        public const string Report = "Reports";
        public const string BankReconReport = "Bank Reconciliation";

        //CashTransferEntryExtension
        public const string ReportMenu = "Reports";
        public const string CheckVendorReport = "Check and Check Voucher";
        public const string FundTransferReport = "Fund Transfer Form";
        public const string PrintCheck = "Print Check";
        public const string FundTransactionReport = "Fund Transaction Form";

        //JournalEntryExtension
        public const string PrintedForms = "Printed Form";
        public const string JournalVoucher = "Journal Voucher";

        //POOrderEntryExtension
        public const string PurchaseOrderReport = "Purchase Order Report";

        //RQRequestEntryExtension
        public const string RemainingBudgetAmt = "Remaining Budget Amount";
        public const string ApprovedRqstAmt = "Approved Request Amount";
        public const string UnapprovedRqstAmt = "Unapproved Request Amount";

        //CashAdvanceEntry 
        public const string Action = "Actions";
        public const string SubAct = "Sub Account";
        public const string Vendor = "Vendor";
        public const string Address = "Address";
        public const string TIN = "TIN";
        public const string CreateAPBill = "Create AP Bill";
        public const string CreateAPBillImport = "Create AP Bill (Import)";
        public const string LoadRequest = "Load";
        public const string SubmitReceipt = "Add";
        public const string SubmitReceipts = "Submit Receipts";
        public const string CancelSubmitReceipt = "Close";
        public const string OpenTransaction = "Open Transaction";
        public const string OpenReclassReceipt = "Open Reclass Receipt";
        public const string PrintCashAdvance = " Print Cash Advance Request";
        public const string CannotDeleteReceipt = "Receipt already submitted. Cannot be deleted.";

        //CashAdvanceReclassProcess 
        public const string Extend = "Extend";
        public const string Amt = "Amount";

        //FundMaint 
        public const string IncreaseFund = "Increase Fund";
        public const string DecreaseFund = "Decrease Fund";
        public const string CloseFund = "Close Fund";
        public const string ExpenseReceipt = "Expense Receipt";
        public const string VoidedCheck = "Voided Check";
        public const string MonthEnd = "Month-End Transaction";
        public const string IncreasedBy = "Increased By";
        public const string DecreaseBy = "Decreased By";
        public const string PrintFundEstablishmentForm = "Print Fund Establishment";

        //FundTransactionEntry 
        public const string ReleaseCash = "Release Cash";
        public const string CashFundAdvanceRequestForm = "Cash Fund Advance Request Form";
        public const string DocumentDetailsCannotBeEmpty = "Document Details Cannot Be Empty.";
        public const string DateOfUseCannotBeEmpty = "'Expected Date of Use' cannot be empty for Cash Advance transactions.";
        public const string ReceiptDetailsCannotBeEmpty = "Receipt Details Cannot Be Empty.";
        public const string TotalAmountCannotBeZero = "Total Amount Cannot Be Zero.";
        public const string LiquidationAmountCannotBeZero = "Liquidation Amount Cannot Be Zero.";

        //JournalTransationlnq
        public const string TotalDebitAmt = "Total Debit";
        public const string TotalCreditAmt = "Total Credit";
        public const string CancelRequest = "Cancel Request";
        public const string CreateInventoryIssue = "Create Inventory Issue";
        public const string CreatePurchaseRequest = "Create Purchase Request";
        public const string OpenIssue = "Open Issue";
        public const string OpenRequest = "Open Request";

        //MONTHEND
        public const string ShowSubmitReceipt = "Add Receipts";
        public const string AutomaticallyReleaseMonthEndJournalTransaction = "Automatically Release Month-End Journal Transaction";
        public const string MonthEndShouldOnlyBeCreatedDuringTheEndOfTheMonth = "Month-end Transaction should be created and processed during the end of the month.";
        public const string ValidateAmountReceivedAndReleasedUponLiquidation = "Validate Amount Received And Released Upon Liquidation";
        public const string EnableFundTransactionLimit = "Enable Fund Transaction Limit";
        public const string EnableFundRequestReclassification = "Enable Fund Request Reclassification";
        public const string NoOfDaysToLiquidate = "No. of Days to Liquidate";
        public const string MonthEndDetailsCannotBeEmpty = "Month-End Details cannot be empty";

        //ReplenishmentEntry 
        public const string Submit = "Submit";
        public const string FundReplenishmentForm = "Fund Replenishment";

        //RLReplenishmentReportSettings
        public const string ColumnOrder = "Column Order";
        public const string ItemClass = "Item Class";

        //RLProjectBudgetEntry
        public const string CompareLedgerID = "Compare to Ledger";
        public const string CompareFinYear = "Compare to Year";
        public const string CompareProjectID = "Compare to Project";
        public const string Method = "Distribution Method";
        public const string ApplyToAll = "Apply to All Articles in This Node";
        public const string ApplyToSubGroups = "Apply to Subarticles";
        public const string Multiplier = "Multiplier (%)";

        public const string APScreenID = "AP301000";
        public const string Module = "Module";

        //Request for Payment
        public const string RFPRequestReference = "RFP Ref Nbr";
        public const string RequestForPayment = "Request for Payment";
        public const string RFPRefNbrCannotBeBlank = "RFP Ref Nbr cannot be empty for Request for Payment transactions.";

        //Notification
        public const string NotificationSetup = "Default Notification Setup";


        //Cache Names
        public const string ATPTEFM2023R2Enhancements = "CFM 2023-R2 Enhancements";
        public const string ATPTEFMSetup = "EFM Setup";
        public const string ATPTEFMSetupEmployeeCA = "EFM Setup Employee";
        public const string ATPTEFMSetupApproval = "Cash Advance Setup Approval";
        public const string ATPTEFMFeatures = "Extended Financials Feature Setup";
        public const string ATPTEFMEnableDisable = "EFM Enable/Disable Features";
        public const string ATPTEFMCashAdvance = "Cash Advance";
        public const string ATPTEFMCARequestDetail = "Cash Advance Request Detail";
        public const string ATPTEFMCAReceiptDetail = "Cash Advance Receipt Detail";
        public const string ATPTEFMBudget = "Budget";
        public const string ATPTEFMPBudget = "Project Budget";
        public const string ATPTEFMBudgetHistory = "Budget History";
        public const string ATPTEFMProjectBudgetHistory = "Project Budget History";
        public const string ATPTEFMProjectBudgetLineSummary = "Project Budget Line Summary";
        public const string ATPTEFMReplenishment = "Replenishment";
        public const string ATPTEFMProjectBudgetLine = "Project Budget Line";
        public const string ATPTEFMReqClass = "Employee Request Class";
        public const string ATPTEFMReqClassItems = "Employee Request Class Items";
        public const string ATPTEFMFund = "Fund";
        public const string ATPTEFMFundTransaction = "Fund Transaction";
        public const string ATPTEFMFundTransactionDetail = "Fund Transaction Detail";
        public const string ATPTEFMFundTransactionReceiptDetail = "Fund Transaction Receipt Detail";
        public const string ATPTEFMFundTransactionReclassficationReceiptDetail = "Fund Transaction Reclassfication Receipt Detail";
        public const string ATPTEEFMReplinesmentDetail = "Replineshment Detail";
        public const string ATPTEFMReplenishmentSetupApproval = "Replineshment Detail";
        public const string ATPTEFMReplenishmentTaxDetail = "Replineshment Tax Detail";
        public const string ATPTEFMMonthEnd = "Month-End Transaction";
        public const string ATPTEFMMonthEndSetupApproval = "Month-End Setup Approval";
        public const string ATPTEFMFundTransactionPreference = "Fund Management Preferences";
        public const string ATPTEFM2023EnhancementsPreference = "CFM 2023 R2 Enhancements";
        public const string ATPTEFMReplenishmentReportSettings = "Replenishment Report Settings";
        public const string ActivateCashAdvance = "Activate Cash Advance";
        public const string ActivateFunds = "Activate Funds";
        public const string FundTransactionImportMode = "Fund Transaction Import Mode";
        public const string IsImported = "Is Imported";
        public const string InactiveTaxCategory = "Tax Category is Inactive";
        public const string ImportFundFirst = "In order to proceed with this transaction, you need to migrate fund data";
        public const string ExpenseReceiptIsAlreadyAddedInReplenishment = "The Expense Receipt cannot be cancelled because other receipts associated with its Fund Transaction have already been included in a Replenishment transaction.";
        public const string RestrictCustodianByBranch = "Restrict Custodian by Branch";

        //CashEntry
        public const string FundTransfer = "Funds Transfers";
        public const string TransferNbr = "TransferNbr";
        public const string SCREENID_FT = "RL641100";
        public const string Preview = "Preview";

        //CashFund Data Fix
        public const string InvalidPassword = "Invalid Password";
        public const string ClearingAccountNotConfigured = "Fund Management Preferences clearing account or subaccount is not configured.";

        //CFM Error Messages
        public const string DocumentDetailsShouldNotBeEmpty = "Document Details Should Not Be Empty";
        public const string FundTransactionGreaterThanFundLimit = "Fund transaction amount is greater than the fund’s limitation amount.";
        public const string AmountReceivedCannotBeGreaterThanFundReturn = "Amount Received cannot be greater than Fund Return (Refund).";
        public const string AmountReleasedCannotBeGreaterThanFundReturn = "Amount Released cannot be greater than Fund Return (Refund).";
        public const string BalanceShouldNotBeNegative = "The Balance should not be a Negative Amount";
        public const string DetailsCannotBeEmpty = "Details Cannot Be Empty";
        public const string ClaimTotalIsZero = "Claim Total Is Zero";
        public const string FundTransactionLimitAmtMustBeGreaterThanZero = "Fund Transaction limit amount must be greater than zero";
        public const string FundTransactionLimitPercentMustBeGreaterThanZero = "Fund Transaction limit percentage must be greater than zero";
        public const string NoLinesMatchTax = "The {0} tax cannot be applied to the document because there are no document lines whose tax category contains the {0} tax.";
        public const string EmployeeNotFound = "Employee not found.";
        public const string VendorNotFound = "Vendor not found.";
        public const string LocationNotFound = "Location not found";
        public const string EmployeeInactive = "An error occured during processing of the field Requested by value {0} AP Error: The Requestor specified is inactive ";
        public const string VendorInactive = "An error occured during processing of the field Vendor value {0} AP Error: The Vendor specified is inactive ";
        public const string CurrencyMismatch = "Currency mismatch: Document currency {0} does not match Cash Account currency {1}";
        public const string PaymentCurrencyMismatch = "Currency mismatch: Payment currency {0} does not match Fund currency {1} for Bill {2}";
        public const string ReplenishmentBillMirrorReplenishmentTaxesValidation = "Cannot find related Expense Receipt Taxes for {0}-{1}: Line: {2}. Expense Receipt RefNbr used: {3}. Replenishment RefNbr used: {4}";
        public const string ReceiptCannotBeDeletedLinkedBill = "The receipt cannot be deleted because it is already linked to a created bill.";
        public const string DetailTotalShouldBeEqualToDocumentTotal = "Detail Total should be equal to {0} amount";
        public const string CannotCloseFundWithPendingTransactions = "Cannot close fund. There is at least one pending transaction: {0}";
        public const string SetupStillHasEnabledCheckboxesWhichAreAlreadyRemoved = "Setup still has enabled checkboxes which are already removed.";
        public const string CouldNotCancelReplenishmentBillGenerated = "Could not cancel the replenishment; Bill already generated";
        public const string BudgetCAReceiptGreaterThanRequest = "Budget validation error: Receipt amount is greater than Request amount.";
        public const string CannotBeFoundInSystem = "'{0}' cannot be found in the system";
        public const string DuplicateApprovalMapType = "Duplicate Approval Map for the same Fund Type is not allowed.";

        //Import by Scenario Screen ID
        public const string FromImportScenarioScreenID = "SM206036";


        //CFM Screen ID
        public const string CashAdvanceScreenID = "AT.PT.31.03";


        [PXLocalizable]
        public class ATPTEFMLocalizedParameterString
        {
            public const string _receiptAmtGreaterThanRequestAmt = "Receipt amount of {0}-{1} should not be greater than Request amount";

            public const string _pBudgetReceiptAmtGreaterThanRequestAmt = "Receipt amount of {0}-{1}-{2}-{3} should not be greater than Request amount";
        }
        public class MessagesWithParameters
        {
            public static string ReceiptAmtGreaterThanRequestAmt(string acc, string subAcc) =>
                PXLocalizer.LocalizeFormat(ATPTEFMLocalizedParameterString._receiptAmtGreaterThanRequestAmt, acc, subAcc);

            public static string PBudgetReceiptAmtGreaterThanRequestAmt(string project, string task, string costcode, string accgroup) =>
                PXLocalizer.LocalizeFormat(ATPTEFMLocalizedParameterString._pBudgetReceiptAmtGreaterThanRequestAmt, project, task, costcode, accgroup);
        }

        [PXLocalizable]
        public class ATPTFieldExtensions
        {
            public const string LiquidationRFPNbr = "Liquidation / RFP Ref Nbr.";

        }

        public const string ErrorGeneratingCSVFile = "Error generating CSV file: {0}";
        public const string FTSetupGLAccountValidation = "Use Expense Account From under GL Accounts tab in Fund Management Preferences setup is empty.";
        public const string VendorAddress = "Vendor Address";
        public const string VendorTIN = "Vendor TIN";

        // Setup/Feature diagnostics
        public const string CashFundManagementFeatureDisabled = "Cash Fund Management is disabled in Cash Advance Preferences.";

        // Data Fix - Stuck CA Refund Fixer
        public const string CARefNbrNotFound = "Cash Advance with RefNbr '{0}' not found.";
        public const string CANoVendorRefund = "Cash Advance '{0}' does not have a Vendor Refund associated.";
        public const string CANoPpmRefNbr = "Cash Advance '{0}' does not have a Prepayment Reference Number.";
        public const string CAPpmNotFound = "Prepayment with RefNbr '{0}' not found.";
        public const string CARefundNotFound = "Vendor Refund with RefNbr '{0}' not found.";
    }
}

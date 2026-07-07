<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="ATPT2012.aspx.cs" Inherits="Page_ATPT2012" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="Document" TypeName="CashFundManagement.BLC.ATPTEFMFundMaint" EnableAttributes="true">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="viewInvoice" Visible="False">
            </px:PXDSCallbackCommand>
            <px:PXDSCallbackCommand DependOnGrid="gridUnreplenishedReceipts" Name="viewUnreplenishedBill" Visible="False"/>
            <px:PXDSCallbackCommand DependOnGrid="gridCheck" Name="viewLinkToCheck" Visible="False"/>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Document" Style="z-index: 100" Width="100%" TabIndex="100">
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="S" />
            <px:PXDropDown ID="edFundType" runat="server" DataField="FundType" CommitChanges="True" IsClientControl="True">
            </px:PXDropDown>
            <px:PXSelector ID="edFundCD" runat="server" DataField="FundCD">
            </px:PXSelector>
            <px:PXDropDown ID="edStatus" runat="server" DataField="Status" IsClientControl="True">
            </px:PXDropDown>
            <px:PXCheckBox ID="edHold" runat="server" AlreadyLocalized="False" DataField="Hold" CommitChanges="True" IsClientControl="True" Text="Hold">
            </px:PXCheckBox>
            <px:PXCheckBox ID="edApproved" runat="server" AlreadyLocalized="False" DataField="Approved" CommitChanges="True" IsClientControl="True" Text="Hold">
            </px:PXCheckBox>
            <px:PXDateTimeEdit ID="edDocumentDate" runat="server" AlreadyLocalized="False" DataField="DocumentDate" IsClientControl="True">
            </px:PXDateTimeEdit>
            <px:PXLayoutRule runat="server" ColumnSpan="2">
            </px:PXLayoutRule>
            <px:PXTextEdit ID="edDescr" runat="server" AlreadyLocalized="False" DataField="Descr" IsClientControl="True">
            </px:PXTextEdit>
            <px:PXLayoutRule runat="server" StartColumn="True" />
            <px:PXSegmentMask ID="edBranchID" runat="server" DataField="BranchID" CommitChanges="True">
            </px:PXSegmentMask>
            <px:PXSelector ID="edCustodianID" runat="server" DataField="CustodianID" CommitChanges="True" AutoRefresh="True">
            </px:PXSelector>
            <px:PXSelector DataField="CuryID" ID="PXCurrencyRate1" runat="server" RateTypeView="currencyinfo" DataMember="_Currency_" DataSourceID="ds" CommitChanges="True"/>
            <px:PXSelector ID="edPayeeID" runat="server" DataField="PayeeID" CommitChanges="True" AutoRefresh="True">
            </px:PXSelector>
            <px:PXNumberEdit ID="edCuryInitialFund" runat="server" AlreadyLocalized="False" DataField="CuryInitialFund" IsClientControl="True" CommitChanges="True">
            </px:PXNumberEdit>
            <px:PXLayoutRule runat="server" StartColumn="True">
            </px:PXLayoutRule>

            <px:PXLayoutRule runat="server" StartColumn="true" GroupCaption="Balances">
            </px:PXLayoutRule>
            <px:PXNumberEdit ID="edCuryFundAmt" runat="server" AlreadyLocalized="False" DataField="CuryFundAmt" IsClientControl="True">
            </px:PXNumberEdit>
            <px:PXNumberEdit ID="edCuryBalanceAmt" runat="server" AlreadyLocalized="False" DataField="CuryBalanceAmt" IsClientControl="True">
            </px:PXNumberEdit>
            <px:PXNumberEdit ID="edCuryOnReplenishmentAmt" runat="server" AlreadyLocalized="False" DataField="CuryOnReplenishmentAmt" IsClientControl="True">
            </px:PXNumberEdit>
            <px:PXNumberEdit ID="edCuryLiquidatedAmt" runat="server" AlreadyLocalized="False" DataField="CuryLiquidatedAmt" IsClientControl="True">
            </px:PXNumberEdit>
            <px:PXNumberEdit ID="edCuryUnliquidatedAmt" runat="server" AlreadyLocalized="False" DataField="CuryUnliquidatedAmt" IsClientControl="True">
            </px:PXNumberEdit>

        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" DataMember="CurrentDocument">
        <Items>
            <px:PXTabItem Text="Transaction History">
                <Template>
                    <px:PXGrid ID="grdTransactionHistory" runat="server" Width="100%"
                        DataSourceID="ds" MatrixMode="True" SkinID="Inquire" PageSize="20" SyncPosition="True"
                        TabIndex="1900" AutoAdjustColumns="True">
                        <EmptyMsg AnonFilteredAddMessage="No records found.
Try to change filter to see records here."
                            AnonFilteredMessage="No records found.
Try to change filter to see records here."
                            ComboAddMessage="No records found.
Try to change filter or modify parameters above to see records here."
                            FilteredAddMessage="No records found.
Try to change filter to see records here."
                            FilteredMessage="No records found.
Try to change filter to see records here."
                            NamedComboAddMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            NamedComboMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            NamedFilteredAddMessage="No records found as '{0}'.
Try to change filter to see records here."
                            NamedFilteredMessage="No records found as '{0}'.
Try to change filter to see records here." />
                        <Levels>
                            <px:PXGridLevel DataKeyNames="RefNbr" DataMember="CurrentTransactionHistoryView" >
                                <Columns>
                                    <px:PXGridColumn DataField="TransactionType" CommitChanges="True">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="RefNbr" Width="250px" LinkCommand="viewTransaction">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="FundBranchID" Width="200px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="FundType">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="Status" Width="150px" CommitChanges="True">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="TransactionDate" Width="130px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="CuryFundTransactionDocumentAmt" TextAlign="Left" Width="200px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="CuryWithholdingTax" TextAlign="Left" Width="180px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="CuryUnliquidatedAmt" TextAlign="Left" Width="200px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="CuryLiquidatedAmt" TextAlign="Left" Width="200px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="CuryFundReturnAmt" TextAlign="Left" Width="130px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="CuryBalanceAmt" TextAlign="Left" Width="190px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="CheckNbr" LinkCommand="viewCheck" Width="200px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="CuryCheckAmt" TextAlign="Left" Width="200px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="ReversingJournalBatchNbr" LinkCommand="viewReverseBatch" Width="200px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="ReplenishmentRefNbr" LinkCommand="viewReplenishmentER" Width="210px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="ProjectID" Width="200px" CommitChanges="True">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="ProjectTaskID" Width="100px" CommitChanges="True">
                                    </px:PXGridColumn>
                                </Columns>
                                <Mode AllowAddNew="false" AllowDelete="false" AllowUpdate="false" />
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Financial Details">
                <Template>
                    <px:PXLayoutRule runat="server" StartRow="True" ControlSize="SM" GroupCaption="Link to AP" LabelsWidth="M" StartColumn="True">
                    </px:PXLayoutRule>
                    <px:PXTextEdit ID="edEstablishmentRefNbr" runat="server" AlreadyLocalized="False" DataField="EstablishmentRefNbr" LinkCommand="" IsClientControl="True">
                        <LinkCommand Command="viewInvoice" Target="ds">
                        </LinkCommand>
                    </px:PXTextEdit>
                    <px:PXTextEdit ID="edCloseFundRefNbr" runat="server" AlreadyLocalized="False" DataField="CloseFundRefNbr" IsClientControl="True">
                        <LinkCommand Command="viewCloseFundInvoice" Target="ds"></LinkCommand>
                    </px:PXTextEdit>
                    <px:PXTextEdit ID="edExpenseBatchNbr" runat="server" AlreadyLocalized="False" DataField="ExpenseBatchNbr" IsClientControl="True">
                        <LinkCommand Command="viewExpenseBatchNbr" Target="ds"></LinkCommand>
                    </px:PXTextEdit>
                    <px:PXLayoutRule runat="server" ControlSize="M" GroupCaption="Project Details" StartColumn="True">
                    </px:PXLayoutRule>
                    <px:PXSelector ID="edProjectID" runat="server" DataField="ProjectID" CommitChanges="true">
                    </px:PXSelector>
                    <px:PXSelector ID="edProjectTaskID" runat="server" DataField="ProjectTaskID" CommitChanges="true" AutoRefresh="True">
                    </px:PXSelector>
                    <px:PXSegmentMask ID="edCostCodeID" runat="server" CommitChanges="True" DataField="CostCodeID">
                    </px:PXSegmentMask>
                    <px:PXLayoutRule runat="server" ControlSize="M" GroupCaption="Link to Accounts" StartColumn="True">
                    </px:PXLayoutRule>
                    <px:PXSegmentMask ID="edAccountID" runat="server" DataField="AccountID">
                    </px:PXSegmentMask>
                    <px:PXSegmentMask ID="edSubID" runat="server" DataField="SubID">
                    </px:PXSegmentMask>
                    <px:PXSegmentMask ID="edClearingAccount" runat="server" DataField="ClearingAccount">
                    </px:PXSegmentMask>
                    <px:PXSegmentMask ID="edClearingSubaccount" runat="server" DataField="ClearingSubaccount">
                    </px:PXSegmentMask>
                     <px:PXLayoutRule runat="server" GroupCaption="Link to AP (Unreplenished Receipts)" StartRow="True">
                    </px:PXLayoutRule>
                     <px:PXGrid ID="gridUnreplenishedReceipts" runat="server"  Width="650px" Height="100px" DataSourceID="ds" AutoAdjustColumns="true"
                        FilesIndicator="false" NoteIndicator="false" SkinID="Inquire" >
                        <EmptyMsg AnonFilteredAddMessage="No records found.
Try to change filter to see records here." AnonFilteredMessage="No records found.
Try to change filter to see records here." ComboAddMessage="No records found.
Try to change filter or modify parameters above to see records here." FilteredAddMessage="No records found.
Try to change filter to see records here." FilteredMessage="No records found.
Try to change filter to see records here." NamedComboAddMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here." NamedComboMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here." NamedFilteredAddMessage="No records found as '{0}'.
Try to change filter to see records here." NamedFilteredMessage="No records found as '{0}'.
Try to change filter to see records here." />
                        <Levels>
                            <px:PXGridLevel DataMember="APInvoiceDocument" DataKeyNames="DocType,RefNbr">
                                <Columns>
                                    <px:PXGridColumn DataField="DocType">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="RefNbr" Width="140px" LinkCommand="viewUnreplenishedBill">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="CuryOrigDocAmt" TextAlign="Right" Width="100px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="TaxZoneID">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="TaxCalcMode">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="Status">
                                    </px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <ActionBar ActionsVisible="False">
                        </ActionBar>
                    </px:PXGrid>
                    <px:PXLayoutRule runat="server" GroupCaption="Link to Check" StartRow="True">
                    </px:PXLayoutRule>
                    <px:PXGrid ID="gridCheck" runat="server" DataSourceID="ds" SkinID="Inquire" MatrixMode="True" SyncPosition="True">
                        <EmptyMsg AnonFilteredAddMessage="No records found.
Try to change filter to see records here."
                            AnonFilteredMessage="No records found.
Try to change filter to see records here."
                            ComboAddMessage="No records found.
Try to change filter or modify parameters above to see records here."
                            FilteredAddMessage="No records found.
Try to change filter to see records here."
                            FilteredMessage="No records found.
Try to change filter to see records here."
                            NamedComboAddMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            NamedComboMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            NamedFilteredAddMessage="No records found as '{0}'.
Try to change filter to see records here."
                            NamedFilteredMessage="No records found as '{0}'.
Try to change filter to see records here." />
                        <Levels>
                            <px:PXGridLevel DataMember="APPaymentDocument" DataKeyNames="DocType,RefNbr">
                                <RowTemplate>
                                    <px:PXDropDown ID="edDocType" runat="server" DataField="DocType" Enabled="False" IsClientControl="True">
                                    </px:PXDropDown>
                                    <px:PXSelector ID="edRefNbr" runat="server" AllowEdit="True" DataField="RefNbr" Enabled="False">
                                    </px:PXSelector>
                                    <px:PXNumberEdit ID="edCuryOrigDocAmt" runat="server" AlreadyLocalized="False" DataField="CuryOrigDocAmt" Enabled="False" IsClientControl="True">
                                    </px:PXNumberEdit>
                                    <px:PXDropDown ID="edStatusApDoc" runat="server" DataField="Status" Enabled="False" IsClientControl="True">
                                    </px:PXDropDown>
                                    <pxa:PXCurrencyRate DataField="CuryID" ID="edCury" runat="server" DataMember="_Currency_" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="DocType">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="RefNbr" Width="140px" LinkCommand="viewLinkToCheck">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="CuryOrigDocAmt" TextAlign="Right" Width="100px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="Status">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="CuryID">
                                    </px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Settings">
                <Template>
                    <px:PXFormView ID="frmReplenishment" runat="server" DataMember="CurrentDocument" DataSourceID="ds" SkinID="Transparent" TabIndex="20100">
                        <Template>
                            <px:PXLayoutRule runat="server" ControlSize="SM" LabelsWidth="M" StartRow="True" GroupCaption="Replenishment">
                            </px:PXLayoutRule>
                            <px:PXDropDown ID="edReplenishmentLimit" runat="server" CommitChanges="True" DataField="ReplenishmentLimit" IsClientControl="True">
                            </px:PXDropDown>
                            <px:PXNumberEdit ID="edReplenishPointPercent" runat="server" AlreadyLocalized="False" CommitChanges="True" DataField="ReplenishPointPercent" IsClientControl="True">
                            </px:PXNumberEdit>
                            <px:PXNumberEdit ID="edReplenishmentAmt" runat="server" AlreadyLocalized="False" CommitChanges="True" DataField="ReplenishmentAmt" IsClientControl="True">
                            </px:PXNumberEdit>
                            <px:PXDropDown ID="edReplenishmentRestriction" runat="server" DataField="ReplenishmentRestriction" IsClientControl="True">
                            </px:PXDropDown>

                            <px:PXLayoutRule runat="server" ControlSize="SM" LabelsWidth="M" StartColumn="true" GroupCaption="Fund Transaction">
                            </px:PXLayoutRule>
                            <px:PXDropDown ID="edFundTransactionLimit" runat="server" CommitChanges="True" DataField="FundTransactionLimit" IsClientControl="True">
                            </px:PXDropDown>
                            <px:PXNumberEdit ID="edFundTransactionPointPercent" runat="server" AlreadyLocalized="False" CommitChanges="True" DataField="FundTransactionPointPercent" IsClientControl="True">
                            </px:PXNumberEdit>
                            <px:PXNumberEdit ID="edFundTransactionAmt" runat="server" AlreadyLocalized="False" CommitChanges="True" DataField="FundTransactionAmt" IsClientControl="True">
                            </px:PXNumberEdit>
                            <px:PXDropDown ID="edFundTransactionRestriction" runat="server" DataField="FundTransactionRestriction" IsClientControl="True" CommitChanges="True">
                            </px:PXDropDown>
                        </Template>
                    </px:PXFormView>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem BindingContext="form" Text="Approvals">
                <Template>
                    <px:PXGrid ID="gridApproval" runat="server" DataSourceID="ds" NoteIndicator="True" SkinID="DetailsInTab" Style="left: 0px; top: 0px;" TabIndex="26200" Width="100%">
                        <EmptyMsg AnonFilteredAddMessage="No records found.
Try to change filter to see records here."
                            AnonFilteredMessage="No records found.
Try to change filter to see records here."
                            ComboAddMessage="No records found.
Try to change filter or modify parameters above to see records here."
                            FilteredAddMessage="No records found.
Try to change filter to see records here."
                            FilteredMessage="No records found.
Try to change filter to see records here."
                            NamedComboAddMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            NamedComboMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            NamedFilteredAddMessage="No records found as '{0}'.
Try to change filter to see records here."
                            NamedFilteredMessage="No records found as '{0}'.
Try to change filter to see records here." />
                        <Levels>
                            <px:PXGridLevel DataKeyNames="ApprovalID" DataMember="Approval">
                                <Columns>
                                    <px:PXGridColumn DataField="ApproverEmployee__AcctCD" Width="160px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="ApproverEmployee__AcctName" Width="160px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="ApprovedByEmployee__AcctCD" Width="100px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="ApprovedByEmployee__AcctName" Width="160px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="ApproveDate" Width="90px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn AllowNull="False" AllowUpdate="False" DataField="Status" RenderEditorText="True">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="WorkgroupID" Width="150px">
                                    </px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" />
                        <Mode AllowAddNew="False" AllowDelete="False" AllowUpdate="False" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
    </px:PXTab>
</asp:Content>
<asp:Content ID="contDialogs" ContentPlaceHolderID="phDialogs" runat="Server">
    <px:PXSmartPanel ID="pnlIncreaseFund" runat="server" CaptionVisible="True" Caption="Increase Fund"
        Style="position: static" LoadOnDemand="True" Key="IncreaseFundDocument" DependsOnView="IncreaseFundDocument" AutoCallBack-Target="frmIncreaseFund"
        AutoCallBack-Command="Refresh" DesignView="Content">
        <px:PXFormView ID="frmIncreaseFund" runat="server" DataMember="IncreaseFundDocument" SkinID="Transparent" DataSourceID="ds">
            <Template>
                <px:PXLayoutRule runat="server" ControlSize="M" LabelsWidth="M" StartColumn="True" />

                <px:PXNumberEdit ID="edIncreaseFund" runat="server" AlreadyLocalized="False" DataField="IncreaseFund" DefaultLocale="" CommitChanges="true" />

                <px:PXPanel ID="PXPanelButtons" runat="server" SkinID="Buttons">
                    <px:PXButton ID="btnOk" runat="server" Text="OK" DialogResult="OK" CommanSourceID="ds" Width="100px" Height="30px" />
                    <px:PXButton ID="btnMyCommandCancel" runat="server" Text="Cancel" DialogResult="Cancel" CommanSourceID="ds" Width="100px" Height="30px" />
                </px:PXPanel>
            </Template>
        </px:PXFormView>
    </px:PXSmartPanel>

    <px:PXSmartPanel ID="pnlDecreaseFund" runat="server" CaptionVisible="True" Caption="Decrease Fund"
        Style="position: static" LoadOnDemand="True" Key="DecreaseFundDocument" DependsOnView="DecreaseFundDocument" AutoCallBack-Target="frmDecreaseFund"
        AutoCallBack-Command="Refresh" DesignView="Content">
        <px:PXFormView ID="frmDecreaseFund" runat="server" DataMember="DecreaseFundDocument" SkinID="Transparent" DataSourceID="ds">
            <Template>
                <px:PXLayoutRule runat="server" ControlSize="M" LabelsWidth="M" StartColumn="True" />

                <px:PXNumberEdit ID="edDecreaseFund" runat="server" AlreadyLocalized="False" DataField="DecreaseFund" DefaultLocale="" CommitChanges="true" />

                <px:PXPanel ID="PXPanelButtons" runat="server" SkinID="Buttons">
                    <px:PXButton ID="btnOk" runat="server" Text="OK" DialogResult="OK" CommanSourceID="ds" Width="100px" Height="30px" />
                    <px:PXButton ID="btnMyCommandCancel" runat="server" Text="Cancel" DialogResult="Cancel" CommanSourceID="ds" Width="100px" Height="30px" />
                </px:PXPanel>
            </Template>
        </px:PXFormView>
    </px:PXSmartPanel>
</asp:Content>

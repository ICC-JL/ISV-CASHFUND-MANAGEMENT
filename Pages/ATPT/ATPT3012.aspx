<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="ATPT3012.aspx.cs" Inherits="Page_ATPT3012" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="Replenishments" TypeName="CashFundManagement.BLC.ATPTEFMReplenishmentEntry" EnableAttributes="true">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="ShowSubmitReceipt" Visible="False">
            </px:PXDSCallbackCommand>
            <px:PXDSCallbackCommand Name="SubmitReceipt" Visible="False">
            </px:PXDSCallbackCommand>
            <px:PXDSCallbackCommand Name="CancelSubmitReceipt" Visible="False">
            </px:PXDSCallbackCommand>
            <px:PXDSCallbackCommand Name="OpenTransaction" Visible="False" DependOnGrid="PXGrid2">
            </px:PXDSCallbackCommand>
            <px:PXDSCallbackCommand Name="OpenReceipts" Visible="False" DependOnGrid="PXGrid1">
            </px:PXDSCallbackCommand>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Replenishments" TabIndex="2100">
        <Template>
            <px:PXLayoutRule runat="server" StartRow="True" StartColumn="True" />
            <px:PXDropDown ID="edFundType" runat="server" DataField="FundType" IsClientControl="True" CommitChanges="True">
            </px:PXDropDown>
            <px:PXSelector ID="edReplenishmentNbr" runat="server" DataField="ReplenishmentNbr">
            </px:PXSelector>
            <px:PXSelector ID="edFundID" runat="server" CommitChanges="True" DataField="FundID" AutoRefresh="True">
            </px:PXSelector>
            <px:PXDropDown ID="edStatus" runat="server" DataField="Status" IsClientControl="True">
            </px:PXDropDown>
            <px:PXCheckBox ID="edHold" runat="server" AlreadyLocalized="False" DataField="Hold" Text="Hold" CommitChanges="True" IsClientControl="True">
            </px:PXCheckBox>
            <px:PXDateTimeEdit ID="edDate" runat="server" AlreadyLocalized="False" DataField="Date" IsClientControl="True">
            </px:PXDateTimeEdit>
            <px:PXCheckBox ID="edApproved" runat="server" AlreadyLocalized="False" DataField="Approved" Text="Approved" IsClientControl="True">
            </px:PXCheckBox>
            <px:PXCheckBox ID="edRequestApproval" runat="server" AlreadyLocalized="False" DataField="RequestApproval" Text="Request Approval" IsClientControl="True">
            </px:PXCheckBox>
            <px:PXLayoutRule runat="server" ColumnSpan="2">
            </px:PXLayoutRule>
            <px:PXTextEdit ID="edDescr" runat="server" AlreadyLocalized="False" DataField="Descr" IsClientControl="True">
            </px:PXTextEdit>
            <px:PXLayoutRule runat="server" StartColumn="True">
            </px:PXLayoutRule>
            <px:PXSelector ID="edCustodianID" runat="server" DataField="CustodianID">
            </px:PXSelector>
            <px:PXSelector ID="edPayeeID" runat="server" DataField="PayeeID">
            </px:PXSelector>
            <px:PXSelector ID="edDepartmentID" runat="server" DataField="DepartmentID">
            </px:PXSelector>
            <px:PXSegmentMask ID="edBranchID" runat="server" DataField="BranchID" CommitChanges="True">
            </px:PXSegmentMask>
            <px:PXLayoutRule runat="server" StartColumn="True">
            </px:PXLayoutRule>
            <px:PXNumberEdit ID="edClaimAmount" runat="server" AlreadyLocalized="False" DataField="ClaimAmount" IsClientControl="True">
            </px:PXNumberEdit>
            <px:PXNumberEdit ID="edWithholdingTaxAmount" runat="server" AlreadyLocalized="False" DataField="WithholdingTaxAmount" IsClientControl="True">
            </px:PXNumberEdit>
            <px:PXNumberEdit ID="edVatAmount" runat="server" AlreadyLocalized="False" DataField="VatAmount" IsClientControl="True">
            </px:PXNumberEdit>
        </Template>
    </px:PXFormView>

    <px:PXSmartPanel ID="PanelSubmitReceipts" runat="server" Height="396px" Width="910px" Caption="Add Receipts" CaptionVisible="True" Key="ReceiptsForSubmit" AutoReload="True" CancelButtonID="PXButtonCancel" AutoRepaint="True" CallBackMode-CommitChanges="True" AlreadyLocalized="False" CreateOnDemand="True" TabIndex="9700" DesignView="Content">
        <px:PXGrid ID="gridReceiptsForSubmit" runat="server" Height="240px" Width="100%" DataSourceID="ds" SkinID="Inquire" NoteIndicator="False" FilesIndicator="False" SyncPosition="True" TabIndex="12400">
            <AutoSize Enabled="true" />
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
                <px:PXGridLevel DataKeyNames="ClaimDetailID" DataMember="ReceiptsForSubmit">
                    <Columns>
                        <px:PXGridColumn AllowCheckAll="True" AllowNull="False" DataField="Selected" TextAlign="Center" Type="CheckBox" Width="30px" />
                        <px:PXGridColumn DataField="ClaimDetailCD" />
                        <px:PXGridColumn DataField="ExpenseDate" Width="90px">
                        </px:PXGridColumn>
                        <px:PXGridColumn DataField="ExpenseRefNbr" Width="140px">
                        </px:PXGridColumn>
                        <px:PXGridColumn DataField="EmployeeID" Width="140px">
                        </px:PXGridColumn>
                        <px:PXGridColumn DataField="TranDesc" Width="280px">
                        </px:PXGridColumn>
                        <px:PXGridColumn DataField="TaxZoneID" Width="120px">
                        </px:PXGridColumn>
                        <px:PXGridColumn DataField="CuryTranAmtWithTaxes" TextAlign="Right" Width="100px">
                        </px:PXGridColumn>
                        <px:PXGridColumn DataField="CuryID">
                        </px:PXGridColumn>
                        <px:PXGridColumn DataField="Status">
                        </px:PXGridColumn>
                    </Columns>
                </px:PXGridLevel>
            </Levels>
            <Mode AllowAddNew="False" AllowDelete="False" AllowUpdate="False" />
        </px:PXGrid>
        <px:PXPanel ID="PXPanelBtn" runat="server" SkinID="Buttons">
            <px:PXButton ID="PXButtonAdd" runat="server" Text="Add" CommandName="SubmitReceipt" CommandSourceID="ds" />
            <px:PXButton ID="PXButtonAddClose" runat="server" Text="Add & Close" CommandSourceID="ds" DialogResult="OK" />
            <px:PXButton ID="PXButtonClose" runat="server" DialogResult="Cancel" Text="Close" CommandName="CancelSubmitReceipt" CommandSourceID="ds" />
        </px:PXPanel>
    </px:PXSmartPanel>

    <px:PXSmartPanel ID="pnlTaxRows" runat="server" CaptionVisible="True" Key="Tax_Rows" AcceptButtonID="Ok" AutoRepaint="true" AutoReload="true"
        Width="764px" Height="360px" Caption="Document Taxes">
        <px:PXGrid ID="grdTaxRows" runat="server" Width="100%"
            DataSourceID="ds" SkinID="Details" AdjustPageSize="Auto"
            NoteIndicator="false" FilesIndicator="false" SyncPosition="true">
            <AutoSize Enabled="true" />
            <Levels>
                <px:PXGridLevel DataMember="Tax_Rows">
                    <Columns>
                        <px:PXGridColumn DataField="TaxID" CommitChanges="true" />
                        <px:PXGridColumn DataField="TaxRate" TextAlign="Right" CommitChanges="true" />
                        <px:PXGridColumn DataField="CuryTaxableAmt" TextAlign="Right" CommitChanges="true" />
                        <px:PXGridColumn DataField="CuryTaxAmt" TextAlign="Right" CommitChanges="true" />
                        <px:PXGridColumn DataField="NonDeductibleTaxRate" TextAlign="Right" CommitChanges="true" />
                        <px:PXGridColumn DataField="CuryExpenseAmt" TextAlign="Right" CommitChanges="true" />
                        <px:PXGridColumn DataField="Tax__TaxType" CommitChanges="true" />
                        <px:PXGridColumn DataField="Tax__PendingTax" TextAlign="Center" Type="CheckBox" CommitChanges="true" />
                        <px:PXGridColumn DataField="Tax__ReverseTax" TextAlign="Center" Type="CheckBox" CommitChanges="true" />
                        <px:PXGridColumn DataField="Tax__ExemptTax" TextAlign="Center" Type="CheckBox" CommitChanges="true" />
                        <px:PXGridColumn DataField="Tax__StatisticalTax" TextAlign="Center" Type="CheckBox" CommitChanges="true" />
                    </Columns>
                </px:PXGridLevel>
            </Levels>
            <AutoSize Enabled="true" />
        </px:PXGrid>
        <px:PXPanel ID="pnlCommitTaxRows" runat="server" SkinID="Buttons">
            <px:PXButton ID="btnCommitTaxRows" runat="server" DialogResult="Yes" Text="Ok" CommandName="CommitTaxes" CommandSourceID="ds" />
        </px:PXPanel>
    </px:PXSmartPanel>

</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds">
        <Items>
            <px:PXTabItem Text="Replenishment Details">
                <Template>
                    <px:PXGrid ID="PXGrid1" runat="server" DataSourceID="ds" SkinID="DetailsInTab" TabIndex="7500" Width="100%" SyncPosition="True">
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
                            <px:PXGridLevel DataMember="ReplenishmentDetails" DataKeyNames="ReplenishmentDetailID">
                                <RowTemplate>
                                    <px:PXSelector ID="edExpenseReceiptNbr" runat="server" DataField="ExpenseReceiptNbr">
                                    </px:PXSelector>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__BranchID" Width="90px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="ExpenseReceiptNbr" Width="140px" LinkCommand="OpenReceipts">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__ExpenseDate" Width="90px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__TranDesc" Width="280px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__Qty" TextAlign="Right" Width="100px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__InventoryID">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__UOM" Width="72px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__CuryUnitCost" TextAlign="Right" Width="100px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__CuryExtCost" TextAlign="Right" Width="100px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="CuryTaxTotal" TextAlign="Right" Width="100px" LinkCommand="ViewTaxes" CommitChanges="true">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__ExpenseAccountID" Width="120px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__ExpenseSubID" Width="140px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__ExpenseRefNbr" Width="140px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="Contract__ContractCD" Width="200px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__TaskID" Width="100px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__CostCodeID" Width="100px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__UsrATPTVendID" Width="140px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__UsrATPTVendName" Width="220px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__UsrATPTAddress" Width="180px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__UsrATPTVendTIN" Width="180px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__TaxZoneID" Width="120px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="EPExpenseClaimDetails__TaxCategoryID" Width="120px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="InventoryItem__ItemType">
                                    </px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" />
                        <ActionBar>
                            <Actions>
                                <AddNew Enabled="False" />
                                <EditRecord Enabled="False" />
                            </Actions>
                            <CustomItems>
                                <px:PXToolBarButton Key="cmdSubmitReceipt" DisplayStyle="Text" AlreadyLocalized="False" SuppressHtmlEncoding="False" UsesSignalR="False">
                                    <AutoCallBack Target="ds" Command="ShowSubmitReceipt" />
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Taxes">
                <Template>
                    <px:PXFormView ID="frmCurrent" runat="server" DataSourceID="ds" DataMember="CurrentReplenishment" SkinID="Transparent" TabIndex="2500">
                        <Template>
                            <px:PXLayoutRule runat="server" StartRow="True">
                            </px:PXLayoutRule>
                            <%--<px:PXCheckBox ID="edOverrideTax" runat="server" AlignLeft="True" AlreadyLocalized="False" DataField="OverrideTax" IsClientControl="True" Text="Override Default Tax">
                            </px:PXCheckBox>--%>
                        </Template>
                    </px:PXFormView>
                    <px:PXGrid ID="grdTaxes" runat="server" DataSourceID="ds" SkinID="DetailsInTab" TabIndex="2700" Width="100%">
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
                            <px:PXGridLevel DataMember="Taxes" DataKeyNames="ReplenishmentTaxDetailID">
                                <Columns>
                                    <px:PXGridColumn CommitChanges="True" DataField="TaxID" Width="140px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="TaxType" TextAlign="Right" Width="100px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="TaxRate" TextAlign="Right" Width="100px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="TaxableAmt" TextAlign="Right" Width="100px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="TaxAmt" TextAlign="Right" Width="100px">
                                    </px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Financial Details">
                <Template>
                    <px:PXLayoutRule runat="server" GroupCaption="Tax" StartGroup="True">
                    </px:PXLayoutRule>
                    <px:PXFormView ID="PXFormView1" runat="server" DataKeyNames="ReplenishmentNbr" DataMember="ReplenishmentCurrent" DataSourceID="ds">
                        <Template>
                            <px:PXLayoutRule runat="server" StartRow="True">
                            </px:PXLayoutRule>
                            <px:PXSelector ID="edTaxZone" runat="server" CommitChanges="True" DataField="TaxZone">
                            </px:PXSelector>
                        </Template>
                    </px:PXFormView>
                    <px:PXLayoutRule runat="server" GroupCaption="Link to AP" StartGroup="True">
                    </px:PXLayoutRule>
                    <px:PXGrid ID="PXGrid2" runat="server" DataSourceID="ds" SkinID="Inquire" TabIndex="7000" FilesIndicator="False" Height="100px" NoteIndicator="False" Width="100%">
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
                            <px:PXGridLevel DataMember="APInvoiceDocument" DataKeyNames="DocType,RefNbr">
                                <Columns>
                                    <px:PXGridColumn DataField="DocType">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="RefNbr" Width="140px" LinkCommand="OpenTransaction">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="CuryOrigDocAmt" TextAlign="Right" Width="100px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="TaxZoneID" Width="120px">
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
                    <px:PXLayoutRule runat="server" GroupCaption="Link to Check" StartGroup="True">
                    </px:PXLayoutRule>
                    <px:PXGrid ID="gridAPPaymentDocument" runat="server" AllowPaging="True" AutoAdjustColumns="True" DataSourceID="ds" FilesIndicator="False" Height="100px" NoteIndicator="False" SkinID="Inquire" Width="100%" AdjustPageSize="Auto" SyncPosition="True">
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
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="DocType">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="RefNbr" Width="140px" CommitChanges="True" LinkCommand="ViewCheck">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="OrigDocAmt" TextAlign="Right" Width="100px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="Status">
                                    </px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Container="Window" Enabled="True" />
                        <ActionBar ActionsVisible="False">
                        </ActionBar>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Approvals" BindingContext="form" VisibleExp="DataControls[&quot;edRequestApproval&quot;].Value = 1">
                <Template>
                    <px:PXGrid ID="gridApproval" runat="server" DataSourceID="ds" Width="100%" SkinID="DetailsInTab" NoteIndicator="True" Style="left: 0px; top: 0px;" TabIndex="26200">
                        <AutoSize Enabled="True"></AutoSize>
                        <Mode AllowAddNew="False" AllowDelete="False" AllowUpdate="False"></Mode>
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
Try to change filter to see records here."></EmptyMsg>
                        <Levels>
                            <px:PXGridLevel DataMember="Approval" DataKeyNames="ApprovalID">
                                <Columns>
                                    <px:PXGridColumn DataField="ApproverEmployee__AcctCD" Width="160px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ApproverEmployee__AcctName" Width="160px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ApprovedByEmployee__AcctCD" Width="100px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ApprovedByEmployee__AcctName" Width="160px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ApproveDate" Width="90px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Status" AllowNull="False" AllowUpdate="False" RenderEditorText="True"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="WorkgroupID" Width="150px"></px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
    </px:PXTab>
</asp:Content>

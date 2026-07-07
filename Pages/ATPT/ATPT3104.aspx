<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="ATPT3104.aspx.cs" Inherits="Page_ATPT3104" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="Document" TypeName="CashFundManagement.BLC.ATPTEFMMonthEndEntry" EnableAttributes="true">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="ShowSubmitReceipt" Visible="False">
            </px:PXDSCallbackCommand>
            <px:PXDSCallbackCommand Name="viewBatch" Visible="False">
            </px:PXDSCallbackCommand>
            <px:PXDSCallbackCommand Name="viewReversingBatch" Visible="False">
            </px:PXDSCallbackCommand>
            <px:PXDSCallbackCommand Name="SubmitReceipt" Visible="False">
            </px:PXDSCallbackCommand>
            <px:PXDSCallbackCommand Name="CancelSubmitReceipt" Visible="False">
            </px:PXDSCallbackCommand>
        </CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Document" TabIndex="1400">
		<Template>
			<px:PXLayoutRule runat="server" StartRow="True" StartColumn="True"/>
		    <px:PXSelector ID="edRefNbr" runat="server" DataField="RefNbr">
            </px:PXSelector>
            <px:PXDropDown ID="edStatus" runat="server" DataField="Status">
            </px:PXDropDown>
            <px:PXCheckBox ID="edHold" runat="server" AlreadyLocalized="False" DataField="Hold" Text="Hold" CommitChanges="True">
            </px:PXCheckBox>
            <px:PXCheckBox ID="edApproved" runat="server" AlreadyLocalized="False" DataField="Approved" Text="Approved">
            </px:PXCheckBox>
            <px:PXMaskEdit ID="edFinPeriodID" runat="server" AlreadyLocalized="False" CommitChanges="True" DataField="FinPeriodID" IsClientControl="True">
            </px:PXMaskEdit>
            <px:PXDateTimeEdit ID="edDate" runat="server" AlreadyLocalized="False" DataField="Date" CommitChanges="True" DefaultLocale="" IsClientControl="True">
            </px:PXDateTimeEdit>
            <px:PXCheckBox ID="edRequestApproval" runat="server" AlreadyLocalized="False" DataField="RequestApproval" Text="Request Approval">
            </px:PXCheckBox>
            <px:PXLayoutRule runat="server" StartColumn="True">
            </px:PXLayoutRule>
            <px:PXSelector ID="edFundID" runat="server" CommitChanges="True" DataField="FundID" AutoRefresh="true">
            </px:PXSelector>
            <px:PXSegmentMask ID="edAccountID" runat="server" DataField="AccountID">
            </px:PXSegmentMask>
            <px:PXSegmentMask ID="edSubID" runat="server" DataField="SubID">
            </px:PXSegmentMask>
		    <px:PXSegmentMask ID="edCreditAccountID" runat="server" DataField="CreditAccountID">
            </px:PXSegmentMask>
            <pxa:PXCurrencyRate DataField="CuryID" ID="edCury" runat="server" DataSourceID="ds" RateTypeView="_ATPTEFMMonthEnd_CurrencyInfo_"
                DataMember="_ATPTEFMMonthEnd_PX.Objects.CM.Currency+curyID_" Width="250px"></pxa:PXCurrencyRate>
            <px:PXLayoutRule runat="server" StartColumn="True">
            </px:PXLayoutRule>
            <px:PXNumberEdit ID="edAmount" runat="server" AlreadyLocalized="False" DataField="Amount" DefaultLocale="" IsClientControl="True" CommitChanges="true">
            </px:PXNumberEdit>
		</Template>
	</px:PXFormView>

    <px:PXSmartPanel ID="PanelSubmitReceipts" runat="server" Height="396px" Width="910px" Caption="Add Receipts" CaptionVisible="True" Key="ExpenseReceipts" AutoReload="True" CancelButtonID="PXButtonCancel" AutoRepaint="True" CallBackMode-CommitChanges="True" AlreadyLocalized="False" CreateOnDemand="True" TabIndex="9700" DesignView="Content">
        <px:PXGrid ID="gridExpenseReceipts" runat="server" Height="240px" Width="100%" DataSourceID="ds" SkinID="Inquire" NoteIndicator="False" FilesIndicator="False" SyncPosition="True" TabIndex="12400" >
            <AutoSize Enabled="true" />
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
                <px:PXGridLevel DataKeyNames="ClaimDetailID" DataMember="ExpenseReceipts">
                    <Columns>
                        <px:PXGridColumn AllowCheckAll="True" AllowNull="False" DataField="Selected" TextAlign="Center" Type="CheckBox" Width="30px" />
                        <px:PXGridColumn DataField="ClaimDetailCD" />
                        
                        <px:PXGridColumn DataField="ExpenseDate" Width="90px">
                        </px:PXGridColumn>
                        <px:PXGridColumn DataField="InventoryID">
                        </px:PXGridColumn>
                        <px:PXGridColumn DataField="ExpenseAccountID" Width="120px">
                        </px:PXGridColumn>
                        <px:PXGridColumn DataField="ExpenseSubID" Width="140px">
                        </px:PXGridColumn>
                        <px:PXGridColumn DataField="ContractID">
                        </px:PXGridColumn>
                        <px:PXGridColumn DataField="TaskID">
                        </px:PXGridColumn>
                        <px:PXGridColumn DataField="CostCodeID">
                        </px:PXGridColumn>
                        <px:PXGridColumn DataField="CuryExtCost" TextAlign="Right" Width="100px">
                        </px:PXGridColumn>
                        <px:PXGridColumn DataField="TranDesc" Width="280px">
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
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds">
		<Items>
			<px:PXTabItem Text="Document Details">
			    <Template>
                    <px:PXGrid ID="PXGrid1" runat="server" DataSourceID="ds" TabIndex="2800" SkinID="DetailsInTab" Width="100%" AutoAdjustColumns="True">
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
                            <px:PXGridLevel DataMember="Details">
                                <Columns>
                                    <px:PXGridColumn DataField="BranchID" Width="140px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="ExpenseReceiptRefNbr" Width="140px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="InventoryID">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="AccountID" Width="120px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="SubID" Width="140px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="ContractID">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="TaskID">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="CostCodeID">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="Amount">
                                    </px:PXGridColumn>
                                </Columns>
                                 <RowTemplate>
                                     <px:PXNumberEdit ID="edDetailAmount" runat="server" AlreadyLocalized="False" DataField="Amount" IsClientControl="True" CommitChanges="true">
                                    </px:PXNumberEdit>
                                </RowTemplate>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" />
                        <ActionBar>
                            <Actions>
                                <AddNew Enabled="False" />
                                <EditRecord Enabled="False" />
                            </Actions>
                            <CustomItems>
                                <px:PXToolBarButton Key="cmdSubmitReceipt" DisplayStyle="Text" >
					                <AutoCallBack Target="ds" Command="ShowSubmitReceipt"  />
				                </px:PXToolBarButton>
			                </CustomItems>
                        </ActionBar>
                    </px:PXGrid>
                </Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Tab item 2">
			</px:PXTabItem>
             <px:PXTabItem Text="Financial Details">
                <Template>
                    <px:PXFormView ID="frmFinancialDetails" runat="server" DataMember="CurrentDocument" DataSourceID="ds" SkinID="Transparent" TabIndex="20100">
                        <Template>
                            <px:PXLayoutRule runat="server" ControlSize="SM" LabelsWidth="S" StartRow="True">
                            </px:PXLayoutRule>
                            <px:PXTextEdit ID="edJournalBatchNbr" runat="server" AlreadyLocalized="False" DataField="JournalBatchNbr" IsClientControl="True">
                                <LinkCommand Command="viewBatch" Target="ds"></LinkCommand>
                            </px:PXTextEdit>
                            <px:PXTextEdit ID="edReversingJournalBatchNbr" runat="server" AlreadyLocalized="False" DataField="ReversingJournalBatchNbr" IsClientControl="True">
                                <LinkCommand Command="viewReversingBatch" Target="ds"></LinkCommand>
                            </px:PXTextEdit>
                            <px:PXLayoutRule runat="server" ControlSize="L" LabelsWidth="S">
                            </px:PXLayoutRule>
                            <px:PXSegmentMask ID="edBranchID" runat="server" DataField="BranchID" CommitChanges="True">
                            </px:PXSegmentMask>
                        </Template>
                    </px:PXFormView>
                </Template>
            </px:PXTabItem>
                    <px:PXTabItem Text="Approval Details" BindingContext="form" VisibleExp="DataControls[&quot;edRequestApproval&quot;].Value = 1">
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

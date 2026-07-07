<%@ Page Language="C#" MasterPageFile="~/MasterPages/TabView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="ATPT1003.aspx.cs" Inherits="Page_ATPT1003" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="Preference" TypeName="CashFundManagement.BLC.ATPTEFMCASetupMaint">
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXTab ID="tab" runat="server" DataSourceID="ds" Height="150px" Style="z-index: 100" Width="100%" DataMember="Preference">
        <Items>
            <px:PXTabItem Text="General Settings">
                <Template>
                    <px:PXFormView ID="PXFormView2" runat="server" DataMember="Preference" DataSourceID="ds" TabIndex="1700" >
                        <Template> 
                            <px:PXLayoutRule runat="server" ID="enableDisableRow" StartRow="True" />
                            <px:PXCheckBox runat="server" ID="edEnableCFM" DataField="EnableCFM" />
                            <px:PXLayoutRule runat="server" GroupCaption="Numbering Settings" StartGroup="True" LabelsWidth="XM">
                            </px:PXLayoutRule>
                            <px:PXSelector ID="edCANumberingID" runat="server" DataField="CANumberingID" AllowEdit="True" edit="1">
                            </px:PXSelector>
                            <px:PXSelector ID="edLiquidationNumberingID" runat="server" DataField="LiquidationNumberingID" AllowEdit="True" edit="1">
                            </px:PXSelector>
                            <px:PXLayoutRule runat="server" GroupCaption="Cash Advance Settings" StartGroup="True" LabelsWidth="SM">
                            </px:PXLayoutRule>
                            <px:PXCheckBox ID="edAllowableCAAmt" runat="server" AlreadyLocalized="False" CommitChanges="True" DataField="AllowableCAAmt" IsClientControl="True" Text="Restrict Allowable CA Amount">
                            </px:PXCheckBox>
                            <px:PXDropDown ID="edRVAllowableCAAmt" runat="server" DataField="RVAllowableCAAmt" IsClientControl="True">
                            </px:PXDropDown>
                            <px:PXCheckBox ID="edAllowableOpenCA" runat="server" AlreadyLocalized="False" CommitChanges="True" DataField="AllowableOpenCA" IsClientControl="True" Text="Restrict Allowable Open CA">
                            </px:PXCheckBox>
                            <px:PXDropDown ID="edRVAllowableOpenCA" runat="server" DataField="RVAllowableOpenCA" IsClientControl="True">
                            </px:PXDropDown>
                            <px:PXCheckBox ID="edIsFilterByEmployeeDelegates" runat="server" AlreadyLocalized="False" CommitChanges="True" DataField="IsFilterByEmployeeDelegates" IsClientControl="True" Text="Filtered View">
                            </px:PXCheckBox>
                            <px:PXCheckBox ID="edAutoReleaseAP" runat="server" AlreadyLocalized="False" DataField="AutoReleaseAP" IsClientControl="True" Text="Auto Release AP Bill">
                            </px:PXCheckBox>
                            <px:PXCheckBox ID="edRestrictCAEmployees" runat="server" AlreadyLocalized="False" DataField="RestrictCAEmployees" IsClientControl="True" Text="Restrict Employees to CA">
                            </px:PXCheckBox>
                            <px:PXCheckBox ID="edCopyAPNotes" runat="server" AlreadyLocalized="False" DataField="CopyAPNotes" IsClientControl="True" Text="Copy Notes and Files to AP">
                            </px:PXCheckBox>
                            <px:PXCheckBox ID="edIsRequireApprovalCashAdvanceBill" runat="server" AlreadyLocalized="False" DataField="IsRequireApprovalCashAdvanceBill" IsClientControl="True" CommitChanges="True">
                            </px:PXCheckBox>
                            <px:PXCheckBox ID="edAllowSubmissionExcessCA" runat="server" AlreadyLocalized="False" DataField="AllowSubmissionExcessCA" IsClientControl="True" Text="Allow Submission of Excess CA using Vendor Refund">
                            </px:PXCheckBox>
                            <px:PXCheckBox ID="edCopyECNotes" runat="server" AlreadyLocalized="False" DataField="CopyECNotes" IsClientControl="True">
                            </px:PXCheckBox>
                            <px:PXLayoutRule runat="server" StartColumn="True">
                            </px:PXLayoutRule>
                            <px:PXLayoutRule runat="server" GroupCaption="Liquidation Settings" StartGroup="True" LabelsWidth="M">
                            </px:PXLayoutRule>
                            <px:PXDropDown ID="edLiquidationRule" runat="server" CommitChanges="True" DataField="LiquidationRule" IsClientControl="True">
                            </px:PXDropDown>
                            <px:PXNumberEdit ID="edStandardAllowableDays" runat="server" AlreadyLocalized="False" DataField="StandardAllowableDays" IsClientControl="True" SmartSuggestionMode="">
                            </px:PXNumberEdit>
                            <px:PXSegmentMask ID="edReClassAccoundID" runat="server" CommitChanges="True" DataField="ReClassAccoundID">
                            </px:PXSegmentMask>
                            <px:PXSegmentMask ID="edReClassSubID" runat="server" CommitChanges="True" DataField="ReClassSubID">
                            </px:PXSegmentMask>
                            <px:PXCheckBox ID="edLiquidationDateBasedOnWorkCalendar" runat="server" AlreadyLocalized="False" DataField="LiquidationDateBasedOnWorkCalendar" IsClientControl="True" Text="Liquidation Date based on Work Calendar">
                            </px:PXCheckBox>               
                            <px:PXCheckBox ID="edAutoApplyPPT" runat="server" AlreadyLocalized="False" DataField="AutoApplyPPT" IsClientControl="True" Text="Auto Apply Prepayment to Invoice">
                            </px:PXCheckBox>
                            <px:PXCheckBox ID="edAutoApplyCredAdjPPT" runat="server" AlreadyLocalized="False" DataField="AutoApplyCredAdjPPT" IsClientControl="True" Text="Auto Apply Prepayment to Invoice">
                            </px:PXCheckBox>
                            <px:PXCheckBox ID="edAllowManualReceipts" runat="server" AlreadyLocalized="False" DataField="AllowManualReceipts" IsClientControl="True" Text="Allow Manual Receipts">
                            </px:PXCheckBox>
                            <px:PXCheckBox ID="edRequireExtRef" runat="server" AlreadyLocalized="False" DataField="RequireExtRef" IsClientControl="True" Text="Require External Reference Nbr.">
                            </px:PXCheckBox>
                            <px:PXCheckBox ID="edIsRequireApprovalLiquidationBill" runat="server" AlreadyLocalized="False" DataField="IsRequireApprovalLiquidationBill" IsClientControl="True">
                            </px:PXCheckBox>
<%--                            <px:PXCheckBox ID="edErrorDuplicateExtRef" runat="server" AlreadyLocalized="False" DataField="ErrorDuplicateExtRef" IsClientControl="True" Text="Require External Reference Nbr.">
                            </px:PXCheckBox>--%>
                            <px:PXCheckBox ID="edRequireVendorDetails" runat="server" AlreadyLocalized="False" DataField="RequireVendorDetails" IsClientControl="True" Text="Require Vendor Details on Receipts Tab and Expense Receipt">
                            </px:PXCheckBox>
                            <px:PXCheckBox ID="edCashAdvanceAccountEnable" runat="server" AlreadyLocalized="False" DataField="CashAdvanceAccountEnable" Text="Enable Account">
                            </px:PXCheckBox>
                            <px:PXCheckBox ID="edCashAdvanceSubAccountEnable" runat="server" AlreadyLocalized="False" DataField="CashAdvanceSubAccountEnable" Text="Enable Subaccount">
                            </px:PXCheckBox>
                            <px:PXCheckBox ID="edSetNonStockItemDescriptionAsDefault" runat="server" AlreadyLocalized="False" CommitChanges="True" DataField="SetNonStockItemDescriptionAsDefault">
                            </px:PXCheckBox>
                        </Template>
                    </px:PXFormView>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Cash Advance Approval">
                <Template>

                    <px:PXFormView ID="PXFormView1" runat="server" DataMember="Preference" DataSourceID="ds" TabIndex="9700">
                        <Template>
                            <px:PXCheckBox ID="edCashAdvanceRequestApproval" runat="server" AlreadyLocalized="False" DataField="CashAdvanceRequestApproval" Text="Require Approval for Cash Advance" IsClientControl="True">
                            </px:PXCheckBox>
                        </Template>
                    </px:PXFormView>

                    <px:PXGrid ID="PXGrid1" runat="server" DataSourceID="ds" Height="300px" SkinID="DetailsInTab" TabIndex="8400" Width="550px">
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
                            <px:PXGridLevel DataMember="CashAdvanceApproval">
                                <RowTemplate>
                                    <px:PXSelector ID="edAssignmentMapID" runat="server" DataField="AssignmentMapID" AllowEdit="True" AutoRefresh="True" edit="1" TextField="Name">
                                    </px:PXSelector>
                                    <px:PXSelector ID="edAssignmentNotificationID" runat="server" DataField="AssignmentNotificationID">
                                    </px:PXSelector>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="AssignmentMapID" TextAlign="Left" Width="220px" TextField="AssignmentMapID_EPAssignmentMap_Name">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="AssignmentNotificationID" Width="280px">
                                    </px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                    </px:PXGrid>

                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Employees">
                <Template>
                    <px:PXGrid ID="PXGrid3" runat="server" DataSourceID="ds" TabIndex="4100" SkinID="DetailsInTab" AutoAdjustColumns="true" Width="100%">
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
                            <px:PXGridLevel DataMember="EmployeeCashAdvance">
                                <Columns>
                                    <px:PXGridColumn DataField="AcctCD" />
                                    <px:PXGridColumn DataField="AcctName" />
                                    <px:PXGridColumn DataField="UsrATPTEFMAllowableDays" TextAlign="Right" />
                                    <px:PXGridColumn DataField="UsrATPTEFMMaxCAAmt" TextAlign="Right" />
                                    <px:PXGridColumn DataField="UsrATPTEFMOpenCA" TextAlign="Right" />
                                    <px:PXGridColumn DataField="UsrATPTEFMCAUser" TextAlign="Center" Type="CheckBox" CommitChanges="true" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="0" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Migration Settings">
                <Template>
                    <px:PXLayoutRule runat="server" GroupCaption="Cash Fund Management" StartColumn="True" StartRow="True">
                    </px:PXLayoutRule>
                    <px:PXCheckBox ID="edIsCashAdvanceMigration" runat="server" AlreadyLocalized="False" DataField="IsCashAdvanceMigration" IsClientControl="True" Text="Cash Advance" CommitChanges="True" />
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="200" />
    </px:PXTab>
</asp:Content>

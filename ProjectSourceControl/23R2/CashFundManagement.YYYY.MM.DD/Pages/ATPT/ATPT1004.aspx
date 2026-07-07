<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="ATPT1004.aspx.cs" Inherits="Page_ATPT1004" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="CashFundManagement.BLC.ATPTEFMFeaturesMaint" PrimaryView="Setup">
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Setup" TabIndex="5700">
		<Template>
			<px:PXLayoutRule runat="server" StartRow="True" ControlSize="XL" GroupCaption="Budget Features" LabelsWidth="M" StartColumn="True"/>
		    <px:PXDropDown ID="edBudgetModules" runat="server" DataField="BudgetModules" AllowMultiSelect="True" CommitChanges="True">
            </px:PXDropDown>
            <px:PXDropDown ID="edBudgetFeatureSet" runat="server" DataField="BudgetFeatureSet" AllowMultiSelect="True" CommitChanges="True">
            </px:PXDropDown>
            <px:PXLabel ID="LabelSpace1" runat="server" AlreadyLocalized="False"></px:PXLabel>
            <px:PXDropDown ID="edBudgetDocumentAmount" runat="server" DataField="BudgetDocumentAmount">
            </px:PXDropDown>
            <px:PXDropDown ID="edBudgetRequestAmount" runat="server" DataField="BudgetRequestAmount">
            </px:PXDropDown>
            <px:PXDropDown ID="edBudgetBudgetAmount" runat="server" DataField="BudgetBudgetAmount">
            </px:PXDropDown>
            <px:PXDropDown ID="edBudgetSpentAmount" runat="server" DataField="BudgetSpentAmount">
            </px:PXDropDown>
            <px:PXDropDown ID="edBudgetReturnAmount" runat="server" DataField="BudgetReturnAmount">
            </px:PXDropDown>
            <px:PXDropDown ID="edBudgetApprovedAmount" runat="server" DataField="BudgetApprovedAmount">
            </px:PXDropDown>
            <px:PXDropDown ID="edBudgetUnapprovedAmount" runat="server" DataField="BudgetUnapprovedAmount">
            </px:PXDropDown>
            <px:PXSelector ID="edBudgetLedgerID" runat="server" DataField="BudgetLedgerID" CommitChanges="True">
            </px:PXSelector>
            <px:PXDropDown ID="edBudgetCalculation" runat="server" DataField="BudgetCalculation">
            </px:PXDropDown>
            <px:PXDropDown ID="edBudgetValidation" runat="server" DataField="BudgetValidation">
            </px:PXDropDown>

            <px:PXLabel ID="LabelSpace2" runat="server" AlreadyLocalized="False"></px:PXLabel>
<%--            <px:PXLabel ID="LabelSubGroup1" runat="server" AlreadyLocalized="False">- Column Names -</px:PXLabel>

            <px:PXTextEdit ID="edBudgetDocumentAmountLabel" runat="server" AlreadyLocalized="False" DataField="BudgetDocumentAmountLabel">
            </px:PXTextEdit>
            <px:PXTextEdit ID="edBudgetRequestAmountLabel" runat="server" AlreadyLocalized="False" DataField="BudgetRequestAmountLabel">
            </px:PXTextEdit>
            <px:PXTextEdit ID="edBudgetBudgetAmountLabel" runat="server" AlreadyLocalized="False" DataField="BudgetBudgetAmountLabel">
            </px:PXTextEdit>
            <px:PXTextEdit ID="edBudgetSpentAmountLabel" runat="server" AlreadyLocalized="False" DataField="BudgetSpentAmountLabel">
            </px:PXTextEdit>
            <px:PXTextEdit ID="edBudgetApprovedAmountLabel" runat="server" AlreadyLocalized="False" DataField="BudgetApprovedAmountLabel">
            </px:PXTextEdit>
            <px:PXTextEdit ID="edBudgetUnapprovedAmountLabel" runat="server" AlreadyLocalized="False" DataField="BudgetUnapprovedAmountLabel">
            </px:PXTextEdit>--%>

            <px:PXLayoutRule runat="server" ControlSize="XL" GroupCaption="Project Budget Features" LabelsWidth="M" StartColumn="True"/>
		    <px:PXDropDown ID="edProjectBudgetModules" runat="server" DataField="ProjectBudgetModules" AllowMultiSelect="True" CommitChanges="True">
            </px:PXDropDown>
            <px:PXDropDown ID="edProjectBudgetFeatureSet" runat="server" DataField="ProjectBudgetFeatureSet" AllowMultiSelect="True" CommitChanges="True">
            </px:PXDropDown>
            <px:PXLabel ID="LabelSpace3" runat="server" AlreadyLocalized="False"></px:PXLabel>
            <px:PXDropDown ID="edProjectBudgetDocumentAmount" runat="server" DataField="ProjectBudgetDocumentAmount">
            </px:PXDropDown>
            <px:PXDropDown ID="edProjectBudgetRequestAmount" runat="server" DataField="ProjectBudgetRequestAmount">
            </px:PXDropDown>
            <px:PXDropDown ID="edProjectBudgetReturnAmount" runat="server" DataField="ProjectBudgetReturnAmount">
            </px:PXDropDown>
            <px:PXDropDown ID="edProjectBudgetBudgetAmount" runat="server" DataField="ProjectBudgetBudgetAmount">
            </px:PXDropDown>
            <px:PXDropDown ID="edProjectBudgetSpentAmount" runat="server" DataField="ProjectBudgetSpentAmount">
            </px:PXDropDown>
            <px:PXDropDown ID="edProjectBudgetApprovedAmount" runat="server" DataField="ProjectBudgetApprovedAmount">
            </px:PXDropDown>
            <px:PXDropDown ID="edProjectBudgetUnapprovedAmount" runat="server" DataField="ProjectBudgetUnapprovedAmount">
            </px:PXDropDown>
<%--            <px:PXDropDown ID="edProjectBudgetPOAmount" runat="server" DataField="ProjectBudgetPOAmount">
            </px:PXDropDown>--%>
            <px:PXSelector ID="edProjectBudgetLedgerID" runat="server" DataField="ProjectBudgetLedgerID" CommitChanges="True">
            </px:PXSelector>
            <px:PXDropDown ID="edProjectBudgetCalculation" runat="server" DataField="ProjectBudgetCalculation">
            </px:PXDropDown>
            <px:PXDropDown ID="edProjectBudgetValidation" runat="server" DataField="ProjectBudgetValidation">
            </px:PXDropDown>

		    <px:PXLabel ID="LabelSpace4" runat="server" AlreadyLocalized="False"></px:PXLabel>
<%--            <px:PXLabel ID="LabelSubGroup2" runat="server" AlreadyLocalized="False">- Column Names -</px:PXLabel>

		    <px:PXTextEdit ID="edProjectBudgetDocumentAmountLabel" runat="server" AlreadyLocalized="False" DataField="ProjectBudgetDocumentAmountLabel">
            </px:PXTextEdit>
            <px:PXTextEdit ID="edProjectBudgetRequestAmountLabel" runat="server" AlreadyLocalized="False" DataField="ProjectBudgetRequestAmountLabel">
            </px:PXTextEdit>--%>
<%--            <px:PXTextEdit ID="edProjectBudgetReturnAmountLabel" runat="server" AlreadyLocalized="False" DataField="ProjectBudgetReturnAmountLabel">
            </px:PXTextEdit>--%>
<%--            <px:PXTextEdit ID="edProjectBudgetBudgetAmountLabel" runat="server" AlreadyLocalized="False" DataField="ProjectBudgetBudgetAmountLabel">
            </px:PXTextEdit>
            <px:PXTextEdit ID="edProjectBudgetSpentAmountLabel" runat="server" AlreadyLocalized="False" DataField="ProjectBudgetSpentAmountLabel">
            </px:PXTextEdit>
            <px:PXTextEdit ID="edProjectBudgetApprovedAmountLabel" runat="server" AlreadyLocalized="False" DataField="ProjectBudgetApprovedAmountLabel">
            </px:PXTextEdit>
            <px:PXTextEdit ID="edProjectBudgetUnapprovedAmountLabel" runat="server" AlreadyLocalized="False" DataField="ProjectBudgetUnapprovedAmountLabel">
            </px:PXTextEdit>--%>
<%--            <px:PXTextEdit ID="edProjectBudgetPOAmountLabel" runat="server" AlreadyLocalized="False" DataField="ProjectBudgetPOAmountLabel">
            </px:PXTextEdit>--%>

		    <px:PXLayoutRule runat="server" ControlSize="L" GroupCaption="Transfer Asset" LabelsWidth="SM" StartRow="True" StartColumn="True">
            </px:PXLayoutRule>
            <px:PXDropDown ID="edTransferAssetCustodian" runat="server" DataField="TransferAssetCustodian">
            </px:PXDropDown>
            <px:PXDropDown ID="edTransferAssetBuilding" runat="server" DataField="TransferAssetBuilding">
            </px:PXDropDown>
            <px:PXDropDown ID="edTransferAssetFloor" runat="server" DataField="TransferAssetFloor">
            </px:PXDropDown>
            <px:PXDropDown ID="edTransferAssetRoom" runat="server" DataField="TransferAssetRoom">
            </px:PXDropDown>
		    <px:PXLayoutRule runat="server" ControlSize="L" LabelsWidth="SM" StartColumn="True" ColumnWidth="XXL">
            </px:PXLayoutRule>
            <px:PXLayoutRule runat="server" GroupCaption="Fund Transaction" StartGroup="True" ControlSize="L">
            </px:PXLayoutRule>
            <px:PXLayoutRule runat="server" ControlSize="XXL" SuppressLabel="True">
            </px:PXLayoutRule>
            <px:PXCheckBox ID="edLimitValidation" runat="server" AlreadyLocalized="False" CommitChanges="True" DataField="LimitValidation">
            </px:PXCheckBox>
		    <px:PXLayoutRule runat="server">
            </px:PXLayoutRule>
            <px:PXNumberEdit ID="edLimitValidationAmt" runat="server" AlreadyLocalized="False" CommitChanges="True" DataField="LimitValidationAmt" SuppressLabel="False" Size="SM" DefaultLocale="">
            </px:PXNumberEdit>
		    <px:PXLayoutRule runat="server" ControlSize="L" GroupCaption="Cash Advance" LabelsWidth="SM" StartGroup="True" SuppressLabel="True">
            </px:PXLayoutRule>
		    <px:PXCheckBox ID="edCashAdvanceOverride" runat="server" AlreadyLocalized="False" DataField="CashAdvanceOverride" Text="Override Receipts">
            </px:PXCheckBox>
		</Template>
		<AutoSize Container="Window" Enabled="True" MinHeight="200" />
	</px:PXFormView>
</asp:Content>

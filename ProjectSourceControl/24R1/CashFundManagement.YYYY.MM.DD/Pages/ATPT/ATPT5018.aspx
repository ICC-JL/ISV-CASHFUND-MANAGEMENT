<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="ATPT5018.aspx.cs" Inherits="Page_ATPT5018" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" 
        PrimaryView="Summary" TypeName="CashFundManagement.BLC.ATPTEFMProjectBudgetProcess">
		<CallbackCommands>
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
    <px:PXGrid ID="grid" runat="server" Height="400px" Width="100%" Style="z-index: 100" AutoAdjustColumns="True"
		AllowPaging="True" AllowSearch="True" AdjustPageSize="Auto" DataSourceID="ds" SkinID="PrimaryInquire" TabIndex="700"
        FastFilterFields="LedgerID_description,ProjectID_description,ProjectTaskID,CostCodeID_description,FinYear">
        <EmptyMsg ComboAddMessage="No records found.
        Try to change filter or modify parameters above to see records here." NamedComboMessage="No records found as &#39;{0}&#39;.
        Try to change filter or modify parameters above to see records here." NamedComboAddMessage="No records found as &#39;{0}&#39;.
        Try to change filter or modify parameters above to see records here." FilteredMessage="No records found.
        Try to change filter to see records here." FilteredAddMessage="No records found.
        Try to change filter to see records here." NamedFilteredMessage="No records found as &#39;{0}&#39;.
        Try to change filter to see records here." NamedFilteredAddMessage="No records found as &#39;{0}&#39;.
        Try to change filter to see records here." AnonFilteredMessage="No records found.
        Try to change filter to see records here." AnonFilteredAddMessage="No records found.
        Try to change filter to see records here."></EmptyMsg>
        <Levels>
        <px:PXGridLevel DataMember="Summary">
            <Columns>
                <px:PXGridColumn DataField="Selected" AllowCheckAll="True" TextAlign="Center" Type="CheckBox" Width="30px" />
                <px:PXGridColumn DataField="LedgerID" Width="120px" />
                <px:PXGridColumn DataField="FinYear" />
                <px:PXGridColumn DataField="ProjectID" />
                <px:PXGridColumn DataField="ProjectTaskID" />
                <px:PXGridColumn DataField="CostCodeID" />
                <px:PXGridColumn DataField="AccountGroupID" />
<%--                <px:PXGridColumn DataField="InventoryID" />--%>
                <px:PXGridColumn DataField="Description" />
                <px:PXGridColumn DataField="Amount" TextAlign="Right" Width="100px" />
            </Columns>
            </px:PXGridLevel>
        </Levels>
        <ActionBar PagerVisible="Bottom">
            <PagerSettings Mode="NumericCompact" />
            <CustomItems>
                <px:PXToolBarLabel Key="countWarn" Visible="False" SuppressHtmlEncoding="False">
                <ActionBar GroupIndex="0" Order="0" ToolBarVisible="Bottom"></ActionBar>
                </px:PXToolBarLabel>
            </CustomItems>
        </ActionBar>
	</px:PXGrid>
</asp:Content>

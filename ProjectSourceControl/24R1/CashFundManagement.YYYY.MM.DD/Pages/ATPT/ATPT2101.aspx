<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="ATPT2101.aspx.cs" Inherits="Page_ATPT2101" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="CashFundManagement.BLC.ATPTEFMCashAdvanceMaint" PrimaryView="Filter">
		<CallbackCommands>
			<px:PXDSCallbackCommand Name="Cancel" Visible="False">  </px:PXDSCallbackCommand>
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Filter" NoteField="" TabIndex="2700">
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="L" />
            <px:PXSelector CommitChanges="True" ID="edEmployee" runat="server" DataField="EmployeeID" AutoRefresh="true" />
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid ID="grid" runat="server" DataSourceID="ds" Height="150px" Style="z-index: 100" Width="100%" SkinID="PrimaryInquire" SyncPosition="True" KeepPosition="True" FastFilterFields="CashAdvanceNbr,Descr" TabIndex="5700">
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
			<px:PXGridLevel DataMember="CashAdvance" DataKeyNames="ReqClassID,CashAdvanceNbr">
				<Columns>
					<px:PXGridColumn DataField="ReqClassID" Width="120px" />
					<px:PXGridColumn DataField="CashAdvanceNbr" Width="140px" LinkCommand="EditDetail" />
					<px:PXGridColumn DataField="Status" />
					<px:PXGridColumn DataField="Date" Width="90px" />
					<px:PXGridColumn DataField="DateOfUse" Width="90px" />
					<px:PXGridColumn DataField="LiqDate" Width="90px" />
                    <px:PXGridColumn DataField="FinPeriodID" Width="72px" />
					<px:PXGridColumn DataField="Descr" Width="280px" />
					<px:PXGridColumn DataField="RequestedByID" Width="140px" />
					<px:PXGridColumn DataField="RequestedByID_description" Width="220px" />
					<px:PXGridColumn DataField="CuryRequestedAmount" Width="180px" />
					<px:PXGridColumn DataField="DepartmentID_EPDepartment_description" Width="220px" />
					<px:PXGridColumn DataField="CreatedByID" Width="280px" />
					<px:PXGridColumn DataField="CreatedDateTime" Width="280px" />
					<px:PXGridColumn DataField = "LastModifiedByID" Width = "250px" />
					<px:PXGridColumn DataField = "LastModifiedDateTime" Width = "250px" />
				</Columns>
				<Mode AllowUpdate="False" />
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
		<ActionBar DefaultAction="editDetail" />
	</px:PXGrid>
</asp:Content>

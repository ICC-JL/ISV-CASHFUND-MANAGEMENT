<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="ATPT5017.aspx.cs" Inherits="Page_ATPT5017" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="Summary" TypeName="CashFundManagement.BLC.ATPTEFMFundRequestReclassifyProcess" >
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
    <px:PXGrid ID="grid" runat="server" Height="400px" Width="100%" Style="z-index: 100"
		AllowPaging="True" AllowSearch="True" AdjustPageSize="Auto" DataSourceID="ds" SkinID="Primary" TabIndex="100">
		<Levels>
			<px:PXGridLevel DataKeyNames="RefNbr" DataMember="Summary">
			    <Columns>
                    <px:PXGridColumn DataField="Selected" TextAlign="Center" Type="CheckBox" Width="60px">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="RefNbr" Width="140px">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="RequestedByID" Width="280px">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="ReclassifyBalanceAmt" Width="280px">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="LiqDate" Width="100px" TextAlign="Right">
                    </px:PXGridColumn>
                </Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="200" />
	</px:PXGrid>
</asp:Content>
<asp:Content ID="contDialogs" ContentPlaceHolderID="phDialogs" runat="Server">
    <px:PXSmartPanel ID="panel" runat="server" CaptionVisible="True" Caption="Days Extension" LoadOnDemand="True" Key="ExtendFilter" DesignView="Content" AlreadyLocalized="False" AutoRepaint="True" CreateOnDemand="True" TabIndex="400">
        <px:PXFormView ID="formExtend" runat="server" SkinID="Transparent" DataMember="ExtendFilter" DataSourceID="ds" TabIndex="22700">
            <Template>
                <px:PXLayoutRule runat="server" StartRow="True">
                </px:PXLayoutRule>
                <px:PXNumberEdit ID="edDays" runat="server" AlreadyLocalized="False" DataField="Days" DefaultLocale="" CommitChanges="true">
                </px:PXNumberEdit>

                <px:PXPanel ID="PXPanel1" runat="server" AlreadyLocalized="False" SkinID="Buttons" ValidateRequestMode="Inherit">
                    <px:PXButton ID="btnMyCommandOK" runat="server" AlreadyLocalized="False" DialogResult="OK" Text="OK" ValidateRequestMode="Inherit" />
                    <px:PXButton ID="btnMyCommandCancel" runat="server" AlreadyLocalized="False" DialogResult="Cancel" Text="Cancel" ValidateRequestMode="Inherit" />
                </px:PXPanel>
            </Template>
        </px:PXFormView>
    </px:PXSmartPanel>
</asp:Content>
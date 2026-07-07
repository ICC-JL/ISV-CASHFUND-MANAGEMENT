<%@ Page Language="C#" MasterPageFile="~/MasterPages/TabView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="ATPT9202.aspx.cs" Inherits="Page_ATPT9202" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="Preference" TypeName="CashFundManagement.BLC.ATPTEFM2023EnhancementsPreference">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXLayoutRule runat="server" StartColumn="True" ControlSize="M">
    </px:PXLayoutRule>
    <px:PXFormView ID="PXFormView5" runat="server" SkinID="Transparent" DataMember="Preference" DataSourceID="ds" TabIndex="1700">
        <Template>
            <px:PXLayoutRule runat="server" StartRow="True" GroupCaption="ENABLE ENHANCEMENTS" LabelsWidth="M" />
            <px:PXCheckBox ID="edCloseFundWithoutReplenishment" runat="server" AlignLeft="True" AlreadyLocalized="False" CommitChanges="True" DataField="CloseFundWithoutReplenishment" IsClientControl="True">
            </px:PXCheckBox>
            <px:PXCheckBox ID="edIncreaseDecreaseFund" runat="server" AlignLeft="True" AlreadyLocalized="False" CommitChanges="True" DataField="IncreaseDecreaseFund" IsClientControl="True">
            </px:PXCheckBox>

            <%--<px:PXLayoutRule runat="server" StartRow="True" GroupCaption="INCREASE / DECREASE FUND SETTINGS" LabelsWidth="M" />
            <px:PXCheckBox ID="edRequireApprovalOnFundIncreaseCredAdj" runat="server" AlignLeft="True" AlreadyLocalized="False" CommitChanges="True" DataField="RequireApprovalOnFundIncreaseCredAdj" IsClientControl="True">
            </px:PXCheckBox>
            <px:PXCheckBox ID="edRequireApprovalOnFundDecreaseDebAdj" runat="server" AlignLeft="True" AlreadyLocalized="False" CommitChanges="True" DataField="RequireApprovalOnFundDecreaseDebAdj" IsClientControl="True">
            </px:PXCheckBox>--%>

        </Template>
    </px:PXFormView>
</asp:Content>

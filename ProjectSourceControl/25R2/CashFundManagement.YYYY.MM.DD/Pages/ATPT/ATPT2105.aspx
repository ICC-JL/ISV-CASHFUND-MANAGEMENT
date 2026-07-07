<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="ATPT2105.aspx.cs" Inherits="Page_ATPT2105" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="ReqClasses" TypeName="CashFundManagement.BLC.ATPTEFMReqClassMaint">
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="ReqClasses" TabIndex="100" DefaultControlID="edTranType">
        <CallbackCommands>
            <Save PostData="Self" />
        </CallbackCommands>
        <Template>
            <px:PXLayoutRule runat="server" ControlSize="M" LabelsWidth="SM" StartColumn="True" />
            <px:PXDropDown ID="edTranType" runat="server" CommitChanges="True" DataField="TranType">
                <AutoCallBack Command="Cancel" Target="ds"></AutoCallBack>
            </px:PXDropDown>
            <px:PXSelector ID="edReqClassID" runat="server" AutoRefresh="True" CommitChanges="True" DataField="ReqClassID">
                <AutoCallBack Command="Cancel" Target="ds"></AutoCallBack>
            </px:PXSelector>
            <px:PXSelector ID="edNumberingID" runat="server" AllowEdit="True" DataField="NumberingID">
            </px:PXSelector>
            <px:PXLayoutRule runat="server" ColumnSpan="2">
            </px:PXLayoutRule>
            <px:PXTextEdit ID="edDescr" runat="server" AlreadyLocalized="False" DataField="Descr" IsClientControl="True">
            </px:PXTextEdit>
            <px:PXLayoutRule runat="server" ControlSize="M" LabelsWidth="SM" StartColumn="True">
            </px:PXLayoutRule>
            <px:PXCheckBox ID="edRestrictItemList" runat="server" AlreadyLocalized="False" DataField="RestrictItemList" IsClientControl="True" Text="Restrict Item List">
            </px:PXCheckBox>
            <px:PXCheckBox ID="edEnableDocumentOverride" runat="server" AlreadyLocalized="False" DataField="EnableDocumentOverride" IsClientControl="True" Text="Enable Document Override">
            </px:PXCheckBox>
            <px:PXCheckBox ID="edRestrictMultInvIns" runat="server" AlreadyLocalized="False" DataField="RestrictMultInvIns" IsClientControl="True" Text="Restrict Multiple Inventory Instance">
            </px:PXCheckBox>
            <px:PXLayoutRule runat="server" ControlSize="M" LabelsWidth="SM" StartColumn="True">
            </px:PXLayoutRule>
            <px:PXNumberEdit ID="edNoDaysLiquidate" runat="server" AlreadyLocalized="False" DataField="NoDaysLiquidate" IsClientControl="True">
            </px:PXNumberEdit>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" DataMember="CurrentReqClass">
        <Items>
            <px:PXTabItem Text="Item List">
                <Template>
                    <px:PXGrid ID="PXGrid1" runat="server" DataSourceID="ds" SkinID="DetailsInTab" TabIndex="1700">
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
                            <px:PXGridLevel DataKeyNames="ReqClassItemsID" DataMember="Items">
                                <RowTemplate>
                                    <px:PXNumberEdit ID="edAmount" runat="server" AlreadyLocalized="False" CommitChanges="True" DataField="Amount" Enabled="False" IsClientControl="True">
                                    </px:PXNumberEdit>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="InventoryID">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="InventoryID_InventoryItem_descr" Width="280px">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="Amount" TextAlign="Right" CommitChanges="True">
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="IsPerDiem" CommitChanges="True" Type="CheckBox" TextAlign="Center">
                                    </px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" />
                        <Mode AllowUpload="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="GL Accounts">
                <Template>
                    <px:PXLayoutRule runat="server" ControlSize="M" LabelsWidth="M" StartColumn="True">
                    </px:PXLayoutRule>
                    <px:PXDropDown ID="edUseExpenseAcctFrom" runat="server" DataField="UseExpenseAcctFrom" IsClientControl="True">
                    </px:PXDropDown>
                    <px:PXSegmentMask ID="edCombineExpSub" runat="server" DataField="CombineExpSub">
                    </px:PXSegmentMask>
                    <px:PXSegmentMask ID="edExpenseAcctID" runat="server" CommitChanges="True" DataField="ExpenseAcctID">
                    </px:PXSegmentMask>
                    <px:PXSegmentMask ID="edExpenseSubID" runat="server" DataField="ExpenseSubID">
                    </px:PXSegmentMask>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
    </px:PXTab>
</asp:Content>

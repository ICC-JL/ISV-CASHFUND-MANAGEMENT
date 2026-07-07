<%@ Page Language="C#" MasterPageFile="~/MasterPages/TabView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="ATPT1005.aspx.cs" Inherits="Page_ATPT1005" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:content id="cont1" contentplaceholderid="phDS" runat="Server">
    <px:pxdatasource id="ds" runat="server" visible="True" width="100%" primaryview="Preference" typename="CashFundManagement.BLC.ATPTEFMFundTransactionPreference">
        <callbackcommands>
            <px:PXDSCallbackCommand Name="ViewAssignmentMap" Visible="False" DependOnGrid="gridFundsRequestApproval" />
            <px:PXDSCallbackCommand Name="ViewAssignmentMap" Visible="False" DependOnGrid="gridFundTransactionRequestApproval" />
            <px:PXDSCallbackCommand Name="ViewAssignmentMap" Visible="False" DependOnGrid="gridReplenishmentRequestApproval" />
            <px:PXDSCallbackCommand Name="ViewAssignmentMap" Visible="False" DependOnGrid="gridMonthEndRequestApproval" />
        </callbackcommands>
    </px:pxdatasource>
</asp:content>
<asp:content id="cont2" contentplaceholderid="phF" runat="Server">
    <px:pxtab id="tab" runat="server" datasourceid="ds" height="150px" style="z-index: 100" width="100%" datamember="Preference">
        <items>
            <px:pxtabitem text="General Settings">
                <template>
                    <px:pxlayoutrule runat="server" startcolumn="True">
                    </px:pxlayoutrule>

                    <px:pxformview id="PXFormView5" runat="server" skinid="Transparent" datamember="Preference" datasourceid="ds" tabindex="1700">
                        <template>
                            <px:pxlayoutrule runat="server" groupcaption="Numbering Settings" startcolumn="True" labelswidth="M">
                            </px:pxlayoutrule>
                            <px:pxselector id="edFundNumberingID" runat="server" datafield="FundNumberingID" allowedit="True" edit="1">
                            </px:pxselector>
                            <px:pxselector id="edFundTransactionNumberingID" runat="server" datafield="FundTransactionNumberingID" allowedit="True" edit="1">
                            </px:pxselector>
                            <px:pxselector id="edReplenishmentNumberingID" runat="server" datafield="ReplenishmentNumberingID" allowedit="True" edit="1">
                            </px:pxselector>
                            <px:pxselector id="edMonthEndNumberingID" runat="server" datafield="MonthEndNumberingID" allowedit="True" edit="1">
                            </px:pxselector>
                        </template>
                    </px:pxformview>

                    <px:pxformview id="PXFormView3" runat="server" skinid="Transparent" datamember="Preference" datasourceid="ds" tabindex="1700">
                        <template>
                            <px:pxlayoutrule runat="server" groupcaption="Default Account Settings" labelswidth="M">
                            </px:pxlayoutrule>
                            <px:pxdropdown id="edReplenismentLimit" runat="server" datafield="ReplenishmentLimit" isclientcontrol="True">
                            </px:pxdropdown>
                            <px:pxdropdown id="edFundTransactionLimit" runat="server" datafield="FundTransactionLimit" isclientcontrol="True">
                            </px:pxdropdown>
                            <px:pxselector id="edPCFAccount" runat="server" datafield="PCFAccount" allowedit="True" edit="1">
                            </px:pxselector>
                            <px:pxselector id="edPCFSubaccount" runat="server" datafield="PCFSubaccount" allowedit="True" edit="1">
                            </px:pxselector>
                            <px:pxselector id="edRVFAccount" runat="server" datafield="RVFAccount" allowedit="True" edit="1">
                            </px:pxselector>
                            <px:pxselector id="edRVFSubaccount" runat="server" datafield="RVFSubaccount" allowedit="True" edit="1">
                            </px:pxselector>
                            <px:pxselector id="edClearingAccount" runat="server" datafield="ClearingAccount" allowedit="True" edit="1">
                            </px:pxselector>
                            <px:pxselector id="edClearingSubaccount" runat="server" datafield="ClearingSubaccount" allowedit="True" edit="1">
                            </px:pxselector>
                            <px:pxlayoutrule runat="server" startrow="True" groupcaption="LIQUIDATION SETTINGS" labelswidth="M" />
                            <px:pxnumberedit id="edNoOfDaysToLiquidate" runat="server" datafield="NoOfDaysToLiquidate" commitchanges="True">
                            </px:pxnumberedit>
                            <px:pxselector id="edReclassificationItem" runat="server" autorefresh="True" datafield="ReclassificationItem" commitchanges="True" allowedit="True">
                            </px:pxselector>
                            <px:pxcheckbox id="edLiquidationDateBasedOnWorkCalendar" runat="server" alreadylocalized="False" datafield="LiquidationDateBasedOnWorkCalendar" isclientcontrol="True" text="Liquidation Date based on Work Calendar">
                            </px:pxcheckbox>
                        </template>
                    </px:pxformview>

                    <px:pxlayoutrule runat="server" startcolumn="True" controlsize="M">
                    </px:pxlayoutrule>
                    <px:pxformview id="frmApproval" runat="server" datamember="Preference" datasourceid="ds" skinid="Transparent" tabindex="4300">

                        <template>
                            <px:pxlayoutrule runat="server" groupcaption="Approval Settings">
                            </px:pxlayoutrule>
                            <px:pxcheckbox id="edFundsApprovalSetup" runat="server" alreadylocalized="False" datafield="FundsApprovalSetup" isclientcontrol="True" alignleft="True" text="Require Approval for Funds">
                            </px:pxcheckbox>
                            <px:pxcheckbox id="edFundTransactionRequestApproval" runat="server" alreadylocalized="False" datafield="FundTransactionRequestApproval" isclientcontrol="True" alignleft="True" text="Require Approval for Fund Transaction">
                            </px:pxcheckbox>
                            <px:pxcheckbox id="edReplenishmentRequestApproval" runat="server" alreadylocalized="False" datafield="ReplenishmentRequestApproval" isclientcontrol="True" alignleft="True" text="Require Approval for Replenishment">
                            </px:pxcheckbox>
                            <px:pxcheckbox id="edMonthEndRequestApproval" runat="server" alreadylocalized="False" datafield="MonthEndRequestApproval" isclientcontrol="True" alignleft="True">
                            </px:pxcheckbox>
                        </template>
                    </px:pxformview>
                    <px:pxformview id="frmOther" runat="server" datamember="Preference" datasourceid="ds" tabindex="800" skinid="Transparent">

                        <template>
                            <px:pxlayoutrule runat="server" groupcaption="Other Settings" startcolumn="True">
                            </px:pxlayoutrule>
                            <px:pxcheckbox id="edIsFilterByEmployeeDelegates" runat="server" alignleft="True" alreadylocalized="False" commitchanges="True" datafield="IsFilterByEmployeeDelegates" isclientcontrol="True" text="Filtered View">
                            </px:pxcheckbox>
                            <px:pxcheckbox id="edAllowManualReceipts" runat="server" alignleft="True" alreadylocalized="False" commitchanges="True" datafield="AllowManualReceipts" isclientcontrol="True">
                            </px:pxcheckbox>
                            <px:pxcheckbox id="edIsRequireApprovalReplenishment" runat="server" alignleft="True" alreadylocalized="False" commitchanges="True" datafield="IsRequireApprovalReplenishment" isclientcontrol="True">
                            </px:pxcheckbox>
                            <px:pxcheckbox id="edIsRequireApprovalOnFundEstablishment" runat="server" alignleft="True" alreadylocalized="False" commitchanges="True" datafield="IsRequireApprovalOnFundEstablishment" isclientcontrol="True">
                            </px:pxcheckbox>
                            <px:pxcheckbox id="edRequireVendorDetails" runat="server" alignleft="True" alreadylocalized="False" commitchanges="True" datafield="RequireVendorDetails" isclientcontrol="True">
                            </px:pxcheckbox>
                            <px:pxcheckbox id="edRequireExternalReferenceNbr" alignleft="True" runat="server" alreadylocalized="False" datafield="RequireExternalReferenceNbr" isclientcontrol="True">
                            </px:pxcheckbox>
                            <px:pxcheckbox id="edRequireApprovalOnFundIncreaseCredAdj" runat="server" alignleft="True" alreadylocalized="False" commitchanges="True" datafield="RequireApprovalOnFundIncreaseCredAdj" isclientcontrol="True">
                            </px:pxcheckbox>
                            <px:pxcheckbox id="edRequireApprovalOnFundDecreaseDebAdj" runat="server" alignleft="True" alreadylocalized="False" commitchanges="True" datafield="RequireApprovalOnFundDecreaseDebAdj" isclientcontrol="True">
                            </px:pxcheckbox>
                            <px:pxcheckbox id="edAutoReleaseMonthEndJournal" runat="server" alignleft="True" alreadylocalized="False" commitchanges="True" datafield="AutoReleaseMonthEndJournal" isclientcontrol="True">
                            </px:pxcheckbox>
                            <px:pxcheckbox id="edValidateAmountReceivedAndReleasedUponLiquidation" runat="server" alignleft="True" alreadylocalized="False" commitchanges="True" datafield="ValidateAmountReceivedAndReleasedUponLiquidation" isclientcontrol="True">
                            </px:pxcheckbox>
                            <px:pxcheckbox id="edEnableFundTransactionLimit" runat="server" alignleft="True" alreadylocalized="False" commitchanges="True" datafield="EnableFundTransactionLimit" isclientcontrol="True">
                            </px:pxcheckbox>
                            <px:pxcheckbox id="edSetNonStockItemDescriptionAsDefault" runat="server" alignleft="True" alreadylocalized="False" commitchanges="True" datafield="SetNonStockItemDescriptionAsDefault" isclientcontrol="True">
                            </px:pxcheckbox>
                            <px:pxcheckbox id="edRestrictCustodianByBranch" runat="server" alignleft="True" alreadylocalized="False" commitchanges="True" datafield="RestrictCustodianByBranch" isclientcontrol="True">
                            </px:pxcheckbox>
                        </template>
                    </px:pxformview>
                </template>
            </px:pxtabitem>
            <px:pxtabitem text="GL ACCOUNTS">
                <template>
                    <px:pxlayoutrule runat="server" controlsize="M" labelswidth="M" startcolumn="True">
                    </px:pxlayoutrule>
                    <px:pxdropdown id="edUseExpenseAcctFrom" runat="server" datafield="UseExpenseAcctFrom" isclientcontrol="True">
                    </px:pxdropdown>
                    <px:pxsegmentmask id="edCombineExpSub" runat="server" datafield="CombineExpSub">
                    </px:pxsegmentmask>
                </template>
            </px:pxtabitem>
            <px:pxtabitem text="Migration Settings">
                <template>
                    <px:pxlayoutrule runat="server" groupcaption="Cash Fund Management" startcolumn="True" startrow="True">
                    </px:pxlayoutrule>
                    <px:pxcheckbox id="edIsFundsMigration" runat="server" alreadylocalized="False" datafield="IsFundsMigration" isclientcontrol="True" commitchanges="True" />
                </template>
            </px:pxtabitem>
            <px:pxtabitem text="Approval">
                <template>
                    <px:pxformview id="frmModule" runat="server" datamember="Preference" datasourceid="ds" skinid="Transparent" width="100%">
                        <template>
                            <px:pxlayoutrule runat="server" controlsize="XM" labelswidth="XS">
                            </px:pxlayoutrule>
                            <px:pxdropdown id="edApprovalModule" runat="server" commitchanges="True" datafield="ApprovalModule" isclientcontrol="True">
                            </px:pxdropdown>
                        </template>
                    </px:pxformview>
                    <px:pxgrid id="gridFundsRequestApproval" runat="server" datasourceid="ds" skinid="DetailsInTab" height="300px" tabindex="2300" width="100%" style="left: 0px; top: 0px">
                        <emptymsg anonfilteredaddmessage="No records found.
Try to change filter to see records here."
                            anonfilteredmessage="No records found.
Try to change filter to see records here."
                            comboaddmessage="No records found.
Try to change filter or modify parameters above to see records here."
                            filteredaddmessage="No records found.
Try to change filter to see records here."
                            filteredmessage="No records found.
Try to change filter to see records here."
                            namedcomboaddmessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            namedcombomessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            namedfilteredaddmessage="No records found as '{0}'.
Try to change filter to see records here."
                            namedfilteredmessage="No records found as '{0}'.
Try to change filter to see records here." />
                        <levels>
                            <px:pxgridlevel datamember="FundsApproval">
                                <rowtemplate>
                                    <px:pxselector id="edMEAssignmentMapID" runat="server" allowedit="True" autorefresh="True" datafield="AssignmentMapID" textfield="Name" edit="1">
                                    </px:pxselector>
                                </rowtemplate>
                                <columns>
                                    <px:pxgridcolumn datafield="IsActive" width="60px" textalign="Center" type="CheckBox">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="FundType" width="280px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AssignmentMapID" width="180px" textalign="Left" textfield="AssignmentMapID_EPAssignmentMap_Name" commitchanges="true" LinkCommand="ViewAssignmentMap">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AssignmentNotificationID" width="280px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ModuleType" width="280px">
                                    </px:pxgridcolumn>
                                </columns>
                            </px:pxgridlevel>
                        </levels>
                    </px:pxgrid>
                    <px:pxgrid id="gridFundTransactionRequestApproval" runat="server" datasourceid="ds" skinid="DetailsInTab" SyncPosition="True"  width="100%" height="300px">
                        <emptymsg anonfilteredaddmessage="No records found.
Try to change filter to see records here."
                            anonfilteredmessage="No records found.
Try to change filter to see records here."
                            comboaddmessage="No records found.
Try to change filter or modify parameters above to see records here."
                            filteredaddmessage="No records found.
Try to change filter to see records here."
                            filteredmessage="No records found.
Try to change filter to see records here."
                            namedcomboaddmessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            namedcombomessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            namedfilteredaddmessage="No records found as '{0}'.
Try to change filter to see records here."
                            namedfilteredmessage="No records found as '{0}'.
Try to change filter to see records here." />
                        <levels>
                            <px:pxgridlevel datamember="FundTransactionApproval">
                                <rowtemplate>
                                    <px:pxselector id="edFTAssignmentMapID" runat="server" allowedit="True" autorefresh="True" datafield="AssignmentMapID" textfield="Name" edit="1">
                                    </px:pxselector>
                                </rowtemplate>
                                <columns>
                                    <px:pxgridcolumn datafield="IsActive" width="60px" textalign="Center" type="CheckBox">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="FundTransactionType" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AssignmentMapID" width="180px" textalign="Left" textfield="AssignmentMapID_EPAssignmentMap_Name" commitchanges="True" LinkCommand="ViewAssignmentMap">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AssignmentNotificationID" width="280px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ModuleType" width="280px">
                                    </px:pxgridcolumn>
                                </columns>
                            </px:pxgridlevel>
                        </levels>
                    </px:pxgrid>
                    <px:pxgrid id="gridReplenishmentRequestApproval" runat="server" datasourceid="ds" skinid="DetailsInTab" SyncPosition="True" width="100%" height="300px">
                        <emptymsg anonfilteredaddmessage="No records found.
Try to change filter to see records here."
                            anonfilteredmessage="No records found.
Try to change filter to see records here."
                            comboaddmessage="No records found.
Try to change filter or modify parameters above to see records here."
                            filteredaddmessage="No records found.
Try to change filter to see records here."
                            filteredmessage="No records found.
Try to change filter to see records here."
                            namedcomboaddmessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            namedcombomessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            namedfilteredaddmessage="No records found as '{0}'.
Try to change filter to see records here."
                            namedfilteredmessage="No records found as '{0}'.
Try to change filter to see records here." />
                        <levels>
                            <px:pxgridlevel datamember="ReplenishmentApproval">
                                <rowtemplate>
                                    <px:pxselector id="edREAssignmentMapID" runat="server" allowedit="True" autorefresh="True" datafield="AssignmentMapID" textfield="Name" edit="1">
                                    </px:pxselector>
                                </rowtemplate>
                                <columns>
                                    <px:pxgridcolumn datafield="IsActive" width="60px" textalign="Center" type="CheckBox">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AssignmentMapID" width="180px" textalign="Left" textfield="AssignmentMapID_EPAssignmentMap_Name" commitchanges="true" LinkCommand="ViewAssignmentMap">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AssignmentNotificationID" width="280px">
                                    </px:pxgridcolumn>
                                </columns>
                            </px:pxgridlevel>
                        </levels>
                    </px:pxgrid>
                    <px:pxgrid id="gridMonthEndRequestApproval" runat="server" datasourceid="ds" skinid="DetailsInTab" SyncPosition="True" width="100%" height="300px">
                        <emptymsg anonfilteredaddmessage="No records found.
Try to change filter to see records here."
                            anonfilteredmessage="No records found.
Try to change filter to see records here."
                            comboaddmessage="No records found.
Try to change filter or modify parameters above to see records here."
                            filteredaddmessage="No records found.
Try to change filter to see records here."
                            filteredmessage="No records found.
Try to change filter to see records here."
                            namedcomboaddmessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            namedcombomessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            namedfilteredaddmessage="No records found as '{0}'.
Try to change filter to see records here."
                            namedfilteredmessage="No records found as '{0}'.
Try to change filter to see records here." />
                        <levels>
                            <px:pxgridlevel datamember="MonthEndApproval">
                                <rowtemplate>
                                    <px:pxselector id="edMETAssignmentMapID" runat="server" allowedit="True" autorefresh="True" datafield="AssignmentMapID" textfield="Name" edit="1">
                                    </px:pxselector>
                                </rowtemplate>
                                <columns>
                                    <px:pxgridcolumn datafield="IsActive" width="60px" textalign="Center" type="CheckBox">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AssignmentMapID" width="180px" textalign="Left" textfield="AssignmentMapID_EPAssignmentMap_Name" commitchanges="true" LinkCommand="ViewAssignmentMap">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AssignmentNotificationID" width="280px">
                                    </px:pxgridcolumn>
                                </columns>
                            </px:pxgridlevel>
                        </levels>
                    </px:pxgrid>
                </template>
            </px:pxtabitem>
            <px:pxtabitem text="Replenishment Report Settings">
                <template>
                    <px:pxformview id="PXFormView4" runat="server" datamember="Preference" datasourceid="ds" tabindex="2500">
                        <template>
                            <px:pxdropdown id="edFundType" runat="server" datafield="FundType" isclientcontrol="True" labelwidth="150px">
                            </px:pxdropdown>
                        </template>
                    </px:pxformview>

                    <px:pxgrid id="PXGrid2" runat="server" datasourceid="ds" skinid="DetailsInTab" height="300px" tabindex="2300" width="560px" style="left: 0px; top: 0px">
                        <emptymsg anonfilteredaddmessage="No records found.
Try to change filter to see records here."
                            anonfilteredmessage="No records found.
Try to change filter to see records here."
                            comboaddmessage="No records found.
Try to change filter or modify parameters above to see records here."
                            filteredaddmessage="No records found.
Try to change filter to see records here."
                            filteredmessage="No records found.
Try to change filter to see records here."
                            namedcomboaddmessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            namedcombomessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            namedfilteredaddmessage="No records found as '{0}'.
Try to change filter to see records here."
                            namedfilteredmessage="No records found as '{0}'.
Try to change filter to see records here." />
                        <levels>
                            <px:pxgridlevel datamember="ReplenishmentReport">
                                <rowtemplate>


                                    <px:pxselector id="edItemClass" runat="server" datafield="ItemClass">
                                    </px:pxselector>
                                    <px:pxnumberedit id="edReplenishmentID" runat="server" alreadylocalized="False" datafield="ReplenishmentID" isclientcontrol="True" smartsuggestionmode="">
                                    </px:pxnumberedit>
                                </rowtemplate>
                                <columns>

                                    <px:pxgridcolumn datafield="IsActive" textalign="Center" type="CheckBox" width="60px">
                                    </px:pxgridcolumn>

                                    <px:pxgridcolumn datafield="ItemClass" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Descr">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ReplenishmentID" textalign="Right">
                                    </px:pxgridcolumn>
                                </columns>
                            </px:pxgridlevel>
                        </levels>
                    </px:pxgrid>
                </template>
            </px:pxtabitem>
        </items>
        <autosize container="Window" enabled="True" minheight="200" />
    </px:pxtab>
</asp:content>

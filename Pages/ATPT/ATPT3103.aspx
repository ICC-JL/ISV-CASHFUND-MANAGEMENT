<%@ page language="C#" masterpagefile="~/MasterPages/FormTab.master" autoeventwireup="true"
    validaterequest="false" codefile="ATPT3103.aspx.cs" inherits="Page_ATPT3103" title="Untitled Page" %>

<%@ mastertype virtualpath="~/MasterPages/FormTab.master" %>

<asp:content id="cont1" contentplaceholderid="phDS" runat="Server">
    <px:pxdatasource id="ds" runat="server" visible="True" width="100%" primaryview="CashAdvances" typename="CashFundManagement.BLC.ATPTEFMCashAdvanceEntry" enableattributes="true">
        <callbackcommands>
            <px:pxdscallbackcommand name="loadRequest" visible="False">
            </px:pxdscallbackcommand>
            <px:pxdscallbackcommand name="openTransaction" visible="False" dependongrid="PXGrid2">
            </px:pxdscallbackcommand>
            <px:pxdscallbackcommand name="openExpClaim" visible="False" dependongrid="PXGrid2">
            </px:pxdscallbackcommand>
            <px:pxdscallbackcommand name="submitReceipt" visible="False">
            </px:pxdscallbackcommand>
            <px:pxdscallbackcommand name="cancelSubmitReceipt" visible="False">
            </px:pxdscallbackcommand>
            <px:pxdscallbackcommand name="CurrencyView" visible="False">
            </px:pxdscallbackcommand>
            <px:pxdscallbackcommand name="viewVoidCheck" visible="False" dependongrid="gridVoided">
            </px:pxdscallbackcommand>
        </callbackcommands>
    </px:pxdatasource>
</asp:content>
<asp:content id="cont2" contentplaceholderid="phF" runat="Server">
    <px:pxformview id="form" runat="server" datasourceid="ds" style="z-index: 100" width="100%" datamember="CashAdvances" tabindex="100">
        <template>
            <px:pxlayoutrule runat="server" startcolumn="True" controlsize="SM" />
            <px:pxselector id="edReqClassID" runat="server" commitchanges="True" datafield="ReqClassID" autorefresh="True">
                <gridproperties fastfilterfields="ReqClassID,Descr">
                </gridproperties>
                <autocallback command="Cancel" target="ds"></autocallback>
            </px:pxselector>
            <px:pxselector id="edCashAdvanceNbr" runat="server" datafield="CashAdvanceNbr" autorefresh="True">
                <autocallback command="Cancel" target="ds"></autocallback>
            </px:pxselector>
            <px:pxdropdown id="edStatus" runat="server" datafield="Status" isclientcontrol="True">
            </px:pxdropdown>
            <px:pxcheckbox id="edHold" runat="server" alreadylocalized="False" datafield="Hold" text="Hold" commitchanges="True" isclientcontrol="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edApproved" runat="server" alreadylocalized="False" datafield="Approved" text="Approved" isclientcontrol="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edReclassified" runat="server" alreadylocalized="False" datafield="Reclassified" text="Reclassified" isclientcontrol="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edIsOverbudget" runat="server" alreadylocalized="False" datafield="IsOverbudget" commitchanges="True" isclientcontrol="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edHasInitialBudget" runat="server" alreadylocalized="False" datafield="HasInitialBudget" commitchanges="True" isclientcontrol="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edIsImported" runat="server" alreadylocalized="False" commitchanges="True" datafield="IsImported" isclientcontrol="True" text="Imported">
            </px:pxcheckbox>
            <px:pxdatetimeedit id="edDate" runat="server" alreadylocalized="False" datafield="Date" commitchanges="True" defaultlocale="" isclientcontrol="True">
            </px:pxdatetimeedit>
            <px:pxmaskedit id="edFinPeriodID" runat="server" alreadylocalized="False" commitchanges="True" datafield="FinPeriodID" isclientcontrol="True">
            </px:pxmaskedit>
            <px:pxcheckbox id="edRequestApproval" runat="server" alreadylocalized="False" datafield="RequestApproval" text="Request Approval" isclientcontrol="True">
            </px:pxcheckbox>
            <px:pxlayoutrule runat="server" columnspan="2">
            </px:pxlayoutrule>
            <px:pxtextedit id="edDescr" runat="server" alreadylocalized="False" datafield="Descr" isclientcontrol="True">
            </px:pxtextedit>
            <px:pxlayoutrule runat="server" startcolumn="True" labelswidth="SM">
            </px:pxlayoutrule>
            <px:pxsegmentmask commitchanges="True" id="edBranchID" runat="server" datafield="BranchID"></px:pxsegmentmask>
            <px:pxselector id="edRequestedByID" runat="server" datafield="RequestedByID" commitchanges="True" autorefresh="True">
                <gridproperties fastfilterfields="Location__VTaxZoneID"></gridproperties>
            </px:pxselector>
            <px:pxselector id="edDepartmentID" runat="server" datafield="DepartmentID" commitchanges="True">
            </px:pxselector>
            <px:pxdatetimeedit id="edDateOfUse" runat="server" alreadylocalized="False" datafield="DateOfUse" commitchanges="True" isclientcontrol="True">
            </px:pxdatetimeedit>
            <px:pxdatetimeedit id="edInitialLiqDate" runat="server" alreadylocalized="False" datafield="InitialLiqDate" isclientcontrol="True" commitchanges="True">
            </px:pxdatetimeedit>
            <px:pxdatetimeedit id="edLiqDate" runat="server" alreadylocalized="False" datafield="LiqDate" isclientcontrol="True" commitchanges="True">
            </px:pxdatetimeedit>
            <%--            <pxa:PXCurrencyRate DataField="CuryID" ID="edCury" runat="server" DataSourceID="ds" RateTypeView="_ATPTEFMCashAdvance_CurrencyInfo_"
                DataMember="_ATPTEFMCashAdvance_PX.Objects.CM.Currency+curyID_" Width="250px"></pxa:PXCurrencyRate>--%>
            <pxa:pxcurrencyrate datafield="CuryID" id="edCury" runat="server" datasourceid="ds" ratetypeview="_ATPTEFMCashAdvance_CurrencyInfo_" datamember="_Currency_" commitchanges="True">
            </pxa:pxcurrencyrate>
            <px:pxlayoutrule runat="server" startcolumn="True">
            </px:pxlayoutrule>
            <px:pxnumberedit id="edCuryRequestedAmount" runat="server" datafield="CuryRequestedAmount" commitchanges="True">
            </px:pxnumberedit>
            <px:pxnumberedit id="edCuryActualSpentAmount" runat="server" datafield="CuryActualSpentAmount" isclientcontrol="True" commitchanges="true">
            </px:pxnumberedit>
            <px:pxnumberedit id="edCuryWhtTaxAmount" runat="server" datafield="CuryWhtTaxAmount" commitchanges="True">
            </px:pxnumberedit>
            <px:pxnumberedit id="edRefundAmount" runat="server" alreadylocalized="False" datafield="RefundAmount" isclientcontrol="True" commitchanges="true">
            </px:pxnumberedit>
            <px:pxnumberedit id="edCuryChangeAmount" runat="server" datafield="CuryChangeAmount" commitchanges="true" />
        </template>
    </px:pxformview>
</asp:content>
<asp:content id="cont3" contentplaceholderid="phG" runat="Server">
    <px:pxtab id="tab" runat="server" width="100%" height="150px" datasourceid="ds" datamember="CurrentCashAdvance">
        <items>
            <px:pxtabitem text="Request Details">
                <template>
                    <px:pxgrid id="PXGrid1" runat="server" datasourceid="ds" skinid="DetailsInTab" tabindex="2000" width="100%" style="left: 0px; top: 0px" autoadjustcolumns="True">
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
                            <px:pxgridlevel datamember="CashAdvanceRequestLines" datakeynames="CashAdvanceRequestDetailID">
                                <columns>
                                    <px:pxgridcolumn datafield="InventoryID" autocallback="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="InventoryItem__Descr" width="280px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Remarks" width="280px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Qty" width="100px" commitchanges="True" textalign="Right">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Uom" width="72px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="CuryUnitCost" commitchanges="True" textalign="Right" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="CuryAmount" width="100px" commitchanges="True" textalign="Right">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AccountID" width="120px" autocallback="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AccountDescription" syncvisibility="false">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn commitchanges="True" datafield="SubID" width="140px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ProjectID" width="200px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ProjectTaskID" width="100px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="CostCodeID" width="100px" commitchanges="True">
                                    </px:pxgridcolumn>
                                </columns>
                                <rowtemplate>
                                    <px:pxselector commitchanges="true" id="edInventoryID" runat="server" datafield="InventoryID" allowedit="true" autorefresh="True" />
                                    <%-- <px:PXSegmentMask CommitChanges="True" ID="edInventoryID" runat="server" DataField="InventoryID" AllowEdit="True" Size="XM" AutoRefresh="True"/>--%>
                                    <px:pxsegmentmask id="edSubID" runat="server" datafield="SubID" autorefresh="True" />
                                </rowtemplate>
                            </px:pxgridlevel>
                        </levels>
                        <autosize enabled="True" />
                    </px:pxgrid>
                </template>
            </px:pxtabitem>
            <px:pxtabitem text="Receipts">
                <template>
                    <px:pxgrid id="PXGrid2" runat="server" datasourceid="ds" tabindex="2200" skinid="DetailsInTab" width="100%" syncposition="true">
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
                            <px:pxgridlevel datakeynames="CashAdvanceReceiptDetailIID" datamember="CashAdvanceReceiptLines">
                                <columns>
                                    <px:pxgridcolumn datafield="Reversed" commitchanges="True" type="CheckBox">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="LiquidationRef" linkcommand="OpenExpClaim" width="140px" commitchanges="true">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ExpenseReceiptRefNbr" linkcommand="OpenTransaction" width="140px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Date" width="90px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="InventoryID" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="InventoryItem__Descr" width="280px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="LineDescription" width="280px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="InventoryItem__BaseUnit" width="72px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ATPTEFMCARequestDetail__Qty" textalign="Right" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ATPTEFMCARequestDetail__CuryUnitCost" textalign="Right" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ATPTEFMCARequestDetail__CuryAmount" textalign="Right" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="TaxZoneID" width="120px" commitchanges="true">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="TaxCategoryID" width="120px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AtcCode" width="120px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="NetQty" commitchanges="True" textalign="Right" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="CuryNetUnitCost" commitchanges="True" textalign="Right" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="CuryNetAmt" commitchanges="True" textalign="Right" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AccountID" width="120px" commitchanges="True" autocallback="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AccountDescription" syncvisibility="false">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="SubID" width="140px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn commitchanges="True" datafield="ProjectID" width="200px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ProjectTaskID" width="100px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="CostCodeID" width="100px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="RefNbr" width="140px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="VendID" width="140px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="VendorName" width="220px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="VendorAddress" width="180px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="VendorTin" width="180px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="InventoryItem__ItemType">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Remarks" width="250px">
                                    </px:pxgridcolumn>
                                </columns>
                                <rowtemplate>
                                    <px:pxselector id="edVendorID" runat="server" datafield="VendID" commitchanges="True">
                                        <gridproperties fastfilterfields="AcctCD,AcctName" />
                                    </px:pxselector>
                                    <px:pxsegmentmask id="edSubIDRequest" runat="server" datafield="SubID" autorefresh="True" />
                                </rowtemplate>
                            </px:pxgridlevel>
                        </levels>
                        <autosize enabled="True" />
                        <Mode AllowUpload="True" />
                        <actionbar>
                            <customitems>
                                <px:pxtoolbarbutton alreadylocalized="False" suppresshtmlencoding="False" usessignalr="False">
                                    <autocallback command="LoadRequest" target="ds" />
                                </px:pxtoolbarbutton>
                            </customitems>
                        </actionbar>
                    </px:pxgrid>
                </template>
            </px:pxtabitem>
            <px:pxtabitem text="Budget Details" repaintondemand="False">
                <template>
                    <px:pxgrid id="BudgetGrid" runat="server" autoadjustcolumns="True" datasourceid="ds" skinid="Inquire"
                        tabindex="6900" width="100%">
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
                            <px:pxgridlevel datamember="Budget">
                                <columns>
                                    <px:pxgridcolumn datafield="AcctID" width="120px"/>
                                    <px:pxgridcolumn datafield="SubID" width="140px" />
                                    <px:pxgridcolumn datafield="CuryID" />
                                    <px:pxgridcolumn datafield="DocAmt" textalign="Right" width="100px" />
                                    <px:pxgridcolumn datafield="InitialBudget" textalign="Right" width="100px" />
                                    <px:pxgridcolumn datafield="BudgetAmt" commitchanges="True" textalign="Right" width="100px" />
                                    <px:pxgridcolumn datafield="RequestAmt" textalign="Right" width="100px" />
                                    <px:pxgridcolumn datafield="ApprovedAmt" textalign="Right" width="100px" />
                                    <px:pxgridcolumn datafield="UnapprovedAmt" textalign="Right" width="100px" />
                                    <px:pxgridcolumn datafield="SpentAmt" textalign="Right" width="100px" />
                                    <px:pxgridcolumn datafield="ReturnAmt" textalign="Right" width="100px" />
                                </columns>
                            </px:pxgridlevel>
                        </levels>
                        <autosize enabled="True" />
                        <Mode AllowColMoving="False"></Mode>
                        <actionbar>
                            <actions>
                                <addnew enabled="False" />
                                <delete enabled="False" />
                                <editrecord enabled="False" />
                            </actions>
                        </actionbar>
                    </px:pxgrid>
                </template>
            </px:pxtabitem>
            <px:pxtabitem text="Project Budget Details" repaintondemand="False" bindingcontext="form">
                <template>
                    <px:pxgrid runat="server" id="grdRLProjectBudget" height="400px" width="100%" skinid="Inquire"
                        style="position: static" autoadjustcolumns="True" datasourceid="ds">
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
                            <px:pxgridlevel datamember="ProjectBudget">
                                <columns>
                                    <px:pxgridcolumn datafield="ProjectID" />
                                    <px:pxgridcolumn datafield="ProjectTaskID" />
                                    <px:pxgridcolumn datafield="CostCodeID" />
                                    <px:pxgridcolumn datafield="AccountGroupID" width="100px" />
                                    <px:pxgridcolumn datafield="CuryID" />
                                    <px:pxgridcolumn datafield="DocAmt" width="100px" />
                                    <px:pxgridcolumn datafield="InitialBudget" width="100px" />
                                    <px:pxgridcolumn datafield="BudgetAmt" width="100px" />
                                    <px:pxgridcolumn datafield="RequestAmt" textalign="Right" width="100px" />
                                    <px:pxgridcolumn datafield="ApprovedAmt" width="100px" />
                                    <px:pxgridcolumn datafield="UnapprovedAmt" width="100px" />
                                    <px:pxgridcolumn datafield="SpentAmt" width="100px" />
                                    <px:pxgridcolumn datafield="ReturnAmt" textalign="Right" width="100px" />
                                </columns>
                            </px:pxgridlevel>
                        </levels>
                        <autosize enabled="True" />
                        <Mode AllowColMoving="False"></Mode>
                    </px:pxgrid>
                </template>
            </px:pxtabitem>
            <px:pxtabitem bindingcontext="form" text="Approval Details" visibleexp="DataControls[&quot;edRequestApproval&quot;].Value = 1">
                <template>
                    <px:pxgrid id="gridApproval" runat="server" datasourceid="ds" noteindicator="True" skinid="DetailsInTab" style="left: 0px; top: 0px;" tabindex="26200" width="100%">
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
                            <px:pxgridlevel datakeynames="ApprovalID" datamember="Approval">
                                <columns>
                                    <px:pxgridcolumn datafield="ApproverEmployee__AcctCD" width="160px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ApproverEmployee__AcctName" width="160px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ApprovedByEmployee__AcctCD" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ApprovedByEmployee__AcctName" width="160px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ApproveDate" width="90px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn allownull="False" allowupdate="False" datafield="Status" rendereditortext="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="WorkgroupID" width="150px">
                                    </px:pxgridcolumn>
                                </columns>
                            </px:pxgridlevel>
                        </levels>
                        <autosize enabled="True" />
                        <mode allowaddnew="False" allowdelete="False" allowupdate="False" />
                    </px:pxgrid>
                </template>
            </px:pxtabitem>
            <px:pxtabitem text="Financial Details">

                <template>
                    <px:pxlayoutrule runat="server" controlsize="M" labelswidth="SM" startcolumn="True">
                    </px:pxlayoutrule>
                    <px:pxlayoutrule runat="server" groupcaption="Link to AP" startgroup="True">
                    </px:pxlayoutrule>
                    <px:pxsegmentmask id="edBranchID" runat="server" datafield="BranchID">
                    </px:pxsegmentmask>
                    <px:pxlayoutrule runat="server" groupcaption="Tax" startgroup="True">
                    </px:pxlayoutrule>
                    <px:pxselector id="edTaxZoneID" runat="server" datafield="TaxZoneID">
                    </px:pxselector>
                    <px:pxlayoutrule runat="server" groupcaption="Link to Bills and Adjustments" startgroup="True">
                    </px:pxlayoutrule>
                    <px:pxdropdown id="edBillType" runat="server" datafield="BillType" isclientcontrol="True">
                    </px:pxdropdown>
                    <px:pxtextedit id="edBillRefNbr" runat="server" alreadylocalized="False" datafield="BillRefNbr" isclientcontrol="True">
                        <linkcommand target="ds" command="ViewBill"></linkcommand>
                    </px:pxtextedit>
                    <px:pxnumberedit id="edBillBalance" runat="server" alreadylocalized="False" datafield="BillBalance" isclientcontrol="True">
                    </px:pxnumberedit>
                    <px:pxdropdown id="edBillStatus" runat="server" datafield="BillStatus" isclientcontrol="True">
                    </px:pxdropdown>
                    <px:pxlayoutrule runat="server" groupcaption="Link to Checks and Payments" startgroup="True">
                    </px:pxlayoutrule>
                    <px:pxdropdown id="edPmtType" runat="server" datafield="PmtType" isclientcontrol="True">
                    </px:pxdropdown>
                    <px:pxtextedit id="edPmtRefNbr" runat="server" alreadylocalized="False" datafield="PmtRefNbr" isclientcontrol="True">
                        <linkcommand target="ds" command="ViewPayment"></linkcommand>
                    </px:pxtextedit>
                    <px:pxnumberedit id="edPmtBalance" runat="server" alreadylocalized="False" datafield="PmtBalance" isclientcontrol="True">
                    </px:pxnumberedit>
                    <px:pxdropdown id="edPmtStatus" runat="server" datafield="PmtStatus" isclientcontrol="True">
                    </px:pxdropdown>
                    <px:pxlayoutrule runat="server" groupcaption="Link to Prepayment" startgroup="True">
                    </px:pxlayoutrule>
                    <px:pxdropdown id="edPpmType" runat="server" datafield="PpmType" isclientcontrol="True">
                    </px:pxdropdown>
                    <px:pxtextedit id="edPpmRefNbr" runat="server" alreadylocalized="False" datafield="PpmRefNbr" isclientcontrol="True">
                        <linkcommand target="ds" command="ViewPrepayment"></linkcommand>
                    </px:pxtextedit>
                    <px:pxnumberedit id="edPpmBalance" runat="server" alreadylocalized="False" datafield="PpmBalance" isclientcontrol="True">
                    </px:pxnumberedit>
                    <px:pxdropdown id="edPpmStatus" runat="server" datafield="PpmStatus" isclientcontrol="True">
                    </px:pxdropdown>
                    <px:pxlayoutrule runat="server" groupcaption="Link to Reclassification" startgroup="True">
                    </px:pxlayoutrule>
                    <px:pxdropdown id="edReclassifyType" runat="server" datafield="ReclassifyType" isclientcontrol="True">
                    </px:pxdropdown>
                    <px:pxtextedit id="edReclassifiedInvoiceRefNbr" runat="server" alreadylocalized="False" datafield="ReclassifiedInvoiceRefNbr" isclientcontrol="True">
                        <linkcommand target="ds" command="viewReclassifyBill"></linkcommand>
                    </px:pxtextedit>
                    <px:pxnumberedit id="edReclassifyBalance" runat="server" alreadylocalized="False" datafield="ReclassifyBalance" isclientcontrol="True">
                    </px:pxnumberedit>
                    <px:pxdropdown id="edReclassifyStatus" runat="server" datafield="ReclassifyStatus" isclientcontrol="True">
                    </px:pxdropdown>
                    <px:pxlayoutrule runat="server" groupcaption="Link to Vendor Refund" startgroup="True">
                    </px:pxlayoutrule>
                    <px:pxdropdown id="edVendorRefundType" runat="server" datafield="VendorRefundType" isclientcontrol="True">
                    </px:pxdropdown>
                    <px:pxtextedit id="edVendorRefundRefNbr" runat="server" alreadylocalized="False" datafield="VendorRefundRefNbr" isclientcontrol="True">
                        <linkcommand target="ds" command="viewRefund"></linkcommand>
                    </px:pxtextedit>
                    <px:pxnumberedit id="edVendorRefundBalance" runat="server" alreadylocalized="False" datafield="VendorRefundBalance" isclientcontrol="True">
                    </px:pxnumberedit>
                    <px:pxdropdown id="edVendorRefundStatus" runat="server" datafield="VendorRefundStatus" isclientcontrol="True">
                    </px:pxdropdown>
                </template>

            </px:pxtabitem>
            <px:pxtabitem text="Voided Documents">
                <template>
                    <px:pxgrid id="gridVoided" runat="server" datasourceid="ds" noteindicator="True" skinid="Inquire" style="left: 0px; top: 0px;" tabindex="26200" width="100%">
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
                            <px:pxgridlevel datamember="VoidedDocuments" datakeynames="SortID,RefNbr">
                                <columns>
                                    <px:pxgridcolumn datafield="DocType" width="100px"></px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="RefNbr" width="120px" linkcommand="ViewVoidCheck"></px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="BranchID" width="150px"></px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Date" width="100px"></px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="VendorID" width="120px"></px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Descr" width="320px"></px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Amount" width="100px" textalign="Right"></px:pxgridcolumn>
                                </columns>
                            </px:pxgridlevel>
                        </levels>
                        <autosize enabled="True" />
                    </px:pxgrid>
                </template>
            </px:pxtabitem>
        </items>
        <autosize container="Window" enabled="True" minheight="150" />
    </px:pxtab>

    <px:pxsmartpanel id="PanelSubmitReceipts" runat="server" height="396px" width="910px" caption="Add Receipt" captionvisible="True" key="ReceiptsForSubmit" autoreload="True" cancelbuttonid="PXButtonCancel" autorepaint="True" callbackmode-commitchanges="True" alreadylocalized="False" createondemand="True" tabindex="9700" designview="Content">
        <px:pxgrid id="gridReceiptsForSubmit" runat="server" height="240px" width="100%" datasourceid="ds" skinid="Inquire" noteindicator="False" filesindicator="False" syncposition="True" tabindex="12400">
            <autosize enabled="true" />
            <levels>
                <px:pxgridlevel datakeynames="ClaimDetailID" datamember="ReceiptsForSubmit">
                    <columns>
                        <px:pxgridcolumn allowcheckall="True" allownull="False" datafield="Selected" textalign="Center" type="CheckBox" width="30px" />
                        <px:pxgridcolumn datafield="InventoryID">
                        </px:pxgridcolumn>
                        <px:pxgridcolumn datafield="InventoryID_description" width="280px">
                        </px:pxgridcolumn>
                        <px:pxgridcolumn datafield="RunningQty" textalign="Right" width="100px">
                        </px:pxgridcolumn>
                        <px:pxgridcolumn datafield="CuryUnitCost" textalign="Right" width="100px">
                        </px:pxgridcolumn>
                        <px:pxgridcolumn datafield="Balance" textalign="Right" width="100px">
                        </px:pxgridcolumn>
                    </columns>
                </px:pxgridlevel>
            </levels>
            <mode allowaddnew="False" allowdelete="False" allowupdate="False" />
        </px:pxgrid>
        <px:pxpanel id="PXPanelBtn" runat="server" skinid="Buttons">
            <px:pxbutton id="PXButtonAdd" runat="server" text="Add" commandname="SubmitReceipt" commandsourceid="ds" />
            <px:pxbutton id="PXButtonAddClose" runat="server" text="Add & Close" commandsourceid="ds" dialogresult="OK" />
            <px:pxbutton id="PXButtonClose" runat="server" dialogresult="Cancel" text="Close" commandname="CancelSubmitReceipt" commandsourceid="ds" />
        </px:pxpanel>
    </px:pxsmartpanel>
</asp:content>

<%@ page language="C#" masterpagefile="~/MasterPages/FormTab.master" autoeventwireup="true"
    validaterequest="false" codefile="ATPT3011.aspx.cs" inherits="Page_ATPT3011" title="Untitled Page" %>

<%@ mastertype virtualpath="~/MasterPages/FormTab.master" %>

<asp:content id="cont1" contentplaceholderid="phDS" runat="Server">
    <px:pxdatasource id="ds" runat="server" visible="True" width="100%" primaryview="FundTransactions" typename="CashFundManagement.BLC.ATPTEFMFundTransactionEntry" enableattributes="true">
        <callbackcommands>
            <px:pxdscallbackcommand name="openTransaction" visible="False" dependongrid="PXGrid2">
            </px:pxdscallbackcommand>
            <px:pxdscallbackcommand name="openReclassReceipt" visible="False" dependongrid="PXGrid3">
            </px:pxdscallbackcommand>
            <px:pxdscallbackcommand name="loadRequest" visible="False">
            </px:pxdscallbackcommand>
            <px:pxdscallbackcommand name="SubmitReceipt" visible="False">
            </px:pxdscallbackcommand>
            <px:pxdscallbackcommand name="CancelSubmitReceipt" visible="False">
            </px:pxdscallbackcommand>
        </callbackcommands>
    </px:pxdatasource>
</asp:content>
<asp:content id="cont2" contentplaceholderid="phF" runat="Server">
    <px:pxformview id="form" runat="server" datasourceid="ds" style="z-index: 100" width="100%" datamember="FundTransactions" tabindex="8400">
        <template>
            <px:pxlayoutrule runat="server" startrow="True" startcolumn="True" />
            <px:pxdropdown id="edFundType" runat="server" datafield="FundType" isclientcontrol="True" commitchanges="True">
            </px:pxdropdown>
            <px:pxselector id="edRefNbr" runat="server" datafield="RefNbr" commitchanges="true">
            </px:pxselector>
            <px:pxselector id="edFundID" runat="server" datafield="FundID" commitchanges="True" autorefresh="True">
            </px:pxselector>
            <px:pxdropdown id="edFundTransactionType" runat="server" datafield="FundTransactionType" commitchanges="True" isclientcontrol="True">
            </px:pxdropdown>
            <px:pxdropdown id="edStatus" runat="server" datafield="Status" commitchanges="True" isclientcontrol="True">
            </px:pxdropdown>
            <px:pxcheckbox id="edHold" runat="server" alreadylocalized="False" datafield="Hold" text="Hold" commitchanges="True" isclientcontrol="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edApproved" runat="server" alreadylocalized="False" datafield="Approved" text="Approved" isclientcontrol="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edIsImported" runat="server" alreadylocalized="False" datafield="IsImported" text="Approved" isclientcontrol="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edIsOverbudget" runat="server" alreadylocalized="False" datafield="IsOverbudget" commitchanges="True" isclientcontrol="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edHasInitialBudget" runat="server" alreadylocalized="False" datafield="HasInitialBudget" commitchanges="True" isclientcontrol="True">
            </px:pxcheckbox>
            <px:pxdatetimeedit id="edDate" runat="server" alreadylocalized="False" datafield="Date" commitchanges="True" defaultlocale="" isclientcontrol="True">
            </px:pxdatetimeedit>
            <px:pxmaskedit id="edFinPeriodID" runat="server" datafield="FinPeriodID" commitchanges="True">
            </px:pxmaskedit>
            <px:pxlayoutrule runat="server" columnspan="2">
            </px:pxlayoutrule>
            <px:pxtextedit id="edDescr" runat="server" alreadylocalized="False" datafield="Descr" defaultlocale="" isclientcontrol="True">
            </px:pxtextedit>
            <px:pxcheckbox id="edRequestApproval" runat="server" alreadylocalized="False" datafield="RequestApproval" text="Request Approval" isclientcontrol="True">
            </px:pxcheckbox>
            <px:pxlayoutrule runat="server" startcolumn="True" labelswidth="SM">
            </px:pxlayoutrule>
            <px:pxselector id="edRequestedByID" runat="server" datafield="RequestedByID" commitchanges="True">
            </px:pxselector>
            <px:pxselector id="edDepartmentID" runat="server" datafield="DepartmentID" commitchanges="True">
            </px:pxselector>
            <px:pxselector id="edBranchID" runat="server" datafield="BranchID" commitchanges="True" autorefresh="True">
            </px:pxselector>
            <px:pxdropdown id="edCashAdvanceStatus" runat="server" datafield="CashAdvanceStatus" commitchanges="True" isclientcontrol="True">
            </px:pxdropdown>
            <px:pxdatetimeedit id="edDateOfUse" runat="server" alreadylocalized="False" datafield="DateOfUse" commitchanges="True" isclientcontrol="True">
            </px:pxdatetimeedit>
            <px:pxdatetimeedit id="edInitialLiqDate" runat="server" alreadylocalized="False" datafield="InitialLiqDate" isclientcontrol="True" commitchanges="True">
            </px:pxdatetimeedit>
            <px:pxdatetimeedit id="edLiqDate" runat="server" alreadylocalized="False" datafield="LiqDate" isclientcontrol="True" commitchanges="True">
            </px:pxdatetimeedit>
            <px:pxcheckbox id="chkShowBudgetValidation" runat="server" alreadylocalized="False" datafield="ShowBudgetValidation" isclientcontrol="True">
            </px:pxcheckbox>
            <px:pxcheckbox id="edNoFund" runat="server" alreadylocalized="False" datafield="NoFund" text="Reimbursement w/out Fund" commitchanges="True" isclientcontrol="True">
            </px:pxcheckbox>
            <px:pxlayoutrule runat="server" startcolumn="True" labelswidth="SM">
            </px:pxlayoutrule>
            <px:pxnumberedit id="edRequestedAmount" runat="server" alreadylocalized="False" datafield="RequestedAmount" defaultlocale="" isclientcontrol="True">
            </px:pxnumberedit>
            <px:pxnumberedit id="edActualSpentAmount" runat="server" alreadylocalized="False" datafield="ActualSpentAmount" defaultlocale="" isclientcontrol="True">
            </px:pxnumberedit>
            <px:pxnumberedit id="edTotalWhtAmount" runat="server" alreadylocalized="False" datafield="TotalWhtAmount" defaultlocale="" isclientcontrol="True">
            </px:pxnumberedit>
            <px:pxnumberedit id="edChangeAmount" runat="server" alreadylocalized="False" datafield="ChangeAmount" defaultlocale="" isclientcontrol="True">
            </px:pxnumberedit>
            <px:pxnumberedit id="edAmountReceived" runat="server" alreadylocalized="False" datafield="AmountReceived" defaultlocale="" isclientcontrol="True" commitchanges="True">
            </px:pxnumberedit>
            <px:pxnumberedit id="edAmountReleased" runat="server" alreadylocalized="False" datafield="AmountReleased" defaultlocale="" isclientcontrol="True" commitchanges="True">
            </px:pxnumberedit>
            <px:pxnumberedit id="edReclassificationAmt" runat="server" alreadylocalized="False" datafield="ReclassificationAmt" defaultlocale="" isclientcontrol="True" commitchanges="True">
            </px:pxnumberedit>
            <px:pxnumberedit id="edBalance" runat="server" alreadylocalized="False" datafield="Balance" isclientcontrol="True" commitchanges="True">
            </px:pxnumberedit>
        </template>
    </px:pxformview>
</asp:content>
<asp:content id="cont3" contentplaceholderid="phG" runat="Server">
    <px:pxtab id="tab" runat="server" width="100%" height="150px" datasourceid="ds">
        <items>
            <px:pxtabitem text="Document Details" repaintondemand="False">
                <template>
                    <px:pxgrid id="PXGrid1" runat="server" datasourceid="ds" skinid="DetailsInTab" tabindex="1200" width="100%" matrixmode="True" syncposition="True" repaintcolumns="True" autoadjustcolumns="True">
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
                            <px:pxgridlevel datamember="FundTransactionDetails">
                                <columns>
                                    <px:pxgridcolumn datafield="Date" width="90px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Particulars" width="220px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="InventoryID" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Description" width="280px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="LineDescription" width="280px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Qty" textalign="Right" width="100px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="UnitRecordID" width="72px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="UnitCost" width="100px" commitchanges="True" textalign="Right">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Amount" width="100px" commitchanges="True" textalign="Right">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AccountID" width="120px" autocallback="True" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AccountDescription" syncvisibility="false">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AccountGroup" syncvisibility="false" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="SubID" width="140px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ProjectID" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn commitchanges="True" datafield="ProjectTaskID">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="CostCodeID" commitchanges="True">
                                    </px:pxgridcolumn>
                                </columns>
                                <rowtemplate>
                                    <px:pxsegmentmask id="edSubID" runat="server" datafield="SubID" autorefresh="True" />
                                </rowtemplate>
                            </px:pxgridlevel>
                        </levels>
                        <mode initnewrow="True" allowupload="True" />
                        <autosize enabled="True" />
                    </px:pxgrid>
                </template>
            </px:pxtabitem>
            <px:pxtabitem text="Receipts" repaintondemand="False">
                <template>
                    <px:pxgrid id="PXGrid2" runat="server" datasourceid="ds" skinid="DetailsInTab" tabindex="3700" width="100%">
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
                            <px:pxgridlevel datakeynames="FundTransactionReceiptDetailID" datamember="FundTransactionReceiptLines">
                                <columns>
                                    <px:pxgridcolumn datafield="ExpenseReceiptRefNbr" width="140px" linkcommand="OpenTransaction">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="BranchID" width="90px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Date" width="90px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="InventoryID" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Descr" width="280px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="LineDescription" width="280px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Qty" textalign="Right" width="100px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="InventoryItem__BaseUnit" width="72px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="UnitCost" textalign="Right" width="100px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Amount" textalign="Right" width="100px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AccountID" width="120px" autocallback="True" commitchanges="true">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AccountDescription" syncvisibility="false">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AccountGroup" syncvisibility="false" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="SubID" width="140px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ProjectID" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ProjectTaskID" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="CostCodeID">
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
                                    <px:pxgridcolumn datafield="TaxZoneID" width="120px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AtcCode" width="120px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="TaxCategoryID" width="120px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn commitchanges="True" datafield="NetQty" textalign="Right" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn commitchanges="True" datafield="NetUnitCost" textalign="Right" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn commitchanges="True" datafield="NetAmt" textalign="Right" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn commitchanges="True" datafield="WhtAmount" textalign="Right" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="InventoryItem__ItemType">
                                    </px:pxgridcolumn>
                                </columns>
                                <rowtemplate>
                                    <px:pxselector id="edVendorID" runat="server" datafield="VendID" commitchanges="True">
                                        <gridproperties fastfilterfields="AcctCD,AcctName" />
                                    </px:pxselector>
                                    <px:pxsegmentmask id="edSubIDReceipt" runat="server" datafield="SubID" autorefresh="True" />
                                </rowtemplate>
                            </px:pxgridlevel>
                        </levels>
                        <mode initnewrow="True" allowupload="True" />
                        <autosize enabled="True" />
                        <actionbar>
                            <customitems>
                                <px:pxtoolbarbutton alreadylocalized="False" suppresshtmlencoding="False" usessignalr="False">
                                    <autocallback command="LoadRequest" target="ds" />
                                </px:pxtoolbarbutton>
                            </customitems>
                        </actionbar>
                    </px:pxgrid>
                    <px:pxformview id="PXFormView1" runat="server" datasourceid="ds" height="150px" width="100%" datamember="FundTransactionCash" tabindex="3900">
                        <template>
                            <px:pxlayoutrule runat="server" groupcaption="Cash" startrow="True">
                            </px:pxlayoutrule>
                            <px:pxnumberedit id="edReceivedAmount" runat="server" alreadylocalized="False" datafield="ReceivedAmount" commitchanges="True" defaultlocale="" isclientcontrol="True">
                            </px:pxnumberedit>
                            <px:pxnumberedit id="edReleasedAmount" runat="server" alreadylocalized="False" datafield="ReleasedAmount" commitchanges="True" defaultlocale="" isclientcontrol="True">
                            </px:pxnumberedit>
                        </template>
                    </px:pxformview>
                </template>
            </px:pxtabitem>
            <px:pxtabitem text="Reclassification Receipts" repaintondemand="False">
                <template>
                    <px:pxgrid id="PXGrid3" runat="server" datasourceid="ds" skinid="Inquire" tabindex="3700" width="100%" repaintcolumns="True">
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
                            <px:pxgridlevel datamember="FundTransactionReclassficationReceiptDetail">
                                <columns>
                                    <px:pxgridcolumn datafield="ExpenseReceiptRefNbr" width="140px" linkcommand="OpenReclassReceipt">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Date" width="90px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="InventoryID" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="InventoryItem__BaseUnit" width="72px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AccountID" width="120px" autocallback="True" commitchanges="true">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="AccountDescription" syncvisibility="false">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="SubID" width="140px" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ProjectID" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ProjectTaskID" commitchanges="True">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="CostCodeID">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="RefNbr" width="140px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="TaxZoneID" width="120px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="TaxCategoryID" width="120px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn commitchanges="True" datafield="NetQty" textalign="Right" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn commitchanges="True" datafield="NetUnitCost" textalign="Right" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn commitchanges="True" datafield="NetAmt" textalign="Right" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn commitchanges="True" datafield="WhtAmount" textalign="Right" width="100px">
                                    </px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="InventoryItem__ItemType">
                                    </px:pxgridcolumn>
                                </columns>
                                <rowtemplate>
                                    <px:pxsegmentmask id="edReclassSubID" runat="server" datafield="SubID" autorefresh="True" />
                                </rowtemplate>
                            </px:pxgridlevel>
                        </levels>
                        <mode initnewrow="True" />
                        <autosize enabled="True" />
                    </px:pxgrid>
                </template>
            </px:pxtabitem>
            <px:pxtabitem text="Approvals" bindingcontext="form" visibleexp="DataControls[&quot;edRequestApproval&quot;].Value = 1">
                <template>
                    <px:pxgrid id="gridApproval" runat="server" datasourceid="ds" width="100%" skinid="DetailsInTab" noteindicator="True" style="left: 0px; top: 0px;" tabindex="26200">
                        <autosize enabled="True"></autosize>
                        <mode allowaddnew="False" allowdelete="False" allowupdate="False"></mode>
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
Try to change filter to see records here.">
                        </emptymsg>
                        <levels>
                            <px:pxgridlevel datamember="Approval">
                                <columns>
                                    <px:pxgridcolumn datafield="ApproverEmployee__AcctCD" width="160px"></px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ApproverEmployee__AcctName" width="160px"></px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ApprovedByEmployee__AcctCD" width="100px"></px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ApprovedByEmployee__AcctName" width="160px"></px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="ApproveDate" width="90px"></px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="Status" allownull="False" allowupdate="False" rendereditortext="True"></px:pxgridcolumn>
                                    <px:pxgridcolumn datafield="WorkgroupID" width="150px"></px:pxgridcolumn>
                                </columns>
                            </px:pxgridlevel>
                        </levels>
                    </px:pxgrid>
                </template>
            </px:pxtabitem>
            <px:pxtabitem text="Budget Details" repaintondemand="false">
                <template>
                    <px:pxgrid runat="server" id="gridBudget" width="100%" height="400px" repaintcolumns="True" datasourceid="ds" skinid="Inquire"
                        style="position: static" tabindex="1800" autoadjustcolumns="True">
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
                                    <px:pxgridcolumn datafield="AcctID" width="120px" />
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
        </items>
        <autosize container="Window" enabled="True" minheight="150" />
    </px:pxtab>

    <px:pxsmartpanel id="PanelSubmitReceipts" runat="server" height="396px" width="910px" caption="Add Receipt" captionvisible="True" key="ReceiptsForSubmit" autoreload="True" cancelbuttonid="PXButtonCancel" autorepaint="True" callbackmode-commitchanges="True" alreadylocalized="False" createondemand="True" tabindex="9700" designview="Content">
        <px:pxgrid id="gridReceiptsForSubmit" runat="server" height="240px" width="100%" datasourceid="ds" skinid="Inquire" noteindicator="False" filesindicator="False" syncposition="True" tabindex="12400">
            <autosize enabled="true" />
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
                <px:pxgridlevel datakeynames="ClaimDetailID" datamember="ReceiptsForSubmit">
                    <columns>
                        <px:pxgridcolumn allowcheckall="True" allownull="False" datafield="Selected" textalign="Center" type="CheckBox" width="30px" />
                        <px:pxgridcolumn datafield="InventoryID">
                        </px:pxgridcolumn>
                        <px:pxgridcolumn datafield="InventoryID_description" width="280px">
                        </px:pxgridcolumn>
                        <px:pxgridcolumn datafield="LineDescription" width="280px">
                        </px:pxgridcolumn>
                        <px:pxgridcolumn datafield="RunningQty" textalign="Right" width="100px">
                        </px:pxgridcolumn>
                        <px:pxgridcolumn datafield="UnitCost" textalign="Right" width="100px">
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

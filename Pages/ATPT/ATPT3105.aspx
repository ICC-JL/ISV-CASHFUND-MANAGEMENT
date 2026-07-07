<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="ATPT3105.aspx.cs" Inherits="Page_ATPT3105" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
         TypeName="CashFundManagement.BLC.ATPTEFMProjectBudgetEntry" PrimaryView="ProjectFilter">
		<CallbackCommands>
            <px:PXDSCallbackCommand Name="Save" CommitChanges="True" RepaintControls="All" RepaintControlsIDs="grid" />
			<px:PXDSCallbackCommand Visible="false" Name="Distribute" />
			<px:PXDSCallbackCommand Visible="false" Name="DistributeOK" />
			<px:PXDSCallbackCommand Visible="true" Name="PreloadArticles" />
			<px:PXDSCallbackCommand Visible="false" Name="PreloadArticlesOK" />
			<px:PXDSCallbackCommand Visible="false" Name="PreloadArticlesNext" />
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" DataMember="ProjectFilter" TabIndex="900">
		<Template>
			<px:PXLayoutRule runat="server" StartRow="True" LabelsWidth="SM" ControlSize="M" />
            <px:PXSelector CommitChanges="True" runat="server" ID="edFinYear" DataField="FinYear" />
			<px:PXSelector CommitChanges="True" runat="server" ID="edLedgerID" DataField="LedgerID" />
			<px:PXSelector CommitChanges="True" runat="server" ID="edProjectID" DataField="ProjectID" />
			<px:PXSelector CommitChanges="True" runat="server" ID="edLastModifiedByID" DataField="LastModifiedByID" />

            <px:PXLayoutRule runat="server" StartColumn="true" LabelsWidth="SM" ControlSize="M" />
            <px:PXSelector CommitChanges="True" runat="server" ID="edCompareFinYear" DataField="CompareFinYear" />
			<px:PXSelector CommitChanges="True" runat="server" ID="edCompareLedgerID" DataField="CompareLedgerID" />
			<px:PXSelector CommitChanges="True" runat="server" ID="edCompareProjectID" DataField="CompareProjectID" />
        </Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
    <px:PXGrid ID="grid" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" Height="150px" SkinID="DetailsInTab" TabIndex="18300" SyncPosition="True" FilesIndicator="True" NoteIndicator="True">
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
    <px:PXGridLevel DataKeyNames="RLProjectBudgetLineSummaryID" DataMember="Summary">
        <Columns>
            <px:PXGridColumn DataField="Released" TextAlign="Center" Type="CheckBox" Width="60px" />
            <px:PXGridColumn DataField="ProjectTaskID" TextAlign="Left" Width="100px" CommitChanges="True" />
			<px:PXGridColumn DataField="AccountGroupID" TextAlign="Left" Width="100px" />
            <px:PXGridColumn DataField="CostCodeID" TextAlign="Left" Width="100px" CommitChanges="True" />
<%--            <px:PXGridColumn DataField="InventoryID" TextAlign="Left" Width="100px" CommitChanges="True" />--%>
            <px:PXGridColumn DataField="Description" TextAlign="Left" Width="100px" />
            <px:PXGridColumn DataField="Amount" TextAlign="Right" Width="100px" CommitChanges="True" />
            <px:PXGridColumn DataField="DistributedAmount" TextAlign="Right" Width="100px" CommitChanges="True" />
            <px:PXGridColumn DataField="FinPeriod01" TextAlign="Right" Width="100px" />
            <px:PXGridColumn DataField="FinPeriod02" TextAlign="Right" Width="100px" />
            <px:PXGridColumn DataField="FinPeriod03" TextAlign="Right" Width="100px" />
            <px:PXGridColumn DataField="FinPeriod04" TextAlign="Right" Width="100px" />
            <px:PXGridColumn DataField="FinPeriod05" TextAlign="Right" Width="100px" />
            <px:PXGridColumn DataField="FinPeriod06" TextAlign="Right" Width="100px" />
            <px:PXGridColumn DataField="FinPeriod07" TextAlign="Right" Width="100px" />
            <px:PXGridColumn DataField="FinPeriod08" TextAlign="Right" Width="100px" />
            <px:PXGridColumn DataField="FinPeriod09" TextAlign="Right" Width="100px" />
            <px:PXGridColumn DataField="FinPeriod10" TextAlign="Right" Width="100px" />
            <px:PXGridColumn DataField="FinPeriod11" TextAlign="Right" Width="100px" />
            <px:PXGridColumn DataField="FinPeriod12" TextAlign="Right" Width="100px" />
        </Columns>
        </px:PXGridLevel>
    </Levels>
    <ActionBar>
		<CustomItems>
			<px:PXToolBarButton Text="Distribute">
				<AutoCallBack Command="distribute" Target="ds" />
			</px:PXToolBarButton>
		</CustomItems>
	</ActionBar>
	    <Mode AllowUpload="True" />
	</px:PXGrid>
    <px:PXSmartPanel ID="spDistributeDlg" runat="server" Key="DistrFilter" AutoRepaint="true"
		AutoCallBack-Target="DistributePrm" AutoCallBack-Enabled="true" LoadOnDemand="True"
		AcceptButtonID="cbOk" CancelButtonID="cbCancel" Caption="Distribute Year Amount by Months"
		CaptionVisible="True" DesignView="Content" CommandName="distributeOK" CommandSourceID="ds" Width ="390px">
		<px:PXFormView ID="DistributePrm" runat="server" DataSourceID="ds" Style="z-index: 108"
			Width="100%" DataMember="DistrFilter" Caption="Dispose Parameters" SkinID="Transparent">
			<Activity Height="" HighlightColor="" SelectedColor="" Width="" />
			<Template>
				<px:PXLayoutRule ID="PXLayoutRule99" runat="server" />
				<px:PXDropDown CommitChanges="True" ID="edMethod" runat="server" DataField="Method" Width="220" />
                <px:PXLayoutRule ID="PXLayoutRule98" runat="server" />
                <px:PXCheckBox CommitChanges="True" ID="ApplyToAll" runat="server" DataField="ApplyToAll" />
                <px:PXCheckBox CommitChanges="True" ID="ApplyToSubGroups" runat="server" DataField="ApplyToSubGroups" />
			</Template>
		</px:PXFormView>
		<px:PXPanel ID="PXPanel1" runat="server" SkinID="Buttons">
			<px:PXButton ID="cbOk" runat="server" Text="OK" DialogResult="OK" />
			<px:PXButton ID="cbCancel" runat="server" Text="Cancel" DialogResult="Cancel" />
		</px:PXPanel>
	</px:PXSmartPanel>
    <px:PXSmartPanel ID="spPreloadArticlesDlg" runat="server" Key="PreloadFilter"
        AutoCallBack-Target="PXWizard1" AutoCallBack-Enabled="true"
        CommandName="preloadArticlesOK" CommandSourceID="ds"
		Width="380" Caption="Preload Budget Articles" CaptionVisible="True"
		Style="display: none;" AutoRepaint="true">
        <px:PXWizard ID="PXWizard1" runat="server" Height="240" DataMember="PreloadFilter" SkinID="Flat">
			<NextCommand Target="ds" Command="preloadArticlesNext" />
			<Pages>
				<px:PXWizardPage>
					<Template>
						<px:PXFormView ID="formPanel" runat="server" SkinID="Transparent" DataMember="PreloadFilter">
							<Template>
								<px:PXPanel ID="pnl1" runat="server" RenderStyle="Simple" Style="padding: 10px;">
								    <px:PXLayoutRule ID="lyoPG1_1" runat="server" />
								    <px:PXLabel runat="server" ID="lblPG1_1" Style="font-weight: bold; text-align: right">Step 1 of 2</px:PXLabel>
								    <px:PXLabel runat="server" ID="lblPG1_2" Height="35px" Style="font-style: italic; overflow: visible;">This wizard allows you to preload the budget articles from the actual ledger or from another budget.</px:PXLabel>
								    <px:PXLabel runat="server" ID="lblPG1_3" Height="20px" Style="font-style: italic; overflow: visible;">Select the source parameters:</px:PXLabel>
                                </px:PXPanel>

                                <px:PXPanel ID="pnl2" runat="server" RenderStyle="Simple" Style="padding-left: 30px;">
                                    <px:PXLayoutRule ID="lyoPG1_2" runat="server" LabelsWidth="SM" ControlSize="SM" ColumnSpan="2" />
								    <px:PXSelector ID="prFinYear" runat="server" DataField="FinYear" CommitChanges="True"/>
								    <px:PXSelector ID="prLedgerID" runat="server" DataField="LedgerID" CommitChanges="True" AutoRefresh="True" />
								    <px:PXSelector ID="prProjectID" runat="server" DataField="ProjectID" CommitChanges="True" />
								    <px:PXNumberEdit ID="prMutliplier" runat="server" DataField="Mutliplier" CommitChanges="True" Width="150" />
								</px:PXPanel>
							</Template>
						</px:PXFormView>
					</Template>
				</px:PXWizardPage>
				<px:PXWizardPage>
					<Template>
						<px:PXFormView ID="formPanel" runat="server" SkinID="Transparent" DataMember="PreloadFilter">
							<Template>
								<px:PXPanel ID="pnl1" runat="server" RenderStyle="Simple" Style="padding: 10px;">
								    <px:PXLayoutRule ID="lyoPG1_1" runat="server" />
								    <px:PXLabel runat="server" ID="lblPG1_1" Style="font-weight: bold; text-align: right">Step 2 of 2</px:PXLabel>
								    <px:PXLabel runat="server" ID="lblPG1_2" Height="35px" Style="font-style: italic; overflow: visible;">Specify the preload action if the articles are already entered for the budget.</px:PXLabel>
                                </px:PXPanel>

                                <px:PXGroupBox ID="pnl2" runat="server" CommitChanges="True" DataField="PreloadAction" RenderStyle="Simple" Style="padding-left: 30px;">
									<Template>
										<px:PXLayoutRule ID="lyoPG1_2" runat="server" />
										<px:PXRadioButton ID="prChoice1" Value="1" runat="server" GroupName="PreloadMode" Text="Update Existing Articles Only" />
										<px:PXRadioButton ID="prChoice2" Value="2" runat="server" GroupName="PreloadMode" Text="Load Nonexistent Articles Only" />
										<px:PXRadioButton ID="prChoice3" Value="3" runat="server" GroupName="PreloadMode" Text="Both" />
									</Template>
								</px:PXGroupBox>
							</Template>
						</px:PXFormView>
					</Template>
				</px:PXWizardPage>
			</Pages>
		</px:PXWizard>
    </px:PXSmartPanel>
</asp:Content>

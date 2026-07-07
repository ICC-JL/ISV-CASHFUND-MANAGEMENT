# Acumatica Custom Screen Creation Guide

This guide provides step-by-step instructions for creating custom screens (ASPX pages) in Acumatica ERP.

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Screen Numbering Convention](#screen-numbering-convention)
3. [Master Page Selection](#master-page-selection)
4. [Creating ASPX File](#creating-aspx-file)
5. [Creating ASPX.CS Code-Behind](#creating-aspxcs-code-behind)
6. [Analyzing Screen Layout Images](#analyzing-screen-layout-images)
7. [Common Control Patterns](#common-control-patterns)
8. [Screen Examples](#screen-examples)

---

## Prerequisites

Before creating a custom screen:
1. **PXGraph/Business Logic**: Ensure your Graph (BLC) is created and compiled
2. **DACs**: All required Data Access Classes must be defined
3. **Views**: PXSelect views must be defined in your Graph
4. **Actions**: Any custom actions should be implemented
5. **Screen ID**: Obtain the next available screen ID following project conventions

---

## Screen Numbering Convention

For **ATPT** project, follow this numbering scheme:

| Range | Purpose | Example |
|-------|---------|---------|
| ATPT1xxx | Setup/Configuration screens | ATPT1001 - Preferences |
| ATPT2xxx | Master data/maintenance screens | ATPT2001 - AR Subtypes |
| ATPT3xxx | Transaction entry screens | ATPT3001 - 2550M Entry |
| ATPT4xxx | Process/Inquiry screens | ATPT4001 - Fund Entry |
| ATPT5xxx | Reports | ATPT5007 - Report Viewer |
| ATPT9xxx | Utilities/Admin screens | ATPT9201 - Data Fix |

**File Location**: `Pages\ATPT\ATPT####.aspx` and `Pages\ATPT\ATPT####.aspx.cs`

---

## Master Page Selection

Choose the appropriate master page based on screen layout:

### 1. **FormView.master** - Single Form View
- **Use for**: Setup screens, preferences, single-record maintenance
- **Layout**: Single form occupying the entire content area
- **Example**: ATPT1001.aspx (Preferences)

```aspx
<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" 
    AutoEventWireup="true" ValidateRequest="false" 
    CodeFile="ATPT1001.aspx.cs" Inherits="Page_ATPT1001" 
    Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>
```

**Content Placeholders**:
- `phDS` - PXDataSource
- `phF` - PXFormView

---

### 2. **ListView.master** - Grid View
- **Use for**: List views, master data maintenance without details
- **Layout**: Single grid occupying the entire content area
- **Example**: ATPT2001.aspx (AR Subtypes)

```aspx
<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" 
    AutoEventWireup="true" ValidateRequest="false" 
    CodeFile="ATPT2001.aspx.cs" Inherits="Page_ATPT2001" 
    Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>
```

**Content Placeholders**:
- `phDS` - PXDataSource
- `phL` - PXGrid

---

### 3. **FormTab.master** - Form + Tabs
- **Use for**: Entry screens with form header and tabbed details/grids
- **Layout**: Form on top, tabs below
- **Example**: ATPT3001.aspx (2550M Entry)

```aspx
<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" 
    AutoEventWireup="true" ValidateRequest="false" 
    CodeFile="ATPT3001.aspx.cs" Inherits="Page_ATPT3001" 
    Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>
```

**Content Placeholders**:
- `phDS` - PXDataSource
- `phF` - PXFormView (header)
- `phG` - PXTab (tabbed content)

---

### 4. **FormDetail.master** - Form + Grid
- **Use for**: Master-detail entry screens
- **Layout**: Form on top, grid below
- **Example**: Fund Entry, Cash Advance Entry

```aspx
<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" 
    AutoEventWireup="true" ValidateRequest="false" 
    CodeFile="ATPT4001.aspx.cs" Inherits="Page_ATPT4001" 
    Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>
```

**Content Placeholders**:
- `phDS` - PXDataSource
- `phF` - PXFormView (master)
- `phG` - PXGrid (detail)

---

## Creating ASPX File

### Step 1: Define Page Directives

```aspx
<%@ Page Language="C#" MasterPageFile="~/MasterPages/[MasterPage].master" 
    AutoEventWireup="true" ValidateRequest="false" 
    CodeFile="ATPT####.aspx.cs" Inherits="Page_ATPT####" 
    Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/[MasterPage].master" %>
```

### Step 2: Define PXDataSource

```aspx
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
    <px:PXDataSource ID="ds" runat="server" 
        Visible="True" 
        Width="100%" 
        PrimaryView="Document" 
        TypeName="Namespace.Graph.GraphName">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Save" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand Name="Delete" PostData="Self" />
            <!-- Add custom actions here -->
            <px:PXDSCallbackCommand Name="CustomAction" Visible="false" CommitChanges="true" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
```

**Key Properties**:
- `TypeName`: Fully qualified name of your PXGraph (e.g., `CashFundManagement.BLC.ATPTEFMFundEntry`)
- `PrimaryView`: Name of the main view in your Graph (usually `Document` or `CurrentDocument`)
- `PageLoadBehavior`: Optional - `InsertRecord` for screens that start in insert mode

### Step 3: Define Form View (if applicable)

```aspx
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
    <px:PXFormView ID="form" runat="server" 
        DataSourceID="ds" 
        Style="z-index: 100" 
        Width="100%" 
        DataMember="Document" 
        TabIndex="100">
        <Template>
            <!-- Layout rules and controls go here -->
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M"/>
            
            <!-- Field controls -->
            <px:PXSelector ID="edRefNbr" runat="server" DataField="RefNbr" />
            <px:PXDateTimeEdit ID="edTranDate" runat="server" DataField="TranDate" />
            <px:PXDropDown ID="edStatus" runat="server" DataField="Status" />
            
        </Template>
        <AutoSize Container="Window" Enabled="True" MinHeight="200" />
    </px:PXFormView>
</asp:Content>
```

### Step 4: Define Grid (if applicable)

```aspx
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid ID="grid" runat="server" 
        Height="400px" 
        Width="100%" 
        Style="z-index: 100"
        AllowPaging="True" 
        AllowSearch="True" 
        AdjustPageSize="Auto" 
        DataSourceID="ds" 
        SkinID="Details" 
        TabIndex="200">
        <Levels>
            <px:PXGridLevel DataKeyNames="LineNbr" DataMember="Details">
                <RowTemplate>
                    <!-- Define fields in edit mode -->
                    <px:PXLayoutRule runat="server" StartColumn="True" />
                    <px:PXNumberEdit ID="edLineNbr" runat="server" DataField="LineNbr" />
                    <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" />
                </RowTemplate>
                <Columns>
                    <!-- Define grid columns -->
                    <px:PXGridColumn DataField="LineNbr" Width="70px" />
                    <px:PXGridColumn DataField="Description" Width="200px" />
                    <px:PXGridColumn DataField="Amount" Width="100px" />
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="200" />
    </px:PXGrid>
</asp:Content>
```

### Step 5: Define Tabs (if using FormTab.master)

```aspx
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" 
        Width="100%" 
        Height="150px" 
        DataSourceID="ds" 
        DataMember="Document">
        <Items>
            <px:PXTabItem Text="General">
                <Template>
                    <px:PXLayoutRule runat="server" StartColumn="True" />
                    <!-- Tab content here -->
                </Template>
            </px:PXTabItem>
            
            <px:PXTabItem Text="Details">
                <Template>
                    <px:PXGrid ID="gridDetails" runat="server" 
                        DataSourceID="ds" 
                        Width="100%" 
                        SkinID="Details">
                        <!-- Grid definition -->
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="200" />
    </px:PXTab>
</asp:Content>
```

---

## Creating ASPX.CS Code-Behind

Create the code-behind file following this template:

**File Location**: `Pages\ATPT\ATPT####.aspx.cs`

```csharp
using System;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Page_ATPT#### : PX.Web.UI.PXPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Page initialization logic (usually empty)
    }
}
```

**Important Notes**:
- Class name MUST match the `Inherits` attribute in ASPX: `Page_ATPT####`
- MUST inherit from `PX.Web.UI.PXPage`
- Keep the code-behind minimal - business logic belongs in the Graph
- The `Page_Load` method is typically empty for Acumatica screens

---

## Analyzing Screen Layout Images

When provided with a screen layout image or design mockup, follow this analysis workflow:

### Step 1: Identify Screen Structure

**Questions to answer**:
1. Is this a **single form**, **form + grid**, **form + tabs**, or **grid only**?
2. How many **sections** or **groups** are visible?
3. Are there **tabs**? If yes, what are they?
4. Are there **grids**? Are they in tabs or main content?

**Master Page Selection Matrix**:

| Layout Type | Master Page | When to Use |
|-------------|-------------|-------------|
| Single form only | FormView.master | Setup, preferences, single-record screens |
| Grid only | ListView.master | List maintenance screens |
| Form + Single Grid | FormDetail.master | Master-detail entry |
| Form + Multiple Grids/Complex Layout | FormTab.master | Transaction entry with tabs |

### Step 2: Map to PXGraph Views

For each section of the screen, identify the corresponding view in the Graph:

**Example Analysis**:
```
Screen Layout:
├── Header Section (Form)
│   ├── Reference Number
│   ├── Date
│   ├── Status
│   └── Customer
└── Details Section (Grid)
    ├── Line Number
    ├── Item
    ├── Quantity
    └── Amount

Graph Mapping:
├── PrimaryView: "Document" → Header fields map to ATPTEFMDocument DAC
└── DetailView: "Details" → Grid maps to ATPTEFMDocumentDetail DAC
```

### Step 3: Identify Control Types

Map each field to appropriate Acumatica control:

| Field Type | Control | Example |
|------------|---------|---------|
| Reference Number (key) | `<px:PXSelector>` | RefNbr, DocumentID |
| Text | `<px:PXTextEdit>` | Description, Notes |
| Date | `<px:PXDateTimeEdit>` | TranDate, DueDate |
| Dropdown/Status | `<px:PXDropDown>` | Status, Type |
| Numeric | `<px:PXNumberEdit>` | Amount, Quantity |
| Currency | `<px:PXNumberEdit>` | CuryAmount (with currency info) |
| Checkbox | `<px:PXCheckBox>` | IsActive, Hold |
| Selector/Lookup | `<px:PXSelector>` | Customer, Vendor, Account |
| Branch | `<px:PXBranchSelector>` | BranchID |
| Employee | `<px:PXSelector>` with Employee search | EmployeeID |
| Mask | `<px:PXMaskEdit>` | Period (202401) |

### Step 4: Identify Layout Groups

Look for visual groupings or sections:

```aspx
<!-- Separate sections with PXLayoutRule -->
<px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M"/>
<!-- First column fields -->

<px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M"/>
<!-- Second column fields -->

<px:PXLayoutRule runat="server" StartRow="True" GroupCaption="Financial Information"/>
<!-- Grouped fields with caption -->
```

### Step 5: Determine Field Sizes

**Label Width** (`LabelsWidth`):
- `XS` - Extra Small (~50px)
- `S` - Small (~100px)
- `SM` - Small-Medium (~120px)
- `M` - Medium (~150px)
- `L` - Large (~200px)
- `XL` - Extra Large (~250px)
- `XXL` - Extra Extra Large (~300px)

**Control Size** (`ControlSize`):
- `XS`, `S`, `SM`, `M`, `L`, `XL`, `XXL` - Same progression
- Default (no ControlSize) - Auto-size based on field type

### Step 6: Map Actions/Buttons

Identify buttons in the toolbar or screen:

```aspx
<CallbackCommands>
    <px:PXDSCallbackCommand Name="Save" CommitChanges="True" />
    <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
    <px:PXDSCallbackCommand Name="Delete" PostData="Self" />
    <px:PXDSCallbackCommand Name="Release" Visible="false" CommitChanges="true" />
    <px:PXDSCallbackCommand Name="Cancel" Visible="false" CommitChanges="true" />
</CallbackCommands>
```

---

## Common Control Patterns

### 1. Reference Number (Key Field)

```aspx
<px:PXSelector ID="edRefNbr" runat="server" 
    DataField="RefNbr" 
    AutoRefresh="True" 
    CommitChanges="True" />
```

### 2. Status Dropdown

```aspx
<px:PXDropDown ID="edStatus" runat="server" 
    DataField="Status" 
    Enabled="False" />
```

### 3. Branch Selector

```aspx
<px:PXBranchSelector ID="edBranchID" runat="server" 
    DataField="BranchID" 
    CommitChanges="True" />
```

### 4. Customer/Vendor Selector

```aspx
<px:PXSegmentMask ID="edCustomerID" runat="server" 
    DataField="CustomerID" 
    CommitChanges="True" 
    AllowEdit="True" />
```

### 5. Date Field

```aspx
<px:PXDateTimeEdit ID="edTranDate" runat="server" 
    DataField="TranDate" 
    CommitChanges="True" />
```

### 6. Currency Amount

```aspx
<px:PXNumberEdit ID="edCuryAmount" runat="server" 
    DataField="CuryAmount" 
    CommitChanges="True" />
```

### 7. Account Selector

```aspx
<px:PXSegmentMask ID="edAccountID" runat="server" 
    DataField="AccountID" 
    CommitChanges="True" />
```

### 8. Checkbox

```aspx
<px:PXCheckBox ID="edHold" runat="server" 
    DataField="Hold" 
    CommitChanges="True" 
    AlignLeft="true" />
```

### 9. Period Selector

```aspx
<px:PXMaskEdit ID="edFinPeriod" runat="server" 
    DataField="FinPeriodID" 
    CommitChanges="True" />
```

### 10. Multi-line Text

```aspx
<px:PXTextEdit ID="edDescription" runat="server" 
    DataField="Description" 
    TextMode="MultiLine" 
    Height="60px" />
```

---

## Screen Examples

### Example 1: Simple Setup Screen (FormView.master)

**Screen**: ATPT1001 - Preferences

**Structure**:
- Master Page: `FormView.master`
- Single form with multiple sections
- No grids or tabs

**ASPX** (`ATPT1001.aspx`):
```aspx
<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" 
    AutoEventWireup="true" ValidateRequest="false" 
    CodeFile="ATPT1001.aspx.cs" Inherits="Page_ATPT1001" 
    Title="Preferences" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
    <px:PXDataSource ID="ds" runat="server" 
        Visible="True" 
        PrimaryView="Document" 
        SuspendUnloading="False" 
        TypeName="CashFundManagement.Graph.Setup.ATPTEFMSetupMaint">
    </px:PXDataSource>
</asp:Content>

<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
    <px:PXFormView ID="form" runat="server" 
        DataSourceID="ds" 
        Style="z-index: 100" 
        Width="100%" 
        DataMember="Document" 
        TabIndex="100">
        <Template>
            <!-- First Column -->
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="M"/>
            <px:PXCheckBox ID="edEnableFeature" runat="server" 
                DataField="EnableFeature" 
                CommitChanges="True" 
                AlignLeft="true" />
            
            <!-- BIR Setup Section -->
            <px:PXLayoutRule runat="server" 
                GroupCaption="BIR Setup" 
                StartColumn="True" 
                LabelsWidth="SM" 
                ControlSize="M"/>
            <px:PXTextEdit ID="edBusinessPermitNo" runat="server" 
                DataField="BusinessPermitNo" />
            <px:PXDateTimeEdit ID="edBusinessPermitDate" runat="server" 
                DataField="BusinessPermitDate" />
        </Template>
        <AutoSize Container="Window" Enabled="True" MinHeight="200" />
    </px:PXFormView>
</asp:Content>
```

**Code-Behind** (`ATPT1001.aspx.cs`):
```csharp
using System;
using PX.Web.UI;

public partial class Page_ATPT1001 : PXPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
}
```

---

### Example 2: List Maintenance Screen (ListView.master)

**Screen**: ATPT2001 - AR Subtypes Maintenance

**Structure**:
- Master Page: `ListView.master`
- Single grid with inline editing
- No header form

**ASPX** (`ATPT2001.aspx`):
```aspx
<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" 
    AutoEventWireup="true" ValidateRequest="false" 
    CodeFile="ATPT2001.aspx.cs" Inherits="Page_ATPT2001" 
    Title="AR Subtypes" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" 
        Visible="True" 
        Width="100%" 
        PrimaryView="Document" 
        TypeName="CashFundManagement.Graph.Setup.ATPTEFMARSubtypesMaint">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Save" CommitChanges="True" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>

<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
    <px:PXGrid ID="grid" runat="server" 
        Height="400px" 
        Width="100%" 
        Style="z-index: 100"
        AllowPaging="True" 
        AllowSearch="True" 
        AdjustPageSize="Auto" 
        DataSourceID="ds" 
        SkinID="Primary" 
        TabIndex="100">
        <Levels>
            <px:PXGridLevel DataKeyNames="SubtypeID" DataMember="Document">
                <RowTemplate>
                    <px:PXTextEdit ID="edSubtypeID" runat="server" 
                        DataField="SubtypeID" />
                    <px:PXTextEdit ID="edDescr" runat="server" 
                        DataField="Descr" />
                    <px:PXSelector ID="edNumberingID" runat="server" 
                        DataField="NumberingID" />
                </RowTemplate>
                <Columns>
                    <px:PXGridColumn DataField="SubtypeID" Width="140px" />
                    <px:PXGridColumn DataField="Descr" Width="280px" />
                    <px:PXGridColumn DataField="NumberingID" Width="120px" />
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="200" />        
    </px:PXGrid>
</asp:Content>
```

**Code-Behind** (`ATPT2001.aspx.cs`):
```csharp
using System;
using PX.Web.UI;

public partial class Page_ATPT2001 : PXPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
}
```

---

### Example 3: Transaction Entry Screen (FormTab.master)

**Screen**: ATPT3001 - BIR 2550M Entry

**Structure**:
- Master Page: `FormTab.master`
- Form header with document info
- Multiple tabs with different data sections

**ASPX** (`ATPT3001.aspx`):
```aspx
<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" 
    AutoEventWireup="true" ValidateRequest="false" 
    CodeFile="ATPT3001.aspx.cs" Inherits="Page_ATPT3001" 
    Title="BIR 2550M Entry" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" 
        Visible="True" 
        Width="100%" 
        TypeName="CashFundManagement.Graph.Transaction.ATPTBIR2550MEntry" 
        PrimaryView="Document" 
        PageLoadBehavior="InsertRecord">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="PrintSchedule" Visible="false" CommitChanges="true" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>

<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" 
        DataSourceID="ds" 
        Style="z-index: 100" 
        Width="100%" 
        DataMember="Document" 
        TabIndex="100">
        <Template>
            <px:PXLayoutRule runat="server" StartRow="True" StartColumn="True"/>
            <px:PXMaskEdit ID="edPeriod" runat="server" 
                DataField="Period" 
                CommitChanges="True" />
            
            <px:PXLayoutRule runat="server" ColumnSpan="2" />
            <px:PXTextEdit ID="edTaxPayerName" runat="server" 
                DataField="TaxPayerName" />
            
            <px:PXLayoutRule runat="server" StartColumn="True" />
            <px:PXTextEdit ID="edTin" runat="server" 
                DataField="Tin" />
        </Template>
    </px:PXFormView>
</asp:Content>

<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" 
        Width="100%" 
        Height="150px" 
        DataSourceID="ds" 
        DataMember="Document">
        <Items>
            <px:PXTabItem Text="Part 2.1">
                <Template>
                    <px:PXLayoutRule runat="server" 
                        StartColumn="True" 
                        LabelsWidth="XXL" 
                        StartRow="True" />
                    
                    <px:PXLabel ID="PXLabel1" runat="server">
                        12 Vatable Sales/Receipt - Private (Sch.1)
                    </px:PXLabel>
                    
                    <px:PXLayoutRule runat="server" 
                        LabelsWidth="XXS" 
                        StartColumn="True" />
                    <px:PXNumberEdit ID="edF12A" runat="server" 
                        DataField="F12A" 
                        CommitChanges="True" />
                </Template>
            </px:PXTabItem>
            
            <px:PXTabItem Text="Part 2.2">
                <Template>
                    <!-- Tab 2 content -->
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="200" />
    </px:PXTab>
</asp:Content>
```

**Code-Behind** (`ATPT3001.aspx.cs`):
```csharp
using System;
using PX.Web.UI;

public partial class Page_ATPT3001 : PXPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
}
```

---

### Example 4: Master-Detail Entry (FormDetail.master)

**Screen**: ATPT4001 - Fund Entry

**Structure**:
- Master Page: `FormDetail.master`
- Form header (master record)
- Grid below (detail records)

**ASPX** (`ATPT4001.aspx`):
```aspx
<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" 
    AutoEventWireup="true" ValidateRequest="false" 
    CodeFile="ATPT4001.aspx.cs" Inherits="Page_ATPT4001" 
    Title="Fund Entry" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" 
        Visible="True" 
        Width="100%" 
        TypeName="CashFundManagement.BLC.ATPTEFMFundEntry" 
        PrimaryView="Document">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand Name="Save" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="Delete" PostData="Self" />
            <px:PXDSCallbackCommand Name="Release" Visible="false" CommitChanges="true" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>

<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" 
        DataSourceID="ds" 
        Width="100%" 
        DataMember="Document">
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M"/>
            
            <px:PXSelector ID="edRefNbr" runat="server" 
                DataField="RefNbr" 
                AutoRefresh="True" />
            
            <px:PXDropDown ID="edStatus" runat="server" 
                DataField="Status" 
                Enabled="False" />
            
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M"/>
            
            <px:PXDateTimeEdit ID="edTranDate" runat="server" 
                DataField="TranDate" 
                CommitChanges="True" />
            
            <px:PXSegmentMask ID="edCustomerID" runat="server" 
                DataField="CustomerID" 
                CommitChanges="True" 
                AllowEdit="True" />
        </Template>
        <AutoSize Container="Window" Enabled="True" MinHeight="100" />
    </px:PXFormView>
</asp:Content>

<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid ID="grid" runat="server" 
        DataSourceID="ds" 
        Width="100%" 
        SkinID="Details" 
        TabIndex="200">
        <Levels>
            <px:PXGridLevel DataKeyNames="RefNbr,LineNbr" DataMember="Details">
                <RowTemplate>
                    <px:PXLayoutRule runat="server" StartColumn="True" />
                    <px:PXNumberEdit ID="edLineNbr" runat="server" 
                        DataField="LineNbr" />
                    <px:PXTextEdit ID="edDescription" runat="server" 
                        DataField="Description" />
                    <px:PXNumberEdit ID="edCuryAmount" runat="server" 
                        DataField="CuryAmount" />
                </RowTemplate>
                <Columns>
                    <px:PXGridColumn DataField="LineNbr" Width="70px" />
                    <px:PXGridColumn DataField="Description" Width="200px" />
                    <px:PXGridColumn DataField="CuryAmount" Width="100px" />
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="200" />
    </px:PXGrid>
</asp:Content>
```

**Code-Behind** (`ATPT4001.aspx.cs`):
```csharp
using System;
using PX.Web.UI;

public partial class Page_ATPT4001 : PXPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
}
```

---

## Best Practices

### 1. Naming Conventions
- **File names**: `ATPT####.aspx` and `ATPT####.aspx.cs`
- **Control IDs**: Use `ed` prefix for editable controls (e.g., `edRefNbr`, `edCustomerID`)
- **Grid IDs**: Use `grid` or descriptive name (e.g., `gridDetails`, `gridLines`)
- **Form IDs**: Use `form` for main form
- **Tab IDs**: Use `tab` for main tab control

### 2. DataField Binding
- Always match DAC field names exactly
- Use `CommitChanges="True"` for fields that trigger events or calculations
- Use `AutoRefresh="True"` on key selectors

### 3. Layout Rules
- Start each column with `<px:PXLayoutRule runat="server" StartColumn="True"/>`
- Group related fields using `GroupCaption`
- Use appropriate `LabelsWidth` and `ControlSize` for visual consistency

### 4. AutoSize
- Always include `<AutoSize Container="Window" Enabled="True" MinHeight="200" />` on forms and grids
- This ensures proper responsive behavior

### 5. TabIndex
- Set `TabIndex` to control tab order (100, 200, 300, etc.)
- Forms typically start at 100, grids at 200

### 6. Performance
- Use `SuspendUnloading="False"` on PXDataSource if screen doesn't require complex state management
- Set `AllowPaging="True"` and `AdjustPageSize="Auto"` on large grids
- Use `SkinID="Primary"` for main grids, `SkinID="Details"` for detail grids

### 7. Validation
- After creating screen files, publish customization to test
- Test all actions and callbacks
- Verify field bindings and calculations
- Test with actual data

---

## Troubleshooting

### Common Issues

1. **Screen doesn't load**
   - Verify `TypeName` in PXDataSource matches Graph namespace and class name
   - Ensure Graph is compiled and published

2. **Fields don't display data**
   - Check `DataMember` matches View name in Graph
   - Verify `DataField` matches DAC property name exactly (case-sensitive)

3. **Actions don't appear**
   - Add `PXDSCallbackCommand` for custom actions
   - Verify action is defined in Graph and has `[PXButton]` attribute

4. **Grid doesn't show data**
   - Check `DataKeyNames` matches DAC key fields
   - Verify `DataMember` matches detail view name

5. **Layout issues**
   - Review `PXLayoutRule` placements
   - Check `LabelsWidth` and `ControlSize` settings
   - Verify `ColumnSpan` usage

5. **Screen primary tool bar don't appear**
   - Verify that `Visible` property is defined and set to True on the `PXDataSource` definition. 


---

## Additional Resources

- **Acumatica Screen Editor**: Use for visual design (Tools > Customize > Screen Editor)
- **Inspect Element**: Use browser dev tools to inspect generated HTML
- **Reference Screens**: Study standard Acumatica screens for patterns
- **Developer Documentation**: https://help.acumatica.com/

---

## Summary Checklist

When creating a new custom screen:

- [ ] Determine screen ID (ATPT####)
- [ ] Identify master page (FormView/ListView/FormTab/FormDetail)
- [ ] Create ASPX file with correct directives
- [ ] Define PXDataSource with correct TypeName and PrimaryView
- [ ] Add CallbackCommands for custom actions
- [ ] Create form/grid/tab structure based on requirements
- [ ] Map all fields to DAC properties
- [ ] Apply appropriate control types
- [ ] Set layout rules for visual organization
- [ ] Create code-behind file (.aspx.cs)
- [ ] Test screen after publishing
- [ ] Verify all functionality (CRUD operations, actions, calculations)

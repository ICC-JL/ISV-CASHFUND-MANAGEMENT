---
name: acumatica-mobile
description: Use when customizing the Acumatica mobile app via MSDL, mobile sitemaps, workspaces, generic inquiries, dashboards, actions, attachments, signatures, reports, smart panels, redirections, or Workflow API exposure. Triggers on keywords such as mobile app, MSDL, mobile sitemap, screen mapping, Expose to Mobile, IsExposedToMobile.
---

# Acumatica Mobile App Customization Skill

This skill covers customization of the Acumatica mobile app using the **Mobile Site Map Definition Language (MSDL)** and related platform tools. It is derived from the Acumatica T400 Basic Customization of the Mobile App and T410 Advanced Customization of the Mobile App courses (2026 R1).

## Core Concepts

- **MSDL (Mobile Site Map Definition Language):** A declarative language used in the Customization Project Editor (Mobile Application page) to define how Acumatica ERP forms appear and behave in the mobile app.
- **WSDL Schema:** Describes every action, field, container, and complex type of an Acumatica ERP form. Used to discover the names needed for mapping.
- **Element Inspector:** In Acumatica ERP, press `Ctrl + Alt` and click a UI element to see its Action Name and other properties.
- **Mobile Site Map:** Defines screens and their metadata. The global site map is updated via `update sitemap { ... }`.
- **Mobile Workspaces:** Group screens in the mobile app. Managed via the **Mobile Workspaces (AU220012)** form and saved into the customization project.

## MSDL Object Hierarchy

```text
screen
  container
    layout
      group
        field
    containerAction / recordAction / listAction / selectionAction
  dialog
    dialogAction
    container
      field
      recordAction
```

## Common MSDL Patterns

### Add a New Screen (Generic Inquiry)

```msdl
add screen GI000023 {
  add container "Result" {
    add field "Name"
    add field "Subject"
    add field "Status"
    add field "Type"
  }
}
```

### Add a Generic Inquiry with Parameters

```msdl
add screen GI400001 {
  type = FilterListScreen
  add container "Filter_" {
    add field "DateFrom"
    add field "DateTo"
    add field "Customer"
    add field "OpenOnly"
  }
  add container "Result" {
    add field "CustomerID"
    add field "OrderNbr"
    add field "Quantity"
    add field "SalesOrderTotal"
    add field "Status"
  }
}
```

### Update an Existing Screen

```msdl
update screen SO301000 {
  update container "OrderSummary" {
    update layout "OrderHeader" {
      add layout "OrderHeaderDates" {
        displayName = "OrderHeaderDateRow"
        layout = "Inline"
        add field "Date"
        add field "RequestedOn"
      }
    }
  }
}
```

### Remove a Field

```msdl
update screen SO301000 {
  update container "OrderSummary" {
    update layout "OrderSettingsTab" {
      update group "BillToInfoGroup" {
        remove field "AddressesBillToAddress#AddressLine2"
      }
    }
  }
}
```

### Add Standard Actions

```msdl
add container "InvoiceSummary" {
  add recordAction "Save" {
    behavior = Save
  }
  add recordAction "Cancel" {
    behavior = Cancel
  }
  add containerAction "Insert" {
    behavior = Create
  }
  add recordAction "Release" {
    behavior = Record
  }
}
```

### Layout, Groups, and Tabs

```msdl
add layout "InvoiceHeader" {
  layout = "HeaderSimple"
  add layout "InvoiceHeaderRow1" {
    layout = "Inline"
    add field "Type"
    add field "Date"
  }
}

add group "FinancialDetails" {
  displayName = "Financial Details"
  collapsable = True
  collapsed = True
  add field "FinancialLinkToGL#ARAccount"
}

add layout "Details" {
  layout = DataTab
  add containerLink "Details" {
    control = "ListItem"
  }
}
```

### Many-to-One / Selection Container

```msdl
add container "AddOrder" {
  type = SelectionActionList
  visible = False
  listActionsToExpand = 1
  add field "Selected" {
    special = ListSelection
  }
  add field "OrderType"
  add listAction "AddShipment" {
    icon = "system://Check"
    behavior = Void
  }
}
```

Open it from another container:

```msdl
add containerAction "SelectShipment" {
  behavior = Void
  redirect = True
  redirectToContainer = "AddOrder$List"
}
```

### Smart Panel (dialog)

```msdl
add dialog QuickProcessD {
  add dialogAction "Process" {
    CloseDialog = True
    DialogResult = OK
    DisplayName = "Process"
  }
  openAs = Form
  add container "ProcessOrder" {
    includeDialogActions = true
    add field "WarehouseID"
    add recordAction "Process" {
      icon = "system://Check"
    }
  }
}

update container "OrderSummary" {
  add recordAction "QuickProcess" {
    redirect = true
    redirectToDialog = "QuickProcessD"
  }
}
```

### Update Mobile Site Map

```msdl
update sitemap {
  add item "GI000023" {
    displayName = "Sales Activities"
  }
}
```

### Folder for Built-In Filters

```msdl
add folder "InvoicedItems" {
  type = HubFolder
  displayName = "Invoiced Items"
  add item "GI000008" {
    displayName = "Invoiced Items"
  }
}
```

### Screen Types

- `type = Dashboard` — dashboard screen.
- `type = FilterListScreen` — generic inquiry with parameters.
- `type = Report` — report screen.
- `type = SelectionActionList` — selection list / many-to-one container.
- `openAs = Form` — open dialog as a separate screen.

### Attachments

```msdl
attachments {
  add type "jpg" {
    extension = "jpg"
  }
  add type "png" {
    extension = "png"
  }
}
```

Receipt enhancement:

```msdl
attachments {
  imageAdjustmentPreset = Receipt
}
```

### Signature Action

```msdl
add recordAction "SignReport" {
  behavior = SignReport
  displayName = "Sign"
}
```

### Report Action

```msdl
add recordAction "PrintSalesOrder" {
  redirect = True
}
```

And the report screen:

```msdl
add screen SO641010 {
  type = Report
}

update sitemap {
  add item "SO641010" {
    visible = False
  }
}
```

## Workflow API Exposure (C#)

When an action cannot be exposed through MSDL alone, or you need to expose existing workflow actions:

```csharp
using PX.Data;
using PX.Data.WorkflowAPI;
using PX.Objects.PM;

public class ProjectEntry_CustomizedWorkflow : PXGraphExtension<
    ProjectEntry_Workflow,
    ProjectEntry>
{
    public override void Configure(PXScreenConfiguration config)
    {
        Configure(config.GetScreenConfigurationContext<ProjectEntry, PMProject>());
    }

    protected virtual void Configure(WorkflowContext<ProjectEntry, PMProject> context)
    {
        context.UpdateScreenConfigurationFor(screen => screen
            .WithActions(actions =>
            {
                actions.Update(
                    g => g.lockBudget,
                    a => a.IsExposedToMobile(true));
            })
        );
    }
}
```

## Discovering Object Names

1. Open the Acumatica ERP form.
2. Click **Settings > Web Service > Service Description** to view the WSDL schema.
3. Use the **Content** complex type to find container names.
4. Use each container's complex type to find field names.
5. Use the **Actions** complex type to find action names.
6. For UI elements, use the **Element Inspector** (`Ctrl + Alt + click`) to read the Action Name.

## Key Conventions

- Container names usually match the tab name; the primary container is often `<FormTitle>Summary`.
- Reference a field from another container with `ContainerName#FieldName`.
- Always map `Save` and `Cancel` actions for editable screens.
- Map `Cancel` wherever it exists in the WSDL schema so discarded changes are properly rolled back on the server.
- `redirectToContainer` value usually ends with `$List` when opening a secondary container as a list.
- `dialogAction` must have a matching action inside the dialog's container when `includeDialogActions = true` is used.

## Common Pitfalls

- Adding a screen to the site map is not enough — add it to a workspace via **Mobile Workspaces (AU220012)** or it appears only in **Other**.
- Built-in filters on generic inquiries can create multiple Home-screen entries; group them with a `HubFolder`.
- For complex smart panels (anything beyond a simple table), use the `dialog` object, not just a container.
- Actions exposed through the **Actions** page of the Customization Project Editor are equivalent to Workflow API exposure.

## Testing Checklist

1. Save the MSDL script and verify no errors appear in the Errors area.
2. Publish the customization project.
3. Sign out and sign back into the mobile app to refresh the site map.
4. Navigate to the target workspace and open the screen.
5. Verify fields, layouts, actions, tabs, and redirections behave as expected.

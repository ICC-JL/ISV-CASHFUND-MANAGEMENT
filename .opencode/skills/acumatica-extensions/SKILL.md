---
name: acumatica-extensions
description: Use when creating graph extensions (PXGraphExtension) or DAC extensions (PXCacheExtension) to customize existing Acumatica functionality.
---

# Acumatica Extensions Skill

## Graph Extension

```csharp
public class ATPTEFMAPInvoiceEntryExtension : PXGraphExtension<APInvoiceEntry>
{
    public override void Initialize()
    {
        base.Initialize();
        // Initialization logic
    }
    
    protected virtual void _(Events.RowPersisting<APInvoice> e)
    {
        if (e.Row == null) return;
        
        // Custom validation
        if (e.Row.DocType == APDocType.Invoice)
        {
            // Validate custom logic
        }
    }
    
    protected virtual void _(Events.RowSelected<APInvoice> e)
    {
        if (e.Row == null) return;
        
        // UI customization
        PXUIFieldAttribute.SetVisible<APInvoiceExtension.usrATPTEFMCustomField>(
            e.Cache, e.Row, true);
    }
}
```

## DAC Extension

```csharp
public sealed class ATPTEFMAPInvoiceExtension : PXCacheExtension<APInvoice>
{
    #region UsrATPTEFMCustomField
    [PXDBString(50, IsUnicode = true)]
    [PXUIField(DisplayName = ATPTEFMMessages.CustomField)]
    public string UsrATPTEFMCustomField { get; set; }
    public abstract class usrATPTEFMCustomField : PX.Data.BQL.BqlString.Field<usrATPTEFMCustomField> { }
    #endregion
}
```

## Extension Registration
In the customization project or extension library, register extensions:
```csharp
public class ATPTEFMExtensionSetup : PXGraphExtension<APInvoiceEntry>
{
    public static void Initialize()
    {
        // Register extension
    }
}
```

## Common Extension Patterns

### Override Base Graph Action
```csharp
public delegate IEnumerable ReleaseDelegate(PXAdapter adapter);
[PXOverride]
public IEnumerable Release(PXAdapter adapter, ReleaseDelegate baseMethod)
{
    // Pre-processing
    var result = baseMethod(adapter);
    // Post-processing
    return result;
}
```

### Add Custom View to Base Graph
```csharp
public PXSelect<ATPTEFMCustomTable,
    Where<ATPTEFMCustomTable.refNbr.IsEqual<APInvoice.refNbr.FromCurrent>>> CustomView;
```

### Access Base Graph Members
```csharp
public APInvoiceEntry BaseGraph => Base;
```

### Extend Existing Event Handler
```csharp
protected virtual void APInvoice_RowSelected(PXCache cache, PXRowSelectedEventArgs e)
{
    if (e.Row == null) return;
    
    // Call base handler if needed
    // base.APInvoice_RowSelected(cache, e);
    
    // Custom logic
    APInvoice doc = (APInvoice)e.Row;
    if (doc.DocType == APDocType.Invoice)
    {
        // Custom UI logic
    }
}
```

### Add Custom Button to Existing Screen
```csharp
public PXAction<APInvoice> customAction;
[PXButton(CommitChanges = true)]
[PXUIField(DisplayName = ATPTEFMMessages.CustomAction, MapEnableRights = PXCacheRights.Update)]
protected virtual IEnumerable CustomAction(PXAdapter adapter)
{
    APInvoice doc = Documents.Current;
    if (doc == null) return adapter.Get();
    
    // Custom action logic
    
    return adapter.Get();
}
```

## Extension Best Practices
- Always call `base.Initialize()` in override
- Check `e.Row != null` in all event handlers
- Use `PXUIFieldAttribute` for UI customizations
- Respect base graph's transaction scope
- Avoid conflicting with other extensions

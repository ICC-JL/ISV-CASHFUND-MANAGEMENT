---
name: acumatica-graph
description: Use when creating or modifying Acumatica Graphs/BLCs, implementing event handlers, actions, or business logic. Covers RowSelected, FieldDefaulting, FieldUpdated, actions, and transaction handling.
---

# Acumatica Graph/BLC Development Skill

## Complete Graph Template

```csharp
public class ATPTEFMEntityMaint : PXGraph<ATPTEFMEntityMaint, ATPTEFMEntity>
{
    #region Views
    [PXViewName(ATPTEFMMessages.Entity)]
    public SelectFrom<ATPTEFMEntity>.View Document;
    
    [PXViewName(ATPTEFMMessages.EntityDetails)]
    public SelectFrom<ATPTEFMEntityDetail>.Where<ATPTEFMEntityDetail.FK.Entity.SameAsCurrent>.View Details;
    #endregion
    
    #region Event Handlers
    protected virtual void _(Events.RowSelected<ATPTEFMEntity> e)
    {
        if (e.Row == null) return;
        // UI logic here
    }
    
    protected virtual void _(Events.FieldDefaulting<ATPTEFMEntity.status> e)
    {
        if (e.Row == null) return;
        e.NewValue = ATPTEFMStatus.Hold;
    }
    
    protected virtual void _(Events.FieldUpdated<ATPTEFMEntity.branchID> e)
    {
        if (e.Row == null) return;
        // Update related fields
    }
    
    protected virtual void _(Events.RowPersisting<ATPTEFMEntity> e)
    {
        if (e.Row == null) return;
        // Validation before save
    }
    #endregion
    
    #region Actions
    public PXAction<ATPTEFMEntity> release;
    [PXButton(CommitChanges = true)]
    [PXUIField(DisplayName = ATPTEFMMessages.Release, MapEnableRights = PXCacheRights.Update)]
    protected virtual IEnumerable Release(PXAdapter adapter)
    {
        // Release logic here
        return adapter.Get();
    }
    #endregion
}
```

## Event Handler Patterns

### RowSelected - UI Logic
```csharp
protected virtual void _(Events.RowSelected<ATPTEFMEntity> e)
{
    if (e.Row == null) return;
    
    bool isReleased = e.Row.Status == ATPTEFMStatus.Released;
    PXUIFieldAttribute.SetEnabled<ATPTEFMEntity.description>(e.Cache, e.Row, !isReleased);
}
```

### FieldDefaulting - Set Defaults
```csharp
protected virtual void _(Events.FieldDefaulting<ATPTEFMEntity.status> e)
{
    if (e.Row == null) return;
    e.NewValue = ATPTEFMStatus.Hold;
}
```

### FieldUpdated - Update Related Fields
```csharp
protected virtual void _(Events.FieldUpdated<ATPTEFMEntity.branchID> e)
{
    if (e.Row == null) return;
    e.Cache.SetValueExt<ATPTEFMEntity.customerID>(e.Row, null);
}
```

### FieldVerifying - Field Validation
```csharp
protected virtual void _(Events.FieldVerifying<ATPTEFMEntity.amount> e)
{
    if (e.Row == null) return;
    decimal? amount = (decimal?)e.NewValue;
    if (amount < 0)
    {
        e.Cache.RaiseExceptionHandling<ATPTEFMEntity.amount>(
            e.Row, amount, 
            new PXSetPropertyException(ATPTEFMMessages.AmountMustBePositive, PXErrorLevel.Error));
    }
}
```

### RowUpdating - Row Validation
```csharp
protected virtual void _(Events.RowUpdating<ATPTEFMEntity> e)
{
    ATPTEFMEntity row = e.NewRow;
    if (row == null) return;
    
    if (row.Status == ATPTEFMStatus.Released && row.Amount <= 0)
    {
        throw new PXException(ATPTEFMMessages.ReleasedDocumentMustHaveAmount);
    }
}
```

### RowPersisting - Pre-Save Validation
```csharp
protected virtual void _(Events.RowPersisting<ATPTEFMEntity> e)
{
    if (e.Row == null) return;
    
    if (string.IsNullOrEmpty(e.Row.Description))
    {
        e.Cancel = true;
        throw new PXException(ATPTEFMMessages.DescriptionRequired);
    }
}
```

### RowInserted - Insert Defaults
```csharp
protected virtual void _(Events.RowInserted<ATPTEFMEntity> e)
{
    if (e.Row == null) return;
    
    // Insert default detail records
    ATPTEFMEntityDetail detail = new ATPTEFMEntityDetail();
    detail.RefNbr = e.Row.RefNbr;
    Details.Insert(detail);
}
```

## Error Handling
```csharp
// Field-level error
e.Cache.RaiseExceptionHandling<ATPTEFMEntity.amount>(
    e.Row, e.Row.Amount, 
    new PXSetPropertyException(ATPTEFMMessages.AmountMustBePositive, PXErrorLevel.Error));

// Row-level error
throw new PXException(ATPTEFMMessages.DocumentCannotBeDeleted);

// Warning
PXTrace.WriteWarning(ATPTEFMMessages.WarningMessage);
```

## Transaction Handling
```csharp
using (PXTransactionScope ts = new PXTransactionScope())
{
    // Multiple operations
    graph.Actions.PressSave();
    ts.Complete();
}
```

## Performance Tips
- Use `PXSelectReadonly` for read-only queries
- Implement proper database indexing
- Avoid N+1 queries - use joins
- Use `PXDatabase.SelectMulti` for bulk operations
- Cache frequently accessed data with `PXSelect.StoreCached`

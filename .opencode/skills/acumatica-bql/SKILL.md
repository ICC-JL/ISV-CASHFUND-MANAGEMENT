---
name: acumatica-bql
description: Use when writing or reviewing Fluent BQL queries, defining graph views, or working with data access queries in Acumatica. Covers FK patterns, joins, aggregates, and view definitions.
---

# Acumatica Fluent BQL Skill

## Critical Rules
1. **Always use Fluent BQL** - Standard BQL is deprecated
2. **Always use FK classes** - Never manually define relationships
3. **Use `.SameAsCurrent`** for master-detail Where clauses
4. **Use `.FromCurrent`** for field comparisons

## View Definitions

### Primary View
```csharp
[PXViewName(ATPTEFMMessages.Entity)]
public SelectFrom<ATPTEFMEntity>.View Document;
```

### Detail with FK.SameAsCurrent
```csharp
[PXViewName(ATPTEFMMessages.EntityDetails)]
public SelectFrom<ATPTEFMEntityDetail>
    .Where<ATPTEFMEntityDetail.FK.Entity.SameAsCurrent>
    .View Details;
```

### Detail with FromCurrent
```csharp
[PXViewName(ATPTEFMMessages.EntityDetails)]
public SelectFrom<ATPTEFMEntityDetail>
    .Where<ATPTEFMEntityDetail.refNbr.IsEqual<ATPTEFMEntity.refNbr.FromCurrent>>
    .View Details;
```

### View with Join
```csharp
[PXViewName(ATPTEFMMessages.Customers)]
public SelectFrom<Customer>
    .InnerJoin<ATPTEFMEntity>.On<Customer.bAccountID.IsEqual<ATPTEFMEntity.customerID.FromCurrent>>
    .View Customers;
```

## Data Access Patterns

### Simple Select
```csharp
SelectFrom<ATPTEFMEntity>.View.Select(this);
```

### With Where and parameters
```csharp
SelectFrom<ATPTEFMEntity>
    .Where<ATPTEFMEntity.status.IsEqual<@P.AsString>>
    .View.Select(this, ATPTEFMStatus.Released);
```

### With FK Join
```csharp
SelectFrom<ATPTEFMEntity>
    .InnerJoin<Customer>.On<ATPTEFMEntity.FK.Customer>
    .Where<ATPTEFMEntity.status.IsEqual<@P.AsString>>
    .View.Select(this, ATPTEFMStatus.Released);
```

### With Multiple Joins
```csharp
SelectFrom<ATPTEFMEntity>
    .InnerJoin<Customer>.On<ATPTEFMEntity.FK.Customer>
    .InnerJoin<Branch>.On<ATPTEFMEntity.FK.Branch>
    .Where<ATPTEFMEntity.status.IsEqual<@P.AsString>>
    .View.Select(this, ATPTEFMStatus.Released);
```

### With OrderBy
```csharp
SelectFrom<ATPTEFMEntity>
    .Where<ATPTEFMEntity.status.IsEqual<@P.AsString>>
    .OrderBy<ATPTEFMEntity.tranDate.Desc>
    .View.Select(this, ATPTEFMStatus.Released);
```

### Single Record
```csharp
SelectFrom<ATPTEFMEntity>
    .Where<ATPTEFMEntity.refNbr.IsEqual<@P.AsString>>
    .View.SelectSingleBound(this, null, refNbr);
```

### Aggregate
```csharp
SelectFrom<ATPTEFMEntity>
    .Where<ATPTEFMEntity.status.IsEqual<@P.AsString>>
    .AggregateTo<GroupBy<ATPTEFMEntity.customerID>, Sum<ATPTEFMEntity.curyAmount>>
    .View.Select(this, ATPTEFMStatus.Released);
```

### Complex Query
```csharp
SelectFrom<ATPTEFMEntityDetail>
    .InnerJoin<InventoryItem>.On<ATPTEFMEntityDetail.FK.InventoryItem>
    .InnerJoin<ATPTEFMEntity>.On<ATPTEFMEntityDetail.FK.Entity>
    .Where<ATPTEFMEntity.refNbr.IsEqual<@P.AsString>
        .And<ATPTEFMEntityDetail.lineNbr.IsGreater<@P.AsInt>>>
    .OrderBy<ATPTEFMEntityDetail.lineNbr.Asc>
    .View.Select(this, refNbr, 0);
```

## FK Class Pattern
```csharp
public static class FK
{
    public class Customer : PrimaryKeyOf<Customer>.By<Customer.bAccountID>.ForeignKeyOf<ATPTEFMEntity>.By<customerID>
    { }
}
```

## Performance Tips
- Use `PXSelectReadonly` for read-only queries
- Avoid N+1 queries - use joins
- Use `PXDatabase.SelectMulti` for bulk operations
- Cache frequently accessed data with `PXSelect.StoreCached`

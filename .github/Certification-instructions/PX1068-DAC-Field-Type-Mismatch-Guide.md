# PX1068: DAC Field Property Type Mismatch - Resolution Guide

## Overview
**Error Code:** PX1068  
**Message:** "The type of the DAC field property does not correspond to the type of the BQL field."

This error occurs when the C# property type in a DAC doesn't match the corresponding BQL field type in the abstract class declaration.

---

## Understanding the Error

Each DAC field consists of two parts:
1. **Property declaration** - the C# property with its type (`string`, `int?`, `decimal?`, etc.)
2. **BQL field declaration** - the abstract class that inherits from a BQL field type

These two MUST match according to the type mapping table below.

---

## Type Mapping Reference

| C# Property Type | BQL Field Type | PX Attribute Examples |
|------------------|----------------|----------------------|
| `string` | `BqlString.Field<>` | `[PXDBString]`, `[PXString]` |
| `int?` | `BqlInt.Field<>` | `[PXDBInt]`, `[PXInt]` |
| `decimal?` | `BqlDecimal.Field<>` | `[PXDBDecimal]`, `[PXDecimal]`, `[PXDBCurrency]` |
| `bool?` | `BqlBool.Field<>` | `[PXDBBool]`, `[PXBool]` |
| `DateTime?` | `BqlDateTime.Field<>` | `[PXDBDate]`, `[PXDate]`, `[PXDBDateAndTime]` |
| `Guid?` | `BqlGuid.Field<>` | `[PXDBGuid]`, `[PXGuid]`, `[PXNote]` |
| `short?` | `BqlShort.Field<>` | `[PXDBShort]`, `[PXShort]` |
| `long?` | `BqlLong.Field<>` | `[PXDBLong]`, `[PXLong]` |
| `byte?` | `BqlByte.Field<>` | `[PXDBByte]`, `[PXByte]` |
| `double?` | `BqlDouble.Field<>` | `[PXDBDouble]`, `[PXDouble]` |
| `byte[]` | `BqlByteArray.Field<>` | `[PXDBBinary]` |

---

## Common Mistakes and Fixes

### Mistake 1: Wrong BQL Type for String Property
```csharp
// ❌ INCORRECT - Property is string but BQL is BqlInt
[PXDBString(30)]
public virtual string MyField { get; set; }
public abstract class myField : BqlInt.Field<myField> { }

// ✅ CORRECT
[PXDBString(30)]
public virtual string MyField { get; set; }
public abstract class myField : BqlString.Field<myField> { }
```

### Mistake 2: Wrong BQL Type for Decimal Property
```csharp
// ❌ INCORRECT - Property is decimal but BQL is BqlString
[PXDBDecimal(2)]
public virtual decimal? Amount { get; set; }
public abstract class amount : BqlString.Field<amount> { }

// ✅ CORRECT
[PXDBDecimal(2)]
public virtual decimal? Amount { get; set; }
public abstract class amount : BqlDecimal.Field<amount> { }
```

### Mistake 3: Wrong BQL Type for Int Property
```csharp
// ❌ INCORRECT - Property is int but BQL is BqlString
[PXDBInt]
public virtual int? LineNbr { get; set; }
public abstract class lineNbr : BqlString.Field<lineNbr> { }

// ✅ CORRECT
[PXDBInt]
public virtual int? LineNbr { get; set; }
public abstract class lineNbr : BqlInt.Field<lineNbr> { }
```

### Mistake 4: Wrong BQL Type for Bool Property
```csharp
// ❌ INCORRECT - Property is bool but BQL is BqlInt
[PXDBBool]
public virtual bool? IsActive { get; set; }
public abstract class isActive : BqlInt.Field<isActive> { }

// ✅ CORRECT
[PXDBBool]
public virtual bool? IsActive { get; set; }
public abstract class isActive : BqlBool.Field<isActive> { }
```

### Mistake 5: Wrong BQL Type for DateTime Property
```csharp
// ❌ INCORRECT - Property is DateTime but BQL is BqlString
[PXDBDate]
public virtual DateTime? DocDate { get; set; }
public abstract class docDate : BqlString.Field<docDate> { }

// ✅ CORRECT
[PXDBDate]
public virtual DateTime? DocDate { get; set; }
public abstract class docDate : BqlDateTime.Field<docDate> { }
```

### Mistake 6: Wrong BQL Type for Guid Property
```csharp
// ❌ INCORRECT - Property is Guid but BQL is BqlString
[PXNote]
public virtual Guid? NoteID { get; set; }
public abstract class noteID : BqlString.Field<noteID> { }

// ✅ CORRECT
[PXNote]
public virtual Guid? NoteID { get; set; }
public abstract class noteID : BqlGuid.Field<noteID> { }
```

---

## How to Fix PX1068 Errors

### Step 1: Identify the Mismatch
Look at the error message which will point to the specific field. Compare:
- The C# property type (e.g., `string`, `int?`, `decimal?`)
- The BQL field type (e.g., `BqlString.Field`, `BqlInt.Field`)

### Step 2: Determine the Correct Type
Use the type mapping table above to determine what the BQL type should be based on:
- The property type
- The attribute used (e.g., `[PXDBString]` = string = `BqlString.Field`)

### Step 3: Update the BQL Field Declaration
Change the abstract class to inherit from the correct BQL field type.

---

## Using Required Namespace

Make sure to include the proper namespace for BQL types:
```csharp
using PX.Data.BQL;
```

Or use fully qualified names:
```csharp
public abstract class myField : PX.Data.BQL.BqlString.Field<myField> { }
```

---

## Quick Detection Script (PowerShell)

Use this PowerShell script to find potential PX1068 issues in your codebase:

```powershell
# PX1068 Detection Script
# Searches for common type mismatches in DAC files

$projectPath = "C:\Your\Project\Path"
$dacFiles = Get-ChildItem -Path $projectPath -Filter "*.cs" -Recurse

foreach ($file in $dacFiles) {
    $content = Get-Content $file.FullName -Raw
    
    # Pattern to find regions with both property and abstract class
    $regions = [regex]::Matches($content, '#region\s+(\w+).*?#endregion', [System.Text.RegularExpressions.RegexOptions]::Singleline)
    
    foreach ($region in $regions) {
        $regionContent = $region.Value
        
        # Check for string property with non-BqlString
        if ($regionContent -match 'virtual\s+string\s*\?' -and $regionContent -match 'Bql(?!String)\.Field') {
            Write-Host "Potential mismatch in $($file.Name): string property with non-BqlString field"
        }
        
        # Check for int property with non-BqlInt  
        if ($regionContent -match 'virtual\s+int\s*\?' -and $regionContent -match 'Bql(?!Int)\.Field') {
            Write-Host "Potential mismatch in $($file.Name): int? property with non-BqlInt field"
        }
        
        # Check for decimal property with non-BqlDecimal
        if ($regionContent -match 'virtual\s+decimal\s*\?' -and $regionContent -match 'Bql(?!Decimal)\.Field') {
            Write-Host "Potential mismatch in $($file.Name): decimal? property with non-BqlDecimal field"
        }
        
        # Check for bool property with non-BqlBool
        if ($regionContent -match 'virtual\s+bool\s*\?' -and $regionContent -match 'Bql(?!Bool)\.Field') {
            Write-Host "Potential mismatch in $($file.Name): bool? property with non-BqlBool field"
        }
        
        # Check for DateTime property with non-BqlDateTime
        if ($regionContent -match 'virtual\s+DateTime\s*\?' -and $regionContent -match 'Bql(?!DateTime)\.Field') {
            Write-Host "Potential mismatch in $($file.Name): DateTime? property with non-BqlDateTime field"
        }
        
        # Check for Guid property with non-BqlGuid
        if ($regionContent -match 'virtual\s+Guid\s*\?' -and $regionContent -match 'Bql(?!Guid)\.Field') {
            Write-Host "Potential mismatch in $($file.Name): Guid? property with non-BqlGuid field"
        }
    }
}

Write-Host "Scan complete."
```

---

## Checklist for ISV Certification

- [ ] All string properties use `BqlString.Field<>`
- [ ] All int? properties use `BqlInt.Field<>`
- [ ] All decimal? properties use `BqlDecimal.Field<>`
- [ ] All bool? properties use `BqlBool.Field<>`
- [ ] All DateTime? properties use `BqlDateTime.Field<>`
- [ ] All Guid? properties use `BqlGuid.Field<>`
- [ ] All long? properties use `BqlLong.Field<>`
- [ ] All short? properties use `BqlShort.Field<>`
- [ ] Run Acuminator analysis with no PX1068 errors
- [ ] Verify Filter classes in Graph files
- [ ] Verify Projection DACs
- [ ] Verify DAC Extensions

---

## Special Cases

### Selector Attributes
When using selector attributes like `[Customer]`, `[Inventory]`, `[Account]`, the underlying type is usually `int?`, so use `BqlInt.Field<>`:

```csharp
[Customer]
public virtual int? CustomerID { get; set; }
public abstract class customerID : BqlInt.Field<customerID> { }
```

### PXDBScalar Fields
For calculated/formula fields using `[PXDBScalar]`, match the BQL type to the return type of the scalar expression.

### Unbound Fields
Unbound fields (using `[PXString]`, `[PXInt]`, etc. without DB prefix) follow the same rules.

---

## Related Acuminator Errors

| Error Code | Description |
|------------|-------------|
| PX1068 | Property type doesn't match BQL field type |
| PX1019 | Missing abstract class for DAC field |
| PX1021 | Invalid DAC field definition |

---

## References

- [Acumatica DAC Documentation](https://help.acumatica.com/)
- [Acuminator GitHub](https://github.com/Acumatica/Acuminator)
- [BQL Types Reference](https://help.acumatica.com/Help?ScreenId=ShowWiki&pageid=5e8dc3c3-47a2-4c9c-a589-d8f3ca2c9c6d)

---

**Document Version:** 1.0  
**Last Updated:** January 2026  
**Applicable Acumatica Versions:** 2023 R2+

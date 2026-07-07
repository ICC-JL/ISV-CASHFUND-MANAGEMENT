# Fixing PX1065: Missing BQL Field Error

## Overview

This guide provides step-by-step instructions for fixing PX1065 Acuminator diagnostic to meet Acumatica certification requirements.

---

## What Is PX1065?

**Error Code**: PX1065  
**Severity**: Error  
**Message**: The DAC field property does not have a corresponding BQL field.

**Why It Matters**: Acumatica Framework requires all DAC field properties to have a corresponding BQL field (public abstract class). Missing BQL fields can cause runtime errors or unexpected behavior in numerous places in the application code.

---

## How to Identify PX1065 Errors

### In Visual Studio

1. **Build the solution**
2. **Open Error List** (View → Error List)
3. **Filter by** "PX1065" or "Acuminator"
4. Errors appear on DAC field properties that don't have a matching BQL field

### What Triggers the Error

The error appears on C# properties with Acumatica attributes (like `[PXDBString]`, `[PXDBInt]`, `[PXDBBool]`, etc.) that don't have a corresponding BQL field above them.

**Example of PX1065 Error**:

```csharp
public class ATPTEFMSample : IBqlTable
{
    // ❌ PX1065 Error on this property - Missing BQL field
    [PXDBString(15, IsKey = true, IsUnicode = true)]
    [PXUIField(DisplayName = "Reference Nbr")]
    public virtual string RefNbr { get; set; }
}
```

---

## How to Fix PX1065

### Step-by-Step Fix Instructions

**Step 1**: Identify the DAC field property that has the PX1065 error

- Look for properties with Acumatica attributes (`[PXDBString]`, `[PXDBInt]`, `[PXDBBool]`, etc.)
- Verify there's no BQL field (public abstract class) above it

**Step 2**: Add the missing BQL field

- The BQL field must be a **public abstract class**
- Name it exactly like the property but with the **first letter in lowercase** (camelCase)
- Place it **immediately above** the property
- Inherit from the appropriate `PX.Data.BQL.Bql{Type}.Field<T>` class

**Format**:

```csharp
public abstract class {fieldName} : PX.Data.BQL.Bql{Type}.Field<{fieldName}> { }
```

**Step 3**: Choose the correct BQL field type

Match the BQL field base class to the C# property type:

| Property Type | BQL Field Base Class    | Example                                              |
| ------------- | ----------------------- | ---------------------------------------------------- |
| `string`      | `BqlString.Field<T>`    | `public abstract class refNbr : BqlString.Field<refNbr> { }`    |
| `int?`        | `BqlInt.Field<T>`       | `public abstract class customerID : BqlInt.Field<customerID> { }` |
| `bool?`       | `BqlBool.Field<T>`      | `public abstract class isActive : BqlBool.Field<isActive> { }`   |
| `DateTime?`   | `BqlDateTime.Field<T>`  | `public abstract class tranDate : BqlDateTime.Field<tranDate> { }` |
| `decimal?`    | `BqlDecimal.Field<T>`   | `public abstract class amount : BqlDecimal.Field<amount> { }`    |
| `Guid?`       | `BqlGuid.Field<T>`      | `public abstract class noteID : BqlGuid.Field<noteID> { }`       |
| `short?`      | `BqlShort.Field<T>`     | `public abstract class lineNbr : BqlShort.Field<lineNbr> { }`    |
| `long?`       | `BqlLong.Field<T>`      | `public abstract class recordID : BqlLong.Field<recordID> { }`   |
| `byte[]`      | `BqlByteArray.Field<T>` | `public abstract class data : BqlByteArray.Field<data> { }`      |

### Complete Fix Example

**Before (Error ❌)**:

```csharp
public class ATPTEFMSample : IBqlTable
{
    // ❌ PX1065 Error: Missing BQL field
    [PXDBString(15, IsKey = true, IsUnicode = true)]
    [PXUIField(DisplayName = "Reference Nbr")]
    public virtual string RefNbr { get; set; }
}
```

**After (Fixed ✅)**:

```csharp
public class ATPTEFMSample : IBqlTable
{
    #region RefNbr
    // ✅ BQL field added
    public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
    
    [PXDBString(15, IsKey = true, IsUnicode = true)]
    [PXUIField(DisplayName = "Reference Nbr")]
    public virtual string RefNbr { get; set; }
    #endregion
}
```

---

## Common Scenarios

### Scenario 1: Bound DAC Field (Database-Persisted Field)

**Purpose**: Fields that are stored in the database.

```csharp
#region CustomerID
public abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }

[PXDBInt]
[PXUIField(DisplayName = "Customer")]
[Customer]
public virtual int? CustomerID { get; set; }
#endregion
```

### Scenario 2: Unbound DAC Field (Non-Persisted Field)

**Purpose**: Calculated or temporary fields not stored in database.

```csharp
#region TotalAmount
public abstract class totalAmount : PX.Data.BQL.BqlDecimal.Field<totalAmount> { }

[PXDecimal(4)]
[PXUIField(DisplayName = "Total Amount")]
public virtual decimal? TotalAmount { get; set; }
#endregion
```

### Scenario 3: DAC Extension Field

**Purpose**: Adding custom fields to existing Acumatica DACs.

```csharp
public sealed class ATPTEFMAPInvoiceExtension : PXCacheExtension<APInvoice>
{
    #region UsrATPTEFMFundID
    public abstract class usrATPTEFMFundID : PX.Data.BQL.BqlInt.Field<usrATPTEFMFundID> { }
    
    [PXDBInt]
    [PXUIField(DisplayName = "Fund")]
    [PXSelector(typeof(Search<ATPTEFMFund.fundID>))]
    public int? UsrATPTEFMFundID { get; set; }
    #endregion
}
```

### Scenario 4: Identity Field (Auto-Increment)

```csharp
#region FundID
public abstract class fundID : PX.Data.BQL.BqlInt.Field<fundID> { }

[PXDBIdentity]
[PXUIField(DisplayName = "Fund ID", Visibility = PXUIVisibility.Invisible)]
public virtual int? FundID { get; set; }
#endregion
```

### Scenario 5: Key Field with Selector

```csharp
#region FundCD
public abstract class fundCD : PX.Data.BQL.BqlString.Field<fundCD> { }

[PXDBString(30, IsKey = true, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCCCCCCCCCCCCCCCCC")]
[PXDefault]
[PXUIField(DisplayName = "Fund Code", Visibility = PXUIVisibility.SelectorVisible)]
[PXSelector(typeof(Search<ATPTEFMFund.fundCD>))]
public virtual string FundCD { get; set; }
#endregion
```

### Scenario 6: Note/Attachment Field

```csharp
#region NoteID
public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }

[PXNote]
public virtual Guid? NoteID { get; set; }
#endregion
```

---

## Using Acuminator's Automated Code Fix

### Quick Fix (Recommended)

1. **Click on the PX1065 error** in the Error List
2. **Press `Ctrl + .`** (Quick Actions and Refactorings)
3. **Select "Add missing BQL field"**
4. Acuminator will automatically generate the correct BQL field

### Benefits of Automated Fix

✅ Correct naming (camelCase)  
✅ Correct inheritance (BqlType.Field<T>)  
✅ Placed in the right location  
✅ Saves time

---

## Project-Specific Guidelines (ATPTEFM)

### Standard Pattern for All DAC Fields

```csharp
#region FieldName
public abstract class fieldName : PX.Data.BQL.Bql{Type}.Field<fieldName> { }

[Attributes]
[PXUIField(DisplayName = ATPTEFMMessages.FieldDisplayName)]
public virtual {Type}? FieldName { get; set; }
#endregion
```

### Complete DAC Example

```csharp
using PX.Data;
using PX.Data.BQL;
using System;

namespace CashFundManagement.DAC
{
    [Serializable]
    [PXCacheName(ATPTEFMMessages.Fund)]
    public class ATPTEFMFund : ATPTEFMAudit, IBqlTable
    {
        #region FundID
        public abstract class fundID : BqlInt.Field<fundID> { }
        
        [PXDBIdentity]
        [PXUIField(DisplayName = ATPTEFMMessages.FundID, Visibility = PXUIVisibility.Invisible)]
        public virtual int? FundID { get; set; }
        #endregion
        
        #region FundCD
        public abstract class fundCD : BqlString.Field<fundCD> { }
        
        [PXDBString(30, IsKey = true, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCCCCCCCCCCCCCCCCC")]
        [PXDefault]
        [PXUIField(DisplayName = ATPTEFMMessages.FundCD, Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<fundCD>))]
        public virtual string FundCD { get; set; }
        #endregion
        
        #region Status
        public abstract class status : BqlString.Field<status> { }
        
        [PXDBString(1, IsFixed = true)]
        [PXDefault(ATPTEFMFundStatus.Active)]
        [PXUIField(DisplayName = ATPTEFMMessages.Status, Visibility = PXUIVisibility.SelectorVisible)]
        [ATPTEFMFundStatus.List]
        public virtual string Status { get; set; }
        #endregion
        
        #region Description
        public abstract class description : BqlString.Field<description> { }
        
        [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = ATPTEFMMessages.Description)]
        public virtual string Description { get; set; }
        #endregion
        
        #region CuryAmount
        public abstract class curyAmount : BqlDecimal.Field<curyAmount> { }
        
        [PXDBDecimal(4)]
        [PXUIField(DisplayName = ATPTEFMMessages.Amount)]
        public virtual decimal? CuryAmount { get; set; }
        #endregion
        
        #region TranDate
        public abstract class tranDate : BqlDateTime.Field<tranDate> { }
        
        [PXDBDate]
        [PXUIField(DisplayName = ATPTEFMMessages.Date)]
        public virtual DateTime? TranDate { get; set; }
        #endregion
        
        #region IsActive
        public abstract class isActive : BqlBool.Field<isActive> { }
        
        [PXDBBool]
        [PXDefault(true)]
        [PXUIField(DisplayName = ATPTEFMMessages.Active)]
        public virtual bool? IsActive { get; set; }
        #endregion
        
        #region NoteID
        public abstract class noteID : BqlGuid.Field<noteID> { }
        
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        #endregion
    }
}
```

---

## Common Mistakes to Avoid

### ❌ Mistake 1: Missing BQL Field Entirely

```csharp
// ❌ NO BQL field
[PXDBString(15)]
[PXUIField(DisplayName = "Reference Nbr")]
public virtual string RefNbr { get; set; }
```

### ❌ Mistake 2: Wrong Inheritance

```csharp
// ❌ Wrong: Using IBqlField instead of BqlString.Field<T>
public abstract class refNbr : IBqlField { }

[PXDBString(15)]
public virtual string RefNbr { get; set; }
```

### ❌ Mistake 3: Wrong Casing (All Lowercase)

```csharp
// ❌ Wrong: "refnbr" instead of "refNbr"
public abstract class refnbr : BqlString.Field<refnbr> { }

[PXDBString(15)]
public virtual string RefNbr { get; set; }
```

### ❌ Mistake 4: Wrong BQL Type

```csharp
// ❌ Wrong: Using BqlInt for string property
public abstract class refNbr : BqlInt.Field<refNbr> { }

[PXDBString(15)]
public virtual string RefNbr { get; set; }
```

### ❌ Mistake 5: BQL Field Placed After Property

```csharp
// ❌ Wrong: BQL field should be BEFORE property
[PXDBString(15)]
public virtual string RefNbr { get; set; }

public abstract class refNbr : BqlString.Field<refNbr> { }
```

### ✅ Correct Pattern

```csharp
#region RefNbr
// ✅ Correct: BQL field before property, correct type, correct casing
public abstract class refNbr : BqlString.Field<refNbr> { }

[PXDBString(15, IsKey = true, IsUnicode = true)]
[PXUIField(DisplayName = ATPTEFMMessages.RefNbr)]
public virtual string RefNbr { get; set; }
#endregion
```

---

## Quick Reference

### BQL Field Template

```csharp
public abstract class {fieldName} : PX.Data.BQL.Bql{Type}.Field<{fieldName}> { }
```

### Naming Convention

- **Property Name**: PascalCase (e.g., `RefNbr`, `CustomerID`, `TranDate`)
- **BQL Field Name**: camelCase (e.g., `refNbr`, `customerID`, `tranDate`)
- **Rule**: First letter lowercase, all other letters match exactly

### Type Mapping Quick Reference

```
string    → BqlString.Field<T>
int?      → BqlInt.Field<T>
bool?     → BqlBool.Field<T>
DateTime? → BqlDateTime.Field<T>
decimal?  → BqlDecimal.Field<T>
Guid?     → BqlGuid.Field<T>
short?    → BqlShort.Field<T>
long?     → BqlLong.Field<T>
byte[]    → BqlByteArray.Field<T>
```

---

## Certification Checklist

Before submitting for Acumatica certification:

- [ ] **Build the solution** and verify no PX1065 errors remain
- [ ] **All DAC field properties** have corresponding BQL fields
- [ ] **BQL fields use camelCase** naming
- [ ] **BQL fields are placed immediately before** their properties
- [ ] **BQL fields inherit from correct** `BqlType.Field<T>` class
- [ ] **All DACs and DAC extensions** follow this pattern
- [ ] **Run Acuminator analysis** (ISV Level 3)
- [ ] **No errors in Error List** related to PX1065

---

## Troubleshooting

### Issue: "I fixed the field but still see the error"

**Solution**:
1. Rebuild the solution (Ctrl+Shift+B)
2. Close and reopen the file
3. Check that the BQL field name matches exactly (camelCase)
4. Verify the BQL field is directly above the property

### Issue: "Acuminator suggests wrong BQL field type"

**Solution**:
- Manually verify the property type
- Use the table above to select the correct BQL type
- For nullable types (int?, bool?, etc.), still use Bql{Type}.Field<T>

### Issue: "Error on inherited DAC property"

**Solution**:
- If the property is in a base class, add the BQL field in the base class
- Cannot fix inherited properties in derived class
- Error displays on DAC name, not on property

---

## Find All PX1065 Errors

### Using Visual Studio Find (Ctrl+Shift+F)

Search for DAC properties without BQL fields:

```regex
^\s*\[PX.*\]\s*\n\s*public virtual
```

This pattern finds properties that might be missing BQL fields.

---

## Summary

**PX1065 Error**: Missing BQL field for DAC property  
**Fix**: Add `public abstract class {fieldName} : Bql{Type}.Field<{fieldName}> { }` above the property  
**Critical for**: Acumatica certification, runtime stability, BQL query functionality

Use Acuminator's code fix (`Ctrl + .`) for automated correction, or follow this guide for manual fixes.

---

## Related Guides

- [PX1066: BQL Field Name Typo Fix Guide](PX1066-BQL-Field-Name-Typo-Fix-Guide.md)
- [PR Review Guidelines](PR-Review-Guidelines.md)
- [Acumatica Development Guidelines](../copilot-instructions.md)

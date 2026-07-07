# Fixing PX1066: BQL Field Name Typo Warning

## Overview

This guide provides step-by-step instructions for fixing PX1066 Acuminator diagnostic to meet Acumatica certification requirements.

---

## What Is PX1066?

**Error Code**: PX1066  
**Severity**: Warning (ISV Level 3: Informational)  
**Message**: The name of the BQL field may contain a mistake.

**Why It Matters**: BQL field names must match the property name exactly (except for casing). Typos or incorrect naming can cause BQL queries to fail, create confusion in code, or lead to unexpected behavior.

---

## How to Identify PX1066 Warnings

### In Visual Studio

1. **Build the solution**
2. **Open Error List** (View → Error List)
3. **Filter by** "PX1066" or "Acuminator"
4. Warnings appear on BQL fields with names that don't match their corresponding property

### What Triggers the Warning

The warning appears when:

1. **A BQL field name is similar but not identical to a property name** (likely a typo)
2. **The casing doesn't match properly** between BQL field and property
3. **A BQL field exists without a corresponding property** with a matching name

**Example of PX1066 Warning**:

```csharp
public class ATPTEFMSample : IBqlTable
{
    // ❌ PX1066 Warning: "nteID" should be "noteID"
    public abstract class nteID : PX.Data.BQL.BqlGuid.Field<nteID> { }
    
    [PXNote]
    public virtual Guid? NoteID { get; set; }
}
```

---

## How to Fix PX1066

### Step-by-Step Fix Instructions

**Step 1**: Identify the BQL field with the PX1066 warning

- Locate the BQL field (public abstract class)
- Find the corresponding DAC field property

**Step 2**: Compare the names

- **BQL field** should use **camelCase** (first letter lowercase)
- **Property** should use **PascalCase** (first letter uppercase)
- **All other letters must match exactly**

**Step 3**: Rename the BQL field to match the property

- Change only the first letter to lowercase
- Keep all other letters exactly the same as the property name
- Update all three places:
  1. Class name
  2. Generic type parameter
  3. Any references in BQL queries (if applicable)

### Complete Fix Example

**Before (Warning ❌)**:

```csharp
public class ATPTEFMSample : IBqlTable
{
    #region NoteID
    // ❌ PX1066 Warning: "nteID" doesn't match "NoteID"
    public abstract class nteID : PX.Data.BQL.BqlGuid.Field<nteID> { }
    
    [PXNote]
    public virtual Guid? NoteID { get; set; }
    #endregion
}
```

**After (Fixed ✅)**:

```csharp
public class ATPTEFMSample : IBqlTable
{
    #region NoteID
    // ✅ Fixed: "noteID" correctly matches "NoteID"
    public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
    
    [PXNote]
    public virtual Guid? NoteID { get; set; }
    #endregion
}
```

---

## Common Typo Scenarios

### Scenario 1: Missing Letter(s)

**Problem**: BQL field name has missing characters

```csharp
// ❌ Wrong: "nteID" missing the 'o'
public abstract class nteID : BqlGuid.Field<nteID> { }
[PXNote]
public virtual Guid? NoteID { get; set; }

// ✅ Correct
public abstract class noteID : BqlGuid.Field<noteID> { }
[PXNote]
public virtual Guid? NoteID { get; set; }
```

### Scenario 2: Wrong Casing (All Lowercase)

**Problem**: BQL field uses all lowercase instead of camelCase

```csharp
// ❌ Wrong: "customerid" all lowercase
public abstract class customerid : BqlInt.Field<customerid> { }
[Customer]
public virtual int? CustomerID { get; set; }

// ✅ Correct: Preserve "ID" casing
public abstract class customerID : BqlInt.Field<customerID> { }
[Customer]
public virtual int? CustomerID { get; set; }
```

### Scenario 3: Abbreviation Mismatch

**Problem**: BQL field uses different abbreviation than property

```csharp
// ❌ Wrong: "refNr" instead of "refNbr"
public abstract class refNr : BqlString.Field<refNr> { }
[PXDBString(15)]
public virtual string RefNbr { get; set; }

// ✅ Correct
public abstract class refNbr : BqlString.Field<refNbr> { }
[PXDBString(15)]
public virtual string RefNbr { get; set; }
```

### Scenario 4: Extra/Different Letters

**Problem**: BQL field has extra or different letters

```csharp
// ❌ Wrong: "transDate" instead of "tranDate"
public abstract class transDate : BqlDateTime.Field<transDate> { }
[PXDBDate]
public virtual DateTime? TranDate { get; set; }

// ✅ Correct
public abstract class tranDate : BqlDateTime.Field<tranDate> { }
[PXDBDate]
public virtual DateTime? TranDate { get; set; }
```

### Scenario 5: Completely Different Name

**Problem**: BQL field has a different name than property

```csharp
// ❌ Wrong: "note" instead of "noteID"
public abstract class note : BqlGuid.Field<note> { }
[PXNote]
public virtual Guid? NoteID { get; set; }

// ✅ Correct
public abstract class noteID : BqlGuid.Field<noteID> { }
[PXNote]
public virtual Guid? NoteID { get; set; }
```

---

## Name Matching Rules

### The Golden Rule

**Property Name** → **BQL Field Name**
- Take the property name (PascalCase)
- Change ONLY the first letter to lowercase
- Keep everything else EXACTLY the same

### Examples of Correct Matching

| Property Name (PascalCase) | BQL Field Name (camelCase) | Notes |
|---------------------------|---------------------------|--------|
| `RefNbr` | `refNbr` | Only first letter changes |
| `CustomerID` | `customerID` | Keep "ID" in uppercase |
| `TranDate` | `tranDate` | Not "trandate" |
| `CuryAmount` | `curyAmount` | Not "curyamount" |
| `NoteID` | `noteID` | Not "noteid" or "noteId" |
| `IsActive` | `isActive` | Not "isactive" |
| `BranchID` | `branchID` | Keep "ID" uppercase |
| `UsrATPTEFMFundCD` | `usrATPTEFMFundCD` | Keep all caps after first |

### Common Incorrect Patterns

| Property Name | ❌ Wrong BQL Name | ✅ Correct BQL Name | Issue |
|--------------|------------------|-------------------|--------|
| `RefNbr` | `refnbr` | `refNbr` | All lowercase |
| `RefNbr` | `refNr` | `refNbr` | Missing letter |
| `CustomerID` | `customerId` | `customerID` | Wrong ID casing |
| `CustomerID` | `custID` | `customerID` | Abbreviated |
| `TranDate` | `trandate` | `tranDate` | All lowercase |
| `TranDate` | `transDate` | `tranDate` | Extra letter |
| `CuryAmount` | `currAmount` | `curyAmount` | Different word |
| `CuryAmount` | `curyAmt` | `curyAmount` | Abbreviated |
| `NoteID` | `noteid` | `noteID` | All lowercase |
| `NoteID` | `nteID` | `noteID` | Missing letter |

---

## Using Acuminator's Automated Code Fix

### Quick Fix (Recommended)

1. **Click on the PX1066 warning** in the Error List (or on the BQL field in code)
2. **Press `Ctrl + .`** (Quick Actions and Refactorings)
3. **Select "Rename BQL field to match property"**
4. Acuminator will automatically rename the BQL field correctly

### Benefits of Automated Fix

✅ Correct casing applied automatically  
✅ All references updated  
✅ Type parameter renamed  
✅ Saves time and prevents mistakes

---

## Manual Renaming Steps

If you need to rename manually:

### Step 1: Identify the Correct Name

Look at the property name and apply camelCase:
```csharp
Property: CustomerID  →  BQL Field: customerID
Property: RefNbr     →  BQL Field: refNbr
Property: TranDate   →  BQL Field: tranDate
```

### Step 2: Update the BQL Field

Change three places:

```csharp
// Before
public abstract class nteID : BqlGuid.Field<nteID> { }
//                     ^^^^^                  ^^^^^
//                  (1) class name      (2) type parameter

// After
public abstract class noteID : BqlGuid.Field<noteID> { }
//                     ^^^^^^                  ^^^^^^
```

### Step 3: Rebuild and Verify

1. Save the file
2. Rebuild the solution (Ctrl+Shift+B)
3. Check Error List to confirm warning is gone

---

## Special Cases

### Case 1: DAC Extension Fields

DAC extension fields typically start with `Usr`:

```csharp
// ✅ Correct pattern for extension fields
public sealed class ATPTEFMAPInvoiceExtension : PXCacheExtension<APInvoice>
{
    #region UsrATPTEFMFundCD
    public abstract class usrATPTEFMFundCD : BqlString.Field<usrATPTEFMFundCD> { }
    //                    ^^^^^^^^^^^^^^^^ - lowercase 'u', keep rest as is
    
    [PXDBString(30)]
    [PXUIField(DisplayName = "Fund Code")]
    public string UsrATPTEFMFundCD { get; set; }
    //            ^^^^^^^^^^^^^^^^ - PascalCase
    #endregion
}
```

### Case 2: Fields with Acronyms

Keep acronyms in the same case after the first letter:

```csharp
// ✅ Correct: Keep "ID" uppercase
public abstract class customerID : BqlInt.Field<customerID> { }
public virtual int? CustomerID { get; set; }

// ✅ Correct: Keep "CD" uppercase
public abstract class fundCD : BqlString.Field<fundCD> { }
public virtual string FundCD { get; set; }

// ✅ Correct: Keep "API" uppercase
public abstract class apiKey : BqlString.Field<apiKey> { }
public virtual string APIKey { get; set; }
```

### Case 3: Inherited Properties

If the property is inherited from a base DAC:

```csharp
// Base DAC
public class ATPTEFMBase : IBqlTable
{
    #region RefNbr
    public abstract class refNbr : BqlString.Field<refNbr> { }
    
    [PXDBString(15)]
    public virtual string RefNbr { get; set; }
    #endregion
}

// Derived DAC - Don't redefine the BQL field
public class ATPTEFMDerived : ATPTEFMBase
{
    // ✅ No need to redefine refNbr BQL field
    // It's inherited from ATPTEFMBase
}
```

---

## Project-Specific Examples (ATPTEFM)

### Standard Fields

```csharp
#region FundID
public abstract class fundID : BqlInt.Field<fundID> { }
[PXDBIdentity]
public virtual int? FundID { get; set; }
#endregion

#region FundCD
public abstract class fundCD : BqlString.Field<fundCD> { }
[PXDBString(30, IsKey = true)]
public virtual string FundCD { get; set; }
#endregion

#region RefNbr
public abstract class refNbr : BqlString.Field<refNbr> { }
[PXDBString(15, IsKey = true)]
public virtual string RefNbr { get; set; }
#endregion

#region CuryAmount
public abstract class curyAmount : BqlDecimal.Field<curyAmount> { }
[PXDBDecimal(4)]
public virtual decimal? CuryAmount { get; set; }
#endregion

#region TranDate
public abstract class tranDate : BqlDateTime.Field<tranDate> { }
[PXDBDate]
public virtual DateTime? TranDate { get; set; }
#endregion
```

---

## Common Mistakes to Avoid

### ❌ Mistake 1: All Lowercase

```csharp
// ❌ Wrong
public abstract class customerid : BqlInt.Field<customerid> { }
public virtual int? CustomerID { get; set; }

// ✅ Correct
public abstract class customerID : BqlInt.Field<customerID> { }
public virtual int? CustomerID { get; set; }
```

### ❌ Mistake 2: Typo in BQL Field Name

```csharp
// ❌ Wrong: "nteID" is a typo
public abstract class nteID : BqlGuid.Field<nteID> { }
public virtual Guid? NoteID { get; set; }

// ✅ Correct
public abstract class noteID : BqlGuid.Field<noteID> { }
public virtual Guid? NoteID { get; set; }
```

### ❌ Mistake 3: Using Abbreviation Different from Property

```csharp
// ❌ Wrong: "refNr" vs "refNbr"
public abstract class refNr : BqlString.Field<refNr> { }
public virtual string RefNbr { get; set; }

// ✅ Correct
public abstract class refNbr : BqlString.Field<refNbr> { }
public virtual string RefNbr { get; set; }
```

### ❌ Mistake 4: Completely Different Name

```csharp
// ❌ Wrong: "amount" vs "curyAmount"
public abstract class amount : BqlDecimal.Field<amount> { }
public virtual decimal? CuryAmount { get; set; }

// ✅ Correct
public abstract class curyAmount : BqlDecimal.Field<curyAmount> { }
public virtual decimal? CuryAmount { get; set; }
```

---

## Certification Checklist

Before submitting for Acumatica certification:

- [ ] **All PX1066 warnings resolved**
- [ ] **BQL field names match properties** exactly (except first letter)
- [ ] **BQL fields use camelCase**
- [ ] **Properties use PascalCase**
- [ ] **No typos in BQL field names**
- [ ] **Acronyms (ID, CD, etc.) maintain correct casing**
- [ ] **Rebuild solution** and verify no warnings
- [ ] **Run Acuminator analysis** (ISV Level 3)

---

## Quick Reference

### Naming Convention Rule

```
Property Name (PascalCase)  →  BQL Field Name (camelCase)
     RefNbr                 →       refNbr
   CustomerID               →     customerID
    TranDate                →      tranDate
  CuryAmount                →    curyAmount
    NoteID                  →      noteID
```

### Template

```csharp
#region PropertyName
public abstract class propertyName : Bql{Type}.Field<propertyName> { }
//                    ^^^^^^^^^^^^ camelCase      ^^^^^^^^^^^^ camelCase

[Attributes]
public virtual Type? PropertyName { get; set; }
//                   ^^^^^^^^^^^^ PascalCase
#endregion
```

---

## Troubleshooting

### Issue: "I renamed the field but still see the warning"

**Solution**:
1. Rebuild the solution (Ctrl+Shift+B)
2. Close and reopen the file
3. Verify you changed BOTH the class name AND the generic type parameter
4. Check for exact match (including casing of acronyms like ID, CD)

### Issue: "Should I use 'id' or 'ID' in camelCase?"

**Solution**:
- For property `CustomerID`, use `customerID` (keep ID uppercase after first letter)
- For property `FundCD`, use `fundCD` (keep CD uppercase)
- **Rule**: Only change the first letter to lowercase, keep everything else as-is

### Issue: "Warning on inherited property"

**Solution**:
- Check the base DAC for the BQL field definition
- Fix the BQL field name in the base DAC, not the derived DAC
- Derived DACs inherit BQL fields from base DACs

---

## Find All PX1066 Warnings

### Using Visual Studio

1. **Build** → **Build Solution** (Ctrl+Shift+B)
2. **View** → **Error List**
3. **Filter**: Set to "Warnings" and search for "PX1066"
4. Click each warning to jump to the code location

### Using Acuminator

1. Right-click on project → **Acuminator** → **Code Analysis**
2. Filter by "PX1066"
3. Use batch code fix if available

---

## Summary

**PX1066 Warning**: BQL field name doesn't match property name  
**Fix**: Rename BQL field to match property (camelCase vs PascalCase)  
**Rule**: Property name with first letter lowercase = BQL field name  
**Critical for**: Code consistency, Acumatica certification, query reliability

Use Acuminator's code fix (`Ctrl + .`) for automated renaming, or follow the naming rules in this guide for manual fixes.

---

## Related Guides

- [PX1065: Missing BQL Field Fix Guide](PX1065-Missing-BQL-Field-Fix-Guide.md)
- [PR Review Guidelines](PR-Review-Guidelines.md)
- [Acumatica Development Guidelines](../copilot-instructions.md)

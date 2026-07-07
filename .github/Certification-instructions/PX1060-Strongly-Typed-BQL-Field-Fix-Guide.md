# Fixing PX1060: DAC Fields Should Be Strongly Typed

## Overview

This guide provides step-by-step instructions for fixing PX1060 Acuminator diagnostic to meet Acumatica certification requirements.

---

## What Is PX1060?

**Error Code**: PX1060  
**Severity**: Warning  
**Message**: DAC fields should be strongly typed to be used in fluent BQL queries.

**Why It Matters**: Weakly-typed DAC BQL fields (using `IBqlField`) do not support Fluent BQL (FBQL) queries. Strongly-typed BQL fields enable compile-time code checks, IntelliSense support, and are required for modern Acumatica development (2021R1+).

---

## How to Identify PX1060 Warnings

### In Visual Studio

1. **Build the solution**
2. **Open Error List** (View → Error List)
3. **Filter by** "PX1060" or "Acuminator"
4. Warnings appear on BQL fields that inherit from `IBqlField` instead of strongly-typed base classes

### What Triggers the Warning

The warning appears when a BQL field (public abstract class) inherits from `PX.Data.IBqlField` interface instead of the strongly-typed fluent BQL base classes like `BqlInt.Field<T>`, `BqlString.Field<T>`, etc.

**Example of PX1060 Warning**:

```csharp
public class ATPTEFMSample : IBqlTable
{
    #region ProductID
    // ❌ PX1060 Warning: Weakly-typed BQL field using IBqlField
    public abstract class productID : PX.Data.IBqlField { }
    
    [PXDBIdentity]
    public virtual int? ProductID { get; set; }
    #endregion
}
```

---

## How to Fix PX1060

### Step-by-Step Fix Instructions

**Step 1**: Identify the BQL field with PX1060 warning

- Look for BQL fields (public abstract classes) inheriting from `IBqlField`
- Note the corresponding property type

**Step 2**: Determine the correct strongly-typed base class

- Match the property type to the appropriate `BqlType.Field<T>` class
- Use the type mapping table below

**Step 3**: Replace `IBqlField` with the strongly-typed base class

- Change from: `public abstract class fieldName : IBqlField { }`
- Change to: `public abstract class fieldName : BqlType.Field<fieldName> { }`

### Type Mapping Reference

| Property Type | Old (Weakly-Typed) | New (Strongly-Typed) |
|--------------|-------------------|---------------------|
| `int?` | `IBqlField` | `PX.Data.BQL.BqlInt.Field<T>` |
| `string` | `IBqlField` | `PX.Data.BQL.BqlString.Field<T>` |
| `bool?` | `IBqlField` | `PX.Data.BQL.BqlBool.Field<T>` |
| `DateTime?` | `IBqlField` | `PX.Data.BQL.BqlDateTime.Field<T>` |
| `decimal?` | `IBqlField` | `PX.Data.BQL.BqlDecimal.Field<T>` |
| `Guid?` | `IBqlField` | `PX.Data.BQL.BqlGuid.Field<T>` |
| `short?` | `IBqlField` | `PX.Data.BQL.BqlShort.Field<T>` |
| `long?` | `IBqlField` | `PX.Data.BQL.BqlLong.Field<T>` |
| `byte?` | `IBqlField` | `PX.Data.BQL.BqlByte.Field<T>` |
| `float?` | `IBqlField` | `PX.Data.BQL.BqlFloat.Field<T>` |
| `double?` | `IBqlField` | `PX.Data.BQL.BqlDouble.Field<T>` |
| `byte[]` | `IBqlField` | `PX.Data.BQL.BqlByteArray.Field<T>` |

### Complete Fix Example

**Before (Warning ❌)**:

```csharp
public class ATPTEFMProduct : IBqlTable
{
    #region ProductID
    // ❌ PX1060 Warning: Using IBqlField
    public abstract class productID : PX.Data.IBqlField { }
    
    [PXDBIdentity]
    public virtual int? ProductID { get; set; }
    #endregion
    
    #region ProductCD
    // ❌ PX1060 Warning: Using IBqlField
    public abstract class productCD : PX.Data.IBqlField { }
    
    [PXDBString(30, IsKey = true, IsUnicode = true)]
    public virtual string ProductCD { get; set; }
    #endregion
}
```

**After (Fixed ✅)**:

```csharp
public class ATPTEFMProduct : IBqlTable
{
    #region ProductID
    // ✅ Fixed: Using strongly-typed BqlInt.Field<T>
    public abstract class productID : PX.Data.BQL.BqlInt.Field<productID> { }
    
    [PXDBIdentity]
    public virtual int? ProductID { get; set; }
    #endregion
    
    #region ProductCD
    // ✅ Fixed: Using strongly-typed BqlString.Field<T>
    public abstract class productCD : PX.Data.BQL.BqlString.Field<productCD> { }
    
    [PXDBString(30, IsKey = true, IsUnicode = true)]
    public virtual string ProductCD { get; set; }
    #endregion
}
```

---

## Common Scenarios

### Scenario 1: Integer Identity Field

```csharp
#region FundID
// ❌ Before
public abstract class fundID : IBqlField { }

// ✅ After
public abstract class fundID : PX.Data.BQL.BqlInt.Field<fundID> { }

[PXDBIdentity]
[PXUIField(DisplayName = "Fund ID", Visibility = PXUIVisibility.Invisible)]
public virtual int? FundID { get; set; }
#endregion
```

### Scenario 2: String Key Field

```csharp
#region RefNbr
// ❌ Before
public abstract class refNbr : IBqlField { }

// ✅ After
public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }

[PXDBString(15, IsKey = true, IsUnicode = true)]
[PXUIField(DisplayName = "Reference Nbr")]
public virtual string RefNbr { get; set; }
#endregion
```

### Scenario 3: Boolean Field

```csharp
#region IsActive
// ❌ Before
public abstract class isActive : IBqlField { }

// ✅ After
public abstract class isActive : PX.Data.BQL.BqlBool.Field<isActive> { }

[PXDBBool]
[PXDefault(true)]
[PXUIField(DisplayName = "Active")]
public virtual bool? IsActive { get; set; }
#endregion
```

### Scenario 4: Decimal Currency Field

```csharp
#region CuryAmount
// ❌ Before
public abstract class curyAmount : IBqlField { }

// ✅ After
public abstract class curyAmount : PX.Data.BQL.BqlDecimal.Field<curyAmount> { }

[PXDBDecimal(4)]
[PXUIField(DisplayName = "Amount")]
public virtual decimal? CuryAmount { get; set; }
#endregion
```

### Scenario 5: DateTime Field

```csharp
#region TranDate
// ❌ Before
public abstract class tranDate : IBqlField { }

// ✅ After
public abstract class tranDate : PX.Data.BQL.BqlDateTime.Field<tranDate> { }

[PXDBDate]
[PXUIField(DisplayName = "Transaction Date")]
public virtual DateTime? TranDate { get; set; }
#endregion
```

### Scenario 6: Guid Note Field

```csharp
#region NoteID
// ❌ Before
public abstract class noteID : IBqlField { }

// ✅ After
public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }

[PXNote]
public virtual Guid? NoteID { get; set; }
#endregion
```

---

## Using Acuminator's Automated Code Fix

### Quick Fix (Recommended)

1. **Click on the PX1060 warning** in the Error List (or on the BQL field in code)
2. **Press `Ctrl + .`** (Quick Actions and Refactorings)
3. **Select "Use strongly-typed BQL field"** or similar option
4. Acuminator will automatically change `IBqlField` to the correct `BqlType.Field<T>`

### Benefits of Automated Fix

✅ Correct type automatically detected  
✅ Proper generic parameter applied  
✅ Saves time and prevents errors  
✅ Consistent with Acumatica standards

---

## Shortened Syntax (Optional)

You can use shortened syntax by adding `using PX.Data.BQL;` to your using statements:

**Full Syntax**:
```csharp
public abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }
```

**Shortened Syntax** (with `using PX.Data.BQL;`):
```csharp
public abstract class customerID : BqlInt.Field<customerID> { }
```

**Recommended Practice**: Always include `using PX.Data.BQL;` at the top of DAC files and use shortened syntax for cleaner code.

---

## Project-Specific Guidelines (ATPTEFM)

### Standard Pattern for All DAC Fields

```csharp
using PX.Data;
using PX.Data.BQL;
using System;

namespace CashFundManagement.DAC
{
    [Serializable]
    [PXCacheName(ATPTEFMMessages.EntityName)]
    public class ATPTEFMEntity : ATPTEFMAudit, IBqlTable
    {
        #region FieldName
        public abstract class fieldName : Bql{Type}.Field<fieldName> { }
        
        [Attributes]
        [PXUIField(DisplayName = ATPTEFMMessages.FieldDisplayName)]
        public virtual {Type}? FieldName { get; set; }
        #endregion
    }
}
```

### Complete DAC Example

```csharp
using PX.Data;
using PX.Data.BQL;
using PX.Objects.GL;
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
        
        [PXDBString(30, IsKey = true, IsUnicode = true)]
        [PXDefault]
        [PXUIField(DisplayName = ATPTEFMMessages.FundCD, Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<fundCD>))]
        public virtual string FundCD { get; set; }
        #endregion
        
        #region Status
        public abstract class status : BqlString.Field<status> { }
        
        [PXDBString(1, IsFixed = true)]
        [PXDefault(ATPTEFMFundStatus.Active)]
        [PXUIField(DisplayName = ATPTEFMMessages.Status)]
        [ATPTEFMFundStatus.List]
        public virtual string Status { get; set; }
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

### ❌ Mistake 1: Still Using IBqlField

```csharp
// ❌ Wrong: Old weakly-typed interface
public abstract class productID : IBqlField { }

[PXDBInt]
public virtual int? ProductID { get; set; }
```

### ❌ Mistake 2: Wrong BQL Type for Property Type

```csharp
// ❌ Wrong: Using BqlString for int property
public abstract class productID : BqlString.Field<productID> { }

[PXDBInt]
public virtual int? ProductID { get; set; }
```

### ❌ Mistake 3: Missing Generic Type Parameter

```csharp
// ❌ Wrong: Missing <productID> generic parameter
public abstract class productID : BqlInt.Field { }

[PXDBInt]
public virtual int? ProductID { get; set; }
```

### ❌ Mistake 4: Incorrect Generic Type Parameter

```csharp
// ❌ Wrong: Using class name instead of field name
public abstract class productID : BqlInt.Field<ATPTEFMProduct> { }

[PXDBInt]
public virtual int? ProductID { get; set; }
```

### ✅ Correct Pattern

```csharp
#region ProductID
// ✅ Correct: Strongly-typed with correct type and generic parameter
public abstract class productID : BqlInt.Field<productID> { }

[PXDBInt]
[PXUIField(DisplayName = "Product ID")]
public virtual int? ProductID { get; set; }
#endregion
```

---

## Why Strongly-Typed BQL Fields Matter

### Benefits

1. **Compile-Time Checking**: Errors caught during compilation, not at runtime
2. **IntelliSense Support**: Better code completion in Visual Studio
3. **Fluent BQL Support**: Required for modern Fluent BQL (FBQL) queries
4. **Type Safety**: Prevents type mismatches in BQL queries
5. **Better Refactoring**: Easier to rename and track field usage
6. **Acumatica Standards**: Aligns with Acumatica best practices (2021R1+)

### Example: Fluent BQL Query Benefits

**With Weakly-Typed (IBqlField)**:
```csharp
// ❌ No compile-time checking, runtime errors possible
SelectFrom<ATPTEFMFund>
    .Where<ATPTEFMFund.status.IsEqual<@P.AsString>>
    .View.Select(this, "A");
// If 'status' field type changes, no compiler warning!
```

**With Strongly-Typed (BqlString.Field<T>)**:
```csharp
// ✅ Compile-time type checking, IntelliSense support
SelectFrom<ATPTEFMFund>
    .Where<ATPTEFMFund.status.IsEqual<@P.AsString>>
    .View.Select(this, "A");
// If 'status' field type changes, compiler error immediately!
```

---

## Migration Strategy

### For Large Projects

If you have many BQL fields to convert:

1. **Use Acuminator Analysis**: Run full project analysis to find all PX1060 warnings
2. **Prioritize Core DACs**: Fix main business logic DACs first
3. **Batch Fix**: Use Acuminator's batch code fix feature if available
4. **Test Incrementally**: Test after each DAC file is fixed
5. **Update in Phases**: Don't try to fix everything at once

### Search Pattern

Use Visual Studio Find (Ctrl+Shift+F) to locate all weakly-typed BQL fields:

```regex
: IBqlField
```

or more specifically:

```regex
public abstract class \w+ : (?:PX\.Data\.)?IBqlField
```

---

## Certification Checklist

Before submitting for Acumatica certification:

- [ ] **All PX1060 warnings resolved** (no IBqlField usage)
- [ ] **All BQL fields use strongly-typed base classes**
- [ ] **Correct BQL type matches property type**
- [ ] **Generic type parameter matches field name**
- [ ] **Using directive includes** `using PX.Data.BQL;`
- [ ] **Project builds without warnings**
- [ ] **Run Acuminator analysis** (ISV Level 3)
- [ ] **Fluent BQL queries compile successfully**

---

## Quick Reference

### Conversion Template

```csharp
// Before
public abstract class {fieldName} : IBqlField { }

// After
public abstract class {fieldName} : Bql{Type}.Field<{fieldName}> { }
```

### Type Quick Reference

```
int?      → BqlInt.Field<T>
string    → BqlString.Field<T>
bool?     → BqlBool.Field<T>
DateTime? → BqlDateTime.Field<T>
decimal?  → BqlDecimal.Field<T>
Guid?     → BqlGuid.Field<T>
short?    → BqlShort.Field<T>
long?     → BqlLong.Field<T>
byte?     → BqlByte.Field<T>
float?    → BqlFloat.Field<T>
double?   → BqlDouble.Field<T>
byte[]    → BqlByteArray.Field<T>
```

---

## Troubleshooting

### Issue: "Can't find BqlInt.Field<T> class"

**Solution**:
1. Add `using PX.Data.BQL;` to the top of your file
2. Or use fully qualified name: `PX.Data.BQL.BqlInt.Field<T>`
3. Ensure you're using Acumatica 2021R1 or later

### Issue: "Generic type constraint error"

**Solution**:
- Verify the generic parameter matches the BQL field name (camelCase)
- Example: `BqlInt.Field<customerID>` not `BqlInt.Field<CustomerID>`

### Issue: "Which type to use for custom types?"

**Solution**:
- For custom types, determine the underlying database type
- Match the BQL type to the database column type
- Example: Custom status enum stored as string → `BqlString.Field<T>`

---

## Related Diagnostics

- **PX1065**: Missing BQL Field - [Fix Guide](PX1065-Missing-BQL-Field-Fix-Guide.md)
- **PX1066**: BQL Field Name Typo - [Fix Guide](PX1066-BQL-Field-Name-Typo-Fix-Guide.md)

---

## Summary

**PX1060 Warning**: BQL field uses weakly-typed `IBqlField` interface  
**Fix**: Change to strongly-typed `BqlType.Field<fieldName>` base class  
**Rule**: Match BQL type to property type, use field name as generic parameter  
**Critical for**: Fluent BQL support, compile-time checking, Acumatica certification

Use Acuminator's code fix (`Ctrl + .`) for automated conversion, or follow the type mapping table in this guide for manual fixes.

---

## Additional Resources

- [Acumatica Fluent BQL Documentation](https://help.acumatica.com/)
- [PX1065: Missing BQL Field Fix Guide](PX1065-Missing-BQL-Field-Fix-Guide.md)
- [PX1066: BQL Field Name Typo Fix Guide](PX1066-BQL-Field-Name-Typo-Fix-Guide.md)
- [PR Review Guidelines](PR-Review-Guidelines.md)
- [Acumatica Development Guidelines](../copilot-instructions.md)

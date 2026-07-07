# PX1097: PXOverride Methods Must Be Public and Non-Virtual

## Overview

**Diagnostic Code:** PX1097  
**Severity:** Error  
**Category:** Acuminator Certification Requirement

## Description

Methods decorated with the `[PXOverride]` attribute must be declared as `public` and must **not** have `virtual`, `abstract`, or `override` modifiers.

This is because the Acumatica Framework's `PXOverride` mechanism is separate from C#'s virtual/override inheritance pattern and currently only supports public, non-virtual methods.

## Common Violations

### ❌ Incorrect: Using `virtual` modifier

```csharp
[PXOverride]
public virtual void SomeMethod(SomeMethodDelegate baseMethod)
{
    // Implementation
}
```

### ❌ Incorrect: Using non-public accessibility

```csharp
[PXOverride]
protected void SomeMethod(SomeMethodDelegate baseMethod)
{
    // Implementation
}
```

### ❌ Incorrect: Using `override` modifier

```csharp
[PXOverride]
public override void SomeMethod(SomeMethodDelegate baseMethod)
{
    // Implementation
}
```

## How to Fix

### ✅ Correct: Public and non-virtual

```csharp
[PXOverride]
public void SomeMethod(SomeMethodDelegate baseMethod)
{
    // Implementation
}
```

## Fix Pattern

| Before | After |
|--------|-------|
| `public virtual` | `public` |
| `protected virtual` | `public` |
| `internal virtual` | `public` |
| `protected` | `public` |
| `public override` | `public` (requires refactoring - see note below) |

## Quick Fix Steps

1. **Locate the error** - Find the method flagged with PX1097
2. **Remove `virtual` modifier** - Change `public virtual` to `public`
3. **Change accessibility** - If not `public`, change to `public`
4. **Rebuild** - Verify the error is resolved

## Search Pattern for Finding Violations

Use this regex pattern to find potential violations in your codebase:

```regex
\[PXOverride\]\s*\n\s*public\s+virtual
```

Or search for:
```regex
\[PXOverride\]\s*\n\s*(protected|internal|private)
```

## Important Notes

### Methods with `override` modifier
If the method has the `override` modifier, simply removing it may break compatibility with the base method. In this case, you need to refactor:
- Remove the `override` modifier
- Ensure the method signature matches the expected `PXOverride` pattern
- The code fix from Acuminator cannot automatically fix `override` methods

### Required Method Structure for PXOverride

A properly structured `PXOverride` method must:

1. Be declared in a **graph extension**
2. Be `public` and non-virtual
3. **Not** be `static`
4. **Not** be a generic method
5. Include a **delegate parameter** as the last parameter to call the base method
6. Have a matching method name with the base method
7. Include XML documentation referencing the base method (recommended)

### Example of Proper Structure

```csharp
public class MyGraphExtension : PXGraphExtension<MyGraph>
{
    // 1. Define the delegate matching the base method signature
    public delegate void ProcessDocumentDelegate(Document doc);
    
    /// Overrides <seealso cref="MyGraph.ProcessDocument(Document)"/>
    [PXOverride]
    public void ProcessDocument(Document doc, ProcessDocumentDelegate baseMethod)
    {
        // Custom logic before
        ValidateDocument(doc);
        
        // Call base method
        baseMethod(doc);
        
        // Custom logic after
        LogProcessing(doc);
    }
}
```

## Automated Fix Script

For bulk fixes, you can use the following PowerShell script to identify files that need fixing:

```powershell
# Find all files with potential PX1097 violations
Get-ChildItem -Path ".\**\*.cs" -Recurse | 
    Select-String -Pattern '\[PXOverride\]' -Context 0,1 | 
    Where-Object { $_.Context.PostContext -match 'virtual|protected|internal|private' } |
    Select-Object Path, LineNumber, Line
```

## Related Diagnostics

- **PX1096** - A method with the PXOverride attribute must have a delegate parameter
- **PX1098** - The signature of the overriding method does not match the signature of the overridden method

## References

- [Acuminator PX1097 Documentation](https://github.com/Acumatica/Acuminator/blob/master/docs/diagnostics/PX1097.md)
- [To Override a Virtual Method](https://help.acumatica.com/Help?ScreenId=ShowWiki&pageid=6fa2a444-17b4-42f9-9e6a-64e85167626a)
- [Override of a Method](https://help.acumatica.com/Help?ScreenId=ShowWiki&pageid=635c830e-4617-4d5c-9fa5-035952311aa9)
- [PXOverrideAttribute](https://help.acumatica.com/Help?ScreenId=ShowWiki&pageid=cdc4f1df-a4cc-5de5-a379-5078ad449965)

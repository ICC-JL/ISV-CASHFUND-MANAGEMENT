# Pull Request Review Guidelines

**CRITICAL**: These guidelines are ONLY to be applied during Pull Request (PR) review. Do NOT apply these during regular development or coding assistance.

---

## When to Use This Document

✅ **Use During**:
- Pull Request reviews
- Code review sessions
- Pre-merge validation
- Quality assurance checks

❌ **Do NOT Use During**:
- Regular development
- Feature implementation
- Bug fixing
- Code generation requests
- General coding assistance

---

## Pull Request Review Checklist

### 1. Code Quality Standards

#### Naming Conventions
- [ ] All classes start with `ATPTEFM` prefix
- [ ] DACs follow pattern: `ATPTEFM<EntityName>`
- [ ] Graphs follow pattern: `ATPTEFM<EntityName>Entry` or `ATPTEFM<EntityName>Maint`
- [ ] Extensions follow pattern: `ATPTEFM<BaseClass>Extension`
- [ ] Field names use PascalCase (e.g., `RefNbr`, `TranDate`, `CuryAmount`)
- [ ] No hardcoded strings - all UI text in `ATPTEFMMessages` class

#### DAC Standards
- [ ] All bound DACs inherit from `ATPTEFMAudit` base class
- [ ] Primary Key (PK) class is defined
- [ ] Foreign Key (FK) class is defined for all relationships
- [ ] FK class naming: `ProjectPrefix<FKName>` (e.g., `ATPTCustomer`, `ATPTBranch`)
- [ ] All DAC fields have proper attributes
- [ ] NoteID field included for screens requiring attachments/notes
- [ ] Using statements included (not fully qualified type names)

#### BQL Standards
- [ ] All queries use Fluent BQL syntax (no legacy BQL)
- [ ] FK classes used with `.SameAsCurrent` or `.FromCurrent` instead of manual field relationships
- [ ] Master-detail views use FK classes: `Where<Detail.FK.Master.SameAsCurrent>`
- [ ] No manual field comparisons where FK classes exist
- [ ] Proper use of `.View` suffix on view declarations

**Examples to Check For**:

❌ **INCORRECT** (Manual field relationship):
```csharp
public SelectFrom<ATPTEFMEntityDetail>
    .Where<ATPTEFMEntityDetail.refNbr.IsEqual<Current<ATPTEFMEntity.refNbr>>>
    .View Details;
```

✅ **CORRECT** (Using FK class):
```csharp
public SelectFrom<ATPTEFMEntityDetail>
    .Where<ATPTEFMEntityDetail.FK.Entity.SameAsCurrent>
    .View Details;
```

❌ **INCORRECT** (Manual join):
```csharp
SelectFrom<ATPTEFMEntity>
    .InnerJoin<Customer>.On<Customer.bAccountID.IsEqual<ATPTEFMEntity.customerID>>
    .View.Select(this);
```

✅ **CORRECT** (Using FK class):
```csharp
SelectFrom<ATPTEFMEntity>
    .InnerJoin<Customer>.On<ATPTEFMEntity.FK.Customer>
    .View.Select(this);
```

#### Graph Standards
- [ ] Views have `[PXViewName(ATPTEFMMessages.ViewName)]` attribute
- [ ] Event handlers follow pattern: `protected virtual void _(Events.EventType<DAC> e)`
- [ ] Actions have `[PXButton]` and `[PXUIField]` attributes
- [ ] Action methods return `IEnumerable` for adapter pattern
- [ ] Null checks in event handlers: `if (e.Row == null) return;`
- [ ] Modified/added codes would not throw a possible `Object Reference` error

#### Extension Standards
- [ ] Graph extensions override `Initialize()` if needed
- [ ] DAC extensions are `sealed` class
- [ ] Extension field names start with `Usr` prefix
- [ ] SQL ALTER TABLE scripts provided for DAC extensions

#### Attribute Standards
- [ ] StringList attributes have Values, Labels, ValuesArr, LabelsArr regions
- [ ] BQL accessors provided for each option (lowercase class names)
- [ ] Constructor initializes base with arrays
- [ ] Using `PX.Data.BQL` namespace (not `PX.Data.BQL.BqlString`)

### 2. SQL Script Validation

For new DACs or DAC extensions:

- [ ] SQL script exists in `..\..\SCRIPTS\<TableName>.sql`
- [ ] Script includes `CompanyID` column (first column)
- [ ] Audit fields included (CreatedByID, CreatedByScreenID, CreatedDateTime, etc.)
- [ ] Primary key defined with CompanyID
- [ ] Appropriate indexes created for foreign keys
- [ ] IF NOT EXISTS check included
- [ ] Descriptive comments provided

### 3. Performance Review

- [ ] Read-only queries use `PXSelectReadonly`
- [ ] No N+1 query patterns (use joins instead of multiple selects)
- [ ] Bulk operations use `PXDatabase.SelectMulti`
- [ ] Frequently accessed data cached using `PXSelect.StoreCached`
- [ ] Proper indexing strategy in database scripts

### 4. Error Handling

- [ ] Field-level errors use `e.Cache.RaiseExceptionHandling<>`
- [ ] Row-level errors use `throw new PXException(ATPTEFMMessages.ErrorMessage)`
- [ ] Warnings use `PXTrace.WriteWarning(ATPTEFMMessages.WarningMessage)`
- [ ] All error messages stored in `ATPTEFMMessages` class
- [ ] No generic exception messages

### 5. Localization

- [ ] All display names use `ATPTEFMMessages` constants
- [ ] No hardcoded UI strings
- [ ] Error messages support parameterization via `PXLocalizer`
- [ ] Field labels defined in `ATPTEFMMessages`

### 6. Documentation

- [ ] Complex logic has explanatory comments
- [ ] Public methods have XML documentation comments
- [ ] Non-obvious business rules documented
- [ ] SQL scripts have descriptive table/column comments
- [ ] No commented-out code (remove or explain)

### 7. File Organization

- [ ] One class per file
- [ ] File name matches class name exactly
- [ ] Files in correct folder (DAC, BLC, Extensions, etc.)
- [ ] Proper namespace structure
- [ ] Using statements organized (no unused)

### 8. Workflow Implementation (if applicable)

- [ ] Workflow extension class created: `<Graph>_Workflow`
- [ ] State identifier defined
- [ ] All states configured with available actions
- [ ] Transitions defined for state changes
- [ ] Initial state marked with `.IsInitial()`

### 9. ASPX Screen Validation (if applicable)

- [ ] Screen ID follows numbering convention (ATPT1xxx, ATPT2xxx, etc.)
- [ ] Correct master page selected
- [ ] PXDataSource TypeName matches Graph fully qualified name
- [ ] PXDataSource PrimaryView matches Graph view name
- [ ] Control IDs use `ed` prefix
- [ ] DataField matches DAC property name (case-sensitive)
- [ ] Code-behind class inherits from `PX.Web.UI.PXPage`
- [ ] Code-behind class name matches Inherits attribute

### 10. Security & Best Practices

- [ ] No hardcoded connection strings
- [ ] No hardcoded file paths
- [ ] No sensitive data in comments
- [ ] PXCache rights properly defined for actions
- [ ] Transaction scope used for multi-table updates
- [ ] Using statements for IDisposable objects
- [ ] No possible `Null Object Reference` errors

---

## PR Review Process

### Step 1: Initial Scan
1. Check that PR description clearly explains changes
2. Verify branch name follows convention
3. Confirm no merge conflicts
4. Check file count is reasonable for PR scope

### Step 2: Code Review
1. Review each changed file against checklist above
2. Check for adherence to naming conventions
3. Validate BQL queries use FK classes
4. Verify error handling patterns
5. Confirm all strings externalized

### Step 3: SQL Script Review (if applicable)
1. Validate SQL scripts exist for DAC changes
2. Check CompanyID included
3. Verify indexes created
4. Confirm IF NOT EXISTS guards

### Step 4: Documentation Review
1. Check code comments adequate
2. Verify complex logic explained
3. Confirm README or doc updates if needed

---

## Common Issues to Flag

### Critical Issues (Block Merge)
- ❌ Missing `ATPTEFM` prefix on classes
- ❌ Hardcoded UI strings (not in `ATPTEFMMessages`)
- ❌ Legacy BQL syntax instead of Fluent BQL
- ❌ Manual field relationships instead of FK classes
- ❌ Missing SQL scripts for new DACs
- ❌ Missing CompanyID in SQL scripts
- ❌ Compilation errors or warnings
- ❌ Bound DACs not inheriting from `ATPTEFMAudit`

### Major Issues (Request Changes)
- ⚠️ Missing FK class definitions in DACs
- ⚠️ N+1 query patterns
- ⚠️ Poor error handling (generic exceptions)
- ⚠️ Missing null checks in event handlers
- ⚠️ Incorrect file organization
- ⚠️ Missing PXViewName attributes
- ⚠️ Hardcoded file paths or connection strings

### Minor Issues (Request Improvements)
- 💡 Missing XML documentation on public methods
- 💡 Inconsistent code formatting
- 💡 Unused using statements
- 💡 Non-descriptive variable names
- 💡 Missing comments for complex logic
- 💡 Opportunities for code reuse

---

## PR Feedback Templates

### Requesting Changes - Critical Issue
```
**Critical**: [Issue Description]

**Current Code**:
```csharp
[problematic code]
```

**Required Fix**:
```csharp
[correct code]
```

**Reason**: [Explain why this violates standards]
**Reference**: [Link to guideline section]
```

### Requesting Changes - Major Issue
```
**Issue**: [Issue Description]

This doesn't follow our [Standard Name] guideline. Please update to:
```csharp
[suggested code]
```

See: [Reference to copilot-instructions.md section]
```

### Suggesting Improvements
```
**Suggestion**: [Improvement Description]

Consider:
```csharp
[improved code]
```

**Benefits**: [Explain advantages]
```

### Approving with Comments
```
**Looks good!** Minor suggestions:
- [Optional improvement 1]
- [Optional improvement 2]

These are non-blocking. Approved for merge.
```

---

## Post-Review Actions

### After Approval
1. Verify all requested changes implemented
2. Confirm build still passes
3. Check no new files added without review
4. Validate merge strategy (squash vs merge commit)

### After Merge
1. Verify CI/CD pipeline succeeds
2. Confirm deployment to integration environment
3. Validate functionality in deployed environment
4. Update related documentation if needed

---

## FK Class Examples for Review

### Correct FK Class Definition in DAC
```csharp
public static class FK
{
    public class ATPTBranch : Branch.PK.ForeignKeyOf<ATPTEFMEntity>.By<branchID> { }
    public class ATPTCustomer : Customer.PK.ForeignKeyOf<ATPTEFMEntity>.By<customerID> { }
    public class ATPTEmployee : Employee.PK.ForeignKeyOf<ATPTEFMEntity>.By<employeeID> { }
}
```

### Correct FK Usage in Views
```csharp
// Master view
public SelectFrom<ATPTEFMEntity>.View Document;

// Detail view with FK
public SelectFrom<ATPTEFMEntityDetail>
    .Where<ATPTEFMEntityDetail.FK.Entity.SameAsCurrent>
    .View Details;

// Related data with FK join
public SelectFrom<ATPTEFMPayment>
    .InnerJoin<ATPTEFMEntity>.On<ATPTEFMPayment.FK.Entity>
    .Where<ATPTEFMEntity.refNbr.IsEqual<@P.AsString>>
    .View Payments;
```

### Correct FK Usage in Queries
```csharp
// Query with FK join
var results = SelectFrom<ATPTEFMEntity>
    .InnerJoin<Customer>.On<ATPTEFMEntity.FK.Customer>
    .InnerJoin<Branch>.On<ATPTEFMEntity.FK.Branch>
    .Where<ATPTEFMEntity.status.IsEqual<@P.AsString>>
    .View.Select(this, status);
```

---

## References

- **Main Guidelines**: `.github\copilot-instructions.md`
- **Custom Screen Guide**: `.github\instructions\CustomScreen.md`
- **Build Instructions**: `.bat\build-and-deploy.bat` header comments
- **Acumatica Documentation**: https://help.acumatica.com/

---

## Review Sign-Off

After completing review, provide summary:

```
## Review Summary

**Files Reviewed**: [count]
**Critical Issues**: [count] - [list or "None"]
**Major Issues**: [count] - [list or "None"]  
**Minor Suggestions**: [count] - [list or "None"]

**Overall Assessment**: [Approve / Request Changes / Needs Discussion]

**Next Steps**:
- [Action item 1]
- [Action item 2]
```

---

**Remember**: These guidelines ensure code quality, maintainability, and adherence to Acumatica best practices. Be thorough but constructive in your feedback.

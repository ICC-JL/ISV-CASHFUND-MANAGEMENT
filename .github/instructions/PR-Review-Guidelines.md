# Pull Request Review Guidelines

**CRITICAL**: These guidelines are **ONLY** to be applied during Pull Request (PR) review, code review sessions, pre-merge validation, and quality-assurance checks. Do **NOT** apply these during regular development, feature implementation, bug fixing, code generation, or general coding assistance.

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

## PR Review Process

### Step 1: Initial Scan
1. Check that the PR description clearly explains the changes.
2. Verify the branch name follows convention.
3. Confirm there are no merge conflicts.
4. Check that the file count is reasonable for the PR scope.

### Step 2: Code Review
1. Review each changed file against the checklist below.
2. Check adherence to naming conventions.
3. Validate BQL queries use FK classes.
4. Verify error-handling patterns.
5. Confirm all UI strings are externalized.

### Step 3: SQL Script & ProjectSourceControl Review (if applicable)
1. Validate SQL scripts exist for DAC changes.
2. Check `CompanyID` is included.
3. Verify indexes are created.
4. Confirm `IF NOT EXISTS` guards are used.
5. Verify the corresponding `Sql_<DACName>.xml` files in `ProjectSourceControl/` are updated for the affected Acumatica versions.

### Step 4: Documentation Review
1. Check that code comments are adequate.
2. Verify complex logic is explained.
3. Confirm README or documentation updates are included if needed.

---

## PR Review Checklist

### 1. Naming Conventions

- [ ] All classes start with `ATPTEFM` prefix
- [ ] DAC names: `ATPTEFM<EntityName>`
- [ ] Graph names: `ATPTEFM<EntityName>Entry` or `ATPTEFM<EntityName>Maint`
- [ ] Extension names: `ATPTEFM<Base>Extension`
- [ ] Field names use PascalCase (e.g., `RefNbr`, `TranDate`, `CuryAmount`)
- [ ] All UI strings use `ATPTEFMMessages` constants (no hardcoded strings)

### 2. DAC Standards

- [ ] All bound DACs inherit from `ATPTEFMAudit`
- [ ] `PK` class is defined with `PrimaryKeyOf`
- [ ] `FK` class is defined for all relationships
- [ ] FK class naming follows `ProjectPrefix<FKName>` (e.g., `ATPTCustomer`, `ATPTBranch`)
- [ ] All fields have abstract BQL classes
- [ ] All DAC fields have proper attributes (e.g., `PXDBString`, `Branch`, `Customer`)
- [ ] `NoteID` field is included for screens requiring attachments/notes
- [ ] Using statements are included (not fully qualified type names)

### 3. BQL Standards

- [ ] All queries use Fluent BQL syntax (no legacy BQL)
- [ ] FK classes are used with `.SameAsCurrent` or `.FromCurrent`
- [ ] Master-detail views use FK classes: `Where<Detail.FK.Master.SameAsCurrent>`
- [ ] No manual field comparisons where FK classes exist
- [ ] View declarations use the `.View` suffix

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

### 4. Graph Standards

- [ ] Views have `[PXViewName(ATPTEFMMessages.ViewName)]` attribute
- [ ] Event handlers follow pattern: `protected virtual void _(Events.EventType<DAC> e)`
- [ ] Actions have `[PXButton]` and `[PXUIField]` attributes
- [ ] Action methods return `IEnumerable` for the adapter pattern
- [ ] Null checks in event handlers: `if (e.Row == null) return;`
- [ ] Modified/added code cannot throw a possible `Object Reference` error

#### Event Handler Patterns

✅ **Correct RowSelected handler**:
```csharp
protected virtual void _(Events.RowSelected<ATPTEFMEntity> e)
{
    if (e.Row == null) return;

    // UI logic here
}
```

✅ **Correct FieldDefaulting handler**:
```csharp
protected virtual void _(Events.FieldDefaulting<ATPTEFMEntity.status> e)
{
    if (e.Row == null) return;

    e.NewValue = ATPTEFMStatus.Hold;
}
```

✅ **Correct RowPersisting handler**:
```csharp
protected virtual void _(Events.RowPersisting<ATPTEFMEntity> e)
{
    if (e.Row == null) return;

    // Validation logic here
}
```

#### Action Patterns

✅ **Correct action definition**:
```csharp
public PXAction<ATPTEFMEntity> release;

[PXButton(CommitChanges = true)]
[PXUIField(DisplayName = ATPTEFMMessages.Release, MapEnableRights = PXCacheRights.Update)]
protected virtual IEnumerable Release(PXAdapter adapter)
{
    // Release logic here
    return adapter.Get();
}
```

### 5. Extension Standards

- [ ] Graph extensions override `Initialize()` if needed
- [ ] DAC extensions are `sealed` classes
- [ ] Extension field names start with `Usr` prefix
- [ ] SQL `ALTER TABLE` scripts are provided for DAC extensions

### 6. Attribute Standards

- [ ] StringList attributes have Values, Labels, ValuesArr, LabelsArr regions
- [ ] BQL accessors are provided for each option (lowercase class names)
- [ ] Constructor initializes base with arrays
- [ ] Code uses `PX.Data.BQL` namespace (not `PX.Data.BQL.BqlString`)

### 7. SQL Script Validation

For new DACs or DAC extensions:

- [ ] SQL script exists in `..\..\SCRIPTS\<TableName>.sql`
- [ ] Script includes `CompanyID` column (first column)
- [ ] Audit fields are included (`CreatedByID`, `CreatedByScreenID`, `CreatedDateTime`, etc.)
- [ ] Primary key is defined with `CompanyID`
- [ ] Appropriate indexes are created for foreign keys
- [ ] `IF NOT EXISTS` check is included
- [ ] Descriptive comments are provided

### 8. ProjectSourceControl XML Synchronization

When adding or modifying a bounded DAC field, update the corresponding `Sql_<DACName>.xml` file(s) in `ProjectSourceControl/` for each affected Acumatica version.

- [ ] New fields added to the relevant `Sql_<DACName>.xml` files
- [ ] Removed or renamed fields reflected in the XML files
- [ ] Version folders updated (e.g., 24R1, 24R2, 25R1, 25R2, 26R1) based on the PR scope
- [ ] XML files reviewed for consistency with the SQL script

### 9. Performance Review

- [ ] Read-only queries use `PXSelectReadonly`
- [ ] No N+1 query patterns (use joins instead of multiple selects)
- [ ] Bulk operations use `PXDatabase.SelectMulti`
- [ ] Frequently accessed data is cached using `PXSelect.StoreCached`
- [ ] Database scripts have a proper indexing strategy

### 10. Error Handling

- [ ] Field-level errors use `e.Cache.RaiseExceptionHandling<>`
- [ ] Row-level errors use `throw new PXException(ATPTEFMMessages.ErrorMessage)`
- [ ] Warnings use `PXTrace.WriteWarning(ATPTEFMMessages.WarningMessage)`
- [ ] All error messages are stored in `ATPTEFMMessages` class
- [ ] No generic exception messages

### 11. Localization

- [ ] All display names use `ATPTEFMMessages` constants
- [ ] No hardcoded UI strings
- [ ] Error messages support parameterization via `PXLocalizer`
- [ ] Field labels are defined in `ATPTEFMMessages`

### 12. Documentation

- [ ] Complex logic has explanatory comments
- [ ] Public methods have XML documentation comments
- [ ] Non-obvious business rules are documented
- [ ] SQL scripts have descriptive table/column comments
- [ ] No commented-out code (remove or explain)

### 13. File Organization

- [ ] One class per file
- [ ] File name matches class name exactly
- [ ] Files are in the correct folder (`DAC`, `BLC`, `Extensions`, etc.)
- [ ] Proper namespace structure is used
- [ ] Using statements are organized and no unused usings remain

### 14. Workflow Implementation (if applicable)

- [ ] Workflow extension class created: `<Graph>_Workflow`
- [ ] State identifier is defined
- [ ] All states are configured with available actions
- [ ] Transitions are defined for state changes
- [ ] Initial state is marked with `.IsInitial()`

#### Workflow Pattern

✅ **Correct workflow extension**:
```csharp
public class ATPTEFMEntityMaint_Workflow : PXGraphExtension<ATPTEFMEntityMaint>
{
    public override void Configure(PXScreenConfiguration config)
    {
        var context = config.GetScreenConfigurationContext<ATPTEFMEntityMaint, ATPTEFMEntity>();

        context.AddScreenConfigurationFor(screen =>
        {
            return screen
                .StateIdentifierIs<ATPTEFMEntity.status>()
                .AddDefaultFlow(flow => flow
                    .WithFlowStates(fss =>
                    {
                        fss.Add<ATPTEFMStatus.hold>(flowState =>
                        {
                            return flowState
                                .IsInitial()
                                .WithActions(actions =>
                                {
                                    actions.Add(g => g.release);
                                });
                        });
                    })
                    .WithTransitions(transitions =>
                    {
                        transitions.AddGroupFrom<ATPTEFMStatus.hold>(ts =>
                        {
                            ts.Add(t => t.To<ATPTEFMStatus.released>()
                                .IsTriggeredOn(g => g.release));
                        });
                    }));
        });
    }
}
```

### 15. ASPX Screen Validation (if applicable)

- [ ] Screen ID follows numbering convention (e.g., `ATPT1xxx`, `ATPT2xxx`)
- [ ] Correct master page is selected
- [ ] `PXDataSource` `TypeName` matches the Graph fully qualified name
- [ ] `PXDataSource` `PrimaryView` matches the Graph view name
- [ ] Control IDs use correct prefixes (e.g., `ed` for editors, `form`, `grid`, `tab`)
- [ ] `DataField` matches the DAC property name (case-sensitive)
- [ ] Code-behind class inherits from `PX.Web.UI.PXPage`
- [ ] Code-behind class name matches the `Inherits` attribute
- [ ] No hardcoded client-side strings or inline scripts without justification

### 16. Front-End Code Review (HTML / TypeScript / Aurelia)

When reviewing `.html`, `.ts`, Aurelia view-models, and related front-end assets:

- [ ] HTML is semantic and accessible (proper labels, ARIA attributes where needed)
- [ ] Two-way binding syntax is correct and intentional
- [ ] Component lifecycle hooks are used appropriately
- [ ] TypeScript uses strong typing; avoid `any` unless justified
- [ ] Nullable reference types / strict null checks are respected
- [ ] Observables, event listeners, and subscriptions are properly disposed to avoid memory leaks
- [ ] Services use dependency injection correctly
- [ ] Async/await is preferred over raw promises where appropriate
- [ ] Errors are handled at service and component boundaries
- [ ] No hardcoded UI strings (use localization tokens)
- [ ] Code is organized into feature folders and follows project conventions
- [ ] No unused imports, variables, or dead code

### 17. C# Best Practices

- [ ] SOLID principles are respected
- [ ] Async/await is used correctly; no `async void` except for event handlers, no deadlocks from `.Result` or `.Wait()`
- [ ] Exception handling is specific; no empty `catch` blocks or generic `catch (Exception)` without logging/rethrow
- [ ] LINQ usage is readable and not over-complicated; avoid multiple enumerations of `IEnumerable`
- [ ] Nullable reference types are honored; null checks exist where needed
- [ ] Dependency injection is preferred over manual service location
- [ ] `IDisposable` objects are disposed with `using` statements or proper lifetime management
- [ ] Resource disposal (files, streams, database connections, PX objects) is handled safely
- [ ] Code is organized logically; large methods/classes are refactored
- [ ] Magic strings/numbers are avoided; constants or configuration are used
- [ ] Thread-safety is considered for shared/static state

### 18. Security & Best Practices

- [ ] No hardcoded connection strings
- [ ] No hardcoded file paths
- [ ] No sensitive data in comments
- [ ] `PXCache` rights are properly defined for actions
- [ ] Transaction scope is used for multi-table updates
- [ ] `using` statements are used for `IDisposable` objects
- [ ] No possible `Null Object Reference` errors

### 19. Certification Issues

Check for:

- PX1097: Potential issues with DAC field attributes
- PX1068: DAC field type mismatches
- PX1066: BQL field name typos
- PX1065: Missing BQL fields
- PX1060: Strongly-typed BQL field issues

---

## Common Issues to Flag

### Critical Issues (Block Merge)

- ❌ Missing `ATPTEFM` prefix on classes
- ❌ Hardcoded UI strings (not in `ATPTEFMMessages`)
- ❌ Legacy BQL syntax instead of Fluent BQL
- ❌ Manual field relationships instead of FK classes
- ❌ Missing SQL scripts for new DACs
- ❌ Missing `CompanyID` in SQL scripts
- ❌ Compilation errors or warnings
- ❌ Bound DACs not inheriting from `ATPTEFMAudit`
- ❌ Security issues such as hardcoded secrets or unsafe resource handling

### Major Issues (Request Changes)

- ⚠️ Missing FK class definitions in DACs
- ⚠️ N+1 query patterns
- ⚠️ Poor error handling (generic exceptions)
- ⚠️ Missing null checks in event handlers
- ⚠️ Incorrect file organization
- ⚠️ Missing `PXViewName` attributes
- ⚠️ Hardcoded file paths or connection strings
- ⚠️ C# async/await anti-patterns (`async void`, `.Result`, `.Wait()`)
- ⚠️ IDisposable resources not disposed

### Minor Issues (Request Improvements)

- 💡 Missing XML documentation on public methods
- 💡 Inconsistent code formatting
- 💡 Unused using statements
- 💡 Non-descriptive variable names
- 💡 Missing comments for complex logic
- 💡 Opportunities for code reuse
- 💡 Front-end accessibility or TypeScript strictness improvements

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

This does not follow our [Standard Name] guideline. Please update to:

```csharp
[suggested code]
```

See: [Reference to PR-Review-Guidelines.md section]
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

## Output Format

Provide a structured report with:

1. **Summary**: Pass/Fail with issue count and overall assessment (Approve / Request Changes / Needs Discussion)
2. **Critical Issues**: Must fix before merge
3. **Major Issues**: Should fix, request changes
4. **Minor Suggestions**: Nice to have
5. **Files Reviewed**: List of files checked
6. **Next Steps**: Action items for the author

Use the review sign-off format below when finishing:

### Review Sign-Off

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

## References

- **Main Guidelines**: `.github\copilot-instructions.md`
- **Custom Screen Guide**: `.github\instructions\CustomScreen.md`
- **Certification Guide**: `.github\Certification-instructions\Acumatica-Certification-Guide.md`
- **Build Instructions**: `.bat\build-and-deploy.bat` header comments
- **Acumatica Documentation**: https://help.acumatica.com/

---

**Remember**: These guidelines ensure code quality, maintainability, and adherence to Acumatica best practices. Be thorough but constructive in your feedback.
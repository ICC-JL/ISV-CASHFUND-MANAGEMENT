# Acumatica ERP Development Guidelines

## Project Context
- All development is for Acumatica ERP using C# (.Net Framework 4.8)
- Project Prefix: **ATPTEFM**
- Customization Level: ISV Solution (Multi-tenant)

## Naming Conventions

### Class Names
- All class names MUST start with the project prefix: `ATPTEFM`
- DAC (Data Access Class): `ATPTEFM<EntityName>` (e.g., `ATPTEFMFund`, `ATPTEFMCashAdvance`)
- Graph (Business Logic): `ATPTEFM<EntityName>Entry` or `ATPTEFM<EntityName>Maint` (e.g., `ATPTEFMFundEntry`, `ATPTEFMCashAdvanceMaint`)
- Graph Extensions: `ATPTEFM<BaseGraph>Extension` (e.g., `ATPTEFMAPInvoiceEntryExtension`)
- DAC Extensions: `ATPTEFM<BaseDAC>Extension` (e.g., `ATPTEFMAPInvoiceExtension`)
- Attributes: `ATPTEFM<Purpose>Attribute` (e.g., `ATPTEFMFundStatusAttribute`)
- Helper Classes: `ATPTEFM<Purpose>Helper` (e.g., `ATPTEFMHelper`, `ATPTEFMDataFixHelper`)

### String Constants
- All display names, error messages, and UI strings MUST be constants stored in `ATPTEFMMessages` class
- Example: `DisplayName = ATPTEFMMessages.Customer`
- Never use hardcoded strings for user-facing text

### Field Names
- DAC Fields: PascalCase (e.g., `RefNbr`, `TranDate`, `CuryAmount`)
- Database Fields: Match DAC field names
- Use Acumatica naming conventions for standard fields (e.g., `RefNbr` not `ReferenceNumber`)

## Code Structure

### DAC (Data Access Class) Guidelines
```csharp
using PX.Data;
using PX.Objects.AR;
using PX.Objects.GL;
using System;

namespace CashFundManagement.DAC
{
    [Serializable]
    [PXCacheName(ATPTEFMMessages.EntityName)]
    public class ATPTEFMEntityName : ATPTEFMAudit, IBqlTable
    {
        #region Keys
        public class PK : PrimaryKeyOf<ATPTEFMEntityName>.By<refNbr>
        {
            public static ATPTEFMEntityName Find(PXGraph graph, string refNbr) 
                => FindBy(graph, refNbr);
        }
        
        public static class FK
        {
            public class ATPTBranch : PrimaryKeyOf<Branch>.By<Branch.branchID>.ForeignKeyOf<ATPTEFMEntityName>.By<branchID>
            {
            }
            
            public class ATPTCustomer : PrimaryKeyOf<Customer>.By<Customer.bAccountID>.ForeignKeyOf<ATPTEFMEntityName>.By<customerID>
            {
            }
        }
        #endregion
        
        #region RefNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        [PXDefault]
        [PXUIField(DisplayName = ATPTEFMMessages.RefNbr, Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<ATPTEFMEntityName.refNbr>))]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
        #endregion
        
        #region BranchID
        [Branch(useDefaulting: false)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion
        
        #region CustomerID
        [Customer]
        public virtual int? CustomerID { get; set; }
        public abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }
        #endregion
        
        #region NoteID
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion
        
        // Audit fields (CreatedByID, CreatedDateTime, LastModifiedByID, etc.) 
        // are inherited from ATPTEFMAudit base class
    }
}
```

**Important Notes:**
- **Using Statements**: Always include necessary using statements at the top instead of fully qualifying types (e.g., `using PX.Objects.GL;` instead of `PX.Objects.GL.Branch`)
- **Inherit from `ATPTEFMAudit`**: All bound DACs should inherit from `ATPTEFMAudit` base class to automatically include audit fields (CreatedByID, CreatedByScreenID, CreatedDateTime, LastModifiedByID, LastModifiedByScreenID, LastModifiedDateTime, Tstamp)
- **Define FK class**: Always create a `FK` static class with foreign key relationships using the pattern:
  ```csharp
  public class ProjectPrefix<FKName> : PrimaryKeyOf<ForeignTable>.By<ForeignTable.keyField>.ForeignKeyOf<CurrentDAC>.By<currentField>
  ```
- **SQL Script Generation**: After creating a DAC, generate a corresponding SQL CREATE TABLE script and save it to `..\..\SCRIPTS\<TableName>.sql` (relative to project root)
  - Always include `CompanyID` column in the script
  - Follow Acumatica table naming conventions
  - Include all necessary indexes and constraints

### Graph (Business Logic) Guidelines
```csharp
public class ATPTEFMEntityMaint : PXGraph<ATPTEFMEntityMaint, ATPTEFMEntity>
{
    #region Views
    [PXViewName(ATPTEFMMessages.Entity)]
    public SelectFrom<ATPTEFMEntity>.View Document;
    
    
    [PXViewName(ATPTEFMMessages.EntityDetails)]
    public SelectFrom<ATPTEFMEntityDetail>.Where<ATPTEFMEntityDetail.FK.ATPTEFMEntity.SameAsCurrent>.View Details;
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

### Extension Guidelines
```csharp
// Graph Extension
public class ATPTEFMBaseGraphExtension : PXGraphExtension<BaseGraph>
{
    public override void Initialize()
    {
        base.Initialize();
        // Initialization logic
    }
    
    protected virtual void _(Events.RowPersisting<BaseDAC> e)
    {
        if (e.Row == null) return;
        
        // Custom validation logic
    }
}

// DAC Extension
public sealed class ATPTEFMBaseDACExtension : PXCacheExtension<BaseDAC>
{
    #region UsrATPTEFMCustomField
    [PXDBString(50, IsUnicode = true)]
    [PXUIField(DisplayName = ATPTEFMMessages.CustomField)]
    public string UsrATPTEFMCustomField { get; set; }
    public abstract class usrATPTEFMCustomField : PX.Data.BQL.BqlString.Field<usrATPTEFMCustomField> { }
    #endregion
}
```

**Important Notes for DAC Extensions:**
- When adding fields to DAC extensions or modifying existing DACs, always provide the corresponding SQL script in a code block for easy copying to SQL editor
- SQL scripts should include ALTER TABLE statements for extensions or CREATE TABLE for new DACs

**Example SQL Script for DAC Extension:**
```sql
-- Add UsrCustomField to BaseTable
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[BaseTable]') AND name = 'UsrCustomField')
BEGIN
    ALTER TABLE [dbo].[BaseTable]
    ADD [UsrCustomField] [nvarchar](50) NULL
    
    PRINT 'Column UsrCustomField added to BaseTable'
END
ELSE
BEGIN
    PRINT 'Column UsrCustomField already exists in BaseTable'
END
GO
```

### Attribute Guidelines

#### StringList Attribute Template
```csharp
using PX.Data;
using PX.Data.BQL;

namespace CashFundManagement.Attributes
{
    public class ATPTEFMEntityStatusAttribute : PXStringListAttribute
    {
        #region Values
        public const string Hold = "H";
        public const string Balanced = "B";
        public const string Released = "R";
        public const string Closed = "C";
        #endregion

        #region Labels
        public const string HoldLabel = "On Hold";
        public const string BalancedLabel = "Balanced";
        public const string ReleasedLabel = "Released";
        public const string ClosedLabel = "Closed";
        #endregion

        public static readonly string[] ValuesArr = new string[] { Hold, Balanced, Released, Closed };
        public static readonly string[] LabelsArr = new string[] { HoldLabel, BalancedLabel, ReleasedLabel, ClosedLabel };

        public ATPTEFMEntityStatusAttribute() : base(ValuesArr, LabelsArr) { }

        #region BQL Accessors
        public class hold : BqlString.Constant<hold> { public hold() : base(Hold) { } }
        public class balanced : BqlString.Constant<balanced> { public balanced() : base(Balanced) { } }
        public class released : BqlString.Constant<released> { public released() : base(Released) { } }
        public class closed : BqlString.Constant<closed> { public closed() : base(Closed) { } }
        #endregion
    }
}
```

**Important Notes for StringList Attributes:**
1. **Using Statements**: Include `using PX.Data.BQL;` at the top (not `PX.Data.BQL.BqlString`)
2. **Values Region**: Define constant strings for all option values; string length matches the PXDBString field length
3. **Labels Region**: Define constant strings for all display labels
4. **Arrays**: Create readonly arrays `ValuesArr` and `LabelsArr` in a single line each
5. **Constructor**: Initialize base class with the two arrays
6. **BQL Accessors**: Create nested classes for each option:
   - Class name: lowercase version of the option (e.g., `hold`, `balanced`)
   - Inherit from: `BqlString.Constant<T>` where T is the class name
   - Constructor: Call `base(Value)` with the constant value

## BQL (Business Query Language)

### Query Patterns
**IMPORTANT**: Always use Fluent BQL syntax for all queries. Standard BQL syntax is deprecated and should not be used.

**CRITICAL**: Always use FK (Foreign Key) classes with `.SameAsCurrent` or `.FromCurrent` properties instead of manually defining field relationships. This ensures type safety and maintainability.

```csharp
// Master-Detail View with FK (PREFERRED METHOD)
public SelectFrom<ATPTEFMEntity>.View Document;

public SelectFrom<ATPTEFMEntityDetail>
    .Where<ATPTEFMEntityDetail.FK.Entity.SameAsCurrent>
    .View Details;

// Using FromCurrent for parameter-based filtering
public SelectFrom<ATPTEFMEntityDetail>
    .Where<ATPTEFMEntityDetail.refNbr.IsEqual<ATPTEFMEntity.refNbr.FromCurrent>>
    .View Details;

// Simple Select
SelectFrom<ATPTEFMEntity>
    .View.Select(this);

// Select with Where clause using parameters
SelectFrom<ATPTEFMEntity>
    .Where<ATPTEFMEntity.status.IsEqual<@P.AsString>>
    .View.Select(this, ATPTEFMStatus.Released);

// Select with FK-based Join (PREFERRED)
SelectFrom<ATPTEFMEntity>
    .InnerJoin<Customer>.On<ATPTEFMEntity.FK.Customer>
    .Where<ATPTEFMEntity.status.IsEqual<@P.AsString>>
    .View.Select(this, ATPTEFMStatus.Released);

// Select with Multiple FK-based Joins (PREFERRED)
SelectFrom<ATPTEFMEntity>
    .InnerJoin<Customer>.On<ATPTEFMEntity.FK.Customer>
    .InnerJoin<Branch>.On<ATPTEFMEntity.FK.Branch>
    .Where<ATPTEFMEntity.status.IsEqual<@P.AsString>>
    .View.Select(this, ATPTEFMStatus.Released);

// Select with OrderBy
SelectFrom<ATPTEFMEntity>
    .Where<ATPTEFMEntity.status.IsEqual<@P.AsString>>
    .OrderBy<ATPTEFMEntity.tranDate.Desc>
    .View.Select(this, ATPTEFMStatus.Released);

// Select Single Record
SelectFrom<ATPTEFMEntity>
    .Where<ATPTEFMEntity.status.IsEqual<@P.AsString>>
    .OrderBy<ATPTEFMEntity.tranDate.Desc>
    .View.SelectSingleBound(this, null, ATPTEFMStatus.Released);

// Select with Aggregate
SelectFrom<ATPTEFMEntity>
    .Where<ATPTEFMEntity.status.IsEqual<@P.AsString>>
    .AggregateTo<GroupBy<ATPTEFMEntity.customerID>, Sum<ATPTEFMEntity.curyAmount>>
    .View.Select(this, ATPTEFMStatus.Released);

// Complex Query with FK and Current Context
SelectFrom<ATPTEFMEntityDetail>
    .InnerJoin<InventoryItem>.On<ATPTEFMEntityDetail.FK.InventoryItem>
    .InnerJoin<ATPTEFMEntity>.On<ATPTEFMEntityDetail.FK.Entity>
    .Where<ATPTEFMEntity.refNbr.IsEqual<@P.AsString>
        .And<ATPTEFMEntityDetail.lineNbr.IsGreater<@P.AsInt>>>
    .OrderBy<ATPTEFMEntityDetail.lineNbr.Asc>
    .View.Select(this, refNbr, 0);
```

**Graph View Definitions Using FK Classes:**

```csharp
public class ATPTEFMEntityMaint : PXGraph<ATPTEFMEntityMaint, ATPTEFMEntity>
{
    #region Views
    
    // Primary view
    [PXViewName(ATPTEFMMessages.Entity)]
    public SelectFrom<ATPTEFMEntity>.View Document;
    
    // Detail view using FK.SameAsCurrent (PREFERRED)
    [PXViewName(ATPTEFMMessages.EntityDetails)]
    public SelectFrom<ATPTEFMEntityDetail>
        .Where<ATPTEFMEntityDetail.FK.Entity.SameAsCurrent>
        .View Details;
    
    // Alternative: Detail view using FromCurrent
    [PXViewName(ATPTEFMMessages.EntityDetails)]
    public SelectFrom<ATPTEFMEntityDetail>
        .Where<ATPTEFMEntityDetail.refNbr.IsEqual<ATPTEFMEntity.refNbr.FromCurrent>>
        .View Details;
    
    // View with FK-based join
    [PXViewName(ATPTEFMMessages.Customers)]
    public SelectFrom<Customer>
        .InnerJoin<ATPTEFMEntity>.On<Customer.bAccountID.IsEqual<ATPTEFMEntity.customerID.FromCurrent>>
        .View Customers;
    
    #endregion
}
```

**FK Class Benefits:**
- **Type Safety**: Compile-time validation of relationships
- **Maintainability**: Relationship defined once in DAC FK class
- **Readability**: Clear intent with `.SameAsCurrent` and FK class names
- **Refactoring**: Changes to FK propagate automatically
- **IntelliSense**: Better IDE support and discoverability

**When to Use `.SameAsCurrent` vs `.FromCurrent`:**
- **`.SameAsCurrent`**: Use with FK classes in Where clauses for master-detail relationships
  ```csharp
  .Where<ATPTEFMEntityDetail.FK.Entity.SameAsCurrent>
  ```
- **`.FromCurrent`**: Use with field comparisons when you need to reference current record's field value
  ```csharp
  .Where<ATPTEFMEntityDetail.refNbr.IsEqual<ATPTEFMEntity.refNbr.FromCurrent>>
  ```

**Fluent BQL Benefits:**
- More readable and maintainable
- Better IntelliSense support
- Compile-time safety
- Standardized across modern Acumatica versions (2021R1+)
- Type-safe FK relationships

## Workflow Framework (2021R1+)

### Workflow Definition
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
                        fss.Add<ATPTEFMStatus.released>(flowState =>
                        {
                            return flowState
                                .WithActions(actions =>
                                {
                                    actions.Add(g => g.reverse);
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

## Best Practices

### Performance
- Use `PXSelectReadonly` for read-only queries
- Implement proper indexing in database tables
- Avoid N+1 queries - use joins instead of multiple selects
- Use `PXDatabase.SelectMulti` for bulk operations
- Cache frequently accessed data using `PXSelect.StoreCached`

### Error Handling
```csharp
// Field-level errors
if (e.Row.Amount < 0)
{
    e.Cache.RaiseExceptionHandling<ATPTEFMEntity.amount>(
        e.Row, e.Row.Amount, 
        new PXSetPropertyException(ATPTEFMMessages.AmountMustBePositive, PXErrorLevel.Error));
}

// Row-level errors
throw new PXException(ATPTEFMMessages.DocumentCannotBeDeleted);

// Row-level warnings
PXTrace.WriteWarning(ATPTEFMMessages.WarningMessage);
```

### Transaction Handling
```csharp
using (PXTransactionScope ts = new PXTransactionScope())
{
    // Multiple operations here
    graph.Actions.PressSave();
    
    ts.Complete();
}
```

### Localization
- All UI strings in `ATPTEFMMessages` must support localization
- Use `PXLocalizer` for runtime string formatting
- Test with multiple languages/locales

## Code Organization

### SQL Script Generation
After creating a new DAC, generate a corresponding SQL CREATE TABLE script:

**Location**: `..\..\SCRIPTS\<TableName>.sql` (relative to project root)

**Template**:
```sql
-- Table: ATPTEFMEntityName
-- Description: [Brief description of the table purpose]

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ATPTEFMEntityName]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ATPTEFMEntityName](
        [CompanyID] [int] NOT NULL,
        [RefNbr] [nvarchar](15) NOT NULL,
        [BranchID] [int] NULL,
        [CustomerID] [int] NULL,
        [Status] [char](1) NULL,
        [TranDate] [datetime] NULL,
        [Description] [nvarchar](256) NULL,
        [CuryID] [nvarchar](5) NULL,
        [CuryAmount] [decimal](19, 4) NULL,
        -- Audit Fields
        [CreatedByID] [uniqueidentifier] NULL,
        [CreatedByScreenID] [char](8) NULL,
        [CreatedDateTime] [datetime] NULL,
        [LastModifiedByID] [uniqueidentifier] NULL,
        [LastModifiedByScreenID] [char](8) NULL,
        [LastModifiedDateTime] [datetime] NULL,
        [tstamp] [timestamp] NOT NULL,
        [NoteID] [uniqueidentifier] NULL,
        
        CONSTRAINT [PK_ATPTEFMEntityName] PRIMARY KEY CLUSTERED 
        (
            [CompanyID] ASC,
            [RefNbr] ASC
        )
    )
    
    -- Create indexes
    CREATE NONCLUSTERED INDEX [IX_ATPTEFMEntityName_BranchID] ON [dbo].[ATPTEFMEntityName]
    (
        [CompanyID] ASC,
        [BranchID] ASC
    )
    
    CREATE NONCLUSTERED INDEX [IX_ATPTEFMEntityName_CustomerID] ON [dbo].[ATPTEFMEntityName]
    (
        [CompanyID] ASC,
        [CustomerID] ASC
    )
    
    PRINT 'Table ATPTEFMEntityName created successfully'
END
ELSE
BEGIN
    PRINT 'Table ATPTEFMEntityName already exists'
END
GO
```

**Important Notes:**
- Always include `CompanyID` as the first column and part of the primary key
- Include all  by the audit fields from `ATPTEFMAudit` base class
- Add appropriate indexes for foreign keys and frequently queried fields
- Use appropriate SQL data types matching the DAC field attributes
- Include descriptive comments
- Use IF NOT EXISTS to prevent errors on re-runs

### Project Structure
```
CashFundManagement/
├── Attributes/         # Custom attributes
├── BLC/               # Business Logic Controllers (Graphs)
│   └── Workflow/      # Workflow definitions
├── Classes/           # Helper/Utility classes
├── DAC/               # Data Access Classes
│   ├── Setup/         # Setup/Configuration DACs
│   └── Unbound/       # Unbound DACs (non-persisted)
├── Extensions/        # Graph and DAC extensions
│   ├── BLC/           # Graph extensions
│   └── DAC/           # DAC extensions
├── Helper/            # Helper classes
├── Messages/          # ATPTEFMMessages class
└── Properties/        # Assembly info
```

### File Naming
- One class per file
- File name = Class name (e.g., `ATPTEFMFund.cs`, `ATPTEFMFundEntry.cs`)
- Use folders to organize by functional area

## Build & Deployment

### Build Script
- Use `.bat\build-and-deploy.bat` for building and deploying
- Update `ACUMATICA_SITE_PATH` for your local environment
- Script supports 23R2, 24R1, 24R2, 25R1 profiles
- Auto-detects MSBuild (VS 2022 Enterprise/Professional/Community)

### Pre-deployment Checklist
- [ ] All compilation warnings resolved
- [ ] Code follows naming conventions (ATPT prefix)
- [ ] All strings externalized to ATPTEFMMessages
- [ ] DACs have proper keys and indexes defined
- [ ] Workflows tested for all state transitions
- [ ] No hardcoded connection strings or paths
- [ ] Extensions properly registered
- [ ] Database scripts included (if schema changes)

## Pull Request Review Guidelines
- Use `.github\instructions\PR-Review-Guidelines.md` or https://github.com/ICC-Dev-Team/ISV-CASHFUND-MANAGEMENT/blob/main/.github/instructions/PR-Review-Guidelines.md for PR review checklist
- Ensure all guidelines are followed before approving PRs
- Verify code quality, performance, and security aspects


## Additional Resources
- Acumatica Developer Documentation: https://help.acumatica.com/
- API Reference: https://help.acumatica.com/Help?ScreenId=ShowWiki&pageid=api-reference
- Community Forums: https://community.acumatica.com/
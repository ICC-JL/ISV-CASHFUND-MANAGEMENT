# Acumatica ERP Development Guidelines

## Project Context

- All development is for Acumatica ERP using C# (.Net Framework 4.8)
- Project Prefix: **ATPTEFM**
- Customization Level: ISV Solution (Multi-tenant)

## Naming Conventions

- **Classes**: Must start with `ATPTEFM` prefix
  - DAC: `ATPTEFM<EntityName>` (e.g., `ATPTEFMFund`)
  - Graph: `ATPTEFM<EntityName>Entry` or `ATPTEFM<EntityName>Maint`
  - Graph Extensions: `ATPTEFM<BaseGraph>Extension`
  - DAC Extensions: `ATPTEFM<BaseDAC>Extension`
  - Attributes: `ATPTEFM<Purpose>Attribute`
  - Helpers: `ATPTEFM<Purpose>Helper`
- **Strings**: All UI strings in `ATPTEFMMessages` class. Never hardcode.
- **Fields**: PascalCase (e.g., `RefNbr`, `TranDate`, `CuryAmount`)

## Core Rules

- All bound DACs inherit from `ATPTEFMAudit` (includes audit fields)
- Always define `PK` and `FK` classes in DACs
- Always use Fluent BQL (standard BQL is deprecated)
- Use `FK.SameAsCurrent` or `FK.FromCurrent` for relationships
- Generate SQL scripts after creating/modifying DACs
- **Synchronize ProjectSourceControl XML files**: When adding or modifying a bounded DAC field, update the corresponding `Sql_<DACName>.xml` file(s) in `ProjectSourceControl/`. Always ask the user which version(s) (e.g., 24R1, 25R2) should receive the update.
- One class per file, filename matches class name

## Documentation Standards

- **DAC Fields**: Every DAC field must have a `/// <summary>` XML comment explaining its purpose and business logic.
  - Include what the field represents, how it's used, and any auto-calculated behavior.
  - Example:
    ```csharp
    /// <summary>
    /// Unique identifier for the cash fund. Auto-generated based on the Fund Numbering Sequence.
    /// </summary>
    [PXDBString(15, IsKey = true, IsUnicode = true)]
    public virtual string FundNbr { get; set; }
    ```
  - Example (calculated field):
    ```csharp
    /// <summary>
    /// Current balance of the cash fund in the specified currency.
    /// Updated automatically when transactions are released.
    /// </summary>
    [PXDBDecimal(2)]
    public virtual decimal? CurrentBalance { get; set; }
    ```

## Project Structure

```
CashFundManagement/
├── Attributes/         # Custom attributes
├── BLC/               # Business Logic Controllers (Graphs)
│   └── Workflow/      # Workflow definitions
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

## Agent Delegation Rules

When the user asks for:

- **DAC creation/modification** -> Delegate to @acumatica-dac-specialist
- **Graph/BLC creation or business logic** -> Delegate to @acumatica-graph-specialist
- **BQL queries or view definitions** -> Delegate to @acumatica-bql-specialist
- **Code review or certification checks** -> Delegate to @acumatica-reviewer
- **Workflow definitions** -> Delegate to @acumatica-graph-specialist
- **Mobile app customization / MSDL** -> Delegate to @acumatica-mobile-specialist

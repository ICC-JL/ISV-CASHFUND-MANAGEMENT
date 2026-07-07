---
description: Fix Acuminator PX1067 warnings by redeclaring weakly typed BQL fields from a base DAC in the derived DAC.
---

Fix the Acuminator PX1067 warning for the DAC file(s) provided by the user: $ARGUMENTS

PX1067 means: "The DAC does not contain a redeclaration of a BQL field declared in the base DAC."

For each file/path the user provided, follow these steps exactly:

1. **Resolve the target file**
   - If the argument is a full file path, use it as-is.
   - If it is a relative path or file name, locate it under the workspace root.
   - If multiple files are provided, process each one independently.

2. **Read the target DAC file**
   - Identify the class declaration and its immediate base class (e.g., `public class ATPTEFMCashAdvance : Base.ATPTEFMAudit, ...`).
   - Determine the base DAC's namespace-qualified name and locate its `.cs` file in the same project or workspace.

3. **Read the base DAC file**
   - Find every BQL field declared as `public abstract class <Name> : IBqlField { ... }`. These are weakly typed and trigger PX1067 when not redeclared in the derived DAC.
   - For each weakly typed BQL field, find the corresponding DAC property (same name, case-insensitive) and determine its CLR type so you can choose the correct strongly typed BQL field class.

4. **Map the property type to the strongly typed BQL field class**
   Use the standard Acumatica BQL field types:
   - `Guid?` → `PX.Data.BQL.BqlGuid.Field<Name>`
   - `string` / `String` → `PX.Data.BQL.BqlString.Field<Name>`
   - `DateTime?` → `PX.Data.BQL.BqlDateTime.Field<Name>`
   - `int?` / `Int32?` → `PX.Data.BQL.BqlInt.Field<Name>`
   - `bool?` → `PX.Data.BQL.BqlBool.Field<Name>`
   - `decimal?` → `PX.Data.BQL.BqlDecimal.Field<Name>`
   - `long?` / `Int64?` → `PX.Data.BQL.BqlLong.Field<Name>`
   - `short?` / `Int16?` → `PX.Data.BQL.BqlShort.Field<Name>`
   - `byte?` → `PX.Data.BQL.BqlByte.Field<Name>`
   - `byte[]` / `Byte[]` → `PX.Data.BQL.BqlByteArray.Field<Name>`
   - `double?` → `PX.Data.BQL.BqlDouble.Field<Name>`
   - `float?` → `PX.Data.BQL.BqlFloat.Field<Name>`

5. **Edit the derived DAC file**
   - Add a new region named `#region Redeclared Base BQL Fields` (or `#region Audit BQL Fields` if the base class is an audit base class) near the top of the derived class, after any `PK`/`UK` key classes but before the first data field region.
   - For each weakly typed BQL field found in the base class, add one line:
     ```csharp
     public new abstract class <Name> : <StronglyTypedBQLField> { }
     ```
   - Use `new` because the abstract class is being hidden from the base class.
   - Preserve the exact BQL field name from the base class (e.g., `Tstamp` stays Pascal-case if that is how it was declared).
   - Do NOT redeclare the property itself; only redeclare the abstract BQL field classes.
   - Do NOT modify the base DAC file in any way.
   - Do NOT change any other logic, attributes, or comments in the derived DAC.

6. **Report the result**
   - For each file, list the base DAC and the BQL fields that were redeclared.
   - Mention if no weakly typed BQL fields were found and therefore no changes were made.
   - Suggest next steps: rebuild the project and re-run Acuminator to verify the PX1067 warning is gone.

7. **Route to the correct specialist if needed**
   - If you are a primary agent, delegate the actual file editing to the `@acumatica-dac-specialist` subagent and return a concise summary to the user.

Remember: no SQL scripts or ProjectSourceControl XML updates are needed for PX1067 fixes because no bound columns are added or modified.

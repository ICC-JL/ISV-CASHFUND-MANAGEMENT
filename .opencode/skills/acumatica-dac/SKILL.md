---
name: acumatica-dac
description: Use when creating or modifying Acumatica DACs, generating SQL scripts, or working with data access classes. Covers PK/FK patterns, field attributes, audit inheritance, and SQL generation.
---

# Acumatica DAC Development Skill

## Complete DAC Template

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
            public class Branch : PrimaryKeyOf<Branch>.By<Branch.branchID>.ForeignKeyOf<ATPTEFMEntityName>.By<branchID>
            { }

            public class Customer : PrimaryKeyOf<Customer>.By<Customer.bAccountID>.ForeignKeyOf<ATPTEFMEntityName>.By<customerID>
            { }
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
    }
}
```

## SQL CREATE TABLE Template

```sql
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

    CREATE NONCLUSTERED INDEX [IX_ATPTEFMEntityName_BranchID] ON [dbo].[ATPTEFMEntityName]
    ([CompanyID] ASC, [BranchID] ASC)

    CREATE NONCLUSTERED INDEX [IX_ATPTEFMEntityName_CustomerID] ON [dbo].[ATPTEFMEntityName]
    ([CompanyID] ASC, [CustomerID] ASC)

    PRINT 'Table ATPTEFMEntityName created successfully'
END
ELSE
BEGIN
    PRINT 'Table ATPTEFMEntityName already exists'
END
GO
```

## DAC Extension Template

```csharp
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

## SQL ALTER TABLE for Extension

```sql
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[BaseTable]') AND name = 'UsrATPTEFMCustomField')
BEGIN
    ALTER TABLE [dbo].[BaseTable]
    ADD [UsrATPTEFMCustomField] [nvarchar](50) NULL
    PRINT 'Column UsrATPTEFMCustomField added to BaseTable'
END
ELSE
BEGIN
    PRINT 'Column UsrATPTEFMCustomField already exists in BaseTable'
END
GO
```

## StringList Attribute Template

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

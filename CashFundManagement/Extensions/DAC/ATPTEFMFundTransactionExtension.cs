using CashFundManagement.Attributes;
using CashFundManagement.DAC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Data.BQL;
using PX.Data.SQLTree;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.TM;
using System;
using System.Collections.Generic;
using static CashFundManagement.DAC.ATPTEFMFundTransaction;

namespace CashFundManagement.Extensions.DAC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// 2025-02-10 : 009490 - CFM 2024R1 - Fund ID Lookup [Fund Transaction Enhancement]
    /// 010199 - CFM 2024R1 - Fund Transaction Screen [Fund ID Field]
    /// </remarks>
    public sealed class ATPTEFMFundTransactionExtension : PXCacheExtension<ATPTEFMFundTransaction>
    {
#if Version23R2
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsFMFilterByEmployeeDelegates; 
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive && ATPTEFMPrefetchSetup.IsFMFilterByEmployeeDelegates;
#endif

        #region RefNbr
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXSelectorAttribute))]
        [PXSelector(typeof(Search2<
            refNbr,
            InnerJoin<EPEmployee,
                On<EPEmployee.bAccountID, Equal<requestedByID>>>,
            Where<createdByID, Equal<Optional<AccessInfo.userID>>,
                Or<EPEmployee.userID, Equal<Optional<AccessInfo.userID>>,
                Or<noteID, Approver<Optional<AccessInfo.contactID>>,
                Or<requestedByID, WingmanUser<Optional<AccessInfo.userID>>>>>>,
            OrderBy<
                Desc<refNbr>>>),
            typeof(branchID),
            typeof(date),
            typeof(refNbr),
            typeof(fundID),
            typeof(descr),
            typeof(requestedByID),
            typeof(departmentID),
            typeof(requestedAmount))]
        public string RefNbr { get; set; }
        public abstract class refNbr : BqlString.Field<refNbr> { }
        #endregion

        #region RequestedByID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXSelectorAttribute))]
        [PXSubordinateAndWingmenSelector]
        public int? RequestedByID { get; set; }
        public abstract class requestedByID : BqlInt.Field<requestedByID> { }
        #endregion

        #region FundID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXSelectorAttribute))]
        [PXSelector(typeof(Search2<
            ATPTEFMFund.fundCD,
            LeftJoin<EPEmployee, 
                On<ATPTEFMFund.custodianID, Equal<EPEmployee.bAccountID>>>,
            Where<ATPTEFMFund.fundType, Equal<Current<fundType>>,
                And<ATPTEFMFund.status, Equal<ATPTEFMFundStatusAttribute.activeValue>,
                And<ATPTEFMFund.isActive, Equal<boolTrue>,
                And<Where<ATPTEFMFund.custodianID, DelegateOf<Current<requestedByID>>,
                    Or<ATPTEFMFund.custodianID, Equal<Current<requestedByID>>,
                    Or<ATPTEFMFund.custodianID, IsSuperiorContactOf<Current<requestedByID>>>>>>>>>,
            OrderBy<
                Desc<ATPTEFMFund.fundCD>>>),
                    typeof(ATPTEFMFund.fundCD),
                    typeof(EPEmployee.acctName),
                    SubstituteKey = typeof(ATPTEFMFund.fundCD),
                    DescriptionField = typeof(ATPTEFMFund.descr))]
        public string FundID { get; set; }
        public abstract class fundID : BqlString.Field<fundID> { }
        #endregion
    }
    #region BQL Operators
    //Company tree, get all superior contacts of the current baccountid
    public class IsSuperiorContactOf<Operand> : IBqlComparison, IBqlCreator, IBqlVerifier
        where Operand : IBqlOperand, new()
    {
        private IBqlCreator _operand;

        public void Verify(PXCache cache, object item, List<object> pars, ref bool? result, ref object value)
        {
            result = null;
            value = null;
        }

        public virtual bool AppendExpression(ref SQLExpression exp, PXGraph graph, BqlCommandInfo info, BqlCommand.Selection selection)
        {
            bool flag = true;
            if (info.Fields is BqlCommand.EqualityList equalityList)
            {
                equalityList.NonStrict = true;
            }

            SQLExpression exp2 = null;
            if (!typeof(IBqlCreator).IsAssignableFrom(typeof(Operand)))
            {
                if (info.BuildExpression)
                {
                    exp2 = BqlCommand.GetSingleExpression(typeof(Operand), graph, info.Tables, selection, BqlCommand.FieldPlace.Condition);
                }
                info.Fields?.Add(typeof(Operand));
            }
            else
            {
                if (_operand == null)
                {
                    _operand = _operand.createOperand<Operand>();
                }
                flag &= _operand.AppendExpression(ref exp2, graph, info, selection);
            }

            if (!info.BuildExpression)
            {
                return flag;
            }

            Query query = new Query();
            query[typeof(EPCompanyTreeMember.workGroupID)]
                .From(typeof(EPCompanyTreeMember))
                .Join(typeof(BAccount))
                .On(SQLExpression.EQ(typeof(BAccount.defContactID), typeof(EPCompanyTreeMember.contactID))
                    .And(Column.SQLColumn(typeof(BAccount.bAccountID)).EQ(exp2))
                    .And(Column.SQLColumn(typeof(EPCompanyTreeMember.active)).EQ(1)))
                .Where(new SQLConst(1).EQ(1));

            Query query2 = new Query();
            query2[typeof(BAccount.bAccountID)]
                .From(typeof(BAccount))
                .Join(typeof(EPCompanyTreeMember))
                .On(SQLExpression.EQ(typeof(BAccount.defContactID), typeof(EPCompanyTreeMember.contactID)))
                .Join(typeof(EPCompanyTreeH))
                .On(SQLExpression.EQ(typeof(EPCompanyTreeMember.workGroupID), typeof(EPCompanyTreeH.parentWGID))
                    .And(Column.SQLColumn(typeof(EPCompanyTreeH.parentWGID)).NE(Column.SQLColumn(typeof(EPCompanyTreeH.workGroupID)))))
                .Where(Column.SQLColumn(typeof(EPCompanyTreeH.workGroupID)).In(query));

            exp = exp.In(query2);
            return flag;
        }
    }
    //Employee Delegates, get all delegates/wingmans of the current baccountid
    public class DelegateOf<BAccountID> : IBqlComparison, IBqlCreator, IBqlVerifier 
        where BAccountID : IBqlOperand, new()
    {
        private IBqlCreator _operand;

        public void Verify(PXCache cache, object item, List<object> pars, ref bool? result, ref object value)
        {
            result = null;
            value = null;
        }

        public bool AppendExpression(ref SQLExpression exp, PXGraph graph, BqlCommandInfo info, BqlCommand.Selection selection)
        {
            bool flag = true;
            SQLExpression exp2 = null;
            flag &= BqlCommand.AppendExpression<BAccountID>(ref exp2, graph, info, selection, ref _operand);
            if (graph == null || !info.BuildExpression)
            {
                return flag;
            }

            exp = exp.In(new Query().Select<EPWingman.wingmanID>()
                .From<EPWingman>()
                .Where(new Column<EPWingman.employeeID>().EQ(exp2)
                    .And(new Column<EPWingman.isActive>().EQ(new SQLConst(1)))
                    .And(new Column<EPWingman.delegationOf>().EQ(new SQLConst("E")))));
            return flag;
        }
    }
    #endregion
}

using CashFundManagement.DAC;
using PX.Data.BQL;

namespace CashFundManagement.Extensions.DAC
{
    public class ATPTEFMEPAssignmentMapExtension
    {
        public static class ATPTEFMAssignmentMapType
        {
            public class AssignmentMapTypeATPTEFMCashAdvance : BqlString.Constant<AssignmentMapTypeATPTEFMCashAdvance>
            {
                public AssignmentMapTypeATPTEFMCashAdvance() : base(typeof(ATPTEFMCashAdvance).FullName) { }
            }

            public class AssignmentMapTypeATPTEFMFundTransaction : BqlString.Constant<AssignmentMapTypeATPTEFMFundTransaction>
            {
                public AssignmentMapTypeATPTEFMFundTransaction() : base(typeof(ATPTEFMFundTransaction).FullName) { }
            }

            public class AssignmentMapTypeATPTEFMFund : BqlString.Constant<AssignmentMapTypeATPTEFMFund>
            {
                public AssignmentMapTypeATPTEFMFund() : base(typeof(ATPTEFMFund).FullName) { }
            }

            public class AssignmentMapTypeATPTEFMReplenishment : BqlString.Constant<AssignmentMapTypeATPTEFMReplenishment>
            {
                public AssignmentMapTypeATPTEFMReplenishment() : base(typeof(ATPTEFMReplenishment).FullName) { }
            }

            public class AssignmentMapTypeATPTEFMMonthEnd : BqlString.Constant<AssignmentMapTypeATPTEFMMonthEnd>
            {
                public AssignmentMapTypeATPTEFMMonthEnd() : base(typeof(ATPTEFMMonthEnd).FullName) { }
            }
        }
    }
}
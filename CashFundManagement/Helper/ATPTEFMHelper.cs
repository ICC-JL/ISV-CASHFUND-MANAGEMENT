using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.GL;
using PX.Objects.GL.FinPeriods;
using System;

namespace CashFundManagement.Helper {
    public static class ATPTEFMHelper
    {
        public static PXAdapter StartLongOperation(this PXGraph graph, PXAdapter adapter, Action method)
        {
            PXLongOperation.StartOperation(graph, delegate
            {
                method();
            });
            return adapter;
        }

        public static void StartLongOperation(PXGraph graph, Action method)
        {
            PXLongOperation.StartOperation(graph, () =>
            {
                method();
            });
        }

        public static PXSetPropertyException GetPropertyException(IBqlTable row, string message, PXErrorLevel errorLevel)
        {
        #if Version24R1
            return new PXSetPropertyException(row, message, errorLevel);
        #else
            return new PXSetPropertyException(message, errorLevel);
        #endif
        }

        public static string GetFinPeriod(PXGraph graph, DateTime? date)
        {
            if (date == null) return null;

            MasterFinPeriod period = SelectFrom<MasterFinPeriod>
                .Where<MasterFinPeriod.startDate.IsLessEqual<@P.AsDateTime>
                    .And<MasterFinPeriod.endDate.IsGreater<@P.AsDateTime>>>
                .View.Select(graph, date, date);

            if (period == null)
            {
                return null;
            }
            return period.FinPeriodID;
        }

        public class stringEmpty : BqlType<IBqlString, string>.Constant<stringEmpty> {
            public stringEmpty()
                : base(string.Empty) {
            }
        }
    }
}

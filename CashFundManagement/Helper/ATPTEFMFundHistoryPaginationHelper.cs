using System.Collections.Generic;
using System.Linq;

namespace CashFundManagement.Helper {
    internal class ATPTEFMFundHistoryPaginationHelper
    {
        internal int StartIndex { get; set; }
        internal int TotalRows { get; set; }
        internal object NextRecord { get; set; }
        internal object PreviousRecord { get; set; }

        internal static ATPTEFMFundHistoryPaginationHelper GetRecordPosition<T>(IEnumerable<T> data, object referenceNumber, int rowsToRetrieve = 0) where T : class
        {
            int startIndex = data.Cast<T>().ToList().FindIndex(x => (string)x.GetType().GetProperty("RefNbr").GetValue(x) == (string)referenceNumber);
            int totalRows = data.Count();
            object nextRecord = data.Cast<T>().ToList().SkipWhile(x => (string)x.GetType().GetProperty("RefNbr").GetValue(x) != (string)referenceNumber).Skip(1).FirstOrDefault();
            object prevRecord = data.Cast<T>().ToList().TakeWhile(x => (string)x.GetType().GetProperty("RefNbr").GetValue(x) != (string)referenceNumber).LastOrDefault();

            return new ATPTEFMFundHistoryPaginationHelper
            {
                StartIndex = startIndex,
                TotalRows = totalRows,
                NextRecord = nextRecord,
                PreviousRecord = prevRecord
            };
        }
    }
}

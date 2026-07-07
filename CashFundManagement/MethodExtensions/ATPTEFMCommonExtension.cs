using PX.Data;
using System;


namespace CashFundManagement.MethodExtensions {

    public static class ATPTEFMCommonExtension
    {
        public static decimal? RoundDecimal(this decimal? value, int numberOfDecimal)
            => Math.Round((value ?? 0), numberOfDecimal);

        public static bool IsDeleted(this PXCache cache, IBqlTable table)
        {
            int count = 0;

            foreach (var item in cache.Deleted) count += 1;

            if (cache.GetStatus(table) == PXEntryStatus.Notchanged && count > 0) return true;

            if (cache.GetStatus(table) == PXEntryStatus.Deleted) return true;

            return false;
        }

        public static bool HasChange(this PXCache cache, IBqlTable table)
        {
            return cache.GetStatus(table) != PXEntryStatus.Notchanged;
        }

    }
}
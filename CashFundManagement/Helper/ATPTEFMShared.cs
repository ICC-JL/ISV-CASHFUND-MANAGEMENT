using PX.Data;
using System.Collections;
using System.Security.Permissions;

namespace CashFundManagement.Helper {
    public static class ATPTEFMShared
    {
        [ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
        [SecurityPermission(SecurityAction.Assert, Unrestricted = true)]
        public class ATPTPXGraphWithWorkflow<TGraph, TPrimary> : PXGraph
            where TGraph : PXGraph
            where TPrimary : class, IBqlTable, new()
        {
            public PXSave<TPrimary> Save;
            public PXCancel<TPrimary> Cancel;
            public PXInsert<TPrimary> Insert;
            public PXDelete<TPrimary> Delete;
            public PXCopyPasteAction<TPrimary> CopyPaste;
            public PXFirst<TPrimary> First;
            public PXPrevious<TPrimary> Previous;
            public PXNext<TPrimary> Next;
            public PXLast<TPrimary> Last;

            /// <exclude />
            public override bool CanClipboardCopyPaste()
            {
                return true;
            }

            public bool SkipScopeTransaction { get; set; } = false;

            public PXAction<TPrimary> RemoveFromHold;
            [PXButton(CommitChanges = true), PXUIField(DisplayName = "Remove Hold")]
            public virtual IEnumerable removeFromHold(PXAdapter adapter) => adapter.Get();

            public PXAction<TPrimary> PutOnHold;
            [PXButton(CommitChanges = true), PXUIField(DisplayName = "Hold")]
            public virtual IEnumerable putOnHold(PXAdapter adapter) => adapter.Get();
        }
    }
}

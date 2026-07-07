using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Data;
using PX.Objects.CA;
using PX.SM;
using System;
using System.Collections;
using System.Collections.Generic;


namespace CashFundManagement.Extensions.BLC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public class ATPTEFMCashTransferEntryExtension : PXGraphExtension<CashTransferEntry>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        #region Print FundTransfer
        public PXAction<CATransfer> ATPTEFMPrintFundTransfer;
        [PXButton(Category = ATPTEFMMessages.Report), PXUIField(DisplayName = "Print Fund Transfer")]
        public IEnumerable aTPTEFMPrintFundTransfer(PXAdapter adapter)
        {
            foreach (CATransfer transfer in adapter.Get<CATransfer>())
            {
                CashTransferEntry graph = PXGraph.CreateInstance<CashTransferEntry>();


                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    [ATPTEFMMessages.TransferNbr] = transfer.TransferNbr
                };

                var report = new PXReportRequiredException(parameters, ATPTEFMMessages.SCREENID_FT, ATPTEFMMessages.FundTransfer);
                report.Mode = PXBaseRedirectException.WindowMode.New;
                this.Base.Transfer.View.RequestRefresh();

                throw new PXRedirectWithReportException(graph, report, ATPTEFMMessages.Preview);
            }
            return adapter.Get();
        }
        #endregion


        #region Events

        #region Print Fund Transfer
        protected virtual void _(Events.RowSelected<CATransfer> e)
        {
            CATransfer row = e.Row;

            if (row == null)
                return;
            ATPTEFMPrintFundTransfer.SetVisible(false);
        }

        #endregion

        #endregion
    }
}
using ATPTPhilippineTax.Attributes;
using CashFundManagement.DAC;
using CashFundManagement.Helper;
using CashFundPhilippineTax.Messages;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.Common;
using PX.Objects.EP;
using PX.Objects.TX;
using System.Linq;

namespace CashFundPhilippineTax.Graph.Extension {
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS} <br/>
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG <br/>
    /// 2025-02-11 : 010203 - CFM 2024R1 - Replenishment Tax Clean up
    /// 2025-03-28 : Remove override view of Philtax base view : 10964 : RFS
    /// 2025-06-23 : Remove code for ATC code deletion on Replenishment Bill creation. This is now handled by the <see cref="ATPTAPInvoiceDefaultATCAttribute"/>. 012116 {RRS}
    /// </remarks>
    public class ATPTAPInvoiceEntry : PXGraphExtension<ATPTPhilippineTax.Graph.Extensions.ATPTAPInvoiceEntry, APInvoiceEntry>
    {
#if Version23R2
        public static bool IsActive() => ATPTPhilippineTax.Helpers.ATPTModule.IsActive; 
#else
        public static bool IsActive() => ATPTPhilippineTax.Helpers.ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;

#endif

        #region Additional Views

        //public PXSelectReadonly2<
        //    EPTaxTran,
        //    InnerJoin<EPExpenseClaimDetails,
        //        On<EPExpenseClaimDetails.claimDetailID, Equal<EPTaxTran.claimDetailID>>>,
        //    Where<EPExpenseClaimDetails.aPRefNbr, Equal<Current<APInvoice.refNbr>>,
        //        And<EPExpenseClaimDetails.aPDocType, Equal<Current<APInvoice.docType>>>>>
        //    ATPTEmployeeExpense;

        #endregion

        #region Override
        //TODO : Reimplement to Initialize method.
        /// <remarks>
        /// 010745 - Add validation for Bill linked to Replenishment. {JCL} </br> 
        /// </remarks>
        public delegate void PersistDelegate();
        [PXOverride]
        public void Persist(PersistDelegate baseMethod)
        {
            APInvoice row = Base.Document.Current;

            if (row != null && this.Base.Accessinfo?.ScreenID == "AP.30.10.00")
            {
                ATPTEFMReplenishment linkedReplenishment = ATPTEFMReplenishment.PK.Find(Base, row.OrigRefNbr);

                if (this.Base1.ATPTSetup.Current.ValidateClaimInBills == true && (linkedReplenishment != null))
                {
                    if (this.Base1.ATPTEmployeeExpense.Select()?.Any() ?? false)
                    {
                        decimal? whtTot = decimal.Zero;

                        foreach (PXResult<EPExpenseClaimDetails, EPTaxTran> ECRow in this.Base1.ATPTEmployeeExpense.Select())
                        {
                            EPTaxTran epTaxTran = (EPTaxTran)ECRow;

                            if (Tax.PK.Find(this.Base, epTaxTran?.TaxID)?.TaxType == CSTaxType.Withholding)
                                whtTot += epTaxTran.CuryTaxAmt;
                        }

                        if (row.CuryOrigWhTaxAmt != whtTot)
                            throw new PXException(ATPTMessages.BillClaimTotalError);
                    }
                }
            }
            baseMethod();
        }
        #endregion

        /// <remarks>
        /// 010745 - Hide Expense Receipt Details Tab if empty. {JCL} </br>
        /// </remarks>
        protected void _(Events.RowSelected<APInvoice> e)
        {
            APInvoice row = e.Row;

            if (this.Base.Accessinfo.ScreenID == "AP.30.10.00")
            {
                if (row != null)
                    if (!(this.Base1.ATPTEmployeeExpense?.Any() ?? false))
                        this.Base1.ATPTEmployeeExpense.AllowSelect = false;
            }
        }

    }
}
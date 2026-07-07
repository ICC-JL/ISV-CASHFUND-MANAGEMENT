using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Helper;

namespace CashFundManagement.BLC
{
    public class ATPTEFMReqClassMaint : PXGraph<ATPTEFMReqClassMaint, ATPTEFMReqClass>, PXImportAttribute.IPXPrepareItems
    {
        public PXSelect<ATPTEFMReqClass, Where<ATPTEFMReqClass.tranType, Equal<Optional<ATPTEFMReqClass.tranType>>>,
            OrderBy<Asc<ATPTEFMReqClass.reqClassID>>> ReqClasses;

        public PXSelect<ATPTEFMReqClass, Where<ATPTEFMReqClass.tranType, Equal<Current<ATPTEFMReqClass.tranType>>,
            And<ATPTEFMReqClass.reqClassID, Equal<Current<ATPTEFMReqClass.reqClassID>>>>> CurrentReqClass;
        [PXImport(typeof(ATPTEFMReqClass))]
        public PXSelect<ATPTEFMReqClassItems, Where<ATPTEFMReqClassItems.tranType, Equal<Current<ATPTEFMReqClass.tranType>>,
            And<ATPTEFMReqClassItems.reqClassID, Equal<Current<ATPTEFMReqClass.reqClassID>>>>> Items;



        #region Events
        protected virtual void _(Events.RowSelected<ATPTEFMReqClassItems> e)
        {
            ATPTEFMReqClassItems classItems = e.Row;

            if (classItems == null) return;

            if (classItems?.IsPerDiem == true)
            {
                PXUIFieldAttribute.SetEnabled<ATPTEFMReqClassItems.amount>(e.Cache, null, true);
                //if (classItems.Amount <= Decimal.Zero)
                //{
                //    Items.Cache.RaiseExceptionHandling<ATPTEFMReqClassItems.amount>(classItems, classItems.Amount, new PXSetPropertyException(Messages.ATPTEFMMessages.AmountMustBeGreaterThanZero, PXErrorLevel.Error));
                //}
            }
        }

        //protected virtual void ATPTEFMReqClass_TranType_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        //{
        //    ATPTEFMReqClass row = e.Row as ATPTEFMReqClass;
        //    if (row != null)
        //    {
        //        row.ReqClassID = null;

        //        ATPTEFMReqClass reqclass = PXSelect<ATPTEFMReqClass,
        //            Where<ATPTEFMReqClass.tranType, Equal<Required<ATPTEFMReqClass.tranType>>>,
        //            OrderBy<Asc<ATPTEFMReqClass.reqClassID>>
        //            >.SelectSingleBound(this, null, row.TranType);
        //        if (reqclass != null)
        //            row.ReqClassID = reqclass.ReqClassID;
        //        else
        //            row.ReqClassID = null;
        //    }
        //}
        //protected virtual void ATPTEFMReqClass_ReqClassID_FieldVerifying(PXCache cache, PXFieldVerifyingEventArgs e)
        //{
        //    ATPTEFMReqClass reqclass = PXSelect<ATPTEFMReqClass,
        //            Where<ATPTEFMReqClass.tranType, Equal<Required<ATPTEFMReqClass.tranType>>,
        //            And<ATPTEFMReqClass.reqClassID, Equal<Required<ATPTEFMReqClass.reqClassID>>>
        //            >>.Select(this, new object[] { cache.GetValue<ATPTEFMReqClass.tranType>(e.Row), e.NewValue });
        //    if (reqclass == null)
        //        e.NewValue = null;
        //}

        //protected virtual void ATPTEFMReqClass_ReqClassID_FieldDefaulting(PXCache cache, PXFieldDefaultingEventArgs e)
        //{
        //    ATPTEFMReqClass reqclass = PXSelect<ATPTEFMReqClass,
        //             Where<ATPTEFMReqClass.tranType, Equal<Required<ATPTEFMReqClass.tranType>>>,
        //             OrderBy<Asc<ATPTEFMReqClass.reqClassID>>
        //             >.SelectSingleBound(this, null, cache.GetValue<ATPTEFMReqClass.tranType>(e.Row));
        //    if (reqclass == null)
        //        e.NewValue = null;
        //    else
        //        e.NewValue = reqclass.ReqClassID;
        //}
        #endregion

        #region Methods

        public override void Persist()
        {
            ATPTEFMReqClassItems classItems = this.Items.Current;
            if (classItems != null)
            {
                if (classItems?.IsPerDiem == true)
                {
                    if (classItems.Amount <= Decimal.Zero)
                    {
                        Items.Cache.RaiseExceptionHandling<ATPTEFMReqClassItems.amount>(classItems, classItems.Amount, ATPTEFMHelper.GetPropertyException(classItems, Messages.ATPTEFMMessages.AmountMustBeGreaterThanZero, PXErrorLevel.Error));
                    }
                }
            }
            base.Persist();
        }
            private void DeleteItemList()
        {
            // Delete current records
            foreach (var prop in Items.Select())
            {
                Items.Delete(prop);
            }
        }

        #endregion

        #region Imports
        private bool _Deleted = false;
        public bool PrepareImportRow(string viewName, IDictionary keys, IDictionary values)
        {
            if (!_Deleted)
            {
                DeleteItemList();
                _Deleted = true;
            }
            return true;
        }

        public bool RowImporting(string viewName, object row)
        {
            return row == null;
        }

        public bool RowImported(string viewName, object row, object oldRow)
        {

            return oldRow == null;
        }

        public void PrepareItems(string viewName, IEnumerable items)
        {

        }
        #endregion


    }
}
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CashFundManagement.Attributes {
    public class ATPTEFMCARequestClassItemSelectorAttribute : PXCustomSelectorAttribute
    {
        private Type _RequestClass;
        private Type _CurrentTable;
        private Type _CARef;
        private Type _FTRef;
        private Type _IsReclassifyExpenseReceipt;
        public ATPTEFMCARequestClassItemSelectorAttribute(Type CurrentTable, Type RequestClass, Type CARef, Type FTRef, Type IsReclassifyExpenseReceipt)
           : base(typeof(InventoryItem.inventoryID))
        {
            _RequestClass = RequestClass;
            _CurrentTable = CurrentTable;
            _CARef = CARef;
            _FTRef = FTRef;
            _IsReclassifyExpenseReceipt = IsReclassifyExpenseReceipt;
            DescriptionField = typeof(InventoryItem.descr);
            SubstituteKey = typeof(InventoryItem.inventoryCD);
        }

        protected virtual IEnumerable GetRecords()
        {
            PXCache cache = this._Graph.Caches[_CurrentTable];

            IBqlTable currentRow = null;
            foreach (IBqlTable item in this._Graph.Caches.Currents)
            {
                if (item != null && (item.GetType().FullName == _CurrentTable.FullName))
                {
                    currentRow = item;
                    break;
                }
            }

            List<InventoryItem> receipts = new List<InventoryItem>();
            if (currentRow != null)
            {
                string RequestClass = (string)cache.GetValue(currentRow, _RequestClass.Name);
                string CARef = (string)cache.GetValue(currentRow, _CARef.Name);
                string FTRef = (string)cache.GetValue(currentRow, _FTRef.Name);
                bool? IsReclassifyExpenseReceipt = (bool?)cache.GetValue(currentRow, _IsReclassifyExpenseReceipt.Name);

                ATPTEFMReqClass reqclass = PXSelect<ATPTEFMReqClass, Where<ATPTEFMReqClass.reqClassID, Equal<Required<ATPTEFMReqClass.reqClassID>>>>.Select(this._Graph, RequestClass);
                ATPTEFMCARequestDetail reqReference = PXSelect<ATPTEFMCARequestDetail, Where<ATPTEFMCARequestDetail.cashAdvanceNbr, Equal<Required<ATPTEFMCARequestDetail.cashAdvanceNbr>>>>.Select(this._Graph, CARef);
                ATPTEFMFundTransactionDetail ftDetail = PXSelect<ATPTEFMFundTransactionDetail, Where<ATPTEFMFundTransactionDetail.fundTransactionRefNbr, Equal<Required<ATPTEFMFundTransactionDetail.fundTransactionRefNbr>>>>.Select(this._Graph, FTRef);

                // Reimbursement fund transactions carry no planned detail lines, so their receipts must never be filtered by the "allow manual receipts" setting. {RFS}
                ATPTEFMFundTransaction linkedFundTransaction = SelectFrom<ATPTEFMFundTransaction>.Where<ATPTEFMFundTransaction.refNbr.IsEqual<@P.AsString>>.View.Select(this._Graph, FTRef);
                bool isReimbursement = linkedFundTransaction != null && linkedFundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue;

                foreach (InventoryItem item in PXSelect<InventoryItem, Where<InventoryItem.itemType, Equal<INItemTypes.expenseItem>, And<InventoryItem.itemStatus, Equal<InventoryItemStatus.active>>>>.Select(this._Graph))
                {
                    if (Helper.ATPTEFMPrefetchSetup.IsCAAllowManualReceipts == false && reqReference != null)
                    {
                        ATPTEFMCARequestDetail items = PXSelect<ATPTEFMCARequestDetail, Where<ATPTEFMCARequestDetail.cashAdvanceNbr, Equal<Required<ATPTEFMCARequestDetail.cashAdvanceNbr>>,
                            And<ATPTEFMCARequestDetail.inventoryID, Equal<Required<ATPTEFMCARequestDetail.inventoryID>>>>>.Select(this._Graph, CARef, item.InventoryID);
                        if (items != null)
                        {
                            receipts.Add(item);
                        }
                    }
                    else if (Helper.ATPTEFMPrefetchSetup.IsFTAllowManualReceipts == false && (ftDetail != null) && (!IsReclassifyExpenseReceipt ?? false) && !isReimbursement)
                    {
                        ATPTEFMFundTransactionDetail items = PXSelect<ATPTEFMFundTransactionDetail, Where<ATPTEFMFundTransactionDetail.fundTransactionRefNbr, Equal<Required<ATPTEFMFundTransactionDetail.fundTransactionRefNbr>>,
                            And<ATPTEFMFundTransactionDetail.inventoryID, Equal<Required<ATPTEFMFundTransactionDetail.inventoryID>>>>>.Select(this._Graph, FTRef, item.InventoryID);
                        if (items != null)
                        {
                            receipts.Add(item);
                        }
                    }
                    else if (reqclass != null && (reqclass.RestrictItemList ?? false))
                    {
                        ATPTEFMReqClassItems items = PXSelect<ATPTEFMReqClassItems, Where<ATPTEFMReqClassItems.reqClassID, Equal<Required<ATPTEFMReqClassItems.reqClassID>>,
                            And<ATPTEFMReqClassItems.inventoryID, Equal<Required<ATPTEFMReqClassItems.inventoryID>>>>>.Select(this._Graph, RequestClass, item.InventoryID);
                        if (items != null)
                            receipts.Add(item);
                    }
                    else
                        receipts.Add(item);
                }
            }
            else
            {
                foreach (InventoryItem item in PXSelect<InventoryItem, Where<InventoryItem.itemType, Equal<INItemTypes.expenseItem>, And<InventoryItem.itemStatus, Equal<InventoryItemStatus.active>>>>.Select(this._Graph))
                {
                    receipts.Add(item);
                }
            }                        
            return receipts.GroupBy(r => r.InventoryID).Select(r => r.First());
        }
    }
}
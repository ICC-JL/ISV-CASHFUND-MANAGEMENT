using CashFundManagement.DAC.Setup;
using PX.Data;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CashFundManagement.Attributes {
    public class ATPTEFMReqClassAttribute : PXCustomSelectorAttribute
    {
        private Type _TranType;
        private Type _CurrentTable;
        public ATPTEFMReqClassAttribute(Type CurrentTable, Type TranType)
            : base(typeof(ATPTEFMReqClass.reqClassID),
                  typeof(ATPTEFMReqClass.tranType),
                  typeof(ATPTEFMReqClass.reqClassID),
                  typeof(ATPTEFMReqClass.descr))
        {
            _TranType = TranType;
            _CurrentTable = CurrentTable;
            DescriptionField = typeof(ATPTEFMReqClass.descr);
        }

        /// <remarks>
        /// 2025-11-04 : CFM EMPLOYEE REQUEST CLASS DID NOT APPEAR ON APPROVAL MAP CASEID: 014235  {JLG} <br/>
        /// </remarks>
        public IEnumerable GetRecords()
        {
            List<ATPTEFMReqClass> resultRows = new List<ATPTEFMReqClass>();

            PXCache currentCache = this._Graph.Caches[_CurrentTable];

            IBqlTable currentRow = null;
            foreach (IBqlTable item in this._Graph.Caches.Currents)
            {
                if (item != null && (item.GetType().FullName == _CurrentTable.FullName))
                {
                    currentRow = item;
                    break;
                }
            }

            if (currentRow == null)
            {
                foreach (ATPTEFMReqClass reqclass in PXSelectOrderBy<ATPTEFMReqClass,
                       OrderBy<Asc<ATPTEFMReqClass.reqClassID>>>.Select(this._Graph))
                {
                    if (reqclass.ReqClassID != null)
                        resultRows.Add(reqclass);
                }
                return resultRows;
            }

            string TranType = (string)currentCache.GetValue(currentRow, _TranType.Name);
            
            foreach (ATPTEFMReqClass reqclass in PXSelect<ATPTEFMReqClass, Where<ATPTEFMReqClass.tranType,
                Equal<Required<ATPTEFMReqClass.tranType>>>,
                OrderBy<Asc<ATPTEFMReqClass.reqClassID>>>.Select(this._Graph, TranType))
            {
                if (reqclass.ReqClassID != null)
                    resultRows.Add(reqclass);
            }
            return resultRows;
        }

    }
}
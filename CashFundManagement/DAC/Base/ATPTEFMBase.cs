using PX.Data;

namespace CashFundManagement.DAC.Base {
#if Version24R1 
    public class ATPTEFMBase : PXBqlTable { } 
#else
    public class ATPTEFMBase { }
#endif
}

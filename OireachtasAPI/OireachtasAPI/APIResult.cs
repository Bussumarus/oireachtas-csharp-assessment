using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OireachtasAPI
{
    

    [Serializable]
    public class Bills
    {
        public class Head
        {

            public BillCount counts;
            public DateRange dateRange;
            public string lang;
        }

        [Serializable]
        public class BillCount : Result
        {
            public int billCount;
        }
    }

    

    [Serializable]
    public class Bill
    {
        
        
    }

    
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OireachtasAPI
{
    public abstract class Result
    {
        public int resultCount;
    }

    [Serializable]
    public class DateRange
    {
        public DateTime start;
        public DateTime end;
    }   
}

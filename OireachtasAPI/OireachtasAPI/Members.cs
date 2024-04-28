using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OireachtasAPI
{
    [Serializable]
    public class Members
    {
        public class Head
        {
            public MemberCount counts;
            public DateRange dateRange;
            public string lang;
        }

        [Serializable]
        public class Member
        {
            public string memberCode;
            public string firstName;
            public string lastName;
            public string gender;
            public string dateOfDeath;
            public string fullName;
            public string pId;
            public string wikiTitle;
        }

        [Serializable]
        public class MemberCount : Result
        {
            public int memberCount;
        }

        public Head head;
        public IList<Member> results;
    }
}

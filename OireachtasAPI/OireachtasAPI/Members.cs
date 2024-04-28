using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OireachtasAPI
{
    public class Members
    {
        public MemberHead head { get; set; }
        public MemberDetails[] results { get; set; }
    }

    public class MemberHead : Head
    {
        public MemberCount counts { get; set; }
    }

    public class MemberCount : Count
    {
        public int memberCount { get; set; }
    }

    public class MemberDetails
    {
        public Member member { get; set; }
    }

    public class Member
    {
        public string memberCode { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string gender { get; set; }
        public object dateOfDeath { get; set; }
        public string fullName { get; set; }
        public string pId { get; set; }
        public object wikiTitle { get; set; }
        public string uri { get; set; }
        public MembershipDetails[] memberships { get; set; }
    }

    public class MembershipDetails
    {
        public Membership membership { get; set; }
    }

    public class Membership
    {
        public Represent[] represents { get; set; }
        public DateRange dateRange { get; set; }
        public object[] offices { get; set; }
        public Party[] parties { get; set; }
        public string uri { get; set; }
        public House house { get; set; }
    }

    public class RepresentDetails
    {
        public Represent represent { get; set; }
    }

    public class Represent
    {
        public string representCode { get; set; }
        public string showAs { get; set; }
        public string uri { get; set; }
        public string representType { get; set; }
    }

    public class PartyDetails
    {
        public Party party { get; set; }
    }

    public class Party
    {
        public string showAs { get; set; }
        public DateRange dateRange { get; set; }
        public string partyCode { get; set; }
        public string uri { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class UserTest
    {
        public UserTest()
        {
            CardSellTest = new HashSet<CardSellTest>();
        }

        public int UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public virtual ICollection<CardSellTest> CardSellTest { get; set; }
    }
}

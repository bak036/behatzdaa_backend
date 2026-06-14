using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class FriendBringsFriend
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string AddedClubMemberFirstName { get; set; }
        public string AddedClubMemberLastName { get; set; }
        public string AddedClubMemberId { get; set; }
        public string AddedClubMemberPhoneNumber { get; set; }
        public string AddingClubMemberFirstName { get; set; }
        public string AddingClubMemberLastName { get; set; }
        public string AddingClubMemberId { get; set; }
        public string AddingClubMemberPhoneNumber { get; set; }
    }
}

using AutoMapper.Execution;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.EF.Club
{
    public class AllowanceHistory
    {
        public long Id { get; set; }
        public DateTime? InsertDate { get; set; }
        public string MemberID { get; set; }
        public int PremiumType { get; set; }
        public bool ClubCreditCard { get; set; }
        public int IDFResponse { get; set; }
    }
}

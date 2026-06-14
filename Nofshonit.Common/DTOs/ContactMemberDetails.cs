﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class ContactMemberDetails
    {
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string partnerPhoneNumber { get; set; }
        public string partnerEmail { get; set; }
        public DateTime? PrivacyPolicyAcceptedDate { get; set; }

        public ContactMemberDetails(string phone, string mail, string partnerPhone = null, string partnerMail = null, DateTime? privacyPolicyAcceptedDate = null)
        {
            phoneNumber = phone;
            email = mail;
            partnerPhoneNumber = partnerPhone;
            partnerEmail = partnerMail;
            PrivacyPolicyAcceptedDate = privacyPolicyAcceptedDate;
        }
    }
}

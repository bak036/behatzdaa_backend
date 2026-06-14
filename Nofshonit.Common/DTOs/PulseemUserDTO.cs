using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class PulseemUserDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public int MyProperty { get; set; }

        public string Email { get; set; }

        public string MobilePhone { get; set; }

        public string PartnerEmail { get; set; }

        public int PulseemGroupID { get; set; }

        public string PulseemOrganizationName { get; set; }

        public DateTime? birthDate { get; set; }

        public PulseemUserDTO(UserDTO userDTO)
        {
            FirstName = userDTO.FirstName;
            LastName = userDTO.LastName;
            Phone = userDTO.PhoneNumber;
            Email = userDTO.Email;
            MobilePhone = userDTO.MobilePhone;
            PartnerEmail = userDTO.PartnerEmail;
            birthDate = userDTO.BirthDate;
        }
    }
}
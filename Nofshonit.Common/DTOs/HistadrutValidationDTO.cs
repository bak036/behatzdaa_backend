using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class HistadrutValidationDTO
    {
        public string Id { get; set; }

        public string Password { get; set; }

        public HistadrutValidationDTO(string id, string password)
        {
            Id = id;
            Password = password;
        }
    }
}

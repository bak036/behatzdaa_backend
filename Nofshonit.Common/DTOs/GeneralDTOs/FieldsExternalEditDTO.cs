using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.GeneralDTOs
{
    public class FieldsExternalEditDTO
    {
        public long CategoryNumber { get; set; }
        public bool IsHeaderChanged { get; set; }
        public bool IsDescriptionChanged { get; set; }
        public bool IsShortDescriptionChanged { get; set; }
    }
}

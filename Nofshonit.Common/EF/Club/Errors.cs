using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class Errors
    {
        public int ErrorId { get; set; }
        public string Text { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public int MessageId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Event
{
    public class ShellTicketHub<T>
    {
        public bool Status { get; set; }
        public int ErrorId { get; set; }
        public int JourneyId { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}

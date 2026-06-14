using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class FileUploadDescription
    {
        public int FileActionId { get; set; }
        public string FileActionDescription { get; set; }
        public string FileActionName { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
    }
}

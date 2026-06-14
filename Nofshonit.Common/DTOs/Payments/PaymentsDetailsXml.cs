using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Nofshonit.Common.DTOs.Payments
{
    public class PaymentsDetailsXml
    {
        [XmlAttributeAttribute()]
        public string source { get; set; }

        [XmlAttributeAttribute()]
        public string payments { get; set; }

        [XmlAttributeAttribute()]
        public string authNumber { get; set; }

        [XmlAttributeAttribute()]
        public string cardId { get; set; }

        [XmlAttributeAttribute()]
        public string tranId { get; set; }

        [XmlAttributeAttribute()]
        public string terminalNumber { get; set; }
    }
}

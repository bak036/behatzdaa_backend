using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Nofshonit.Common.DTOs
{
    public class DetailsXml
    {
        [XmlAttributeAttribute()]
        public string MemberId { get; set; }

        [XmlAttributeAttribute()]
        public string PhoneNumber { get; set; }

        [XmlAttributeAttribute()]
        public string FirstName { get; set; }

        [XmlAttributeAttribute()]
        public string LastName { get; set; }

        public DetailsParameters Parameters { get; set; }

        [XmlTypeAttribute(AnonymousType = true)]
        public partial class DetailsParameters
        {
            [XmlAttributeAttribute()]
            public string param1 { get; set; }

            [XmlAttributeAttribute()]
            public string param2 { get; set; }

            [XmlAttributeAttribute()]
            public string param3 { get; set; }

            [XmlAttributeAttribute()]
            public string param4 { get; set; }

            [XmlAttributeAttribute()]
            public string param5 { get; set; }

            [XmlAttributeAttribute()]
            public string param6 { get; set; }

            [XmlAttributeAttribute()]
            public string param7 { get; set; }

            [XmlAttributeAttribute()]
            public string param8 { get; set; }

            [XmlAttributeAttribute()]
            public string param9 { get; set; }

            [XmlAttributeAttribute()]
            public string param10 { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Nofshonit.BL.Utils
{
    public static class XmlGenerator
    {
        public static string ObjectToXml<T>(T data)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stringwriter, data);
            return stringwriter.ToString();
        }

        public static T XmlToObject<T>(string xmlString)
        {
            var stringReader = new System.IO.StringReader(xmlString);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stringReader);
        }
    }
}

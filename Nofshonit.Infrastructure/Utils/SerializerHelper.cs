using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Nofshonit.Infrastructure.Utils
{
    public static class SerializerHelper
    {
        public static string JsonObjectToString<T>(this T toSerialize)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(toSerialize);
            }
            catch (Exception)
            {
                 return "";
            }
           
        }

        public static string XmlObjectToString<T>(this T toSerialize)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(toSerialize.GetType());
                using (var textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, toSerialize);
                    return textWriter.ToString();
                }
            }
            catch (Exception)
            {              
                return "";
            }
           
        }

        public static List<string> HtmlToString(string toSerialize)
        {
            try
            {
                var innerTexts = new List<string>();
                var document = new HtmlDocument();
                document.LoadHtml(toSerialize);

                foreach(var node in document.DocumentNode.DescendantsAndSelf())
                {
                    if (node.NodeType == HtmlNodeType.Text)
                    {
                        if (node.InnerText.Trim() != "")
                        {
                            innerTexts.Add(node.InnerText.Trim());
                        }
                    }
                }
                return innerTexts;
            }
            catch (Exception)
            {
                return new List<string>();
            }

        }
    }
}

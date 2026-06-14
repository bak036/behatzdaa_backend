using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

    public class Xmlbuilder
    {           
        public static string GetBalanceXmlString(int CampaignBalance, int BenefitGeneralBalance, int BenefitPersonalBalance)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("DtsResult");
            xmlDoc.AppendChild(root);
            AppendElement(xmlDoc, root, "resultCode", "0");
            AppendElement(xmlDoc, root, "resultMessage", "OK");

            AppendElement(xmlDoc, root, "CampaignBalance", CampaignBalance.ToString());
            AppendElement(xmlDoc, root, "BenefitGeneralBalance", BenefitGeneralBalance.ToString());
            AppendElement(xmlDoc, root, "BenefitPersonalBalance", BenefitPersonalBalance.ToString());

            return xmlDoc.InnerXml;
        }

        public static string GetFailureXmlString(string resultCode, string resultMessage )
        {

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("DtsResult");
            xmlDoc.AppendChild(root);
            AppendElement(xmlDoc, root, "resultCode", resultCode);
            AppendElement(xmlDoc, root, "resultMessage", resultMessage);

            return xmlDoc.InnerXml;
        }

        private static void AppendElement(XmlDocument xmlDoc, XmlElement Father, string elementName, string elementValue)
        {
            XmlElement element = xmlDoc.CreateElement(elementName);
            element.InnerText = elementValue;
            Father.AppendChild(element);
        }
    }

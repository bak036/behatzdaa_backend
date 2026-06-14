using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using OrderDll.TeleclalPayments;
using OrderDll.TelemesserSms;
using System.Configuration;

namespace OrderDll
{
    public class LC_SummerMethods
    {
        public static void SendMail(string message, string subject)
        {
            SqlMethods.SendMail(message, subject);
        }

        public static string CheckMember(string memberID)
        {
            
            try
            {
                if (!string.IsNullOrEmpty(memberID))
                    memberID = memberID.Trim().PadLeft(9, '0');

                DataTable dtSumOrders = new DataTable("Item");
                //XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.LoadXml("<?xml version='1.0' encoding='windows-1255'?><Transaction><MemberID>" + memberID + "</MemberID><Error/></Transaction>");
                StringBuilder sb = new StringBuilder("<?xml version='1.0' encoding='windows-1255'?><Transaction><MemberID>" + memberID + "</MemberID><Error/>");
                StringWriter sw = new StringWriter();
                XmlTextWriter xtw = new XmlTextWriter(sw);
                //XmlElement newElem = xmlDoc.CreateElement("Items");

                //Debit lcPayments = new Debit();
                //string lcAns = lcPayments.DebitCardFull("dtsivr", "leumi2008ivr", cardNumber, cvv, validUntil, 0, 0, 0, 0, 2, 1, 0, memberID, phone, true);

                //if (lcAns != "0")
                //{
                //    sb.Append("<Error>" + lcAns + "<Error>");
                //    //xmlDoc.GetElementsByTagName("Transaction")[0].AppendChild(newElem.SelectSingleNode("Items"));
                //}
                //else
                //    sb.Append("<Error/>");

                //DataBase.OpenConnection();
                SqlMethods.SumOrdersForMember_LC(memberID, ref dtSumOrders);
                
                if (dtSumOrders != null && dtSumOrders.Rows.Count > 0)
                {
                    dtSumOrders.WriteXml(xtw, true);
                    sb.Append(sw.ToString().Replace("DocumentElement", "Items"));
                    //newElem.InnerXml = sw.ToString().Replace("DocumentElement", "Items");
                    //xmlDoc.GetElementsByTagName("Transaction")[0].AppendChild(newElem.SelectSingleNode("Items"));
                }
                else
                {
                    sb.Append("<Items/>");
                    //xmlDoc.GetElementsByTagName("Transaction")[0].AppendChild(newElem);
                }
                //if (xmlDoc != null)
                //    return xmlDoc.InnerXml;
                sb.Append("</Transaction>");
                return sb.ToString();
            }
            catch (Exception ex)
            {

                SendMail("Function -> CheckMemberBalance OnDLL \n memberID -> " + memberID
                + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message
                + "\n ex.Source -> " + ex.Source + "\n ex.StackTrace -> " + ex.StackTrace, "LC_SummerIVR");
            }
            //finally
            //{
            //    DataBase.CloseConnection();
            //}

            return "<?xml version='1.0' encoding='windows-1255'?><Transaction><MemberID>" + memberID + "</MemberID><Error>19<Error><Items/></Transaction>";
        }

        public static string MakeOrder(int? PremiumType, string memberID, string productID, string quantity, string cardNumber,
            string DialPhone, string ValidUntil, string CVV, string SMSPhoneNumber)
        {
            try
            {
                //int sumOrders = 0;//CheckMember(memberID);
                //if (sumOrders >= 2)
                //    return "";//לא ניתן לבצע שינוי/הזמנה

                //int quant = 0;
                //if (!int.TryParse(quantity, out quant))
                //    return "";//שגיאה, סך כמות להזמנה לא בפורמט מספרי

                //if ((quant + sumOrders) >= 2)
                //    return "";//הזמנה גדולה מהכמות המותרת

                if (!string.IsNullOrEmpty(quantity))
                    quantity = quantity.Trim();

                if (!string.IsNullOrEmpty(cardNumber))
                    cardNumber = cardNumber.Trim();

                if (!string.IsNullOrEmpty(productID))
                    productID = productID.Trim();

                if (!string.IsNullOrEmpty(memberID))
                    memberID = memberID.Trim().PadLeft(9, '0');

                string xmlReturn = "<?xml version='1.0' encoding='windows-1255'?><Transaction>";
                int quant = 0;

                //memberID = memberID.Trim().PadLeft(9, '0');
                //productID = productID.Trim();

                //DataBase.OpenConnection();

                if (!string.IsNullOrEmpty(quantity) && !string.IsNullOrEmpty(cardNumber))
                {
                    quant = int.Parse(quantity);
                }
                else
                {
                    quant = SqlMethods.GetQuantityForProductID(productID, memberID);
                    if (quant == 0)
                    {
                        xmlReturn += "<Error>98</Error><Answer/><Coupon/></Transaction>";
                        return xmlReturn;
                    }
                    else
                        quant = 0 - quant;
                }


                if (quant < 0 && string.IsNullOrEmpty(cardNumber))
                {
                    cardNumber = SqlMethods.GetCardNumberForMemberIDLC(memberID, productID);
                    if (string.IsNullOrEmpty(cardNumber))
                    {
                        xmlReturn += "<Error>19</Error><Answer/><Coupon/></Transaction>";
                        return xmlReturn;
                    }
                }
                // get user order history
                int TotalOrdered = SqlMethods.TotalOrderedForMember_LC(memberID);

                if (TotalOrdered + quant > 2) // user no allow to order 
                {
                    xmlReturn += "<Error>99</Error><Answer/><Coupon/></Transaction>";
                    return xmlReturn;
                }
                else
                {
                    //string[] xmlParams = new string[8];
                    //metaData = "<?xml version='1.0' encoding='windows-1255'?><Details><DialPhone>03-9640875</DialPhone><ValidUntil>12/10</ValidUntil><CVV>875</CVV><Amount>123.00</Amount><SMSPhoneNumber>054-7074056</SMSPhoneNumber></Details>";


                    //            if (!string.IsNullOrEmpty(metaData))
                    //            {
                    //                try
                    //                {
                    //                    XmlDocument xmlDoc = new XmlDocument();
                    //                    xmlDoc.LoadXml(metaData);

                    //                    if (xmlDoc != null)
                    //                    {
                    //                        XmlNode node = null;

                    //                        string[] xmlItems = new string[] { "DialPhone", "CC", "ValidUntil", 
                    //                            "CVV", "Phone","Amount","SMSPhoneNumber", "AttNumber"};

                    //                        for (int i = 0; i < xmlParams.Length; i++)
                    //                        {
                    //                            switch (i)
                    //                            {
                    //                                case 1:
                    //                                    xmlParams[i] = cardNumber;
                    //                                    break;
                    //                                case 7:
                    //                                    xmlParams[i] = productID;
                    //                                    break;
                    //                                default:
                    //                                    node = xmlDoc.SelectSingleNode("Details/" + xmlItems[i]);
                    //                                    if (node != null)
                    //                                        xmlParams[i] = node.InnerText;
                    //                                    break;
                    //                            }
                    //                        }
                    //                        xmlParams[4] = xmlParams[0];//Phone = DialPhone

                    //                    }
                    //                    metaData = string.Format(
                    //                        @"<Source>IVR</Source>
                    //                                    <DialPhone>{0}</DialPhone>
                    //                                    <CC>{1}</CC>
                    //                                    <ValidUntil>{2}</ValidUntil>
                    //                                    <CVV>{3}</CVV>
                    //                                    <Phone>{4}</Phone>
                    //                                    <Amount>{5}</Amount>
                    //                                    <SlikaSapakDebit>0</SlikaSapakDebit>
                    //                                    <GC></GC>
                    //                                    <SMSPhoneNumber>{6}</SMSPhoneNumber>
                    //                                    <AttNumber>{7}</AttNumber>", xmlParams);
                    //                    metaData = metaData.Replace("'", "''");
                    //                }
                    //                catch (Exception ex)
                    //                {

                    //                }
                    //            }

                    string xmlCoupon = "";
                    string metaData = string.Format(
                                @"<Source>IVR</Source><DialPhone>{0}</DialPhone><CC>{1}</CC><ValidUntil>{2}</ValidUntil><CVV>{3}</CVV><Phone>{4}</Phone><Amount>{5}</Amount><SlikaSapakDebit>0</SlikaSapakDebit><GC></GC><SMSPhoneNumber>{6}</SMSPhoneNumber><AttNumber>{7}</AttNumber>",
                                DialPhone, cardNumber, ValidUntil, CVV, SMSPhoneNumber, "0", SMSPhoneNumber, productID);

                    metaData = metaData.Replace("'", "''");


                    string lcAns = "0";
                    if (quant > 0)
                    {
                        Debit lcPayments = new Debit();
                        lcAns = lcPayments.DebitCardFull("dtsivr", "leumi2008ivr", cardNumber, CVV, ValidUntil, 1, 0, 0, 0, 2, 1, 0, memberID, DialPhone, true);
                    }

                    if (lcAns != "0")
                    {
                        xmlReturn += "<Error>" + lcAns + "</Error><Answer/><Coupon/></Transaction>";
                        return xmlReturn;
                        //xmlDoc.GetElementsByTagName("Transaction")[0].AppendChild(newElem.SelectSingleNode("Items"));
                    }
                    else
                        xmlReturn += "<Error/>";

                    xmlReturn += "<Answer>";

                    string ans = Main.InsertNewOrderLeumiCard_GCPoints(PremiumType, productID, memberID, quant.ToString(), metaData, cardNumber);

                    if (quant > 0)
                    {
                        string atractionName = GetAtractionName(productID);
                        SendSmsWS wsSendSms = new SendSmsWS();
                        Result r = new Result();
                        string smsText = "";
                        if (productID == "1600110-1" || productID == "1001046-1")//If Success & CinemaCity OR Dominos
                        {
                            string terminal;// = "1001046";
                            if (productID == "1600110-1")
                                terminal = "1288789";
                            else
                                terminal = "1001046";

                            xmlCoupon += "<Coupon>";
                            string[] coupons = new string[quant];

                            for (int i = 0; i < quant; i++)
                            {
                                if (i > 0)
                                    xmlCoupon += "|";
                                if (productID == "1600110-1")//Cinema City Code
                                {
                                    coupons[i] = SqlMethods.GetCinemaCityCoupon(memberID, cardNumber, SMSPhoneNumber);
                                    xmlCoupon += coupons[i];
                                }
                                else//Dominos Code
                                    xmlCoupon += coupons[i] = "2010";

                                if (!string.IsNullOrEmpty(SMSPhoneNumber))
                                {
                                    try
                                    {
                                        smsText = "הזמנתך " + ans + " בוצעה בהצלחה. קוד הקופון ל" + atractionName + " הינו " + coupons[i];
                                        r = wsSendSms.SendSms("Dts123", "2034af1aeefb477283b806582d998132", SMSPhoneNumber, System.Web.HttpUtility.HtmlEncode(smsText), memberID, 0, 120);//, ref b, ref c, null, null, null);
                                    }
                                    catch (Exception ex)
                                    {
                                        SendMail("We will not send SMS memberID " + memberID + " SMSPhoneNumber " + SMSPhoneNumber + " productID " + productID + " quantity " + quantity + " smsText " + smsText + " ex.Message " + ex.Message, "LC SMS NOT SEND");
                                    }
                                }

                            }

                            xmlCoupon += "</Coupon>";

                            SqlMethods.DoExercise(memberID, productID, terminal);

                        }
                        else if (!string.IsNullOrEmpty(SMSPhoneNumber))//הזמנה רגילה
                        {
                            smsText = "הזמנתך " + ans + " בוצעה בהצלחה. לרשותך " + quantity + " כרטיסים ל" + atractionName;
                            try
                            {
                                r = wsSendSms.SendSms("Dts123", "2034af1aeefb477283b806582d998132", SMSPhoneNumber, System.Web.HttpUtility.HtmlEncode(smsText), memberID, 0, 120);//, ref b, ref c, null, null, null);
                            }
                            catch (Exception ex)
                            {
                                SendMail("We will not send SMS memberID " + memberID + " SMSPhoneNumber " + SMSPhoneNumber + " productID " + productID + " quantity " + quantity + " smsText " + smsText + " ex.Message " + ex.Message, "LC SMS NOT SEND");
                            }

                        }
                        if (!string.IsNullOrEmpty(SMSPhoneNumber) && r.result != "OK")
                            SendMail("We will not send SMS memberID " + memberID + " SMSPhoneNumber " + SMSPhoneNumber + " productID " + productID + " quantity " + quantity + " smsText " + smsText + " ResultFromTeleclal " +  r.result, "LC SMS NOT SEND");

                    }

                    //Add WS Answer Value
                    xmlReturn += ans + "</Answer>";

                    //Add Coupon Value
                    if (string.IsNullOrEmpty(xmlCoupon))
                        xmlReturn += "<Coupon/>";
                    else
                        xmlReturn += xmlCoupon;

                    return xmlReturn += "</Transaction>";
                }
            }
            catch (Exception ex)
            {
                SqlMethods.SendMail("MakeOrder -> \n memberID -> " + memberID
                    + "\n productID -> " + productID
                    + "\n quantity -> " + quantity
                    + "\n cardNumber -> " + cardNumber
                    + "\n DialPhone -> " + DialPhone
                    + "\n ValidUntil -> " + ValidUntil
                    + "\n CVV -> " + CVV
                    + "\n SMSPhoneNumber -> " + SMSPhoneNumber
                    + "\n Exception TYPE -> " + ex.GetType() 
                    + "\n Exception Message -> " + ex.Message
                    + "\n ex.Source -> " + ex.Source 
                    + "\n ex.StackTrace -> " + ex.StackTrace, "LC_SummerIVR");

                return "<?xml version='1.0' encoding='windows-1255'?><Transaction><Error>19</Error><Answer/><Coupon/></Transaction>";//להחזיר שגיאה
            }
            //finally
            //{
            //    DataBase.CloseConnection();
            //}

        }

        private static int GetTotalOrdered(string orderHistoryXML)
        {
            int totalOrdersQuantity = 0;
            try
            {
                XmlDocument documnet = new XmlDocument();
                documnet.LoadXml(orderHistoryXML);


            }
            catch (Exception)
            {
                return -1;
            }
            return totalOrdersQuantity;
        }

        private static string GetAtractionName(string productID)
        {
//            1000002-2	צוק מנרה
//1000012-2	ספארק המים - ימית - אטרקציה
//1000016-1	פארק נחשונית
//1000019-2	קייקי כפר בלום
//1000041-2	קיפצובה
//1000043-2	מיני ישראל
//1000049-2	אגם חי - קיבוץ יראון
//1000051-1	אבוקייק
//1000069-2	פעלטון
//1000077-2	אייסקייט
//1000091-1	לונה גל
//1001046-1	דומינוס פיצה
//100114-2	בית חלומותי
//100142-2	סופרלנד
//100143-2	מימדיון
//100144-2	לונה פארק
//100145-2	צפארי
//1600110-1	ניו סינמה סיטי
            switch (productID)
            {
                case "1000002-2":
                    return "צוק מנרה";
                case "1000012-2":
                    return "ספארק המים - ימית";
                case "1000016-1":
                    return "פארק נחשונית";
                case "1000019-2":
                    return "קייקי כפר בלום";
                case "1000041-2":
                    return "קיפצובה";
                case "1000043-2":
                    return "מיני ישראל";
                case "1000049-2":
                    return "אגם חי - קיבוץ יראון";
                case "1000051-1":
                    return "אבוקייק";
                case "1000069-2":
                    return "פעלטון";
                case "1000077-2":
                    return "אייסקייט";
                case "1000091-1":
                    return "לונה גל";
                case "1001046-1":
                    return "דומינוס פיצה";
                case "100114-2":
                    return "בית חלומותי";
                case "100142-2":
                    return "סופרלנד";
                case "100143-2":
                    return "מימדיון";
                case "100144-2":
                    return "לונה פארק";
                case "100145-2":
                    return "צפארי";
                case "1600110-1":
                    return "ניו סינמה סיטי";
            }
            return "";
        }

        public static string CheckMemberBalanceForCampaigns(string memberID, string quantity, ref string dbName)
        {
            

            //if (string.IsNullOrEmpty(cardNumber) ||
            //    cardNumber.Length != 16 ||
            //    !BaseMethods.IsDigit(cardNumber))
            //    return "9";
            //if (cardNumber == "4580100000000000")
            //    return "601,659";
            //if (cardNumber == "4580100000000001")
            //    return "707";
            //if (cardNumber.Length == 16 && cardNumber.StartsWith("4580"))
            //    return "0";
            //return "19";
            int intQuantity = 0;
            if (string.IsNullOrEmpty(quantity) ||
                !int.TryParse(quantity, out intQuantity))
                return "5";

            
            //    dbName = SqlMethods.GetDBNameByCardPrefix(cardNumber);
            //if (dbName == "32")
            //    return "9";

            //we need to add check short/full card from NETS, the CardPrefix return wrong db
            dbName = "BankLeumi";


            //Get MemberID
            //bool encryptionRequired = SqlMethods.IsEncoded(dbName);
            
            //if (encryptionRequired)
            //    memberID = SqlMethods.GetMemberIDByCardNumberEncrypted(dbName, cardNumber);
            //else 
            //    memberID = SqlMethods.GetMemberIDByCardNumber(dbName, cardNumber);
            //if (memberID == "2")//Unknown card
            //    return "0";
            //else
                memberID = memberID.Trim();

            //Get Campaigns
            DataTable dt = new DataTable();
            SqlMethods.GetPaidCampaigns(dbName, memberID, ref dt);
            if (dt == null || 
                dt.Rows.Count == 0)
                return "0";//No campaigns connected to card 

            CheckLimitsForCampaign(dbName, intQuantity, ref dt);
            CheckLimitsForMemberToCampaign(dbName, intQuantity, ref dt, memberID);

            DataRow[] rows = dt.Select("status = 'True'");
            if (rows.Length < 1)
                return "3";//There is limit to campaign

            string camp = "";
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToBoolean(row["Status"]))
                    camp += row["CampaignID"].ToString() + ",";
            }
            if (!string.IsNullOrEmpty(camp))
            {
                camp = camp.Remove(camp.LastIndexOf(','));
                return camp;
            }
            return "0";
        }

        private static void CheckLimitsForCampaign(string dbName, int quantity, ref DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                Int64 sumOfUse = SqlMethods.GetSumUseCampaign(dbName, row["CampaignID"].ToString(), row["BenefitID"].ToString(), "");
                Int64 campaignLimit = Convert.ToInt64(row["CampaignLimit"]);
                if (campaignLimit < sumOfUse + quantity)
                    row["Status"] = false;
            }
        }

        private static void CheckLimitsForMemberToCampaign(string dbName, int quantity, ref DataTable dt, string memberID)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToBoolean(row["Status"]))
                {
                    Int64 sumOfUse = SqlMethods.GetSumUseCampaignForMember(dbName, row["CampaignID"].ToString(), row["BenefitID"].ToString(), memberID);
                    Int64 benefitPersonLimit = Convert.ToInt64(row["BenefitPersonLimit"]);
                    if (benefitPersonLimit < sumOfUse + quantity)
                        row["Status"] = false;
                }
            }
        }

        public static string ImplementationCampaign(string memberID, string cardNumber, string campaignID, string quantity, string DialPhone, string ValidUntil, string smsPhone, string cvv)
        {
            string checkValues = CheckParams(ref memberID, ref cardNumber, ref campaignID, ref quantity, ref DialPhone, ref ValidUntil, ref smsPhone, ref cvv);
            if (!string.IsNullOrEmpty(checkValues))
                return "5";//Codes For Empty Params
            
            string dbName = "";
            string camp = CheckMemberBalanceForCampaigns(memberID, quantity, ref dbName);
            if (!camp.Contains(campaignID))
                return camp;//There is no Campaign

            //string tempMemberID = null;
            //bool encryptionRequired = SqlMethods.IsEncoded(dbName);
            //if (encryptionRequired)
            //    tempMemberID = SqlMethods.GetMemberIDByCardNumberEncrypted(dbName, cardNumber);
            //else
            //    tempMemberID = SqlMethods.GetMemberIDByCardNumber(dbName, cardNumber);
            //if (tempMemberID != memberID)
            //    return "4";

            //int orgID = SqlMethods.GetOrgIDByCardPrefix(cardNumber);
            //if (orgID == 0)
            //    return "9";

            //we need to add check short/full card from NETS, the CardPrefix return wrong db
            int orgID = 28;

            Int64 benefitID = 0;
            Int64 groupID = 0;
            Int64 merchantID = 0;
            float money = 0;

            SqlMethods.GetPropToInsertCampaignUse(ref benefitID, ref groupID, ref merchantID, ref money, campaignID, orgID);//Must be one benefit to campaign

            if (benefitID == 0 ||
                groupID == 0 ||
                merchantID == 0)
                return "19";//Can't find Campaign Prop

            int rows = SqlMethods.InsertCampaignUse(campaignID, benefitID, groupID, merchantID, memberID, cardNumber, quantity, dbName);
            if (rows < 1)
                return "19";           
            

            bool takeCache = true;
            bool.TryParse(ConfigurationSettings.AppSettings["TakeCache"], out takeCache);
            
            //string teleclalAns = "0";
            string chargeResultString = "", confirmationNumber;
            bool chargeResult = false;
            //Take Money
            if (takeCache && money != 0)
            {
               //CreditCard
                chargeResult = Main.ChargeCreditCard(out chargeResultString, out confirmationNumber, orgID.ToString(), money.ToString(), 1, cardNumber, cvv, memberID, ValidUntil, "Debit");
             }

            string returnVal = "1";

            if (!chargeResult)
            {
                //IF Can't Take Money
                int deleteRows = SqlMethods.DeleteCampaignUse(campaignID, benefitID, groupID, merchantID, memberID, cardNumber, quantity, dbName, "<Teleclal>" + chargeResultString + "</Teleclal>");
                if (deleteRows < 1)
                {
                    SqlMethods.SendMail("Can't Delete Campaign Use"
                            + "\n campaignID -> " + campaignID
                            + "\n benefitID -> " + benefitID
                            + "\n groupID -> " + groupID
                            + "\n money -> " + money
                            + "\n cardNumber -> " + cardNumber
                            + "\n merchantID -> " + merchantID
                            + "\n quantity -> " + quantity
                            + "\n dbName -> " + dbName
                            + "\n deleteRows -> " + deleteRows, "IVR Leumi");
                }
                returnVal = "2";
            }

            //if (encryptionRequired)
            //    SqlMethods.EncryptCardNumber(dbName, cardNumber, "CampaignMembersUse");

            return returnVal;
        }



        private static string CheckParams(ref string memberID, ref string cardNumber, ref string campaignID,
            ref string quantity, ref string DialPhone, ref string ValidUntil, ref string smsPhone, ref string cvv)
        {
            if (!string.IsNullOrEmpty(memberID) &&
                BaseMethods.IsDigit(memberID.Trim()))
                memberID = memberID.Trim().PadLeft(9, '0');
            else
                return "1";

            if (!string.IsNullOrEmpty(cardNumber) &&
                BaseMethods.IsDigit(cardNumber.Trim()))
                cardNumber = cardNumber.Trim();
            else
                return "2";

            if (!string.IsNullOrEmpty(campaignID) &&
                BaseMethods.IsDigit(campaignID.Trim()))
                campaignID = campaignID.Trim();
            else
                return "3";

            if (!string.IsNullOrEmpty(quantity) &&
                BaseMethods.IsDigit(quantity.Trim()))
                quantity = quantity.Trim();
            else
                return "4";

            if (!string.IsNullOrEmpty(DialPhone) &&
                BaseMethods.IsDigit(DialPhone.Trim()))
                DialPhone = DialPhone.Trim();
            else
                return "5";

            if (!string.IsNullOrEmpty(ValidUntil))
                ValidUntil = ValidUntil.Trim();
            else
                return "6";

            if (!string.IsNullOrEmpty(smsPhone) &&
                BaseMethods.IsDigit(smsPhone.Trim()))
                smsPhone = smsPhone.Trim();
            else
                return "7";
            if (!string.IsNullOrEmpty(cvv) &&
                BaseMethods.IsDigit(cvv.Trim()))
                cvv = cvv.Trim();
            else
                return "8";
            return null;
        }

        //public static void SendSMS()
        //{
        //    BaseMethods.SendSMS("0542266997", "באזזר 5707811-5085 ארוחת ראנץ M+ פאי מתנה עד 31/12, לתמיכה 035114158", "035114158", 0, 120);
        //}
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Web;
using OrderDll.TelemesserSms;
using System.Threading;
using MSXML2;
using System.Xml;

/// <summary>
/// Summary description for BaseMethods
/// </summary>
public class BaseMethods
{
	public BaseMethods()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    //TODO: Bild CheckIfRealID
    public static bool CheckIfRealID(string memberID)
    {
        // CHECK THE ID NUMBER
        int mone = 0;
        int incNum;
        for (int i = 0; i < 9; i++)
        {
            incNum = Convert.ToInt32(memberID[i].ToString());
            incNum *= (i % 2) + 1;
            if (incNum > 9)
                incNum -= 9;
            mone += incNum;
        }
        if (mone % 10 == 0)
            return true;
        return false;
    }

    public static bool IsDigit(string valueToCheck)
    {
        if (string.IsNullOrEmpty(valueToCheck))
            return false;
        
        //Check The Template
        valueToCheck = valueToCheck.Trim();
        char[] arr = valueToCheck.ToCharArray();
        foreach (char c in arr)
        {
            if (!char.IsDigit(c))
                return false;
        }
        return true;
    }

    public static bool IsDigit(string valueToCheck, int placeToJump)
    {
        if (string.IsNullOrEmpty(valueToCheck))
            return false;
        
        //Check The Template With '-'
        char[] arr = valueToCheck.ToCharArray();
        for (int i = 0; i < arr.Length; i++)
        {
            if (!char.IsDigit(arr[i]))
            {
                if (placeToJump != i)
                    return false;
            }
        }
        return true;
    }

    public static string AddZeroToID(string memberID)
    {
        return memberID.PadLeft(9, '0');
    }

    public static bool SendSMS(string phoneNumber, string smsText, string senderName, byte DeliveryDelayInMinutes, byte ExpirationDelayInMinutes, string OrganizationID, string memberID, long CouponID, byte SmsType)
    {
        int orgID = 0;
        int.TryParse(OrganizationID, out orgID);
        long SMSQueueID = SMSQueue.AddToSmsQueue(orgID, memberID, CouponID, senderName, phoneNumber, smsText, DeliveryDelayInMinutes, ExpirationDelayInMinutes, SmsType, 100);
        if (SMSQueueID > 0)
            return true;
        else
        {
            SqlMethods.SendMail("Function -> SendSMS"
                            + "\n phoneNumber -> " + phoneNumber
                            + "\n smsText -> " + smsText
                            + "\n senderName -> " + senderName
                            + "\n DeliveryDelayInMinutes -> " + DeliveryDelayInMinutes
                            + "\n ExpirationDelayInMinutes -> " + ExpirationDelayInMinutes
                            + "\n SMSQueueID -> " + SMSQueueID , "SendSMS not enter to SMSQueue table");
            return false;
        }
        //    try
        //    {
        //        r = wsSendSms.SendSms(userName, password, phoneNumber, System.Web.HttpUtility.HtmlEncode(smsText), senderName, DeliveryDelayInMinutes, ExpirationDelayInMinutes);//, ref b, ref c, null, null, null);
        //        if (r.result == "OK")
        //            return r.result;
        //        else
        //            Thread.Sleep(1500);
        //    }
        //    catch (Exception ex)
        //    {
        //        int i;
        //        for (i = 0; i < 5; i++)
        //        {
        //            try
        //            {
        //                r = wsSendSms.SendSms(userName, password, phoneNumber, System.Web.HttpUtility.HtmlEncode(smsText), senderName, DeliveryDelayInMinutes, ExpirationDelayInMinutes);//, ref b, ref c, null, null, null);
        //                if (r.result == "OK")
        //                    return r.result;
        //            }
        //            catch (Exception)
        //            {
        //                Thread.Sleep(3000);
        //            }
        //        }
        //        if (i == 4)
        //        {
        //            SqlMethods.SendMail("Function -> SendSMS"
        //                            + "\n phoneNumber -> " + phoneNumber
        //                            + "\n smsText -> " + smsText
        //                            + "\n senderName -> " + senderName
        //                            + "\n DeliveryDelayInMinutes -> " + DeliveryDelayInMinutes
        //                            + "\n ExpirationDelayInMinutes -> " + ExpirationDelayInMinutes
        //                            + "\n ResultFromTeleclal -> " + r.result
        //                            + "\n Exception TYPE -> " + ex.GetType()
        //                            + "\n Exception Message -> " + ex.Message
        //                            + "\n ex.Source -> " + ex.Source
        //                            + "\n ex.StackTrace -> " + ex.StackTrace, "Leumi Campaign");
        //        }
        //    }

        //return r.result;
    }

    //public static string SendSMS(string phoneNumber, string smsText, string senderName, int DeliveryDelayInMinutes, int ExpirationDelayInMinutes)
    //{
    //    SendSmsWS wsSendSms = new SendSmsWS();
    //    Result r = new Result();
    //    string userName = "Dts123";
    //    string password = "2034af1aeefb477283b806582d998132";
    //    for (int z = 0; z < 5; z++)
    //    {
    //        try
    //        {
    //            r = wsSendSms.SendSms(userName, password, phoneNumber, System.Web.HttpUtility.HtmlEncode(smsText), senderName, DeliveryDelayInMinutes, ExpirationDelayInMinutes);//, ref b, ref c, null, null, null);
    //            if (r.result == "OK")
    //                return r.result;
    //            else
    //                Thread.Sleep(1500);
    //        }
    //        catch (Exception ex)
    //        {
    //            int i;
    //            for (i = 0; i < 5; i++)
    //            {
    //                try
    //                {
    //                    r = wsSendSms.SendSms(userName, password, phoneNumber, System.Web.HttpUtility.HtmlEncode(smsText), senderName, DeliveryDelayInMinutes, ExpirationDelayInMinutes);//, ref b, ref c, null, null, null);
    //                    if (r.result == "OK")
    //                        return r.result;
    //                }
    //                catch (Exception)
    //                {
    //                    Thread.Sleep(3000);
    //                }
    //            }
    //            if (i == 4)
    //            {
    //                SqlMethods.SendMail("Function -> SendSMS"
    //                                + "\n phoneNumber -> " + phoneNumber
    //                                + "\n smsText -> " + smsText
    //                                + "\n senderName -> " + senderName
    //                                + "\n DeliveryDelayInMinutes -> " + DeliveryDelayInMinutes
    //                                + "\n ExpirationDelayInMinutes -> " + ExpirationDelayInMinutes
    //                                + "\n ResultFromTeleclal -> " + r.result
    //                                + "\n Exception TYPE -> " + ex.GetType()
    //                                + "\n Exception Message -> " + ex.Message
    //                                + "\n ex.Source -> " + ex.Source
    //                                + "\n ex.StackTrace -> " + ex.StackTrace, "Leumi Campaign");
    //            }
    //        }
    //    }
        
    //    return r.result;
    //}

    public static string GetNetIDForCard(string cardNumber, ref string dbName)
    {
        DataTable allNetsWithCardTamplate = new DataTable();
        SqlMethods.GetAllNetsWithCardTamplate(ref allNetsWithCardTamplate);
        if (allNetsWithCardTamplate != null && allNetsWithCardTamplate.Rows.Count > 0)
        {
            foreach (DataRow row in allNetsWithCardTamplate.Rows)
            {
                string CardExists = GetDbNameForShort(row, cardNumber);
                if (!string.IsNullOrEmpty(CardExists))
                {
                    if (row["NetDB"] != DBNull.Value)
                        dbName = row["NetDB"].ToString().Trim();
                    if (row["OrganizationID"] != DBNull.Value)
                        return row["OrganizationID"].ToString();
                }
            }
        }
        return null;
    }

    private static string GetDbNameForShort(DataRow row, string cardNumber)
    {
        string dbName = null;
        if ((row["IDCodeTemplate"].ToString().Length) == (cardNumber.Length))
        {
            bool thisDb = CheckCardToDb(cardNumber, row["IDCodeTemplate"].ToString());
            if (thisDb)
                dbName = row["NetDB"].ToString();
        }
        return dbName;
    }

    private static bool CheckCardToDb(string cardNumber, string cardTamplate)
    {
        int counter = 0;

        foreach (char c in cardTamplate)
        {
            if (char.IsLetterOrDigit(c) || cardNumber[counter] == '=')
            {
                if (cardNumber[counter] != c)
                    return false;
            }
            counter++;
        }
        return true;
    }

    public static string GetCheckCardNumberAnswer(string errorID, string orgID, string memberID)
    {
        string errMessage = SqlMethods.GetErrorMessage(errorID);
        string message = string.Format(
        @"<?xml version='1.0' encoding='windows-1255' ?> 
        <Checkcardnumber>
          <Error>{0}</Error>
          <Message>{1}</Message>
          <Orgid>{2}</Orgid> 
          <Memberid>{3}</Memberid> 
        </Checkcardnumber>", errorID, errMessage, orgID, memberID);
        return message;
    }

    public static string GetWSClubsAnswer(string errorID, string memberID)
    {
        return GetWSClubsAnswer(errorID, memberID, "");
    }

    public static string GetWSClubsAnswer(string errorID, string memberID, string addTextToMessage)
    {
        Int64 ans = 0;
        Int64.TryParse(errorID, out ans);
        string errMessage = "בוצעה הזמנה";
        if (ans < 100)
            errMessage = SqlMethods.GetErrorMessage(errorID);
        //for CreditCard message
        if (!string.IsNullOrEmpty(addTextToMessage))
        {
            if (Int64.TryParse(addTextToMessage, out ans))
                errMessage = addTextToMessage;
            else
                errMessage += " " + addTextToMessage;
        }
            
        string message = string.Format(
        @"<?xml version='1.0' encoding='windows-1255' ?> 
        <WSClubs>
          <Error>{0}</Error>
          <Message>{1}</Message>
          <Memberid>{2}</Memberid> 
        </WSClubs>", errorID, errMessage, memberID);
        return message;
    }

    public static string GetCheckLimitsAnswer(string errorID, string balance, string price)
    {
        return GetCheckLimitsAnswer(errorID, balance, price, null);
    }

    public static string GetCheckLimitsAnswer(string errorID, string balance, string price, string [] errors)
    {
        string errMessage = SqlMethods.GetErrorMessage(errorID);
        if (errors != null)
        {
            for (int i = 0; i < errors.Length; i++)
                errMessage = errMessage + " " + errors[i];
        }
        if (string.IsNullOrEmpty(balance))
            balance = "0";
        if (string.IsNullOrEmpty(price))
            price = "0.00";
        string message = string.Format(
        @"<?xml version='1.0' encoding='windows-1255' ?> 
        <Checklimits>
          <Error>{0}</Error>
          <Message>{1}</Message>
          <Balance>{2}</Balance>
          <Price>{3}</Price>
        </Checklimits>", errorID, errMessage, balance, price);
        return message;
    }

    public static string GetIVROrderOnLineAnswer(string errorID, string databaseName, string paymentID)
    {
        string errMessage = "";
        if (int.Parse(errorID) > 100)
        {
            Payments.UpdateOrderStatus(databaseName, paymentID);
            errMessage = "ההזמנה בוצעה בהצלחה";
        }
        else
            errMessage = SqlMethods.GetErrorMessage(errorID);
        string message = string.Format(
        @"<?xml version='1.0' encoding='windows-1255' ?> 
            <Ivrorderonline>
                <Error>{0}</Error> 
                <Message>{1}</Message> 
            </Ivrorderonline>", errorID, errMessage);
        return message;
    }

    /// <summary>
    /// Chrge credit card
    /// </summary>
    public void Charge()
    { }

    public static string AkaLogin(string memberID, string cardNumber, string databaseName)
    {
        int miluimPremiumType = -1, releasedPremiumType = -1;
        string resultXml = null, result = null;
        resultXml = AkaUserLogin(memberID, cardNumber, ref miluimPremiumType, ref releasedPremiumType);
        if (resultXml == null) // Thila service is not available 
        {
            result = SqlMethods.GetMessageTextByMessageID("8");//"השירות אינו זמין אנא נסה שוב מאוחר יותר";
            return result;
        }
        else if (miluimPremiumType != 2 && releasedPremiumType < 0) // user is not allowed to login
        {
            result = GetErrorMessage(miluimPremiumType, releasedPremiumType);
            return result;
        }
        UpdateMember(databaseName, memberID, miluimPremiumType, releasedPremiumType);
        return "1";
    }

    private static void GetCorrectPremiumType(string databaseName, ref int PremiumType, ref int AnotherPremiumType, int miluimPremiumType, int releasedPremiumType)
    {
        if (databaseName == "Miluim" || databaseName == "MiluimDevelopment" || databaseName == "MiluimTest")
        {
            PremiumType = miluimPremiumType;
            AnotherPremiumType = releasedPremiumType;
        }
        else if (databaseName == "HonorReleases" || databaseName == "HonorReleasesTest" || databaseName == "ReleasesDevelopment")
        {
            PremiumType = releasedPremiumType;
            AnotherPremiumType = miluimPremiumType;
        }
    }

    private static void UpdateMember(string databaseName, string personalNumber, int miluimPremiumType, int releasedPremiumType)
    {
        int PremiumType = 0;
        int AnotherPremiumType = 0;
        GetCorrectPremiumType(databaseName, ref PremiumType, ref AnotherPremiumType, miluimPremiumType, releasedPremiumType);

        string query = string.Format(@"Update {0}..AllMembers set PremiumType = '{1}', AnotherPremiumType = '{2}', LastUpdate = GetDate() 
                                where MemberId = '{3}'", databaseName, PremiumType, AnotherPremiumType, personalNumber);
        DataBase.ExecuteNonQuery(query);
    }

    private static string GetErrorMessage(int miluimPremiumType, int releasedPremiumType)
    {
        string result = null;
        if (miluimPremiumType == -1 && releasedPremiumType == -1)
            result = SqlMethods.GetMessageTextByMessageID("9");
        else if (miluimPremiumType == -2 && releasedPremiumType == -2)
            result = SqlMethods.GetMessageTextByMessageID("10");
        else if (miluimPremiumType == -3 && releasedPremiumType == -3) // IP blocked 
            result = SqlMethods.GetMessageTextByMessageID("11");
        else if (miluimPremiumType == -4 && releasedPremiumType == -4) // user is not allowed to login\
            result = SqlMethods.GetMessageTextByMessageID("13");
        else
            result = SqlMethods.GetMessageTextByMessageID("8");
        return result;
    }

    private static string AkaUserLogin(string memberID, string cardNumber, ref int miluimPremiumType, ref int releasedPremiumType)
    {
        string url = GetAkaURL(memberID, cardNumber);
        string response;
        XmlDocument doc = new XmlDocument();

        // to get page data (using msxml4)       
        XMLHTTP http = new XMLHTTP();
        try
        {
            http.open("GET", url, false, null, null);
            http.send(null);
            response = http.responseText;
            doc.LoadXml(response);

            miluimPremiumType = System.Convert.ToInt32(doc.DocumentElement["PremiumTypeForMiluimnikPail"].InnerText);
            releasedPremiumType = System.Convert.ToInt32(doc.DocumentElement["PremiumTypeForReleasedWithHonor"].InnerText);
        }
        catch (Exception ex)
        {
            string mailbody = string.Format("personalNumber:{0} ,cardNumber:{1} Exeption Massege:{2}",
                memberID, cardNumber, ex.StackTrace);
            SqlMethods.SendMail(mailbody, "service is not available");

            return null; // service is not available 
        }
        return response;
    }

    private static string GetAkaURL(string memberID, string cardNumber)
    {
        string url = "https://www.aka.idf.il/Main/HttpWebServices/wsZakautChayal.aspx?" +
                     "personalNumber=" + memberID + "&docNumber=" + cardNumber;
        return url;
    }

    public static void ChangeDB(ref string orgID, ref string dbName)
    {
        if (orgID != ConfigurationSettings.AppSettings["Miluim"])
        {
            orgID = ConfigurationSettings.AppSettings["Miluim"];
            dbName = "Miluim";
        }
        else
        {
            orgID = ConfigurationSettings.AppSettings["HonorReleases"];
            dbName = "HonorReleases";
        }
    }

    internal static bool CheckIP(string ipList, string ipUserHost)
    {
        ipUserHost = ipUserHost.Trim();
        string[] ipArr = ipList.Split(',');
        for (int i = 0; i < ipArr.Length; i++)
        {
            if (ipArr[i].Trim() == ipUserHost)
                return true;
        }
        return false;
    }



    
    internal static string CheckValidUserGetDBName(ref string organizationID, string organizationPassword, string ipUserHost, ref string memberID, ref bool encriptionRequired)
    {
        bool methodPremission = false;
        string orgUserID = "", ipList = "", dbName = "", mainField = "";

        SqlMethods.OrganizationLogin(ref organizationID, organizationPassword, ref orgUserID, ref methodPremission, ref ipList);

        if (string.IsNullOrEmpty(ipList)
        || !BaseMethods.IsDigit(organizationID))
            return "9";
        else
        {
            bool passIP = BaseMethods.CheckIP(ipList, ipUserHost);
            if (!passIP)
                return "30";
        }

        dbName = SqlMethods.GetPasswordANDdbNameForOrgID(organizationID, ref mainField, ref encriptionRequired);
        
        if (mainField == null || mainField == "0")
        {
            if (!BaseMethods.IsDigit(memberID))
                return BaseMethods.GetWSClubsAnswer("4", memberID);

            if (memberID.Length < 9)//ת"ז פחות מ9 ספרות
                memberID = BaseMethods.AddZeroToID(memberID);
        }

        return dbName;
    }

    public static string CheckUserAndPassword(string organizationId, string organizationPassword, string ipUserHost)
    {
       if (string.IsNullOrEmpty(organizationId))
            return "22";

        if (string.IsNullOrEmpty(organizationPassword))
            return "10";

        try
        {
            bool methodPremission = false;
            string orgUserId = "";
            string ipList = "";
            //Check Valid Organization
            SqlMethods.OrganizationLogin(ref organizationId, organizationPassword, ref orgUserId, ref methodPremission, ref ipList);
            if (string.IsNullOrEmpty(ipList)
                || !IsDigit(organizationId))
                return "9";
            var passIp = CheckIP(ipList, ipUserHost);
            if (!passIp)
                return "30";
        }
        catch (Exception ex)
        {
           return "19";
        }
        return "";
    }
}

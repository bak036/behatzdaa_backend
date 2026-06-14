/*                                                              ב"ה
 *                  
 *        שם הקובץ : SqlMethods.cs
 *          פרויקט : WSOrdersOnline
 *           לקליטת הזמנות מכל אירגון  WebService  -המשמשות את ה  sql תיאור : "מחסן" השאילתות 
 *          שם התוכניתן : משה חן
 *           היסטורית גירסאות  :  25-09-08 דניאל - תיקון השאילתא לבירור סטטוס כרטיס     
 * 
 *          DTS- כל הזכויות שמורות ל
 * 
 */
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Xml;
using System.Text;
using System.IO;
using System.Data.Common;
using System.Collections.Generic;
using OrderDll;
using ManagementStockClient.Models;
using System.Linq;
using Nofshonit.Logs;

/// <summary>
/// Summary description for SqlMethods
/// </summary>
public class SqlMethods
{
    public class Coupon
    {
        internal string CouponStockID;
        internal string CouponCode;
        internal string CouponOrderTime;
        internal string CouponStockIdentity;
        internal bool isInternalCoupon = false;
    }

    public SqlMethods()
    {
    }

    public static void SendMail(string strMail)
    {
        SendMail(strMail, "WSOrdersOnLine ERROR");
    }

    public static void SendMail(string strMail, string subject, string FromMailAddress, string ToMailAddress)
    {
        try
        {
            //SmtpClient client = new SmtpClient();
            //MailMessage message = new MailMessage(new MailAddress(FromMailAddress), new MailAddress(ToMailAddress));

            //message.Subject = subject;
            //message.Body = strMail;
            //client.Send(message);
            DtsLoggger.Logger.Info($"Subject:{subject}, strmail:{strMail}");
        }
        catch (Exception ex)
        {

        }

    }

    public static void SendMail(string strMail, string subject)
    {
        try
        {
            //SmtpClient client = new SmtpClient();
            //MailMessage message = new MailMessage(new MailAddress("defaultEmail@yourdomain.com"), new MailAddress("moshech@dts-4u.com"));
            //message.CC.Add(new MailAddress("helpdesk@dts-4u.com"));

            //message.Subject = subject;
            //message.Body = strMail;
            //client.Send(message);
            DtsLoggger.Logger.Info($"Subject:{subject}, strmail:{strMail}");
        }
        catch (Exception ex)
        {

        }

    }

    internal static string GetPasswordANDdbNameForOrgID(string OrganizationID, ref string mainField, ref bool encriptionRequired)
    {
        string OrgphoneNumber = null;
        bool enforcementIdentity = false;
        string metaData = null;
        bool ActiveCardNotNeededToOrder = false;
        bool AllowAnonymousTransactions = false;
        bool notUsed = false;
        bool isOrdersTable = false;
        return GetPasswordANDdbNameForOrgID(OrganizationID, ref mainField, ref enforcementIdentity,
                metaData, ref ActiveCardNotNeededToOrder, ref OrgphoneNumber, ref encriptionRequired, ref AllowAnonymousTransactions, ref notUsed, ref isOrdersTable);
    }

    internal static string GetPasswordANDdbNameForOrgID(string OrganizationID, ref string mainField, ref bool enforcementIdentity,
                string metaData, ref bool ActiveCardNotNeededToOrder, ref bool AllowAnonymousTransactions)
    {
        string OrgphoneNumber = null;
        bool encriptionRequired = false;
        bool notUsed = false;
        bool isOrdersTable = false;
        return GetPasswordANDdbNameForOrgID(OrganizationID, ref mainField, ref enforcementIdentity,
                metaData, ref ActiveCardNotNeededToOrder, ref OrgphoneNumber, ref encriptionRequired, ref AllowAnonymousTransactions, ref notUsed, ref isOrdersTable);
    }

    internal static string GetPasswordANDdbNameForOrgID(string OrganizationID, ref string mainField, ref bool enforcementIdentity,
            string metaData, ref bool ActiveCardNotNeededToOrder, ref bool AllowAnonymousTransactions, ref bool isTradeSite, ref bool isOrdersTable)
    {
        string OrgphoneNumber = null;
        bool encriptionRequired = false;
        return GetPasswordANDdbNameForOrgID(OrganizationID, ref mainField, ref enforcementIdentity,
                metaData, ref ActiveCardNotNeededToOrder, ref OrgphoneNumber, ref encriptionRequired, ref AllowAnonymousTransactions, ref isTradeSite, ref isOrdersTable);
    }
    internal static string GetPasswordANDdbNameForOrgID(string OrganizationID, ref string mainField, ref bool enforcementIdentity,
                string metaData, ref bool ActiveCardNotNeededToOrder,
                ref string orgPhoneSms, ref bool encriptionRequired, ref bool AllowAnonymousTransactions, ref bool isTradeSite, ref bool isOrdersTable)
    {
        SqlConnection connection = new SqlConnection();
        SqlDataReader reader = null;
        string dbName = "";
        //string ip = "";
        try
        {
            SqlParameter p = new SqlParameter("@OrganizationID", SqlDbType.Int);
            p.Value = OrganizationID;
            reader = DataBase.ExecuteReader(
                @"  SET ARITHABORT ON
                        SELECT Top 1 DBName, SmsMobile, MainField, IdentityOnOrders, ActiveCardNotNeeded,
                        case when IsEncodedCards  = 1 then convert (bit, 1) else convert (bit, 0) end as IsEncodedCards,
                        AllowAnonymousTransactions,isnull(cast(OrganizationParamsXML as xml).query('attributes/operation/a65').value('/','int'),0) as isTradeSite,
                        IsOrdersTable
                        FROM Organizations 
                        WHERE OrganizationID = @OrganizationID", connection, p);

            while (reader.Read())
            {
                if (reader[1] != DBNull.Value)
                    orgPhoneSms = reader.GetString(1);
                if (reader[2] != DBNull.Value)
                    mainField = reader[2].ToString().Trim();
                enforcementIdentity = Convert.ToBoolean(reader[3]);
                dbName = reader[0].ToString().Trim();
                //if (reader[4] != DBNull.Value)
                //    ip = reader[4].ToString().Trim();

                if (reader["ActiveCardNotNeeded"] != DBNull.Value)
                    ActiveCardNotNeededToOrder = Convert.ToBoolean(reader["ActiveCardNotNeeded"]);

                encriptionRequired = Convert.ToBoolean(reader["IsEncodedCards"]);

                if (reader["AllowAnonymousTransactions"] != DBNull.Value)
                    AllowAnonymousTransactions = Convert.ToBoolean(reader["AllowAnonymousTransactions"]);

                if (reader["isTradeSite"] != DBNull.Value)
                    isTradeSite = Convert.ToBoolean(reader["isTradeSite"]);

                isOrdersTable = Convert.ToBoolean(reader["IsOrdersTable"]);

                break;
            }
            //if (!metaData.Contains("FromGiftCard"))
            //{
            //    if (OrganizationID != "17")
            //    {
            //        if (!string.IsNullOrEmpty(dbName)) //&& OrganizationID != "8") // skip the ip test for IPa
            //        {
            //            string[] ipArr = ip.Split(',');
            //            for (int i = 0; i < ipArr.Length; i++)
            //            {
            //                if (ipArr[i].Trim() == ipUserHost)
            //                    return dbName;
            //            }
            //            return "30";
            //        }
            //    }
            //    else
            //        return dbName;
            //}
            //else
            return dbName;
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetPasswordANDdbNameForOrgID \n OrganizationID -> " + OrganizationID
                + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message + "\n ex.Source -> " + ex.Source + "\n ex.StackTrace -> " + ex.StackTrace);
            return "19";
        }
        finally
        {
            if (reader != null)
            {
                if (!reader.IsClosed)
                    reader.Close();
            }
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }
        return "19";
    }

    internal static string ThereIsMemberID(string dbName, string MemberID)
    {
        string back = "2";
        string query;
        //if (dbName == "LeumiCard_GCPoints")
        //    return back;
        try
        {
            query = string.Format(@"IF EXISTS 
                                        (SELECT MemberId 
                                        FROM {0}..AllMembers 
                                        WHERE MemberId = '{1}') 
                                    SELECT '1' 
                                        ELSE 
                                    SELECT '2'", dbName, MemberID);
            back = (string)DataBase.ExecuteScalar(query);
            return back;
        }
        catch (Exception ex)
        {
            SendMail("Function -> ThereIsMemberID \n dbName -> " + dbName + "\n MemberID -> " + MemberID
                + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
    }

    internal static string CheckCardStatusForMemberID(string dbName, string MemberID, ref string cardNumber)
    {
        if (dbName == "LeumiCard_GCPoints")
            return "100";
        SqlConnection connection = new SqlConnection();
        SqlDataReader readerStatus = null;
        DataTable dt = new DataTable();
        try
        {
            readerStatus = DataBase.ExecuteReader(
                "SELECT CardNumber,CardStatus FROM " + dbName + "..Cards WHERE IDMember = '" + MemberID + "'", connection);
            if (readerStatus != null)
                dt.Load(readerStatus);
            //object oX = DataBase.ExecuteScalar("IF EXISTS (SELECT CardStatus " 
            //                                            + "FROM " + dbName + "..Cards "
            //                                            + "WHERE IDMember = '" + MemberID + "' AND CardStatus = 1) "
            //                                    + "SELECT '1' "
            //                                 + "ELSE "
            //                                    + "(SELECT CardStatus "
            //                                    + "FROM " + dbName + "..Cards  "
            //                                    + "WHERE IDMember = '" + MemberID + "'");
            ////  היות ויש יותר מכרטיס אחד עלינו לודא שאם יש כרטיס פעיל הסטטוס המוחזר יהיה 1 - תוקן ע"י דניאל  
            //back = Convert.ToInt16(oX);
            //return back.ToString();
        }
        catch (Exception ex)
        {
            SendMail("Function -> CheckCardStatusForMemberID \n dbName -> " + dbName + "\n MemberID -> " + MemberID
                + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        finally
        {
            if (readerStatus != null)
            {
                if (!readerStatus.IsClosed)
                    readerStatus.Close();
            }
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows.Count == 1)
            {
                cardNumber = dt.Rows[0].ItemArray[0].ToString();
                return dt.Rows[0].ItemArray[1].ToString();
            }
            else
            {
                for (int i = 1; i >= 0; i--)
                {
                    for (int z = 0; z < dt.Rows.Count; z++)
                    {
                        if (dt.Rows[z].ItemArray[1].ToString() == i.ToString())
                        {
                            cardNumber = dt.Rows[z].ItemArray[0].ToString();
                            return i.ToString();
                        }
                    }
                }
                return "2";
            }
        }
        else
            return "-1";
    }


    internal static string CheckIfVariantExists(string dbName, string ProductID, ref bool isCampaign, ref int GroupLimitsID,
    ref int implementationType, ref DateTime lastImplementationDate, ref bool splitOrder, ref int variantType, ref string posBarCode)
    {
        bool disable = true;
        string back = null;
        int BusinessSubTypeID = 0;
        SqlDataReader readerCheck = null;
        SqlConnection connection = new SqlConnection();
        try
        {
            var sql = string.Format(@"SELECT Top 1 Type, DisabledToOrder, ISCampaign, 
                            GroupLimitsID,TypeCalcImplementationDate, LastImplementationDate ,ShowDate,BusinessSubTypeID, SplitOrder, VariantType, POSBarCode
                            FROM {0}..productsVars WHERE FullBarCode = '{1}'", dbName, ProductID);
            readerCheck = DataBase.ExecuteReader(sql, connection);
            if (readerCheck.Read())
            {
                if (readerCheck["DisabledToOrder"] != DBNull.Value)
                    disable = Convert.ToBoolean(readerCheck["DisabledToOrder"]);
                if (readerCheck["Type"] != DBNull.Value)
                    back = readerCheck["Type"].ToString();
                if (readerCheck["POSBarCode"] != DBNull.Value)
                    posBarCode = readerCheck["POSBarCode"].ToString();
                if (readerCheck["ISCampaign"] != DBNull.Value)
                    isCampaign = Convert.ToBoolean(readerCheck["ISCampaign"]);
                if (readerCheck["GroupLimitsID"] != DBNull.Value)
                    GroupLimitsID = Convert.ToInt32(readerCheck["GroupLimitsID"]);
                else
                    GroupLimitsID = 0;
                if (readerCheck["VariantType"] != DBNull.Value)
                    variantType = Convert.ToInt32(readerCheck["VariantType"]);
                if (readerCheck["BusinessSubTypeID"] != DBNull.Value)
                    BusinessSubTypeID = Convert.ToInt32(readerCheck["BusinessSubTypeID"]);

                if (BusinessSubTypeID == 6)
                {
                    if (readerCheck["TypeCalcImplementationDate"] != DBNull.Value)
                    {
                        int iType = Convert.ToInt32(readerCheck["TypeCalcImplementationDate"]);
                        if (iType == 1) // show LastImplementationDate and not ShowDate
                        {
                            if (readerCheck["LastImplementationDate"] != DBNull.Value)
                            {
                                lastImplementationDate = Convert.ToDateTime(readerCheck["LastImplementationDate"]);
                                lastImplementationDate = new DateTime(lastImplementationDate.Year, lastImplementationDate.Month, lastImplementationDate.Day, 23, 59, 59);
                            }
                        }
                        else
                        {
                            if (readerCheck["ShowDate"] != DBNull.Value)
                                lastImplementationDate = Convert.ToDateTime(readerCheck["ShowDate"]);
                        }
                    }
                    else
                    {
                        if (readerCheck["ShowDate"] != DBNull.Value)
                            lastImplementationDate = Convert.ToDateTime(readerCheck["ShowDate"]);

                    }
                }
                else
                {
                    if (readerCheck["TypeCalcImplementationDate"] != DBNull.Value)
                    {
                        implementationType = Convert.ToInt32(readerCheck["TypeCalcImplementationDate"]);
                        if (readerCheck["LastImplementationDate"] != DBNull.Value)
                            lastImplementationDate = Convert.ToDateTime(readerCheck["LastImplementationDate"]);
                    }
                }
                if (readerCheck["SplitOrder"] != DBNull.Value)
                    splitOrder = Convert.ToBoolean(readerCheck["SplitOrder"]);
                else
                    splitOrder = false;
            }
            else
                return null;
            if (string.IsNullOrEmpty(back))
                return null;
            //בוטלה בדיקה במילואים ומשתחררים כי הבדיקה מתבצעת כבר באתר
            if (dbName != "Miluim" && dbName != "MiluimTest" && dbName != "HonorReleases" && dbName != "HonorReleasesTest" && dbName != "MiluimDevelopment" && dbName != "ReleasesDevelopment" && dbName != "Hot" && dbName.ToUpper() != "KNOWLEDGE4ALL" && disable)
                return "28";
            //if (disable)//בטבלת המקטים זה הפוך אם נכון מקט פג תוקף
            //    return "28";
            return back.Trim();
        }
        catch (Exception ex)
        {
            //SendMail("Function -> CheckIfVariantExists \n dbName -> " + dbName + "\n ProductID -> " + ProductID
            //    + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message + "\n ex.Source -> " + ex.Source + "\n StackTrace -> " + ex.StackTrace);
            return "19";
        }
        finally
        {
            if (readerCheck != null)
            {
                if (!readerCheck.IsClosed)
                    readerCheck.Close();
            }
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }
    }

    internal static string CheckVariantType(string dbName, string ProductID, ref bool isCampaign, ref int GroupLimitsID, ref int variantType,
                                            ref int BusinessSubTypeID, ref bool MarkSeats)
    {
        string Type = null;
        SqlDataReader readerCheck = null;
        SqlConnection connection = new SqlConnection();
        try
        {
            readerCheck = DataBase.ExecuteReader(string.Format(@"SELECT Top 1 Type, ISCampaign, GroupLimitsID, VariantType,BusinessSubTypeID,MarkSeats
                                                                FROM {0}..productsVars 
                                                                WHERE FullBarCode = '{1}'", dbName, ProductID), connection);
            if (readerCheck.Read())
            {
                if (readerCheck["VariantType"] != DBNull.Value)
                    variantType = Convert.ToInt32(readerCheck["VariantType"]);
                if (readerCheck["Type"] != DBNull.Value)
                    Type = readerCheck["Type"].ToString();
                if (readerCheck["ISCampaign"] != DBNull.Value)
                    isCampaign = Convert.ToBoolean(readerCheck["ISCampaign"]);
                if (readerCheck["GroupLimitsID"] != DBNull.Value)
                    GroupLimitsID = Convert.ToInt32(readerCheck["GroupLimitsID"]);
                else
                    GroupLimitsID = 0;
                if (readerCheck["BusinessSubTypeID"] != DBNull.Value)
                    BusinessSubTypeID = Convert.ToInt16(readerCheck["BusinessSubTypeID"]);
                if (readerCheck["MarkSeats"] != DBNull.Value)
                    MarkSeats = Convert.ToBoolean(readerCheck["MarkSeats"]);
            }
            else
                return null;
            if (string.IsNullOrEmpty(Type))
                return null;

            return Type.Trim();
        }
        catch (Exception ex)
        {
            SendMail("Function -> CheckIfVariantExists \n dbName -> " + dbName + "\n ProductID -> " + ProductID
                + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message + "\n ex.Source -> " + ex.Source + "\n StackTrace -> " + ex.StackTrace);
            return "19";
        }
        finally
        {
            if (readerCheck != null)
            {
                if (!readerCheck.IsClosed)
                    readerCheck.Close();
            }
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }
    }
    internal static string InsertNewOrder(int? PremiumType, string dbName, string type, string MemberID, string ProductID, string Quantity,
      string OriginalOrderID, string cardNumber, string Reason, bool insertCard, string metaData, string catalogicPrice,
      string irgunPriceFormula, string varDiscountFormula, string varPriceFormula, string varComissionFormula, string cancelComission,
      bool isCampaign, int GroupLimitsID, string MarketingCommission, string paymentID, string lastImplementationDate)
    {
        long temp;
        return InsertNewOrder(PremiumType, dbName, type, MemberID, ProductID, Quantity,
      OriginalOrderID, cardNumber, Reason, insertCard, metaData, catalogicPrice,
      irgunPriceFormula, varDiscountFormula, varPriceFormula, varComissionFormula, cancelComission,
      isCampaign, GroupLimitsID, MarketingCommission, paymentID, lastImplementationDate, 0, out temp, null, false);
    }
    internal static string InsertNewOrder(int? PremiumType, string dbName, string type, string MemberID, string ProductID, string Quantity,
        string OriginalOrderID, string cardNumber, string Reason, bool insertCard, string metaData, string catalogicPrice,
        string irgunPriceFormula, string varDiscountFormula, string varPriceFormula, string varComissionFormula, string cancelComission,
        bool isCampaign, int GroupLimitsID, string MarketingCommission, string paymentID, string lastImplementationDate,
        long Cid, out long atractionIdentity, Coupon coupon, bool isTradeSite,  string parentMultiVariant = "", int coins = 0, bool isOrderTable = false, GeneralOrdersItem generalOrderItem = null, string dtsRedimCode = "")
    {
        int? orderId = null;
        /*
        float priceDiscountformula = 0;
        if(!string.IsNullOrEmpty(varDiscountFormula))
        {
            float.TryParse(varDiscountFormula, out priceDiscountformula);
        }
        varDiscountFormula = priceDiscountformula.ToString();
        */
        if (type == "ANA")
            type = "TZIMERS";
        long identityForBack = 0, identity = 0;

        //14/10/2013
        string DateEXE_Value = "'01/01/1900'";
        string QuantityBlance = "0";
        string OrderDate;

        if (string.IsNullOrEmpty(paymentID))
            paymentID = "NULL";

        DateTime? OrderDateXml = ExtractDateOrderFromXml(metaData);//תאריך הזמנה מתוך ה xml

        if (OrderDateXml.HasValue)
            OrderDate = Convert.ToString(OrderDateXml);
        else
            OrderDate = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        //terminalNo to exe      
        string terminalNo = ExtractTerminalExeFromXml(metaData);//get termibalNo from xml



        if (!string.IsNullOrEmpty(terminalNo))
        {
            DateEXE_Value = "convert(datetime,'" + OrderDate + "',103)";//dateExe eqal to DateOrder when terminal existing in xml
            QuantityBlance = type == "ATRACTIONS" ? Quantity : "1";//in spa/tzimer/movies balance = 1 in row

            terminalNo = "'" + terminalNo + "'";
        }
        else
        {
            terminalNo = "NULL";
        }



        //string OrderDate = Convert.ToString(System.DateTime.Now);
        //14/10/2013

        int quant = int.Parse(Quantity);
        int OrderQuntity = -101;
        string asmchta = null;
        string orderAsmchta = string.Empty;
        SqlDataReader reader = null;
        atractionIdentity = 0;


        long? categoryNumber = GetCategoryNumberByBarcodeAndDbName(dbName, ProductID);
        int orgID = GetOrgIDByDbName(dbName);

        //Pointers + check if check stock is active
        bool IsManagmentStockEnabled = ConfigurationManager.AppSettings["SkipManagmentStock"] == null ||
                                       ConfigurationManager.AppSettings["SkipManagmentStock"] != "1";

        ManagementStockClient.Api mApiClient = new ManagementStockClient.Api();
        BaseApiResult checkStock = null;
        CheckStockRequestModel checkStockObject = null;

        //--------------------CheckStock-------------------
        if (IsManagmentStockEnabled)
        {
            checkStockObject = new CheckStockRequestModel()
            {
                OrgID = orgID,
                VariantData = new List<VariantData>()
                {
                    new VariantData()
                    {
                        Quantity = quant,
                        FullBarcode = ProductID,
                        BenefitID = Convert.ToInt32(categoryNumber.GetValueOrDefault())
                    }
                }
            };


            checkStock = mApiClient.CheckStock(checkStockObject);
            //אזל המלאי
            if (checkStock.Code != BaseApiResult.HTTPResponseCode.Success ||
                checkStock.VariantResult.First().ResultCode != VariantResult.ResultCodeEnum.SUCCESS)
            {

                return "95";
            }
        }
        //-------------------------------------------------



        SqlCommand command = new SqlCommand();
        SqlTransaction transaction = null;
        try
        {
            //DataBase.BeginTransaction();
            DataBase.BeginTransaction(ref transaction, command);
            PremiumType = Convert.ToInt32(DataBase.ExecuteScalarForTransaction($@"SELECT PremiumType from {dbName}..AllMembers WHERE MemberId = '{generalOrderItem.MemberId}'", command));

            //First regular orders table
            if (generalOrderItem != null && generalOrderItem.OrderId == 0)
            {
                //var exists = Convert.ToInt32(DataBase.ExecuteScalarForTransaction($@"SELECT COUNT(*) FROM {dbName}..Orders WHERE ExternalGuid = {generalOrderItem.ExternalGuid}",command));

                //Insert to Orders table, get the orderId
                orderId = Convert.ToInt32(DataBase.ExecuteScalarForTransaction($@"INSERT INTO {dbName}..Orders(MemberId, InsertDate, IsSentToFriend, FriendName, FriendMobile, NumberOfRetries, ExternalGuid,
                                                    CreditCardToken, OrderStatusId, CreditCard16Digits, CreditCardExpirey) VALUES('{generalOrderItem.MemberId}', GETDATE(), '{generalOrderItem.IsSentToFriend}',
                                                    '{generalOrderItem.FriendName}', '{generalOrderItem.FriendMobile}', 0, '{generalOrderItem.ExternalGuid}', '{generalOrderItem.CreditCardToken}',
                                                    {generalOrderItem.OrderStatusId}, '{generalOrderItem.CreditCard16Digits ?? string.Empty}', '{generalOrderItem.CreditCardExpirey ?? string.Empty}') SELECT SCOPE_IDENTITY()", command));


                generalOrderItem.OrderId = orderId.Value;
            }
            else if (generalOrderItem != null)
            {
                orderId = generalOrderItem.OrderId;
            }

            if (type == "ATRACTIONS" || type == "CLOSE_DAYS")
            {
                SqlConnection connection = new SqlConnection();
                string query = "";
                //הפרדת שורות בין הזמנות לאותו הוריאנט
                //BUG 1183 : ATRACTIONSOrders לא לחבר הזמנות בטבלת 
                //                if (quant > 0)
                //                {
                //                    query = string.Format(@"SELECT MemberOrderQuntity,MemberOrderAsmchta  FROM {0}..{1}Orders 
                //                        inner join {0}..productsVars on fullbarcode = barcode 
                //                        WHERE MemberID= '{2}' AND BarCode = '{3}' AND MemberOrderBlance = 0 AND SplitOrder <>  1 
                //                        AND MemberOrderQuntity >= 0 And {0}..{1}Orders.LastImplementationDate = '{4}' "
                //                        , dbName, type, MemberID, ProductID, lastImplementationDate);
                //                }
                //else


                if (dbName == "ITU" && quant > 0)
                {
                    query = string.Format(@"SELECT Top 1 MemberOrderQuntity,MemberOrderAsmchta  FROM {0}..{1}Orders 
                                        inner join {0}..productsVars on fullbarcode = barcode 
                                        WHERE MemberID= '{2}' AND BarCode = '{3}' AND MemberOrderBlance = 0 AND isnull(SplitOrder, 0) <>  1 
                                        AND MemberOrderQuntity >= 0"
                        // And {0}..{1}Orders.LastImplementationDate = '{4}' " הורדנו להסתדרות וחשמל, אם יבקשו נפתח
                        , dbName, type, MemberID, ProductID, lastImplementationDate);
                }
                else if (quant < 0)
                {
                    orderAsmchta = ExtractAsmachtaFromXml(metaData);
                    if (string.IsNullOrEmpty(orderAsmchta))
                    {//במקרה שהזיכוי נשלח מפונקציית השירות של הארגונים ללא אסמכתא
                        query = string.Format(@"SELECT MemberOrderQuntity,MemberOrderAsmchta  FROM {0}..{1}Orders 
                                                WHERE MemberID= '{2}' AND BarCode = '{3}' AND MemberOrderBlance = 0 
                                                AND MemberOrderQuntity >= 0
                                                And isnull(LastImplementationDate, '2099-12-31') > GetDate() ",
                                                                dbName, type, MemberID, ProductID);
                    }
                    else
                    {//במקרה שהזיכוי נשלח מפונקציית הביטול במערכת הניהול כולל אסמכתא ייחודית לביטול
                        query = string.Format(@"SELECT MemberOrderQuntity,MemberOrderAsmchta  FROM {0}..{1}Orders 
                                                WHERE MemberID= '{2}' AND BarCode = '{3}' AND MemberOrderBlance = 0 
                                                AND MemberOrderQuntity >= 0 And MemberOrderAsmchta = '{4}'",
                                                                dbName, type, MemberID, ProductID, orderAsmchta);
                    }
                    //}
                }
                if (!string.IsNullOrEmpty(query))
                {

                    reader = DataBase.ExecuteReader(query, connection);
                    while (reader.Read())
                    {
                        OrderQuntity = Convert.ToInt32(reader["MemberOrderQuntity"]);
                        identity = Convert.ToInt64(reader["MemberOrderAsmchta"]);
                        if (quant < 0 && OrderQuntity >= Math.Abs(quant))
                            break;
                    }
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                            reader.Close();
                        if (connection.State != ConnectionState.Closed)
                            connection.Close();
                    }
                }
                if (OrderQuntity == -101 && identity == 0)
                {
                    if (quant < 1)
                        return "23";
                    if (insertCard)
                    {
                        atractionIdentity = identity = Convert.ToInt64(DataBase.ExecuteScalarForTransaction("INSERT INTO " + dbName + ".." + type
                                            + "Orders (MemberID ,BarCode ,MemberOrderDateEXE ,MemberOrderQuntity ,MemberOrderBlance,CardNumber, LastImplementationDate,CardId,MemberTerminalExe,ParentMultiVariant, OrderId, CardNumber)  VALUES ('" + MemberID
                                            + "', '" + ProductID + "', " + DateEXE_Value + ", " + Quantity + ", " + QuantityBlance + ", '" + cardNumber + "', '" + lastImplementationDate + "'," + Cid + "," + terminalNo + ", '" + parentMultiVariant + "', " + (orderId.HasValue ? orderId.Value.ToString() : "NULL") + ", '" + dtsRedimCode + "') SELECT SCOPE_IDENTITY()", command));
                    }
                    else
                    {
                        if (coupon != null)//external coupon entered
                        {
                            if (!insertCouponData(coupon, MemberID, command))
                            {
                                transaction.Rollback();
                                return "81";
                            }

                            atractionIdentity = identity = Convert.ToInt64(DataBase.ExecuteScalarForTransaction("INSERT INTO " + dbName + ".." + type
                                        + @"Orders (OrderDate,MemberID ,BarCode ,MemberOrderDateEXE ,MemberOrderQuntity ,MemberOrderBlance, LastImplementationDate,UpdatePriceDate,CardId,MemberTerminalExe,ParentMultiVariant, OrderId, CardNumber) 
                                          VALUES (convert(datetime,'" + OrderDate + "',103),'" + MemberID + "', '" + ProductID + "', " + DateEXE_Value + ", "
                                                                              + Quantity + ", " + QuantityBlance + " ,'" + lastImplementationDate + "','" + lastImplementationDate + "'," + Cid + "," + terminalNo + ", '" + parentMultiVariant + "', " + (orderId.HasValue ? orderId.Value.ToString() : "NULL") + ", '" + dtsRedimCode + "') SELECT SCOPE_IDENTITY()", command));

                            int resault = Convert.ToInt32(DataBase.ExecuteNonQueryForTransaction("INSERT INTO " + dbName +
                                         "..CouponsStock_ATROrders VALUES(" + atractionIdentity + "," + coupon.CouponStockIdentity + ")", command));
                            if (resault <= 0)
                            {
                                transaction.Rollback();
                                return "19";
                            }
                        }
                        else
                        {
                            atractionIdentity = identity = Convert.ToInt64(DataBase.ExecuteScalarForTransaction("INSERT INTO " + dbName + ".." + type
                                         + @"Orders (OrderDate,MemberID ,BarCode ,MemberOrderDateEXE ,MemberOrderQuntity ,MemberOrderBlance, LastImplementationDate,UpdatePriceDate,CardId,MemberTerminalExe, ParentMultiVariant, OrderId, CardNumber) 
                                           VALUES (convert(datetime,'" + OrderDate + "',103),'" + MemberID + "', '" + ProductID + "', " + DateEXE_Value + ", "
                                                                              + Quantity + ", " + QuantityBlance + " , '" + lastImplementationDate + "','" + lastImplementationDate + "'," + Cid + "," + terminalNo + ", '" + parentMultiVariant + "', " + (orderId.HasValue ? orderId.Value.ToString() : "NULL") + ", '" + dtsRedimCode + "') SELECT SCOPE_IDENTITY()", command));
                        }
                    }
                }
                else
                {
                    OrderQuntity = OrderQuntity + quant;
                    if (OrderQuntity < 0)
                        return "23";
                    if (identity != 0)
                    {
                        int rowUp = Convert.ToInt32(
                            DataBase.ExecuteNonQueryForTransaction(
                            "UPDATE " + dbName + ".." + type + "Orders SET MemberOrderQuntity = "
                            + OrderQuntity + " WHERE MemberID = '" + MemberID + "' AND BarCode = '" + ProductID + "' AND MemberOrderAsmchta = '" + identity
                            + "' AND MemberOrderBlance = 0 AND MemberOrderQuntity >= 0", command));
                        if (rowUp < 1)
                        {
                            SendMail("Function -> InsertNewOrder (Cannot Do Update) \n dbName -> " + dbName + "\n ProductID -> " + ProductID
                            + "\n Type OF Order -> " + type + "\n MemberID -> " + MemberID + "\n Quantity -> " + Quantity + "\n OriginalOrderID -> "
                            + OriginalOrderID + "\nIdentity -> " + identity);
                            transaction.Rollback();
                            return "0";
                            //identity = Convert.ToInt64(DataBase.ExecuteScalar("SELECT MAX(MemberOrderAsmchta) FROM " + dbName + ".." + type
                            //    + "Orders WHERE BarCode = '" + ProductID + "' AND MemberOrderBlance = 0  AND MemberId = " + MemberID));
                        }
                    }
                    else
                    {
                        SendMail("Function -> InsertNewOrder (Identity Is Null) \n dbName -> " + dbName + "\n ProductID -> " + ProductID
                            + "\n Type OF Order -> " + type + "\n MemberID -> " + MemberID + "\n Quantity -> " + Quantity + "\n OriginalOrderID -> "
                            + OriginalOrderID + "\nIdentity -> " + identity);
                        transaction.Rollback();
                        return "0";
                    }
                }
            }
            else if (type == "BUY_MONEY")
            {
                decimal var = Convert.ToDecimal(
                    DataBase.ExecuteScalarForTransaction("SELECT shortNameVar FROM " + dbName
                    + "..productsVars WHERE FullBarCode = '" + ProductID + "'", command));
                var = var * quant;
                if (Math.Abs(var) > 3000)
                    return "20";
                //string cardNumber = (string)DataBase.ExecuteScalar("SELECT CardNumber FROM " + dbName + "..Cards WHERE IDMember = '" + MemberID + "' AND CardStatus = 1");
                if (quant > 0)
                {
                    identity = Convert.ToInt64(
                        DataBase.ExecuteScalarForTransaction("INSERT INTO " + dbName
                        + "..Requests (RequestTime,RequestSource,RequestOp,RequestType,RequestStatus,ID1,Card1,Amount,Remark,ReasonCode) VALUES (GETDATE(),5,0,6,3,'"
                        + MemberID + "', '" + cardNumber + "', " + var + ", '" + OriginalOrderID + "','" + Reason + "') SELECT SCOPE_IDENTITY()", command));
                }
                else
                {
                    identity = Convert.ToInt64(
                        DataBase.ExecuteScalarForTransaction("INSERT INTO " + dbName
                        + "..Requests (RequestTime,RequestSource,RequestOp,RequestType,RequestStatus,ID1,Card1,Amount,Remark,ReasonCode) VALUES (GETDATE(),5,0,6,4,'"
                        + MemberID + "', '" + cardNumber + "', " + var + ", '" + OriginalOrderID + "','" + Reason + "') SELECT SCOPE_IDENTITY()", command));
                }
            }
            else
            {
                //if (dbName != "HonorReleases" && dbName != "Miluim" && dbName != "Shufersal"
                //    && dbName != "HonorReleasesTest" && dbName != "MiluimTest" && dbName != "ShufersalTest"
                //    && dbName != "MiluimDevelopment" && dbName != "ReleasesDevelopment" && dbName != "ShufersalDevelopment" &&
                //    dbName != "Hot" && dbName != "LeumiCard" && dbName != "Yes" && dbName != "Teva" && dbName != "KNOWLEDGE4ALL" && dbName != "NofshonitClub")
                if (!isTradeSite)
                {
                    if (type == "MOVIES")
                    {
                        int movieCount = Convert.ToInt32(
                            DataBase.ExecuteScalarForTransaction("SELECT BusinessShortName FROM " + dbName
                            + "..productsVars WHERE FullBarCode = '" + ProductID + "'", command));
                        quant = quant * movieCount;
                    }
                }
                if (quant > 0)
                {
                    var now = OrderDate;
                    for (int i = 0; i < quant; i++)
                    {
                            identity = Convert.ToInt64(
                                DataBase.ExecuteScalarForTransaction("INSERT INTO " + dbName + ".." + type + "Orders "
                                + @" (IsInCancelProcess,OrderDate,MemberId,BarCode,OrderDateExe,OrderQuantity,OrderBalance, LastImplementationDate,UpdatePriceDate,CardId,TerminalExe,ParentMultiVariant, OrderId, CardNumber)  
                                VALUES (0,convert(datetime,'" + OrderDate + "',103),'" + MemberID + "', '"
                                + ProductID + "', " + DateEXE_Value + ", 1, " + QuantityBlance + ", '" + lastImplementationDate + "','" + lastImplementationDate + "'," + Cid + "," + terminalNo + ", '" + parentMultiVariant + "', " + (orderId.HasValue ? orderId.Value.ToString() : "NULL") + ", '" + dtsRedimCode + "') SELECT SCOPE_IDENTITY()", command));
                            //var query = "INSERT INTO " + dbName + ".." + type + "Orders "
                            //    + @" (IsInCancelProcess,OrderDate,MemberId,BarCode,OrderDateExe,OrderQuantity,OrderBalance, LastImplementationDate,UpdatePriceDate,CardId,TerminalExe,ParentMultiVariant)   
                            //    VALUES (0,convert(datetime,'" + OrderDate + "',103),'" + MemberID + "', '"
                            //    + ProductID + "', " + DateEXE_Value + ", 1, " + QuantityBlance + ", '" + lastImplementationDate + "','" + lastImplementationDate + "'," + Cid + "," + terminalNo + ", '" + parentMultiVariant + "') SELECT SCOPE_IDENTITY()";
                            //identity = Convert.ToInt64(
                            //    DataBase.ExecuteScalarForTransaction(query, command));
                        asmchta += "," + identity;
                    }
                }
                else
                {
                    orderAsmchta = ExtractAsmachtaFromXml(metaData);
                    if (!string.IsNullOrEmpty(orderAsmchta)) //כאשר יש אסמכתא - גיפט קארד
                    {
                        identity = Convert.ToInt32(orderAsmchta);
                        int rowUp = Convert.ToInt32(
                                DataBase.ExecuteNonQueryForTransaction("UPDATE " + dbName + ".." + type
                                + "Orders SET OrderQuantity = 0 WHERE (MemberId = '" + MemberID + "' AND OrderAsmchta = '" + identity + "')", command));
                        asmchta += "," + identity;
                    }
                    else ////במקרה שנשלח מפונקציית השירות של הארגונים ללא אסמכתא
                    {
                        int sumOrders = 0;
                        object sumOrderFromTable = DataBase.ExecuteScalarForTransaction(
                            "SELECT SUM(OrderQuantity) as OrderQuantity FROM " + dbName + ".." + type
                            + "Orders WHERE MemberId = '" + MemberID + "' AND BarCode = '" + ProductID + "' AND OrderBalance = 0", command);
                        if (sumOrderFromTable != DBNull.Value)
                            sumOrders = Convert.ToInt32(sumOrderFromTable);
                        sumOrders = sumOrders + quant;
                        if (sumOrders < 0)
                            return "23";

                        for (int i = quant + 1; i <= 0; i++)
                        {
                            identity = Convert.ToInt64(
                                DataBase.ExecuteScalarForTransaction("SELECT MAX(OrderAsmchta) AS OrderAsmchta FROM " + dbName + ".." + type
                                + "Orders WHERE (MemberId = '" + MemberID + "' AND BarCode = '" + ProductID + "' AND OrderBalance = 0 AND OrderQuantity > 0)", command));
                            int rowUp = Convert.ToInt32(
                                DataBase.ExecuteNonQueryForTransaction("UPDATE " + dbName + ".." + type
                                + "Orders SET OrderQuantity = 0 WHERE (MemberId = '" + MemberID + "' AND OrderAsmchta = '" + identity + "')", command));
                            asmchta += "," + identity;
                        }
                    }
                }
            }

            if (identity == 0)
            {
                try
                {
                    //DataBase.OpenConnectionAgain();
                    //DataBase.Rollback();
                    transaction.Rollback();
                    return "0";
                }
                catch (Exception ex)
                {
                    SendMail("Function -> identity == 0 (Rollback) \n dbName -> " + dbName + "\n ProductID -> " + ProductID
                    + "\n Type OF Order -> " + type + "\n MemberID -> " + MemberID + "\n Quantity -> " + Quantity + "\n OriginalOrderID -> "
                    + OriginalOrderID + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
                }
            }

            if (type != "BUY_MONEY")
            {
                if (dbName == "LeumiCard_GCPoints")
                {//בתוספת רישום לתגית XMLDATA
                    identityForBack = Convert.ToInt64(
                        DataBase.ExecuteScalarForTransaction("INSERT INTO " + dbName + @"..WebServiceTransaction 
                        (TTransactionDateTime,TTransactionProductID,TTransactionMemberID,TTransactionquantity,TTransactionMetaData,TTransactionOrder,TTransactionStatus,XMLParam) 
                        VALUES (convert(datetime,'" + OrderDate + "',103), '" + ProductID + "', '" + MemberID + "', " + Quantity
                                                              + ",'" + OriginalOrderID + "','" + identity + "',1,'" + metaData + "') SELECT SCOPE_IDENTITY()", command));
                }
                else
                {
                    string query;
                        //varPriceFormula = "Math.ceil(@IrgunPriceFormula# - @VarDiscountFormula#)";
                        query = string.Format(@"INSERT INTO {0}..WebServiceTransaction 
                    (TTransactionDateTime, TTransactionProductID,TTransactionMemberID, TTransactionquantity,
                    TTransactionMetaData, TTransactionOrder, TTransactionStatus,CatalogicPrice, IrgunPrice,
                    CustomerDiscount, CustomerPrice, DistributionComission, XMLParam, CancelCommission,ISCampaign,GroupLimitsID,MarketingCommission, PaymentID, Coins,OrderId, premiumType) 
                    VALUES (convert(datetime,'" + OrderDate + "',103),'{1}', '{2}', {3}, '{4}', '{5}', '{6}', '{7}', '{8}', '{9}','{10}' ,'{11}', '{12}', '{13}', '{14}', '{15}',{16}, {17}, {18}, {19}, {20}) SELECT SCOPE_IDENTITY()",
                        dbName, ProductID, MemberID, Quantity, OriginalOrderID, identity, 1, catalogicPrice,
                        irgunPriceFormula, varDiscountFormula, varPriceFormula, varComissionFormula, metaData, cancelComission, isCampaign, GroupLimitsID, MarketingCommission, paymentID, coins, (orderId.HasValue ? orderId.Value.ToString() : "NULL"), PremiumType);

                    identityForBack = Convert.ToInt64(DataBase.ExecuteScalarForTransaction(query, command));

                }
            }
            else
                identityForBack = identity;


            if (!string.IsNullOrEmpty(asmchta))
            {
                if (type != "SPA")
                    type = type.Trim().Remove(type.Length - 1);
                asmchta = asmchta.Substring(1);
                string[] allAamchta = asmchta.Split(',');
                for (int i = 0; i < allAamchta.Length; i++)
                {
                    DataBase.ExecuteNonQueryForTransaction("INSERT INTO " + dbName + ".." + type
                        + "OrdersToWebOrders (WebOrderAsmachta ," + type + "OrderAsmachta) VALUES (" + identityForBack + ","
                        + Convert.ToInt64(allAamchta[i]) + ")", command);
                }
            }

            transaction.Commit();

            //Check transaction success
            if (identityForBack >= 100)
            {
                if (IsManagmentStockEnabled)
                {
                    //Update stock
                    var updateStock = mApiClient.UpdateStock(checkStockObject);

                    //אזל המלאי
                    if (checkStock.Code != BaseApiResult.HTTPResponseCode.Success ||
                        checkStock.VariantResult.First().ResultCode != VariantResult.ResultCodeEnum.SUCCESS)
                    {
                        return "95";
                    }
                }
            }
            return identityForBack.ToString().Trim();
        }
        catch (Exception ex)
        {
            SendMail("Function -> InsertNewOrder (Before Commit) \n dbName -> " + dbName + "\n ProductID -> " + ProductID
                + "\n Type OF Order -> " + type + "\n MemberID -> " + MemberID + "\n Quantity -> " + Quantity + "\n OriginalOrderID -> "
                + OriginalOrderID + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message + "\n ex.Source -> " + ex.Source + "\n ex.StackTrace -> " + ex.StackTrace);
            try
            {
                transaction.Rollback();
                //DataBase.OpenConnectionAgain();
                //DataBase.Rollback();
            }
            catch (Exception ex1)
            {
                SendMail("Function -> InsertNewOrder (Rollback) \n dbName -> " + dbName + "\n ProductID -> " + ProductID
                + "\n Type OF Order -> " + type + "\n MemberID -> " + MemberID + "\n Quantity -> " + Quantity + "\n OriginalOrderID -> "
                + OriginalOrderID + "\n Exception TYPE -> " + ex1.GetType() + "\n Exception Message -> " + ex1.Message);
            }
            if (ex is SqlException)
            {
                LoggerHelper.Error("SQL EXCEPTION ACCURED : " + ((SqlException)ex).Message + " INNER EXCEPTION : " + ((SqlException)ex).InnerException);
                if (((SqlException)ex).Number == -2)
                    return "98";
            }
            return "19";
        }
        finally
        {
            if (command.Connection.State != ConnectionState.Closed)
                command.Connection.Close();
        }
        return identityForBack.ToString().Trim();
    }


    //    internal static string InsertNewOrder(string dbName, string type, string MemberID, string ProductID, string Quantity,
    //     string OriginalOrderID, string cardNumber, string Reason, bool insertCard, string metaData, string catalogicPrice,
    //     string irgunPriceFormula, string varDiscountFormula, string varPriceFormula, string varComissionFormula, string cancelComission,
    //     bool isCampaign, int GroupLimitsID, string MarketingCommission, string paymentID, string lastImplementationDate)
    //    {
    //        if (type == "ANA")
    //            type = "TZIMERS";
    //        long identityForBack = 0, identity = 0;
    //        string OrderDate = Convert.ToString(System.DateTime.Now);
    //        int quant = int.Parse(Quantity);
    //        int OrderQuntity = -101;
    //        string asmchta = null;
    //        SqlDataReader reader = null;

    //        SqlCommand command = new SqlCommand();
    //        SqlTransaction transaction = null;
    //        try
    //        {
    //            //DataBase.BeginTransaction();
    //            DataBase.BeginTransaction(ref transaction, command);
    //            if (type == "ATRACTIONS" || type == "CLOSE_DAYS")
    //            {
    //                SqlConnection connection = new SqlConnection();
    //                string query;
    //                //הפרדת שורות בין הזמנות לאותו הוריאנט
    //                //BUG 1183 : ATRACTIONSOrders לא לחבר הזמנות בטבלת 
    //                //                if (quant > 0)
    //                //                {
    //                //                    query = string.Format(@"SELECT MemberOrderQuntity,MemberOrderAsmchta  FROM {0}..{1}Orders 
    //                //                        inner join {0}..productsVars on fullbarcode = barcode 
    //                //                        WHERE MemberID= '{2}' AND BarCode = '{3}' AND MemberOrderBlance = 0 AND SplitOrder <>  1 
    //                //                        AND MemberOrderQuntity >= 0 And {0}..{1}Orders.LastImplementationDate = '{4}' "
    //                //                        , dbName, type, MemberID, ProductID, lastImplementationDate);
    //                //                }
    //                //else
    //                if (quant < 0)
    //                {
    //                    XmlDocument xmlDoc = new XmlDocument();
    //                    if (!string.IsNullOrEmpty(metaData))
    //                        xmlDoc.LoadXml(metaData);
    //                    XmlNode xmlNnode = xmlDoc.SelectSingleNode("Details/Asmachta");
    //                    string orderAsmchta = string.Empty;
    //                    if (xmlNnode != null && !string.IsNullOrEmpty(xmlNnode.InnerText))
    //                        orderAsmchta = xmlNnode.InnerText;

    //                    if (string.IsNullOrEmpty(orderAsmchta))
    //                    {//במקרה שהזיכוי נשלח מפונקציית השירות של הארגונים ללא אסמכתא
    //                        query = string.Format(@"SELECT MemberOrderQuntity,MemberOrderAsmchta  FROM {0}..{1}Orders 
    //                                                WHERE MemberID= '{2}' AND BarCode = '{3}' AND MemberOrderBlance = 0 
    //                                                AND MemberOrderQuntity >= 0
    //                                                And isnull(LastImplementationDate, '2099-12-31') > GetDate() ",
    //                                                                dbName, type, MemberID, ProductID);
    //                    }
    //                    else
    //                    {//במקרה שהזיכוי נשלח מפונקציית הביטול במערכת הניהול כולל אסמכתא ייחודית לביטול
    //                        query = string.Format(@"SELECT MemberOrderQuntity,MemberOrderAsmchta  FROM {0}..{1}Orders 
    //                                                WHERE MemberID= '{2}' AND BarCode = '{3}' AND MemberOrderBlance = 0 
    //                                                AND MemberOrderQuntity >= 0 And MemberOrderAsmchta = '{4}'",
    //                                                                dbName, type, MemberID, ProductID, orderAsmchta);
    //                    }
    //                    //}

    //                    reader = DataBase.ExecuteReader(query, connection);
    //                    while (reader.Read())
    //                    {
    //                        OrderQuntity = Convert.ToInt32(reader["MemberOrderQuntity"]);
    //                        identity = Convert.ToInt64(reader["MemberOrderAsmchta"]);
    //                        if (quant < 0 && OrderQuntity >= Math.Abs(quant))
    //                            break;
    //                    }
    //                    if (reader != null)
    //                    {
    //                        if (!reader.IsClosed)
    //                            reader.Close();
    //                        if (connection.State != ConnectionState.Closed)
    //                            connection.Close();
    //                    }
    //                }
    //                if (OrderQuntity == -101 && identity == 0)
    //                {
    //                    if (quant < 1)
    //                        return "23";
    //                    if (!insertCard)
    //                    {
    //                        identity = Convert.ToInt64(
    //                            DataBase.ExecuteScalarForTransaction("INSERT INTO " + dbName + ".." + type
    //                                    + @"Orders (OrderDate,MemberID ,BarCode ,MemberOrderDateEXE ,MemberOrderQuntity ,MemberOrderBlance, LastImplementationDate) 
    //                                   
    //                                    VALUES (convert(datetime,'" + OrderDate + "',103),'" + MemberID
    //                                    + "', '" + ProductID + "', '01/01/1900', " + Quantity + ", 0 , '" + lastImplementationDate + "') SELECT SCOPE_IDENTITY()", command));

    //                    }
    //                    else
    //                    {
    //                        identity = Convert.ToInt64(
    //                            DataBase.ExecuteScalarForTransaction("INSERT INTO " + dbName + ".." + type
    //                                    + "Orders (MemberID ,BarCode ,MemberOrderDateEXE ,MemberOrderQuntity ,MemberOrderBlance,CardNumber, LastImplementationDate) VALUES ('" + MemberID
    //                                    + "', '" + ProductID + "', '01/01/1900', " + Quantity + ", 0, '" + cardNumber + "', '" + lastImplementationDate + "') SELECT SCOPE_IDENTITY()", command));

    //                    }
    //                }
    //                else
    //                {
    //                    OrderQuntity = OrderQuntity + quant;
    //                    if (OrderQuntity < 0)
    //                        return "23";
    //                    if (identity != 0)
    //                    {
    //                        int rowUp = Convert.ToInt32(
    //                            DataBase.ExecuteNonQueryForTransaction(
    //                            "UPDATE " + dbName + ".." + type + "Orders SET MemberOrderQuntity = "
    //                            + OrderQuntity + " WHERE MemberID = '" + MemberID + "' AND BarCode = '" + ProductID + "' AND MemberOrderAsmchta = '" + identity
    //                            + "' AND MemberOrderBlance = 0 AND MemberOrderQuntity >= 0", command));
    //                        if (rowUp < 1)
    //                        {
    //                            SendMail("Function -> InsertNewOrder (Cannot Do Update) \n dbName -> " + dbName + "\n ProductID -> " + ProductID
    //                            + "\n Type OF Order -> " + type + "\n MemberID -> " + MemberID + "\n Quantity -> " + Quantity + "\n OriginalOrderID -> "
    //                            + OriginalOrderID + "\nIdentity -> " + identity);
    //                            transaction.Rollback();
    //                            return "0";
    //                            //identity = Convert.ToInt64(DataBase.ExecuteScalar("SELECT MAX(MemberOrderAsmchta) FROM " + dbName + ".." + type
    //                            //    + "Orders WHERE BarCode = '" + ProductID + "' AND MemberOrderBlance = 0  AND MemberId = " + MemberID));
    //                        }
    //                    }
    //                    else
    //                    {
    //                        SendMail("Function -> InsertNewOrder (Identity Is Null) \n dbName -> " + dbName + "\n ProductID -> " + ProductID
    //                            + "\n Type OF Order -> " + type + "\n MemberID -> " + MemberID + "\n Quantity -> " + Quantity + "\n OriginalOrderID -> "
    //                            + OriginalOrderID + "\nIdentity -> " + identity);
    //                        transaction.Rollback();
    //                        return "0";
    //                    }
    //                }
    //            }
    //            else if (type == "BUY_MONEY")
    //            {
    //                decimal var = Convert.ToDecimal(
    //                    DataBase.ExecuteScalarForTransaction("SELECT shortNameVar FROM " + dbName
    //                    + "..productsVars WHERE FullBarCode = '" + ProductID + "'", command));
    //                var = var * quant;
    //                if (Math.Abs(var) > 3000)
    //                    return "20";
    //                //string cardNumber = (string)DataBase.ExecuteScalar("SELECT CardNumber FROM " + dbName + "..Cards WHERE IDMember = '" + MemberID + "' AND CardStatus = 1");
    //                if (quant > 0)
    //                {
    //                    identity = Convert.ToInt64(
    //                        DataBase.ExecuteScalarForTransaction("INSERT INTO " + dbName
    //                        + "..Requests (RequestTime,RequestSource,RequestOp,RequestType,RequestStatus,ID1,Card1,Amount,Remark,ReasonCode) VALUES (GETDATE(),5,0,6,3,'"
    //                        + MemberID + "', '" + cardNumber + "', " + var + ", '" + OriginalOrderID + "','" + Reason + "') SELECT SCOPE_IDENTITY()", command));
    //                }
    //                else
    //                {
    //                    identity = Convert.ToInt64(
    //                        DataBase.ExecuteScalarForTransaction("INSERT INTO " + dbName
    //                        + "..Requests (RequestTime,RequestSource,RequestOp,RequestType,RequestStatus,ID1,Card1,Amount,Remark,ReasonCode) VALUES (GETDATE(),5,0,6,4,'"
    //                        + MemberID + "', '" + cardNumber + "', " + var + ", '" + OriginalOrderID + "','" + Reason + "') SELECT SCOPE_IDENTITY()", command));
    //                }
    //            }
    //            else
    //            {
    //                if (dbName != "HonorReleases" && dbName != "Miluim" && dbName != "Shufersal"
    //                    && dbName != "HonorReleasesTest" && dbName != "MiluimTest" && dbName != "ShufersalTest"
    //                    && dbName != "MiluimDevelopment" && dbName != "ReleasesDevelopment" && dbName != "ShufersalDevelopment" &&
    //                    dbName != "Hot" && dbName != "LeumiCard" && dbName != "Yes")
    //                {
    //                    if (type == "MOVIES")
    //                    {
    //                        int movieCount = Convert.ToInt32(
    //                            DataBase.ExecuteScalarForTransaction("SELECT BusinessShortName FROM " + dbName
    //                            + "..productsVars WHERE FullBarCode = '" + ProductID + "'", command));
    //                        quant = quant * movieCount;
    //                    }
    //                }
    //                if (quant > 0)
    //                {
    //                    for (int i = 0; i < quant; i++)
    //                    {
    //                        identity = Convert.ToInt64(
    //                            DataBase.ExecuteScalarForTransaction("INSERT INTO " + dbName + ".." + type + "Orders "
    //                            + @" (OrderDate,MemberId,BarCode,OrderDateExe,OrderQuantity,OrderBalance, LastImplementationDate) 
    //                                VALUES (convert(datetime,'" + OrderDate + "',103),'" + MemberID + "', '"
    //                            + ProductID + "', '01/01/1900', 1, 0, '" + lastImplementationDate + "') SELECT SCOPE_IDENTITY()", command));
    //                        asmchta += "," + identity;
    //                    }
    //                }
    //                else
    //                {
    //                    int sumOrders = 0;
    //                    object sumOrderFromTable = DataBase.ExecuteScalarForTransaction(
    //                        "SELECT SUM(OrderQuantity) as OrderQuantity FROM " + dbName + ".." + type
    //                        + "Orders WHERE MemberId = '" + MemberID + "' AND BarCode = '" + ProductID + "' AND OrderBalance = 0", command);
    //                    if (sumOrderFromTable != DBNull.Value)
    //                        sumOrders = Convert.ToInt32(sumOrderFromTable);
    //                    sumOrders = sumOrders + quant;
    //                    if (sumOrders < 0)
    //                        return "23";
    //                    for (int i = quant + 1; i <= 0; i++)
    //                    {
    //                        identity = Convert.ToInt64(
    //                            DataBase.ExecuteScalarForTransaction("SELECT MAX(OrderAsmchta) AS OrderAsmchta FROM " + dbName + ".." + type
    //                            + "Orders WHERE (MemberId = '" + MemberID + "' AND BarCode = '" + ProductID + "' AND OrderBalance = 0 AND OrderQuantity > 0)", command));
    //                        int rowUp = Convert.ToInt32(
    //                            DataBase.ExecuteNonQueryForTransaction("UPDATE " + dbName + ".." + type
    //                            + "Orders SET OrderQuantity = 0 WHERE (MemberId = '" + MemberID + "' AND OrderAsmchta = '" + identity + "')", command));
    //                        asmchta += "," + identity;
    //                    }
    //                }
    //            }

    //            if (identity == 0)
    //            {
    //                try
    //                {
    //                    //DataBase.OpenConnectionAgain();
    //                    //DataBase.Rollback();
    //                    transaction.Rollback();
    //                    return "0";
    //                }
    //                catch (Exception ex)
    //                {
    //                    SendMail("Function -> identity == 0 (Rollback) \n dbName -> " + dbName + "\n ProductID -> " + ProductID
    //                    + "\n Type OF Order -> " + type + "\n MemberID -> " + MemberID + "\n Quantity -> " + Quantity + "\n OriginalOrderID -> "
    //                    + OriginalOrderID + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
    //                }
    //            }

    //            if (type != "BUY_MONEY")
    //            {
    //                if (dbName == "LeumiCard_GCPoints")
    //                {//בתוספת רישום לתגית XMLDATA
    //                    identityForBack = Convert.ToInt64(
    //                        DataBase.ExecuteScalarForTransaction("INSERT INTO " + dbName + @"..WebServiceTransaction 
    //                        (TTransactionDateTime,TTransactionProductID,TTransactionMemberID,TTransactionquantity,TTransactionMetaData,TTransactionOrder,TTransactionStatus,XMLParam) 
    //                        VALUES (convert(datetime,'" + OrderDate + "',103), '" + ProductID + "', '" + MemberID + "', " + Quantity
    //                                                              + ",'" + OriginalOrderID + "','" + identity + "',1,'" + metaData + "') SELECT SCOPE_IDENTITY()", command));
    //                }
    //                else
    //                {
    //                    string query = string.Format(@"INSERT INTO {0}..WebServiceTransaction 
    //                    (TTransactionDateTime, TTransactionProductID,TTransactionMemberID, TTransactionquantity,
    //                    TTransactionMetaData, TTransactionOrder, TTransactionStatus,CatalogicPrice, IrgunPrice,
    //                    CustomerDiscount, CustomerPrice, DistributionComission, XMLParam, CancelCommission,ISCampaign,GroupLimitsID,MarketingCommission, PaymentID) 
    //                    VALUES (convert(datetime,'" + OrderDate + "',103),'{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}','{10}' ,'{11}', '{12}', '{13}', '{14}', '{15}',{16}, '{17}') SELECT SCOPE_IDENTITY()",
    //                    dbName, ProductID, MemberID, Quantity, OriginalOrderID, identity, 1, catalogicPrice,
    //                    irgunPriceFormula, varDiscountFormula, varPriceFormula, varComissionFormula, metaData, cancelComission, isCampaign, GroupLimitsID, MarketingCommission, paymentID);

    //                    identityForBack = Convert.ToInt64(DataBase.ExecuteScalarForTransaction(query, command));
    //                }
    //            }
    //            else
    //                identityForBack = identity;


    //            if (!string.IsNullOrEmpty(asmchta))
    //            {
    //                if (type != "SPA")
    //                    type = type.Trim().Remove(type.Length - 1);
    //                asmchta = asmchta.Substring(1);
    //                string[] allAamchta = asmchta.Split(',');
    //                for (int i = 0; i < allAamchta.Length; i++)
    //                {
    //                    DataBase.ExecuteNonQueryForTransaction("INSERT INTO " + dbName + ".." + type
    //                        + "OrdersToWebOrders (WebOrderAsmachta ," + type + "OrderAsmachta) VALUES (" + identityForBack + ","
    //                        + Convert.ToInt64(allAamchta[i]) + ")", command);
    //                }
    //            }

    //            transaction.Commit();
    //            return identityForBack.ToString().Trim();
    //        }
    //        catch (Exception ex)
    //        {
    //            SendMail("Function -> InsertNewOrder (Before Commit) \n dbName -> " + dbName + "\n ProductID -> " + ProductID
    //                + "\n Type OF Order -> " + type + "\n MemberID -> " + MemberID + "\n Quantity -> " + Quantity + "\n OriginalOrderID -> "
    //                + OriginalOrderID + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message + "\n ex.Source -> " + ex.Source + "\n ex.StackTrace -> " + ex.StackTrace);
    //            try
    //            {
    //                transaction.Rollback();
    //                //DataBase.OpenConnectionAgain();
    //                //DataBase.Rollback();
    //            }
    //            catch (Exception ex1)
    //            {
    //                SendMail("Function -> InsertNewOrder (Rollback) \n dbName -> " + dbName + "\n ProductID -> " + ProductID
    //                + "\n Type OF Order -> " + type + "\n MemberID -> " + MemberID + "\n Quantity -> " + Quantity + "\n OriginalOrderID -> "
    //                + OriginalOrderID + "\n Exception TYPE -> " + ex1.GetType() + "\n Exception Message -> " + ex1.Message);
    //            }
    //            return "19";
    //        }
    //        finally
    //        {
    //            if (command.Connection.State != ConnectionState.Closed)
    //                command.Connection.Close();
    //        }
    //        return identityForBack.ToString().Trim();
    //    }


    internal static string ExtractAsmachtaFromXml(string metaData)
    {
        XmlDocument xmlDoc = new XmlDocument();
        if (!string.IsNullOrEmpty(metaData))
            xmlDoc.LoadXml(metaData);
        XmlNode xmlNnode = xmlDoc.SelectSingleNode("Details/Asmachta");
        string orderAsmchta = string.Empty;
        if (xmlNnode != null && !string.IsNullOrEmpty(xmlNnode.InnerText))
            orderAsmchta = xmlNnode.InnerText;
        return orderAsmchta;
    }

    internal static DateTime? ExtractDateOrderFromXml(string metaData)
    {
        DateTime? OrderDate;
        XmlDocument xmlDoc = new XmlDocument();
        if (!string.IsNullOrEmpty(metaData))
            xmlDoc.LoadXml(metaData);
        XmlNode xmlNnode = xmlDoc.SelectSingleNode("Details/OrderDate");//MemberTerminalExe
        string orderAsmchta = string.Empty;
        if (xmlNnode != null && !string.IsNullOrEmpty(xmlNnode.InnerText))
            orderAsmchta = xmlNnode.InnerText;
        try
        {

            OrderDate = Convert.ToDateTime(orderAsmchta);
        }
        catch (Exception)
        {
            OrderDate = null;


        }

        return OrderDate;
    }

    internal static string ExtractTerminalExeFromXml(string metaData)
    {
        XmlDocument xmlDoc = new XmlDocument();
        if (!string.IsNullOrEmpty(metaData))
            xmlDoc.LoadXml(metaData);
        XmlNode xmlNnode = xmlDoc.SelectSingleNode("Details/MemberTerminalExe");//MemberTerminalExe
        string MemberTerminalExe = string.Empty;
        if (xmlNnode != null && !string.IsNullOrEmpty(xmlNnode.InnerText))
            MemberTerminalExe = xmlNnode.InnerText;

        return MemberTerminalExe;
    }


    internal static string IsTheLineWasInsertBefore(string dbName, string OriginalOrderID, string type)
    {
        string back = null;
        try
        {
            if (type != "BUY_MONEY")
            {
                back = (string)DataBase.ExecuteScalar("IF EXISTS (SELECT TTransactionID FROM " + dbName
                    + "..WebServiceTransaction WHERE TTransactionMetaData = '" + OriginalOrderID + "') SELECT '11' ELSE SELECT '0'");
            }
            else
            {

                if (dbName.ToUpper() == "ITU")
                    back = (string)DataBase.ExecuteScalar("IF EXISTS (SELECT RequestID FROM " + dbName
                                                           + "..Requests WHERE RemarkIdx = '" + OriginalOrderID +
                                                           "') SELECT '11' ELSE SELECT '0'");
                else
                    back = (string)DataBase.ExecuteScalar("IF EXISTS (SELECT RequestID FROM " + dbName
                                                           + "..Requests WHERE Remark Like '" + OriginalOrderID +
                                                           "') SELECT '11' ELSE SELECT '0'");
            }
            return back;
        }
        catch (Exception ex)
        {
            SendMail("Function -> IsTheLineWasInsertBefore \n dbName -> " + dbName + "\n OriginalOrderID -> " + OriginalOrderID
                + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
    }



    internal static string CheckCardStatusForCardNumber(string dbName, string MemberID)
    {
        object back = null;
        try
        {
            back = DataBase.ExecuteScalar("SELECT CardNumber FROM " + dbName + "..Cards WHERE (IDMember = '" + MemberID + "') AND (CardStatus = 1)");
            if (back != null)
                return back.ToString();
            back = DataBase.ExecuteScalar("SELECT CardStatus FROM " + dbName + "..Cards WHERE (IDMember = '" + MemberID + "')  AND (CardStatus = 0)");
            if (back != null)
                return "18";
            back = DataBase.ExecuteScalar("SELECT CardStatus FROM " + dbName + "..Cards WHERE (IDMember = '" + MemberID + "')");
            if (back != null)
                return "17";
            return "26";
        }
        catch (Exception ex)
        {
            SendMail("Function -> CheckCardStatusForCardNumber \n dbName -> " + dbName + "\n MemberID -> " + MemberID
                + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
    }

    //internal static DataTable GetAllReasonForDBName(string dbName)
    //{
    //    //SqlDataReader reader = null;
    //    DataTable dt = null;
    //    try
    //    {
    //        dt = DataBase.FillDataTable("SELECT Code FROM " + dbName + "..RechargeResones WHERE IsActive = 1", "Codes");
    //        //reader = DataBase.ExecuteReader("SELECT Code FROM " + dbName + "..RechargeResones WHERE IsActive = 1");
    //        //if (reader != null)
    //        //    dt.Load(reader);
    //    }
    //    catch (Exception ex)
    //    {
    //        SendMail("Function -> GetAllReasonForDBName \n dbName -> " + dbName
    //            + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message + "\n ex.Source -> " + ex.Source + "\n ex.StackTrace -> " + ex.StackTrace);
    //        //return "19";
    //    }
    //    finally
    //    {
    //        //if (!reader.IsClosed)
    //        //    reader.Close();
    //    }
    //    return dt;
    //}

    //internal static DataTable GetAllReasonForDBName(string dbName)
    //{
    //    SqlDataReader readerReason = null;
    //    DataTable dt = null;
    //    try
    //    {
    //        readerReason = DataBase.ExecuteReader("SELECT Code FROM " + dbName + "..RechargeResones WHERE IsActive = 1");
    //        if (readerReason != null)
    //            if (readerReason.HasRows)
    //                dt.Load(readerReason);
    //    }
    //    catch (Exception ex)
    //    {
    //        SendMail("Function -> GetAllReasonForDBName \n dbName -> " + dbName
    //            + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message + "\n ex.Source -> " + ex.Source + "\n ex.StackTrace -> " + ex.StackTrace);
    //        //return "19";
    //    }
    //    finally
    //    {
    //        if (readerReason != null)
    //        {
    //            if (!readerReason.IsClosed)
    //                readerReason.Close();
    //        }
    //    }
    //    return dt;
    //}

    internal static Int16 IFExistsReason(string dbName, string reason)
    {
        Int16 reasonExists = 25;
        try
        {
            reasonExists = Convert.ToInt16(DataBase.ExecuteScalar("IF Exists "
                + "(Select * From " + dbName + "..RechargeResones Where Code = '" + reason + "') "
                + "Select '1' Else Select '25'"));
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetAllReasonForDBName \n dbName -> " + dbName
                + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message + "\n ex.Source -> " + ex.Source + "\n ex.StackTrace -> " + ex.StackTrace);
            return 19;
        }
        return reasonExists;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    internal static string validateActivation(string dbName, string MemberID, string cardNumber,
        string memberIsraeliID, string last4Digits)
    {
        //SqlDataReader reader = null;
        DataTable dataTable = new DataTable();

        try
        {
            string query = string.Format(@"SELECT TZ, Last4Digits, CardStatus  FROM {0}..AllMembers INNER JOIN {0}..Cards ON 
                            {0}..AllMembers.MemberId = {0}..Cards.IDMember WHERE  ({0}..AllMembers.MemberId = '{1}') 
                                AND ({0}..Cards.CardNumber = '{2}') and (tz = '{3}'  or Last4Digits='{4}')",
                                                       dbName, MemberID, cardNumber, memberIsraeliID, last4Digits);

            //reader = DataBase.ExecuteReader(query);
            dataTable = DataBase.FillDataTable(query, "CustomerDetail");
            //DataBase.ExecuteAdapter(query, ref dataTable);

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return "26"; // card or member id was not found 
            }
            DataRow row = dataTable.Rows[0];

            if (Convert.ToString(row["CardStatus"]) == "1") // card already activate 
                return "35";
            if (Convert.ToString(row["CardStatus"]) == "2") // card Blocked 
                return "17";

            if ((!string.IsNullOrEmpty(memberIsraeliID) && memberIsraeliID == Convert.ToString(row["TZ"])) ||
                (!string.IsNullOrEmpty(last4Digits) && last4Digits == Convert.ToString(row["Last4Digits"])))
            {
                if (memberHaveAnotherCard(dbName, MemberID, cardNumber))
                    return "41";
                else
                    return "32";
            }
        }

        catch (Exception ex)
        {
            SendMail("Function -> validateActivation \n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        finally
        {
            //if (reader != null)
            //    reader.Close();
        }

        return "36"; // wrong memberIsraeliID and/or wrong last4Digits
    }

    private static bool memberHaveAnotherCard(string dbName, string MemberID, string cardNumber)
    {
        string result;
        string query = string.Format(@"if exists (select IDMember from {0}..Cards where 
            (CardNumber <> '{1}' and IDMember = '{2}') ) select 1 else select 2", dbName, cardNumber, MemberID);

        result = DataBase.ExecuteScalar(query).ToString();
        if (result == "1")
            return true;
        else
            return false;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // activate the card and save all additional information
    internal static string activate(string dbName, string MemberID, string cardNumber, StringBuilder customerDetails, string ipUserHost)
    {

        string activateQuery = null;

        activateQuery = "UPDATE " + dbName + "..Cards " + "SET CardStatus = '1', ActivationTime = GETDATE(), " +
                "ActivationIP = '" + ipUserHost + "'" +
                " where CardNumber = '" + cardNumber + "'";

        try
        {
            DataBase.ExecuteNonQuery(activateQuery); // activate the card
            DataBase.ExecuteNonQuery(customerDetails.ToString()); // save additional details about the member
        }

        catch (Exception ex)
        {
            SendMail("Function -> validateActivation \n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }

        return "38"; // card activation end successfully
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // write the toekn and the time it was created to organizations table
    // and return the number of rows affected.
    internal static string writeTokenToDB(string token, string organizationID)
    {
        int numOfRowsAffected = 0;

        string updateQuery = "UPDATE Organizations SET token = '" + token + "', tokenCreatedTime = GETDATE() " +
                             "WHERE  OrganizationID = '" + organizationID + "'";
        try
        {
            numOfRowsAffected = DataBase.ExecuteNonQuery(updateQuery);
        }

        catch (Exception ex)
        {
            SendMail("Function -> writeTokenToDB \n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        return numOfRowsAffected.ToString();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    internal static string getCampaigns(string organizationID, string category)
    {
        XmlDocument xmlDoc = new XmlDocument();
        DataTable dataTable = new DataTable();
        string campaignLimit = "";
        if (!string.IsNullOrEmpty(category))//For Leumi
            campaignLimit = ",CampaignLimit, ImgUrl, CampaigNameRequired ";

        //        string campaignQuery = @"SELECT Campaigns.CampaignID, Campaigns.CampaignName, Campaigns.CreationTime, 
        //                                    Campaigns.CampaignStartTime, Campaigns.CampaignEndTime " + campaignLimit +
        //                                @"FROM  Campaigns  
        //	                                WHERE (Campaigns.CampaignStatus = '2') 
        //                                        and datediff(d,getDate(),  Campaigns.CampaignEndTime) >= 0
        //                                        and datediff(d, Campaigns.CampaignStartTime, getdate()) >= 0
        //                                        and (Campaigns.OrganizationID = '" + organizationID + "')";

        string campaignQuery = string.Format(
                  @"SELECT Campaigns.CampaignID, Campaigns.CampaignName, Campaigns.CreationTime, 
                                    Campaigns.CampaignStartTime, Campaigns.CampaignEndTime {0}
                                FROM  Campaigns  
	                                WHERE (Campaigns.CampaignStatus = '2') 
                                    and datediff(d,getDate(),  Campaigns.CampaignEndTime) >= 0
                                    and datediff(d, Campaigns.CampaignStartTime, getdate()) >= 0
                                    and (Campaigns.OrganizationID = '{1}')", campaignLimit, organizationID);

        if (!string.IsNullOrEmpty(category))//For Leumi
            campaignQuery += " and (Campaigns.CategoryID = " + category + ") Order By cast(CampaignName as nvarchar(max))";

        try
        {
            // init SqlDataAdapter with select command and connection
            dataTable = DataBase.FillDataTable(campaignQuery, "campaign");

            //DataBase.ExecuteAdapter(campaignQuery, ref dataTable);

            MemoryStream ms = new MemoryStream();

            dataTable.WriteXml(ms);
            ms.Position = 0;
            StreamReader sReader = new StreamReader(ms, System.Text.Encoding.UTF8);

            xmlDoc.Load(sReader);

            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList nodeList = root.ChildNodes;
            XmlDocumentFragment frag = xmlDoc.CreateDocumentFragment();
            string campaignID = "";
            string benefits = "";

            foreach (XmlNode node in nodeList)
            {
                campaignID = node.SelectSingleNode("CampaignID").InnerText.Trim();
                benefits = getCampaignBenefits(campaignID, organizationID);
                benefits = benefits.Replace("DocumentElement", "Benefits");
                frag.InnerXml = benefits;
                node.AppendChild(frag);
            }
        }
        catch (Exception ex)
        {
            SendMail("Function -> getCampaigns \n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        return xmlDoc.OuterXml;
    }

    internal static string getCampaignBenefits(string campaginID, string organizationID)
    {
        XmlDocument xmlDoc = new XmlDocument();

        DataTable dt = new DataTable();
        string query = GetBenefitsQuery(campaginID, organizationID);



        dt = DataBase.FillDataTable(query, "benefit");
        //DataBase.ExecuteAdapter(query, ref dt);
        MemoryStream ms = new MemoryStream();
        dt.WriteXml(ms);
        ms.Position = 0;
        StreamReader sReader = new StreamReader(ms, System.Text.Encoding.UTF8);

        xmlDoc.Load(sReader);

        XmlNodeList nodeList = xmlDoc.DocumentElement.ChildNodes;
        XmlDocumentFragment frag = xmlDoc.CreateDocumentFragment();

        string benefitID = "";
        foreach (XmlNode node in nodeList)
        {
            benefitID = node.SelectSingleNode("BenefitID").InnerText.Trim();
            frag.InnerXml = getBenefitConditions(benefitID);
            node.AppendChild(frag);
        }
        return xmlDoc.OuterXml;
    }

    private static string GetBenefitsQuery(string campaginID, string organizationID)
    {
        //אוסף את כל ההטבות לקמפיין
        if (organizationID != "28")
        {
            return
                @"SELECT   CampaignBenefits.BenefitID, CampaignBenefits.BenefitName, CampaignBenefits.benefitParams, CampaignBenefits.BenefitGeneralLimit, 
                        CampaignBenefits.BenefitPersonLimit, CampaignBenefits.BenefitAccountLimit, 
                        CampaignBenefitTypes.BenefitTypeName, Business.StoreName
                        FROM  CampaignBenefits INNER JOIN CampaignBenefitTypes 
                        ON CampaignBenefits.BenefitType = CampaignBenefitTypes.BenefitTypeID INNER JOIN Business 
                        ON CAST (CampaignBenefits.MerchantID AS nvarchar(50)) = Business.BuisnessID 
                        WHERE   
                        (CampaignBenefits.BenefitStatus = '1') and (CampaignBenefits.CampaignID = '" + campaginID + "')" +
                @"union All 
                        SELECT   CampaignBenefits.BenefitID, CampaignBenefits.BenefitName, CampaignBenefits.benefitParams, CampaignBenefits.BenefitGeneralLimit, 
                        CampaignBenefits.BenefitPersonLimit, CampaignBenefits.BenefitAccountLimit, 
                        CampaignBenefitTypes.BenefitTypeName, Merchants.MerchantName
                        FROM  CampaignBenefits INNER JOIN CampaignBenefitTypes 
                        ON CampaignBenefits.BenefitType = CampaignBenefitTypes.BenefitTypeID INNER JOIN Merchants 
                        ON CampaignBenefits.MerchantID = Merchants.MerchantID
                        WHERE   (CampaignBenefits.BenefitStatus = '1') and (CampaignBenefits.CampaignID=' " + campaginID + "')";
        }
        else
        {//לטובת פרוייקט לאומי התווספו פרטים נוספי לשליחה בהטבה
            return
                @"SELECT     CampaignBenefits.BenefitID, CampaignBenefits.BenefitName, CampaignBenefits.BenefitGeneralLimit, 
                        CampaignBenefits.BenefitPersonLimit, CampaignBenefits.BenefitAccountLimit, CampaignBenefitTypes.BenefitTypeName, 
                        business.StoreName, CampaignBenefits.XMLParam, CampaignBenefits.FullDetails, 
                        CampaignBenefits.LimitClient1 as YearlyClientLimit, CampaignBenefits.LimitClient2 as QuarterlyClientLimit, 
                        CampaignBenefits.LimitClient3 as MonthlyClientLimit, CampaignBenefits.LimitClient4 as WeeklyClientLimit, 
                        CampaignBenefits.LimitClient5 as DailyClientLimit, CampaignBenefits.LimitBenefit1 as YearlyBenefitLimit, 
                        CampaignBenefits.LimitBenefit2 as QuarterlyBenefitLimit, CampaignBenefits.LimitBenefit3 as MonthlyBenefitLimit, 
                        CampaignBenefits.LimitBenefit4 as WeeklyBenefitLimit, CampaignBenefits.LimitBenefit5 as DailyBenefitLimit
                        FROM CampaignMerchants INNER JOIN
                        business ON dbo.CampaignMerchants.MerchantID = business.BuisnessID INNER JOIN
                        CampaignBenefits INNER JOIN
                        CampaignBenefitTypes ON CampaignBenefits.BenefitType = CampaignBenefitTypes.BenefitTypeID ON 
                        CampaignMerchants.CampaignID = CampaignBenefits.CampaignID AND 
                        CampaignMerchants.MerchantID = CampaignBenefits.MerchantID
                        WHERE  (CampaignBenefits.BenefitStatus = '1') AND (CampaignBenefits.CampaignID = '" + campaginID + "') AND (CampaignMerchants.MerchantType = 1)" +
                @"union All 
                        SELECT   CampaignBenefits.BenefitID, CampaignBenefits.BenefitName, CampaignBenefits.BenefitGeneralLimit, 
                        CampaignBenefits.BenefitPersonLimit, CampaignBenefits.BenefitAccountLimit, CampaignBenefitTypes.BenefitTypeName, 
                        Merchants.MerchantName, CampaignBenefits.XMLParam, CampaignBenefits.FullDetails, 
                        CampaignBenefits.LimitClient1 as YearlyClientLimit, CampaignBenefits.LimitClient2 as QuarterlyClientLimit, 
                        CampaignBenefits.LimitClient3 as MonthlyClientLimit, CampaignBenefits.LimitClient4 as WeeklyClientLimit, 
                        CampaignBenefits.LimitClient5 as DailyClientLimit, CampaignBenefits.LimitBenefit1 as YearlyBenefitLimit, 
                        CampaignBenefits.LimitBenefit2 as QuarterlyBenefitLimit, CampaignBenefits.LimitBenefit3 as MonthlyBenefitLimit, 
                        CampaignBenefits.LimitBenefit4 as WeeklyBenefitLimit, CampaignBenefits.LimitBenefit5 as DailyBenefitLimit
                        FROM  CampaignBenefits INNER JOIN CampaignBenefitTypes 
                        ON CampaignBenefits.BenefitType = CampaignBenefitTypes.BenefitTypeID INNER JOIN Merchants 
                        ON CampaignBenefits.MerchantID = Merchants.MerchantID
                        WHERE   (CampaignBenefits.BenefitStatus = '1') and (CampaignBenefits.CampaignID=' " + campaginID + "')";
        }
    }

    internal static string getBenefitConditions(string benefitID)
    {
        XmlDocument xmlDoc = new XmlDocument();

        XmlElement allConditions = xmlDoc.CreateElement("conditions");
        XmlElement condition = xmlDoc.CreateElement("condition");
        XmlElement conditionType = xmlDoc.CreateElement("type");
        XmlElement conditionParam = xmlDoc.CreateElement("param");


        string query = @"SELECT  CampaignConditionTypes.ConditionType, CampaignBenefitConditions.XMLParam 
                                                  FROM CampaignBenefitConditions INNER JOIN
                                                  CampaignConditionTypes ON CampaignBenefitConditions.ConditionTypeID = CampaignConditionTypes.ConditionTypeID
                                                    WHERE CampaignBenefitConditions.BenefitID = '" + benefitID + "'";
        //  SqlCommand command = new SqlCommand(query, connection);
        SqlDataReader reader = null;
        SqlConnection connection = new SqlConnection();
        try
        {

            reader = DataBase.ExecuteReader(query, connection);

            while (reader.Read())
            {
                conditionType.InnerText = reader["ConditionType"].ToString().Trim();
                conditionParam.InnerText = reader["XMLParam"].ToString().Trim();
                condition.AppendChild(conditionType);
                condition.AppendChild(conditionParam);
                allConditions.AppendChild(condition);
            }
        }
        catch (Exception ex)
        {
            SendMail("Function -> createConditionElement \n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        finally
        {
            if (reader != null)
                reader.Close();
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }
        return allConditions.OuterXml;
    }

    internal static string getVariants(string DBName, string organizationID)
    {
        DataTable dataTable = new DataTable();

        string query = GetVariantsQuery(DBName, organizationID);

        dataTable = DataBase.FillDataTable(query, "variant");

        //DataBase.ExecuteAdapter(query, ref dataTable);

        MemoryStream ms = new MemoryStream();
        dataTable.WriteXml(ms);
        ms.Position = 0;
        StreamReader sReader = new StreamReader(ms, System.Text.Encoding.UTF8);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(sReader);

        return xmlDoc.OuterXml;
    }

    private static string GetVariantsQuery(string DBName, string organizationID)
    {
        if (organizationID != "4")
        {
            return string.Format(
            @"SELECT {0}..productsVars.FullBarCode, {0}..productsVars.BusinessName, {0}..productsVars.VarName, {0}..productsVars.Type, 
            {0}..productsVars.StartDate, {0}..productsVars.EndDate, Providers.Name 
            FROM {0}..productsVars 
            INNER JOIN Providers ON {0}..productsVars.ProviderId = Providers.Id"
            , DBName);
        }
        else
        {//לטובת לאומי חוז
            return string.Format(
            @"SELECT {0}..productsVars.FullBarCode, {0}..productsVars.BusinessName, {0}..productsVars.VarName, {0}..productsVars.Type, 
            {0}..productsVars.StartDate, {0}..productsVars.EndDate, Providers.Name 
            FROM {0}..productsVars 
            INNER JOIN Providers ON {0}..productsVars.ProviderId = Providers.Id
            WHERE {0}..productsVars.DisabledToOrder = '0'"
            , DBName);
        }
    }



    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // All the function that related to the WS getMemberBenefit()
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    internal static string getMemberCampaigns(string DBName, string organizationID, string memberID)
    {
        XmlDocument xmlDoc = new XmlDocument();
        DataTable dataTable = new DataTable();

        string campaignQuery = @"SELECT Campaigns.CampaignID, Campaigns.CampaignName, Campaigns.CreationTime, 
                                    Campaigns.CampaignStartTime, Campaigns.CampaignEndTime FROM  Campaigns  
	                                WHERE (Campaigns.CampaignStatus = '2') and datediff(d,getDate(),  Campaigns.CampaignEndTime) >= 0
                                    and (Campaigns.OrganizationID = '" + organizationID + "')";
        try
        {
            // init SqlDataAdapter with select command and connection
            dataTable = DataBase.FillDataTable(campaignQuery, "campaign");
            //DataBase.ExecuteAdapter(campaignQuery, ref dataTable);
            MemoryStream ms = new MemoryStream();
            dataTable.WriteXml(ms);
            ms.Position = 0;
            StreamReader sReader = new StreamReader(ms, System.Text.Encoding.UTF8);

            xmlDoc.Load(sReader);

            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList nodeList = root.ChildNodes;
            XmlDocumentFragment frag = xmlDoc.CreateDocumentFragment();
            string campaignID = "";
            string benefits = "";

            foreach (XmlNode node in nodeList)
            {
                campaignID = node.SelectSingleNode("CampaignID").InnerText.Trim();
                benefits = getCampaignBenefitsForMember(DBName, campaignID, memberID);
                benefits = benefits.Replace("DocumentElement", "Benefits");

                frag.InnerXml = benefits;
                node.AppendChild(frag);
            }
        }
        catch (Exception ex)
        {
            SendMail("Function -> getMemberCampaigns \nDBName --> " + DBName + "\norganizationID --> " + organizationID +
                "\nmemberID --> " + memberID + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        return xmlDoc.OuterXml;
    }

    internal static string getCampaignBenefitsForMember(string DBName, string campaginID, string memberID)
    {
        XmlDocument xmlDoc = new XmlDocument();

        DataTable dt = new DataTable();
        string query = @"SELECT   CampaignBenefits.BenefitID, CampaignBenefits.BenefitName, CampaignBenefits.benefitParams, CampaignBenefits.BenefitGeneralLimit, 
                        CampaignBenefits.BenefitPersonLimit, CampaignBenefits.BenefitAccountLimit, 
                        CampaignBenefitTypes.BenefitTypeName, Merchants.MerchantName 
                        FROM  CampaignBenefits INNER JOIN CampaignBenefitTypes 
                        ON CampaignBenefits.BenefitType = CampaignBenefitTypes.BenefitTypeID INNER JOIN Merchants 
                        ON CampaignBenefits.MerchantID = Merchants.MerchantID 
                        WHERE   (CampaignBenefits.BenefitStatus = '1') and (CampaignBenefits.CampaignID=' " + campaginID + "')";

        dt = DataBase.FillDataTable(query, "benefit");
        //DataBase.ExecuteAdapter(query, ref dt);
        MemoryStream ms = new MemoryStream();
        dt.WriteXml(ms);
        ms.Position = 0;
        StreamReader sReader = new StreamReader(ms, System.Text.Encoding.UTF8);

        xmlDoc.Load(sReader);

        XmlNodeList nodeList = xmlDoc.DocumentElement.ChildNodes;
        XmlDocumentFragment frag = xmlDoc.CreateDocumentFragment();
        XmlElement element;
        string benefitID = "";
        string variantBarcode = "";
        string benefitType = "";
        string chareOnMemberCard = "";
        string variantNeeded = "0";
        foreach (XmlNode node in nodeList)
        {
            //check if the member granted for this benefit. if he isn't then remove the node from the xml
            //if (!benefitAllowdForMember(memberID, variantID))
            //    xmlDoc.RemoveChild(node);
            variantNeeded = node.SelectSingleNode("xmluserparam").SelectSingleNode("VariantNeeded").InnerText.Trim();
            if (variantNeeded.Equals("1"))
            {
                element = xmlDoc.CreateElement("onMemberCard");
                variantBarcode = node.SelectSingleNode("xmluserparam").SelectSingleNode("VariantBarcode").InnerText.Trim();
                benefitType = getBenefitType(DBName, variantBarcode);
                chareOnMemberCard = isBenefitCharged(DBName, memberID, variantBarcode, benefitType);
                node.SelectSingleNode("xmluserparam").AppendChild(element);
            }

            benefitID = node.SelectSingleNode("BenefitID").InnerText.Trim();
            frag.InnerXml = getBenefitConditions(benefitID);
            node.AppendChild(frag);
        }
        return xmlDoc.OuterXml;
    }


    internal static string getBenefitType(string DBName, string variantBarcode)
    {
        string result = "";
        string query = "select [type] from " + DBName + "..productsVars where (fullbarcode = '" + variantBarcode + "')";
        try
        {
            result = (string)DataBase.ExecuteScalar(query);

        }
        catch (Exception ex)
        {
            SendMail("Function -> getBenefitType \n barcode: " + variantBarcode + "Exception TYPE -> "
                                                    + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        return result;
    }

    internal static string getMemberVariants(string DBName, string memberID)
    {
        DataTable dataTable = new DataTable();
        XmlDocument xmlDoc = new XmlDocument();
        string query = "SELECT " + DBName + "..productsVars.FullBarCode, " + DBName + "..productsVars.BusinessName, " + DBName + "..productsVars.VarName, " +
            DBName + "..productsVars.Type, " + DBName + "..productsVars.StartDate, " + DBName + "..productsVars.EndDate, Providers.Name " +
            "FROM " + DBName + "..productsVars INNER JOIN Providers ON " + DBName + "..productsVars.ProviderId = Providers.Id ";

        dataTable = DataBase.FillDataTable(query, "variant");

        //DataBase.ExecuteAdapter(query, ref dataTable);
        MemoryStream ms = new MemoryStream();
        dataTable.WriteXml(ms);
        ms.Position = 0;
        StreamReader sReader = new StreamReader(ms, System.Text.Encoding.UTF8);
        xmlDoc.Load(sReader);

        XmlNodeList nodeList = xmlDoc.DocumentElement.ChildNodes;
        XmlElement element;
        string barcode = "";
        string benefitType = "";
        string chareOnMemberCard = "";
        foreach (XmlNode node in nodeList)
        {
            element = xmlDoc.CreateElement("onMemberCard");
            barcode = node.SelectSingleNode("FullBarCode").InnerText.Trim();
            benefitType = node.SelectSingleNode("Type").InnerText.Trim();
            chareOnMemberCard = isBenefitCharged(DBName, memberID, barcode, benefitType);
            element.InnerText = chareOnMemberCard;
            node.AppendChild(element);
        }

        return xmlDoc.OuterXml;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    // Function check if a specific benefit (by variant barcode) is already writen on member card
    // the function returns
    // 1 - if benefitBarcode charged on memberID card and has not been used.
    // 2 - otherwise

    internal static string isBenefitCharged(string DBName, string memberID, string benefitBarcode, string benefitType)
    {
        string result = "";
        string query = "if exists (select MemberOrderQuntity FROM " + DBName + ".." + benefitType + "Orders where (memberid = '" + memberID +
            "') and (barcode = '" + benefitBarcode + "') and (memberorderblance < MemberOrderQuntity))  select '1'   else select '2' ";

        try
        {
            result = (string)DataBase.ExecuteScalar(query);

        }
        catch (Exception ex)
        {
            SendMail("Function -> isBenefitCharged \n member ID: " + memberID + "Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        return result;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    internal static string getEqualMoney(string DBName, string memberID)
    {
        string result = "";
        DataTable dataTable = new DataTable();

        string query = string.Format(@"SELECT TTransactionID, TTransactionProductID, TTransactionDateTime, MemberOrderQuntity, MemberOrderBlance, 
                MemberOrderDateEXE, shortnamevar, BusnessName, ProviderName, Type 
                    FROM {0}..WebServiceTransaction INNER JOIN {0}..v_wIdenticalMoneyPerMen ON 
                    TTransactionOrder = MemberOrderAsmchta where TTransactionMemberID =  '{1}'", DBName, memberID);

        try
        {
            dataTable = DataBase.FillDataTable(query, "equalMoney");
            if (dataTable.Rows.Count > 0)
            {
                MemoryStream ms = new MemoryStream();
                dataTable.WriteXml(ms);
                ms.Position = 0;
                StreamReader sReader = new StreamReader(ms, System.Text.Encoding.UTF8);
                result = sReader.ReadToEnd();
                result = result.Replace("DocumentElement", "EqualMoneyUses");
            }
        }
        catch (Exception ex)
        {
            SendMail("Function -> getEqualMoney \n member ID: " + memberID + "Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        return result;
    }

    public static long? GetCategoryNumberByBarcodeAndDbName(string dbName, string barcode)
    {

        string sql = string.Format("select top 1 * from {0}..CategoryVariants WHERE Barcode='{1}'", dbName, barcode);
        var reader = DataBase.ExecuteReader_DefaultDataBase(sql);

        if (reader == null || !reader.HasRows) return null;
        reader.Read();
        try
        {
            var categoryNumber = reader.GetInt64(reader.GetOrdinal("CategoryNumber"));
            return categoryNumber;
        }
        catch (Exception ex)
        {
            string t = "";
            return null;
        }
    }



    internal static string getCampaignUses(string DBName, string memberID)
    {
        string result = "";
        DataTable dataTable = new DataTable();

        string query = string.Format(@"SELECT {0}..CampaignMembersUse.BenefitID, {0}..CampaignMembersUse.UseTime, " +
        "{0}..CampaignMembersUse.ID, {0}..CampaignMembersUse.NumOfUse, CampaignBenefitTypes.BenefitTypeName, " +
        "CampaignBenefits.BenefitName, CampaignBenefits.BenefitCost, MerchantPOSs.PosName, Merchants.MerchantName, " +
        "MerchantPOSs.PosAddress FROM {0}..CampaignMembersUse INNER JOIN CampaignBenefits ON {0}..CampaignMembersUse.BenefitID = " +
        "CampaignBenefits.BenefitID INNER JOIN CampaignBenefitTypes ON CampaignBenefits.BenefitType = CampaignBenefitTypes.BenefitTypeID " +
        "INNER JOIN MerchantPOSs ON {0}..CampaignMembersUse.POSID = MerchantPOSs.POSID INNER JOIN Merchants ON " +
        "{0}..CampaignMembersUse.MerchantID = Merchants.MerchantID " +
        "WHERE ({0}..CampaignMembersUse.ID = '{1}')" +
        "union ALL " +
        "SELECT {0}..CampaignMembersUse.BenefitID, {0}..CampaignMembersUse.UseTime, {0}..CampaignMembersUse.ID, " +
        "{0}..CampaignMembersUse.NumOfUse, CampaignBenefitTypes.BenefitTypeName, CampaignBenefits.BenefitName, " +
        "CampaignBenefits.BenefitCost, business.StoreName AS PosName, business.StoreName AS MerchantName, " +
        "business.StoreAddress AS PosAddress FROM {0}..CampaignMembersUse INNER JOIN CampaignBenefits ON " +
        "{0}..CampaignMembersUse.BenefitID = CampaignBenefits.BenefitID INNER JOIN CampaignBenefitTypes ON " +
        "CampaignBenefits.BenefitType = CampaignBenefitTypes.BenefitTypeID INNER JOIN business ON CAST " +
        "({0}..CampaignMembersUse.MerchantID AS nvarchar(50)) = business.BuisnessID " +
        "WHERE ({0}..CampaignMembersUse.ID = '{1}')", DBName, memberID);

        try
        {
            dataTable = DataBase.FillDataTable(query, "campagin");
            if (dataTable.Rows.Count > 0)
            {
                MemoryStream ms = new MemoryStream();
                dataTable.WriteXml(ms);
                ms.Position = 0;
                StreamReader sReader = new StreamReader(ms, System.Text.Encoding.UTF8);
                result = sReader.ReadToEnd();
                result = result.Replace("DocumentElement", "Campaigns");
            }
        }
        catch (Exception ex)
        {
            SendMail("Function -> getCampaignUses \n member ID: " + memberID + "Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        return result;
    }


    //internal static string getRequests(string DBName, string memberID)
    //{
    //    string result = "";
    //    DataTable dataTable;

    //    string query = @"SELECT " + DBName + "..requests.RequestID," + DBName + "..requests.RequestTime, " + DBName + "..requests.RequestSource, "
    //        + DBName + "..requests.RequestType," + DBName + "..requests.RequestStatus," + DBName +
    //        "..requests.ReasonCode," + DBName + "..requests.Card1," + DBName + "..requests.Amount FROM "
    //        + DBName + "..requests where (id1 = '" + memberID + "')";

    //    try
    //    {
    //        dataTable = DataBase.FillDataTable(query, "request");
    //        MemoryStream ms = new MemoryStream();
    //        dataTable.WriteXml(ms);
    //        ms.Position = 0;
    //        StreamReader sReader = new StreamReader(ms, System.Text.Encoding.UTF8);
    //        result = sReader.ReadToEnd();
    //        result = result.Replace("DocumentElement", "Requests");
    //    }
    //    catch (Exception ex)
    //    {
    //        SendMail("Function -> getRequests \n member ID: " + memberID + "Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
    //        return "19";
    //    }
    //    return result;
    //}
    /// <summary>
    /// check that the the user name and password is correct 
    /// </summary>
    /// <param name="merchantUserName">User name </param>
    /// <param name="merchantPassword">Password</param>
    /// <returns>1 - if user name and password is correct 
    ///          2 - otherwise 
    /// </returns>
    internal static string MerchentLogin(string merchantUserName, string merchantPassword,
                                                ref string merchantId, ref string merchantPos)
    {
        string result;
        SqlDataReader reader = null;
        SqlConnection connection = new SqlConnection();
        string query = @"SELECT POSID, MerchantID FROM MerchantPOSs WHERE 
        (MerchantUser = '" + merchantUserName + "') " + " AND (MerchantPassword = '" + merchantPassword + "') ";

        try
        {
            reader = DataBase.ExecuteReader(query, connection);
            while (reader.Read())
            {
                merchantPos = Convert.ToString(reader["POSID"]);
                merchantId = Convert.ToString(reader["MerchantID"]);
            }
        }
        catch (Exception ex)
        {
            SendMail("Function -> MerchentLogin \n merchantUserName -> " + merchantUserName + "\n merchantPassword -> " + merchantPassword
                + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message
                + "\n ex.Source -> " + ex.Source + "\n StackTrace -> " + ex.StackTrace);
            return "19";
        }
        finally
        {
            try
            {
                if (reader == null)
                    result = "9";
                else
                {
                    reader.Close();
                    result = "1";
                }
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }

        }
        return result;
    }

    internal static string GetDBNameByCardPrefix(string cardNumber)
    {
        try
        {
            string cardPrefix = cardNumber.Substring(0, 5);
            string query = @"SELECT NetDB FROM Nets WHERE IDCodeTemplate like '" + cardPrefix + "%' and NetDB <> 'GazitTest'";
            Object obj = DataBase.ExecuteScalar(query);
            if (obj == null)
                return "32";
            else
                return obj.ToString();
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetDBNameByCardPrefix \nCard number: " + cardNumber +
                "Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
    }

    internal static int GetOrgIDByCardPrefix(string cardNumber)
    {
        try
        {
            string cardPrefix = cardNumber.Substring(0, 5);
            string query = @"SELECT HomeNet FROM Nets WHERE IDCodeTemplate like '" + cardPrefix + "%' and NetDB <> 'GazitTest'";
            Object obj = DataBase.ExecuteScalar(query);
            if (obj != null)
                return Convert.ToInt32(obj);

        }
        catch (Exception ex)
        {
            SendMail("Function -> GetDBNameByCardPrefix \nCard number: " + cardNumber +
                "Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
        }
        return 0;
    }

    internal static int GetOrgIDByDbName(string dbName)
    {
        try
        {
            string query = @"select organizationID from DTS_Online..organizations where dbname = '" +  dbName+ "'";
            Object obj = DataBase.ExecuteScalar(query);
            if (obj != null)
                return Convert.ToInt32(obj);

        }
        catch (Exception ex)
        {
            string t = "";
        }

        return 0;
    }


    internal static string isCustomerExists(string DBName, object cardNumber)
    {
        string query = @" SELECT " + DBName + "..Cards.IDMember FROM " + DBName + "..Cards " +
        "where (" + DBName + "..Cards.CardStatus ='1') and (" + DBName + "..Cards.CardNumber = '" + cardNumber + "')";

        try
        {
            Object obj = DataBase.ExecuteScalar(query);
            if (obj == null)
                return "2";
            else
                return obj.ToString();
        }
        catch (Exception ex)
        {
            SendMail("Function -> isCustomerExists \n DBName: " + DBName + "\nCard number: " + cardNumber +
                "Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
    }

    internal static void saveCampaignUse(string memberID, string cardNumber, string merchantID, string merchantPos,
                                                string DBName, string xmlTransactionDetail)
    {
        DateTime currectTime = DateTime.Now;
        string campaignID = "", benefitID = "", groupID = "";
        string query = @"SELECT CampaignBenefits.BenefitID, CampaignBenefits.CampaignID, CampaignGroups.GroupID
                    FROM CampaignBenefits INNER JOIN CampaignGroups ON CampaignBenefits.CampaignID = CampaignGroups.CampainID
                    where (MerchantID = '" + merchantID + "')";
        SqlDataReader reader = null;
        SqlConnection connection = new SqlConnection();
        try
        {
            reader = DataBase.ExecuteReader(query, connection);
            while (reader.Read())
            {
                benefitID = Convert.ToString(reader["BenefitID"]);
                campaignID = Convert.ToString(reader["CampaignID"]);
                groupID = Convert.ToString(reader["GroupID"]);
            }
            query = @"INSERT INTO " + DBName + "..[CampaignMembersUse] " +
            @"([UseTime] ,[CampaignID],[BenefitID] ,[GroupID] ,[POSID], [MerchantID], [ID]
           ,[CardNumber], [NumOfUse], [Amount], [SlipMumber], [CancelTime], [SaleID], [XMLData])
            VALUES (GetDate() " + "," + campaignID + "," + benefitID + "," + groupID + "," + merchantPos + "," + merchantID + "," +
                      "," + memberID + "," + cardNumber + "," + "1" +
                null + "," + null + "," + null + "," + null + "," + xmlTransactionDetail + " )";

            int rowAffected = DataBase.ExecuteNonQuery(query);
        }
        catch (Exception ex)
        {
            SendMail("saveCampaign failed \n" + ex.StackTrace, "isCustomerExists error");
        }
    }

    internal static void GetMemberProp(string dbName, string MemberID, ref DataTable memberProp)
    {
        try
        {
            DataBase.ExecuteAdapter(string.Format(@"SELECT *
                                    FROM {0}..AllMembers
                                    WHERE MemberId = '{1}'", dbName, MemberID), ref memberProp);
        }
        catch (Exception ex)
        {
            SendMail("GetMemberProp failed \n" + ex.StackTrace, "GetMemberProp error");
        }
        //return dt;
    }

    internal static string GetMemberIDByCardNumber(string DBName, string cardNumber)
    {
        string query = string.Format("SELECT IDMember FROM {0}..Cards where CardNumber = '{1}'", DBName, cardNumber);
        try
        {
            Object obj = DataBase.ExecuteScalar(query);
            if (obj != null && obj != DBNull.Value)
                return obj.ToString();
            else
                return "2";
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetMemberIDByCardNumber \n DBName: " + DBName + "\nCard number: " + cardNumber +
                "Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
    }

    internal static string GetActiveMemberID(string dbName, string cardNumber)
    {
        string query = string.Format(@"Select Case When CardStatus='1' Then IDMember Else '1' END 
            FROM {0}..Cards where CardNumber = '{1}'", dbName, cardNumber);
        try
        {
            Object obj = DataBase.ExecuteScalar(query);
            if (obj != null && obj != DBNull.Value)
                return obj.ToString().Trim();
            else
                return "2";
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetActiveMemberID \n DBName: " + dbName + "\nCard number: " + cardNumber +
                "Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
    }

    internal static void GetMerchantBenefit(string organizationID, string merchantNO, string productID, ref DataTable dtBenefit)
    {
        try
        {
            string query = string.Format(@"SELECT Campaigns.CampaignID, Campaigns.CampaignName, CampaignBenefits.BenefitID, CampaignBenefits.BenefitName,
                CampaignGroups.GroupID, Campaigns.CampaignLimit,  CampaignBenefits.BenefitGeneralLimit, CampaignBenefits.BenefitPersonLimit, 
                CampaignBenefits.BenefitAccountLimit, dbo.CampaignBenefits.HasLimits, 
                CampaignBenefits.LimitClient1, CampaignBenefits.LimitClient2, CampaignBenefits.LimitClient3,
                CampaignBenefits.LimitClient4, CampaignBenefits.LimitClient5, CampaignBenefits.LimitBenefit1,
                CampaignBenefits.LimitBenefit2, CampaignBenefits.LimitBenefit3, CampaignBenefits.LimitBenefit4, 
                CampaignBenefits.LimitBenefit5
                FROM CampaignBenefits INNER JOIN Campaigns 
                ON CampaignBenefits.CampaignID = Campaigns.CampaignID INNER JOIN CampaignGroups 
                ON CampaignGroups.CampainID =  Campaigns.CampaignID
                    WHERE (Campaigns.OrganizationID = '{0}') 
                    AND (CampaignBenefits.MerchantID = '{1}') 
                    AND (Campaigns.CampaignStatus = 2) 
                    AND (CampaignBenefits.XMLParam LIKE '%<Par>{2}</Par>%') 
                    AND (CampaignBenefits.BenefitStatus = 1)
                    AND (DateDiff(d, Campaigns.CampaignStartTime, GetDate())) >=0
                    AND (DateDiff(d, Campaigns.CampaignEndTime, GetDate())) <= 0 ", organizationID, merchantNO, productID);

            DataBase.ExecuteAdapter(query, ref dtBenefit);
        }
        catch (Exception ex)
        {
            SendMail("getCampaignBenefitsForMemberAndMerchantID failed \n" + ex.StackTrace, "getCampaignBenefitsForMemberAndMerchantID error");
        }
    }

    internal static int GetNumberOfUse(string databaseName, string condition)
    {
        try
        {
            string query = string.Format(@"SELECT SUM(NumOfUse) FROM {0}..CampaignMembersUse WHERE ({1})",
                                                                                            databaseName, condition);
            object obj = DataBase.ExecuteScalar(query);
            if (DBNull.Value == obj)
                return -1; // no use at all
            return Convert.ToInt32(obj);
        }
        catch (Exception ex)
        {
            SendMail("GetNumberOfUse failed \n" + ex.StackTrace, "GetNumberOfUse error");
            return -1;
        }
    }
    internal static bool CheckLimitForBenefit(string databaseName, string condition)
    {
        try
        {
            string query = string.Format("IF  ( (SELECT SUM(NumOfUse) FROM {0}..CampaignMembersUse WHERE {1}) " +
                                            " SELECT 'False' " +
                                            " ELSE SELECT 'True'", databaseName, condition);

            return Convert.ToBoolean(DataBase.ExecuteScalar(query));
        }
        catch (Exception ex)
        {

            SendMail("CheckLimitForBenefit failed \n" + ex.StackTrace, "CheckLimitForBenefit error");

        }
        return false;
    }

    internal static bool CheckTradeOrg(string databaseName)
    {
        try
        {
            string query = string.Format("IF (SELECT OrganizationName FROM DTS_OnLine..Organizations WHERE OrganizationParamsXML like '%<a65>1</a65>%' AND DBName = " + databaseName + " ) " +
                                            " SELECT 'True' " +
                                            " ELSE SELECT 'False'");

            return Convert.ToBoolean(DataBase.ExecuteScalar(query));
        }
        catch (Exception ex)
        {

            SendMail("CheckLimitForBenefit failed \n" + ex.StackTrace, "CheckLimitForBenefit error");

        }
        return false;
    }

    internal static void GetVariantProp(string dbName, string productID, ref DataTable dtVar)
    {
        try
        {
            string query = string.Format(@"SELECT * 
                                           FROM {0}..ProductsVars 
                                           WHERE FullBarCode = '{1}'", dbName, productID);
            DataBase.ExecuteAdapter(query, ref dtVar);

        }
        catch (Exception ex)
        {

            SendMail("Function -> GetVariantProp \n dbName -> " + dbName + "\n productID -> " + productID
                            + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message + "\n ex.Source -> " + ex.Source + "\n ex.StackTrace -> " + ex.StackTrace);
        }
    }

    internal static void SumOrdersForMember_LC(string memberID, ref DataTable sumOrders)
    {
        try
        {

            string query = string.Format(@"SELECT BarCode,Sum(MemberOrderQuntity) as Quantity,  Sum(MemberOrderBlance) as Balance
                                            FROM LeumiCard_GCPoints..ATRACTIONSOrders 
                                            WHERE MemberID = '{0}'
                                            AND BarCode IN ('100145-2' , '100143-2' , '100144-2' , '100142-2' , '1000077-2' 
                                            , '1000012-2' , '1000041-2' , '1000002-2' , '1000043-2' , '1000019-2' , '100114-2' 
                                            , '1000049-2' , '1000016-1' , '1000091-1' , '1000051-1' , '1000069-2' , '1600110-1' ,'1001046-1')
                                            GROUP BY BarCode
                                            HAVING Sum(MemberOrderQuntity) <> 0"
                                            , memberID);
            DataBase.ExecuteAdapter(query, ref sumOrders);
        }
        catch (Exception ex)
        {
            SendMail("Function -> SumOrdersForMember_LC \n memberID -> " + memberID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace);

        }
    }

    internal static int TotalOrderedForMember_LC(string memberID)
    {
        int quantity = 0;
        try
        {
            string query = string.Format(@"SELECT Sum(MemberOrderQuntity) As TotalOrdered FROM LeumiCard_GCPoints..ATRACTIONSOrders 
                    WHERE MemberID = '{0}' AND BarCode IN ('100145-2' , '100143-2' , '100144-2' , '100142-2' , '1000077-2' 
                    , '1000012-2' , '1000041-2' , '1000002-2' , '1000043-2' , '1000019-2' , '100114-2', '1000049-2' , '1000016-1' , 
                    '1000091-1' , '1000051-1' , '1000069-2' , '1600110-1' ,'1001046-1')", memberID);
            //DataBase.OpenConnection();
            Object result = DataBase.ExecuteScalar(query);
            if (result == DBNull.Value)
                quantity = 0;
            else
                quantity = int.Parse(result.ToString());
        }
        catch (Exception ex)
        {
            SendMail("Function -> TotalOrderedForMember_LC \n memberID -> " + memberID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "LC_SummerIVR");
            return 0;
        }
        //finally
        //{
        //    DataBase.CloseConnection();
        //}
        return quantity;
    }






    internal static string GetCinemaCityCoupon(string memberID, string cardNumber, string phone)
    {
        try
        {
            object obj = DataBase.ExecuteScalar("EXEC LeumiCard_GCPoints..GetCoupon '" + memberID + "', '" + cardNumber + "', '" + phone + "'");
            if (obj != DBNull.Value && obj != null)
                return obj.ToString();
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetCinemaCityCoupon \n memberID -> " + memberID
                            + "\n cardNumber -> " + cardNumber
                            + "\n phone -> " + phone
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "LC_SummerIVR");

        }
        return "";
    }

    internal static void DoExercise(string memberID, string productID, string terminal)
    {
        string updateQuery = string.Format(
                            @"UPDATE LeumiCard_GCPoints..ATRACTIONSOrders
                               SET MemberOrderBlance = MemberOrderQuntity, MemberOrderDateEXE = GETDATE(),
                               MemberTerminalExe = '{0}'
                             WHERE  MemberID = '{1}' AND BarCode = '{2}'", terminal, memberID, productID);
        try
        {
            DataBase.ExecuteNonQuery(updateQuery);
        }

        catch (Exception ex)
        {
            SendMail("Function -> DoExercise \n Exception TYPE -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message
                + "\n memberID Message -> " + memberID
                + "\n productID Message -> " + productID
                + "\n terminal Message -> " + terminal, "LC_SummerIVR");
        }
    }

    internal static string GetCardNumberForMemberIDLC(string memberID, string productID)
    {
        try
        {
            string query = string.Format(@"SELECT CardNumber 
                                           FROM LeumiCard_GCPoints..ATRACTIONSOrders 
                                           WHERE MemberID = '{0}' AND BarCode = '{1}'", memberID, productID);
            object obj = DataBase.ExecuteScalar(query);
            if (obj != DBNull.Value && obj != null)
                return obj.ToString();

        }
        catch (Exception ex)
        {

            SendMail("Function -> GetCardNumberForMemberIDLC \n memberID -> " + memberID
                + "\n productID -> " + productID
                + "\n Exception TYPE -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message
                + "\n ex.Source -> " + ex.Source
                + "\n ex.StackTrace -> " + ex.StackTrace, "LC_SummerIVR");
        }
        return "";

    }


    internal static int GetQuantityForProductID(string productID, string memberID)
    {
        int quantity = 0;
        try
        {
            string query = string.Format(@"SELECT Sum(MemberOrderQuntity) As TotalOrdered FROM LeumiCard_GCPoints..ATRACTIONSOrders 
                    WHERE MemberID = '{0}' AND BarCode = '{1}'", memberID, productID);
            //DataBase.OpenConnection();
            Object result = DataBase.ExecuteScalar(query);
            if (result == DBNull.Value)
                quantity = 0;
            else
                quantity = int.Parse(result.ToString());
        }
        catch (Exception ex)
        {
            SendMail("Function -> TotalOrderedForMember_LC \n memberID -> " + memberID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "LC_SummerIVR");
            return 0;
        }
        //finally
        //{
        //    DataBase.CloseConnection();
        //}
        return quantity;
    }

    internal static void GetMarketingCommissionForProductID(string ProductID, string dbName, ref string MarketingCommission, string OrgID)
    {
        try
        {
            //            string query = string.Format(@"select MarketingCommission
            //                                            from business as dts join {0}..ProductsVars as org
            //                                            on dts.BuisnessID = org.BusinessId
            //                                            where FullBarCode = '{1}'", dbName, ProductID);


            string query = string.Format(@"select CASE WHEN OrgMarketingCommission IS NULL THEN   MarketingCommission ELSE OrgMarketingCommission END as MarketingCommission
                                            from business as dts join {0}..ProductsVars as org
                                            on dts.BuisnessID = org.BusinessId
                                            left JOIN OrganizatinBusinsess
                                            ON OrganizatinBusinsess.BuisnessID = dts.BuisnessID
                                            AND OrganizatinBusinsess.OrgID = {2}
                                            where FullBarCode = '{1}'", dbName, ProductID, OrgID);

            Object result = null;
            result = DataBase.ExecuteScalar(query);
            if (result != null && result != DBNull.Value)
                MarketingCommission = result.ToString();

        }
        catch (Exception ex)
        {
            SendMail("Function -> GetMarketingCommissionForProductID \n ProductID -> " + ProductID
                            + "\n dbName TYPE -> " + dbName
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "LC_SummerIVR");
        }
    }

    internal static string getCategories(string organizationID)
    {
        XmlDocument xmlDoc = new XmlDocument();
        DataTable dataTable = new DataTable();

        string categoriesQuery = string.Format(
                                        @"SELECT CategoryName, CategoryID
                                            FROM CampaignCategories
                                            WHERE (OrganizationID = {0} AND CategoryStatus = 1)
                                            ORDER BY CategorySort", organizationID);
        try
        {
            // init SqlDataAdapter with select command and connection
            dataTable = DataBase.FillDataTable(categoriesQuery, "category");

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.NewRow();
                row["CategoryName"] = "ללא קטגוריה";
                row["CategoryID"] = 0;
                dataTable.Rows.Add(row);
            }
            //DataBase.ExecuteAdapter(campaignQuery, ref dataTable);

            MemoryStream ms = new MemoryStream();

            dataTable.WriteXml(ms);
            ms.Position = 0;
            StreamReader sReader = new StreamReader(ms, System.Text.Encoding.UTF8);

            xmlDoc.Load(sReader);

            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList nodeList = root.ChildNodes;
            XmlDocumentFragment frag = xmlDoc.CreateDocumentFragment();
            string categoryID = "";
            string campaign = "";

            foreach (XmlNode node in nodeList)
            {
                categoryID = node.SelectSingleNode("CategoryID").InnerText.Trim();
                campaign = getCampaigns(organizationID, categoryID);
                campaign = campaign.Replace("DocumentElement", "Campaigns");
                frag.InnerXml = campaign;
                node.AppendChild(frag);
            }
        }
        catch (Exception ex)
        {
            SendMail("Function -> getCampaigns \n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        return xmlDoc.OuterXml;
    }

    internal static int AddMemberAndCard(string dbName, string MemberID, string CardNumber, string MemberFirstName, string MemberLastName,
                string SmsPhoneNumber, string isPotentialClient, bool encriptionRequired, string email)
    {

        string fullName = "";

        CheckMemberProp(ref fullName, ref MemberFirstName, ref MemberLastName, ref SmsPhoneNumber, ref email);

        string insertQuery;
        if (isPotentialClient == "0")
        {
            insertQuery = string.Format(@"BEGIN TRAN
                                            BEGIN TRY
                                                INSERT INTO {0}..AllMembers
                                                    (MemberId,MemberName,MemberFirstName,MemberLastName,MobilePhone,Email)
                                                VALUES 
                                                    ('{1}', {2}, {3}, {4}, {5}, {7})
                                                INSERT INTO {0}..Cards
                                                    (CardNumber,IDMember,CardStatus,AddedTime)
                                                VALUES
                                                    ('{6}', '{1}', 1, GetDate())
                                            END TRY
                                            BEGIN CATCH
                                                ROLLBACK TRAN
                                                RETURN;
                                            END CATCH;
                                        COMMIT TRAN", dbName, MemberID, fullName, MemberFirstName, MemberLastName, SmsPhoneNumber, CardNumber, email);

        }
        else
        {
            insertQuery = string.Format(@"INSERT INTO {0}..AllMembers
                                            (MemberId,MemberName,MemberFirstName,MemberLastName,MobilePhone,Email)
                                                VALUES ('{1}', {2}, {3}, {4}, {5}, {6})"
                                        , dbName, MemberID, fullName, MemberFirstName, MemberLastName, SmsPhoneNumber, email);
        }
        try
        {
            return DataBase.ExecuteNonQuery(insertQuery);
        }

        catch (Exception ex)
        {
            SendMail("Function -> AddMember \n ExceptionTYPE -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message
                + "\n dbName -> " + dbName
                + "\n MemberID -> " + MemberID
                + "\n CardNumber -> " + CardNumber
                + "\n MemberFirstName -> " + MemberFirstName
                + "\n MemberLastName -> " + MemberLastName
                + "\n Email -> " + email
                + "\n SmsPhoneNumber -> " + SmsPhoneNumber, "Leumi Campaign");

            return 19;
        }
    }

    internal static int AddMember(string dbName, string memberID, string fullName, string mobilePhone, string companyID)
    {
        string insertQuery = string.Format(@"INSERT INTO {0}..AllMembers
                                            (MemberId,MemberName,MobilePhone,PopulationType)
                                                VALUES ('{1}', '{2}', '{3}', '{4}')"
                                    , dbName, memberID, fullName.Replace("'", "''"), mobilePhone, companyID);
        try
        {
            return DataBase.ExecuteNonQuery(insertQuery);
        }
        catch (Exception ex)
        {
            SendMail("Function -> AddMember \n ExceptionTYPE -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message
                + "\n dbName -> " + dbName
                + "\n MemberID -> " + memberID
                + "\n mobilePhone -> " + mobilePhone
                + "\n companyID -> " + companyID, "WSClubs_AddMember");

            return 19;
        }
    }

    private static void CheckMemberProp(ref string fullName, ref string MemberFirstName, ref string MemberLastName, ref string SmsPhoneNumber, ref string email)
    {
        //MemberFirstName
        if (string.IsNullOrEmpty(MemberFirstName))
            MemberFirstName = "null";
        else
        {
            fullName += " " + MemberFirstName;
            MemberFirstName = "'" + MemberFirstName + "'";
        }
        //MemberLastName
        if (string.IsNullOrEmpty(MemberLastName))
            MemberLastName = "null";
        else
        {
            fullName += " " + MemberLastName;
            MemberLastName = "'" + MemberLastName + "'";
        }
        //SmsPhoneNumber
        if (string.IsNullOrEmpty(SmsPhoneNumber))
            SmsPhoneNumber = "null";
        else
            SmsPhoneNumber = "'" + SmsPhoneNumber + "'";
        //fullName
        fullName = fullName.Trim();
        if (string.IsNullOrEmpty(fullName))
        {
            fullName = "null";
        }
        else
            fullName = "'" + fullName + "'";

        //Email
        if (string.IsNullOrEmpty(email))
        {
            email = "null";
        }
        else
            email = "'" + email + "'";
    }

    //    internal static string EncriptCardNumber(string databaseName, string cardNumber)
    //    {
    //        //private static 
    //        string OpenEncription = "OPEN SYMMETRIC KEY DtsKey2010 DECRYPTION BY CERTIFICATE DtsCertificate";
    //        string CloseEncription = "CLOSE SYMMETRIC KEY DtsKey2010";

    //        try
    //        {
    //            string updateQuery = string.Format(@"update {0}..cards set cardNumber = SUBSTRING (CardNumber, Len(CardNumber)-3, 4) Where cardNumber = '{1}'",
    //                    databaseName, cardNumber);

    //            // Encrypt the card number 
    //            string encyptrQuery = string.Format(@"update {0}..cards set EncryptedCard = ENCRYPTBYKEY(KEY_GUID('DtsKey2010'), '{1}')
    //                    where cardnumber = '{1}'", databaseName, cardNumber);

    //            string query = string.Format(@"{0} {1} {2} {3}", OpenEncription, encyptrQuery, updateQuery, CloseEncription);

    //            DataBase.ExecuteNonQuery(query);
    //        }
    //        catch (Exception ex)
    //        {
    //            SendMail("Function -> EncriptCardNumber"
    //                            + "\n database Name -> " + databaseName
    //                            + "\n card Number -> " + cardNumber
    //                            + "\n Exception TYPE -> " + ex.GetType()
    //                            + "\n Exception Message -> " + ex.Message
    //                            + "\n ex.Source -> " + ex.Source
    //                            + "\n ex.StackTrace -> " + ex.StackTrace, "Leumi Campaign");
    //            return "0";
    //        }
    //        return "1";
    //    }



    //    internal static string GetgroupIDForCampaignID(string CampaignID, string orgID)
    //    {//
    //        try
    //        {
    //            string query = string.Format(
    //                            @"SELECT CampaignGroups.GroupID
    //                                FROM CampaignGroups INNER JOIN Campaigns 
    //                                    ON CampaignGroups.CampainID = Campaigns.CampaignID
    //                                WHERE (CampaignGroups.CampainID = {0}) 
    //                                    AND (Campaigns.OrganizationID = {1})", CampaignID, orgID);
    //            Object result = DataBase.ExecuteScalar(query);
    //            if (result != null && result != DBNull.Value)
    //                return result.ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            SendMail("Function -> GetgroupIDForCampaignID"
    //                            + "\n CampaignID -> " + CampaignID
    //                            + "\n orgID -> " + orgID
    //                            + "\n Exception TYPE -> " + ex.GetType()
    //                            + "\n Exception Message -> " + ex.Message
    //                            + "\n ex.Source -> " + ex.Source
    //                            + "\n ex.StackTrace -> " + ex.StackTrace, "Leumi Campaign");
    //            return "0";
    //        }
    //        return null;
    //    }


    internal static string GetgroupIDForCampaignID(string CampaignID, string orgID, ref int groupType, ref string SmsText, ref bool addCoupon, ref int stockCoupon, ref bool sendSMS)
    {//

        SqlDataReader reader = null;
        SqlConnection con = new SqlConnection();

        try
        {
            string query = string.Format(
                            @"SELECT CampaignGroups.GroupID, CampaignGroups.GroupType, SendSms, SmsText, AddCoupon, StockCoupon, SendSms
                                FROM CampaignGroups INNER JOIN Campaigns 
                                    ON CampaignGroups.CampainID = Campaigns.CampaignID
                                WHERE (CampaignGroups.CampainID = {0}) 
                                    AND (Campaigns.OrganizationID = {1})", CampaignID, orgID);
            reader = DataBase.ExecuteReader(query, con);
            if (reader.Read())
            {
                if (reader["SendSms"] != DBNull.Value)
                    sendSMS = Convert.ToBoolean(reader["SendSms"]);
                if (reader["AddCoupon"] != DBNull.Value)
                    addCoupon = Convert.ToBoolean(reader["AddCoupon"]);
                if (reader["StockCoupon"] != DBNull.Value)
                    stockCoupon = Convert.ToInt32(reader["StockCoupon"]);
                if (reader["SmsText"] != DBNull.Value)
                    SmsText = Convert.ToString(reader["SmsText"]);

                if (reader["GroupType"] != DBNull.Value)
                    groupType = Convert.ToInt32(reader["GroupType"]);
                if (reader["GroupID"] != DBNull.Value)
                    return Convert.ToString(reader["GroupID"]);
            }
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetgroupIDForCampaignID"
                            + "\n CampaignID -> " + CampaignID
                            + "\n orgID -> " + orgID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "Leumi Campaign");
            return "0";
        }
        finally
        {
            if (reader != null)
                reader.Close();
            if (con.State != ConnectionState.Closed)
                con.Close();
        }
        return "0";
    }


    internal static Int64 AddMemberToGroupID(string dbName, string MemberID, string CampaignID, string groupID)
    {
        try
        {
            string query = string.Format(
                            @"IF EXISTS 
                            (SELECT IdentityNum 
                            FROM {0}..CampaignGroupMembers 
                            WHERE IdentityNum = '{1}' AND GroupID = {2} AND CampaignID = {3}) 
                            SELECT 0 
                            ELSE 
                            INSERT INTO {0}..CampaignGroupMembers
                            (GroupID,CampaignID,IdentityNum,DateAdded)
                            VALUES
                            ({2}, {3}, '{1}',GetDate())
                            SELECT SCOPE_IDENTITY()", dbName, MemberID, groupID, CampaignID);
            Object result = DataBase.ExecuteScalar(query);
            if (result != DBNull.Value && result != null)
                return Convert.ToInt64(result);

        }
        catch (Exception ex)
        {
            SendMail("Function -> GetgroupIDForCampaignID"
                            + "\n CampaignID -> " + CampaignID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "Leumi Campaign");
        }
        return 0;
    }

    internal static void GetCampaignPropToSMS(string CampaignID, ref string SmsText, ref bool addCoupon, ref int stockCoupon, ref bool sendSms)
    {
        SqlDataReader reader = null;
        SqlConnection con = new SqlConnection();
        string query = string.Format(@"Select SendSms, SmsText, AddCoupon, StockCoupon, SendSms
                                        From Campaigns
                                        Where CampaignID = {0}", CampaignID);
        try
        {
            reader = DataBase.ExecuteReader(query, con);
            if (reader.Read())
            {

                if (reader["SendSms"] == DBNull.Value)
                    sendSms = reader.GetBoolean(0);
                //return;//No need SMS
                if (reader["SmsText"] != DBNull.Value)
                    SmsText = reader.GetString(1);
                if (reader["AddCoupon"] != DBNull.Value)
                    addCoupon = reader.GetBoolean(2);
                if (reader["StockCoupon"] != DBNull.Value)
                    stockCoupon = reader.GetInt32(3);
            }
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetCampaignPropToSMS"
                            + "\n CampaignID -> " + CampaignID
                            + "\n SmsText -> " + SmsText
                            + "\n addCoupon -> " + addCoupon
                            + "\n stockCoupon -> " + stockCoupon
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "Leumi Campaign");
        }
        finally
        {
            if (reader != null)
                reader.Close();
            if (con.State != ConnectionState.Closed)
                con.Close();
        }
    }



    internal static void GetCoupon(int stockCoupon, string MemberID, string CardNumber, string SmsPhoneNumber, string SeventhCardDigit, string CampaignID, out Int64 CouponID, out string CouponCode)
    {
        //string query = string.Format("GetCouponFromStockBuzzer '{0}','{1}','{2}',{3}, '{4}', {5}", MemberID, CardNumber, SmsPhoneNumber, stockCoupon, SeventhCardDigit, CampaignID);
        //try
        //{
        //    object obj = DataBase.ExecuteScalar(query);
        //    if (obj != DBNull.Value && obj != null)
        //        return obj.ToString();
        //}
        //catch (Exception ex)
        //{
        //    SendMail("Function -> GetCoupon"
        //                    + "\n stockCoupon -> " + stockCoupon
        //                    + "\n MemberID -> " + MemberID
        //                    + "\n CardNumber -> " + CardNumber
        //                    + "\n SmsPhoneNumber -> " + SmsPhoneNumber
        //                    + "\n stockCoupon -> " + stockCoupon
        //                    + "\n Exception TYPE -> " + ex.GetType()
        //                    + "\n Exception Message -> " + ex.Message
        //                    + "\n ex.Source -> " + ex.Source
        //                    + "\n ex.StackTrace -> " + ex.StackTrace, "Leumi Campaign");
        //}
        //return "";
        CouponCode = "";
        CouponID = 0;

        SqlCommand commnad = new SqlCommand();
        commnad.CommandType = CommandType.StoredProcedure;
        commnad.CommandText = "GetCouponFromStockBuzzer_CouponID";
        commnad.Parameters.Add("@memberID", SqlDbType.VarChar, 50);
        commnad.Parameters["@memberID"].Value = MemberID;
        commnad.Parameters.Add("@cardNumber", SqlDbType.VarChar, 50);
        commnad.Parameters["@cardNumber"].Value = CardNumber;
        commnad.Parameters.Add("@phoneNumber", SqlDbType.VarChar, 50);
        commnad.Parameters["@phoneNumber"].Value = SmsPhoneNumber;
        commnad.Parameters.Add("@StockID", SqlDbType.Int);
        commnad.Parameters["@StockID"].Value = stockCoupon;
        commnad.Parameters.Add("@SeventhCardDigit", SqlDbType.Char);
        commnad.Parameters["@SeventhCardDigit"].Value = SeventhCardDigit;
        commnad.Parameters.Add("@CampaignID", SqlDbType.BigInt);
        commnad.Parameters["@CampaignID"].Value = CampaignID;
        commnad.Parameters.Add("@CouponID", SqlDbType.BigInt);
        commnad.Parameters["@CouponID"].Direction = ParameterDirection.Output;
        commnad.Parameters.Add("@CouponCode", SqlDbType.VarChar, 50);
        commnad.Parameters["@CouponCode"].Direction = ParameterDirection.Output;
        try
        {
            DataBase.ExecuteScalar(commnad);
            if (commnad.Parameters["@CouponID"] != null && commnad.Parameters["@CouponID"].Value != DBNull.Value)
            {
                if (Int64.TryParse(commnad.Parameters["@CouponID"].Value.ToString(), out CouponID))
                    CouponCode = commnad.Parameters["@CouponCode"].Value.ToString();
            }
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetCoupon"
                            + "\n stockCoupon -> " + stockCoupon.ToString()
                            + "\n MemberID -> " + MemberID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace);
        }
    }


    internal static string OrganizationLogin(ref string OrganizationUserName, string OrganizationPassword, ref string orgUserID, ref bool CheckMethodsPremission, ref string ipList)
    {
        SqlCommand commnad = new SqlCommand();
        commnad.CommandType = CommandType.StoredProcedure;
        commnad.CommandText = "ValidateOrganizationUserID";
        commnad.Parameters.Add("@UserName", SqlDbType.VarChar, 50);
        commnad.Parameters["@UserName"].Value = OrganizationUserName;
        commnad.Parameters.Add("@Password", SqlDbType.VarChar, 50);
        commnad.Parameters["@Password"].Value = OrganizationPassword;
        commnad.Parameters.Add("@OrgUserID", SqlDbType.Int, 1);
        commnad.Parameters["@OrgUserID"].Direction = ParameterDirection.Output;
        commnad.Parameters.Add("@CheckMethodsPremission", SqlDbType.Bit, 1);
        commnad.Parameters["@CheckMethodsPremission"].Direction = ParameterDirection.Output;
        commnad.Parameters.Add("@IPList", SqlDbType.VarChar, 4000);
        commnad.Parameters["@IPList"].Direction = ParameterDirection.Output;
        commnad.Parameters.Add("@OrganizationID", SqlDbType.Int, 1);
        commnad.Parameters["@OrganizationID"].Direction = ParameterDirection.Output;
        try
        {
            DataBase.ExecuteScalar(commnad);
            if (bool.TryParse(commnad.Parameters["@CheckMethodsPremission"].Value.ToString(), out CheckMethodsPremission))
            {
                orgUserID = commnad.Parameters["@OrgUserID"].Value.ToString();
                ipList = commnad.Parameters["@IPList"].Value.ToString();
                OrganizationUserName = commnad.Parameters["@OrganizationID"].Value.ToString();
            }
        }
        catch (Exception ex)
        {
            SendMail("Function -> OrganizationLogin"
                            + "\n OrganizationID -> " + OrganizationUserName
                            + "\n OrganizationPassword -> " + OrganizationPassword
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace);
        }
        return null;
    }

    internal static bool CheckPremissionToMethod(string orgUserID, string methodName)
    {
        try
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter p = new SqlParameter("@MethodName", SqlDbType.NVarChar);
            p.Value = methodName;
            parameters.Add(p);
            SqlParameter p1 = new SqlParameter("@OrgUserID", SqlDbType.Int);
            p1.Value = orgUserID;
            parameters.Add(p1);
            string query = string.Format(
                            @"IF EXISTS
                            (
                                SELECT WSMethods.MethodID
                                FROM WSMethodPremission INNER JOIN WSMethods 
                                ON WSMethodPremission.MethodID = WSMethods.MethodID
                                WHERE (WSMethods.MethodName = @MethodName) 
                                    AND (WSMethodPremission.OrgUserID = @OrgUserID)
                            )
                            Select 'True'
                            Else
                            Select 'False'", methodName, orgUserID);
            Object result = DataBase.ExecuteScalar(query, parameters);
            if (result != DBNull.Value && result != null)
                return Convert.ToBoolean(result);

        }
        catch (Exception ex)
        {
            SendMail("Function -> CheckPremissionToMethod"
                            + "\n orgUserID -> " + orgUserID
                            + "\n methodName -> " + methodName
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "Leumi Campaign");
        }
        return false;
    }

    internal static void GetPaidCampaigns(string dbName, string memberID, ref DataTable dt)
    {
        string query = string.Format(
                        @"SELECT Campaigns.CampaignID, Campaigns.CampaignLimit, 
                            CampaignBenefits.BenefitPersonLimit, 
                            CampaignBenefits.BenefitID ,'True' as [Status]
                        FROM Campaigns INNER JOIN CampaignBenefits 
                            ON Campaigns.CampaignID = CampaignBenefits.CampaignID 
                            INNER JOIN {0}..CampaignGroupMembers 
                            ON Campaigns.CampaignID = {0}..CampaignGroupMembers.CampaignID
                        WHERE (Campaigns.NeedCharge = 1) 
			                AND (Campaigns.CampaignStatus = 2) 
                            AND (CampaignBenefits.BenefitStatus = 1)
			                AND ({0}..CampaignGroupMembers.IdentityNum = '{1}') 
			                AND (GetDate() >= CampaignStartTime) 
                            AND (GetDate() <= CampaignEndTime + 1)", dbName, memberID);
        try
        {
            DataBase.ExecuteAdapter(query, ref dt);
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetPaidCampaigns"
                            + "\n dbName -> " + dbName
                            + "\n memberID -> " + memberID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "IVR Campaign");
        }
    }

    //    internal static string ClearCatchCampaigns()
    //    {
    //        int intCatchTimeSeconds = 0;
    //        string catchTimeSeconds = ConfigurationManager.AppSettings["CatchTimeSeconds"].ToString();
    //        if (string.IsNullOrEmpty(catchTimeSeconds)
    //            || int.TryParse(catchTimeSeconds, out intCatchTimeSeconds))
    //            return "19";
    //        try
    //        {
    //            string query = @"Delete 
    //                                From CampaignsCatch 
    //                                Where DATEDIFF(second, CatchDate, GetDate()) >= " + catchTimeSeconds;
    //            DataBase.ExecuteNonQuery(query);
    //            return "1";
    //        }
    //        catch (Exception ex)
    //        {
    //            SendMail("Function -> ClearCatchCampaigns"
    //                            + "\n Exception TYPE -> " + ex.GetType()
    //                            + "\n Exception Message -> " + ex.Message
    //                            + "\n ex.Source -> " + ex.Source
    //                            + "\n ex.StackTrace -> " + ex.StackTrace, "IVR Campaign");

    //            return "0";
    //        }
    //    }

    //internal static void InsertCatchCampaign(object CampaignID, string memberID, object BenefitAccountLimit)
    //{

    //}


    internal static Int64 GetSumUseCampaign(string dbName, string campID, string benefitID, string checkMemberID)
    {
        try
        {
            string query = string.Format(
                            @"Select Sum(NumOfUse)
                            From {0}..CampaignMembersUse
                            Where CampaignID = {1} AND BenefitID = {2}{3}", dbName, campID, benefitID, checkMemberID);
            object obj = DataBase.ExecuteScalar(query);
            if (obj != DBNull.Value && obj != null)
                return Convert.ToInt64(obj);
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetSumUseCampaign"
                            + "\n campID -> " + campID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace);
        }
        return 0;
    }

    internal static long GetSumUseCampaignForMember(string dbName, string campID, string benefitID, string memberID)
    {
        return GetSumUseCampaign(dbName, campID, benefitID, " AND ID = '" + memberID + "'");
    }

    //    internal static string GetBenefitIDForCampaignID(string CampaignID, string orgID)
    //    {
    //        try
    //        {
    //            string query = string.Format(
    //                            @"SELECT CampaignBenefits.BenefitID
    //                                FROM CampaignBenefits INNER JOIN Campaigns 
    //                                    ON CampaignBenefits.CampainID = Campaigns.CampaignID
    //                                WHERE (CampaignBenefits.CampainID = {0}) 
    //                                    AND (Campaigns.OrganizationID = {1})", CampaignID, orgID);
    //            Object result = DataBase.ExecuteScalar(query);
    //            if (result != null && result != DBNull.Value)
    //                return result.ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            SendMail("Function -> GetBenefitIDForCampaignID"
    //                            + "\n CampaignID -> " + CampaignID
    //                            + "\n orgID -> " + orgID
    //                            + "\n Exception TYPE -> " + ex.GetType()
    //                            + "\n Exception Message -> " + ex.Message
    //                            + "\n ex.Source -> " + ex.Source
    //                            + "\n ex.StackTrace -> " + ex.StackTrace, "Leumi Campaign");
    //            return "0";
    //        }
    //        return null;
    //    }

    //    internal static string GetMerchantIDForCampaignID(string campaignID, int orgID)
    //    {
    //        try
    //        {
    //            string query = string.Format(
    //                            @"SELECT CampaignBenefits.BenefitID
    //                                FROM CampaignBenefits INNER JOIN Campaigns 
    //                                    ON CampaignBenefits.CampainID = Campaigns.CampaignID
    //                                WHERE (CampaignBenefits.CampainID = {0}) 
    //                                    AND (Campaigns.OrganizationID = {1})", CampaignID, orgID);
    //            Object result = DataBase.ExecuteScalar(query);
    //            if (result != null && result != DBNull.Value)
    //                return result.ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            SendMail("Function -> GetBenefitIDForCampaignID"
    //                            + "\n CampaignID -> " + CampaignID
    //                            + "\n orgID -> " + orgID
    //                            + "\n Exception TYPE -> " + ex.GetType()
    //                            + "\n Exception Message -> " + ex.Message
    //                            + "\n ex.Source -> " + ex.Source
    //                            + "\n ex.StackTrace -> " + ex.StackTrace, "Leumi Campaign");
    //            return "0";
    //        }
    //        return null;
    //    }

    internal static void GetPropToInsertCampaignUse(ref Int64 benefitID, ref Int64 groupID, ref Int64 merchantID, ref float money, string campaignID, int orgID)
    {
        SqlDataReader reader = null;
        SqlConnection con = new SqlConnection();
        string query = string.Format(@"SELECT CampaignMerchants.MerchantID, CampaignGroups.GroupID, CampaignBenefits.BenefitID, Campaigns.SumCharge
                                            FROM CampaignMerchants INNER JOIN CampaignBenefits 
                                        ON CampaignMerchants.CampaignID = CampaignBenefits.CampaignID 
                                        INNER JOIN CampaignGroups 
                                        ON CampaignBenefits.CampaignID = CampaignGroups.CampainID 
                                        INNER JOIN Campaigns 
                                        ON CampaignBenefits.CampaignID = Campaigns.CampaignID
                                            WHERE (Campaigns.CampaignID = {0}) AND (Campaigns.OrganizationID = {1})", campaignID, orgID);
        try
        {
            reader = DataBase.ExecuteReader(query, con);
            if (reader.Read())
            {
                if (reader["MerchantID"] != DBNull.Value)
                    merchantID = reader.GetInt64(0);
                if (reader["GroupID"] != DBNull.Value)
                    groupID = reader.GetInt64(1);
                if (reader["BenefitID"] != DBNull.Value)
                    benefitID = reader.GetInt64(2);
                if (reader["SumCharge"] != DBNull.Value)
                    money = (float)reader.GetDecimal(3);//Can't get float from sql
            }
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetPropToInsertCampaignUse"
                            + "\n campaignID -> " + campaignID
                            + "\n benefitID -> " + benefitID
                            + "\n groupID -> " + groupID
                            + "\n money -> " + money
                            + "\n merchantID -> " + merchantID
                            + "\n orgID -> " + orgID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "Leumi Campaign");
        }
        finally
        {
            if (reader != null)
                reader.Close();
            if (con.State != ConnectionState.Closed)
                con.Close();
        }
    }

    internal static int InsertCampaignUse(string campaignID, long benefitID, long groupID, long merchantID, string memberID,
        string cardNumber, string quantity, string dbName)
    {
        string insertQuery = string.Format(
                            @"INSERT INTO {0}..CampaignMembersUse
                                (UseTime,CampaignID,BenefitID,GroupID,POSID,MerchantID,ID,CardNumber,NumOfUse)
                            VALUES
                                (GetDate(),'{1}','{2}','{3}',0,'{4}','{5}','{6}','{7}')",
                            dbName, campaignID, benefitID, groupID, merchantID, memberID, cardNumber.Substring(cardNumber.Length - 4), quantity);
        try
        {
            return DataBase.ExecuteNonQuery(insertQuery);
        }

        catch (Exception ex)
        {
            SendMail("Function -> InsertCampaignUse \n Exception TYPE -> " + ex.GetType()
                + "\n campaignID -> " + campaignID
                + "\n benefitID -> " + benefitID
                + "\n groupID -> " + groupID
                + "\n merchantID -> " + merchantID
                + "\n memberID -> " + memberID
                + "\n cardNumber -> " + cardNumber
                + "\n quantity -> " + quantity
                + "\n dbName -> " + dbName
                + "\n Exception Message -> " + ex.Message
                + "\n memberID Message -> " + memberID
                , "Leumi Campaign");
        }
        return 0;
    }

    internal static int DeleteCampaignUse(string campaignID, long benefitID, long groupID, long merchantID, string memberID,
        string cardNumber, string quantity, string dbName, string teleclalAns)
    {
        string updateQuery = string.Format(
                            @"Update {0}..CampaignMembersUse
                                Set NumOfUse = 0, CancelTime = GetDate(), XMLData = '{8}'
                                WHERE CampaignID = {1} AND BenefitID = {2} 
                                    AND GroupID = {3} AND MerchantID = {4} 
                                    AND ID = {5} AND CardNumber = {6} AND NumOfUse = {7}",
                            dbName, campaignID, benefitID, groupID, merchantID, memberID, cardNumber, quantity, teleclalAns);
        try
        {
            return DataBase.ExecuteNonQuery(updateQuery);
        }

        catch (Exception ex)
        {
            SendMail("Function -> DeleteCampaignUse \n Exception TYPE -> " + ex.GetType()
                + "\n campaignID -> " + campaignID
                + "\n benefitID -> " + benefitID
                + "\n groupID -> " + groupID
                + "\n merchantID -> " + merchantID
                + "\n memberID -> " + memberID
                + "\n cardNumber -> " + cardNumber
                + "\n quantity -> " + quantity
                + "\n dbName -> " + dbName
                + "\n teleclalAns -> " + teleclalAns
                + "\n Exception Message -> " + ex.Message
                , "Leumi Campaign");
        }
        return 0;
    }

    internal static void GetCategoryLimitForCampaignID(string CampaignID, string OrganizationID, ref double dayLimit, ref Int16 categoryID, ref int CategoryLimitQuantity)
    {
        SqlDataReader reader = null;
        SqlConnection con = new SqlConnection();
        try
        {
            string query = string.Format(
                            @"Select LoadLimitDays, CampaignCategories.CategoryID, CategoryLimitQuantity
                                From CampaignCategories Join Campaigns
                                ON CampaignCategories.CategoryID = Campaigns.CategoryID
                                Where CampaignID = {0} AND CampaignCategories.OrganizationID = {1}", CampaignID, OrganizationID);
            reader = DataBase.ExecuteReader(query, con);
            if (reader.Read())
            {
                if (reader["CategoryID"] != DBNull.Value)
                    categoryID = Convert.ToInt16(reader["CategoryID"]);
                if (reader["LoadLimitDays"] != DBNull.Value)
                    dayLimit = Convert.ToDouble(reader["LoadLimitDays"]);
                if (reader["CategoryLimitQuantity"] != DBNull.Value)
                    CategoryLimitQuantity = Convert.ToInt32(reader["CategoryLimitQuantity"]);
                else
                    CategoryLimitQuantity = 0;
            }
        }
        catch (Exception ex)
        {
            categoryID = -1;
            SendMail("Function -> GetCategoryLimitForCampaignID"
                            + "\n CampaignID -> " + CampaignID
                            + "\n OrganizationID -> " + OrganizationID
                            + "\n dayLimit -> " + dayLimit
                            + "\n categoryID -> " + categoryID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace);
        }
    }

    internal static DateTime GetLastDateTimeAdded(string dbName, Int16 categoryID, string MemberID)
    {
        try
        {
            string query = string.Format(@"select Max(DateAdded) From {0}..CampaignGroupMembers join Campaigns On 
                    {0}..CampaignGroupMembers.CampaignID = Campaigns.CampaignID Where Campaigns.CategoryID = {1} 
                    and identityNum ='{2}'", dbName, categoryID, MemberID);
            object obj = DataBase.ExecuteScalar(query);
            if (obj != DBNull.Value && obj != null)
                return Convert.ToDateTime(obj);
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetSumUseCampaign"
                            + "\n dbName -> " + dbName
                            + "\n categoryID -> " + categoryID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace);
        }
        return DateTime.MinValue;
    }

    internal static DateTime GetLastDateTimeAddedCoupon(Int16 categoryID, string MemberID)
    {
        try
        {
            string query = string.Format(@"SELECT MAX(CouponsStock.SendingTime)
                                            FROM CouponsStock INNER JOIN Campaigns 
                                            ON CouponsStock.StockID = Campaigns.StockCoupon
                                            WHERE (CouponsStock.MemberID = '{1}') AND (dbo.Campaigns.CategoryID = {0})", categoryID, MemberID);
            object obj = DataBase.ExecuteScalar(query);
            if (obj != DBNull.Value && obj != null)
                return Convert.ToDateTime(obj);
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetSumUseCampaign"
                            + "\n categoryID -> " + categoryID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace);
        }
        return DateTime.MinValue;
    }

    internal static DateTime GetLastDateTimeAddedCouponGlobal(Int16 categoryID, string MemberID)
    {
        try
        {
            string query = string.Format(@"SELECT MAX(CouponsStock.SendingTime)
                                            FROM CouponsStock INNER JOIN Campaigns 
                                            ON CouponsStock.CampaignID = Campaigns.CampaignID
                                            WHERE (CouponsStock.MemberID = '{1}') AND (dbo.Campaigns.CategoryID = {0})", categoryID, MemberID);
            object obj = DataBase.ExecuteScalar(query);
            if (obj != DBNull.Value && obj != null)
                return Convert.ToDateTime(obj);
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetLastDateTimeAddedCoupon"
                            + "\n categoryID -> " + categoryID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace);
        }
        return DateTime.MinValue;
    }

    internal static int CountLoadCamp(short categoryID, string MemberID)
    {
        try
        {
            string query = string.Format(@"declare @date datetime
                                            set @date = GetDate()
                                            SELECT count(CouponsStock.SendingTime)
                                            FROM CouponsStock INNER JOIN Campaigns 
                                            ON CouponsStock.CampaignID = Campaigns.CampaignID
                                            WHERE (CouponsStock.MemberID = '{1}') AND (dbo.Campaigns.CategoryID = {0})
                                            and month(CouponsStock.SendingTime) =  month(@date)
                                            and year(CouponsStock.SendingTime) =  year(@date)", categoryID, MemberID);
            object obj = DataBase.ExecuteScalar(query);
            return Convert.ToInt32(obj);
        }
        catch (Exception ex)
        {
            SendMail("Function -> CountLoadCamp"
                            + "\n categoryID -> " + categoryID
                            + "\n MemberID -> " + MemberID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace);
        }
        return 0;
    }

    /// <summary>
    /// check if the organization require card number encription 
    /// </summary>
    /// <param name="organizationID"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    internal static bool IsEncoded(string databaseName)
    {
        string query = string.Format(@"Select case when IsEncodedCards  = 1 then convert (bit, 1) else convert (bit, 0) end 
                                        From Organizations Where DBName = '{0}'", databaseName);
        object result = DataBase.ExecuteScalar(query);
        return (bool)result;
    }

    /// <summary>
    /// get the member ID by card number when the card number is encrypted
    /// </summary>
    /// <param name="dbName"></param>
    /// <param name="cardNumber"></param>
    /// <returns></returns>
    internal static string GetMemberIDByCardNumberEncrypted(string DBName, string cardNumber)
    {
        string OpenEncription = "OPEN SYMMETRIC KEY DtsKey2010 DECRYPTION BY CERTIFICATE DtsCertificate";
        string CloseEncription = "CLOSE SYMMETRIC KEY DtsKey2010";

        string SearchQuery = string.Format("SELECT IDMember FROM {0}..Cards where CONVERT(VARCHAR(50),DECRYPTBYKEY(EncryptedCard))  = '{1}' OR CardNumber = '{1}'",
                                                                DBName, cardNumber);
        string query = string.Format("{0} {1} {2}", OpenEncription, SearchQuery, CloseEncription);
        try
        {
            Object obj = DataBase.ExecuteScalar(query);
            if (obj == null)
                return "2";
            else
                return obj.ToString();
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetMemberIDByCardNumberEncrypted \n DBName: " + DBName + "\nCard number: " + cardNumber +
                "Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
    }

    /// <summary>
    /// get the card number by member ID when the card number is encrypted
    /// </summary>
    /// <param name="dbName"></param>
    /// <param name="cardNumber"></param>
    /// <returns></returns>
    internal static string GetCardNumberByMemberIDEncrypted(string DBName, string memberID)
    {
        string OpenEncription = "OPEN SYMMETRIC KEY DtsKey2010 DECRYPTION BY CERTIFICATE DtsCertificate";
        string CloseEncription = "CLOSE SYMMETRIC KEY DtsKey2010";

        string SearchQuery = string.Format(@"SELECT CONVERT(VARCHAR(50),DECRYPTBYKEY(EncryptedCard))
                                                FROM {0}..Cards 
                                                where IDMember = '{1}'",
                                                                DBName, memberID);
        string query = string.Format("{0} {1} {2}", OpenEncription, SearchQuery, CloseEncription);
        try
        {
            Object obj = DataBase.ExecuteScalar(query);
            if (obj == null)
                return null;
            else
                return obj.ToString().Trim();
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetCardNumberByMemberIDEncrypted \n DBName: " + DBName + "\nmemberID: " + memberID +
                "Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
    }

    internal static void EncryptCardNumber(string dbName, string cardNumber, string tableName)
    {
        string cutSubString = "4";
        string cutLengh = "3";
        if (tableName == "CampaignMembersUse")
        {
            cutSubString = "6";
            cutLengh = "5";
        }
        string OpenEncription = "OPEN SYMMETRIC KEY DtsKey2010 DECRYPTION BY CERTIFICATE DtsCertificate";
        string CloseEncription = "CLOSE SYMMETRIC KEY DtsKey2010";

        // Encrypt the card number 
        string encyptrQuery = string.Format(@"update {0}..{2} set EncryptedCard = ENCRYPTBYKEY(KEY_GUID('DtsKey2010'), '{1}')
                    where CardNumber = '{1}'", dbName, cardNumber, tableName);

        string updateQuery = string.Format(@"update {0}..{2} set CardNumber = SUBSTRING (CardNumber, Len(CardNumber)- " + cutLengh + ", " + cutSubString + ") Where CardNumber = '{1}'",
                dbName, cardNumber, tableName);
        string query = string.Format("{0} {1} {2} {3} ", OpenEncription, encyptrQuery, CloseEncription, updateQuery);
        try
        {
            DataBase.ExecuteNonQuery(query);
        }
        catch (Exception ex)
        {
            SendMail("Function -> EncryptCardNumber \n DBName: " + dbName + "\nCard number: " + cardNumber +
                "Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
        }

    }
    internal static string ThereIsCardForMemberID(string dbName, string MemberID)
    {
        return ThereIsCardForMemberID(dbName, MemberID, "");
    }
    internal static string ThereIsCardForMemberID(string dbName, string MemberID, string last4digit)
    {
        string back = "0";
        string query;

        if (!string.IsNullOrEmpty(last4digit))
            last4digit = " AND CardNumber = '" + last4digit + "'";

        try
        {
            query = string.Format("IF EXISTS (SELECT IDMember FROM {0}..Cards WHERE IDMember = '{1}' {2}) SELECT '1' ELSE SELECT '2'", dbName, MemberID, last4digit);
            back = (string)DataBase.ExecuteScalar(query);
            return back;
        }
        catch (Exception ex)
        {
            SendMail("Function -> ThereIsCardForMemberID \n dbName -> " + dbName + "\n MemberID -> " + MemberID
                + "\n Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
    }

    internal static int AddCardForMemberID(string dbName, string MemberID, string CardNumber)
    {
        string updateQuery = string.Format(
                                 @"INSERT INTO {0}..Cards
                                                    (CardNumber,IDMember,CardStatus,AddedTime)
                                                VALUES
                                                    ('{1}', '{2}', 1, GetDate())",
                                 dbName, CardNumber, MemberID);
        //        if (encriptionRequired)
        //        {
        //            updateQuery = string.Format(
        //                              @"OPEN SYMMETRIC KEY DtsKey2010 DECRYPTION BY CERTIFICATE DtsCertificate
        //                                
        //                                declare @cardNO nvarchar(50);
        //                                declare @lengh_cardNO tinyint;
        //                                set @cardNO = '{1}'
        //                                set @lengh_cardNO = len(@cardNO)
        //                                
        //                                INSERT INTO {0}..Cards
        //                                                    (CardNumber,IDMember,CardStatus,AddedTime,EncryptedCard)
        //                                                VALUES
        //                                                    (SUBSTRING ( @cardNO , @lengh_cardNO - 3 , 4), '{2}', 1, GetDate(),ENCRYPTBYKEY(KEY_GUID('DtsKey2010'), '{1}'))
        //                                CLOSE SYMMETRIC KEY DtsKey2010",
        //                              dbName, CardNumber, MemberID);
        //        }
        //        else
        //        {
        //            updateQuery = string.Format(
        //                                 @"INSERT INTO {0}..Cards
        //                                                    (CardNumber,IDMember,CardStatus,AddedTime)
        //                                                VALUES
        //                                                    ('{1}', '{2}', 1, GetDate())",
        //                                 dbName, CardNumber, MemberID);
        //        }
        try
        {
            return DataBase.ExecuteNonQuery(updateQuery);
        }

        catch (Exception ex)
        {
            SendMail("Function -> AddCardForMemberID \n Exception TYPE -> " + ex.GetType()
                + "\n MemberID -> " + MemberID
                + "\n CardNumber -> " + CardNumber
                + "\n dbName -> " + dbName
                //+ "\n encriptionRequired -> " + encriptionRequired
                + "\n Exception Message -> " + ex.Message
                , "Leumi Campaign");
        }
        return 0;
    }

    internal static void GetCardDetails(string DBName, string cardNumber, DataTable table)
    {
        string query = string.Format(@"Select IDMember, TZ, Last4Digits, CardStatus , MemberStatus From {0}..Cards inner join {0}..AllMembers 
            On IDMEmber = MemberID and CardNumber = '{1}'", DBName, cardNumber.Trim());
        try
        {
            DataBase.ExecuteAdapter(query, table);
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetCardDetails"
                            + "\n dbName -> " + DBName
                            + "\n cardNumber -> " + cardNumber
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "WS Error");
        }
    }

    internal static void getBenefitLimitProp(string CampaignID, ref DataTable dtLimits)
    {
        //no need to check BenefitAccountLimit, Max one load on tranzaction
        string query = @"SELECT 0, BenefitID, 0, HasLimits, BenefitGeneralLimit, BenefitPersonLimit,  
                        LimitClient1, LimitClient2, LimitClient3, LimitClient4, LimitClient5, 
                        LimitBenefit1, LimitBenefit2, LimitBenefit3, LimitBenefit4, LimitBenefit5
                        FROM CampaignBenefits 
                        WHERE (CampaignBenefits.CampaignID = " + CampaignID + ")";
        try
        {
            DataBase.ExecuteAdapter(query, ref dtLimits);
        }
        catch (Exception ex)
        {
            SendMail("Function -> getBenefitLimitProp"
                            + "\n CampaignID -> " + CampaignID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "WS Error");
        }
    }



    internal static long CheckLimitCouponsStock(string memberID, int stockCoupon, bool general, string columnsName, string CampaignID)
    {
        string checkMember = "";
        string timeFilter = "";
        object obj = null;
        try
        {
            if (!general)
                checkMember = " AND MemberID = '" + memberID + "'";

            //if (!string.IsNullOrEmpty(CampaignID))//בדיקת מגבלת קמפיין גם בטעינה
            //    checkMember += " AND CampaignID = '" + CampaignID + "'";

            timeFilter = BuildFilter(columnsName);
            if (!string.IsNullOrEmpty(timeFilter))
                timeFilter = string.Format(timeFilter, "SendingTime");

            string query = string.Format(
                        @"Select Count(CouponID)
                            From CouponsStock
                            Where CouponStatus = 1
                            AND CampaignID = {0} {1} {2}", CampaignID, checkMember, timeFilter);
            //AND StockID = {0} {1} {2}", stockCoupon, checkMember, timeFilter);
            obj = DataBase.ExecuteScalar(query);
            if (obj != null && obj != DBNull.Value)
                return Convert.ToInt64(obj);
        }
        catch (Exception ex)
        {
            SendMail("Function -> CheckLimitCouponsStock"
                            + "\n memberID -> " + memberID
                            + "\n stockCoupon -> " + stockCoupon.ToString()
                            + "\n general -> " + general.ToString()
                            + "\n columnsName -> " + columnsName
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "WS Error");
        }
        return 0;
    }



    internal static long CheckLimitCampaignMembersUse(string dbName, long benefitID, string CampaignID, string memberID, bool general, string columnsName)
    {
        string checkMember = "";
        string timeFilter = "";
        object obj = null;
        try
        {
            if (!general)
                checkMember = " AND ID = '" + memberID + "'";

            timeFilter = BuildFilter(columnsName);
            if (!string.IsNullOrEmpty(timeFilter))
                timeFilter = string.Format(timeFilter, "UseTime");

            string query = string.Format(
                        @"Select Sum(NumOfUse)
                            From {0}..CampaignMembersUse
                            Where CampaignID = {1}
                            AND BenefitID = {2} {3} {4}", dbName, CampaignID, benefitID, checkMember, timeFilter);
            obj = DataBase.ExecuteScalar(query);
            if (obj != null && obj != DBNull.Value)
                return Convert.ToInt64(obj);
        }
        catch (Exception ex)
        {
            SendMail("Function -> CheckLimitCampaignMembersUse"
                            + "\n memberID -> " + memberID
                            + "\n dbName -> " + dbName
                            + "\n memberID -> " + memberID
                            + "\n benefitID -> " + benefitID
                            + "\n CampaignID -> " + CampaignID
                            + "\n columnsName -> " + columnsName
                            + "\n general -> " + general.ToString()
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "WS Error");
        }
        return 0;
    }

    private static string BuildFilter(string limitType)
    {
        switch (limitType)
        {
            case "LimitClient5"://DailyLimit
            case "LimitBenefit5":
                return " AND (DATEDIFF(day, GETDATE(), {0}) = 0)";
            case "LimitClient4"://WeeklyLimit
            case "LimitBenefit4":
                return " AND (DATEDIFF(week, GETDATE(), {0}) = 0)";
            case "LimitClient3"://MonthlyLimit
            case "LimitBenefit3":
                return " AND (DATEDIFF(month, GETDATE(), {0}) = 0)";
            case "LimitClient2"://QuarterLimit
            case "LimitBenefit2":
                return " AND (DATEDIFF(quarter, GETDATE(), {0}) = 0)";
            case "LimitClient1"://YearlyLimit
            case "LimitBenefit1"://YearlyLimit
                return " AND (DATEDIFF(year, GETDATE(), {0}) = 0)";
        }
        return "";
    }

    internal static void GetAllNetsWithCardTamplate(ref DataTable dt)
    {
        //DataTable dt = new DataTable();
        try
        {
            DataBase.ExecuteAdapter(@"SELECT Organizations.OrganizationID, Nets.IDCodeTemplate, Nets.NetDB
                                            FROM Nets INNER JOIN Organizations 
                                            ON Nets.NetDB = Organizations.DBName
                                            WHERE (Nets.IVROrder = 1)", ref dt);
        }
        catch (Exception ex)
        {
            SendMail("SqlMethods: Methode:GetAllNetsWithCardTamplate\n ex.Message: " + ex.Message
                + "\nex.Source: " + ex.Source
                + "\nex.StackTrace: " + ex.StackTrace);
        }
        //return dt;
    }

    internal static string GetErrorMessage(string errorID)
    {
        try
        {
            string query = string.Format("Select Message From WSErrors Where ErrorID = {0}", errorID);
            object obj = DataBase.ExecuteScalar(query);
            if (obj != null && obj != DBNull.Value)
                return Convert.ToString(obj);
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetErrorMessage \n errorID -> " + errorID
                + "\n Exception TYPE -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        return "";
    }

    internal static void GetOrgDetails(string orgID, ref string password, ref string smsFromPhone)
    {
        SqlDataReader reader = null;
        SqlConnection con = new SqlConnection();
        try
        {
            string query = string.Format("Select Password, SmsMobile From Organizations Where OrganizationID = {0}", orgID);

            reader = DataBase.ExecuteReader(query, con);

            while (reader.Read())
            {
                if (reader["Password"] != DBNull.Value && reader["Password"] != null)
                    password = reader["Password"].ToString();

                if (reader["SmsMobile"] != DBNull.Value && reader["SmsMobile"] != null)
                    smsFromPhone = reader["SmsMobile"].ToString();
            }

            //object obj = DataBase.ExecuteScalar(query);
            //if (obj != null && obj != DBNull.Value)
            //    return Convert.ToString(obj);
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetOrgDetails \n OrganizationID -> " + orgID
                + "\n Exception TYPE -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message
                + "\n password -> " + password
                + "\n smsFromPhone -> " + smsFromPhone);
        }
        finally
        {
            if (reader != null)
            {
                if (!reader.IsClosed)
                    reader.Close();
            }
            if (con.State != ConnectionState.Closed)
                con.Close();
        }
    }

    internal static string GetDBNameForOrgID(string orgID)
    {
        try
        {
            string query = string.Format("Select DBName From Organizations Where OrganizationID = {0}", orgID);
            object obj = DataBase.ExecuteScalar(query);
            if (obj != null && obj != DBNull.Value)
                return Convert.ToString(obj);
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetDBNameForOrgID \n OrganizationID -> " + orgID
                + "\n Exception TYPE -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        return "";
    }

    public static string GetMessageTextByMessageID(string MessageID)
    {
        string Message = "הודעה כללית";
        int convert;
        if (int.TryParse(MessageID, out convert))
        {
            string Query = @"SELECT [MessageText] FROM [DTS_OnLine].[dbo].[Messages] WHERE MessageID='" + MessageID + "'";
            try
            {
                Message = DataBase.ExecuteScalar(Query).ToString();
            }
            catch (Exception ex)
            {
                Message = "הודעת שגיאה כללית";
            }
        }
        return Message;
    }

    //internal static string GetMemberCardNumberMiluim(string dbName, string cardNumber)
    //{
    //    try
    //    {
    //        string query = string.Format("Select MemberCardNumber From {0}..AllMembers Where MemberId = '{1}'", dbName, cardNumber);
    //        object obj = DataBase.ExecuteScalar(query);
    //        if (obj != null && obj != DBNull.Value)
    //            return Convert.ToString(obj);
    //    }
    //    catch (Exception ex)
    //    {
    //        SendMail("Function -> GetMemberCardNumberMiluim \n dbName -> " + dbName
    //            + "\n cardNumber -> " + cardNumber
    //            + "\n Exception TYPE -> " + ex.GetType()
    //            + "\n Exception Message -> " + ex.Message);
    //        return "19";
    //    }
    //    return "";
    //}

    internal static void GetSubTypeLimits(string dbName, Int16 subType, ref DataTable dtSubTypeLimit)
    {
        string query = string.Format(
                    @"SELECT *
                        FROM {0}..BusinessSubTypeLimits
                        WHERE BusinessSubTypeId = {1}", dbName, subType);
        try
        {
            DataBase.ExecuteAdapter(query, ref dtSubTypeLimit);
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetSubTypeLimits"
                            + "\n dbName -> " + dbName
                            + "\n subType -> " + subType
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "WS Error");
        }
    }

    internal static string GetProductIDFromIVRCode(string databaseName, string productID)
    {
        try
        {
            string query = string.Format("Select FullBarCode From {0}..ProductsVars Where IVRCode = '{1}'", databaseName, productID);
            object obj = DataBase.ExecuteScalar(query);
            if (obj != null && obj != DBNull.Value)
                return Convert.ToString(obj);
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetProductIDFromIVRCode \n databaseName -> " + databaseName
                + "\n productID -> " + productID
                + "\n Exception TYPE -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        return "";
    }

    internal static int UpdateMemberName(string dbName, string MemberID, string MemberFirstName, string MemberLastName, string SmsPhoneNumber, string email)
    {
        string fullName = "";

        CheckMemberProp(ref fullName, ref MemberFirstName, ref MemberLastName, ref SmsPhoneNumber, ref email);

        string updateQuery = string.Format(@"UPDATE {0}..AllMembers
                                                SET MemberName = {2},MemberFirstName = {3},MemberLastName = {4},MobilePhone = {5}, Email = {6}
                                                WHERE MemberId = '{1}'"
                                        , dbName, MemberID, fullName, MemberFirstName, MemberLastName, SmsPhoneNumber, email);
        try
        {
            return DataBase.ExecuteNonQuery(updateQuery);
        }

        catch (Exception ex)
        {
            SendMail("Function -> UpdateMemberName \n ExceptionTYPE -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message
                + "\n dbName -> " + dbName
                + "\n MemberID -> " + MemberID
                + "\n MemberFirstName -> " + MemberFirstName
                + "\n MemberLastName -> " + MemberLastName
                + "\n Email -> " + email
                + "\n SmsPhoneNumber -> " + SmsPhoneNumber, "Leumi Campaign");
            return 19;
        }
    }

    internal static void IsPaymentIDExsits(string dbName, string debitAsmacta, ref string last4Digits, ref string charged, ref string cardOwnerID, string memberID)
    {
        SqlConnection connection = new SqlConnection();
        SqlDataReader reader = null;
        DataTable dt = new DataTable();
        try
        {
            reader = DataBase.ExecuteReader(
                string.Format(@"SELECT CardOwnerID, Last4Digits, Charged
                            FROM {0}..Payments
                                WHERE PaymentID = '{1}' AND MemberID = '{2}'", dbName, debitAsmacta, memberID), connection);
            if (reader.Read())
            {
                if (reader["CardOwnerID"] != DBNull.Value)
                    cardOwnerID = reader["CardOwnerID"].ToString();
                if (reader["Last4Digits"] != DBNull.Value)
                    last4Digits = reader["Last4Digits"].ToString();
                if (reader["Charged"] != DBNull.Value)
                    charged = reader["Charged"].ToString();
            }
        }
        catch (Exception ex)
        {
            SendMail("Function -> IsPaymentIDExsits "
                + "\n dbName -> " + dbName
                + "\n debitAsmacta -> " + debitAsmacta
                + "\n Exception TYPE -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message);
        }
        finally
        {
            if (reader != null)
            {
                if (!reader.IsClosed)
                    reader.Close();
            }
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }
    }

    internal static long AddToGlobalCoupon(string MemberID, string CampaignID, string SeventhCardDigit, string SmsPhoneNumber)
    {
        try
        {
            string query = string.Format(
                            @"INSERT INTO CouponsStock
							(CouponCode,CouponStatus,StockID,MemberID,SendingTime,CampaignID,SeventhCardDigit,PhoneNumber)
							SELECT Max(CouponCode) + 1, 1, 62, '{0}', GetDate(), '{1}', '{2}', '{3}'
                            FROM CouponsStock 
                            WHERE StockID = '62'
							SELECT SCOPE_IDENTITY()", MemberID, CampaignID, SeventhCardDigit, SmsPhoneNumber);
            Object result = DataBase.ExecuteScalar(query);
            if (result != DBNull.Value && result != null)
                return Convert.ToInt64(result);

        }
        catch (Exception ex)
        {
            SendMail("Function -> AddToGlobalCoupon"
                            + "\n CampaignID -> " + CampaignID
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "Leumi Campaign");
        }
        return 0;
    }

    internal static double GetYearlyBenefitCostForMemberID(string MemberID)
    {
        try
        {
            string query = string.Format(@"SELECT SUM(CampaignBenefits.CustomerBenefitCost)
                                            FROM CouponsStock INNER JOIN CampaignBenefits 
                                            ON CouponsStock.CampaignID = CampaignBenefits.CampaignID
                                            JOIN CouponsStocksDetails ON CouponsStocksDetails.StockID = CouponsStock.StockID
                                            WHERE MemberID = '{0}' AND SendingTime >= '{1}-01-01'
                                            AND (CouponsStocksDetails.OrganizationID = 28 OR CouponsStock.StockID = 62)", MemberID, DateTime.Now.Year);
            object obj = DataBase.ExecuteScalar(query);
            if (obj != null && obj != DBNull.Value)
                return Convert.ToDouble(obj);
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetYearlyBenefitCostForMemberID "
                + "\n Exception TYPE -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message
                + "\n MemberID -> " + MemberID);
            return -1;
        }
        return 0;

    }

    internal static double GetCampBenefitCost(string CampaignID)
    {
        try
        {
            string query = string.Format(@"SELECT CustomerBenefitCost
                                            FROM CampaignBenefits 
                                            WHERE CampaignID = '{0}'", CampaignID);
            object obj = DataBase.ExecuteScalar(query);
            if (obj != null && obj != DBNull.Value)
                return Convert.ToDouble(obj);
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetCampBenefitCost "
                + "\n Exception TYPE -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message
                + "\n CampaignID -> " + CampaignID);
            return -1;
        }
        return 0;

    }



    //    internal static int UpdateValueCardTranIDForOrder(string dbName, string MemberID, string productID, string back, long valueCardTranID)
    //    {

    //        string updateQuery = string.Format(@"UPDATE {0}..ATRACTIONSOrders
    //                                                SET ValueCardTranID = {2}
    //                                                WHERE MemberId = '{1}' 
    //                                                AND BarCode = '{3}'
    //                                                AND MemberOrderAsmchta = 
    //                                                                        (
    //                                                                            Select TTransactionOrder
    //                                                                            From {0}..WebServiceTransaction
    //                                                                            Where TTransactionID = {4} 
    //                                                                        )"
    //                                        , dbName, MemberID, valueCardTranID, productID, back);
    //        try
    //        {
    //            return DataBase.ExecuteNonQuery(updateQuery);
    //        }

    //        catch (Exception ex)
    //        {
    //            SendMail("Function -> UpdateValueCardTranIDForOrder \n ExceptionTYPE -> " + ex.GetType()
    //                + "\n Exception Message -> " + ex.Message
    //                + "\n dbName -> " + dbName
    //                + "\n MemberID -> " + MemberID
    //                + "\n productID -> " + productID
    //                + "\n TTransactionID -> " + back
    //                + "\n valueCardTranID -> " + valueCardTranID
    //                , "UpdateValueCardTranID");
    //            return 0;
    //        }
    //    }

    internal static void AddToValueCardTL_ATROrders(string dbName, long valueCardTranID, long atractionIdentity)
    {
        try
        {
            string query = string.Format(
                            @"INSERT INTO {0}..ValueCardTL_ATROrders
                                   (ValueCardTL_ID,ATROrderAsmchta)
                             VALUES
                                   ({1}, {2})"
                            , dbName, valueCardTranID, atractionIdentity);
            DataBase.ExecuteScalar(query);
        }
        catch (Exception ex)
        {
            SendMail("AddToValueCardTL_ATROrders -> \n dbName -> " + dbName
                + "\n valueCardTranID -> " + valueCardTranID
                + "\n atractionIdentity -> " + atractionIdentity
                + "\n Exception Type -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message
                + "\n ex.StackTrace -> " + ex.StackTrace
                + "\n ex.TargetSite -> " + ex.TargetSite);
        }
    }

    internal static long InsertToValueCardTranLog(OrderDll.VCService.VlcOrderData orderData)
    {
        try
        {
            string query = string.Format(
                            @"INSERT INTO ValueCardTranLog
                                (DateAdded,OrganizationID,MemberID,PromoID,Opcode,WaitingOrder,AmountPaid,QuantityOrdered)
                             VALUES
                                   (GetDate(), {0}, '{1}', {2}, {3}, '{4}', {5}, {6})
                            SELECT SCOPE_IDENTITY()"
                            , orderData.OrganizationID, orderData.MemberID, orderData.PromoID, 1, Crypto.EncryptStringAES(orderData.CardNumber, "Vl2013Cd"), orderData.AmountPaid, orderData.QuantityOrdered);
            Object result = DataBase.ExecuteScalar(query);
            if (result != DBNull.Value && result != null)
                return Convert.ToInt64(result);

        }
        catch (Exception ex)
        {
            SendMail("InsertToValueCardTranLog -> \n OrganizationID -> " + orderData.OrganizationID
                + "\n MemberID -> " + orderData.MemberID
                + "\n ProductID -> " + orderData.PromoID
                + "\n Quantity -> " + orderData.QuantityOrdered
                + "\n Exception Type -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message
                + "\n ex.StackTrace -> " + ex.StackTrace
                + "\n ex.TargetSite -> " + ex.TargetSite);
        }
        return 0;
    }

    internal static long GetValueCardTranID(string dbName, long vltranIdToCancel)
    {
        try
        {
            string query = string.Format(@"SELECT ValueCardTL_ID
                                            FROM {0}..ValueCardTL_ATROrders
                                            WHERE ATROrderAsmchta = {1}", dbName, vltranIdToCancel);
            object obj = DataBase.ExecuteScalar(query);
            if (obj != null && obj != DBNull.Value)
                return Convert.ToInt64(obj);
        }
        catch (Exception ex)
        {
            SendMail("Function -> GetValueCardTranID "
                + "\n vltranIdToCancel -> " + vltranIdToCancel
                + "\n Exception TYPE -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message
                + "\n ex.StackTrace -> " + ex.StackTrace
                + "\n ex.TargetSite -> " + ex.TargetSite);
            return -1;
        }
        return 0;
    }

    internal static bool insertCouponData(Coupon coupon, string MemberID, SqlCommand cmd)
    {
        if (coupon.isInternalCoupon || string.IsNullOrEmpty(coupon.CouponCode))
            cmd.CommandText = "InsertInternalCoupon_CouponStock";
        else
            cmd.CommandText = "InsertExternalCoupon_CouponStock";

        cmd.CommandType = CommandType.StoredProcedure;

        SqlParameter p = new SqlParameter("@CouponCode", SqlDbType.VarChar);
        p.Value = coupon.CouponCode;
        SqlParameter p1 = new SqlParameter("@StockID", SqlDbType.Int);
        p1.Value = coupon.CouponStockID;
        SqlParameter p2 = new SqlParameter("@MemberID", SqlDbType.NVarChar);
        p2.Value = MemberID;
        SqlParameter p3 = new SqlParameter("@SendingTime", SqlDbType.DateTime);
        p3.Value = coupon.CouponOrderTime;
        SqlParameter p4 = new SqlParameter("@CouponID", SqlDbType.BigInt);
        p4.Direction = ParameterDirection.Output;

        cmd.Parameters.Add(p);
        cmd.Parameters.Add(p1);
        cmd.Parameters.Add(p2);
        cmd.Parameters.Add(p3);
        cmd.Parameters.Add(p4);

        var result = DataBase.ExecuteScalarForTransaction(cmd.CommandText, cmd);

        cmd.CommandType = CommandType.Text;

        long tmp;
        if (cmd.Parameters["@CouponID"].Value != null &&
            long.TryParse(cmd.Parameters["@CouponID"].Value.ToString(), out tmp) && tmp <= 0)




            //        if ((long)cmd.Parameters["@CouponID"].Value <= 0)
            return false;// "80";

        var value = cmd.Parameters["@CouponID"].Value;
        if (value != null)
            coupon.CouponStockIdentity = ((long)value).ToString();
        return true;

    }

    /// <summary>
    /// check if ip in IncapsulaIPs ranges
    /// </summary>
    /// <param name="requestIp"></param>
    /// <returns></returns>
    internal static bool ValidateIncapsulaIP(string requestIp)
    {
        var result = false;

        SqlCommand commnad = new SqlCommand();
        commnad.CommandType = CommandType.StoredProcedure;
        commnad.CommandText = "ValidateIncapsulaIP";
        SqlParameter param = new SqlParameter();
        param = commnad.Parameters.Add("@IpAddress", SqlDbType.VarChar);
        param.Direction = ParameterDirection.Input;
        param.Value = requestIp;
        try
        {
            var ret = DataBase.ExecuteScalar(commnad);
            result = (int)ret > 0;
        }
        catch (Exception ex)
        {
            SendMail("Function -> ValidateIncapsulaIP \n requestIp: -> " + requestIp
                + "\n Exception TYPE -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message);
        }

        return result;
    }

}

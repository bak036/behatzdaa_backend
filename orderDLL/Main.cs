using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Xml;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System.Data.SqlClient;
using System.Diagnostics;
using OrderDll.creditguard;
using System.Collections.Generic;
using System.Xml.Linq;
using OrderDll.VCService;
using OrderDll;
/// <summary>
/// Summary description for Main
/// </summary>
public class Main
{
    private static string token = string.Empty;
    private static DateTime tokenCreateTime = DateTime.Now.AddDays(-1);

    private static readonly int TokenLifeTime = GetToeknLifeTime();

    public static string LocalNetworkIP = "192.168";

    private static int GetToeknLifeTime()
    {
        int lifeTime;
        try
        {
            lifeTime = Convert.ToInt32(ConfigurationManager.AppSettings["TokenLifeTimeInMinutes"]);
        }
        catch (Exception)
        {
            lifeTime = 5;
        }
        return lifeTime;
    }

    public Main(string a)
    { }

    public static string InsertNewOrderLeumiCard_GCPointsWithConnction(int? PremiumType, string ProductID, string MemberID, string Quantity
        , string MetaData, string CardNumber)
    {
        string ans = null;
        try
        {
            //DataBase.OpenConnection();
            ans = InsertNewOrderLeumiCard_GCPoints(PremiumType, ProductID, MemberID, Quantity, MetaData, CardNumber);
            return ans;
        }
        catch (Exception ex)
        {

        }
        finally
        {
            //DataBase.CloseConnection();
        }
        return ans;
    }

    public static void EncryptCardNumber(string dbName, string CardNumber)
    {
        SqlMethods.EncryptCardNumber(dbName, CardNumber, "Cards");
    }


    public static string InsertNewOrderLeumiCard_GCPoints(int? PremiumType, string ProductID, string MemberID, string Quantity, string MetaData, string CardNumber)
    {
        string typeOrder = null, back = null, error = null, dbName = null, MarketingCommission = "null", posBarCode = string.Empty;
        bool isCampaign = false, splitOrder = false;
        int GroupLimitsID = 0, variantType = 0;


        //TRIM()
        if (!string.IsNullOrEmpty(MetaData))
            MetaData = MetaData.Trim();
        if (!string.IsNullOrEmpty(CardNumber))
            CardNumber = CardNumber.Trim();

        string OriginalOrderID = "1-1";

        //Check Error's
        error = CheckInputValues(ref ProductID, ref MemberID, ref Quantity, ref OriginalOrderID);
        if (!string.IsNullOrEmpty(error))
            return error;

        try
        {
            //DataBase.OpenConnection();
            dbName = "LeumiCard_GCPoints";
            int implementationType = -1;
            DateTime lastImplementationDate = DateTime.MaxValue;

            //Check If The Variant Exists
            typeOrder = SqlMethods.CheckIfVariantExists(dbName, ProductID, ref isCampaign, ref GroupLimitsID,
                                ref implementationType, ref lastImplementationDate, ref splitOrder, ref variantType, ref posBarCode);
            if (typeOrder == "19")//Connection Lost
                return typeOrder;
            else if (string.IsNullOrEmpty(typeOrder))
                return "6";//ProductID Not Exists
            else if (typeOrder == "28")
                return typeOrder;//ProductID Not Enable

            //Insert The Order
            back = SqlMethods.InsertNewOrder(PremiumType, dbName, typeOrder, MemberID, ProductID, Quantity, OriginalOrderID,
                CardNumber, "", true, //true - to insert card number to order
                MetaData, "0", "0", "0", "0", "0", "0", isCampaign, GroupLimitsID, MarketingCommission, "-1",
                DateTime.MaxValue.ToString("yyyy-MM-dd HH:mm:ss"));
            if (back == "0")
                return "19";
            return back;

        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("InsertNewOrderLeumiCard_GCPoints -> \n dbName -> " + dbName + "\n typeOrder -> " + typeOrder
                + "\n MemberID -> " + MemberID
                + "\n ProductID -> " + ProductID + "\n Quantity -> " + Quantity
                + "\n CardNumber -> " + CardNumber + "\n back -> " + back
                + "\n Exception Type -> " + ex.GetType() + "\n Exception Message -> " + ex.Message, "LC_SummerIVR");
            return "19";
        }
        //finally
        //{
        //    DataBase.CloseConnection();
        //}
    }



    public static string InsertNewOrder(int? PremiumType, string OrganizationID, string Password, string ProductID, string MemberID, string Quantity,
        string OriginalOrderID, string MetaData, string Reason, string ipUserHost, int coins = 0, GeneralOrdersItem OrderItem = null, string dtsRedimCode = "", DateTime? lastImplementationDate = null, string parentMultiVariant = "", StringBuilderLogger logTracking = null)
    {
        return InsertNewOrder(PremiumType, OrganizationID, Password, ProductID, MemberID, Quantity, OriginalOrderID, MetaData, Reason, ipUserHost, "-1", coins, OrderItem, dtsRedimCode, lastImplementationDate, parentMultiVariant, logTracking);

    }

    public static string InsertNewOrder(int? PremiumType, string OrganizationID, string Password, string ProductID, string MemberID, string Quantity,
        string OriginalOrderID, string MetaData, string Reason, string ipUserHost, string paymentID, int coins = 0, GeneralOrdersItem OrderItem = null, string dtsRedimCode = "", DateTime? lastImplementationDate = null, string parentMultiVariant = "", StringBuilderLogger logTracking = null)
    {
        return InsertNewOrder( PremiumType, OrganizationID, Password, ProductID, MemberID, Quantity, OriginalOrderID, MetaData, Reason, ipUserHost, paymentID, 0, coins, OrderItem, dtsRedimCode, lastImplementationDate, parentMultiVariant, logTracking);

    }

    public static string InsertNewOrder(int? PremiumType, string OrganizationID, string Password, string ProductID, string MemberID, string Quantity,
        string OriginalOrderID, string MetaData, string Reason, string ipUserHost, string paymentID, long Cid, int coins = 0, GeneralOrdersItem OrderItem = null, string dtsRedimCode = "", DateTime? lastImplementationDate = null, string parentMultiVariant = "", StringBuilderLogger logTracking = null)
    {
        List<string> ValueCardTL_ID = null;
        return InsertNewOrder(PremiumType, OrganizationID, Password, ProductID, MemberID, Quantity, OriginalOrderID, MetaData, Reason, ipUserHost, paymentID, Cid, null, ref ValueCardTL_ID, coins, OrderItem, dtsRedimCode, lastImplementationDate, parentMultiVariant, logTracking);
    }

    public static string InsertNewOrder(int? PremiumType, string OrganizationID, string Password, string ProductID, string MemberID, string Quantity,
       string OriginalOrderID, string MetaData, string Reason, string ipUserHost, string paymentID, long Cid, string payCardNumber, ref List<string> asmchtaORCoupon, int coins = 0, GeneralOrdersItem OrderItem = null, string dtsRedimCode = "", DateTime? lastImplementationDate = null, string parentMultiVariant = "", StringBuilderLogger logTracking = null)
    {
        bool? IsCancelSeatssucceed = null; //NOT RELAVENT! 
        return InsertNewOrder(PremiumType, OrganizationID, Password, ProductID, MemberID, Quantity, OriginalOrderID, MetaData, Reason, ipUserHost, paymentID, Cid, payCardNumber, ref asmchtaORCoupon, ref IsCancelSeatssucceed, coins, OrderItem, dtsRedimCode, lastImplementationDate, parentMultiVariant, logTracking);

    }

    public static string InsertNewOrder(int? PremiumType, string OrganizationID, string Password, string ProductID, string MemberID, string Quantity,
        string OriginalOrderID, string MetaData, string Reason, string ipUserHost, string paymentID, long Cid, string payCardNumber, ref List<string> asmchtaORCoupon, ref bool? IsCancelSeatssucceed, int coins = 0, GeneralOrdersItem OrderItem = null, string dtsRedimCode = "", DateTime? _lastImplementationDate = null, string parentMultiVariant = "", StringBuilderLogger logTracking = null)
    {
        string typeOrder = null, back = null, error = null, dbName = null, cardNumber = null, mainField = null;
        string MarketingCommission = "null", cancelComission = "0", asmachta = string.Empty, posBarCode = string.Empty;
        bool enforcementIdentity = true, ActiveCardNotNeededToOrder = false, splitOrder = false, isCampaign = false;
        int GroupLimitsID = 0, implementationType = -1, variantType = 0;
        bool MarkSeats = false; int BusinessSubTypeID = 0;// cancel MarkSeats Params
        SqlMethods.Coupon Coupon = null; bool isTradeSite = false; bool isOrderTable = false;

        if (logTracking == null) logTracking = new StringBuilderLogger();


        DataTable dtVar = new DataTable();
        XmlDocument xmlDoc = new XmlDocument();
        DateTime lastImplementationDate = DateTime.MaxValue;
        if (logTracking != null)
        {
            logTracking.AppendLine("Tracking OrderDLL:");
        }

        if (_lastImplementationDate.HasValue)
            lastImplementationDate = _lastImplementationDate.Value;



        //Check OrganizationID Empty
        if (string.IsNullOrEmpty(OrganizationID))
            return "22";//חסר קוד אירגון
        else
            OrganizationID = OrganizationID.Trim();

        //Check Password Empty
        if (string.IsNullOrEmpty(Password))
            return "10";//חסרה סיסמה      
        else
            Password = Password.Trim();

        //Check Error's
        error = CheckInputValues(ref ProductID, ref MemberID, ref Quantity, ref OriginalOrderID);
        if (!string.IsNullOrEmpty(error))
            return error;

        //TRIM()
        if (!string.IsNullOrEmpty(MetaData))
            MetaData = MetaData.Trim();
        if (!string.IsNullOrEmpty(ipUserHost))
            ipUserHost = ipUserHost.Trim();

        int quant = Convert.ToInt32(Quantity);

        try
        {

            logTracking.AppendLine($"ipUserHost:{ipUserHost}");

            bool methodPremission = false, AllowAnonymousTransactions = false;
            string orgUserID = "", ipList = "";
            //Check Valid Organization
            if (!MetaData.Contains("FromGiftCard")) //do not check Organization Login to our servers
            {
                //do not check Organization Login to our servers OR localhost
                if (ipUserHost != "::1")
                    if (!ipUserHost.StartsWith(LocalNetworkIP)
                        || ipUserHost == "127.0.0.1")
                    {
                        SqlMethods.OrganizationLogin(ref OrganizationID, Password, ref orgUserID, ref methodPremission, ref ipList);
                        if (string.IsNullOrEmpty(ipList)
                        || !BaseMethods.IsDigit(OrganizationID))
                            return "9";
                        else
                        {
                            bool passIP = BaseMethods.CheckIP(ipList, ipUserHost);
                            if (!passIP)
                            {
                                passIP = SqlMethods.ValidateIncapsulaIP(ipUserHost);
                                if (!passIP)
                                {
                                    return "30";
                                }
                            }

                        }
                    }

            }
            //Check Password And Return DBName
            dbName = SqlMethods.GetPasswordANDdbNameForOrgID(OrganizationID, ref mainField, ref enforcementIdentity, MetaData,
                ref ActiveCardNotNeededToOrder, ref AllowAnonymousTransactions, ref isTradeSite, ref isOrderTable);
            if (dbName == "19")//Connection Lost
                return dbName;
            if (!MetaData.Contains("FromGiftCard"))
            {
                //do not check premission to our servers
                if (!ipUserHost.StartsWith(LocalNetworkIP)
                    || ipUserHost == "127.0.0.1")
                {
                    //Check Premission To Method
                    StackTrace stackTrace = new StackTrace();
                    if (methodPremission)
                    {
                        if (!SqlMethods.CheckPremissionToMethod(orgUserID, stackTrace.GetFrame(2).GetMethod().Name))
                            return "60";
                    }
                }
                //Check ID length & Add Zero's
                if (mainField == null || mainField == "0")
                {
                    if (MemberID.Length < 9)//ת"ז פחות מ9 ספרות
                        MemberID = BaseMethods.AddZeroToID(MemberID);
                }

                if (!AllowAnonymousTransactions)
                {
                    //Check If The Member Exists
                    back = SqlMethods.ThereIsMemberID(dbName, MemberID);
                    if (back == "19" || back == "2")//ID Not Exists OR Connection Lost
                        return back;
                }
            }
            //Check CardStatus For MemberID
            if (ActiveCardNotNeededToOrder != true)
            //if (dbName != "ITU")
            {
                back = SqlMethods.CheckCardStatusForMemberID(dbName, MemberID, ref cardNumber);
                if (back == "-1")
                    return "16";//There Is NoCard For MemberId
                else if (back == "2")
                    return "17";//The Card Blocked 
                else if (back == "0")
                    return "18";//The Card Not Activ
            }
            //CatalogicPrice               CatalogicPrice
            //IrgunPrice                      IrgunPriceFormula
            //CustomerDiscount         VarDiscountFormula
            //CustomerPrice               VarPriceFormula
            //DistributionComission     CarComissionFormula

            //string catalogicPrice = "0";
            //string irgunPriceFormula = "0";
            //string varDiscountFormula = "0";
            //string varPriceFormula = "0";
            //string varComissionFormula = "0";



            if (quant > 0) // if it's a new Order 
            {
                //Check If The Variant Exists
                typeOrder = SqlMethods.CheckIfVariantExists(dbName, ProductID, ref isCampaign, ref GroupLimitsID,
                            ref implementationType, ref lastImplementationDate, ref splitOrder, ref variantType, ref posBarCode);//, ref catalogicPrice, ref irgunPriceFormula, ref varDiscountFormula, ref varPriceFormula, ref varComissionFormula);

                //fix for new orders only (shahar, 8-5-18)
                if (_lastImplementationDate.HasValue)
                    lastImplementationDate = _lastImplementationDate.Value;
            }
            else   // its a cancell Order
            {
                typeOrder = SqlMethods.CheckVariantType(dbName, ProductID, ref isCampaign, ref GroupLimitsID, ref variantType, ref BusinessSubTypeID, ref MarkSeats);//, ref catalogicPrice, ref irgunPriceFormula, ref varDiscountFormula, ref varPriceFormula, ref varComissionFormula);               
            }

            logTracking.AppendLine($"typeOrder:{typeOrder}, quant:{quant}");

            if (variantType == 3 && quant > 0 && payCardNumber.Length != 16)
                return "42";
            if (typeOrder == "19")   //Connection Lost
                return typeOrder;
            else if (string.IsNullOrEmpty(typeOrder))
                return "6";        //ProductID Not Exists
            else if (typeOrder == "28")
                return typeOrder;   //ProductID Not Enable
            //לא נדרש בביחד נקבע לפי IdentityOnOrders בטבלת Organizations
            if (enforcementIdentity)//בודק האם לאכוף שורות חחע
            {
                //Check If The Line Was Insert Before
                back = SqlMethods.IsTheLineWasInsertBefore(dbName, OriginalOrderID, typeOrder);
                if (back == "19" || back == "11")
                    return back;
            }

            PriceMembers pm = new PriceMembers("0", "0", "0", "0", "0", "0");
            //Read Price
            //if (dbName == "HonorReleases" || dbName == "Miluim" || dbName == "Yes" ||
            //    dbName == "HonorReleasesTest" || dbName == "MiluimTest" ||
            //    dbName == "ReleasesDevelopment" || dbName == "MiluimDevelopment" ||
            //    dbName == "Shufersal" || dbName == "ShufersalTest" || dbName == "Hot"
            //    || dbName == "LeumiCard" || dbName == "Nofshonit"
            //    || dbName == "Yediot" || dbName == "YediotTest" || dbName == "Teva" || dbName == "KNOWLEDGE4ALL"
            //    )//&& !MetaData.Contains("FromEnterBenefit"))

            logTracking.AppendLine($"isTradeSite:{isTradeSite}");




            if (isTradeSite)
            {
                if (!string.IsNullOrEmpty(MetaData))
                    xmlDoc.LoadXml(MetaData);

                SqlMethods.GetMarketingCommissionForProductID(ProductID, dbName, ref MarketingCommission, OrganizationID);

                string[] priceEl = new string[] { "Price", "catalogicPrice", "OrganizationPrice", "Comission", "Discount", "CancelComission" };
                if (!string.IsNullOrEmpty(MetaData) && !MetaData.Contains("FromGiftCard") && !MetaData.Contains("FromEnterBenefit") && !MetaData.Contains("IVR"))
                {
                    for (int i = 0; i < priceEl.Length; i++)
                    {
                        XmlNode node = xmlDoc.SelectSingleNode("Details/" + priceEl[i]);
                        if (node != null && !string.IsNullOrEmpty(node.InnerText))
                        {
                            switch (priceEl[i])
                            {
                                case "Price":
                                    pm.varPriceFormula = node.InnerText;
                                    break;
                                case "catalogicPrice":
                                    pm.catalogicPrice = node.InnerText;
                                    break;
                                case "OrganizationPrice":
                                    pm.irgunPriceFormula = node.InnerText;
                                    break;
                                case "Comission":
                                    pm.varComissionFormula = node.InnerText;
                                    break;
                                case "Discount":
                                    pm.varDiscountFormula = node.InnerText;
                                    break;
                                case "CancelComission":
                                    pm.cancelComission = node.InnerText;
                                    cancelComission = !string.IsNullOrEmpty(node.InnerText) ? node.InnerText : "0";
                                    break;
                            }
                        }
                    }

                    XmlNode isCampaignNode = xmlDoc.CreateNode(XmlNodeType.Element, "IsCampaign", null);
                    isCampaignNode.InnerText = isCampaign.ToString();
                    XmlNode Details = xmlDoc.SelectSingleNode("Details");
                    Details.AppendChild(isCampaignNode);
                }
                else
                {
                    SqlMethods.GetVariantProp(dbName, ProductID, ref dtVar);

                    if (dtVar.Rows.Count > 0)
                    {
                        DataTable dtMemberProp = new DataTable();
                        SqlMethods.GetMemberProp(dbName, MemberID, ref dtMemberProp);
                        if (dtMemberProp.Rows.Count > 0)
                        {
                            string[] priceElTable = new string[] { "VarPriceFormula", "CatalogicPrice", "IrgunPriceFormula", "VarComissionFormula", "VarDiscountFormula", "CancelCommission" };//
                            string irgunPriceFormula = "0";
                            string varPriceFormula = "0";
                            string varComissionFormula = "0";
                            string varDiscountFormula = "0";
                            for (int i = 0; i < priceElTable.Length; i++)
                            {
                                if (i == 1)
                                    continue;
                                object tempObj = dtVar.Rows[0][priceElTable[i]];
                                if (tempObj != DBNull.Value && !string.IsNullOrEmpty(tempObj.ToString()))
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            varPriceFormula = tempObj.ToString();
                                            break;
                                        case 2:
                                            irgunPriceFormula = tempObj.ToString();
                                            break;
                                        case 3:
                                            varComissionFormula = tempObj.ToString();
                                            break;
                                        case 4:
                                            varDiscountFormula = tempObj.ToString();
                                            break;
                                        case 5:
                                            cancelComission = tempObj.ToString();
                                            break;
                                    }
                                }

                            }
                            //PriceCalculation pc = new PriceCalculation();
                            //pc.CalculatVariantTable_PriceCalculation(dtVar, dtMemberProp.Rows[0]);
                            PriceCalculation.CalculatVariantTable_PriceCalculation(dtVar, dtMemberProp.Rows[0]);
                            for (int i = 0; i < priceEl.Length; i++)
                            {
                                if (dtVar.Rows[0][priceElTable[i]] != DBNull.Value &&
                                    !string.IsNullOrEmpty(dtVar.Rows[0][priceElTable[i]].ToString()) &&
                                    dtVar.Rows[0][priceElTable[i]].ToString() != "0")
                                {
                                    switch (priceEl[i])
                                    {
                                        case "Price":
                                            pm.varPriceFormula = dtVar.Rows[0][priceElTable[i]].ToString();
                                            break;
                                        case "catalogicPrice":
                                            pm.catalogicPrice = dtVar.Rows[0][priceElTable[i]].ToString();
                                            break;
                                        case "OrganizationPrice":
                                            pm.irgunPriceFormula = dtVar.Rows[0][priceElTable[i]].ToString();
                                            break;
                                        case "Comission":
                                            pm.varComissionFormula = dtVar.Rows[0][priceElTable[i]].ToString();
                                            break;
                                        case "Discount":
                                            pm.varDiscountFormula = dtVar.Rows[0][priceElTable[i]].ToString();
                                            break;
                                        case "CancelCommission":
                                            pm.cancelComission = dtVar.Rows[0][priceElTable[i]].ToString();
                                            break;
                                    }
                                }
                            }

                            //@"<Details>
                            //      <Source>IVR</Source><DialPhone>{0}</DialPhone><CC>{1}</CC><ValidUntil>{2}</ValidUntil><CVV>{3}</CVV><Phone>{4}</Phone>
                            //      <Amount>{5}</Amount><SlikaSapakDebit>0</SlikaSapakDebit><GC></GC><SMSPhoneNumber>{6}</SMSPhoneNumber><AttNumber>{7}</AttNumber>
                            // </Details>",

                            string SlikaSapakDebit = "ידני", source = "GiftCard", cc = "", validUntil = "", cvv = "";
                            string smsPhoneNumber = "", phone = "", orderTZ = "", TitanDetails = "", cancelSeats = "";
                            if (xmlDoc != null)
                            {
                                ReadXmlDetails(xmlDoc, ref source, ref phone, ref cc, ref validUntil, ref cvv, ref smsPhoneNumber,
                                    ref SlikaSapakDebit, ref orderTZ, ref asmachta, ref cancelComission, ref TitanDetails, ref cancelSeats);
                            }
                            if (MetaData.Contains("FromEnterBenefit"))
                                source = "יום המילואים 2010";

                            MetaData = "<Details><SlikaSapakDebit>" + SlikaSapakDebit + "</SlikaSapakDebit><Source>" + source + "</Source><CC>" + cc + "</CC><ValidUntil>" + validUntil + "</ValidUntil><CVV>" + cvv + "</CVV>" +
                                "<Amount>" + pm.varPriceFormula + "</Amount><Phone>" + phone + "</Phone><GC></GC><SMSPhoneNumber>" + smsPhoneNumber + "</SMSPhoneNumber><OrderTZ>" + orderTZ + "</OrderTZ><Price>" + pm.varPriceFormula + "</Price>" +
                                "<catalogicPrice>" + pm.catalogicPrice + "</catalogicPrice><OrganizationPrice>" + pm.irgunPriceFormula + "</OrganizationPrice>" +
                                "<Comission>" + pm.varComissionFormula + "</Comission><Discount>" + pm.varDiscountFormula + "</Discount><IrgunPriceFormula>" + irgunPriceFormula + "</IrgunPriceFormula>" +
                                "<VarPriceFormula>" + varPriceFormula + "</VarPriceFormula><VarDiscountFormula>" + varDiscountFormula + "</VarDiscountFormula><VarComissionFormula>" + varComissionFormula
                                + "</VarComissionFormula><CancelComission>" + cancelComission + "</CancelComission><IsCampaign>" + isCampaign + "</IsCampaign>"
                                + "<Asmachta>" + asmachta + "</Asmachta>" + TitanDetails + "<CancelSeats>" + cancelSeats + "</CancelSeats></Details>";
                        }
                    }
                }
            }

            //Check The Reason To Load Money
            if (typeOrder == "BUY_MONEY")
            {
                //bool flagCode = false;
                if (string.IsNullOrEmpty(Reason))
                    return "24";

                Reason = Reason.Trim();
                if (!BaseMethods.IsDigit(Reason))
                    return "27";

                Int16 reasoneBack = SqlMethods.IFExistsReason(dbName, Reason);
                if (reasoneBack == 19 || reasoneBack == 25)
                    return reasoneBack.ToString();

            }

            if (implementationType != -1)
                lastImplementationDate = DateCalculation.CalculateExpirationDate(implementationType, lastImplementationDate);


            //ValueCard
            VlcOrderData orderData = null;
            long atractionIdentity;

            logTracking.AppendLine($"variantType:{variantType}");

            if (variantType == 3)//value card product
            {//חסרה ולידציה לשדות הרלוונטים להזמנה
                if (quant > 0)//Order
                {
                    if (string.IsNullOrEmpty(payCardNumber) ||
                        string.IsNullOrEmpty(posBarCode))
                        return "79";//ValueCard order parameters are missing

                    asmchtaORCoupon = new List<string>();

                    orderData = new VlcOrderData();
                    orderData.CardNumber = payCardNumber;
                    orderData.MemberID = MemberID;
                    orderData.OrganizationID = Convert.ToInt32(OrganizationID);
                    orderData.PromoID = Convert.ToInt32(posBarCode);
                    orderData.QuantityOrdered = quant;
                }
                else//Cancel
                {
                    long vltranIdToCancel = 0;
                    if (asmchtaORCoupon.Count > 0
                        && long.TryParse(asmchtaORCoupon[0], out vltranIdToCancel))
                    {
                        vltranIdToCancel = SqlMethods.GetValueCardTranID(dbName, vltranIdToCancel);
                        if (vltranIdToCancel > 0)
                        {
                            long returnCode = VlcCancelOrder(vltranIdToCancel);
                            if (returnCode < 0)
                                return returnCode.ToString();
                        }
                        else
                            return "80";//Can't find Order to cancel in ValueCardTranLog Table
                    }
                    else
                        return "80";//Can't find Order to cancel in ValueCardTranLog Table
                }
            }

            if (variantType == 2)
            {
                string root = "Details/";
                Coupon = new SqlMethods.Coupon();

                Coupon.CouponStockID = getInfoFromXMLNode(xmlDoc, root + "CouponStockID");
                Coupon.CouponOrderTime = getInfoFromXMLNode(xmlDoc, root + "CouponOrderTime");

                if (string.IsNullOrEmpty(Coupon.CouponOrderTime))
                    Coupon.CouponOrderTime = DateTime.Now.ToString();

                string internalcoupon = getInfoFromXMLNode(xmlDoc, root + "isInternalCoupon");
                if (!string.IsNullOrEmpty(internalcoupon))
                    Coupon.isInternalCoupon = Convert.ToBoolean(internalcoupon);

                string[] couponArray = getInfoFromXMLNode(xmlDoc, root + "CouponCode").Split(';');

                if (quant > 0 && couponArray.Length != quant)
                    return "82";

                string orderQuantity = quant < 0 ? "-1" : "1";
                foreach (string coupon in couponArray)
                {
                    Coupon.CouponCode = coupon;

                    logTracking.AppendLine($"Begin SqlMethods.InsertNewOrder, Coupon :{coupon}");

                    back = SqlMethods.InsertNewOrder(PremiumType, dbName, typeOrder, MemberID, ProductID, orderQuantity, OriginalOrderID,
                    cardNumber, Reason, false, MetaData, pm.catalogicPrice, pm.irgunPriceFormula, pm.varDiscountFormula,
                    pm.varPriceFormula, pm.varComissionFormula, cancelComission, isCampaign, GroupLimitsID, MarketingCommission, paymentID,
                    lastImplementationDate.ToString("yyyy-MM-dd HH:mm:ss"), Cid, out atractionIdentity, Coupon, isTradeSite, parentMultiVariant, coins, isOrderTable, OrderItem, dtsRedimCode);

                    logTracking.AppendLine($"Return value from SqlMethods.InsertNewOrder: {back}");

                }


                if (back == "0")
                    return "19";

                return back;
            }
            else if (splitOrder == true)//Insert The Order
            {

                logTracking.AppendLine($"splitOrder:{splitOrder}");

                if (variantType == 3)
                    orderData.QuantityOrdered = 1;
                for (int i = 0; i < quant; i++)
                {
                    logTracking.AppendLine($"Begin SqlMethods.InsertNewOrder");

                    back = SqlMethods.InsertNewOrder(PremiumType, dbName, typeOrder, MemberID, ProductID, "1", OriginalOrderID,
                        cardNumber, Reason, false, MetaData, pm.catalogicPrice, pm.irgunPriceFormula, pm.varDiscountFormula,
                        pm.varPriceFormula, pm.varComissionFormula, cancelComission, isCampaign, GroupLimitsID, MarketingCommission, paymentID,
                        lastImplementationDate.ToString("yyyy-MM-dd HH:mm:ss"), Cid, out atractionIdentity, Coupon, isTradeSite, parentMultiVariant, coins, isOrderTable, OrderItem, dtsRedimCode);

                    logTracking.AppendLine($"Return value from SqlMethods.InsertNewOrder: {back}");

                    if (back == "0")
                        return "19";
                    else
                    {
                        if (variantType == 3)//value card product
                        {
                            long vlID = AddValueCardOrder(orderData, dbName, atractionIdentity);
                            asmchtaORCoupon.Add(vlID.ToString());
                        }
                    }
                }
                return back;
            }
            else
            {

                logTracking.AppendLine($"splitOrder:{splitOrder}");

                //if (variantType == 2)
                //{
                //    string root = "Details/";
                //    Coupon = new SqlMethods.Coupon();

                //    Coupon.CouponStockID = getInfoFromXMLNode(xmlDoc, root + "CouponStockID");
                //    Coupon.CouponCode = getInfoFromXMLNode(xmlDoc, root + "CouponCode");
                //    Coupon.CouponOrderTime = getInfoFromXMLNode(xmlDoc, root + "CouponOrderTime");

                //    if (string.IsNullOrEmpty(Coupon.CouponOrderTime))
                //        Coupon.CouponOrderTime = DateTime.Now.ToString();

                //    string internalcoupon = getInfoFromXMLNode(xmlDoc, root + "isInternalCoupon");
                //    if (!string.IsNullOrEmpty(internalcoupon))
                //        Coupon.isInternalCoupon = Convert.ToBoolean(internalcoupon);

                //}

                logTracking.AppendLine($"Begin SqlMethods.InsertNewOrder");

                back = SqlMethods.InsertNewOrder(PremiumType , dbName, typeOrder, MemberID, ProductID, quant.ToString(), OriginalOrderID,
                    cardNumber, Reason, false, MetaData, pm.catalogicPrice, pm.irgunPriceFormula, pm.varDiscountFormula,
                    pm.varPriceFormula, pm.varComissionFormula, cancelComission, isCampaign, GroupLimitsID, MarketingCommission, paymentID,
                    lastImplementationDate.ToString("yyyy-MM-dd HH:mm:ss"), Cid, out atractionIdentity, Coupon, isTradeSite, parentMultiVariant, coins, isOrderTable, OrderItem, dtsRedimCode);

                logTracking.AppendLine($"Return value from SqlMethods.InsertNewOrder: {back}");

                if (back == "0")
                    return "19";

                if (variantType == 3)//value card product
                {
                    long vlID = AddValueCardOrder(orderData, dbName, atractionIdentity);
                    asmchtaORCoupon.Add(vlID.ToString());
                }

                //save mark seats in reservedSeats table
                if (quant < 0 && BusinessSubTypeID == 6 && MarkSeats)
                {

                    logTracking.AppendLine("quant < 0 && BusinessSubTypeID == 6");


                    string source = string.Empty;
                    if (MetaData.Contains("FromGiftCard"))
                    {
                        source = "GiftCard";

                        if (IsCancelSeatsRequired(xmlDoc))
                            InternetBeeReservation.SaveSeatsForCancel(MemberID, Convert.ToInt32(OrganizationID),
                                                                      ProductID,
                                                                      source, dbName, back, ref IsCancelSeatssucceed);

                        else // no need to cancel then only update cancel seats
                            UpdateSeatsToCancleSeats(dbName, asmachta, ProductID);
                    }

                    else
                    {
                        source = "UserCancel";
                        InternetBeeReservation.SaveSeatsForCancel(MemberID, Convert.ToInt32(OrganizationID), ProductID,
                                                                      source, dbName, back, ref IsCancelSeatssucceed);
                    }
                    logTracking.AppendLine($"source:{source}");


                    //InternetBeeReservation.CancelMarkSeats(MemberID, Convert.ToInt32(OrganizationID), ProductID, source, dbName, back,ref IsCancelSeatssucceed);
                }

                return back;
            }


        }
        catch (SqlException ex)
        {

            logTracking.AppendLine($"SqlException:{ex.Message}");

            int Timeout = -2;
            sendExeptionMail(OrganizationID, Password, ProductID, MemberID, Quantity, OriginalOrderID, Reason, typeOrder, back, dbName, ex);
            if (ex.Number == Timeout)
                return "98";
            return "19";
        }
        catch (Exception ex)
        {

            logTracking.AppendLine($"Exception:{ex.Message}");

            sendExeptionMail(OrganizationID, Password, ProductID, MemberID, Quantity, OriginalOrderID, Reason, typeOrder, back, dbName, ex);
            return "19";
        }
        //finally
        //{
        //    DataBase.CloseConnection();
        //}
        //  return "19";
    }


    private static void UpdateSeatsToCancleSeats(string dbName, string asmachta, string ProductID)
    {
        string query = string.Format(@"update {0}..WebServiceTransaction SET 
                        XMLParam = REPLACE(REPLACE(Convert (nvarchar(max), XMLParam), '<seats>', '<CancelSeats>'), '</seats>' , '</CancelSeats>')
                            where webservicetransaction.ttransactionorder = '{1}'
                            AND webservicetransaction.ttransactionproductid='{2}'
                            AND webservicetransaction.ttransactionquantity>0", dbName, asmachta, ProductID);
        DataBase.ExecuteNonQuery(query);
    }

    private static bool IsCancelSeatsRequired(XmlDocument xmlDoc)
    {
        try
        {
            string cancel = xmlDoc.SelectSingleNode("Details/CancelSeats").InnerText;
            if (cancel == "Yes")
                return true;
        }
        catch (Exception)
        {
            return false;
        }
        return false;
    }

    private static string getInfoFromXMLNode(XmlDocument xmlDoc, string nodePath)
    {
        XmlNode node = xmlDoc.SelectSingleNode(nodePath);
        if (node == null) return string.Empty;
        return node.InnerText;
    }

    private static void sendExeptionMail(string OrganizationID, string Password, string ProductID, string MemberID, string Quantity, string OriginalOrderID, string Reason, string typeOrder, string back, string dbName, Exception ex)
    {
        SqlMethods.SendMail("InsertNewOrder -> \n OrganizationID -> " + OrganizationID + "\n Password -> " + Password
            + "\n dbName -> " + dbName + "\n typeOrder -> " + typeOrder + "\n MemberID -> " + MemberID
            + "\n ProductID -> " + ProductID + "\n Quantity -> " + Quantity + "\n OriginalOrderID -> " + OriginalOrderID
            + "\n Reason -> " + Reason + "\n back -> " + back + "\n Exception Type -> " + ex.GetType()
            + "\n Exception Message -> " + ex.Message + "\n ex.StackTrace -> " + ex.StackTrace + "\n ex.TargetSite -> " + ex.TargetSite);
    }

    private static void ReadXmlDetails(XmlDocument xmlDoc, ref string source, ref string phone, ref string cc, ref string validUntil,
        ref string cvv, ref string smsPhoneNumber, ref string SlikaSapakDebit, ref string orderTZ,
        ref string asmachta, ref string cancelComission, ref string TitanDetails, ref string cancelSeats)
    {
        XmlNode node;
        string[] detailsArr = new string[] { "Source", "DialPhone", "CC", "ValidUntil", "CVV", "SMSPhoneNumber",
                                                "SlikaSapakDebit", "OrderTZ", "Asmachta", "CancelComission" ,"TitanDetails", "CancelSeats"};
        for (int i = 0; i < detailsArr.Length; i++)
        {
            node = xmlDoc.SelectSingleNode("Details/" + detailsArr[i]);
            if (node != null)
            {
                switch (i)
                {
                    case 0:
                        source = node.InnerText;
                        break;
                    case 1:
                        phone = node.InnerText;
                        break;
                    case 2:
                        cc = node.InnerText;
                        break;
                    case 3:
                        validUntil = node.InnerText;
                        break;
                    case 4:
                        cvv = node.InnerText;
                        break;
                    case 5:
                        smsPhoneNumber = node.InnerText;
                        break;
                    case 6:
                        SlikaSapakDebit = node.InnerText;
                        break;
                    case 7:
                        orderTZ = node.InnerText;
                        break;
                    case 8:
                        asmachta = node.InnerText;
                        break;
                    case 9:
                        if (!string.IsNullOrEmpty(node.InnerText))
                            cancelComission = node.InnerText;
                        else
                            cancelComission = "0";
                        break;
                    case 10:
                        TitanDetails = node.OuterXml;
                        break;
                    case 11:
                        cancelSeats = node.InnerText;
                        break;

                    default:
                        break;
                }
            }
        }
    }

    private static string CheckInputValues(ref string ProductID, ref string MemberID, ref string Quantity, ref string OriginalOrderID)
    {

        #region Check Empty Value's

        //Check IF MemberID Empty
        if (string.IsNullOrEmpty(MemberID))
            return "5";//ת"ז לא נשלחה
        else
            MemberID = MemberID.Trim();

        //Check IF ProductID Empty
        if (string.IsNullOrEmpty(ProductID))
            return "8";//מק"ט לא נשלח
        else
            ProductID = ProductID.Trim();

        //Check IF Quantity Empty
        if (string.IsNullOrEmpty(Quantity))
            return "14";//כמות לא נשלחה
        else
            Quantity = Quantity.Trim();

        //Check IF OriginalOrderID Empty
        if (string.IsNullOrEmpty(OriginalOrderID))
            return "12";//מספר שורה טקטיקום לא נשלח
        else
            OriginalOrderID = OriginalOrderID.Trim();

        #endregion

        #region CheckID

        //Check MemberID format
        if (!BaseMethods.IsDigit(MemberID))
            return "4";//פורמט ת"ז שגוי

        ////Check ID length & Add Zero's
        //if (MemberID.Length < 9)//ת"ז פחות מ9 ספרות
        //    MemberID = BaseMethods.AddZeroToID(MemberID);

        //Check If Real ID
        //if (!BaseMethods.CheckIfRealID(MemberID))
        //    return "3";//ת"ז שגויה

        #endregion

        #region Check ProductID

        //Check ProductID format
        int indexOfHyphen = ProductID.IndexOf('-');
        if (indexOfHyphen < 0)
            return "7";// פורמט מק"ט שגוי אין מקף


        //הבדיקה בוטלה לבקשת דוד - ארגון המורים שולחים מק"ט עם אותיות
        //if (!BaseMethods.IsDigit(ProductID, indexOfHyphen))
        //    return "7"; // פורמט מק"ט שגוי לא מספרי

        #endregion

        #region Check Quantity

        //Check Quantity format
        int number = 0;
        bool isNumber = int.TryParse(Quantity, out number);
        if (!isNumber)
            return "13";//פורמט כמות שגוי

        //Check Quantity = 0
        if (Quantity == "0")
            return "15"; //כמות שווה ל0

        #endregion

        #region Check OriginalOrderID

        //Check OriginalOrderID format
        int indexOfHhen = OriginalOrderID.IndexOf('-');
        if (indexOfHhen < 0)
            return "21"; //פורמט קוד שורה שגוי
        //if (!BaseMethods.IsDigit(OriginalOrderID))
        //    return "21"; //פורמט קוד שורה שגוי

        #endregion

        //No Error's
        return null;
    }


    public static string CheckIfCardActiv(string OrganizationID, string Password, string MemberID, string MetaData, string ipUserHost)
    {
        string dbName = null;
        string back = null;
        string mainField = null;
        bool enforcementIdentity = true;
        bool ActiveCardNotNeededToOrder = false;

        //Check OrganizationID Empty
        if (string.IsNullOrEmpty(OrganizationID))
            return "22";//חסר קוד אירגון

        //Check Password Empty
        if (string.IsNullOrEmpty(Password))
            return "10";//חסרה סיסמה  

        //Check Password Empty
        if (string.IsNullOrEmpty(MemberID))
            return "5";//חסר ת"ז  

        //Check If Real Card Number
        if (!BaseMethods.IsDigit(MemberID))
            return "4";

        //Check ID length & Add Zero's
        //if (MemberID.Length < 9)//ת"ז פחות מ9 ספרות
        //    MemberID = BaseMethods.AddZeroToID(MemberID);

        ////Check If Real ID
        //if (!BaseMethods.CheckIfRealID(MemberID))
        //    return "3";//ת"ז לא תקנית

        try
        {

            bool methodPremission = false, AllowAnonymousTransactions = false;
            string orgUserID = "";
            string ipList = "";
            //Check Valid Organization
            SqlMethods.OrganizationLogin(ref OrganizationID, Password, ref orgUserID, ref methodPremission, ref ipList);
            if (string.IsNullOrEmpty(ipList)
                || !BaseMethods.IsDigit(OrganizationID))
                return "9";
            else
            {
                bool passIP = BaseMethods.CheckIP(ipList, ipUserHost);
                if (!passIP)
                    return "30";
            }


            //Check Password And Return DBName
            dbName = SqlMethods.GetPasswordANDdbNameForOrgID(OrganizationID, ref mainField, ref enforcementIdentity
                , "", ref ActiveCardNotNeededToOrder, ref AllowAnonymousTransactions);

            if (dbName == "19")//Connection Lost
                return dbName;

            //Check Premission To Method
            StackTrace stackTrace = new StackTrace();
            if (methodPremission)
            {
                if (!SqlMethods.CheckPremissionToMethod(orgUserID, stackTrace.GetFrame(1).GetMethod().Name))
                    return "60";
            }

            //Check ID length & Add Zero's
            if (mainField == null || mainField == "0")
            {
                if (MemberID.Length < 9)//ת"ז פחות מ9 ספרות
                    MemberID = BaseMethods.AddZeroToID(MemberID);
            }

            //Check If The Member Exists
            back = SqlMethods.ThereIsMemberID(dbName, MemberID);
            if (back == "19" || back == "2")//ID Not Exists OR Connection Lost
                return back;

            //Check CardStatus For MemberID
            back = SqlMethods.CheckCardStatusForCardNumber(dbName, MemberID);
            return back;
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("CheckIfCardActiv -> \n dbName -> " + dbName + "\n OrganizationID -> " + OrganizationID
                + "\n Password -> " + Password + "\n MemberID -> " + MemberID + "\n MetaData -> " + MetaData
                + "\n back -> " + back + "\n Exception Type -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        //finally
        //{
        //    DataBase.CloseConnection();
        //}
        //return "19";
    }
    private static string checkMissingDetails(string OrganizationID, string OrganizationPassword, string memberID,
                    string cardNumber, string xmlActivationParmas)
    {
        if (string.IsNullOrEmpty(OrganizationID))
            return "22"; // organization id is missing
        if (string.IsNullOrEmpty(OrganizationPassword))
            return "10"; // organization password is missing
        if (string.IsNullOrEmpty(memberID))
            return "5"; // member id is missing
        if (string.IsNullOrEmpty(cardNumber))
            return "31"; // card number is missing
        if (string.IsNullOrEmpty(xmlActivationParmas))
            return "34"; // additional required field for activation is missing

        return "0";
    }

    public static string ActivateCard(string organizationID, string organizationPassword, string memberID, string cardNumber, string checkOnly, string xmlActivationParmas, string ipUserHost)
    {
        string result;
        string dbName = null;
        string mainField = null;
        bool enforcementIdentity = true;
        bool ActiveCardNotNeededToOrder = false;

        result = checkMissingDetails(organizationID, organizationPassword, memberID, cardNumber, xmlActivationParmas);
        if (result != "0") //one of the function parameters are missing then terminate function
            return result;

        try
        {
            bool methodPremission = false, AllowAnonymousTransactions = false;
            string orgUserID = "";
            string ipList = "";
            //Check Valid Organization
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


            //Check Password And Return DBName
            dbName = SqlMethods.GetPasswordANDdbNameForOrgID(organizationID, ref mainField, ref enforcementIdentity,
                "", ref ActiveCardNotNeededToOrder, ref AllowAnonymousTransactions);
            if (dbName == "19")//Connection Lost
                return dbName;

            //Check Premission To Method
            StackTrace stackTrace = new StackTrace();
            if (methodPremission)
            {
                if (!SqlMethods.CheckPremissionToMethod(orgUserID, stackTrace.GetFrame(1).GetMethod().Name))
                    return "60";
            }

            result = SqlMethods.ThereIsMemberID(dbName, memberID);
            if (result == "19" || result == "2") // customer does not exists or connection lost
                return result;

            if (checkOnly == "1") // only validate but do not activate card
                return validateActivation(dbName, memberID, cardNumber, xmlActivationParmas);
            else if (checkOnly == "2")
                return activate(dbName, memberID, cardNumber, xmlActivationParmas, ipUserHost);
            else
                return "37"; // checkOnly can only be "1" or "2"
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("Activate-> \n OrganizationID -> " + organizationID + "\n Password -> " + organizationPassword
                + "\n dbName -> " + dbName + "\n MemberID -> " + memberID +
                "\n Exception Type -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        //finally
        //{
        //    DataBase.CloseConnection();
        //}
        //return "0";
    }

    internal static string validateActivation(string dbName, string memberID, string cardNumber, string xmlActivationParmas)
    {
        string result = "", last4Digits = "", memberIsraeliID = "";
        XmlDocument xml = new XmlDocument();

        try
        {
            // parse the xml string to get the additional activation field to validate with
            xml.LoadXml(xmlActivationParmas);

            XmlNode root = xml.SelectSingleNode("Details");

            if (root.SelectSingleNode("tz") != null)
                memberIsraeliID = root.SelectSingleNode("tz").InnerText;
            if (root.SelectSingleNode("Last4Digits") != null)
                last4Digits = root.SelectSingleNode("Last4Digits").InnerText;

            if (string.IsNullOrEmpty(memberIsraeliID) && string.IsNullOrEmpty(last4Digits))
                return "34"; // additional required field for validation is missing
        }

        catch (Exception ex)
        {
            SqlMethods.SendMail("validateActivation-> \n " +
             "\ndbName -> " + dbName + "\nmemberID -> " + memberID + "\ncardNumber -> " + cardNumber +
             "\n Exception Type -> " + ex.GetType() +
             "\n Exception Message -> " + ex.Message);

            return "39"; // wrong xml format 
        }

        try
        {
            result = SqlMethods.validateActivation(dbName, memberID, cardNumber, memberIsraeliID, last4Digits);
            return result;
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("validateActivation-> \n dbName -> " + dbName + "\n MemberID -> " + memberID +
                "\n Exception Type -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
    }

    internal static string activate(string dbName, string memberID, string cardNumber, string xmlActivationParmas, string ipUserHost)
    {
        StringBuilder customerDetails = new StringBuilder();
        string result = validateActivation(dbName, memberID, cardNumber, xmlActivationParmas);
        if (result != "32" && result != "41") //if the result is other then validation passed 
            return result;

        customerDetails = parseXml(dbName, memberID, xmlActivationParmas);
        if (customerDetails.ToString() == "39" || customerDetails.ToString() == "40")
            return customerDetails.ToString();

        try
        {
            result = SqlMethods.activate(dbName, memberID, cardNumber, customerDetails, ipUserHost);
            if (result == "19")
                return result;

        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("activate-> \ndbName -> " + dbName + "\nMemberID -> " + memberID + "\ncardNumber -> " + cardNumber +
              "\n Exception Type -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        return "38";
    }


    internal static StringBuilder parseXml(string dbName, string memberID, string xmlActivationParmas)
    {
        StringBuilder customerParams = new StringBuilder();
        DateTime date = new DateTime();
        CultureInfo culture = new CultureInfo("en-GB"); // for the date format dd/mm/yyyy
        XmlDocument xml = new XmlDocument();

        bool haveFirstElement = false;

        xmlActivationParmas = xmlActivationParmas.Replace("'", "''");  // replace all ' with ''
        try
        {
            xml.LoadXml(xmlActivationParmas);
            XmlNode root = xml.SelectSingleNode("Details");

            customerParams.Append("update ");
            customerParams.Append(dbName);
            customerParams.Append("..allmembers set ");

            // always check if the element have data in it before adding to the query
            if (nodeHaveData(root, "tz"))
                addNode("tz", ref customerParams, ref haveFirstElement, root);

            if (nodeHaveData(root, "MemberFirstName"))
                addNode("MemberFirstName", ref customerParams, ref haveFirstElement, root);

            if (nodeHaveData(root, "MemberLastName"))
                addNode("MemberLastName", ref customerParams, ref haveFirstElement, root);

            if (nodeHaveData(root, "PhoneNumber"))
                addNode("PhoneNumber", ref customerParams, ref haveFirstElement, root);

            if (nodeHaveData(root, "MobilePhone"))
                addNode("MobilePhone", ref customerParams, ref haveFirstElement, root);

            if (nodeHaveData(root, "Email"))
                addNode("Email", ref customerParams, ref haveFirstElement, root);

            if (nodeHaveData(root, "Gender"))
                addNode("Gender", ref customerParams, ref haveFirstElement, root);

            if (nodeHaveData(root, "MemberStatus"))
                addNode("MemberStatus", ref customerParams, ref haveFirstElement, root);

            if (nodeHaveData(root, "Last4Digits"))
                addNode("Last4Digits", ref customerParams, ref haveFirstElement, root);

            if (nodeHaveData(root, "MemberSpecialID"))
                addNode("MemberSpecialID", ref customerParams, ref haveFirstElement, root);

            if (nodeHaveData(root, "PopulationType"))
                addNode("PopulationType", ref customerParams, ref haveFirstElement, root);

            //add InterestAreas 
            //if (nodeHaveData(root, "InterestAreas"))
            //    addNode("InterestAreas", ref customerParams, ref haveFirstElement, root);

            try
            {
                if (root.SelectSingleNode("BirthDate") != null)
                {
                    if (!string.IsNullOrEmpty(root.SelectSingleNode("BirthDate").InnerText))
                    {
                        if (haveFirstElement == false)
                            haveFirstElement = true;
                        else
                            customerParams.Append(", ");

                        customerParams.Append("BirthDate = '");
                        date = Convert.ToDateTime(root.SelectSingleNode("BirthDate").InnerText, culture);
                        customerParams.Append(date);
                        customerParams.Append("'");
                    }
                }
                if (root.SelectSingleNode("MariageDate") != null)
                {
                    if (!string.IsNullOrEmpty(root.SelectSingleNode("MariageDate").InnerText))
                    {
                        if (haveFirstElement == false)
                            haveFirstElement = true;
                        else
                            customerParams.Append(", ");

                        customerParams.Append("MariageDate = '");
                        date = Convert.ToDateTime(root.SelectSingleNode("MariageDate").InnerText, culture);
                        customerParams.Append(date);
                        customerParams.Append("'");
                    }
                }
            }
            catch (FormatException ex)
            {
                SqlMethods.SendMail("parseXml-> \ndbName -> " + dbName + "\nMemberID -> " + memberID +
                "\n Exception Type -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
                customerParams = new StringBuilder();
                customerParams.Append("40"); // invalid date format 
            }


            customerParams.Append("where memberid = '");
            customerParams.Append(memberID);
            customerParams.Append("'");
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("parseXml-> \ndbName -> " + dbName + "\nMemberID -> " + memberID +
               "\n Exception Type -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            customerParams = new StringBuilder();
            customerParams.Append("39"); // wrong xml format 
        }

        return customerParams;
    }

    private static void addNode(string elementName, ref StringBuilder customerParams, ref bool haveFirstElement, XmlNode root)
    {
        string addToQuery = "";
        if (haveFirstElement == false)
            haveFirstElement = true;
        else
            customerParams.Append(", ");

        addToQuery = (elementName + "='" + root.SelectSingleNode(elementName).InnerText + "'");
        customerParams.Append(addToQuery);
    }

    private static bool nodeHaveData(XmlNode root, string elementName)
    {
        if (root.SelectSingleNode(elementName) != null)
            return true;
        else
            return false;
    }


    public static string createToekn(string organizationID, string organizationPassword, string ipUserHost)
    {
        string result = null;

        string mainField = null;
        bool enforcementIdentity = true;
        bool ActiveCardNotNeededToOrder = false;

        if (string.IsNullOrEmpty(organizationID))
            return "22";

        if (string.IsNullOrEmpty(organizationPassword))
            return "10";

        try
        {
            bool methodPremission = false, AllowAnonymousTransactions = false;
            string orgUserID = "";
            string ipList = "";
            //Check Valid Organization
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

            //Check Password And Return DBName
            result = SqlMethods.GetPasswordANDdbNameForOrgID(organizationID, ref mainField, ref enforcementIdentity,
                "", ref ActiveCardNotNeededToOrder, ref AllowAnonymousTransactions);
            if (result == "19")//Connection Lost
                return result;

            //Check Premission To Method
            StackTrace stackTrace = new StackTrace();
            if (methodPremission)
            {
                if (!SqlMethods.CheckPremissionToMethod(orgUserID, stackTrace.GetFrame(1).GetMethod().Name))
                    return "60";
            }

            TimeSpan time = DateTime.Now - tokenCreateTime;

            if (time.Minutes < TokenLifeTime && time.Days == 0 && time.Hours == 0)
                return token;

            else // token expired - create a new one 
            {
                token = generateToken();
                token = token.Replace("+", "2").Replace("==", "al").Replace("/", "4");

                tokenCreateTime = DateTime.Now;
            }
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("createToekn-> \n OrganizationID -> " + organizationID + "\n Password -> " + organizationPassword
             + "\n Exception Type -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        return token;
    }

    internal static string generateToken()
    {
        byte[] saltInBytes = new byte[16];
        RNGCryptoServiceProvider saltGenerator = new RNGCryptoServiceProvider();
        saltGenerator.GetBytes(saltInBytes);
        string saltAsString = Convert.ToBase64String(saltInBytes);
        return saltAsString;
    }

    public static string GetToken(string organizationID, string organizationPassword, string ipUserHost)
    {
        string result = null, mainField = null;
        bool enforcementIdentity = true, ActiveCardNotNeededToOrder = false;

        if (string.IsNullOrEmpty(organizationID))
            return "22";

        if (string.IsNullOrEmpty(organizationPassword))
            return "10";

        try
        {
            bool methodPremission = false, AllowAnonymousTransactions = false;
            string orgUserID = "";
            string ipList = "";

            //Check Valid Organization
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

            //Check Password And Return DBName
            result = SqlMethods.GetPasswordANDdbNameForOrgID(organizationID, ref mainField, ref enforcementIdentity,
                "", ref ActiveCardNotNeededToOrder, ref AllowAnonymousTransactions);
            if (result == "19")//Connection Lost
                return result;

            //Check Premission To Method
            StackTrace stackTrace = new StackTrace();
            if (methodPremission)
            {
                if (!SqlMethods.CheckPremissionToMethod(orgUserID, stackTrace.GetFrame(1).GetMethod().Name))
                    return "60";
            }

            TimeSpan time = DateTime.Now - tokenCreateTime;

            if (time.Minutes < TokenLifeTime && time.Days == 0 && time.Hours == 0)
                return token;
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("GetToekn -> \n OrganizationID -> " + organizationID + "\n Password -> " + organizationPassword
             + "\n Exception Type -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        return "-1"; // token expired 
    }


    public static string getAllVariants(string organizationID, string organizationPassword, string ipUserHost)
    {
        string campaignsResult, variantsResult, result;
        string DBName = "";
        string mainField = null;
        bool enforcementIdentity = true;
        bool ActiveCardNotNeededToOrder = false;
        if (string.IsNullOrEmpty(organizationID))
            //|| !BaseMethods.IsDigit(organizationID))
            return "22";
        else
            organizationID = organizationID.Trim();

        if (string.IsNullOrEmpty(organizationPassword))
            return "10";

        try
        {
            bool methodPremission = false, AllowAnonymousTransactions = false;
            string orgUserID = "";
            string ipList = "";
            //Check Valid Organization
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

            //Check Password And Return DBName into DBName
            DBName = SqlMethods.GetPasswordANDdbNameForOrgID(organizationID, ref mainField, ref enforcementIdentity
                , "", ref ActiveCardNotNeededToOrder, ref AllowAnonymousTransactions);
            if (DBName == "19")//Connection Lost
                return DBName;

            //Check Premission To Method
            StackTrace stackTrace = new StackTrace();
            if (methodPremission)
            {
                if (!SqlMethods.CheckPremissionToMethod(orgUserID, stackTrace.GetFrame(1).GetMethod().Name))
                    return "60";
            }

            if (organizationID != "28")
            {
                campaignsResult = SqlMethods.getCampaigns(organizationID, null);
                if (campaignsResult == "19")
                    return campaignsResult;
                campaignsResult = campaignsResult.Replace("DocumentElement", "campaigns");
            }
            else
            {
                campaignsResult = SqlMethods.getCategories(organizationID);
                if (campaignsResult == "19")
                    return campaignsResult;
                campaignsResult = campaignsResult.Replace("DocumentElement", "Categories");
            }

            variantsResult = SqlMethods.getVariants(DBName, organizationID);
            if (variantsResult == "19")
                return variantsResult;
            variantsResult = variantsResult.Replace("DocumentElement", "variants");

            result = "<AllBenefits>" + campaignsResult + variantsResult + "</AllBenefits>";
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("getAllVariants-> \n OrganizationID -> " + organizationID + "\n Password -> " + organizationPassword
             + "\n Exception Type -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }

        //finally
        //{
        //    DataBase.CloseConnection();
        //}

        return result;
    }



    public static string isMandatoryFieldExist(string organizationID, string organizationPassword, string cusomerIdentification)
    {
        if (string.IsNullOrEmpty(organizationID))
            return "22";
        if (string.IsNullOrEmpty(organizationPassword))
            return "10";
        if (string.IsNullOrEmpty(cusomerIdentification))
            return "5";
        return "0";
    }

    public static string getMemberChargesByCardNumber(string organizationID, string organizationPassword, string cardNumber, string ipUserHost)
    {
        string result = "", equalMoney, campaigns, DBName, mainField = null, memberID = "";
        bool enforcementIdentity = true;
        bool ActiveCardNotNeededToOrder = false;

        result = isMandatoryFieldExist(organizationID, organizationPassword, cardNumber);
        if (result != "0")
            return result;

        try
        {
            bool methodPremission = false, AllowAnonymousTransactions = false;
            string orgUserID = "";
            string ipList = "";
            //Check Valid Organization
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


            //Check Password And Return DBName into DBName
            DBName = SqlMethods.GetPasswordANDdbNameForOrgID(organizationID, ref mainField, ref enforcementIdentity
                , "", ref ActiveCardNotNeededToOrder, ref AllowAnonymousTransactions);
            if (DBName == "19")//Connection Lost
                return DBName;

            //Check Premission To Method
            StackTrace stackTrace = new StackTrace();
            if (methodPremission)
            {
                if (!SqlMethods.CheckPremissionToMethod(orgUserID, stackTrace.GetFrame(1).GetMethod().Name))
                    return "60";
            }

            memberID = SqlMethods.GetMemberIDByCardNumber(DBName, cardNumber);
            if (memberID == "2" || memberID == "19")
                return memberID;

            equalMoney = SqlMethods.getEqualMoney(DBName, memberID);
            if (equalMoney == "19")
                return equalMoney;

            campaigns = SqlMethods.getCampaignUses(DBName, memberID);
            if (campaigns == "19")
                return campaigns;

            //requests = SqlMethods.getRequests(DBName, memberID);
            //if (campaigns == "19")
            //    return campaigns;

        }

        catch (Exception ex)
        {
            SqlMethods.SendMail("getMemberCharges-> \n OrganizationID -> " + organizationID + "\n Password -> " + organizationPassword
                + "\n memberID -> " + memberID + "\n Exception Type -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        //result = "<AllChargesAndUses>\n" + equalMoney + "\n" + campaigns + "\n" + requests + "\n" + "</AllChargesAndUses>";
        result = "<AllChargesAndUses>\n" + equalMoney + "\n" + campaigns + "\n" + "</AllChargesAndUses>";
        return result;
    }

    public static string getMemberCharges(string organizationID, string organizationPassword, string memberID, string ipUserHost)
    {
        string result = "", equalMoney, campaigns, DBName, mainField = null; // requests,
        bool enforcementIdentity = true;
        bool ActiveCardNotNeededToOrder = false;


        result = isMandatoryFieldExist(organizationID, organizationPassword, memberID);
        if (result != "0")
            return result;

        try
        {
            bool methodPremission = false, AllowAnonymousTransactions = false;
            string orgUserID = "";
            string ipList = "";
            //Check Valid Organization
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


            //Check Password And Return DBName into DBName
            DBName = SqlMethods.GetPasswordANDdbNameForOrgID(organizationID, ref mainField, ref enforcementIdentity
                , "", ref ActiveCardNotNeededToOrder, ref AllowAnonymousTransactions);
            if (DBName == "19")//Connection Lost
                return DBName;

            //Check Premission To Method
            StackTrace stackTrace = new StackTrace();
            if (methodPremission)
            {
                if (!SqlMethods.CheckPremissionToMethod(orgUserID, stackTrace.GetFrame(1).GetMethod().Name))
                    return "60";
            }

            //Check If The Member Exists
            result = SqlMethods.ThereIsMemberID(DBName, memberID);
            if (result == "19" || result == "2")//ID Not Exists OR Connection Lost
                return result;
            equalMoney = SqlMethods.getEqualMoney(DBName, memberID);
            if (equalMoney == "19")
                return equalMoney;

            campaigns = SqlMethods.getCampaignUses(DBName, memberID);
            if (campaigns == "19")
                return campaigns;

            //requests = SqlMethods.getRequests(DBName, memberID);
            //if (campaigns == "19")
            //    return campaigns;

        }

        catch (Exception ex)
        {
            SqlMethods.SendMail("getMemberCharges-> \n OrganizationID -> " + organizationID + "\n Password -> " + organizationPassword
                + "\n memberID -> " + memberID + "\n Exception Type -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        //result = "<AllChargesAndUses>\n" + equalMoney + "\n" + campaigns + "\n" + requests + "\n" + "</AllChargesAndUses>";
        result = "<AllChargesAndUses>\n" + equalMoney + "\n" + campaigns + "\n" + "</AllChargesAndUses>";
        return result;
    }

    public static string getBenefitsForMember(string organizationID, string organizationPassword, string memberID, string ipUserHost)
    {
        string campaignsResult, variantsResult, result;
        string DBName = "";
        string mainField = null;
        bool enforcementIdentity = true;
        bool ActiveCardNotNeededToOrder = false;

        if (string.IsNullOrEmpty(organizationID))
            return "22";
        if (string.IsNullOrEmpty(organizationPassword))
            return "10";
        if (string.IsNullOrEmpty(memberID))
            return "5";

        try
        {
            bool methodPremission = false, AllowAnonymousTransactions = false;
            string orgUserID = "";
            string ipList = "";
            //Check Valid Organization
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


            //Check Password And Return DBName into DBName
            DBName = SqlMethods.GetPasswordANDdbNameForOrgID(organizationID, ref mainField, ref enforcementIdentity,
                "", ref ActiveCardNotNeededToOrder, ref AllowAnonymousTransactions);
            if (DBName == "19")//Connection Lost
                return DBName;

            //Check Premission To Method
            StackTrace stackTrace = new StackTrace();
            if (methodPremission)
            {
                if (!SqlMethods.CheckPremissionToMethod(orgUserID, stackTrace.GetFrame(1).GetMethod().Name))
                    return "60";
            }

            //Check If The Member Exists
            result = SqlMethods.ThereIsMemberID(DBName, memberID);
            if (result == "19" || result == "2")//ID Not Exists OR Connection Lost
                return result;


            campaignsResult = SqlMethods.getMemberCampaigns(DBName, organizationID, memberID);
            if (campaignsResult == "19")
                return campaignsResult;
            campaignsResult = campaignsResult.Replace("DocumentElement", "campaigns");

            //variantsResult = SqlMethods.getmemberVariants(DBName, memberID);
            //if (variantsResult == "19")
            //    return variantsResult;
            // variantsResult = variantsResult.Replace("DocumentElement", "variants");

            result = "<AllowedBenefits>" + campaignsResult;// +variantsResult + "</AllowedBenefits>";
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("getAllVariants-> \n OrganizationID -> " + organizationID + "\n Password -> " + organizationPassword
             + "\n Exception Type -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        //finally
        //{
        //    DataBase.CloseConnection();
        //}
        return result;

    }

    //public static string GetNetBenefits(string netID, string netPassword, string cardNumber)
    //{
    //    string buisnessID = "";
    //    try
    //    {
    //        //DataBase.OpenConnection();
    //        //buisnessID = SqlMethods.NetLogin(netID, netPassword);
    //        if (buisnessID == "") // netId, or netPosId or password is wrong 
    //            return "9";


    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //    //finally
    //    //{
    //    //    DataBase.CloseConnection();
    //    //}
    //    return "1";
    //}

    //public static string UpdateCustomerDetails(string organizationID, string organizationPassword, string xmlCustomerDetails, string ipUserHost)
    //{
    //    string dbName = null;
    //    string mainField = null;
    //    bool enforcementIdentity = true;
    //    StringBuilder customerDetails = new StringBuilder();

    //    if (string.IsNullOrEmpty(organizationID))
    //        return "22";

    //    if (string.IsNullOrEmpty(organizationPassword))
    //        return "10";

    //    try
    //    {
    //        DataBase.OpenConnection();

    //        //Check Password And Return DBName
    //        dbName = SqlMethods.GetPasswordANDdbNameForOrgID(organizationID, organizationPassword, ref mainField, ref enforcementIdentity, ipUserHost);
    //        if (dbName == "19" || dbName == "9" || dbName == "30")//wrong (organizationPassword or organizationID), Connection Lost or IP is not in authorized
    //            return dbName;

    //        XmlDocument xmlDoc = new XmlDocument();
    //        xmlDoc.LoadXml(xmlCustomerDetails);
    //        XmlNode xml = xmlDoc.SelectSingleNode("Customers");
    //        XmlNodeList nodeList = xml.ChildNodes;
    //        for (int i=0; i<nodeList.Count; i++)
    //        {
    //            customerDetails.Append (parseXml(dbName, null, nodeList.Item(i).OuterXml));
    //            if (customerDetails.ToString() == "39" || customerDetails.ToString() == "40")
    //                return customerDetails.ToString();

    //            customerDetails.Append("\ngo\n");    
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        SqlMethods.SendMail("UpdateCustomerDetails-> \n OrganizationID -> " + organizationID + "\n Password -> " + organizationPassword
    //         +  "\n Exception Type -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
    //        return "39";
    //    }

    //    finally
    //    {
    //        DataBase.CloseConnection();
    //    }
    //    return "41"; 
    //}

    private static string checkMissingDetails(string merchantUser, string merchantPassword, string cardNumber)
    {
        if (string.IsNullOrEmpty(merchantUser))
            return "10"; // merchantUser id is missing
        if (string.IsNullOrEmpty(merchantPassword))
            return "10"; // merchantPassword password is missing
        if (string.IsNullOrEmpty(cardNumber))
            return "31"; // card number is missing

        return "0";
    }

    //public static string GetNetBenefits(string MerchantUser, string MerchantPassword, string cardNumber, string xmlTransactionDetail)
    //{
    //    return "1";
    //}

    public static string isCustomerExists(string merchantUser, string merchantPassword,
                                          string cardNumber, string xmlTransactionDetail)
    {
        string merchantID = "", merchantPos = "";
        string memberID = "";
        string DBName = "";
        string result = "";

        result = checkMissingDetails(merchantUser, merchantPassword, cardNumber);
        if (result != "0") // 
            return result;
        try
        {
            //DataBase.OpenConnection();

            // check the merchent user name and password (MerchantPOSs table)
            result = SqlMethods.MerchentLogin(merchantUser, merchantPassword, ref merchantID, ref merchantPos);
            if (result == "9" || result == "19") // merchantUserName or merchantPassword is wrong or connection lost 
                return result;

            //get the DB name  
            DBName = SqlMethods.GetDBNameByCardPrefix(cardNumber);
            if (DBName == "32" || DBName == "19") //member does not exists OR Connection Lost
                return DBName;

            //Check If The Member Exists
            memberID = SqlMethods.isCustomerExists(DBName, cardNumber);
            if (memberID == "19" || memberID == "2")//member does not exists or the card is not active OR Connection Lost
                return memberID;

            // save the transaction detail 
            SqlMethods.saveCampaignUse(memberID, cardNumber, merchantID, merchantPos, DBName, xmlTransactionDetail);

            return result;
        }

        catch (Exception ex)
        {
            SqlMethods.SendMail("Function -> isCustomerExists \n DBName: " + DBName + "\nCard number: " + cardNumber +
              "Exception TYPE -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";

        }
        //finally
        //{
        //    DataBase.CloseConnection();
        //}
        //return "1";
    }


    // benny 06/09/2012
    // this function is used in IPA only so i move it to IpaMoney project instead of order.Dll
    //    public static void getMemberDetails(string memberID, string cardNumber, ref DataTable dataTable)
    //    {
    //        if (!BaseMethods.IsDigit(memberID))
    //            return;
    //        if (memberID.Length < 9)//ת"ז פחות מ9 ספרות
    //            memberID = BaseMethods.AddZeroToID(memberID);

    //        string query = String.Format(@"SELECT MemberName, Address, CityName, ZIP, PhoneNumber, MobilePhone, Email, 
    //                    WaitingBalance, CardNumber, CardStatus FROM IPA..AllMembers INNER JOIN IPA..Cards ON MemberId = IDMember
    //                    WHERE (MemberId = '{0}' and CardNumber='{1}')", memberID, cardNumber);

    //        dataTable = DataBase.FillDataTable(query, "personalDetails");
    //    }


    //    public static void getMemberDetails(string memberID, ref DataTable dataTable)
    //    {
    //        //DataTable dataTable;
    //        if (!BaseMethods.IsDigit(memberID))
    //            return;
    //        if (memberID.Length < 9)//ת"ז פחות מ9 ספרות
    //            memberID = BaseMethods.AddZeroToID(memberID);
    //        //DataBase.OpenConnection();

    //        string query = String.Format(@"SELECT IPA..AllMembers.MemberName, IPA..AllMembers.Address, IPA..AllMembers.CityName, 
    //                    IPA..AllMembers.ZIP, IPA..AllMembers.PhoneNumber, IPA..AllMembers.MobilePhone, IPA..AllMembers.Email, 
    //                    IPA..Cards.CardNumber, IPA..Cards.CardStatus FROM IPA.dbo.AllMembers INNER JOIN
    //                    IPA.dbo.Cards ON IPA.dbo.AllMembers.MemberId = IPA.dbo.Cards.IDMember
    //                    WHERE (IPA..AllMembers.MemberId = '{0}')", memberID);

    //        dataTable = DataBase.FillDataTable(query, "personalDetails");
    //        //DataBase.ExecuteAdapter(query, ref dataTable);
    //        //DataBase.CloseConnection();
    //        //return dataTable;
    //    }

    private static string ValidateInput(ref string organizationID, ref string organizationPassword, ref string cardNumber,
                                                ref string productID, ref string merchantNO)
    {
        if (string.IsNullOrEmpty(organizationID))
            return "22"; // organization id is missing
        if (string.IsNullOrEmpty(organizationPassword))
            return "10"; // organization password is missing
        if (string.IsNullOrEmpty(cardNumber))
            return "31"; // card number is missing
        if (string.IsNullOrEmpty(productID))
            return "8"; // productID is missing
        if (string.IsNullOrEmpty(merchantNO))
            return "43"; // merchantNO is missing
        organizationID = organizationID.Trim();
        organizationPassword = organizationPassword.Trim();
        cardNumber = cardNumber.Trim();
        productID = productID.Trim();
        merchantNO = merchantNO.Trim();
        return "0";
    }

    public static string GetBenefitBalance(string organizationID, string organizationPassword, string cardNumber,
        string productID, string merchantNO, string IP, out string resultCode)
    {
        bool enforcementIdentity = false, ActiveCardNotNeededToOrder = false;
        bool methodPremission = false, AllowAnonymousTransactions = false;
        string dbName, mainField = "", memberID = "", orgUserID = "", ipList = "", resultMessage, xmlString;

        resultCode = ValidateInput(ref organizationID, ref organizationPassword, ref cardNumber, ref productID, ref merchantNO);

        if (resultCode != "0")// error occur 
        {
            resultMessage = SqlMethods.GetErrorMessage(resultCode);
            xmlString = Xmlbuilder.GetFailureXmlString(resultCode, resultMessage);
            return xmlString;
        }

        try
        {
            //Check Valid Organization
            SqlMethods.OrganizationLogin(ref organizationID, organizationPassword, ref orgUserID, ref methodPremission, ref ipList);
            if (string.IsNullOrEmpty(ipList) || !BaseMethods.IsDigit(organizationID))
            {
                resultCode = "9";
                resultMessage = SqlMethods.GetErrorMessage(resultCode);
                xmlString = Xmlbuilder.GetFailureXmlString(resultCode, resultMessage);
                return xmlString;
            }
            else
            {
                bool passIP = BaseMethods.CheckIP(ipList, IP);
                if (!passIP)
                {
                    resultCode = "30";
                    resultMessage = SqlMethods.GetErrorMessage(resultCode);
                    xmlString = Xmlbuilder.GetFailureXmlString(resultCode, resultMessage);
                    return xmlString;
                }
            }

            //Check Password And Return DBName
            dbName = SqlMethods.GetPasswordANDdbNameForOrgID(organizationID, ref mainField, ref enforcementIdentity,
                "", ref ActiveCardNotNeededToOrder, ref AllowAnonymousTransactions);
            if (dbName == "19")    //Connection Lost 
            {
                resultCode = "19";
                resultMessage = SqlMethods.GetErrorMessage(resultCode);
                xmlString = Xmlbuilder.GetFailureXmlString(resultCode, resultMessage);
                return xmlString;
            }


            //Check Premission To Method
            StackTrace stackTrace = new StackTrace();
            if (methodPremission)
            {
                if (!SqlMethods.CheckPremissionToMethod(orgUserID, stackTrace.GetFrame(1).GetMethod().Name))
                {
                    resultCode = "60";
                    resultMessage = SqlMethods.GetErrorMessage(resultCode);
                    xmlString = Xmlbuilder.GetFailureXmlString(resultCode, resultMessage);
                    return xmlString;
                }
            }

            memberID = SqlMethods.GetActiveMemberID(dbName, cardNumber);
            if (memberID == "1") // card is not active 
            {
                resultCode = "17";
                resultMessage = SqlMethods.GetErrorMessage(resultCode);
                xmlString = Xmlbuilder.GetFailureXmlString(resultCode, resultMessage);
                return xmlString;
            }

            else if (memberID == "2")// unknown card number 
            {
                resultCode = "42";
                resultMessage = SqlMethods.GetErrorMessage(resultCode);
                xmlString = Xmlbuilder.GetFailureXmlString(resultCode, resultMessage);
                return xmlString;
            }

            DataTable dtBenefit = new DataTable();
            SqlMethods.GetMerchantBenefit(organizationID, merchantNO, productID, ref dtBenefit);
            if (dtBenefit == null || dtBenefit.Rows.Count < 1)
            {
                resultCode = "44";
                resultMessage = SqlMethods.GetErrorMessage(resultCode);
                xmlString = Xmlbuilder.GetFailureXmlString(resultCode, resultMessage);
                return xmlString;
            }

            if (dtBenefit.Rows.Count > 1)
            {
                resultCode = "45";
                resultMessage = SqlMethods.GetErrorMessage(resultCode);
                xmlString = Xmlbuilder.GetFailureXmlString(resultCode, resultMessage);
                return xmlString;
            }

            DataRow row = dtBenefit.Rows[0];

            //string CampaignID, BenefitID, CampaignLimit, BenefitGeneralLimit, BenefitPersonLimit;
            int CampaignID, BenefitID, CampaignLimit, BenefitGeneralLimit, BenefitPersonLimit;

            CampaignID = Convert.ToInt32(row["CampaignID"]);
            BenefitID = Convert.ToInt32(row["BenefitID"]);
            CampaignLimit = Convert.ToInt32(row["CampaignLimit"]);
            BenefitGeneralLimit = Convert.ToInt32(row["BenefitGeneralLimit"]);
            BenefitPersonLimit = Convert.ToInt32(row["BenefitPersonLimit"]);

            int CampaignBalance, BenefitGeneralBalance, BenefitPersonalBalance, numOfUse;

            //Check Campaign balance
            string condition = string.Format("CampaignID = {0}", CampaignID);
            numOfUse = SqlMethods.GetNumberOfUse(dbName, condition);
            if (numOfUse > CampaignLimit)
            {
                resultCode = "46";
                resultMessage = SqlMethods.GetErrorMessage(resultCode);
                xmlString = Xmlbuilder.GetFailureXmlString(resultCode, resultMessage);
                return xmlString;
            }
            if (numOfUse == -1)
                CampaignBalance = CampaignLimit;
            else
                CampaignBalance = CampaignLimit - numOfUse;


            //Check Benefit balance
            condition = string.Format("BenefitID = '{0}'", BenefitID);
            numOfUse = SqlMethods.GetNumberOfUse(dbName, condition);
            if (numOfUse > BenefitGeneralLimit)
            {
                resultCode = "47";
                resultMessage = SqlMethods.GetErrorMessage(resultCode);
                xmlString = Xmlbuilder.GetFailureXmlString(resultCode, resultMessage);
                return xmlString;
            }
            if (numOfUse == -1)
                BenefitGeneralBalance = BenefitGeneralLimit;
            else
                BenefitGeneralBalance = BenefitGeneralLimit - numOfUse;

            //Check member general balance
            condition = string.Format("BenefitID = '{0}' AND ID = '{1}'  ", BenefitID, memberID);
            numOfUse = SqlMethods.GetNumberOfUse(dbName, condition);
            if (numOfUse > BenefitPersonLimit)
            {
                resultCode = "48";
                resultMessage = SqlMethods.GetErrorMessage(resultCode);
                xmlString = Xmlbuilder.GetFailureXmlString(resultCode, resultMessage);
                return xmlString;
            }
            if (numOfUse == -1)
                BenefitPersonalBalance = BenefitPersonLimit;
            else
                BenefitPersonalBalance = BenefitPersonLimit - numOfUse;

            xmlString = Xmlbuilder.GetBalanceXmlString(CampaignBalance, BenefitGeneralBalance, BenefitPersonalBalance);
            resultCode = "0";
            return xmlString;//Entitled to benefit
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("GetBenefitBalance-> \n OrganizationID -> " + organizationID + "\n Password -> " + organizationPassword
             + "\n Exception Type -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);

            xmlString = Xmlbuilder.GetFailureXmlString("19", "general exception");
            resultCode = "19";
            return xmlString;
        }
    }

    public static string IsBenefitAllowed(string organizationID, string organizationPassword, string cardNumber, string productID,
            string merchantNO, string IP)
    {
        string dbName, mainField = "", memberID = "";
        bool enforcementIdentity = false;
        string result = ValidateInput(ref organizationID, ref organizationPassword, ref cardNumber, ref productID, ref merchantNO);

        if (result != "0")// error occur 
            return result;
        bool ActiveCardNotNeededToOrder = false;

        try
        {
            bool methodPremission = false, AllowAnonymousTransactions = false;
            string orgUserID = "";
            string ipList = "";
            //Check Valid Organization
            SqlMethods.OrganizationLogin(ref organizationID, organizationPassword, ref orgUserID, ref methodPremission, ref ipList);
            if (string.IsNullOrEmpty(ipList)
                || !BaseMethods.IsDigit(organizationID))
                return "9";
            else
            {
                bool passIP = BaseMethods.CheckIP(ipList, IP);
                if (!passIP)
                    return "30";
            }


            //Check Password And Return DBName
            dbName = SqlMethods.GetPasswordANDdbNameForOrgID(organizationID, ref mainField, ref enforcementIdentity,
                "", ref ActiveCardNotNeededToOrder, ref AllowAnonymousTransactions);
            if (dbName == "19")    //Connection Lost 
                return dbName;

            //Check Premission To Method
            StackTrace stackTrace = new StackTrace();
            if (methodPremission)
            {
                if (!SqlMethods.CheckPremissionToMethod(orgUserID, stackTrace.GetFrame(1).GetMethod().Name))
                    return "60";
            }

            memberID = SqlMethods.GetActiveMemberID(dbName, cardNumber);
            if (memberID == "1") // card is not active 
                return "17";
            else if (memberID == "2")// unknown card number 
                return "42";

            DataTable dtBenefit = new DataTable();
            SqlMethods.GetMerchantBenefit(organizationID, merchantNO, productID, ref dtBenefit);
            if (dtBenefit == null || dtBenefit.Rows.Count < 1)
                return "44";//No Benefit found //to member

            if (dtBenefit.Rows.Count > 1)
                return "45";//More then one benefit

            DataRow row = dtBenefit.Rows[0];

            string CampaignID, BenefitID, CampaignLimit, BenefitGeneralLimit, BenefitPersonLimit;

            CampaignID = row["CampaignID"].ToString();
            BenefitID = row["BenefitID"].ToString();
            CampaignLimit = row["CampaignLimit"].ToString();
            BenefitGeneralLimit = row["BenefitGeneralLimit"].ToString();
            BenefitPersonLimit = row["BenefitPersonLimit"].ToString();

            //Check Campaign Limit
            string condition = string.Format("CampaignID = '{0}' ) >= {1} ", CampaignID, CampaignLimit);
            if (!SqlMethods.CheckLimitForBenefit(dbName, condition))
                return "46";//Over Campaign Limit

            //Check Benefit Limit
            condition = string.Format("BenefitID = '{0}' ) >= {1} ", BenefitID, BenefitGeneralLimit);
            if (!SqlMethods.CheckLimitForBenefit(dbName, condition))
                return "47";//Over Benefit Limit

            //Check member general Limit
            condition = string.Format("BenefitID = '{0}' AND ID = '{1}') >= {2} ", BenefitID, memberID, BenefitPersonLimit);
            if (!SqlMethods.CheckLimitForBenefit(dbName, condition))
                return "48";//Over Person Limit

            if (!Convert.ToBoolean(row["HasLimits"]))
            {
                //and year (UseTime) = Year (GetDate())
                //and year (UseTime) = Year (GetDate()) and month(UseTime) = month(GetDate())
                //and year (UseTime) = Year (GetDate()) and DATENAME(quarter, UseTime) = DATENAME(quarter, GetDate())
                //and year (UseTime) = Year (GetDate()) and DATENAME(week, UseTime)= DATENAME(week, GetDate()) 
                //and year (UseTime) = Year (GetDate()) and month(UseTime) = month(GetDate()) and day (UseTime) = day (GetDate())

                // member limits 

                // product limits 
            }

            return "0";//Entitled to benefit
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("IsBenefitAllowed-> \n OrganizationID -> " + organizationID + "\n Password -> " + organizationPassword
             + "\n Exception Type -> " + ex.GetType() + "\n Exception Message -> " + ex.Message);
            return "19";
        }
        //finally
        //{
        //    DataBase.CloseConnection();
        //}
        //return result;
    }

    public static string UseMerchantBenefit(string organizationID, string organizationPassword, string cardNumber, string productID,
                        string merchantNO, string posNumber, string IP, string NumOfUse)
    {
        int executeResult;
        string dbName, mainField = "", memberID = "";
        bool enforcementIdentity = false;
        string result = ValidateInput(ref organizationID, ref organizationPassword, ref cardNumber, ref productID, ref merchantNO);
        if (result != "0")// error occur 
            return result;
        bool ActiveCardNotNeededToOrder = false;

        try
        {
            bool methodPremission = false, AllowAnonymousTransactions = false;
            string orgUserID = "";
            string ipList = "";
            //Check Valid Organization
            SqlMethods.OrganizationLogin(ref organizationID, organizationPassword, ref orgUserID, ref methodPremission, ref ipList);
            if (string.IsNullOrEmpty(ipList)
                || !BaseMethods.IsDigit(organizationID))
                return "9";
            else
            {
                bool passIP = BaseMethods.CheckIP(ipList, IP);
                if (!passIP)
                    return "30";
            }


            //Check Password And Return DBName
            dbName = SqlMethods.GetPasswordANDdbNameForOrgID(organizationID, ref mainField, ref enforcementIdentity,
                "", ref ActiveCardNotNeededToOrder, ref AllowAnonymousTransactions);
            if (dbName == "19")    //Connection Lost 
                return dbName;
            //Check Premission To Method
            StackTrace stackTrace = new StackTrace();
            if (methodPremission)
            {
                if (!SqlMethods.CheckPremissionToMethod(orgUserID, stackTrace.GetFrame(1).GetMethod().Name))
                    return "60";
            }

            memberID = SqlMethods.GetActiveMemberID(dbName, cardNumber);
            if (memberID == "1") // card is not active 
                return "17";
            else if (memberID == "2")// unknown card number 
                return "42";

            DataTable dtBenefit = new DataTable();
            SqlMethods.GetMerchantBenefit(organizationID, merchantNO, productID, ref dtBenefit);
            if (dtBenefit == null || dtBenefit.Rows.Count < 1)
                return "44";//No Benefit found 

            if (dtBenefit.Rows.Count > 1)
                return "45";//More then one benefit

            DataRow row = dtBenefit.Rows[0];

            string CampaignID, CampaignName, BenefitID, BenefitName, groupID;
            int benefitPersonalLimit = 0;

            CampaignID = row["CampaignID"].ToString();
            CampaignName = row["CampaignName"].ToString();
            BenefitID = row["BenefitID"].ToString();
            BenefitName = row["BenefitName"].ToString();
            groupID = row["GroupID"].ToString();
            benefitPersonalLimit = Convert.ToInt32(row["BenefitPersonLimit"]);


            string query = string.Format(@"insert into {0}..CampaignMembersUse (UseTime, CampaignID, BenefitID, GroupID, POSID, MerchantID, ID, CardNumber, 
                NumOfUse) values (GetDate(), '{1}', '{2}', '{3}' , '{4}', '{5}', '{6}', '{7}', '{8}' )",
                dbName, CampaignID, BenefitID, groupID, posNumber, merchantNO, memberID, cardNumber, NumOfUse);
            executeResult = DataBase.ExecuteNonQuery(query);
            if (executeResult != 1)
                return "19";
            else
            {
                if (organizationID == "17" || organizationID == "18") // yediot - check if user exceed campaign limit
                    CheckCampaignLimit(dbName, memberID, cardNumber, merchantNO, CampaignID, CampaignName, BenefitID,
                                                            BenefitName, benefitPersonalLimit);
            }
        }
        catch (Exception ex)
        {
            return "19";
        }
        return "0";
    }

    public static void CheckCampaignLimit(string dbName, string memberId, string cardNumber, string merchantNO,
                string campaignId, string campaignName, string benefitID, string benefitName, int benefitPersonalLimit)
    {
        int numOfUse = GetNumbOfUse(dbName, memberId, benefitID);

        if (numOfUse > benefitPersonalLimit) // insert alert message to EmailQueue table.
        {
            StringBuilder body = new StringBuilder();
            body.Append("<html xmlns='http://www.w3.org/1999/xhtml'><head> <title> התראה - חריגה ממגבלת קמפיין</title> </head> <body dir='rtl'>");
            body.Append("<b>אירעה חריגה ממגבלת קמפיין.</b><br/>");
            body.Append("מספר עסק: " + merchantNO + "<br/>");
            body.Append("מספר קמפיין: " + campaignId + "<br/>");
            body.Append("שם הקמפיין: " + campaignName + "<br/>");
            body.Append("מספר הטבה: " + benefitID + "<br/>");
            body.Append("שם ההטבה: " + benefitName + "<br/>");
            body.Append("מספר חבר: " + memberId + "<br/>");
            body.Append("מספר כרטיס: " + cardNumber + "<br/>");
            body.Append("מגבלה כללית לחבר: " + benefitPersonalLimit.ToString() + "<br/>");
            body.Append("כמות מימושים לחבר להטבה: " + numOfUse);
            body.Append("</body></html>");

            try
            {
                DataBase.InsertMailToEmailQueueTable("defaultEmail@yourdomain.com", "Mec_netali@ymenuim.co.il", "campaign limit alert", body.ToString(),
                    null, "sh_suzim@ymenuim.co.il", 17, -1, 100000 + 17);
            }
            catch (Exception ex)
            {
                SqlMethods.SendMail("File: Main.cs \nfunction: CheckCampaignLimit.\nException: " + ex.Message,
                                                                        "stored procedure - InsertOrderMail failed");
            }
        }
    }

    private static int GetNumbOfUse(string dbName, string memberId, string benefitID)
    {
        string query = string.Format(@"SELECT SUM(NumOfUse) FROM {0}..CampaignMembersUse Where ID = '{1}' and BenefitID = {2}",
                dbName, memberId, benefitID);
        object result = DataBase.ExecuteScalar(query);

        if (result == DBNull.Value)
            return 0;
        return Convert.ToInt32(result);
    }

    public static string SetMemberCampaign(string OrganizationID, string OrganizationPassword, string MemberID, string CardNumber,
        string CampaignID, string SmsPhoneNumber, string MemberFirstName, string MemberLastName, string IsPotentialClient, string AllowSMS, string ipUserHost, string metaData, string email)
    {
        string SeventhCardDigit = "";

        #region Check Values
        //Check OrganizationID Empty
        if (string.IsNullOrEmpty(OrganizationID))
            //|| !BaseMethods.IsDigit(OrganizationID))
            return "22";//חסר קוד אירגון
        else
            OrganizationID = OrganizationID.Trim();

        //Check Password Empty
        if (string.IsNullOrEmpty(OrganizationPassword))
            return "10";//חסרה סיסמה      
        else
            OrganizationPassword = OrganizationPassword.Trim();

        //Check MemberID Empty
        if (string.IsNullOrEmpty(MemberID))
            return "5";//חסר מזהה לקוח
        else
        {
            MemberID = MemberID.Trim();
            if (!BaseMethods.IsDigit(MemberID))
                return "4";
        }

        if (string.IsNullOrEmpty(IsPotentialClient) ||
            !BaseMethods.IsDigit(IsPotentialClient))
            return "19";
        else
            IsPotentialClient = IsPotentialClient.Trim();


        if (string.IsNullOrEmpty(AllowSMS))
            AllowSMS = "";
        else if (!BaseMethods.IsDigit(AllowSMS))
            return "19";
        else
            AllowSMS = AllowSMS.Trim();

        //Check CardNumber Empty
        if (string.IsNullOrEmpty(CardNumber))
        {
            if (IsPotentialClient == "0")
                return "31";//חסר מספר כרטיס
        }
        else if (!BaseMethods.IsDigit(CardNumber))
            return "31";
        else
        {
            CardNumber = CardNumber.Trim();
            if (CardNumber.Length < 16)
                SeventhCardDigit = "9";
            else
                SeventhCardDigit = CardNumber.Substring(6, 1);
        }

        //Check CampaignID Empty
        if (string.IsNullOrEmpty(CampaignID) ||
            !BaseMethods.IsDigit(CampaignID))
            return "49";//חסר קוד קמפיין      
        else
            CampaignID = CampaignID.Trim();

        //Email
        //if (string.IsNullOrEmpty(email))
        //    return "55";//חסר קוד קמפיין      
        //else
        //    email = email.Trim();

        #endregion

        string dbName = "";
        string mainField = "";
        bool enforcementIdentity = false, AllowAnonymousTransactions = false;
        bool ActiveCardNotNeededToOrder = false;
        string orgSmsPhone = "";


        //TRIM()
        if (!string.IsNullOrEmpty(SmsPhoneNumber))
            SmsPhoneNumber = SmsPhoneNumber.Trim().Replace("'", "''");
        if (!string.IsNullOrEmpty(MemberFirstName))
            MemberFirstName = MemberFirstName.Trim().Replace("'", "''");
        if (!string.IsNullOrEmpty(MemberLastName))
            MemberLastName = MemberLastName.Trim().Replace("'", "''");

        if (!string.IsNullOrEmpty(AllowSMS))
            AllowSMS = AllowSMS.Trim();
        else
            AllowSMS = "";
        if (!string.IsNullOrEmpty(IsPotentialClient))
            IsPotentialClient = IsPotentialClient.Trim();
        else
            IsPotentialClient = "";

        bool methodPremission = false;
        string orgUserID = "";
        string ipList = "";
        //Check Valid Organization
        if (!metaData.Contains("FromGiftCard"))
        {
            SqlMethods.OrganizationLogin(ref OrganizationID, OrganizationPassword, ref orgUserID, ref methodPremission, ref ipList);
            if (string.IsNullOrEmpty(ipList)
                || !BaseMethods.IsDigit(OrganizationID))
                return "9";
            else
            {
                bool passIP = BaseMethods.CheckIP(ipList, ipUserHost);
                if (!passIP)
                    return "30";
            }
        }


        bool encriptionRequired = false; bool isTradeSite = false; bool isOrderTable = false;
        dbName = SqlMethods.GetPasswordANDdbNameForOrgID(OrganizationID, ref mainField,
            ref enforcementIdentity, metaData, ref ActiveCardNotNeededToOrder, ref orgSmsPhone, ref encriptionRequired, ref AllowAnonymousTransactions, ref isTradeSite, ref isOrderTable);
        if (dbName == "19")//Connection Lost
            return dbName;

        if (!metaData.Contains("FromGiftCard"))
        {
            //Check Premission To Method
            StackTrace stackTrace = new StackTrace();
            if (methodPremission)
            {
                if (!SqlMethods.CheckPremissionToMethod(orgUserID, stackTrace.GetFrame(1).GetMethod().Name))
                    return "60";
            }
        }
        //Check ID length & Add Zero's
        if (mainField == null || mainField == "0")
        {
            if (MemberID.Length < 9)//ת"ז פחות מ9 ספרות
                MemberID = BaseMethods.AddZeroToID(MemberID);
        }

        //Find groupID 
        int groupType = 0;
        string checkLimit = "";
        bool addCoupon = false;
        string SmsText = "";
        bool sendSMS = false;
        int stockCoupon = 0;

        string groupID = SqlMethods.GetgroupIDForCampaignID(CampaignID, OrganizationID, ref groupType, ref SmsText, ref addCoupon, ref stockCoupon, ref sendSMS);
        if (groupID == "0")
            return "19";
        if (string.IsNullOrEmpty(groupID))//There is no group
            return "50";

        //בחרו להוסיף קופון אך לא נבחר בנק קופונים במסך העריכה ולכן אנחנו מתייחסים כאילו לא ביקשו להוסיף קופון
        if (stockCoupon == 0 && addCoupon)
            addCoupon = false;


        //string checkLimit = "";
        //bool addCoupon = false;
        //string SmsText = "";
        //bool sendSMS = false;
        //int stockCoupon = 0;

        //SqlMethods.GetCampaignPropToSMS(CampaignID, ref SmsText, ref addCoupon, ref stockCoupon, ref sendSMS);

        if (!metaData.Contains("FromGiftCard"))
        {
            if (string.IsNullOrEmpty(IsPotentialClient) ||
                IsPotentialClient == "5")//Check limit for memberID OR CampaignBenefit
            {
                //Check BenefitCost Limits
                double CampBenefitCost = SqlMethods.GetCampBenefitCost(CampaignID);
                double MemberBenefitCost = SqlMethods.GetYearlyBenefitCostForMemberID(MemberID);
                double YearlyBenefitCost = Convert.ToDouble(ConfigurationManager.AppSettings["Benefit_Limit_Cost_Katin_Buzzer"]);
                if ((MemberBenefitCost + CampBenefitCost) > YearlyBenefitCost)
                    return "77";
                else
                    return "78";
            }

            //Check Category Limit
            double dayLimit = 0;
            Int16 categoryID = 0;
            int CategoryLimitQuantity = 0;
            SqlMethods.GetCategoryLimitForCampaignID(CampaignID, OrganizationID, ref dayLimit, ref categoryID, ref CategoryLimitQuantity);
            if (categoryID == -1)//Error 
                return "19";
            else if ((dayLimit > 0 || CategoryLimitQuantity > 0) && categoryID > 0)//There are campaigns that are not associated category
            {
                //dayLimit - limit for load anoter campaign in this category (in days)


                if (CategoryLimitQuantity > 0)
                {//11-9-2012
                    //The limit is only for the current month
                    int countLoadCamp = SqlMethods.CountLoadCamp(categoryID, MemberID);
                    if (countLoadCamp >= CategoryLimitQuantity)
                        return "53";//Order has not passed the Category Limit Quantity of category
                }
                else
                {
                    //1-5-2011
                    //the limit for load campaign in this category (in month)
                    DateTime lastAddedGroup = SqlMethods.GetLastDateTimeAdded(dbName, categoryID, MemberID);
                    DateTime lastAddedCoupon = SqlMethods.GetLastDateTimeAddedCoupon(categoryID, MemberID);
                    DateTime lastAddedCouponGlobal = SqlMethods.GetLastDateTimeAddedCouponGlobal(categoryID, MemberID);
                    ////for days limit
                    //if (lastAdded.Date.AddDays(dayLimit) > DateTime.Today)
                    //    return "53";//Order has not passed the time limit to category
                    if (lastAddedCouponGlobal > lastAddedGroup)
                        lastAddedGroup = lastAddedCouponGlobal;

                    DateTime maxAdded = DateTime.MinValue;
                    if (lastAddedGroup > lastAddedCoupon)
                        maxAdded = lastAddedGroup;
                    else
                        maxAdded = lastAddedCoupon;

                    //for month limit
                    //על פי הבקשה האחרונה של באזזר - כל רישום שבוצע בשנה שעברה לא נחשב
                    if ((maxAdded.Year == DateTime.Today.Year)
                        && (maxAdded.Month + dayLimit > DateTime.Today.Month))
                        //||
                        //(maxAdded.Year < DateTime.Today.Year)
                        //&& (((maxAdded.Month + dayLimit - 12) > DateTime.Today.Month)))
                        return "53";//Order has not passed the time limit to category
                }
            }
            if (string.IsNullOrEmpty(IsPotentialClient) ||
                IsPotentialClient == "3")//Check category limit for memberID
                return "54";

            //Check Benefit Limit
            checkLimit = CheckLimitsForCampaignBenefit(CampaignID, dbName, MemberID, groupType, addCoupon, stockCoupon);
            if (!string.IsNullOrEmpty(checkLimit))
                return checkLimit;

            if (string.IsNullOrEmpty(IsPotentialClient) ||
                IsPotentialClient == "4")//Check limit for memberID OR CampaignBenefit
                return "56";
        }
        //Check If The Member Exists
        string back = SqlMethods.ThereIsMemberID(dbName, MemberID);

        if (back != "2" && (!string.IsNullOrEmpty(MemberLastName) || !string.IsNullOrEmpty(MemberFirstName)))
            SqlMethods.UpdateMemberName(dbName, MemberID, MemberFirstName, MemberLastName, SmsPhoneNumber, email);

        if (back == "19")//ID Not Exists OR Connection Lost
            return back;
        else if (back == "2")//AddMemberAndCard
        {
            int rows = SqlMethods.AddMemberAndCard(dbName, MemberID, CardNumber, MemberFirstName, MemberLastName, SmsPhoneNumber, IsPotentialClient, encriptionRequired, email);
            if (rows == 19 || rows == 0)//Not insert Or Exception
                return rows.ToString();
            if (encriptionRequired)
                SqlMethods.EncryptCardNumber(dbName, CardNumber, "Cards");
        }
        else if (IsPotentialClient == "0" && !string.IsNullOrEmpty(CardNumber))
        {//התווסף לטובת לקוחות שהצטרפו בעבר אך כעת הם לקוחות הבנק ולא לקוחות פוטנציאלים
            back = SqlMethods.GetCardNumberByMemberIDEncrypted(dbName, MemberID);
            if (string.IsNullOrEmpty(back) //או שאין כרטיס ללקוח   
                || back != CardNumber)//או שכרטיס שקיים אצלנו לא שווה לכרטיס שאיתו נרשם הלקוח
            {//במקרה שכזה מוסיפים כרטיס ללקוח
                SqlMethods.AddCardForMemberID(dbName, MemberID, CardNumber);
                if (encriptionRequired)
                    SqlMethods.EncryptCardNumber(dbName, CardNumber, "Cards");
            }
        }



        Int64 addToGroup = SqlMethods.AddMemberToGroupID(dbName, MemberID, CampaignID, groupID);
        if (addToGroup >= 0 && !addCoupon)//Add Global Coupon
            SqlMethods.AddToGlobalCoupon(MemberID, CampaignID, SeventhCardDigit, SmsPhoneNumber);
        if (addToGroup > 0 || string.IsNullOrEmpty(checkLimit))
        {
            //Send SMS
            if (AllowSMS == "1")
            {
                //addCoupon = false;

                //                int stockCoupon = 0;
                //if (metaData.Contains("FromGiftCard"))
                //    SqlMethods.GetCampaignPropToSMS(CampaignID, ref SmsText, ref addCoupon, ref stockCoupon, ref sendSMS);
                if (sendSMS && !string.IsNullOrEmpty(SmsText))
                {
                    string coupon;
                    Int64 couponID = 0;
                    if (addCoupon && stockCoupon != 0)
                    {
                        SqlMethods.GetCoupon(stockCoupon, MemberID, CardNumber, SmsPhoneNumber, SeventhCardDigit, CampaignID, out couponID, out coupon);
                        if (!string.IsNullOrEmpty(coupon))
                        {
                            if (SmsText.Contains("{0}"))
                                SmsText = string.Format(SmsText, coupon);
                            else
                                SmsText += " " + coupon;
                        }

                    }
                    //Send Email
                    //SqlMethods.SendMail(SmsText, "לאומי באזזר", "", email);
                    if (!BaseMethods.SendSMS(SmsPhoneNumber, SmsText, orgSmsPhone, 0, 120, OrganizationID, MemberID, couponID, 3))
                        SqlMethods.SendMail("We will not send SMS memberID " + MemberID
                            + " SMSPhoneNumber " + SmsPhoneNumber
                            + " SmsText " + SmsText
                            + " orgSmsPhone " + orgSmsPhone
                            + " CampaignID " + CampaignID, "Leumi SMS NOT SEND");
                }
            }

            return "52";//ההטבה נטענה בהצלחה
        }
        else if (addToGroup == 0)
            return "51";//החבר כבר שוייך לאוכלוסיה בעבר
        return "19";
    }

    private static string CheckLimitsForCampaignBenefit(string CampaignID, string dbName, string memberID, int groupType, bool addCoupon, int stockCoupon)
    {
        DataTable dtLimits = new DataTable();
        SqlMethods.getBenefitLimitProp(CampaignID, ref dtLimits);

        //int stockCoupon = 0;
        long benefitID = 0;
        bool hasNoLimits = false;

        //if (dtLimits.Rows[0]["AddCoupon"] != DBNull.Value)
        //    addCoupon = Convert.ToBoolean(dtLimits.Rows[0]["AddCoupon"]);
        //if (dtLimits.Rows[0]["StockCoupon"] != DBNull.Value)
        //    stockCoupon = Convert.ToInt32(dtLimits.Rows[0]["StockCoupon"]);
        if (dtLimits.Rows[0]["BenefitID"] != DBNull.Value)
            benefitID = Convert.ToInt64(dtLimits.Rows[0]["BenefitID"]);

        hasNoLimits = Convert.ToBoolean(dtLimits.Rows[0]["HasLimits"]);

        long limit = 0;
        bool general;

        for (int i = 4; i < dtLimits.Columns.Count; i++)
        {
            general = false;

            if (hasNoLimits && i > 5)//no Benefit Limit
                break;

            if (dtLimits.Rows[0][i] == DBNull.Value)
                continue;//There is no limit

            if (!Int64.TryParse(dtLimits.Rows[0][i].ToString(), out limit))
                continue;//There is no num of limit

            if (i == 4 || i > 10)
                general = true;

            long sumMimosh = 0;
            if (addCoupon)//בדיקה מול טבלת הקופונים לבדיקת זכאות לקמפיין
                sumMimosh = SqlMethods.CheckLimitCouponsStock(memberID, stockCoupon, general, dtLimits.Columns[i].ColumnName, CampaignID);
            else//בדיקה מול טבלת המימושים לבדיקת זכאות לקמפיין
            {
                long sumMimoshByGroup = 0;
                if (groupType == 3)//אם לא מחיייב הוספת קופון וסוג האוכלוסיה - תתווסף בהמשך
                    sumMimoshByGroup = SqlMethods.CheckLimitCouponsStock(memberID, 62, general, dtLimits.Columns[i].ColumnName, CampaignID);

                sumMimosh = SqlMethods.CheckLimitCampaignMembersUse(dbName, benefitID, CampaignID, memberID, general, dtLimits.Columns[i].ColumnName);
                if (sumMimoshByGroup > sumMimosh)// && addCoupon)//לוקחים את הגבוה מבין השניים כמגבלה
                                                 //if (sumMimoshByGroup > sumMimosh)//תיקון לטובת מגבלת טעינות עפ"י מגבלת האתר ז"א עפ"י מימושים
                    sumMimosh = sumMimoshByGroup;
            }

            if (sumMimosh >= limit)//past the limit
            {
                if (general)
                    return "47";
                else
                    return "48";
            }
        }
        return null;
    }



    public static string GetMemberID(string organizationID, string organizationPassword, string cardNumber, string IP)
    {
        XmlDocument document = new XmlDocument();
        XmlElement root = document.CreateElement("Response");
        document.AppendChild(root);


        #region Check Values
        if (string.IsNullOrEmpty(organizationID))// || !BaseMethods.IsDigit(organizationID))
        {
            AddChild("Result", "22", document, root);
            return document.OuterXml;
        }
        organizationID = organizationID.Trim();

        if (string.IsNullOrEmpty(organizationPassword))
        {
            AddChild("Result", "10", document, root);
            return document.OuterXml;
        }
        organizationPassword = organizationPassword.Trim();

        if (string.IsNullOrEmpty(cardNumber))
        {
            AddChild("Result", "31", document, root);
            return document.OuterXml;
        }
        #endregion

        bool methodPremission = false;
        string orgUserID = "";
        string ipList = "";

        //Check Valid Organization
        SqlMethods.OrganizationLogin(ref organizationID, organizationPassword, ref orgUserID, ref methodPremission, ref ipList);
        if (string.IsNullOrEmpty(ipList)
                || !BaseMethods.IsDigit(organizationID))
        {
            AddChild("Result", "9", document, root);
            return document.OuterXml;
        }
        else
        {
            bool passIP = BaseMethods.CheckIP(ipList, IP);
            if (!passIP)
            {
                AddChild("Result", "30", document, root);
                return document.OuterXml;
            }
        }

        bool enforcementIdentity = false, ActiveCardNotNeededToOrder = false, encriptionRequired = false, AllowAnonymousTransactions = false;
        string DBName, orgSmsPhone = "", mainField = ""; bool isTradeSite = false; bool isOrderTable = false;

        DBName = SqlMethods.GetPasswordANDdbNameForOrgID(organizationID, ref mainField,
            ref enforcementIdentity, "", ref ActiveCardNotNeededToOrder, ref orgSmsPhone, ref encriptionRequired, ref AllowAnonymousTransactions, ref isTradeSite, ref isOrderTable);
        if (DBName == "19")//Connection Lost
        {
            AddChild("Result", DBName, document, root);
            return document.OuterXml;
        }

        //Check Premission To Method
        StackTrace stackTrace = new StackTrace();
        if (methodPremission)
        {
            if (!SqlMethods.CheckPremissionToMethod(orgUserID, stackTrace.GetFrame(1).GetMethod().Name))
            {
                AddChild("Result", "60", document, root);
                return document.OuterXml;
            }
        }
        string cardStatus = null, memberID = null, memberStatus = null, tz = null, last4Digits = null;
        DataTable table = new DataTable();
        SqlMethods.GetCardDetails(DBName, cardNumber, table);
        if (table == null || table.Rows.Count == 0)// unknown card number 
        {
            AddChild("Result", "42", document, root);
            return document.OuterXml;
        }
        ExtractData(table.Rows[0], ref memberID, ref cardStatus, ref memberStatus, ref tz, ref last4Digits);

        AddChild("Result", "1", document, root);
        AddChild("MemberID", memberID.Trim(), document, root);
        AddChild("MemberStatus", memberStatus, document, root);
        AddChild("cardStatus", cardStatus, document, root);
        AddChild("tz", tz, document, root);
        AddChild("last4Digits", last4Digits, document, root);
        return document.OuterXml;
    }

    private static void ExtractData(DataRow row, ref string memberID, ref string cardStatus, ref string memberStatus,
        ref string tz, ref string last4Digits)
    {
        try
        {
            memberID = row["IDMember"].ToString();
            cardStatus = row["CardStatus"].ToString();
            memberStatus = row["MemberStatus"].ToString();
            if (row["TZ"] != DBNull.Value)
                tz = row["tz"].ToString();
            if (row["Last4Digits"] != DBNull.Value)
                last4Digits = row["Last4Digits"].ToString();
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("Function -> Main: ExtractData"
                            + "\n Exception TYPE -> " + ex.GetType()
                            + "\n Exception Message -> " + ex.Message
                            + "\n ex.Source -> " + ex.Source
                            + "\n ex.StackTrace -> " + ex.StackTrace, "WS Error");
        }
    }

    private static void AddChild(string elementName, string elementValue, XmlDocument document, XmlElement root)
    {
        XmlElement element = document.CreateElement(elementName);
        element.InnerText = elementValue;
        root.AppendChild(element);
    }

    //בהצדעה: פרמטר א' מספר אישי פרמטר ב' מספר כרטיס
    //שאר הארגונים: פרמטר א' מספר כרטיס פרמטר ב' מספר ת"ז

    public static string CheckCardNumber(string userName, string password, string memberID, string cardNumber, string ipUserHost)
    {
        #region Check Values

        if (string.IsNullOrEmpty(userName))
            return BaseMethods.GetCheckCardNumberAnswer("22", "", "");
        else
            userName = userName.Trim();

        if (string.IsNullOrEmpty(password))
            return BaseMethods.GetCheckCardNumberAnswer("10", "", "");
        else
            password = password.Trim();

        if (string.IsNullOrEmpty(cardNumber))
            return BaseMethods.GetCheckCardNumberAnswer("31", "", "");
        else
            cardNumber = cardNumber.Trim();

        if (string.IsNullOrEmpty(memberID))
            return BaseMethods.GetCheckCardNumberAnswer("5", "", "");
        else
            memberID = memberID.Trim();

        #endregion
        string back = "";
        string dbName = null;
        string orgID = null;

        string orgUserID = string.Empty, ipList = string.Empty;
        bool methodPremission = false;
        SqlMethods.OrganizationLogin(ref userName, password, ref orgUserID, ref methodPremission, ref ipList);
        if (string.IsNullOrEmpty(ipList)
            || orgUserID != "35")
            return BaseMethods.GetCheckCardNumberAnswer("9", "", "");
        else
        {
            bool passIP = BaseMethods.CheckIP(ipList, ipUserHost);
            if (!passIP)
                return BaseMethods.GetCheckCardNumberAnswer("30", "", ""); ;
        }

        //בודק קוד ארגון מול טבלת NETS
        orgID = BaseMethods.GetNetIDForCard(memberID, ref dbName);
        if (string.IsNullOrEmpty(orgID))
            return BaseMethods.GetCheckCardNumberAnswer("42", "", "");

        //ארגון בהצדעה לא פונים עם מספר כרטיס
        if (dbName == "Miluim")// || dbName == "HonorReleases")
        {
            //מוודא ארגון אליו שייך חבר בהצדעה
            back = FindMiluimMember(ref dbName, ref orgID, memberID);
            if (back == "2" || back == "19")
                return BaseMethods.GetCheckCardNumberAnswer(back, "", "");

            //מקבל מספר תעודה קיימת לטובת בדיקות מול אכא
            //מעדכן זכאות בבסיס הנתונים של הארגון

            //פונה לבדיקת זכאות באקא
            string aka = BaseMethods.AkaLogin(memberID, cardNumber, dbName);
            if (aka == "1")
                return BaseMethods.GetCheckCardNumberAnswer("57", orgID, memberID);
            else if (aka.Contains("בעיה כללית בהתקשרות לאתר"))
                return BaseMethods.GetCheckCardNumberAnswer("62", orgID, "");//במידה והחבר לא עובר את הזכאות באקא מבחינתנו לא מוכר
            return BaseMethods.GetCheckCardNumberAnswer("2", orgID, "");//במידה והחבר לא עובר את הזכאות באקא מבחינתנו לא מוכר
        }

        //בדיקת ת"ז המשוייכת לכרטיס
        back = SqlMethods.GetMemberIDByCardNumber(dbName, memberID);
        if (back == "2")
            return BaseMethods.GetCheckCardNumberAnswer(back, orgID, "");
        else
            return BaseMethods.GetCheckCardNumberAnswer("57", orgID, cardNumber);
    }

    public static string CheckLimitsIVR(string userName, string password, string orgID, string memberID, string productID, string quant, string ipUserHost)
    {
        int quantity = 0;
        #region Check Values

        if (string.IsNullOrEmpty(userName))
            return BaseMethods.GetCheckLimitsAnswer("22", "0", "0.00");
        else
            userName = userName.Trim();

        if (string.IsNullOrEmpty(password))
            return BaseMethods.GetCheckLimitsAnswer("10", "0", "0.00");
        else
            password = password.Trim();

        if (string.IsNullOrEmpty(memberID))
            return BaseMethods.GetCheckLimitsAnswer("5", "0", "0.00");
        else
            memberID = memberID.Trim();

        if (string.IsNullOrEmpty(productID))
            return BaseMethods.GetCheckLimitsAnswer("8", "0", "0.00");
        else
        {

            if (!productID.Contains("-"))
                return BaseMethods.GetCheckLimitsAnswer("7", "0", "0.00");
            productID = productID.Trim();
        }

        if (string.IsNullOrEmpty(orgID))
            return BaseMethods.GetCheckLimitsAnswer("22", "0", "0.00");
        else
            orgID = orgID.Trim();

        if (string.IsNullOrEmpty(quant))
            return BaseMethods.GetCheckLimitsAnswer("14", "0", "0.00");
        else
        {
            if (!int.TryParse(quant, out quantity))
                return BaseMethods.GetCheckLimitsAnswer("13", "0", "0.00");
            if (quantity == 0)
                return BaseMethods.GetCheckLimitsAnswer("15", "0", "0.00");
        }
        #endregion

        string dbName = "";
        string orgUserID = string.Empty, ipList = string.Empty;
        bool methodPremission = false;
        SqlMethods.OrganizationLogin(ref userName, password, ref orgUserID, ref methodPremission, ref ipList);
        if (string.IsNullOrEmpty(ipList)
            || orgUserID != "35")
            return BaseMethods.GetCheckLimitsAnswer("9", null, null);
        else
        {
            bool passIP = BaseMethods.CheckIP(ipList, ipUserHost);
            if (!passIP)
                return BaseMethods.GetCheckLimitsAnswer("30", null, null);
        }
        dbName = SqlMethods.GetDBNameForOrgID(orgID);
        productID = SqlMethods.GetProductIDFromIVRCode(dbName, productID);
        DataTable dtVar = new DataTable();
        SqlMethods.GetVariantProp(dbName, productID, ref dtVar);
        if (dtVar == null || dtVar.Rows.Count < 1)
            return BaseMethods.GetCheckLimitsAnswer("6", null, null);

        DataRow varRow = dtVar.Rows[0];
        if (Convert.ToBoolean(varRow["Disabled"]))//בודק אם ניתן להזמין את המוצר
            return BaseMethods.GetCheckLimitsAnswer("28", null, null);

        DataTable memberProp = new DataTable();
        SqlMethods.GetMemberProp(dbName, memberID, ref memberProp);
        if (memberProp == null || memberProp.Rows.Count < 1)
            return BaseMethods.GetCheckLimitsAnswer("2", null, null); ;

        string[] errors = null;
        Int16 subType = 0;

        //בדיקת מגבלות קטגוריה ומגבלות חבר

        //מגבלות קטגוריה
        if (varRow["BusinessSubTypeID"] != DBNull.Value)
        {
            if (Int16.TryParse(varRow["BusinessSubTypeID"].ToString(), out subType))
            {
                DataTable dtSubTypeLimit = new DataTable();
                SqlMethods.GetSubTypeLimits(dbName, subType, ref dtSubTypeLimit);
                if (dtSubTypeLimit != null && dtSubTypeLimit.Rows.Count > 0)
                {
                    LimitLogic.CheckGeneralLimitsLogic(dtSubTypeLimit.Rows[0], ref errors, memberID, dbName, quantity, subType.ToString(), varRow["shortNameVar"].ToString(), orgID);
                    if (errors != null && errors.Length > 0)
                        return BaseMethods.GetCheckLimitsAnswer("48", null, null, errors);
                }
            }
        }

        //מגבלות חבר למוצר
        LimitLogic.CheckGeneralLimitsLogic(varRow, ref errors, memberID, dbName, quantity, null, varRow["shortNameVar"].ToString(), orgID);
        if (errors != null && errors.Length > 0)
            return BaseMethods.GetCheckLimitsAnswer("47", null, null, errors);

        //חישוב מחיר למוצר הנדרש
        //PriceCalculation pc = new PriceCalculation();
        //pc.CalculatVariantTable_PriceCalculation(dtVar, memberProp.Rows[0]);
        PriceCalculation.CalculatVariantTable_PriceCalculation(dtVar, memberProp.Rows[0]);

        float price;
        if (dtVar.Rows[0]["VarPriceFormula"] != DBNull.Value)
        {
            if (!float.TryParse(dtVar.Rows[0]["VarPriceFormula"].ToString(), out price))
                return BaseMethods.GetCheckLimitsAnswer("19", null, null);
            return BaseMethods.GetCheckLimitsAnswer("58", quantity.ToString(), price.ToString());
        }

        return BaseMethods.GetCheckLimitsAnswer("19", null, null);
    }

    //מוודא ארגון אליו שייך חבר בהצדעה
    private static string FindMiluimMember(ref string dbName, ref string orgID, string memberID)
    {
        string back = SqlMethods.ThereIsMemberID(dbName, memberID);
        if (back != "1")
        {
            if (back == "19")
                return back;
            if (back == "2")
            {
                //מחפש בטבלת ארגון משוחררים
                BaseMethods.ChangeDB(ref orgID, ref dbName);
                back = SqlMethods.ThereIsMemberID(dbName, memberID);
            }
        }
        return back;
    }

    public static string IVROrderOnLine(int? PremiumType, string userName, string password, string orgID, string memberID, string productID, string quantity,
                string price, string creditCardNumber, string creditMemberID, string DialPhone, string ValidUntil, string CVV, string SMSPhoneNumber, string ipUserHost)
    {
        string confirmationNumber, chargeResultString, paymentID = null, databaseName = "";

        if (string.IsNullOrEmpty(orgID))
            return BaseMethods.GetIVROrderOnLineAnswer("19", databaseName, paymentID);



        #region Check Values

        if (string.IsNullOrEmpty(userName))
            return BaseMethods.GetIVROrderOnLineAnswer("22", databaseName, paymentID);
        else
            userName = userName.Trim();

        if (string.IsNullOrEmpty(password))
            return BaseMethods.GetIVROrderOnLineAnswer("10", databaseName, paymentID);
        else
            password = password.Trim();

        #endregion

        string orgUserID = string.Empty, ipList = string.Empty;
        bool methodPremission = false;
        SqlMethods.OrganizationLogin(ref userName, password, ref orgUserID, ref methodPremission, ref ipList);
        if (string.IsNullOrEmpty(ipList)
            || orgUserID != "35")
            return BaseMethods.GetIVROrderOnLineAnswer("9", databaseName, paymentID);
        else
        {
            bool passIP = BaseMethods.CheckIP(ipList, ipUserHost);
            if (!passIP)
                return BaseMethods.GetIVROrderOnLineAnswer("30", databaseName, paymentID);
        }

        databaseName = SqlMethods.GetDBNameForOrgID(orgID);
        string totalSum = GetTotalSum(price, quantity);
        bool charge = true;
        productID = SqlMethods.GetProductIDFromIVRCode(databaseName, productID);
        bool.TryParse(ConfigurationSettings.AppSettings["ChargeCreditCard"], out charge);
        bool chargeResult = false;
        if (charge)
        {
            //Debit Credit

            chargeResult = ChargeCreditCard(out chargeResultString, out confirmationNumber, orgID, totalSum, 1, creditCardNumber,
                            CVV, creditMemberID, ValidUntil, "Debit");
            XDocument doc = Payments.GetRoot();
            Payments.AddSource(doc, "IVR");
            if (chargeResult == true)// charge credit card end succesfully
                paymentID = Payments.SavePaymentDetails(creditCardNumber, creditMemberID, totalSum, confirmationNumber, databaseName, memberID, 0, 0, 0, 0, doc.ToString(), "", 0);
            else
                return BaseMethods.GetIVROrderOnLineAnswer("61", databaseName, paymentID);
        }
        string orgPassword = "";
        string orgSmsPhone = "";
        SqlMethods.GetOrgDetails(orgID, ref orgPassword, ref orgSmsPhone);
        string metaData = string.Format(
                                @"<Details><Source>IVR</Source><DialPhone>{0}</DialPhone><CC>{1}</CC><ValidUntil>{2}</ValidUntil><CVV>{3}</CVV><SMSPhoneNumber>{4}</SMSPhoneNumber><SlikaSapakDebit>קרדיט גארד</SlikaSapakDebit><OrderTZ>{5}</OrderTZ></Details>",
                                DialPhone, creditCardNumber.Substring(creditCardNumber.Length - 5, 4), ValidUntil, CVV, SMSPhoneNumber, creditMemberID);

        metaData = metaData.Replace("'", "''");

        string ans = InsertNewOrder(PremiumType, orgID, orgPassword, productID, memberID, quantity, "1-1", metaData, "", "192.168.111.112");

        long result = 0;

        if (!long.TryParse(ans, out result))
            return BaseMethods.GetIVROrderOnLineAnswer("19", databaseName, paymentID);

        if (result > 100)
        {
            bool sendSMS = true;
            bool.TryParse(ConfigurationSettings.AppSettings["SendSms"], out sendSMS);
            if (sendSMS)
            {
                DataTable dtProductRow = new DataTable();
                SqlMethods.GetVariantProp(databaseName, productID, ref dtProductRow);

                string smsText = string.Format("תודה שרכשת {0} כרטיסים עבור {1} במחיר כולל של {2} ש''ח", quantity, dtProductRow.Rows[0]["VarName"].ToString(), price);

                BaseMethods.SendSMS(SMSPhoneNumber, smsText, orgSmsPhone, 0, 120, orgID, memberID, -1, 4);
            }
        }
        return BaseMethods.GetIVROrderOnLineAnswer(ans, databaseName, paymentID);
    }

    private static string GetTotalSum(string price, string quantity)
    {
        double sum = Convert.ToDouble(price);
        double quant = Convert.ToDouble(quantity);
        double totalSum = sum * quant;
        return totalSum.ToString();
    }

    /// </summary>
    /// <param name="chargeResult"></param>
    /// <param name="confirmationNumber"></param>
    /// <param name="organizationID"></param>
    /// <param name="amount">total sum to charge</param>
    /// <param name="cardNumber">credit card number</param>
    /// <param name="CVV"></param>
    /// <param name="creditMemberID">card owner id</param>
    /// <param name="expired">expiration string in mm-yy format </param>
    /// <param name="transactionType">Debit Or Credit</param>
    /// <returns></returns>

    public static bool ChargeCreditCard(out string chargeResult, out string confirmationNumber, string organizationID, string amount, int payments,
        string cardNumber, string CVV, string creditMemberID, string expired, string transactionType)
    {
        string sentXML, resultXML, userName, password, terminalNumber;
        bool chargeCreditCard;
        chargeResult = confirmationNumber = null;

        amount = GetStringAmount(amount);

        if (GetTerminalDetails(out chargeCreditCard, out userName, out password, out terminalNumber, organizationID) == false)
        {
            chargeResult = @"organization: " + organizationID + " do not have terminal or terminal " +
                    "details are missing";
            return false;
        }
        if (chargeCreditCard == false)// test mode - no need to charge the credit card
            chargeResult = "000";
        else
        {
            sentXML = GetCreditGurdString(terminalNumber, cardNumber, CVV, expired, amount, creditMemberID, payments, transactionType);
            RelayService credit = new RelayService();

            resultXML = credit.ashraitTransaction(userName, password, sentXML); // calling WS function
            chargeResult = AnalyzeResult(resultXML, out confirmationNumber);    // analyze charge result
        }
        if (chargeResult != "000")// charge failed
        {
            chargeResult = GetErrorMessage(chargeResult);
            return false;
        }
        else
        {
            return true;
        }
    }

    private static bool GetTerminalDetails(out bool chargeCreditCard, out string userName, out string password, out string terminalNumber, string organizationID)
    {
        userName = password = terminalNumber = null;
        chargeCreditCard = false;
        try
        {
            string query = string.Format(@"Select TradeSiteChargeCreditCard, TradeSitePaymentTerminalNumber,PaymentTerminalUserName,
                PaymentTerminalPassword From Organizations Where OrganizationID = {0}", organizationID);
            DataTable paymentTerminalDetails = DataBase.FillDataTable(query, "PaymentTterminalDetails");
            if (!TerminalDetailsExist(paymentTerminalDetails.Rows[0]))
                return false;
            chargeCreditCard = Convert.ToBoolean(paymentTerminalDetails.Rows[0]["TradeSiteChargeCreditCard"]);
            if (chargeCreditCard == false)
                return true;
            terminalNumber = paymentTerminalDetails.Rows[0]["TradeSitePaymentTerminalNumber"].ToString();
            userName = paymentTerminalDetails.Rows[0]["PaymentTerminalUserName"].ToString();
            password = paymentTerminalDetails.Rows[0]["PaymentTerminalPassword"].ToString();
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }

    private static bool TerminalDetailsExist(DataRow paymentDetailsRow)
    {
        try
        {
            if (paymentDetailsRow == null)
                return false;
            if (string.IsNullOrEmpty(paymentDetailsRow["TradeSiteChargeCreditCard"].ToString()))
                return false;
            if (Convert.ToBoolean(paymentDetailsRow["TradeSiteChargeCreditCard"]) == false)
                return true;
            if (string.IsNullOrEmpty(paymentDetailsRow["TradeSitePaymentTerminalNumber"].ToString()))
                return false;
            if (string.IsNullOrEmpty(paymentDetailsRow["PaymentTerminalUserName"].ToString()))
                return false;
            if (string.IsNullOrEmpty(paymentDetailsRow["PaymentTerminalPassword"].ToString()))
                return false;
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }
    private static string GetStringAmount(string amount)
    {
        double sum = Convert.ToDouble(amount);
        sum = Math.Round(sum, 2);
        sum *= 100;
        amount = sum.ToString().TrimEnd('.');
        return amount;
    }

    public static string GetCreditGurdString(string PaymentTerminal, string cardNumber, string cvv, string expiration,
                    string amount, string id, int installments, string transactionType)
    {
        StringBuilder CreditGurdString = new StringBuilder();

        //mandatory opening tags 
        CreditGurdString.Append("<ashrait>");
        CreditGurdString.Append("<request> ");

        CreditGurdString.Append("<command>doDeal</command> ");
        CreditGurdString.Append("<requestId/>");
        CreditGurdString.Append("<dateTime></dateTime>");
        CreditGurdString.Append("<version>1001</version> ");
        CreditGurdString.Append("<language>Eng</language> ");
        CreditGurdString.Append("<doDeal>  ");
        CreditGurdString.Append("<terminalNumber>" + PaymentTerminal.Trim() + "</terminalNumber> ");
        CreditGurdString.Append("<cardNo>" + cardNumber + "</cardNo> ");
        CreditGurdString.Append("<cvv>" + cvv + "</cvv> ");

        CreditGurdString.Append("<cardExpiration>" + expiration + "</cardExpiration> ");
        CreditGurdString.Append("<id>" + id + "</id> ");//תעודת זהות
        CreditGurdString.Append("<transactionType>" + transactionType + "</transactionType> ");

        if (installments == 1)
            CreditGurdString.Append("<creditType>RegularCredit</creditType> ");
        else
        {
            int firstPayment, periodicalPayment;
            GetInstallmentsString(amount, installments, out firstPayment, out periodicalPayment);

            CreditGurdString.Append("<creditType>Payments</creditType>");
            CreditGurdString.Append("<firstPayment>" + firstPayment.ToString() + "</firstPayment> ");
            CreditGurdString.Append("<periodicalPayment>" + periodicalPayment.ToString() + "</periodicalPayment> ");
            CreditGurdString.Append("<numberOfPayments>" + (installments - 1).ToString() + "</numberOfPayments> ");
        }

        CreditGurdString.Append("<currency>ILS</currency> ");
        CreditGurdString.Append("<transactionCode>Phone</transactionCode> ");
        CreditGurdString.Append("<total>" + amount + "</total> ");
        CreditGurdString.Append("<validation>AutoComm</validation> ");
        CreditGurdString.Append("<user>forall</user> ");
        CreditGurdString.Append("</doDeal> ");

        //mandatory closing tags 
        CreditGurdString.Append("</request> ");
        CreditGurdString.Append("</ashrait>");

        return CreditGurdString.ToString();
    }

    public static void GetInstallmentsString(string amount, int installments, out int firstPayment, out int periodicalPayment)
    {
        int intAmount = Convert.ToInt32(amount) / 100;
        firstPayment = intAmount / installments + intAmount % installments;
        periodicalPayment = (intAmount - firstPayment) / (installments - 1);

        firstPayment *= 100;
        periodicalPayment *= 100;
    }


    public static string AnalyzeResult(string creditGuardResult, out string confirmationNumber)
    {
        string result = "";
        confirmationNumber = null;
        XmlDocument doc = new XmlDocument();
        try
        {
            doc.LoadXml(creditGuardResult);
            result = doc.DocumentElement["response"].GetElementsByTagName("result")[0].InnerText;
            confirmationNumber = doc.DocumentElement["response"].GetElementsByTagName("tranId")[0].InnerText;
        }
        catch (Exception)
        {
            return "Error";
        }

        return result;
    }

    public static string GetErrorMessage(string chargeResult)
    {
        switch (chargeResult)
        {
            case "-1":
                return "לא ניתן לבצע תשלום שם המשתמש או הסיסמא לביצוע הסליקה חסרים";
            case "003":
                return " (003)התקשר לחברת האשראי";
            case "004":
                return "סירוב מחברת האשראי";
            case "006":
                return "מספר תעודת זהות או CVV שגויים";
            case "033":
                return "מספר הכרטיס אינו תקין";
            case "034":
                return "אין אישור לסוג עסקה או לסוג כרטיס זה במסוף זה";
            case "036":
                return "כרטיס פג תוקף";
            case "038":
                return "סכום העיסקה גדול מתקרה לכרטיס";
            case "039":
                return "מספר כרטיס אשראי לא תקין";
            case "405":
                return " (405)נא לפנות למנהל המערכת ולמסור את קוד התשובה";
            default:
                return " החיוב נכשל אנא בדוק שכל השדות נכונים ונסה שנית. קוד שגיאה " + chargeResult;
        }
    }

    public static void LogGeneral_Add__IVR_BuyingProcess(string subTypeID, string memberID, string organizationID,
        string IP, string URL, string resultCodeID, string strDetails, string xmlDetails)
    {
        try
        {
            Dictionary<string, string> LogGeneralStrFields = new Dictionary<string, string>();
            LogGeneralStrFields.Add("fEnvID", "1");
            LogGeneralStrFields.Add("fUserTypeID", "2");
            LogGeneralStrFields.Add("fTypeID", "3");
            LogGeneralStrFields.Add("MemberId", memberID);
            LogGeneralStrFields.Add("OrganizationID", organizationID);
            LogGeneralStrFields.Add("fTypeSubID", subTypeID);
            LogGeneralStrFields.Add("ResultsCode", resultCodeID);
            LogGeneralStrFields.Add("Ip", IP);
            LogGeneralStrFields.Add("Url", URL);
            LogGeneralStrFields.Add("strDetails", strDetails);
            if (xmlDetails != null)
                LogGeneralStrFields.Add("xmlDetails", xmlDetails);
            string sReturn = LogGeneral.LogGeneral_Add(LogGeneralStrFields);
            if ((sReturn + "     ").Substring(0, 6) == "ERROR:")
            {
                //                Mail.SendMail("LogGeneral_Add__MemberBuyingProcess", sReturn, "helpdesk@dts-4u.com");
            }
        }
        catch (Exception ex)
        {
            //          Mail.SendMail("LogGeneral_Add__MemberBuyingProcess", ex.Message, "helpdesk@dts-4u.com");
        }
    }

    public static string WSClubs_AddMember(string organizationID, string organizationPassword, string memberID, string fullName, string mobilePhone, string companyID, string metaData, string ipUserHost)
    {
        #region Check Values
        if (string.IsNullOrEmpty(organizationID))
            return BaseMethods.GetWSClubsAnswer("22", memberID);
        else
            organizationID = organizationID.Trim();

        if (string.IsNullOrEmpty(organizationPassword))
            return BaseMethods.GetWSClubsAnswer("10", memberID);
        else
            organizationPassword = organizationPassword.Trim();

        if (string.IsNullOrEmpty(memberID))
            return BaseMethods.GetWSClubsAnswer("5", memberID);
        else
            memberID = memberID.Trim();

        if (string.IsNullOrEmpty(companyID) ||
            string.IsNullOrEmpty(mobilePhone) ||
            string.IsNullOrEmpty(fullName))
            return BaseMethods.GetWSClubsAnswer("34", memberID);
        else
        {
            mobilePhone = mobilePhone.Trim();
            companyID = companyID.Trim();
            fullName = fullName.Trim();
            if (!BaseMethods.IsDigit(companyID))
                return BaseMethods.GetWSClubsAnswer("34", memberID);
        }

        if (!string.IsNullOrEmpty(metaData))
            metaData = metaData.Trim();
        #endregion
        bool encriptionRequired = false;
        string back = "";
        string dbName = BaseMethods.CheckValidUserGetDBName(ref organizationID, organizationPassword, ipUserHost, ref memberID, ref encriptionRequired);
        int dbNameParse = 0;
        if (int.TryParse(dbName, out dbNameParse))
            return BaseMethods.GetWSClubsAnswer(dbNameParse.ToString(), memberID);
        back = SqlMethods.ThereIsMemberID(dbName, memberID);
        if (back == "19" || back == "1")//ID Not Exists OR Connection Lost
        {
            if (back == "1")
                back = "57";
            return BaseMethods.GetWSClubsAnswer(back, memberID); ;
        }
        else
        {
            int ans = SqlMethods.AddMember(dbName, memberID, fullName, mobilePhone, companyID);
            if (ans > 0)
                return BaseMethods.GetWSClubsAnswer("29", memberID);
            else
                return BaseMethods.GetWSClubsAnswer("19", memberID);
        }
    }


    public static string WSClubs_DebitCreditCard(string organizationID, string organizationPassword, string memberID,
        string creditCardNumber, string creditCardID, string creditCardValidity, string creditCardCVV, string amount,
        string payments, string metaData, string ipUserHost)
    {

        int sumPayments = 0;
        #region Check Values

        if (string.IsNullOrEmpty(organizationID))
            return BaseMethods.GetWSClubsAnswer("22", memberID);
        else
            organizationID = organizationID.Trim();

        if (string.IsNullOrEmpty(organizationPassword))
            return BaseMethods.GetWSClubsAnswer("10", memberID);
        else
            organizationPassword = organizationPassword.Trim();

        if (string.IsNullOrEmpty(memberID))
            return BaseMethods.GetWSClubsAnswer("5", memberID);
        else
            memberID = memberID.Trim();

        float fAmount = 0;
        if (string.IsNullOrEmpty(amount)
            || !float.TryParse(amount, out fAmount)
            || fAmount == 0)
            return BaseMethods.GetWSClubsAnswer("33", memberID);

        if (string.IsNullOrEmpty(creditCardNumber))
            return BaseMethods.GetWSClubsAnswer("31", memberID);
        else
        {
            creditCardNumber = creditCardNumber.Trim();
            if (!BaseMethods.IsDigit(creditCardNumber))
                return BaseMethods.GetWSClubsAnswer("31", memberID);
        }

        if (string.IsNullOrEmpty(creditCardID) ||
            string.IsNullOrEmpty(creditCardValidity) ||
            string.IsNullOrEmpty(creditCardCVV))
            return BaseMethods.GetWSClubsAnswer("62", memberID);
        else
        {
            creditCardID = creditCardID.Trim();
            creditCardValidity = creditCardValidity.Trim();
            creditCardCVV = creditCardCVV.Trim();
            if (!BaseMethods.IsDigit(creditCardID)
                || !BaseMethods.IsDigit(creditCardValidity)
                || !BaseMethods.IsDigit(creditCardCVV)
                || !int.TryParse(payments, out sumPayments))
                return BaseMethods.GetWSClubsAnswer("63", memberID);
            if (creditCardValidity.Length != 4)
                return BaseMethods.GetWSClubsAnswer("63", memberID);
        }

        if (!string.IsNullOrEmpty(metaData))
            metaData = metaData.Trim();
        #endregion
        bool encriptionRequired = false;

        string dbName = BaseMethods.CheckValidUserGetDBName(ref organizationID, organizationPassword, ipUserHost, ref memberID, ref encriptionRequired);

        int dbNameParse = 0;
        if (int.TryParse(dbName, out dbNameParse))
            return BaseMethods.GetWSClubsAnswer(dbNameParse.ToString(), memberID);

        //לבדוק מה קורה אם לא הצלחנו לשייך כרטיס
        string back = SqlMethods.ThereIsCardForMemberID(dbName, memberID, creditCardNumber.Substring(creditCardNumber.Length - 4, 4));
        if (back == "2")
        {
            int ins = SqlMethods.AddCardForMemberID(dbName, memberID, creditCardNumber);
            if (ins > 0 && encriptionRequired)
                SqlMethods.EncryptCardNumber(dbName, creditCardNumber, "Cards");
        }

        bool takeCache = true;
        bool.TryParse(ConfigurationSettings.AppSettings["TakeCache"], out takeCache);
        string chargeResult = "", confirmationNumber = "";

        if (takeCache)
        {
            bool chargeCreditCard = ChargeCreditCard(out chargeResult, out confirmationNumber, organizationID,
                amount, sumPayments, creditCardNumber, creditCardCVV, creditCardID, creditCardValidity, "Debit");
            if (!chargeCreditCard)
                return BaseMethods.GetWSClubsAnswer("61", memberID, chargeResult);
        }
        string payemetId = Payments.SavePaymentDetails(creditCardNumber, creditCardID, amount, confirmationNumber, dbName, memberID, 0, 0, 0, 0, "", "", 0);
        if (!string.IsNullOrEmpty(payemetId))
        {
            //return BaseMethods.GetWSClubsAnswer(payemetId, memberID, chargeResult + " ,confirmationNumber: " + confirmationNumber);
            return BaseMethods.GetWSClubsAnswer(payemetId, memberID, confirmationNumber);
        }
        else
        {
            //להתריע במייל שבוצע חיוב ללא הכנסה לטבלת התשלומים
            return BaseMethods.GetWSClubsAnswer("61", memberID, chargeResult);
        }
    }

    public static string InsertNewOrderClubs(int? PremiumType, string organizationId, string organizationPassword, string memberID, string productID,
        string quantity, string originalOrderID, string debitAsmacta, string catalogicPrice, string IrgunPrice, string customerPrice,
        string metaData, string ipUserHost)
    {
        double d_catalogicPrice = 0;
        double d_IrgunPrice = 0;
        double d_customerPrice = 0;

        if (!double.TryParse(catalogicPrice, out d_catalogicPrice)
            || !double.TryParse(IrgunPrice, out d_IrgunPrice)
            || !double.TryParse(customerPrice, out d_customerPrice))
            return BaseMethods.GetWSClubsAnswer("13", memberID);

        string last4Digits = string.Empty, charged = string.Empty, cardOwnerID = string.Empty;
        bool encriptionRequired = false;

        string orgId = organizationId;
        string dbName = BaseMethods.CheckValidUserGetDBName(ref orgId, organizationPassword, ipUserHost, ref memberID,
                            ref encriptionRequired);
        int dbNameParse = 0;
        if (int.TryParse(dbName, out dbNameParse))
            return BaseMethods.GetWSClubsAnswer(dbNameParse.ToString(), memberID);

        SqlMethods.IsPaymentIDExsits(dbName, debitAsmacta, ref last4Digits, ref charged, ref cardOwnerID, memberID);
        if (string.IsNullOrEmpty(charged))
            return BaseMethods.GetWSClubsAnswer("64", memberID);

        metaData = string.Format(@"<Details><SlikaSapakDebit>קרדיט גארד</SlikaSapakDebit><Source>API</Source><CC>{0}</CC><Amount>{1}</Amount><Phone></Phone>
                        <GC></GC><SMSPhoneNumber></SMSPhoneNumber><OrderMail></OrderMail><OrderName></OrderName><OrderTZ>{2}</OrderTZ>
                        <Price>{3}</Price><catalogicPrice>{4}</catalogicPrice><OrganizationPrice>{5}</OrganizationPrice><Comission></Comission>
                        <Discount></Discount><IrgunPriceFormula></IrgunPriceFormula><VarPriceFormula></VarPriceFormula>
                        <VarDiscountFormula></VarDiscountFormula><VarComissionFormula></VarComissionFormula></Details>"
            , last4Digits, charged, cardOwnerID, d_customerPrice, d_catalogicPrice, d_IrgunPrice);
        string ans = InsertNewOrder(PremiumType, organizationId, organizationPassword, productID, memberID, quantity, originalOrderID, metaData, "0", ipUserHost, debitAsmacta);
        return BaseMethods.GetWSClubsAnswer(ans, memberID);
    }

    private static Int64 VlcCancelOrder(Int64 valueCardTID)
    {
        long returnCodeValueCard = -1;
        //Reference to wcf value card
        try
        {
            ValueCardClient orderClient = new ValueCardClient();
            //Insert to ValueCardTranLog table in DTS_Online DB
            returnCodeValueCard = orderClient.VlcCancelOrder(valueCardTID);
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("VlcCancelOrder -> \n valueCardTID -> " + valueCardTID
                + "\n Exception Type -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message
                + "\n ex.StackTrace -> " + ex.StackTrace
                + "\n ex.TargetSite -> " + ex.TargetSite);
        }
        return returnCodeValueCard;
    }

    private static Int64 AddValueCardOrder(VlcOrderData orderData, string dbName, long atractionIdentity)
    {
        long valueCardTranID = 0;
        //Reference to wcf value card
        try
        {
            ValueCardClient orderClient = new ValueCardClient();
            //Insert to ValueCardTranLog table in DTS_Online DB
            valueCardTranID = orderClient.VlcInsertOrder(orderData);
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("AddValueCardOrder -> \n OrganizationID -> " + orderData.OrganizationID
                + "\n dbName -> " + dbName
                + "\n MemberID -> " + orderData.MemberID
                + "\n ProductID -> " + orderData.PromoID
                + "\n Quantity -> " + orderData.QuantityOrdered
                + "\n atractionIdentity-> " + atractionIdentity.ToString()
                + "\n Exception Type -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message
                + "\n ex.StackTrace -> " + ex.StackTrace
                + "\n ex.TargetSite -> " + ex.TargetSite);
        }
        if (valueCardTranID == 0)//Can't insert order to WCF value card
        {
            //Insert the order to ValueCardTranLog
            //App ValueCard_OrdersSender hendel with the order
            valueCardTranID = SqlMethods.InsertToValueCardTranLog(orderData);
        }
        if (valueCardTranID > 0)//Match ValueCardTranLog To ATRACTIONSOrders in ValueCardTL_ATROrders Table
            SqlMethods.AddToValueCardTL_ATROrders(dbName, valueCardTranID, atractionIdentity);
        return valueCardTranID;
    }

    public static Int64 VlcChangeCardNumber(Int64 valueCardTID, string newCardNumber)
    {
        long valueCardErrorCode = 0;
        try
        {
            ValueCardClient orderClient = new ValueCardClient();
            //Change card number for orders that not implemented
            valueCardErrorCode = orderClient.VlcChangeCardNumber(valueCardTID, newCardNumber);
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("VlcChangeCardNumber -> \n valueCardTID-> " + valueCardTID
                + "\n Exception Type -> " + ex.GetType()
                + "\n Exception Message -> " + ex.Message
                + "\n ex.StackTrace -> " + ex.StackTrace
                + "\n ex.TargetSite -> " + ex.TargetSite);
        }

        return valueCardErrorCode;
    }


    /*
     
1-	הזדהות נכשלה

0	אסמכתא לא נמצאה

1	מומש

2	לא מומש


     */

    public static string CheckOrderImplementaion(string orgId, string orgPassword, int asmachtaId)
    {
        var finalResult = "-1";
        var query = "SELECT DBName FROM DTS_Online..Organizations WHERE OrganizationID=" + orgId + " AND Password='" + orgPassword + "'";
        var dbName = DataBase.ExecuteScalar(query);

        if (dbName != null)
        {

            var queryAsmachtaExist = "SELECT TOP 1 MemberId FROM " + dbName.ToString() + "..ATRACTIONSOrders WHERE MemberOrderAsmchta=" + asmachtaId;
            var result1 = DataBase.ExecuteScalar(queryAsmachtaExist);

            if (result1 != null)
            {

                string orderType = DataBase.ExecuteScalar(
                    $"select [Type] from { dbName }..productsVars where FullBarCode='{result1}'").ToString().Trim();
                if (orderType == "ANA")
                    orderType = "TZIMERS";
                string orderAsmchta = "OrderAsmchta";
                if (orderType == "ATRACTIONS")
                    orderAsmchta = "MemberOrderAsmchta";
                string orderQuantity = "OrderQuntity";
                string orderBalance = "OrderBlance";
                if (orderType == "ATRACTIONS")
                {
                    orderQuantity = "MemberOrderQuntity";
                    orderBalance = "MemberOrderBlance";
                }





                var queryMimush =
               $"SELECT MemberId FROM {dbName}..{orderType}Orders " +
               $"WHERE {orderAsmchta}=(SELECT TOP 1 TTransactionOrder FROM { dbName }..WebServiceTransaction WHERE TTransactionID={asmachtaId}) " +
                $"AND {orderBalance} = {orderQuantity}";


                //var queryMimush =
                //$"SELECT MemberId FROM {dbName}..{orderType}Orders WHERE MemberOrderAsmchta={asmachtaId} AND {orderAsmchta} = " +
                //$"(select TTransactionOrder from productsVars where FullBarCode='{queryAsmachtaExist}')";
                var result2 = DataBase.ExecuteScalar(queryMimush);
                finalResult = result2 != null ? "1" : "2";
            }
            else
            {
                finalResult = "0";
            }

        }

        return finalResult;
    }

    public static string GetCouponCode(string memberId, int couponStockId)
    {

        return
            DataBase.ExecuteScalar(
                    $"select CouponCode from dts_online..CouponsStock where MemberID='{memberId}' and StockID={couponStockId} order by SendingTime desc")
                .ToString();
    }
    static object locker = new object();

    public static string getCoupon(string memberID, string StockiD)
    {
        List<System.Data.SqlClient.SqlParameter> param = new List<System.Data.SqlClient.SqlParameter>();

        param.Add(DataBase.GetStringParam("@memberID", SqlDbType.NVarChar, memberID));
        param.Add(DataBase.GetStringParam("@cardNumber", SqlDbType.NVarChar, string.Empty));
        param.Add(DataBase.GetStringParam("@phoneNumber", SqlDbType.NVarChar, string.Empty));
        param.Add(DataBase.GetStringParam("@StockID", SqlDbType.NVarChar, StockiD));

        string CouponCode;

        CouponCode = DataBase.ScalarCommandFromProcedure("GetCouponFromStock", param, "DTS_Online");

        return CouponCode;
    }
}


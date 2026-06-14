using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web.Configuration;
using OrderDll.StockManage;
using Service = System.Web.Services.Description.Service;

public class LimitLogic
{
    //ISCampaign
    /// <summary>
    /// row.Table.Namespace is used to pass through special filters for SQL
    /// </summary>
    public static void CheckGeneralLimitsLogic(DataRow row, ref string[] error, string memberID, string dataBaseName,
        int sumVarNumber, string subType, string productName, string orgID)
    {
        bool addMember = false;//Add memberID to filter
        int limitInt, sumOrders;
        string filter = "";
        object limitObj;
        string[] coloumnsName = new string[] { "ProductGeneralLimitFormula", "ProductDailyLimitFormula", "ProductWeeklyLimitFormula", "ProductMonthlyLimitFormula", "ProductQuarterLimitFormula", "ProductYearlyLimitFormula", 
                                                        "MemberGeneralLimitFormula", "MemberDailyLimitFormula", "MemberWeeklyLimitFormula", "MemberMonthlyLimitFormula", "MemberQuarterLimitFormula", "MemberYearlyLimitFormula" };

        if (!string.IsNullOrEmpty(productName))
            productName = productName.Trim();

        //row.Table.Namespace is used to pass through special filters
        string specialFilter = string.IsNullOrEmpty(row.Table.Namespace) ? dataBaseName : row.Table.Namespace;

        for (int countLimits = 0; countLimits < coloumnsName.Length; countLimits++)
        {
            if (error != null)
                return;

                limitObj = null;
                limitObj = row[coloumnsName[countLimits]];//Check all columns limit
                if (limitObj == DBNull.Value || !int.TryParse(limitObj.ToString(), out limitInt))
                    continue;

                filter = "";//Member ID filter
                if (countLimits != 0 && countLimits != 6)//Generally limit without filter
                    filter = BuildFilter(countLimits, specialFilter);

                if (!addMember && countLimits > 5)
                    addMember = true;

                sumOrders = 0;
                //if (sumVarNumber > limitInt)//Check if order bigger the limit
                //{
                //    if (string.IsNullOrEmpty(subType) || subType == "-1")//Get sum of orders from WebServiceTransaction
                //        sumOrders = GetSumOfOrders(row["FullBarCode"].ToString(), memberID, dataBaseName, filter, addMember);
                //    else //if (row["ISCampaign"] != DBNull.Value && !Convert.ToBoolean(row["ISCampaign"]))
                //        sumOrders = GetSumOfOrdersSubType(memberID, dataBaseName, subType, filter, addMember);
                //    BuildError(ref error, sumOrders, sumVarNumber, limitInt, countLimits, subType, orgID, productName);
                //}

                //if (error != null)
                //    return;

                if (string.IsNullOrEmpty(subType) || subType == "-1")//Get sum of orders from WebServiceTransaction
                    sumOrders = GetSumOfOrders(row["FullBarCode"].ToString(), memberID, dataBaseName, filter, addMember);
                else //if (row["ISCampaign"] != DBNull.Value && !Convert.ToBoolean(row["ISCampaign"]))
                    sumOrders = GetSumOfOrdersSubType(memberID, dataBaseName, subType, filter, addMember);

                BuildError(ref error, sumOrders, sumVarNumber, limitInt, countLimits, subType, orgID, productName);
        }
    }

    /// <summary>
    /// OverLoad for wallet WS - gets a different Type for parameter row but the code is the same
    /// </summary>
    /// <param name="row">key specialFilter is used for special Filter ,key FullBarCode is used for Full BarCode </param>
    /// <param name="error"></param>
    /// <param name="memberID"></param>
    /// <param name="dataBaseName"></param>
    /// <param name="sumVarNumber"></param>
    /// <param name="subType"></param>
    /// <param name="productName"></param>
    /// <param name="orgID"></param>
    public static void CheckGeneralLimitsLogic(Dictionary<string,object> row, ref string[] error, string memberID, string dataBaseName,
       int sumVarNumber, string subType, string productName, string orgID)
    {
        bool addMember = false;//Add memberID to filter
        int limitInt, sumOrders;
        string filter = "";
        object limitObj;
        string[] coloumnsName = new string[] { "ProductGeneralLimitFormula", "ProductDailyLimitFormula", "ProductWeeklyLimitFormula", "ProductMonthlyLimitFormula", "ProductQuarterLimitFormula", "ProductYearlyLimitFormula", 
                                                        "MemberGeneralLimitFormula", "MemberDailyLimitFormula", "MemberWeeklyLimitFormula", "MemberMonthlyLimitFormula", "MemberQuarterLimitFormula", "MemberYearlyLimitFormula" };

        if (!string.IsNullOrEmpty(productName))
            productName = productName.Trim();

        string specialFilter = row.ContainsKey("specialFilter") ? row["specialFilter"].ToString() : dataBaseName;

        for (int countLimits = 0; countLimits < coloumnsName.Length; countLimits++)
        {
            if (error != null)
                return;

            limitObj = null;
            limitObj = row[coloumnsName[countLimits]];//Check all columns limit
            if (limitObj == DBNull.Value || !int.TryParse(limitObj.ToString(), out limitInt))
                continue;

            filter = "";//Member ID filter
            if (countLimits != 0 && countLimits != 6)//Generally limit without filter
                filter = BuildFilter(countLimits, dataBaseName);

            if (!addMember && countLimits > 5)
                addMember = true;

            sumOrders = 0;

            if (string.IsNullOrEmpty(subType) || subType == "-1")
                sumOrders = GetSumOfOrders(row["FullBarCode"].ToString(), memberID, dataBaseName, filter, addMember);
            else 
                sumOrders = GetSumOfOrdersSubType(memberID, dataBaseName, subType, filter, addMember);

            BuildError(ref error, sumOrders, sumVarNumber, limitInt, countLimits, subType, orgID, productName);
        }
    }

    /// <summary>
    /// check for group limitations
    /// </summary>
    /// <param name="error"></param>
    /// <param name="GroupLimitsID"></param>
    /// <param name="memberID"></param>
    /// <param name="databaseName"></param>
    /// <param name="sum"></param>
    /// <param name="orgID"></param>
    public static void checkGroupYearLimits(ref string error, int GroupLimitsID, string memberID, string databaseName, int sum, string orgID)
    {
        int monthOfYearBegining = 13;
        if (databaseName == "Miluim" || databaseName == "HonorReleases")//תיקון hard coded לפי דרישה
            monthOfYearBegining = 5;

        int groupOrders = GetSumOfOrdersForGroupByYear(GroupLimitsID.ToString(), memberID, databaseName, monthOfYearBegining);

        DataTable Limits = getGroupInfo(GroupLimitsID, databaseName);

        if (Limits != null && Limits.Rows.Count > 0)
        {
            DataRow dt = Limits.Rows[0];
            if (dt["YearLimit"] != null)
            {
                if (Convert.ToInt32(dt["YearLimit"]) - groupOrders <= 0)
                {
                    error = GetErrorByKey("71", orgID);//GroupLimitsMaxedOut
                    error = error.Replace("{3}", "שנה");
                    error = error.Replace("{1}", dt["YearLimit"].ToString());
                    return;
                }

                if (sum > Convert.ToInt32(dt["YearLimit"]) - groupOrders)
                {
                    int left = Convert.ToInt32(dt["YearLimit"]) - groupOrders;
                    //error = GetErrorByKey("74", orgID);//GroupLimitsTooMany
                    error = GetErrorByKey("72", orgID);//GroupLimitsTooMany
                    error = error.Replace("{4}", left.ToString());
                    return;
                }
            }
        }
    }

    private static string GetError(string errorContext, string orgID)
    {
        object obj = null;
        try
        {
            obj = DataBase.ExecuteScalar(string.Format(@"IF EXISTS(Select MessageText From Messages Where MessageContext = '{1}' And OrganizationID = '{0}')
                                                         Select MessageText From Messages Where MessageContext = '{1}' And OrganizationID = '{0}'
                                                         else Select MessageText From Messages Where MessageContext = '{1}'", orgID, errorContext));
            if (obj != null && obj != DBNull.Value)
                return Convert.ToString(obj);
        }
        catch (Exception ex)
        {
            
        }
        return null;
    }

    private static string GetErrorByKey(string MessageKey, string orgID)
    {
        object obj = null;
        try
        {
            string query = string.Format(@"if exists(Select '1' 
		                                            From Messages 
		                                            Where MessageKey = '{1}' 
		                                            And OrganizationID = '{0}')
                                            Select MessageText From Messages Where MessageKey = '{1}' And OrganizationID = '{0}'
                                            else
                                            Select MessageText From Messages Where MessageKey = '{1}' And OrganizationID is null", orgID, MessageKey);


            obj = DataBase.ExecuteScalar(query);
            if (obj != null && obj != DBNull.Value)
                return Convert.ToString(obj);
        }
        catch (Exception)
        { }
        return null;
    }
    /// <summary>
    ///Check quantiny & Build error text
    /// </summary>
    private static void BuildError(ref string[] error, int sumOrders, int sumVarNumber, int limitInt, int countLimits, string subType, string orgID, string productName)
    {
        string errorRow = "";
        string errorStr = "";
        if ((sumOrders + sumVarNumber) > limitInt)
        {
            switch (countLimits)
            {
                case 0:
                case 6:
                    errorRow = "כללית";
                    break;
                case 1:
                case 7:
                    errorRow = "יומית";
                    break;
                case 2:
                case 8:
                    errorRow = "שבועית";
                    break;
                case 3:
                case 9:
                    errorRow = "חודשית";
                    if(subType == "-2")
                        errorRow = "חודש";
                    break;
                case 4:
                case 10:
                    errorRow = "רבעונית";
                    break;
                case 5:
                case 11:
                    errorRow = "שנתית";
                    if (subType == "-2")
                        errorRow = "שנה";
                    break;
            }
            int pastTheLimit = limitInt - (sumOrders + sumVarNumber);//בדיקה האם עברו את המגבלה לחבר/ארגון בקטגוריה/מוצר
            if (string.IsNullOrEmpty(subType))
            {

                if (countLimits > 5)//מגבלת חבר
                {
                    if (countLimits == 8)
                    {
                        if ((limitInt - sumOrders) > 0)
                            errorStr = GetError("OrderLimitMemberWeekly", orgID);//חריגה מהכמות המותרת לשבוע
                        else
                            errorStr = GetError("OrderLimitMemberWeeklyNoCards", orgID);//לא נותרו במלאי לשבוע כרטיסים
                    }
                    else
                    {
                        if ((limitInt - sumOrders) > 0)
                            errorStr = GetError("OrderLimitMember", orgID);
                        else
                            errorStr = GetError("OrderLimitMemberNoCards", orgID);
                    }
                }
                else//מגבלת מוצר
                {
                    if ((limitInt - sumOrders) > 0)
                        errorStr = GetError("OrderLimitOrg", orgID);
                    else
                        errorStr = GetError("OrderLimitOrgNoCards", orgID);
                }

                //        //error = new string [] {"אזל המלאי ..."};
                //error = new string[]{ "לרשותך " + sumOrders.ToString() + " הזמנות קודמות, ביקשת להזמין " + sumVarNumber + "." ,"סך המגבלה ה" + errorRow + " להזמנה היא: " + limitInt.ToString()};
            }
            else if (subType == "-1")
            {
                if (countLimits > 5)//מגבלת חבר
                {
                    if ((limitInt - sumOrders) > 0)
                        errorStr = GetError("BasketLimitMember", orgID);
                    else
                        errorStr = GetError("BasketLimitMemberNoCards", orgID);
                }
                else//מגבלת מוצר
                {
                    if ((limitInt - sumOrders) > 0)
                        errorStr = GetError("BasketLimitOrg", orgID);
                    else
                        errorStr = GetError("BasketLimitOrgNoCards", orgID);
                }
                //error = new string[] { "לרשותך " + sumOrders.ToString() + " הזמנות קודמות, ביקשת להזמין " + sumVarNumber + ".", "סך המגבלה ה" + errorRow + " להזמנה לארגון במוצר זה היא: " + limitInt.ToString() };
            }
            //else if (subType == "-2")//special group limitations
            //{
            //    if ((limitInt - sumOrders) > 0)
            //        errorStr = errorStr = GetError("GroupLimitsTooMany", orgID);
            //    else
            //        errorStr = errorStr = GetError("GroupLimitsMaxedOut", orgID);
            //}
            else
            {
                if (countLimits > 5)
                {
                    if ((limitInt - sumOrders) > 0)
                        errorStr = GetError("CategoryLimitMember", orgID);
                    else
                        errorStr = GetError("CategoryLimitMemberNoCards", orgID);
                }
                else
                {
                    if ((limitInt - sumOrders) > 0)
                        errorStr = GetError("CategoryLimitOrg", orgID);
                    else
                        errorStr = GetError("CategoryLimitOrgNoCards", orgID);
                }
                //        //error = new string[] { "אזלו הכרטיסים לאירגון" };
            }

            if (!string.IsNullOrEmpty(errorStr))
            {
                for (int i = 0; i < 6; i++)
                {
                    string strPlace = "{" + i + "}";
                    string strReplace = "";
                    if (errorStr.Contains(strPlace))
                    {
                        switch (i)
                        {
                            case 0:
                                strReplace = productName;//{0}שם המוצר
                                break;
                            case 1:
                                strReplace = limitInt.ToString();//{1}סך המגבלה
                                break;
                            case 2:
                                strReplace = sumVarNumber.ToString();//{2} סך כמות שהלקוח מבקש לרכוש - כולל סל קניות
                                break;
                            case 3:
                                strReplace = errorRow;//{3} תיאור תקופתי למגבלה - שנתי/חודשי/יומי 
                                break;
                            case 4:
                                strReplace = (limitInt - sumOrders).ToString();//{4} סך יתרה להזמנה
                                break;
                            case 5:
                                strReplace = sumOrders.ToString();//{5} סך כמות שהוזמנה עד עתה מהמוצר/קטגוריה 
                                break;
                        }
                    }
                    errorStr = errorStr.Replace(strPlace, strReplace);
                }
                error = errorStr.Split(',');
            }
        }
    }

    /// <summary>
    /// Main function to limit shopping
    /// </summary>
    private static string BuildFilter(int limitType,string SpecialFilter)
    {
        switch (limitType)
        {
            case 1://DailyLimit
            case 7:
                return " AND (DATEDIFF(day, GETDATE(), TTransactionDateTime) = 0)";
            case 2://WeeklyLimit
            case 8:
                return " AND (DATEDIFF(week, GETDATE(), TTransactionDateTime) = 0)";
            case 3://MonthlyLimit
            case 9:
                return getSpecialFilterForMonth(SpecialFilter);// " AND (DATEDIFF(month, GETDATE(), TTransactionDateTime) = 0)";
            case 4://QuarterLimit
            case 10:
                return " AND (DATEDIFF(quarter, GETDATE(), TTransactionDateTime) = 0)";
            case 5://YearlyLimit
            case 11:
                return getYearPartForSqlWebServiceTransaction(SpecialFilter);
        }
        return "";
    }

    private static string getSpecialFilterForMonth(string fillter)
    {
        switch (fillter)
        {
            case "FirstBusinessDay":
                return "TTransactionDateTime between dbo.getFirstDayOfMonth(getdate()) and dbo.getFirstDayOfNextMonth(getdate())";
            default:
                return " AND (DATEDIFF(month, GETDATE(), TTransactionDateTime) = 0)";
        }
    }

    private static string getYearPartForSqlWebServiceTransaction(string fillter)
    {
      //  switch (fillter)
      //  {
      //      case "Miluim"://תיקון hard coded לפי דרישה
      //      case "HonorReleases"://תיקון hard coded לפי דרישה
      //          if (DateTime.Now.Month >= 5)
      //              return string.Format("AND TTransactionDateTime between '{0}-05-01 00:00:00' and '{1}-05-01 00:00:00'", DateTime.Now.Year, DateTime.Now.Year + 1);
      //          else
       //             return string.Format("AND TTransactionDateTime between '{0}-05-01 00:00:00' and '{1}-05-01 00:00:00'", DateTime.Now.Year - 1, DateTime.Now.Year);
       //     default: return " AND (DATEDIFF(year, GETDATE(), TTransactionDateTime) = 0)";
       // }


        switch (fillter)
        {
            case "Miluim"://תיקון hard coded לפי דרישה
            case "HonorReleases"://תיקון hard coded לפי דרישה
                return " AND (DATEDIFF(year, GETDATE(), TTransactionDateTime) = 0)";
            default: return " AND (DATEDIFF(year, GETDATE(), TTransactionDateTime) = 0)";
        }



        //if (orgName == "Miluim" || orgName == "HonorReleases")//תיקון hard coded לפי דרישה
        //{
        //    if (DateTime.Now.Month >= 5)
        //        return string.Format("AND TTransactionDateTime between '{0}-05-01 00:00:00' and '{1}-05-01 00:00:00'", DateTime.Now.Year, DateTime.Now.Year + 1);
        //    else
        //        return string.Format("AND TTransactionDateTime between '{0}-05-01 00:00:00' and '{1}-05-01 00:00:00'", DateTime.Now.Year - 1, DateTime.Now.Year);
        //}
        //return " AND (DATEDIFF(year, GETDATE(), TTransactionDateTime) = 0)";
    }



    /// <summary>
    /// For BusinessSubTypeID limits
    /// </summary>
    private static int GetSumOfOrdersSubType(string memberID, string dataBaseName, string sybType, string filter, bool addMember)
    {
        try
        {
            if (addMember)
                filter += " AND (TTransactionMemberID = '" + memberID + "')";

            string query = string.Format(@"SELECT SUM(CAST(TTransactionquantity AS int))
                                            FROM {0}..WebServiceTransaction INNER JOIN
                                            {0}..ProductsVars 
                                            ON {0}..WebServiceTransaction.TTransactionProductID = {0}..ProductsVars.FullBarCode
                                            WHERE (BusinessSubTypeID = '{1}') 
                                            AND (IsNull({0}..WebServiceTransaction.ISCampaign, 0) = 0)" + filter, dataBaseName, sybType);

            object obj = DataBase.ExecuteScalar(query);
            if (obj != DBNull.Value)
                return Convert.ToInt32(obj);
        }
        catch (Exception ex)
        { }
        return 0;
    }

    /// <summary>
    /// For product limit
    /// </summary>
    private static int GetSumOfOrders(string variant, string memberID, string databaseName, string filter, bool addMember)
    {
        try
        {
            if (addMember)
                filter += " AND (TTransactionMemberID = '" + memberID + "')";

            string query = string.Format(@"SELECT SUM(CAST(TTransactionquantity AS int))
                                                FROM {0}..WebServiceTransaction
                                                WHERE (TTransactionProductID = '{1}')" + filter
                                                , databaseName, variant);

            object obj = DataBase.ExecuteScalar(query);
            if (obj != DBNull.Value)
                return Convert.ToInt32(obj);
        }
        catch (Exception)
        {
        }
        return 0;
    }

    /// <summary>
    /// For group of products limit
    /// </summary>
    private static int GetSumOfOrdersForGroupByYear(string GroupLimitsID, string memberID, string databaseName, int monthOfYearBegining)
    {
        //     --  AND (year(TTransactionDateTime) = year(getdate()))
//                                               AND ((year(TTransactionDateTime) = year(getdate()) and month(TTransactionDateTime) < {3})
//                                                OR (year(TTransactionDateTime) = (year(getdate())-1) and month(TTransactionDateTime) >= {3})
//                                                    or ( year(TTransactionDateTime) = (year(getdate())) and   month(TTransactionDateTime) < {3} )"
        try
        {
            //WHERE (GroupLimitsID = '{1}')  AND (TTransactionMemberID = '{2}') AND (DATEDIFF(year, GETDATE(), TTransactionDateTime) = 0)
            string query = string.Format(@"SELECT SUM(CAST(TTransactionquantity AS int))
                                                FROM {0}..WebServiceTransaction
                                                WHERE  (TTransactionMemberID = '{2}') AND (GroupLimitsID = '{1}') "
                                                , databaseName, GroupLimitsID, memberID, monthOfYearBegining);
            query += getYearPartForSqlWebServiceTransaction(databaseName);

            object obj = DataBase.ExecuteScalar(query);
            if (obj != DBNull.Value)
                return Convert.ToInt32(obj);
        }
        catch (Exception)
        {
        }
        return 0;
    }

    private static DataTable GroupLimits;
    private static DataTable GroupLimitsHonorReleases;
    private static DateTime interval = DateTime.Now;
    private static DateTime intervalHonorReleases = DateTime.Now;
    private static DataTable getGroupInfo(int groupID, string databaseName)
    {
        if (GroupLimits == null) GroupLimits = new DataTable();
        if (databaseName == "HonorReleases" && GroupLimitsHonorReleases == null) GroupLimitsHonorReleases = new DataTable();
        try
        {
            if (databaseName == "HonorReleases")
            {
                if (GroupLimitsHonorReleases.Rows.Count == 0 || intervalHonorReleases <= DateTime.Now)
                {
                    string query = string.Format("SELECT [ID],[MonthLimit],[YearLimit] FROM {0}..VariantGroupLimits --WHERE (ID = '{1}')", databaseName, groupID.ToString());
                    GroupLimitsHonorReleases = DataBase.FillDataTable(query, "GroupLimitsInfo");
                    intervalHonorReleases = DateTime.Now.AddMinutes(10);
                }

                DataRow[] dt = GroupLimitsHonorReleases.Select("ID = " + groupID);
                if (dt.Length > 0)
                {
                    DataTable ret = GroupLimitsHonorReleases.Clone();
                    ret.ImportRow(dt[0]);
                    return ret;
                }
                else
                    return null;
            }
            else
            {
                if (GroupLimits.Rows.Count == 0 || interval <= DateTime.Now)
                {
                    string query = string.Format("SELECT [ID],[MonthLimit],[YearLimit] FROM {0}..VariantGroupLimits --WHERE (ID = '{1}')", databaseName, groupID.ToString());
                    GroupLimits = DataBase.FillDataTable(query, "GroupLimitsInfo");
                    interval = DateTime.Now.AddMinutes(10);
                }

                DataRow[] dt = GroupLimits.Select("ID = " + groupID);
                if (dt.Length > 0)
                {
                    DataTable ret = GroupLimits.Clone();
                    ret.ImportRow(dt[0]);
                    return ret;
                }
                else
                    return null;
            }
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail(ex.Message + ex.StackTrace, "getGroupInfo "+ databaseName);
            return null;
        }
    }

    /// <summary>
    /// gets the total of variant in stock minus usage of variant in all organizations
    /// for active stock
    /// </summary>
    /// <param name="ProductID"></param>
    /// <returns></returns>
    public static int getStockForVar(string ProductID,out int StockID)
    {
        StockID = 0;
        string query = string.Format(@"select min(stockQuantity - WSTReservation) as amount,[DTS_OnLine].[dbo].[VariantStock].StockID
                                       from [DTS_OnLine].[dbo].[VariantStock] inner join [DTS_OnLine].[dbo].[VIEW_AllClub_StockUsage]
                                       on [DTS_OnLine].[dbo].[VariantStock].StockID = [VIEW_AllClub_StockUsage].stockID
                                       where [FullBarCode] like  '{0}' 
                                       group by [DTS_OnLine].[dbo].[VariantStock].StockID", ProductID);

        string queryTestExists = string.Format("if exists(SELECT  Id FROM  VariantStock WHERE (FullBarCode = '{0}') AND active = 1) select '1' else select '0'", ProductID);

        

       try
       {
           DataTable dt = new DataTable();
           object obj1 = DataBase.ExecuteScalar(queryTestExists);
           if (obj1.ToString() == "1")
           {
               dt = DataBase.FillDataTable(query,"table");
               StockID = Convert.ToInt32(dt.Rows[0][1]);

               int Sum = GetShopingBasckStockQuentity(ProductID);
               if (dt.Rows[0][0] != DBNull.Value)
                   return Convert.ToInt32(dt.Rows[0][0]) - Sum;

               return 0;
           }
           else return -10000;//no limitations found - can not return null for int
       }
       catch (Exception ex)
       {
           SqlMethods.SendMail("error getting the amount ingetStockForVar()  for variant : " + ProductID, "error reading from DB " + ex.Message + " " + ex.StackTrace);
           return 0;
       }

    }

    private static int GetShopingBasckStockQuentity(string ProductID)
    {
        SqlCommand cmd = new SqlCommand();
        SqlParameter p1 = new SqlParameter("@fullBarCode", SqlDbType.NVarChar);
        p1.Value = ProductID;
        SqlParameter p2 = new SqlParameter("@sum", SqlDbType.Decimal);
        p2.Direction = ParameterDirection.Output;
        cmd.Parameters.Add(p1);
        cmd.Parameters.Add(p2);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "GetShopingBasketStockQuantity";
        DataBase.ExecuteScalar(cmd);
        return int.Parse(p2.Value.ToString());
    }
    public static bool CheckStock(string barcode, string dBname, int orgId, int amoubnt, bool isTrade)
    {
        try
        {
            var v = new VariantStock { FullBarCode = barcode, Amount = amoubnt, IsTrade = isTrade };
            var stockws = new OrderDll.StockManage.Service();
            stockws.Timeout = 1000000;
            stockws.Url = WebConfigurationManager.AppSettings["stockws.Url"] ?? "http://172.29.24.54/Service.asmx";
            var lStocks = new VariantStock[1];
            lStocks[0] = v;
            var lsResult = stockws.GetStockResponse(lStocks, dBname, orgId);
            if (lsResult.Any())
            {
                return lsResult[0].CheckStockSeccsess;
            }
        }
        catch (Exception ex)
        {

        }
        return false;
    }
}
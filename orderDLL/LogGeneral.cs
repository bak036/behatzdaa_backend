using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Xml;
using System.Text;


public class LogGeneral
{
    ////example to create xml
    //XmlDocument x1 = new XmlDocument();
    //x1.LoadXml("<?xml version='1.0' encoding='utf-8'?><root><l id='2' name='222'></l></root>");  
    //string s111 = "";
    //s111 += "<?xml version='1.0' encoding='utf-8'?>";
    //s111 += "<root>";
    //for (int a = 1; a <= 100; a++)
    //    s111 += "<l id='" + a.ToString() + "' name='222gtgtg  gt g'></l>";
    //s111 += "</root>";

    //SELECT xmlDetails.query('/root/l[@id="1"]') FROM LogGeneral 

    //SELECT     LogGeneral_Type.TypeName, LogGeneral_TypeSub.ID, LogGeneral_TypeSub.TypeSubName
    //FROM         LogGeneral_TypeSub RIGHT OUTER JOIN
    //                      LogGeneral_Type ON LogGeneral_TypeSub.fTypeID = LogGeneral_Type.ID
    //for xml auto



    //SELECT     LogGeneral_Type.TypeName, LogGeneral_TypeSub.ID, LogGeneral_TypeSub.TypeSubName
    //FROM         LogGeneral_TypeSub RIGHT OUTER JOIN
    //                      LogGeneral_Type ON LogGeneral_TypeSub.fTypeID = LogGeneral_Type.ID
    //for xml raw

    public static string LogGeneral_Add(Dictionary<string, string> LogGeneralStrFields)
    {
        string sReturn = "";
        string ConStr = "";

        try
        {
            //get connection string
            if (LogGeneralStrFields.ContainsKey("ConStr"))
                ConStr = LogGeneralStrFields["ConStr"];
            else
                ConStr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

            //Data Source
            SqlConnection MyConnection = new SqlConnection(ConStr);

            //Create a DataAdapter, and then provide the name of the stored procedure.
            SqlDataAdapter MyDataAdapter = new SqlDataAdapter("LogGeneral_Add", MyConnection);

            //Set the command type as StoredProcedure.
            MyDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            //EnvID
            MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@fEnvID", SqlDbType.Int, 4));
            if (LogGeneralStrFields.ContainsKey("fEnvID"))
                MyDataAdapter.SelectCommand.Parameters["@fEnvID"].Value = Convert.ToInt32((LogGeneralStrFields["fEnvID"]));
            else
                MyDataAdapter.SelectCommand.Parameters["@fEnvID"].Value = DBNull.Value;

            //fUserTypeID
            MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@fUserTypeID", SqlDbType.Int, 4));
            if (LogGeneralStrFields.ContainsKey("fUserTypeID"))
                MyDataAdapter.SelectCommand.Parameters["@fUserTypeID"].Value = Convert.ToInt32((LogGeneralStrFields["fUserTypeID"]));
            else
                MyDataAdapter.SelectCommand.Parameters["@fUserTypeID"].Value = DBNull.Value;

            //OPId
            MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@OPId", SqlDbType.Int, 4));
            if (LogGeneralStrFields.ContainsKey("OPId"))
                MyDataAdapter.SelectCommand.Parameters["@OPId"].Value = Convert.ToInt32((LogGeneralStrFields["OPId"]));
            else
                MyDataAdapter.SelectCommand.Parameters["@OPId"].Value = DBNull.Value;

            //MemberId
            MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@MemberId", SqlDbType.NChar, 9));
            if (LogGeneralStrFields.ContainsKey("MemberId"))
                MyDataAdapter.SelectCommand.Parameters["@MemberId"].Value = LogGeneralStrFields["MemberId"];
            else
                MyDataAdapter.SelectCommand.Parameters["@MemberId"].Value = DBNull.Value;

            //OrganizationID
            MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@OrganizationID", SqlDbType.Int, 4));
            if (LogGeneralStrFields.ContainsKey("OrganizationID"))
                MyDataAdapter.SelectCommand.Parameters["@OrganizationID"].Value = Convert.ToInt32((LogGeneralStrFields["OrganizationID"]));
            else
                MyDataAdapter.SelectCommand.Parameters["@OrganizationID"].Value = DBNull.Value;

            //fTypeID
            MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@fTypeID", SqlDbType.Int, 4));
            if (LogGeneralStrFields.ContainsKey("fTypeID"))
                MyDataAdapter.SelectCommand.Parameters["@fTypeID"].Value = Convert.ToInt32((LogGeneralStrFields["fTypeID"]));
            else
                MyDataAdapter.SelectCommand.Parameters["@fTypeID"].Value = DBNull.Value;

            //fTypeSubID
            MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@fTypeSubID", SqlDbType.Int, 4));
            if (LogGeneralStrFields.ContainsKey("fTypeSubID"))
                MyDataAdapter.SelectCommand.Parameters["@fTypeSubID"].Value = Convert.ToInt32((LogGeneralStrFields["fTypeSubID"]));
            else
                MyDataAdapter.SelectCommand.Parameters["@fTypeSubID"].Value = DBNull.Value;

            //ResultsCode
            MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ResultsCode", SqlDbType.Int, 4));
            if (LogGeneralStrFields.ContainsKey("ResultsCode"))
                MyDataAdapter.SelectCommand.Parameters["@ResultsCode"].Value = Convert.ToInt32((LogGeneralStrFields["ResultsCode"]));
            else
                MyDataAdapter.SelectCommand.Parameters["@ResultsCode"].Value = DBNull.Value;

            //Ip
            MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Ip", SqlDbType.NVarChar, 50));
            if (LogGeneralStrFields.ContainsKey("Ip"))
                MyDataAdapter.SelectCommand.Parameters["@Ip"].Value = LogGeneralStrFields["Ip"];
            else
                MyDataAdapter.SelectCommand.Parameters["@Ip"].Value = DBNull.Value;

            //Url
            MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Url", SqlDbType.NVarChar, 4000));
            if (LogGeneralStrFields.ContainsKey("Url"))
                MyDataAdapter.SelectCommand.Parameters["@Url"].Value = LogGeneralStrFields["Url"];
            else
                MyDataAdapter.SelectCommand.Parameters["@Url"].Value = DBNull.Value;

            //strDetails
            MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@strDetails", SqlDbType.NVarChar, 999999));
            if (LogGeneralStrFields.ContainsKey("strDetails"))
                MyDataAdapter.SelectCommand.Parameters["@strDetails"].Value = LogGeneralStrFields["strDetails"];
            else
                MyDataAdapter.SelectCommand.Parameters["@strDetails"].Value = DBNull.Value;

            //xmlDetails
            try
            {
                if (LogGeneralStrFields.ContainsKey("xmlDetails")) // default value null in sp
                {
                    MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@xmlDetails", SqlDbType.Xml));
                    MyDataAdapter.SelectCommand.Parameters["@xmlDetails"].Value = new SqlXml(new XmlTextReader(LogGeneralStrFields["xmlDetails"], XmlNodeType.Document, null));
                }
            }
            catch (Exception)
            {}

            //return value
            MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Ret", SqlDbType.Int, 4));
            MyDataAdapter.SelectCommand.Parameters["@Ret"].Value = DBNull.Value;
            MyDataAdapter.SelectCommand.Parameters["@Ret"].Direction = ParameterDirection.Output;

            //Open the connection.
            MyConnection.Open();

            //execute command
            MyDataAdapter.SelectCommand.ExecuteNonQuery();

            int retunvalue = (int)MyDataAdapter.SelectCommand.Parameters["@Ret"].Value;
            sReturn = retunvalue.ToString();

            //Dispose the DataAdapter.
            MyDataAdapter.Dispose();

            //Close the connection.
            MyConnection.Close();

        }
        catch (Exception ex)
        {
            //you can add code here to do something like send email or so

            sReturn = "ERROR: " + ex;
        }

        return sReturn;
    }


    public static void LogGeneral_Add__MemberDebugAction()
    {
        string aa1 = "123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 ";

        aa1 += aa1;
        //aa1 += aa1;
        //aa1 += aa1;
        //aa1 += aa1;
        //aa1 += aa1;
        //aa1 += aa1;
        //aa1 += aa1;
        //aa1 += aa1;
        //aa1 += aa1;
        //aa1 += aa1;
        //aa1 += aa1;
        //aa1 += aa1;
        //aa1 += aa1;

        int a22 = aa1.Length;


        //create dic object for params
        Dictionary<string, string> LogGeneralStrFields = new Dictionary<string, string>();
        //LogGeneralStrFields.Add("ConStr", "..............");
        LogGeneralStrFields.Add("fEnvID", "1");
        LogGeneralStrFields.Add("fUserTypeID", "2");
        LogGeneralStrFields.Add("OPId", "3");
        LogGeneralStrFields.Add("MemberId", "AAA");
        LogGeneralStrFields.Add("OrganizationID", "4");
        LogGeneralStrFields.Add("fTypeID", "1");
        LogGeneralStrFields.Add("fTypeSubID", "1");
        LogGeneralStrFields.Add("ResultsCode", "101");
        LogGeneralStrFields.Add("Ip", "iippp");
        LogGeneralStrFields.Add("Url", "uurll");
        LogGeneralStrFields.Add("strDetails", aa1);
        //LogGeneralStrFields.Add("xmlDetails", "1");

        string sReturn = LogGeneral.LogGeneral_Add(LogGeneralStrFields);

        //if((sReturn+"     ").Substring(0,6) == "ERROR:")
        //{
        //    string ee = "33";
        //}

        //sReturn.IndexOf("111",0,
        //return sReturn;
    }

    public class MemberLogInfo
    {
        public string MemberID { get; set; }
        public string OrganizationID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime EndDate { get; set; }
        
    }

    public static DataTable GetMemberLogInfo(MemberLogInfo memInfo)
    {
        DataTable dt = new DataTable();
        SqlParameter id = new SqlParameter("@MemberId", SqlDbType.NChar);
        id.Value = memInfo.MemberID;
        SqlParameter org = new SqlParameter("@OrganizationID", SqlDbType.Int);
        org.Value = memInfo.OrganizationID;
        SqlParameter start = new SqlParameter("@startDate", SqlDbType.DateTime);
        start.Value = memInfo.startDate;
        SqlParameter end = new SqlParameter("@EndDate", SqlDbType.DateTime);
        end.Value = memInfo.EndDate;

        using (SqlCommand cmd = DataBase.CreateCommand())
        {
            try
            {
                cmd.Parameters.Add(id);
                cmd.Parameters.Add(org);
                cmd.Parameters.Add(start);
                cmd.Parameters.Add(end);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "LogGeneralInfoByMemberId";

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                SqlMethods.SendMail("Error in GetMemberLogInfo OrderDll LogGeneral", ex.Message + "---" + ex.StackTrace);
                return null;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
        }

        return dt;
    }


    public static DataTable GetMemberLogRemarks(string LogID)
    {
        DataTable dt = new DataTable();
        try
        {
            Convert.ToInt32(LogID);//validation
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("validation Error in GetMemberLogRemarks OrderDll LogGeneral", ex.Message + "---" + ex.StackTrace);
            return null;
        }

        try
        {
            StringBuilder query = new StringBuilder();
            query.Append("select [InsertDate] as [תאריך],[Remark] as [הודעה] from LogGeneralRemarks where LogGeneralID = ");
            query.Append(LogID);
            dt = DataBase.FillDataTable(query.ToString(), "LogGeneralRemarks");
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("Error in GetMemberLogRemarks OrderDll LogGeneral", ex.Message + "---" + ex.StackTrace);
            return null;
        }


        return dt;
    }

    public static bool addRemarkToLogRemarkTable(string remark, string logID)
    {
        SqlParameter Remark = new SqlParameter("@Remark", SqlDbType.NChar);
        Remark.Value = remark;
        SqlParameter LogID = new SqlParameter("@LogID", SqlDbType.Int);
        LogID.Value = logID;
        using (SqlCommand cmd = DataBase.CreateCommand())
        {
            try
            {
                cmd.Parameters.Add(Remark);
                cmd.Parameters.Add(LogID);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "addRemarkForLogRemarks";

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                int result = cmd.ExecuteNonQuery();
                if (result != 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                SqlMethods.SendMail("Error in GetMemberLogInfo OrderDll LogGeneral", ex.Message + "---" + ex.StackTrace);
                return false;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
        }
    }
}

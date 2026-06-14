using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Configuration;

/// <summary>
/// Summary description for Class1
/// </summary>
public class DataBase
{
    //static SqlTransaction transaction;
    //static SqlConnection con;
    //static SqlCommand com = new SqlCommand();

    //public static void OpenConnection()
    //{
    //    con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);//ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);
    //    com.CommandType = CommandType.Text;
    //    com.Connection = con;
    //    con.Open();
    //}

    //public static void CloseConnection()
    //{
    //    if (com.Connection != null)
    //        con.Close();
    //}

    //public static void OpenConnectionAgain()
    //{
    //    if (com.Connection.State != ConnectionState.Open)
    //        com.Connection.Open();
    //}
    private static string connectionString;

    static DataBase()
    {
        connectionString = ConfigurationManager.AppSettings["ConStrAll"];//ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);
    }

    private static string GetMessageMail(Exception myExeption, string Query)
    {
        string MailBody = "";
        MailBody += "<strong>Message</strong> <br/>" + myExeption.Message + "<br/>";
        MailBody += "<strong>StackTrace</strong> <br/>" + myExeption.StackTrace + "<br/>";
        MailBody += "<strong>Query</strong> <br/>" + Query + "<br/>";

        return MailBody;
    }

    //public static int ExecuteNonQuery(string query)
    //{
    //    SqlConnection connection = new SqlConnection(connectionString);
    //    SqlCommand commnad = new SqlCommand(query, connection);

    //    try
    //    {
    //        if (commnad.Connection.State != ConnectionState.Open)
    //            commnad.Connection.Open();
    //        return commnad.ExecuteNonQuery();
    //    }
    //    catch (Exception ex) { SqlMethods.SendMail(GetMessageMail(ex,query)); return 0; }
    //    finally
    //    {
    //        if (connection.State != ConnectionState.Closed)
    //            connection.Close();
    //    }
    //}

    public static int ExecuteNonQuery(string query)
    {
        WriteDebugLog(query);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand commnad = new SqlCommand(query, connection);

            try
            {
                if (commnad.Connection.State != ConnectionState.Open)
                    commnad.Connection.Open();
                return commnad.ExecuteNonQuery();
            }
            catch (Exception ex) 
            { 
                SqlMethods.SendMail(GetMessageMail(ex, query));
                return 0; 
            }
        }
    }


    //public static int ExecuteNonQuery(string query, string databaseName)
    //{
    //    SqlConnection connection = new SqlConnection(connectionString);
    //    SqlCommand commnad = new SqlCommand(query, connection);

    //    try
    //    {
    //        if (commnad.Connection.State != ConnectionState.Open)
    //            commnad.Connection.Open();
    //        connection.ChangeDatabase(databaseName);
    //        return commnad.ExecuteNonQuery();
    //    }
    //    catch (Exception ex) { SqlMethods.SendMail(GetMessageMail(ex, query)); return 0; }
    //    finally
    //    {
    //        if (connection.State != ConnectionState.Closed)
    //            connection.Close();
    //    }
    //}

    public static int ExecuteNonQuery(string query, string databaseName)
    {
        WriteDebugLog(query);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand commnad = new SqlCommand(query, connection);
            try
            {
                if (commnad.Connection.State != ConnectionState.Open)
                    commnad.Connection.Open();
                connection.ChangeDatabase(databaseName);
                return commnad.ExecuteNonQuery();
            }
            catch (Exception ex) { SqlMethods.SendMail(GetMessageMail(ex, query)); return 0; }
        }
        
    }

    public static int ExecuteNonQueryForTransaction(string query, SqlCommand commnad)
    {
        WriteDebugLog(query);
        if (commnad.Connection.State != ConnectionState.Open)
            commnad.Connection.Open();
        commnad.CommandText = query;
        return commnad.ExecuteNonQuery();
    }

    public static object ExecuteScalarForTransaction(string query, SqlCommand commnad)
    {
        WriteDebugLog(query);
        if (commnad.Connection.State != ConnectionState.Open)
            commnad.Connection.Open();
        
        commnad.CommandText = query;
        return commnad.ExecuteScalar();
    }

    public static SqlDataReader ExecuteReader_DefaultDataBase(string query)
    {
        WriteDebugLog(query);
        SqlConnection connection = new SqlConnection();
        connection.ConnectionString = connectionString;
        SqlCommand command = new SqlCommand(query, connection);

        if (command.Connection.State != ConnectionState.Open)
            command.Connection.Open();
        return command.ExecuteReader(CommandBehavior.CloseConnection);//connection is closed when reader is closed
    }

    public static SqlDataReader ExecuteReader(string query, SqlConnection connection)
    {
        //The connection must be opened on the calling function
        connection.ConnectionString = connectionString;
        WriteDebugLog(query);
        SqlCommand command = new SqlCommand(query, connection);

        if (command.Connection.State != ConnectionState.Open)
            command.Connection.Open();
        return command.ExecuteReader();
        //The connection must be closed on the calling function
    }
    private static string _writeDebugLog = ConfigurationManager.AppSettings["WriteDebugLog"];

    private static void WriteDebugLog(string msg)
    {
        if(_writeDebugLog=="1")
        DtsLoggger.Logger.Info(msg);
    }
    public static SqlDataReader ExecuteReader(string query, SqlConnection connection,SqlParameter parameter)
    {
        WriteDebugLog(query);
        //The connection must be opened on the calling function
        connection.ConnectionString = connectionString;
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.Add(parameter);
        if (command.Connection.State != ConnectionState.Open)
            command.Connection.Open();
        return command.ExecuteReader();
        //The connection must be closed on the calling function
    }

    public static void BeginTransaction(ref SqlTransaction transaction, SqlCommand command)
    {
        SqlConnection connection = new SqlConnection(connectionString);
        command.Connection = connection;
        if (command.Connection.State != ConnectionState.Open)
            command.Connection.Open();
        transaction = connection.BeginTransaction();
        command.Transaction = transaction;
    }

    public static DataTable FillDataTable(string query, string tableName)
    {
        WriteDebugLog(query);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            DataTable data = new DataTable(tableName);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(query, connection);
            sqlAdapter.Fill(data);
            return data;
        }
        
    }

    public static void ExecuteAdapter(string query, ref DataTable dt)
    {
        WriteDebugLog(query);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlDataAdapter adp = new SqlDataAdapter(query, connection);
            adp.Fill(dt);
        }
        
    }

    public static void ExecuteAdapter(string query, DataTable dt)
    {
        WriteDebugLog(query);
        SqlConnection connection = new SqlConnection(connectionString);
        SqlDataAdapter adp = new SqlDataAdapter(query, connection);
        adp.Fill(dt);
    }
    public static object ExecuteScalar(string query)
    {
        WriteDebugLog(query);
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand commnad = new SqlCommand(query, connection);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return commnad.ExecuteScalar();
            }
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail(GetMessageMail(ex, query));
            return null;
        }
    }

    public static object ExecuteScalar(string query,List<SqlParameter> parameters)
    {
        WriteDebugLog(query);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand commnad = new SqlCommand(query, connection);
            commnad.Parameters.AddRange(parameters.ToArray());
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return commnad.ExecuteScalar();
            }
            finally
            {
                commnad.Parameters.Clear();
            }
        }
        
    }

    public static object ExecuteScalar(SqlCommand commnad)
    {
        WriteDebugLog(commnad.CommandText);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            commnad.Connection = connection;
            if (connection.State != ConnectionState.Open)
                connection.Open();

            return commnad.ExecuteScalar();
        }
    }

    public static SqlCommand CreateCommand()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = new SqlConnection(connectionString);
        return cmd;
    }

    public static object ScalarCommand(SqlCommand cmd)
    {
        try
        {
            if (cmd.Connection.State.ToString() == "Closed")
            {
                cmd.Connection.Open();
            }
            return cmd.ExecuteScalar();
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail(GetMessageMail(ex, cmd.CommandText));
            return null;
        }
        finally
        {
            cmd.Connection.Close();
            cmd.Dispose();
        }
    }


    public static void InsertMailToEmailQueueTable(string fromMail, string ToMail, string subject, string body, string BCC, string CC, int organizationID, int paymentId ,int type)
    {
        SqlCommand commnad = new SqlCommand();
        commnad.CommandType = CommandType.StoredProcedure;
        commnad.CommandText = "InsertOrderMail"; 

        List<SqlParameter> paramsList = new List<SqlParameter>();

        paramsList.Add(GetStringParam("@From", SqlDbType.NVarChar, fromMail));
        paramsList.Add(GetStringParam("@To", SqlDbType.NVarChar, ToMail));
        paramsList.Add(GetStringParam("@subject", SqlDbType.NVarChar, subject));
        paramsList.Add(GetStringParam("@Body", SqlDbType.NVarChar, body));
        paramsList.Add(GetIntParam("@isHTML", SqlDbType.Bit, 1));
      //  paramsList.Add(GetIntParam("@TYPE", SqlDbType.Int, 100000 + organizationID));
        paramsList.Add(GetIntParam("@TYPE", SqlDbType.Int, type));
        paramsList.Add(GetIntParam("@PaymentID", SqlDbType.BigInt, paymentId)); 

        if (CC == null)
            CC = string.Empty;
        if (BCC == null)
            BCC = string.Empty;

        paramsList.Add(GetStringParam("@CC", SqlDbType.NVarChar, CC));
        paramsList.Add(GetStringParam("@BCC", SqlDbType.NVarChar, BCC));

        commnad.Parameters.AddRange(paramsList.ToArray());

        try
        {
            // stored procedure return the new row identity
            object result = ExecuteScalar(commnad);
            if (result == DBNull.Value || string.IsNullOrEmpty(result.ToString()))
                SqlMethods.SendMail("Function -> DataBase: AddMailToDB fail to insert new row to EmailQueue table", 
                                                                                            "DataBase: AddMailToDB Error");
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail("Function -> DataBase: AddMailToDB \nException TYPE -> " + ex.GetType() + "\n Exception Message -> " 
                + ex.Message + "\n ex.Source -> " + ex.Source + "\n ex.StackTrace -> " + ex.StackTrace, "DataBase: AddMailToDB Error");
        }
    }

    public static string ScalarCommandFromProcedure(string ProcedureName, List<SqlParameter> sqlParameterList, string dbName)
    {
        string result = "";
        if (string.IsNullOrEmpty(dbName))
            dbName = "DTS_OnLine";

        using (SqlCommand cmd = CreateCommand())
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = ProcedureName;
            cmd.Parameters.AddRange(sqlParameterList.ToArray());

            //  SqlDataAdapter adp = new SqlDataAdapter(cmd);

            try
            {

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                cmd.Connection.ChangeDatabase(dbName);
                //adp.Fill(DtResult);
                result = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
              //  Mail.SendMail("ScalarCommandFromProcedure", DBHelperFunctions.GetErroMessageMail(ex, "procedure name =" + ProcedureName + " databasename=" + dbName));
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
            return result;
        }
    }



    public static SqlParameter GetStringParam(string name, SqlDbType sqlDbType, string paramValue)
    {
        SqlParameter parameter  = new SqlParameter(name, sqlDbType);
        parameter.Value = paramValue;
        return parameter;
    }

    public static SqlParameter GetIntParam(string name, SqlDbType sqlDbType, int paramValue)
    {
        SqlParameter parameter = new SqlParameter(name, sqlDbType);
        parameter.Value = paramValue;
        return parameter;
    }
}





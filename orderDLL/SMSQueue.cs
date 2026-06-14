using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;


    public class SMSQueue
    {
        /// <summary>
        /// Add Request To Send Sms By Automatic Application
        /// </summary>
        /// <param name="OrganizationID"></param>
        /// <param name="MemberID"></param>
        /// <param name="CouponID">From couponsStock table, default: -1</param>
        /// <param name="SenderName">Sender name or number is restricted by the operators</param>
        /// <param name="Subscribers">Recipients separated by commas 050XXXXXXX,052XXXXXXX</param>
        /// <param name="Message">One Message - 70 characters, Restricted by the operators</param>
        /// <param name="DeliveryDelayInMinutes">Waiting time to send the message from the definition</param>
        /// <param name="ExpirationDelayInMinutes">Time that the message can be delayed in the system, we recommend no less than 120</param>
        /// <param name="SmsType">According to SmsTypes Table</param>
        /// <param name="Priority">0 - High priority</param>
        /// <returns>Successfully positive number
        ///         0 - parametrs not set
        ///        -1 - Exception
        /// </returns>
        public static long AddToSmsQueue(int OrganizationID
           ,string MemberID
           ,long CouponID
           ,string SenderName
           ,string Subscribers
           ,string Message
           ,byte DeliveryDelayInMinutes
           ,byte ExpirationDelayInMinutes
           ,byte SmsType
           ,byte Priority)
        {

            if (string.IsNullOrEmpty(Message) ||
                string.IsNullOrEmpty(SenderName) ||
                string.IsNullOrEmpty(Subscribers))
                return 0;
            else
            {
                Message = Message.Trim();
                SenderName = SenderName.Trim();
                Subscribers = Subscribers.Trim();
            }

            Int64 SmsID = -1;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SmsQueue_Insert";

            cmd.Parameters.Add("@SenderName", SqlDbType.NVarChar);
            cmd.Parameters["@SenderName"].Value = SenderName;
            cmd.Parameters.Add("@Subscribers", SqlDbType.VarChar);
            cmd.Parameters["@Subscribers"].Value = Subscribers;
            cmd.Parameters.Add("@MessageLengh", SqlDbType.Int);
            cmd.Parameters["@MessageLengh"].Value = Message.Length;
            cmd.Parameters.Add("@Message", SqlDbType.NVarChar);
            cmd.Parameters["@Message"].Value = Message;
            

            cmd.Parameters.Add("@OrganizationID", SqlDbType.Int);
            if (OrganizationID != -1)
                cmd.Parameters["@OrganizationID"].Value = OrganizationID;
            else
                cmd.Parameters["@OrganizationID"].Value = DBNull.Value;
            
            cmd.Parameters.Add("@MemberID", SqlDbType.NVarChar);
            if (!string.IsNullOrEmpty(MemberID))
                cmd.Parameters["@MemberID"].Value = MemberID;
            else
                cmd.Parameters["@MemberID"].Value = DBNull.Value;

            cmd.Parameters.Add("@CouponID", SqlDbType.BigInt);
            if (CouponID != -1)
                cmd.Parameters["@CouponID"].Value = CouponID;
            else
                cmd.Parameters["@CouponID"].Value = DBNull.Value;

            cmd.Parameters.Add("@DeliveryDelayInMinutes", SqlDbType.TinyInt);
            if (DeliveryDelayInMinutes != 0)
                cmd.Parameters["@DeliveryDelayInMinutes"].Value = DeliveryDelayInMinutes;
            else
                cmd.Parameters["@DeliveryDelayInMinutes"].Value = 0;

            cmd.Parameters.Add("@ExpirationDelayInMinutes", SqlDbType.TinyInt);
            if (ExpirationDelayInMinutes != 0)
                cmd.Parameters["@ExpirationDelayInMinutes"].Value = ExpirationDelayInMinutes;
            else
                cmd.Parameters["@ExpirationDelayInMinutes"].Value = 120;

            cmd.Parameters.Add("@SmsType", SqlDbType.TinyInt);
            if (SmsType != 0)
                cmd.Parameters["@SmsType"].Value = SmsType;
            else
                cmd.Parameters["@SmsType"].Value = DBNull.Value;

            cmd.Parameters.Add("@Priority", SqlDbType.TinyInt);
            if (Priority != 0)
                cmd.Parameters["@Priority"].Value = Priority;
            else
                cmd.Parameters["@Priority"].Value = 100;

            cmd.Parameters.Add("@SmsID", SqlDbType.BigInt);
            cmd.Parameters["@SmsID"].Direction = ParameterDirection.Output;

            try
            {
                DataBase.ExecuteScalar(cmd);
                if (cmd.Parameters["@SmsID"] != null &&
                    cmd.Parameters["@SmsID"].Value != DBNull.Value &&
                    Int64.TryParse(cmd.Parameters["@SmsID"].Value.ToString(), out SmsID))
                    return SmsID;
            }
            catch (Exception ex)
            {
                SqlMethods.SendMail("Message: " + Message 
                                    + ",SenderName: " + SenderName 
                                    + ",Subscribers: " + Subscribers
                                    + ",ex.Message: " + ex.Message
                                    + ",ex.Source: " + ex.Source
                                    + ",ex.StackTrace: " + ex.StackTrace
                                    + ",ex.TargetSite: " + ex.TargetSite, "can't insert to smsQueue");
            }
            return SmsID;
        }
    }

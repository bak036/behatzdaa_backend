using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

public class InternetBeeReservation
{

    //internal enum RESERVATION_STATUS
    //{
    //    New = 0,
    //    Canceled = 1,
    //    Failed = 2
    //};

    
    /// <summary>
    /// save orders that need to be released in ReservedSeats table
    /// </summary>
    /// <param name="memberId"></param>
    /// <param name="organizationId"></param>
    /// <param name="titanOrderId"></param>
    /// <param name="barcode"></param>
    /// <param name="presentationCode"></param>
    /// <param name="sectionCode"></param>
    /// <param name="seats"></param>
    /// <param name="source"></param>
    /// <param name="webServiceTransactionId"></param>
    /// <returns></returns>
    public static int InsertReservation(string memberId, int organizationId, string titanOrderId, string barcode,
                                string presentationCode, string sectionCode, string seats, string source, Int64 webServiceTransactionId)
    {
        string query = string.Format(@"INSERT INTO DTS_Online..ReservedSeats
                    (MemberId, OrganizationID, TitanOrderId, WebServiceTransactionId, Source, ProductBarcode, PresentationCode, SectionCode, Seats) 
                VALUES ('{0}', {1}, '{2}', {3}, '{4}', '{5}', '{6}', '{7}', '{8}')",
                memberId, organizationId, titanOrderId, webServiceTransactionId, source, barcode, presentationCode, sectionCode, seats);
        return DataBase.ExecuteNonQuery(query); ;
    }


    internal static void SaveSeatsForCancel(string memberId, int organizationId, string barcode, string source,string DBName, string newTranID, ref bool? IsCancelSeatssucceed)
    {
        string errorMessage;
        const string subject = "WSOrdersOnLine ERROR - CAN NOT INSERT Reservation TO ReservedSeats";
        IsCancelSeatssucceed = true;
        Int64 orginalTranID = -1;
        XElement doc, titanDetail=null;
        try
        {
            
            doc = GetXml(memberId, barcode, newTranID, DBName, ref orginalTranID);
            if (doc == null)
            {
                IsCancelSeatssucceed = false;
                errorMessage = GetMessage("OrderDll SaveSeatsForCancel failed. \nXMLPARAM Is EMPTY.", DBName, memberId, barcode, source, newTranID, orginalTranID, null);
                SqlMethods.SendMail(errorMessage, subject);
                return;
            }

            titanDetail = (from TitanDetails in doc.Elements("TitanDetails")
                                select TitanDetails).SingleOrDefault();
            if (titanDetail == null)
            {
                IsCancelSeatssucceed = false;
                errorMessage = GetMessage("OrderDll SaveSeatsForCancel failed. \nTitanDetails tag is missing.", DBName, memberId, barcode, source, newTranID, orginalTranID, doc);
                SqlMethods.SendMail(errorMessage, subject);
                return;
            }

            string titanID, presentationCode, sectionCode ;
            GetTitanDetails(titanDetail, out titanID, out presentationCode, out sectionCode);
            if (string.IsNullOrEmpty(presentationCode) || string.IsNullOrEmpty(sectionCode) || string.IsNullOrEmpty(titanID))
            {
                IsCancelSeatssucceed = false;
                errorMessage = GetMessage("OrderDll SaveSeatsForCancel failed. \nPresentationCode or sectionCode or titanOrderID IS empty.", DBName, memberId, barcode, source, newTranID, orginalTranID, titanDetail);
                SqlMethods.SendMail(errorMessage, subject);
                return;
            }
                    
            var cancelSeats = doc.Elements("TitanDetails").Elements("CancelSeats").SingleOrDefault();
            if (cancelSeats != null) // seats already cancled
            {
                IsCancelSeatssucceed = false;
                errorMessage = GetMessage("OrderDll SaveSeatsForCancel failed. \nseats you are trying to release already canceled.", 
                                    DBName, memberId, barcode, source, newTranID, orginalTranID, titanDetail);
                SqlMethods.SendMail(errorMessage, subject);
                return;
            }

            string seatsString = string.Empty;
            XElement element;
            foreach (var seat in doc.Elements("TitanDetails").Elements("seats").Elements("seat"))
            {
                element = seat.Element("row");
                if (element != null)
                    seatsString += element.Value;
                
                element = seat.Element("chair");
                if (element != null)
                    seatsString += "," + element.Value + " ";
            }
            
            if (string.IsNullOrEmpty(seatsString)) //seats information missing. send mail
            {
                IsCancelSeatssucceed = false;
                errorMessage = GetMessage("OrderDll SaveSeatsForCancel failed. \nseats information missing.", 
                                    DBName, memberId, barcode, source, newTranID, orginalTranID, titanDetail);
                SqlMethods.SendMail(errorMessage, subject);
                return;
            }
            
            int result = InsertReservation(memberId, organizationId, titanID, barcode, presentationCode, sectionCode, seatsString, source, orginalTranID);
            if (result <= 0)
            {
                IsCancelSeatssucceed = false;
                errorMessage = GetMessage("OrderDll SaveSeatsForCancel failed. \fail to insert record to table.", DBName, memberId, barcode, source, newTranID, orginalTranID, titanDetail);
                SqlMethods.SendMail(errorMessage, subject);
            }
        }
        catch (Exception ex)
        {
            IsCancelSeatssucceed = false;
            errorMessage = GetMessage("OrderDll SaveSeatsForCancel failed. \nexception thrown.", DBName, memberId, barcode, source, newTranID, orginalTranID, titanDetail);
                SqlMethods.SendMail(errorMessage, subject);
        }
    }

    private static string GetMessage(string header, string dbName, string memberId, string barcode, string source, string newTranID, long orginalTranID, XElement xmlElement)
    {
        var message = new StringBuilder();
        message.Append(header);
        message.Append("\nDetails: \n");
        message.Append("Please cancel the seats in XMLParam \n");
        message.AppendFormat("DBName: '{0}' \n", dbName);
        message.AppendFormat("Member Id: '{0}' \n", memberId);
        message.AppendFormat("Barcode: '{0}' \n", barcode);
        message.AppendFormat("Source: '{0}' \n", source);
        message.AppendFormat("New TransactionID that canceled: '{0}' \n", newTranID);
        message.AppendFormat("Original TransactionID that canceled: '{0}' \n", orginalTranID);
        
        if (xmlElement == null)
            message.Append("XMLParam: null \n");
        else
            message.AppendFormat("XMLParam: '{0}' \n", xmlElement);

        return message.ToString();
    }

    private static void GetTitanDetails(XElement titanDetail, out string titanID, out string presentationCode, out string sectionCode)
    {
        titanID = presentationCode = sectionCode = string.Empty;
        var element = titanDetail.Element("titanOrderID");
        if (element != null) 
            titanID = element.Value;
            
        element = titanDetail.Element("PresentationCode");
        if (element != null)
            presentationCode = element.Value;
            
        element= titanDetail.Element("section");
        if (element!= null) 
            sectionCode = element.Value;
    }

    //internal static void CancelMarkSeats(string memberId, int organizationId, string barcode, string source, string DBName, string newTranID, ref bool? IsCancelSeatssucceed)
    //{

    //    try
    //    {
    //        IsCancelSeatssucceed = true;
    //        Int64 orginalTranID = -1;
    //        XElement doc = GetXml(memberId, barcode, newTranID, DBName, ref orginalTranID);
    //        if (doc != null)
    //        {
    //            var TitanDetail = (from TitanDetails in doc.Elements("TitanDetails")
    //                              select TitanDetails).SingleOrDefault();
    //            if (TitanDetail != null)
    //            {
    //                string titanID = TitanDetail.Element("titanOrderID").Value;
    //                string presentationCode = TitanDetail.Element("PresentationCode").Value;
    //                string sectionCode = TitanDetail.Element("section").Value;
    //                if (string.IsNullOrEmpty(presentationCode) || string.IsNullOrEmpty(sectionCode))
    //                {
    //                    SqlMethods.SendMail(@"OrderDLL--> CancelMarkSeats function Falied.\n presentationCode OR sectionCode IS NULL!! Details: \n DBName -> " + DBName + " \n memberId -> " + memberId
    //                                        + " \n Barcode -> " + barcode + " \n source -> " + source + " \n New TransactionID that canceled -> " + newTranID
    //                                         + " \n Original TransactionID that canceled -> " + orginalTranID + " \n XMLParam -> " + doc
    //                                        + " \n MUST CANCEL SEATS IN XMLParam!!!" + "Exception Code:1"
    //                                        , "WSOrdersOnLine ERROR - CAN NOT INSERT Reservation TO ReservedSeats");
    //                    IsCancelSeatssucceed = false;
    //                    return;
    //                }

                    
    //                var cancelSeats = doc.Elements("TitanDetails").Elements("CancelSeats").SingleOrDefault();
    //                if (cancelSeats == null) // seat was already canceled 
    //                {
    //                    string seatsString = string.Empty;
    //                    foreach (var seat in doc.Elements("TitanDetails").Elements("seats").Elements("seat"))
    //                    {
    //                        seatsString += seat.Element("row").Value;
    //                        seatsString += "," + seat.Element("chair").Value + " ";
    //                    }
    //                    if (!string.IsNullOrEmpty(seatsString)) 
    //                    {
    //                        int result = InsertReservation(memberId, organizationId, titanID, barcode, presentationCode, sectionCode, seatsString, source, orginalTranID);
    //                        if (result <= 0)
    //                        {
    //                            SqlMethods.SendMail(@"OrderDLL--> CancelMarkSeats function Falied. \n Details: \n DBName -> " + DBName + " \n memberId -> " + memberId
    //                                                + " \n Barcode -> " + barcode + " \n source -> " + source + " \n New TransactionID that canceled -> " + newTranID
    //                                                 + " \n Original TransactionID that canceled -> " + orginalTranID + " \n XMLParam -> " + doc
    //                                                + " \n MUST CANCEL SEATS IN XMLParam!!!" + "Exception Code:2"
    //                                                , "WSOrdersOnLine ERROR - CAN NOT INSERT Reservation TO ReservedSeats");
    //                            IsCancelSeatssucceed = false;
    //                            return;
    //                        }
    //                    }
    //                    else //there are no seats -->send mail
    //                    {
    //                        SqlMethods.SendMail(@"OrderDLL--> CancelMarkSeats function Falied. \n THERE ARE NOT SEATS IN XML!! \n Details: \n DBName -> " + DBName + " \n memberId -> " + memberId
    //                                               + " \n Barcode -> " + barcode + " \n source -> " + source + " \n New TransactionID that canceled -> " + newTranID
    //                                                + " \n Original TransactionID that canceled -> " + orginalTranID + " \n XMLParam -> " + doc
    //                                               + " \n MUST CANCEL SEATS IN XMLParam!!!" + "Exception Code:5"
    //                                               , "WSOrdersOnLine ERROR - CAN NOT INSERT Reservation TO ReservedSeats");
    //                        IsCancelSeatssucceed = false;
    //                        return;
    //                    }
    //                }

    //            }
    //            else
    //            {
    //                IsCancelSeatssucceed = false;
    //            }
    //        }
    //        else
    //        {
                
    //            SqlMethods.SendMail(@"OrderDLL--> CancelMarkSeats function Falied. \n XMLPARAM Is EMPTY!! Details: \n DBName -> " + DBName + " \n memberId -> " + memberId +
    //                            " \n Barcode -> " + barcode + " \n source -> " + source + " \n New TransactionID that canceled -> " + newTranID
    //                             + " \n Original TransactionID that canceled -> " + orginalTranID + " \n MUST CANCEL SEATS IN XMLParam!!!" + "Exception Code:3"
    //                            , "WSOrdersOnLine ERROR - CAN NOT INSERT Reservation TO ReservedSeats");
    //            IsCancelSeatssucceed = false;
    //        }
           
                
    //    }
    //    catch (Exception ex)
    //    {
    //        IsCancelSeatssucceed = false;
    //        SqlMethods.SendMail(@"OrderDLL--> CancelMarkSeats function Falied. \n Details: \n DBName -> " + DBName + " \n memberId -> "+memberId+
    //                            " \n Barcode -> "+ barcode+  " \n source -> "+ source+" \n New TransactionID that canceled -> "+ newTranID
    //                           + "\n MUST CAMCEL SEATS IN XMLParam!!!" + "Exception Code:4"
    //            , "WSOrdersOnLine ERROR - CAN NOT INSERT Reservation TO ReservedSeats");
            
    //    }

    //}

    private static XElement GetXml(string memberId, string barcode, string newTranID, string DBName,ref Int64 orginalTranID)
    {
        string query = string.Format(@"SELECT WST.XMLParam,TTransactionId 
                         FROM {0}..WebServiceTransaction WST
                         WHERE [TTransactionMemberID]='{1}' AND [TTransactionProductID]='{2}' AND [TTransactionquantity] > 0  
                         AND [TTransactionOrder]=
                                    (SELECT isnull(atraOrder.OriginalAsmchta,atraOrder.[MemberOrderAsmchta]) FROM {0}..[ATRACTIONSOrders] atraOrder 
                                    WHERE atraOrder.[MemberOrderAsmchta] =(SELECT TTransactionOrder FROM {0}..WebServiceTransaction WHERE TTransactionId='{3}'))",
                         DBName,memberId,barcode,newTranID);

        string orderParams = string.Empty;
        try
        {
            
            SqlConnection connection = new SqlConnection();
            SqlDataReader reader = DataBase.ExecuteReader(query, connection);
            if (reader.Read())
            {
                orginalTranID = Convert.ToInt64(reader["TTransactionId"]);
                orderParams = Convert.ToString(reader["XMLParam"]);
                if (!string.IsNullOrEmpty(orderParams))
                    return XElement.Parse(orderParams);
                return null;
            }
            else
            {
                orginalTranID = -1;
                return null;
            }
        }
        catch (Exception ex)
        {
            
            return null;
        }

    }
   
}

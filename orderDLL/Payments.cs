using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;


public class Payments
{
    /// <summary>
    /// save payment and commissions details in Payment Table.
    /// </summary>
    /// <param name="confirmationNumber">Credit Gurd confirmation number</param>
    /// <param name="databaseName"></param>
    /// <param name="memberID"></param>
    /// <param name="organizationCommission"></param>
    /// <param name="operatorCommission"></param>
    /// <param name="pointsCashValue"></param>
    /// <param name="pointsUsed"></param>
    /// <returns>PaymentID from Payments Table</returns>
    /// 
    /// 
    public static string SavePaymentDetails(string cardNumber, string cardOwnerID, string totalSum,
                                            string confirmationNumber, string databaseName, string memberID,
                                            decimal organizationCommission,
                                            decimal operatorCommission, decimal pointsCashValue, int pointsUsed,
                                            string xmlParams, string Uid,int creditCardStatus)
    {
        return SavePaymentDetails(cardNumber, cardOwnerID, totalSum,
                                  confirmationNumber, databaseName, memberID,
                                  organizationCommission,
                                  operatorCommission, pointsCashValue, pointsUsed,
                                  xmlParams, Uid, 0, creditCardStatus);
    }

    public static string SavePaymentDetails(string cardNumber, string cardOwnerID, string totalSum,
            string confirmationNumber, string databaseName, string memberID, decimal organizationCommission,
            decimal operatorCommission, decimal pointsCashValue, int pointsUsed, string xmlParams, string Uid, decimal TrustProgramCommission,int creditCardStatus)
    {
        string last4Digits, paymentID, query; 
        try
        {
            string PointsChargedStatus = "0";
            if (pointsCashValue > 0)
                PointsChargedStatus = "1";
            last4Digits = totalSum != "0.0" ? Last4Digits(cardNumber) : "";
            query = string.Format(@"INSERT INTO {0}..Payments (TimeStamp, MemberID, CardOwnerID, Last4Digits, ConfirmationNumber, ProductsOrderStatus, 
                Charged, PointsChargedStatus, PointsUsed, PointsCashValue, OrganizationCommission, OperatorCommission, Xml,UniquId,TrustProgramCommission,CreditCardStatus) 
                VALUES (current_timestamp, '{1}', '{2}', '{3}', '{4}', 0, '{5}',{11}, '{6}', '{7}', '{8}', {9}, '{10}','{12}',{13},{14}) SELECT @@Identity ",
             databaseName, memberID, cardOwnerID, last4Digits, confirmationNumber, totalSum, pointsUsed, pointsCashValue,
             organizationCommission, operatorCommission, xmlParams, PointsChargedStatus, Uid, TrustProgramCommission, creditCardStatus);

            paymentID = DataBase.ExecuteScalar(query).ToString();
            return paymentID;
        }
        catch (Exception ex)
        {
            SqlMethods.SendMail(ex.StackTrace + "\n" + ex.Message, "Insert to payment Failed");
            return null;
        }
    }

    private static string Last6Digits(string cardNumber)
    {
        return cardNumber.Substring(cardNumber.Length - 6);
    }
    private static string Last4Digits(string cardNumber)
    {
        return cardNumber.Substring(cardNumber.Length - 4);
    }

    public static void UpdateOrderStatus(string databaseName, string paymentID)
    {
        string query = string.Format("Update {0}..Payments Set ProductsOrderStatus = 1 Where PaymentID = {1}", databaseName, paymentID);
        DataBase.ExecuteNonQuery(query);
    }

    public static void UpdateOrderStatus(string databaseName, string paymentID, string status)
    {
        string query = string.Format("Update {0}..Payments Set ProductsOrderStatus = {2} Where PaymentID = {1}", databaseName, paymentID, status);
        DataBase.ExecuteNonQuery(query);
    }

    public static void UpdateOrderStatusForRefundDeal(string databaseName, string paymentID)
    {
        string query = string.Format("Update {0}..Payments Set ProductsOrderStatus = 2 Where PaymentID = {1}", databaseName, paymentID);
        DataBase.ExecuteNonQuery(query);
    }

    public static XDocument GetRoot()
    {
        XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("payment details"),
                new XElement("Details"));
        return doc;
    }

    public static void AddSource(XDocument doc, string value) { doc.Root.Add(new XElement("source", value)); }
    public static void AddUserID(XDocument doc, string value) { doc.Root.Add(new XElement("userID", value)); }
    public static void AddUserName(XDocument doc, string value) { doc.Root.Add(new XElement("userName", value)); }
    public static void AddPayments(XDocument doc, string value) { doc.Root.Add(new XElement("payments", value)); }
    public static void AddFirstPayment(XDocument doc, string value) { doc.Root.Add(new XElement("firstPayment", value)); }
    public static void AddPeriodicalPayment(XDocument doc, string value) { doc.Root.Add(new XElement("periodicalPayment", value)); }
    public static void AddTranId(XDocument doc, string value) { doc.Root.Add(new XElement("tranId", value)); }
    public static void AddAuthNumber(XDocument doc, string value) { doc.Root.Add(new XElement("authNumber", value)); }
    public static void AddCardId(XDocument doc, string value) { doc.Root.Add(new XElement("cardId", value)); }
    public static void AddTerminalNumber(XDocument doc, string value) { doc.Root.Add(new XElement("terminalNumber", value)); }
}

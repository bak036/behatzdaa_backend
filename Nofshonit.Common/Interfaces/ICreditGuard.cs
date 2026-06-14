using CreditServices.Interfaces;
using PaymentsAPI.Models;
using System.Xml.Linq;

namespace Nofshonit.Common.Interfaces
{
    public interface ICreditGuard
    {
        TransactionResults Payment(CardNumberPaymentModelView cardNumberPaymentModelView);
        TransactionResults PaymentToken(CardTokenPaymentModelView cardTokenPaymentModelView);
        XDocument GetXmlPaymentInformation(string source, string ConfirmationNumber, string AuthNumber, string TerminalNumber, int numOfPayments, string CardId = "2000000000000000");
    }
}

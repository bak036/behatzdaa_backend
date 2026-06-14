using CreditServices.Interfaces;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using PaymentsAPI.Models;
using PurchaseHandler;
using System.Xml.Linq;

namespace Nofshonit.BL.CreditGuard
{
    public class CreditGuard : BaseBL, ICreditGuard
    {
        private readonly IContextManager _contextManager;
        public CreditGuard()
        {
            _contextManager = Container.Resolve<IContextManager>();
        }

        public TransactionResults Payment(CardNumberPaymentModelView cardNumberPaymentModelView)
        {
            cardNumberPaymentModelView.GUID = "0";
            cardNumberPaymentModelView.OrganizationID = _contextManager.CurrentOrganization().OrgId;
            cardNumberPaymentModelView.MemberID = _contextManager.CurrentUser().Id;
            return ApiClient.instance.Payment(cardNumberPaymentModelView);
        }

        public TransactionResults PaymentToken(CardTokenPaymentModelView cardTokenPaymentModelView)
        {
            cardTokenPaymentModelView.GUID = "0";
            cardTokenPaymentModelView.OrganizationID = _contextManager.CurrentOrganization().OrgId;
            cardTokenPaymentModelView.MemberID = _contextManager.CurrentUser().Id;
            return ApiClient.instance.PaymentToken(cardTokenPaymentModelView);
        }

        public XDocument GetXmlPaymentInformation(string source, string ConfirmationNumber, string AuthNumber, string TerminalNumber, int numOfPayments, string CardId)
        {
            CreditGuardXMLHelper helper = new CreditGuardXMLHelper();
            XDocument doc = helper.GetRoot();
            helper.AddSource(doc, source);
            helper.AddPayments(doc, numOfPayments.ToString());
            // Cancel Information
            helper.AddAuthNumber(doc, AuthNumber);
            helper.AddCardId(doc, CardId);
            helper.AddTranId(doc, ConfirmationNumber);
            helper.AddTerminalNumber(doc, TerminalNumber);
            return doc;
        }
    }
}

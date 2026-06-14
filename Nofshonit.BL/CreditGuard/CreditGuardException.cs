using Nofshonit.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.BL.CreditGuard
{
    public class CreditGuardException : Exception
    {
        public string CreditGuardMessage { get; set; }
        public int CreditGuardMessageId { get; set; }


        public CreditGuardException()
        { }

        public CreditGuardException(ECreditGuardErrors error) : base(CreditGuardExceptionMapper(error))
        {
        }

        public CreditGuardException(string message) : base(message)
        {
            CreditGuardMessage = message;
        }

        public CreditGuardException(string message, int messageId) : base(message)
        {
            CreditGuardMessage = message;
            CreditGuardMessageId = messageId;
        }

        public CreditGuardException(string message, Exception inner = null) : base(message, inner)
        {
            CreditGuardMessage = message;
        }


        private static string CreditGuardExceptionMapper(ECreditGuardErrors error)
        {
            switch (error)
            {
                case ECreditGuardErrors.Blocked:
                case ECreditGuardErrors.Stolen:
                case ECreditGuardErrors.CallCreditCardCompany:
                case ECreditGuardErrors.Refusal:
                case ECreditGuardErrors.Fake:
                case ECreditGuardErrors.MustCallCreditCardCompany:
                case ECreditGuardErrors.NotManagedToCall:
                case ECreditGuardErrors.InvalidCard:
                case ECreditGuardErrors.CreditCardLimitForImmediateBilling:
                    return RetrieveMessage(10018);
                case ECreditGuardErrors.NoIdentityAndCardNumberMatch:
                    return RetrieveMessage(10015);
                case ECreditGuardErrors.WrongValidity:
                    return RetrieveMessage(10016);
                default:
                    return RetrieveMessage(10014);
            }
        }

        public static string RetrieveMessage(int key)
        {
            return MessagesUtil.GetMessagesByKey(new List<int> { key }).FirstOrDefault().MessageText;
        }
    }

}

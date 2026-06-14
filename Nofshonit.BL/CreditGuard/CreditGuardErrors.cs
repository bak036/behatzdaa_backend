using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.BL.CreditGuard
{
    public enum ECreditGuardErrors
    {
        Blocked = 1,
        Stolen = 2,
        CallCreditCardCompany = 3,
        Refusal = 4,
        Fake = 5,
        NoIdentityAndCardNumberMatch = 6,
        MustCallCreditCardCompany = 7,
        NotManagedToCall = 9,
        InvalidCard = 33,
        WrongValidity = 36,
        CreditCardLimitForImmediateBilling = 38,
        Other = 99
    }

    public static class CreditGuardErrorsGenerator
    {
        public static ECreditGuardErrors Generate(string error)
        {
            var afterTrim = !string.IsNullOrEmpty(error) ? error.TrimStart(new char[] { '0' }) : null;
            switch (afterTrim)
            {
                case "1":
                    return ECreditGuardErrors.Blocked;
                case "2":
                    return ECreditGuardErrors.Stolen;
                case "3":
                    return ECreditGuardErrors.CallCreditCardCompany;
                case "4":
                    return ECreditGuardErrors.Refusal;
                case "5":
                    return ECreditGuardErrors.Fake;
                case "6":
                case "39":
                    return ECreditGuardErrors.NoIdentityAndCardNumberMatch;
                case "7":
                    return ECreditGuardErrors.MustCallCreditCardCompany;
                case "9":
                    return ECreditGuardErrors.NotManagedToCall;
                case "33":
                    return ECreditGuardErrors.InvalidCard;
                case "36":
                    return ECreditGuardErrors.WrongValidity;
                case "38":
                    return ECreditGuardErrors.CreditCardLimitForImmediateBilling;
                default:
                    return ECreditGuardErrors.Other;
            }
        }
    }

}

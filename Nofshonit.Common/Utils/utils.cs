using Nofshonit.Common.DTOs;
using Nofshonit.Common.Exceptions;
using SendSmsClient;
using SendSmsClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nofshonit.Common.Utils
{
    public static class utils
    {


        public static string CreateOtp(int smsLoginCodeLength)
        {
            Random generator = new Random();
            string code = generator.Next(0, 999999999).ToString("D6").Substring(0, smsLoginCodeLength);
            return code;
        }

        public static OtpSmsLog SendSMS(string connectionString, string smsServiceUrl, string cellPhone, int orgId, string memberId, string message)
        {
            SmsClient client = new SmsClient(connectionString, smsServiceUrl);
            var res = client.SendOtpSMS(orgId, memberId, "Behatsdaa", cellPhone, message, 0, 1);
            return res;
        }

        public static bool ValidateCharactersOnly(string value)
        {
            if (value != null)
            {
                if (!Regex.IsMatch(value, "^['א-תa-zA-Z- ().\"]+$"))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool ValidateCharactersAndNumbersOnly(string value)
        {
            if (value != null)
            {
                if (!Regex.IsMatch(value, "^['א-תa-zA-Z0-9- ().\"]+$"))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool ValidateNumbersOnly(string value)
        {
            if (value != null)
            {
                if (!Regex.IsMatch(value, @"^[0-9]*$"))
                {
                    return false;
                }
            }
            return true;
        }

        public static void ValidatePinCodeFormat(string pinCode,int pinCodeLength)
        {
            const string pinCodeErrorMessage = "יש לקבוע קוד מקוצר בן 5 ספרות שונות, שלא יכיל רצף של שלוש ספרות עוקבות (עולה או יורד), או שלוש ספרות זהות.";

            if (string.IsNullOrEmpty(pinCode) || !Regex.IsMatch(pinCode, $"^\\d{{{pinCodeLength}}}$"))
            {
                throw new BusinessException(pinCodeErrorMessage);
            }
            // רצפים עולים של 3 ספרות
            string incSeq = "0123456789";
            for (int i = 0; i <= incSeq.Length - 3; i++)
            {
                string sub = incSeq.Substring(i, 3);
                if (pinCode.Contains(sub))
                    throw new BusinessException(pinCodeErrorMessage);
            }

            // רצפים יורדים של 3 ספרות
            string decSeq = "9876543210";
            for (int i = 0; i <= decSeq.Length - 3; i++)
            {
                string sub = decSeq.Substring(i, 3);
                if (pinCode.Contains(sub))
                    throw new BusinessException(pinCodeErrorMessage);
            }

            // רצף של 3 ספרות זהות
            if (Regex.IsMatch(pinCode, @"(\d)\1{2,}"))
                throw new BusinessException(pinCodeErrorMessage);

            // קודים שחורים נפוצים
            var blackList = new HashSet<string>
            {
                "00000", "12345", "11111", "22222", "33333",
                "44444", "55555", "66666", "77777", "88888", "99999"
            };
            if (blackList.Contains(pinCode))
                throw new BusinessException(pinCodeErrorMessage);
        }
        public static bool ValidateUpdateContactApproval(bool isUpdateDetailsApproved, UserDTO oldUser, UserDTO newUser)
        {
            bool emailChangedAndAdded =
                ((oldUser.Email ?? "").Trim() != (newUser.Email ?? "").Trim()) &&
                !string.IsNullOrWhiteSpace(newUser.Email);

            bool mobilePhoneChangedAndAdded =
                ((oldUser.MobilePhone ?? "").Trim() != (newUser.MobilePhone ?? "").Trim()) &&
                !string.IsNullOrWhiteSpace(newUser.MobilePhone);

            bool partnerEmailChangedAndAdded =
                ((oldUser.PartnerEmail ?? "").Trim() != (newUser.PartnerEmail ?? "").Trim()) &&
                !string.IsNullOrWhiteSpace(newUser.PartnerEmail);

            bool partnerPhoneChangedAndAdded =
                ((oldUser.PartnerPhone ?? "").Trim() != (newUser.PartnerPhone ?? "").Trim()) &&
                !string.IsNullOrWhiteSpace(newUser.PartnerPhone);

            bool anyFieldChangedAndAdded =
                emailChangedAndAdded ||
                mobilePhoneChangedAndAdded ||
                partnerEmailChangedAndAdded ||
                partnerPhoneChangedAndAdded;

            if (anyFieldChangedAndAdded && !isUpdateDetailsApproved)
            {
                return false;
            }

            return true;
        }



    }
}

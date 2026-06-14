using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.Extensions
{
    public static class StringExtension
    {
        public static string GetTrim(this string str)
        {
            return !string.IsNullOrEmpty(str) ? str.Trim() : str;
        }
    }
}

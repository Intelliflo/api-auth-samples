using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IF.Samples.OAuth.RefreshToken.Security
{
    public static class StringExtensions
    {
        public static string ToUtf8Hex(this string stringToConvert)
        {
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(stringToConvert));
            return base64;
        }
    }
}
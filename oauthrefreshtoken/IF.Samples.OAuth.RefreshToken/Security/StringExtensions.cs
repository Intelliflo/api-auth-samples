using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IF.Samples.OAuth.RefreshToken.Security
{
    /// <summary>
    /// Helpful string extension methods to improve readability of the code
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Encodes the string as UTF-8 and converts it to base64.
        /// </summary>
        /// <param name="stringToConvert">The text to convert</param>
        /// <returns>Base 64 encoded version of the supplied text</returns>
        public static string ToUtf8Hex(this string stringToConvert)
        {
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(stringToConvert));
            return base64;
        }
    }
}
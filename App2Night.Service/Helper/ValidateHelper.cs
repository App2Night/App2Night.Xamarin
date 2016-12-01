using System.Text.RegularExpressions;

namespace App2Night.Service.Helper
{
    public class ValidateHelper
    {
        static Regex ValidEmailRegex = CreateValidEmailRegex();

        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// Validates email.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public static bool EmailIsValid(string emailAddress)
        {
            bool isValid = ValidEmailRegex.IsMatch(emailAddress);
            return isValid;
        }
    }
}
using System.Globalization;

namespace Boilerplate.Core.Classes.ActivityLog
{
    public class UserHelper
    {
        public static CultureInfo GetCultureInfo(string userLang)
        {
            return new CultureInfo(GetCultureLanguage(userLang));
        }

        private static string GetCultureLanguage(string lang)
        {
            return lang.StartsWith("sv") ? "sv" : "en";
        }
    }
}
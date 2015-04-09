using Umbraco.Core.Models;
namespace Camelonta.Boilerplate.Classes
{
    public static class ExtensionMethods
    {
        public static string NavName(this IPublishedContent page)
        {
            if (page.GetProperty("navName").HasValue)
                return page.GetProperty("navName").Value.ToString();
            return page.Name;
        }
    }
}
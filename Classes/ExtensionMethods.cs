using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Models;

namespace Camelonta.Boilerplate.Classes
{
    public static class ExtensionMethods
    {
        public static IEnumerable<DynamicPublishedContent> ShowOnlyVisible(this DynamicPublishedContentList pages)
        {
            return pages.Where(p => p.Visible);
        }
    }
}
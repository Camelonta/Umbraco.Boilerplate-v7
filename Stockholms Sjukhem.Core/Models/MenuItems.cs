using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Boilerplate.Core.Models
{
    public class MenuItems
    {
        public IPublishedContent CurrentPage { get; set; }
        public IEnumerable<IPublishedContent> Pages { get; set; }

        public string Type { get; set; }
        public int Level { get; set; }
    }
}
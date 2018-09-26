using System.Collections.Generic;

namespace Boilerplate.Core.Classes.CamelontaUI
{
    /// <summary>
    /// Utillity methods for grouping items
    /// </summary>
    public class Grouping
    {
        /// <summary>
        /// Groups any list into a list of lists with {itemsPerRow} number of items in each list
        /// </summary>
        public static IEnumerable<IEnumerable<T>> GroupListIntoRows<T>(IEnumerable<T> items, int itemsPerRow)
        {
            var groups = new List<List<T>>();
            var group = new List<T>();
            int count = 0;

            foreach (var item in items)
            {
                count++;
                if (count % itemsPerRow == 0)
                {
                    group.Add(item);
                    groups.Add(group);
                    group = new List<T>();
                }
                else
                {
                    group.Add(item);
                }
            }
            if (group.Count > 0)
            {
                groups.Add(group);
            }

            return groups;
        }
    }
}
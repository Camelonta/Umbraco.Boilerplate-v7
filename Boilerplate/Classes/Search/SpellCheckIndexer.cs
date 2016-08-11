using Examine;
using Examine.LuceneEngine.Config;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Umbraco.Core;
using UmbracoExamine;
using UmbracoExamine.Config;

namespace Camelonta.Boilerplate.Classes.Search
{
    public class SpellCheckIndexer : BaseUmbracoIndexer
    {
        // May be extended to find words from more types
        protected override IEnumerable<string> SupportedTypes
        {
            get
            {
                yield return IndexTypes.Content;
            }
        }

        protected override void AddDocument(Dictionary<string, string> fields, IndexWriter writer, int nodeId, string type)
        {
            var doc = new Document();
            List<string> cleanValues = new List<string>();
            // This example just cleans HTML, but you could easily clean up json too
            CollectCleanValues(fields, cleanValues);
            var allWords = String.Join(" ", cleanValues);
            // Make sure you don't stem the words. You want the full terms, but no whitespace or punctuation.
            doc.Add(new Field("word", allWords, Field.Store.NO, Field.Index.ANALYZED));
            writer.UpdateDocument(new Term("__id", nodeId.ToString(CultureInfo.InvariantCulture)), doc);
        }
        protected override IIndexCriteria GetIndexerData(IndexSet indexSet)
        {
            var indexCriteria = indexSet.ToIndexCriteria(DataService);
            return indexCriteria;
        }
        private void CollectCleanValues(Dictionary<string, string> fields, List<string> cleanValues)
        {
            foreach (var field in fields)
            {
                string value = field.Value;

                if(field.Key == "grid")
                {
                    value = ExamineIndexer.GetGridText(field.Value);
                }

                cleanValues.Add(CleanValue(value));
            }
        }
        private static string CleanValue(string value)
        {
            return HttpUtility.HtmlDecode(value.StripHtml());
        }
    }
}
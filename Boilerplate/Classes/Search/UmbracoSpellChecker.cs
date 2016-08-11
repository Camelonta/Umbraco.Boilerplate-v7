using Examine;
using Examine.LuceneEngine.Providers;
using Examine.SearchCriteria;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SpellChecker.Net.Search.Spell;
using System.Collections.Generic;
using System.Linq;

namespace Camelonta.Boilerplate.Classes.Search
{
    public class UmbracoSpellChecker
    {
        private static readonly object lockObj = new object();
        private static UmbracoSpellChecker instance;
        public static UmbracoSpellChecker Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        var searchProvider = (BaseLuceneSearcher)ExamineManager.Instance.SearchProviderCollection["SpellCheckSearcher"];
                        instance = new UmbracoSpellChecker(searchProvider);
                        instance.EnsureIndexed();
                    }
                }
                return instance;
            }
        }


        private readonly BaseLuceneSearcher searchProvider;
        private readonly SpellChecker.Net.Search.Spell.SpellChecker checker;
        private readonly IndexReader indexReader;
        private bool isIndexed;
        public UmbracoSpellChecker(BaseLuceneSearcher searchProvider)
        {
            this.searchProvider = searchProvider;
            var searcher = (IndexSearcher)searchProvider.GetSearcher();
            indexReader = searcher.GetIndexReader();
            checker = new SpellChecker.Net.Search.Spell.SpellChecker(new RAMDirectory(), new JaroWinklerDistance());
        }
        private void EnsureIndexed()
        {
            if (!isIndexed)
            {
                checker.IndexDictionary(new LuceneDictionary(indexReader, "word"));
                isIndexed = true;
            }
        }
        public string Check(string value)
        {
            EnsureIndexed();
            var existing = indexReader.DocFreq(new Term("word", value));
            if (existing > 0)
                return value;
            var suggestions = checker.SuggestSimilar(value, 10, null, "word", true);
            var jaro = new JaroWinklerDistance();
            var leven = new LevenshteinDistance();
            var ngram = new NGramDistance();
            var metrics = suggestions.Select(s => new
            {
                word = s,
                freq = indexReader.DocFreq(new Term("word", s)),
                jaro = jaro.GetDistance(value, s),
                leven = leven.GetDistance(value, s),
                ngram = ngram.GetDistance(value, s)
            })
            .OrderByDescending(metric =>
                (
                    (metric.freq / 100f) +
                    metric.jaro +
                    metric.leven +
                    metric.ngram
                )
                / 4f
            )
            .ToList();
            return metrics.Select(m => m.word).FirstOrDefault();
        }

        public List<string> SuggestSimilar(string value, int numberOfSuggestions)
        {
            EnsureIndexed();
            return checker.SuggestSimilar(value, numberOfSuggestions, null, "word", true).ToList();
        }
    }
}
using Examine;
using Examine.LuceneEngine;
using Examine.LuceneEngine.Providers;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Store;
using System.Collections.Generic;
using System.Linq;
using UmbracoExamine;
using System.Collections;
using Lucene.Net.Analysis;
using NSubstitute;

namespace Camelonta.Boilerplate.Tests
{
    public class MockIndexFactory
    {
        public static MockedIndex GetMock(
            MockIndexFieldList standardFields,
            MockIndexFieldList userFields,
            IEnumerable<string> indexTypes,
            IEnumerable<string> includeNodeTypes,
            IEnumerable<string> excludeNodeTypes)
        {
            var index = new MockedIndex
            {
                StandardFields = standardFields,
                UserFields = userFields,
                IncludeNodeTypes = includeNodeTypes.ToArray(),
                ExcludeNodeTypes = excludeNodeTypes.ToArray(),
                SimpleDataService = Substitute.For<ISimpleDataService>(),
                LuceneDir = new RAMDirectory()
            };

            index.IndexCriteria = new IndexCriteria(standardFields, userFields, index.IncludeNodeTypes, index.ExcludeNodeTypes, -1);

            index.Analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);

            index.Indexer = new SimpleDataIndexer(index.IndexCriteria, index.LuceneDir, index.Analyzer, index.SimpleDataService, indexTypes, false);

            index.Searcher = new UmbracoExamineSearcher(index.LuceneDir, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));

            return index;
        }
    }

    public class MockIndexFieldList : IEnumerable<IIndexField>
    {
        private List<IIndexField> IndexFieldList { get; set; }

        public MockIndexFieldList()
        {
            IndexFieldList = new List<IIndexField>();
        }

        public MockIndexFieldList AddIndexField(string name, bool enableSorting)
        {
            return AddIndexField(name, string.Empty, enableSorting);
        }

        public MockIndexFieldList AddIndexField(string name)
        {
            return AddIndexField(name, string.Empty);
        }

        public MockIndexFieldList AddIndexField(string name, string type, bool enableSorting = false)
        {
            var indexField = Substitute.For<IIndexField>();
            indexField.Name.Returns(name);
            indexField.EnableSorting.Returns(enableSorting);
            indexField.Type.Returns(type);

            IndexFieldList.Add(indexField);

            return this;
        }

        public IEnumerator<IIndexField> GetEnumerator()
        {
            return IndexFieldList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return IndexFieldList.GetEnumerator();
        }
    }

    public class MockSimpleDataSet : IEnumerable<SimpleDataSet>
    {
        private List<SimpleDataSet> ListOfSimpleData { get; set; }
        private string Type { get; set; }

        public MockSimpleDataSet(string type)
        {
            ListOfSimpleData = new List<SimpleDataSet>();
            Type = type;
        }

        public MockSimpleDataSet AddData(int id, Dictionary<string, string> rowData)
        {
            var nodeDefinition = new IndexedNode { NodeId = id, Type = Type };

            ListOfSimpleData.Add(
                new SimpleDataSet
                {
                    NodeDefinition = nodeDefinition,
                    RowData = rowData
                });
            return this;
        }

        public MockSimpleDataSet AddData(int id, string name, string value)
        {
            var nodeDefinition = new IndexedNode { NodeId = id, Type = Type };

            var rowData = new Dictionary<string, string>
            {
                {name, value}
            };

            ListOfSimpleData.Add(
                new SimpleDataSet
                {
                    NodeDefinition = nodeDefinition,
                    RowData = rowData
                });
            return this;
        }


        public IEnumerator<SimpleDataSet> GetEnumerator()
        {
            return ListOfSimpleData.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ListOfSimpleData.GetEnumerator();
        }
    }

    public class MockedIndex
    {
        public ISearcher Searcher { get; set; }

        public IIndexer Indexer { get; set; }

        public ISimpleDataService SimpleDataService { get; set; }

        public Directory LuceneDir { get; set; }

        public MockIndexFieldList StandardFields { get; set; }

        public IIndexCriteria IndexCriteria { get; set; }

        public Analyzer Analyzer { get; set; }

        public MockIndexFieldList UserFields { get; set; }

        public IEnumerable<string> IndexTypes { get; set; }

        public IEnumerable<string> IncludeNodeTypes { get; set; }

        public IEnumerable<string> ExcludeNodeTypes { get; set; }

    }
}

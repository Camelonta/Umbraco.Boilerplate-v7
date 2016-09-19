using Boilerplate.Core.Classes.Search;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Boilerplate.Web.Tests.Classes
{
    [TestFixture]
    public class SearchTests
    {
        #region Setup

        MockedIndex mockedIndex;
        const string IndexType = "Search";

        List<string> indexFields = new List<string>
        {
            "robotsIndex",
            "grid"
        };

        public void SetUp(MockSimpleDataSet mockDataSet)
        {
            // Create fields
            var fields = new MockIndexFieldList();
            foreach (var indexedField in indexFields)
            {
                fields.AddIndexField(indexedField);
            }

            // Create index
            mockedIndex = MockIndexFactory.GetMock(
            new MockIndexFieldList().AddIndexField("id", "Number", true),
            fields,
            new[] { IndexType },
            new string[] { },
            new string[] { });

            mockedIndex.SimpleDataService.GetAllData(IndexType).Returns(mockDataSet);
            mockedIndex.Indexer.RebuildIndex();
        }

        #endregion

        [Test]
        public void Search_get_SearchResults_by_robotsIndex_and_search_term()
        {
            // Create data
            var mockDataSet = new MockSimpleDataSet(IndexType)
                .AddData(11, new Dictionary<string, string> { { "robotsIndex", "0" }, { "grid", "Test" } })
                .AddData(22, new Dictionary<string, string> { { "robotsIndex", "0" }, { "grid", "Testar" } })
                .AddData(33, new Dictionary<string, string> { { "robotsIndex", "1" }, { "grid", "Test" } });

            SetUp(mockDataSet);
            
            var search = new Search(mockedIndex.Indexer, mockedIndex.Searcher, "Test", 0, 5);

            Assert.AreEqual(2, search.SearchResults.Count);
            Assert.AreEqual(11, search.SearchResults.First().Id);
            Assert.AreEqual(22, search.SearchResults.Last().Id);
        }

        [Test]
        public void Search_match_whole_sentence()
        {
            // Create data
            var mockDataSet = new MockSimpleDataSet(IndexType)
                .AddData(11, new Dictionary<string, string> { { "robotsIndex", "0" }, { "grid", "Katja testar solen skiner och ölen är kall" } })
                .AddData(22, new Dictionary<string, string> { { "robotsIndex", "0" }, { "grid", "testar" } })
                .AddData(33, new Dictionary<string, string> { { "robotsIndex", "0" }, { "grid", "solen skiner" } });

            SetUp(mockDataSet);

            var search = new Search(mockedIndex.Indexer, mockedIndex.Searcher, "Katja testar solen skiner och ölen är kall", 0, 5);

            Assert.AreEqual(1, search.SearchResults.Count);
            Assert.AreEqual(11, search.SearchResults.Single().Id);
        }

        [Test]
        public void Search_only_get_specified_amout_of_items()
        {
            int totalAmount = 1500;
            int take = 10;

            // Create data
            var mockDataSet = new MockSimpleDataSet(IndexType);
            for (int j = 1; j <= totalAmount; j++)
            {
                mockDataSet.AddData(j, new Dictionary<string, string> { { "robotsIndex", "0" }, { "grid", "Test" } });
            }

            SetUp(mockDataSet);

            var search = new Search(mockedIndex.Indexer, mockedIndex.Searcher, "Test", 0, take);

            Assert.AreEqual(take, search.SearchResults.Count);
            Assert.AreEqual(totalAmount, search.TotalResults);
        }
    }
}

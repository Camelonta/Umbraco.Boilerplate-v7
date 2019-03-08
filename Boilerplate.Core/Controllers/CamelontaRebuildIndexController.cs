using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using Examine;
using Examine.LuceneEngine.Providers;
using Examine.Providers;
using Umbraco.Core.Logging;
using Umbraco.Web.WebApi;

namespace Boilerplate.Core.Controllers
{
    /// <summary>
    /// Rebuilds External index if at night (time is after midnight and before 1 am) and deletions exist and a day has passed since last rebuild.
    /// Time is saved in /App_Data/RebuildExternalIndexDateTime.txt.
    /// umbracoSettings.config need to specify this scheduled task to run once per hour. This is inactivated by default.
    /// AppSetting IndexesToRebuild needs to be specified with a csv of the names of indexes included. This is inactivated by default.
    /// </summary>
    public class CamelontaRebuildIndexController: UmbracoApiController
    {
        private readonly string _filePath;
        private readonly string[] _indexNames;

        public CamelontaRebuildIndexController()
        {
            _filePath = HostingEnvironment.MapPath("~\\App_Data") + "\\RebuildExternalIndexDateTime.txt";
            var indexesToRebuildCsv = ConfigurationManager.AppSettings["CamelontaIndexesToRebuild"];
            if (!string.IsNullOrEmpty(indexesToRebuildCsv))
            {
                _indexNames = indexesToRebuildCsv.Split(',');
            }
        }

        [HttpGet]
        public void Init()
        {
            if (IsNight() && IsMoreThanADay())
            {
                if (_indexNames != null && _indexNames.Any())
                {
                    LogHelper.Info<CamelontaRebuildIndexController>("Camelonta - RebuildIndex started");

                    foreach (var indexName in _indexNames)
                    {
                        var provider = ExamineManager.Instance.IndexProviderCollection[indexName];
                        if (provider != null && NumberOfDeletions(provider) > 0)
                        {
                            LogHelper.Info<CamelontaRebuildIndexController>("Camelonta - Rebuilding index: " + indexName);
                            provider.RebuildIndex();
                        }
                    }

                    UpdateTextFile();
                }
            }
        }

        private bool IsNight()
        {
            return DateTime.Now.Hour == 0;
        }

        private bool IsMoreThanADay()
        {
            var textFileContent = ReadTextFile();
            if (!string.IsNullOrEmpty(textFileContent))
            {
                DateTime lastDate;
                if (DateTime.TryParse(textFileContent, out lastDate))
                {
                    var day = new TimeSpan(0, 23, 0, 0);
                    return DateTime.Now - lastDate > day;
                }

                File.Delete(_filePath);
                return true;
            }

            return true;
        }

        private void UpdateTextFile()
        {
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
            }

            File.WriteAllText(_filePath, DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }

        private string ReadTextFile()
        {
            if (File.Exists(_filePath))
            {
                return File.ReadAllText(_filePath);
            }

            return string.Empty;
        }

        private int NumberOfDeletions(BaseIndexProvider provider)
        {
            var numberOfDeletions = 0;
            var luceneIndexer = provider as LuceneIndexer;
            if (luceneIndexer != null)
            {
                if (luceneIndexer.IndexExists())
                {
                    try
                    {
                        using (var reader = luceneIndexer.GetIndexWriter().GetReader())
                        {
                            numberOfDeletions = reader.NumDeletedDocs();
                        }
                    }

                    catch
                    {
                        LogHelper.Warn<CamelontaRebuildIndexController>("Camelonta - RebuildIndex. Could not detect deletions.");
                    }
                }
            }

            return numberOfDeletions;
        }
    }
}
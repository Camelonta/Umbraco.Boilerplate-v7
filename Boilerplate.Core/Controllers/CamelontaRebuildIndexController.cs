using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using Examine;
using Umbraco.Core.Logging;
using Umbraco.Web.WebApi;

namespace Boilerplate.Core.Controllers
{
    /// <summary>
    /// Rebuilds External index if at night (time is after midnight and before 1 am) and more than a week ago since last time (or first time).
    /// Time is saved in /App_Data/RebuildExternalIndexDateTime.txt.
    /// umbracoSettings.config need to specify this scheduled task to run once per hour. This is inactivated by default.
    /// AppSetting IndexesToRebuild needs to be specified with a csv of the names of indexes included.
    /// </summary>
    public class CamelontaRebuildIndexController: UmbracoApiController
    {
        private string _filePath;
        private string[] _indexNames;

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
            if (IsNight() && IsMoreThanAWeek())
            {
                if (_indexNames != null && _indexNames.Any())
                {
                    LogHelper.Info<CamelontaRebuildIndexController>("Camelonta - RebuildIndex started");

                    foreach (var indexName in _indexNames)
                    {
                        LogHelper.Info<CamelontaRebuildIndexController>("Camelonta - Rebuilding index: " + indexName);
                        var provider = ExamineManager.Instance.IndexProviderCollection[indexName];
                        provider.RebuildIndex();
                    }

                    UpdateTextFile();
                }
            }
        }

        private bool IsNight()
        {
            return true;
            return DateTime.Now.Hour == 0;
        }

        private bool IsMoreThanAWeek()
        {
            var textFileContent = ReadTextFile();
            if (!string.IsNullOrEmpty(textFileContent))
            {
                DateTime lastDate;
                if (DateTime.TryParse(textFileContent, out lastDate))
                {
                    var week = new TimeSpan(7);
                    return DateTime.Now - lastDate > week;
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
    }
}
using System;
using System.Web;
using Examine;
using Examine.Providers;
using Skybrud.Umbraco.GridData;
using Skybrud.Umbraco.GridData.Values;
using Umbraco.Core.Logging;
using System.Text;
using System.Text.RegularExpressions;

namespace Camelonta.Boilerplate.App_Start
{
    public class ExamineIndexer
    {
        public ExamineIndexer()
        {
            BaseIndexProvider externalIndexer = ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"];
            externalIndexer.GatheringNodeData += OnExamineGatheringNodeData;
        }

        public static string GetGridText(string content)
        {
            GridDataModel grid = GridDataModel.Deserialize(content);

            StringBuilder combined = new StringBuilder();

            foreach (GridControl ctrl in grid.GetAllControls())
            {

                switch (ctrl.Editor.Alias)
                {

                    case "rte":
                        {

                            // Get the HTML value
                            string html = ctrl.GetValue<GridControlRichTextValue>().Value;

                            // Strip any HTML tags so we only have text
                            string text = Regex.Replace(html, "<.*?>", "");

                            // Extra decoding may be necessary
                            text = HttpUtility.HtmlDecode(text);

                            // Now append the text
                            combined.AppendLine(text);

                            break;

                        }

                    case "media":
                        {
                            GridControlMediaValue media = ctrl.GetValue<GridControlMediaValue>();
                            combined.AppendLine(media.Caption);
                            break;
                        }

                    case "headline":
                    case "quote":
                        {
                            combined.AppendLine(ctrl.GetValue<GridControlTextValue>().Value);
                            break;
                        }

                }

            }

            return combined.ToString();
        }

        private void OnExamineGatheringNodeData(object sender, IndexingNodeDataEventArgs e)
        {
            try
            {
                //string nodeTypeAlias = e.Fields["nodeTypeAlias"];

                //LogHelper.Info<ExamineIndexer>("Gathering node data for node #" + e.NodeId + " (type: " + nodeTypeAlias + ")");

                //if (nodeTypeAlias == "Home" || nodeTypeAlias == "LandingPage" || nodeTypeAlias == "TextPage" || nodeTypeAlias == "BlogPost")
                {

                    string value;

                    if (e.Fields.TryGetValue("grid", out value))
                    {
                        LogHelper.Info<ExamineIndexer>("Node has \"grid\" value\"");
                        e.Fields["grid"] = GetGridText(e.Fields["grid"]);

                    }
                    else
                    {

                        LogHelper.Info<ExamineIndexer>("Node has no \"grid\" value\"");

                    }

                }

            }
            catch (Exception ex)
            {
                LogHelper.Error<ExamineIndexer>("MAYDAY! MAYDAY! MAYDAY!", ex);
            }
        }

    }
}
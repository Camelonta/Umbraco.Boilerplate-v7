using System;
using Examine;
using Examine.Providers;
using Umbraco.Core.Logging;

namespace Boilerplate.Core.Classes.Search
{
    public class ExamineIndexer
    {
        public ExamineIndexer()
        {
            BaseIndexProvider externalIndexer = ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"];
            externalIndexer.GatheringNodeData += OnExamineGatheringNodeData;
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
                        e.Fields["grid"] = Meta.GetGridText(e.Fields["grid"]);

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
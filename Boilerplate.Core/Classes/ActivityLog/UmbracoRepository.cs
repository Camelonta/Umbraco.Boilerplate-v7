using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using umbraco.BusinessLogic;
using umbraco.DataLayer;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Boilerplate.Core.Classes.ActivityLog
{
    public class UmbracoRepository
    {
        // For the log we retrieve these types
        private readonly static LogTypes[] LogTypeList = { LogTypes.Publish, LogTypes.Save, LogTypes.Delete, LogTypes.UnPublish, LogTypes.Move, LogTypes.Copy, LogTypes.RollBack };

        private readonly static string LogTypesAsString = "'" + string.Join("','", LogTypeList) + "'";
        private readonly DateTime _getLogSince = DateTime.Now.AddDays(-183);

        private static ISqlHelper SqlHelper
        {
            get { return Application.SqlHelper; }
        }

        public IEnumerable<LogItem> GetLatestLogItems(int take, int skip)
        {
            string sqlQuery;

            string connectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString;
            if(connectionString.Contains(".sdf"))
            {
                // SQL Server Compact does not support subquery, use OFFSET instead
                sqlQuery = string.Format("{0} ORDER BY DateStamp DESC OFFSET (@skip) ROWS FETCH NEXT (@take) ROWS ONLY", GetInnerSelect(false));
            }
            else
            {
                // SQL Server 2005/2008/2008 R2 does not support OFFSET, but subquery
                sqlQuery = string.Format(";WITH [logResult] AS ({0}) SELECT * FROM [logResult] {1}", GetInnerSelect(), GetWhere());
            }

            return LogItem.ConvertIRecordsReader(SqlHelper.ExecuteReader(sqlQuery,
            SqlHelper.CreateParameter("@dateStamp", _getLogSince),
             SqlHelper.CreateParameter("@skip", GetSkip(skip)),
             SqlHelper.CreateParameter("@take", GetTake(take, skip))));
        }

        private string GetInnerSelect(bool includeRowNumber = true)
        {
            string rowNumberColumn = string.Empty;
            if(includeRowNumber)
            {
                rowNumberColumn = ", ROW_NUMBER() OVER (ORDER BY Datestamp DESC) AS rowNumber";
            }
            return string.Format("SELECT userId, NodeId, DateStamp, logHeader, logComment{1} FROM [umbracoLog] {0}", GetInnerWhere(), rowNumberColumn);
        }

        private string GetInnerWhere()
        {
            return string.Format("WHERE {0} AND DateStamp >= @dateStamp AND NodeId != -1", GetLogCommentSqlQuery());
        }

        private string GetWhere()
        {
            return string.Format("WHERE rowNumber BETWEEN @skip AND @take");
        }

        private int GetSkip(int skip)
        {
            if(skip > 0)
            {
                // Because we are using "BETWEEN" in SQL-query
                skip++;
            }
            return skip;
        }

        private int GetTake(int take, int skip)
        {
            return take + skip;
        }

        private string GetLogCommentSqlQuery()
        {
            string sql = "logHeader IN (" + LogTypesAsString + ") AND (";
            sql += "logComment LIKE 'Save and Publish %' OR ";

            sql += "logComment LIKE 'UnPublish %' OR ";

            sql += string.Format("logComment LIKE 'UmbracoForm%' OR ");

            var logTypes = Enum.GetNames(typeof (LogTypes));
            foreach (var logType in logTypes)
            {
                sql += "(";
                sql += string.Format("logComment LIKE '{0} Content %'", logType);
                sql += string.Format(" OR logComment LIKE '{0} Media %'", logType);
                sql += ")";

                bool isLastItem = logType.Equals(logTypes.Last());
                if (!isLastItem)
                {
                    sql += " OR ";
                }
            }
            return sql + ")";
        }

        // Total count of log items for the pagination
        public int CountLogItems()
        {
            return ApplicationContext.Current.DatabaseContext.Database.ExecuteScalar<int>(string.Format("SELECT COUNT(*) FROM umbracoLog WHERE {0}", GetLogCommentSqlQuery()));
        }

        public IEnumerable<IContent> GetRecycleBinNodes()
        {
            return UmbracoContext.Current.Application.Services.ContentService.GetContentInRecycleBin();
        }

    }
}
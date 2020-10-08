using System;
using System.Collections.Generic;

using VNC;
using VNC.Core;

namespace SupportTools_Excel.AzureDevOpsExplorer.Domain
{
    public class WorkItemQuery
    {
        #region Properties

        public string Name { get; set; }
        // NOTE(crhodes)
        // We can set a default here or do in ViewModel PopulateWorkItemQueries
        public string QueryWithTokens { get; set; }
        public List<string> Fields { get; set; }
        public string Query { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Maybe this should be PrepareQuery
        /// </summary>
        /// <param name="options"></param>
        /// <param name="projectName"></param>
        public void ReplaceQueryTokens(
            Options_AZDO_TFS options,
            string projectName = null)
        {
            Query = QueryWithTokens;

            // TODO(crhodes)
            // Until we better think through how to handle looping delays across Projects
            // Support both the @PROJECT token and TeamProjectFilter

            if (projectName != null)
            {
                Query = Query.Replace("@PROJECT", $"{projectName}");
            }
            else
            {
                if ((options.TeamProjects?.Count ?? 0) > 0)
                {
                    Query += GetTeamProjectsFilter(options);
                }
            }

            Query = Query.Replace("@STARTDATE", options.StartDate.ToShortDateString());
            Query = Query.Replace("@ENDDATE", options.EndDate.ToShortDateString());

            if ((options.WorkItemTypes?.Count ?? 0) > 0)
            {
                Query += GetWorkItemTypesFilter(options);
            }
        }

        private string GetWorkItemTypesFilter(Options_AZDO_TFS options)
        {
            string filter;

            if (options.WorkItemTypes.Count == 1)
            {
                filter = " AND [System.WorkItemType] == " + $"{ options.WorkItemTypes[0].WrapInSngQuotes() }";
            }
            else
            {
                filter = " AND ([System.WorkItemType] in (" + String.Join(",", options.WorkItemTypes) + ")";
            }

            return filter;
        }

        private string GetTeamProjectsFilter(Options_AZDO_TFS options)
        {
            string filter;

            if (options.TeamProjects.Count == 1)
            {
                filter = " AND [System.TeamProject] == " + $"{ options.TeamProjects[0].WrapInSngQuotes() }";
            }
            else
            {
                filter = " AND ([System.TeamProject] in (" + String.Join(",", options.TeamProjects) + ")";
            }

            return filter;
        }

        #endregion
    }
}

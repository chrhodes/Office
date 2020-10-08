using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Office.Interop.Excel;

using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using SupportTools_Excel.Domain;
using VNC.AddinHelper;
using XlHlp = VNC.AddinHelper.Excel;
using SupportTools_Excel.AzureDevOpsExplorer.Application;
using SupportTools_Excel.Presentation.Views;
using SupportTools_Excel.AzureDevOpsExplorer.Domain;
using SupportTools_Excel.AzureDevOpsExplorer.Presentation.Views;
using System.Threading;

namespace SupportTools_Excel.AzureDevOpsExplorer.Application
{
    class AZDOHelper
    {
        internal static void InsertItemDelay(Options_AZDO_TFS options)
        {
            if (options.ItemDelaySeconds > 0)
            {
                Thread.Sleep(Convert.ToInt16(options.ItemDelaySeconds * 1000));
            }
        }

        internal static void DisplayLoopUpdates(long startTicks, Options_AZDO_TFS options, int totalItems, int itemCount)
        {
            if (itemCount % options.LoopUpdateInterval == 0)
            {
                XlHlp.DisplayInWatchWindow($"Completed {itemCount} out of {totalItems}", startTicks);
            }
        }

        internal static void ProcessLoopDelay(Options_AZDO_TFS options)
        {
            if (options.EnableDelays && options.LoopDelaySeconds > 0)
            {
                Thread.Sleep(options.LoopDelaySeconds * 1000);
            }
        }

        internal static void ProcessItemDelay(Options_AZDO_TFS options)
        {
            if (options.EnableDelays && options.ItemDelaySeconds > 0)
            {
                Thread.Sleep(Convert.ToInt16(options.ItemDelaySeconds * 1000));
            }
        }

        // TODO(crhodes)
        // Make this understand @PROJECT
        // @PROJECTS
        // @STARTDATE
        // @ENDDATE
        // @STATES
        // @WORKITEMTYPES

        private static string GetWorkItemTypesFilter(Options_AZDO_TFS options)
        {
            string filter;

            if (options.WorkItemTypes.Count == 1)
            {
                filter = "AND ([System.WorkItemType] == '${options.WorkItemTypes[0]}";
            }
            else
            {
                filter = "AND ([System.WorkItemType] in " + String.Join(",", options.WorkItemTypes);
            }

            return filter;
        }

        private static string GetTeamProjectsFilter(Options_AZDO_TFS options)
        {
            string filter;

            if (options.TeamProjects.Count == 1)
            {
                filter = "AND ([System.TeamProject] == '${options.TeamProjects[0]}";
            }
            else
            {
                filter = "AND ([System.TeamProject] in " + String.Join(",", options.TeamProjects);
            }

            return filter;
        }


        internal static string ParseQueryTokens(
            string tokenizedQuery,
            Options_AZDO_TFS options)
        {
            string query = tokenizedQuery;

            query = query.Replace("@STARTDATE", options.StartDate.ToShortDateString());
            query = query.Replace("@ENDDATE", options.EndDate.ToShortDateString());

            if (options.TeamProjects.Count > 0)
            {
                query += GetTeamProjectsFilter(options);
            }

            if (options.WorkItemTypes.Count > 0)
            {
                query += GetWorkItemTypesFilter(options);
            }

            // NOTE(crhodes)
            // Have moved to startDate and endDate.  No one should be using GoBackDays, but check in Excel Template file (query).
            //query = query.Replace("@goBackDays", options.GoBackDays.ToString());

            return query;
        }

        internal static string ParseQueryTokens(
            string tokenizedQuery,
            Options_AZDO_TFS options,
            Project project)
        {
            string query = tokenizedQuery;

            if (project != null)
            {
                query = query.Replace("@PROJECT", String.Format("{0}", project.Name));
            }

            query = query.Replace("@STARTDATE", options.StartDate.ToShortDateString());
            query = query.Replace("@ENDDATE", options.EndDate.ToShortDateString());

            if (options.WorkItemTypes.Count > 0)
            {
                query += GetWorkItemTypesFilter(options);
            }

            // NOTE(crhodes)
            // Have moved to startDate and endDate.  No one should be using GoBackDays, but check in Excel Template file (query).
            //query = query.Replace("@goBackDays", options.GoBackDays.ToString());

            return query;
        }

        internal static void FetchIdentities(IdentityDescriptor[] descriptors,
            List<TeamFoundationIdentity> globalGroups,
            Dictionary<IdentityDescriptor, TeamFoundationIdentity> globalIdentities)
        {
            long startTicks = XlHlp.DisplayInWatchWindow("Begin");

            TeamFoundationIdentity[] identities;

            // If total membership exceeds batch size limit for Read, break it up
            int batchSizeLimit = 100000;

            if (descriptors.Length > batchSizeLimit)
            {
                int batchNum = 0;
                int remainder = descriptors.Length;
                IdentityDescriptor[] batchDescriptors = new IdentityDescriptor[batchSizeLimit];

                while (remainder > 0)
                {
                    int startAt = batchNum * batchSizeLimit;
                    int length = batchSizeLimit;
                    if (length > remainder)
                    {
                        length = remainder;
                        batchDescriptors = new IdentityDescriptor[length];
                    }

                    Array.Copy(descriptors, startAt, batchDescriptors, 0, length);
                    identities = AzureDevOpsExplorer.Presentation.Views.Server.IdentityManagementService.ReadIdentities(batchDescriptors, MembershipQuery.Direct, ReadIdentityOptions.None);
                    SortIdentities(identities, globalGroups, globalIdentities);
                    remainder -= length;
                }
            }
            else
            {
                identities = AzureDevOpsExplorer.Presentation.Views.Server.IdentityManagementService.ReadIdentities(descriptors, MembershipQuery.Direct, ReadIdentityOptions.None);
                SortIdentities(identities, globalGroups, globalIdentities);
            }

            XlHlp.DisplayInWatchWindow("End", startTicks);
        }

        internal static void SortIdentities(TeamFoundationIdentity[] identitiesToAdd,
            List<TeamFoundationIdentity> _Groups,
            Dictionary<IdentityDescriptor, TeamFoundationIdentity> _Identities)
        {
            foreach (TeamFoundationIdentity identity in identitiesToAdd)
            {
                _Identities[identity.Descriptor] = identity;

                if (identity.IsContainer)
                {
                    _Groups.Add(identity);
                }
            }
        }
    }
}

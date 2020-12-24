using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

using SupportTools_Excel.AzureDevOpsExplorer.Domain;

using SupportTools_Excel.Domain;

using VNC;

using XlHlp = VNC.AddinHelper.Excel;

namespace SupportTools_Excel.AzureDevOpsExplorer.Application
{
    class Body_VersionControlServer
    {
        #region Version Control Server (VCS)

        internal static void Add_TP_Changesets(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            VersionControlServer versionControlServer,
            TeamProject teamProject)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            //TeamProject teamProject = VNC.TFS.Helper.Get_TeamProject(versionControlServer, teamProjectName);

            var path = teamProject.ServerItem;

            //var queryHistory = Server.VersionControlServer.QueryHistory(
            //    teamProject.ServerItem,
            //    VersionSpec.Latest,
            //    0,
            //    RecursionType.Full,
            //    null,
            //    VersionSpec.Latest,
            //    VersionSpec.Latest,
            //    Int32.MaxValue,
            //    true,
            //    true,
            //    false,
            //    false);

            var queryHistory = versionControlServer.QueryHistory(
                teamProject.ServerItem,
                VersionSpec.Latest,
                0,
                RecursionType.Full,
                null,
                null,
                null,
                Int32.MaxValue,
                true,
                true,
                false,
                false);

            XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), teamProject.Name);

            try
            {
                Changeset lastestChangeset = queryHistory.Cast<Changeset>().First();

                string lastChangesetId = lastestChangeset.ChangesetId.ToString();
                string lastChangeSetCreationDate = lastestChangeset.CreationDate.ToString();

                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), lastChangesetId);
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), lastChangeSetCreationDate);
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), teamProject.VersionControlServer.SupportedFeatures.ToString());
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), teamProject.VersionControlServer.WebServiceLevel.ToString());
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message.Equals("Sequence contains no elements"))
                {
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), "No Changesets");
                }
                else
                {
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), ioe.ToString());
                }

            }
            catch (Exception ex)
            {
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), ex.ToString());
            }

            insertAt.IncrementRows();

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_TP_Changesets(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            ICommonStructureService commonStructureService,
            bool listChanges, bool listWorkItems, IEnumerable history)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            foreach (Changeset changeset in history)
            {
                insertAt.ClearOffsets();

                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), changeset.ChangesetId.ToString());
                //XlHlp.AddContentToCell(insertAt.AddOffsetColumn(), changeset.CheckinNote.ToString());
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), changeset.Committer);
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), changeset.CommitterDisplayName);
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), changeset.Owner);
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), changeset.OwnerDisplayName);
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), changeset.CreationDate.ToString());
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), changeset.CheckinNote.ToString());
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), changeset.Comment);
                //XlHlp.AddContentToCell(insertAt.AddOffsetColumn(), changeset.AssociatedWorkItems.Count().ToString());
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), changeset.Changes.Count().ToString());
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), changeset.WorkItems.Count().ToString());
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), changeset.AssociatedWorkItems.Count().ToString());

                insertAt.IncrementRows();

                if (listChanges)
                {
                    insertAt.IncrementColumns();

                    foreach (Change change in changeset.Changes)
                    {
                        try
                        {
                            XlHlp.AddContentToCell(insertAt.AddRowX(1), Section_VersionControlServer.GetChangeInfo(change));
                            //XlHlp.AddContentToCell(insertAt.AddRow(), GetIterationInfo(workItem));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }

                    insertAt.DecrementColumns();
                }

                if (listWorkItems)
                {
                    insertAt.IncrementColumns();

                    foreach (WorkItem workItem in changeset.WorkItems)
                    {
                        try
                        {
                            XlHlp.AddContentToCell(insertAt.AddRowX(1), Section_VersionControlServer.GetWorkItemInfo(workItem));
                            XlHlp.AddContentToCell(insertAt.AddRowX(1), Section_VersionControlServer.GetIterationInfo(workItem, commonStructureService));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }

                    insertAt.DecrementColumns();
                }
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_TP_Developers(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            string teamProjectName,
            SortedDictionary<string, int> developers,
            SortedDictionary<string, DateTime> developersLatestDate,
            SortedDictionary<string, DateTime> developersEarliestDate)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            foreach (string developer in developers.Keys)
            {
                insertAt.ClearOffsets();

                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), teamProjectName);
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), developer);
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), developers[developer].ToString());
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), developersEarliestDate[developer].ToString());
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), developersLatestDate[developer].ToString());

                insertAt.IncrementRows();
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_TP_Shelvesets(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            Shelveset[] shelvesets)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            try
            {
                foreach (Shelveset item in shelvesets)
                {
                    insertAt.ClearOffsets();

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), item.OwnerDisplayName);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), item.OwnerName);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), item.Name);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), item.CreationDate.ToString());
                    //ExcelHlp.AddContentToCell(insertAt.AddOffsetColumn(), item.DisplayName);
                    //ExcelHlp.AddContentToCell(insertAt.AddOffsetColumn(), item.QualifiedName);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), item.CheckinNote.ToString());
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), item.Comment);

                    insertAt.IncrementRows();
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0} - {1}", "TP", ex.ToString());

                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), msg);
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_TP_Teams(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            VersionControlServer versionControlServer,
            TeamProject teamProject,
            IEnumerable<TeamFoundationTeam> allTeams,
            TeamFoundationTeam defaultTeam)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            foreach (var team in allTeams.OrderBy(team => team.Name))
            {
                insertAt.ClearOffsets();

                TeamFoundationIdentity[] teamMembers = team.GetMembers(versionControlServer.TeamProjectCollection, MembershipQuery.Expanded);

                foreach (var member in teamMembers.OrderBy(m => m.UniqueName))
                {
                    insertAt.ClearOffsets();

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), teamProject.Name);

                    // Team 

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), team.Name);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), team.Description);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(),
                        defaultTeam.Name.Equals(team.Name) ? "*" : "");

                    // Members

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), member.DisplayName);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), member.UniqueName);

                    insertAt.IncrementRows();
                }
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_TP_Workspaces(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            Workspace[] workSpaces)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            foreach (Workspace workspace in workSpaces)
            {
                insertAt.ClearOffsets();

                // Keep in same order with headers, supra.

                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), workspace.Computer);
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), workspace.Name);
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), workspace.OwnerDisplayName);
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), workspace.OwnerName);
                //ExcelHlp.AddContentToCell(rngOutput.Offset[currentRow, col++], workspace.DisambiguatedDisplayName);
                //ExcelHlp.AddContentToCell(rngOutput.Offset[currentRow, col++], workspace.DisplayName);
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), workspace.LastAccessDate.ToString());
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), workspace.Comment);
                //ExcelHlp.AddContentToCell(rngOutput.Offset[XlLocation.Rows, col++], workspace.QualifiedName);

                insertAt.IncrementRows();
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using Microsoft.Office.Interop.Excel;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

using SupportTools_Excel.AzureDevOpsExplorer.Domain;

using XlHlp = VNC.AddinHelper.Excel;

namespace SupportTools_Excel.AzureDevOpsExplorer.Application
{
    class Section_TeamProject
    {
        #region Team Project (TP)

        private static List<TeamFoundationIdentity> _TeamProject_Groups =
            new List<TeamFoundationIdentity>();

        // For Team Project
        private static Dictionary<IdentityDescriptor, TeamFoundationIdentity> _TeamProject_Identities =
            new Dictionary<IdentityDescriptor, TeamFoundationIdentity>(IdentityDescriptorComparer.Instance);

        internal static XlHlp.XlLocation AddSections(
            XlHlp.XlLocation insertAt,
            TeamProject teamProject,
            List<string> sectionsToDisplay)
        {
            long startTicks = XlHlp.DisplayInWatchWindow(insertAt);

            if (sectionsToDisplay.Count != 0)
            {

                if (insertAt.OrientVertical)
                {
                    XlHlp.AddContentToCell(insertAt.AddRowX(), "TeamProject (TP) Information");
                }
                else
                {
                    XlHlp.AddContentToCell(insertAt.AddRowX(), "TeamProject (TP) Information");
                    insertAt.DecrementRows();   // AddRow bumped it.
                    insertAt.IncrementColumns();
                }

                if (sectionsToDisplay.Contains("Info"))
                {
                    insertAt = Add_Info(insertAt, teamProject).IncrementPosition(insertAt.OrientVertical);
                }

                if (sectionsToDisplay.Contains("Members"))
                {
                    insertAt = Add_Members(insertAt, teamProject).IncrementPosition(insertAt.OrientVertical);
                }
            }

            XlHlp.DisplayInWatchWindow(insertAt, startTicks, "End");

            return insertAt;
        }

        internal static XlHlp.XlLocation Add_Members(
            XlHlp.XlLocation insertAt,
            TeamProject teamProject)
        {
            long startTicks = XlHlp.DisplayInWatchWindow(insertAt);

            try
            {
                int currentRows = insertAt.RowsAdded;

                // Save the location of the count so we can update later after have traversed all items.

                Range rngTitle = insertAt.GetCurrentRange();

                if (insertAt.OrientVertical)
                {
                    XlHlp.AddLabeledInfo(insertAt.AddRow(), "Members Group", "");
                }
                else
                {
                    XlHlp.AddLabeledInfo(insertAt.AddRow(), "Members Group", "", orientation: XlOrientation.xlUpward);
                    insertAt.IncrementColumns();
                }

                TeamFoundationIdentity[] projectGroups = AzureDevOpsExplorer.Presentation.Views.Server.IdentityManagementService.ListApplicationGroups(
                    teamProject.ArtifactUri.AbsoluteUri, ReadIdentityOptions.None);

                Dictionary<IdentityDescriptor, object> descriptorSet = new Dictionary<IdentityDescriptor, object>(IdentityDescriptorComparer.Instance);

                foreach (TeamFoundationIdentity projectGroup in projectGroups)
                {
                    descriptorSet[projectGroup.Descriptor] = projectGroup.Descriptor;
                }

                // Expanded membership of project groups
                projectGroups = AzureDevOpsExplorer.Presentation.Views.Server.IdentityManagementService.ReadIdentities(descriptorSet.Keys.ToArray(), MembershipQuery.Expanded, ReadIdentityOptions.None);

                // Collect all descriptors
                foreach (TeamFoundationIdentity projectGroup in projectGroups)
                {
                    foreach (IdentityDescriptor mem in projectGroup.Members)
                    {
                        descriptorSet[mem] = mem;
                    }
                }

                // NOTE(crhodes)
                // Might need to ensure that _Global_Groups and _Global_Identities already populated.


                if (Section_TeamProjectCollection._Global_Identities.Count == 0)
                {
                    TeamFoundationIdentity everyoneExpanded = AzureDevOpsExplorer.Presentation.Views.Server.IdentityManagementService.ReadIdentity(
                        GroupWellKnownDescriptors.EveryoneGroup, MembershipQuery.Expanded, ReadIdentityOptions.None);
                    AZDOHelper.FetchIdentities(everyoneExpanded.Members, Section_TeamProjectCollection._Global_Groups, Section_TeamProjectCollection._Global_Identities);
                }


                _TeamProject_Groups.Clear();
                _TeamProject_Identities.Clear();

                AZDOHelper.FetchIdentities(descriptorSet.Keys.ToArray(), _TeamProject_Groups, _TeamProject_Identities);

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);
                // Keep in same order as fields, infra.

                // Group

                XlHlp.AddColumnHeaderToSheet(insertAt.AddOffsetColumn(), 50, "Identifier");
                XlHlp.AddColumnHeaderToSheet(insertAt.AddOffsetColumn(), 50, "Identity");

                // Members

                XlHlp.AddColumnHeaderToSheet(insertAt.AddOffsetColumn(), 15, "IsContainer");
                XlHlp.AddColumnHeaderToSheet(insertAt.AddOffsetColumn(), 50, "DisplayName");
                XlHlp.AddColumnHeaderToSheet(insertAt.AddOffsetColumn(), 80, "UniqueName");
                XlHlp.AddColumnHeaderToSheet(insertAt.AddOffsetColumn(), 40, "IdentityType");
                XlHlp.AddColumnHeaderToSheet(insertAt.AddOffsetColumn(), 20, "UniqueUserId");
                XlHlp.AddColumnHeaderToSheet(insertAt.AddOffsetColumn(), 10, "IsActive");

                insertAt.IncrementRows();

                foreach (TeamFoundationIdentity identity in _TeamProject_Groups)
                {
                    foreach (IdentityDescriptor member in identity.Members)
                    {
                        insertAt.ClearOffsets();

                        try
                        {
                            // Group

                            XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), identity.Descriptor.Identifier);
                            XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), identity.DisplayName);

                            // Members

                            // NOTE(crhodes)
                            // This line is throwing exception.  Why?

                            XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), Section_TeamProjectCollection._Global_Identities[member].IsContainer.ToString());
                            XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), Section_TeamProjectCollection._Global_Identities[member].DisplayName);
                            XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), Section_TeamProjectCollection._Global_Identities[member].UniqueName);
                            XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), Section_TeamProjectCollection._Global_Identities[member].Descriptor.IdentityType);
                            XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), Section_TeamProjectCollection._Global_Identities[member].UniqueUserId.ToString());
                            XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), Section_TeamProjectCollection._Global_Identities[member].IsActive.ToString());

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }

                        insertAt.IncrementRows();
                    }
                }

                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblTPMembers_{0}", insertAt.workSheet.Name));

                insertAt.Group(insertAt.OrientVertical);

                // Update counts.  -2 covers Header and Table Column Header

                if (insertAt.OrientVertical)
                {
                    XlHlp.AddLabeledInfoX(rngTitle, "Members Group", (insertAt.RowsAdded - currentRows - 2).ToString());
                }
                else
                {
                    XlHlp.AddLabeledInfoX(rngTitle, "Members Group", (insertAt.RowsAdded - currentRows - 2).ToString(), orientation: XlOrientation.xlUpward);
                }

                insertAt.EndSectionAndSetNextLocation(insertAt.OrientVertical);

                //insertAt.AddRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            XlHlp.DisplayInWatchWindow(insertAt, startTicks, "End");

            return insertAt;
        }

        internal static XlHlp.XlLocation AddSection_TeamProjects_Info(
            XlHlp.XlLocation insertAt,
            ReadOnlyCollection<CatalogNode> teamProjects)
        {
            long startTicks = XlHlp.DisplayInWatchWindow(insertAt);

            XlHlp.AddLabeledInfo(insertAt.AddRow(), "Team Projects", teamProjects.Count().ToString());

            Worksheet ws = insertAt.workSheet;

            insertAt = DisplayListOf_TeamProjects(insertAt, teamProjects, displayDataOnly: false, string.Format("tblTP_{0}", ws.Name));

            if (!insertAt.OrientVertical)
            {
                // Skip past the info just added.
                insertAt.SetLocation(insertAt.RowStart, insertAt.TableEndColumn + 1);
            }

            return insertAt;
        }

        internal static XlHlp.XlLocation Add_Info(
            XlHlp.XlLocation insertAt,
            TeamProject teamProject)
        {
            long startTicks = XlHlp.DisplayInWatchWindow(insertAt);

            insertAt.MarkStart();

            XlHlp.AddLabeledInfo(insertAt.AddRow(2), "TP Name", teamProject.Name);
            XlHlp.AddLabeledInfo(insertAt.AddRow(2), "AbsoluteUri", teamProject.ArtifactUri.AbsoluteUri);
            XlHlp.AddLabeledInfo(insertAt.AddRow(2), "ServerItem", teamProject.ServerItem);
            XlHlp.AddLabeledInfo(insertAt.AddRow(2), "VCS ServerGuid", teamProject.VersionControlServer.ServerGuid.ToString());

            insertAt.MarkEnd();

            if (!insertAt.OrientVertical)
            {
                // Skip past the info just added.
                insertAt.SetLocation(insertAt.RowStart, insertAt.MarkEndColumn + 1);
            }

            XlHlp.DisplayInWatchWindow(insertAt, startTicks, "End");

            return insertAt;
        }

        internal static XlHlp.XlLocation DisplayListOf_TeamProjects(XlHlp.XlLocation insertAt,
            ReadOnlyCollection<CatalogNode> projectNodes, bool displayDataOnly, string tableSuffix)
        {
            long startTicks = XlHlp.DisplayInWatchWindow(insertAt);

            if (!displayDataOnly)
            {
                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                //XlHlp.AddTitledInfo(insertAt.AddRow(), "Name", teamProjects.Count.ToString());
                //XlHlp.AddContentToCell(insertAt.AddOffsetColumn(), Name, 12, XlHlp.MakeBold.Yes);
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 25, "DisplayName");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 35, "Description");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 35, "Identifier");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 35, "ProjectId");
                //XlHlp.AddColumnHeaderToSheet(insertAt.AddOffsetColumn(), 25, "ProjectName", 12);
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 25, "ProjectState");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 62, "ProjectUri");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 25, "SCC");


                //XlHlp.AddTitledInfo(insertAt.AddRow(2), "TP Name", teamProject.Name);
                //XlHlp.AddTitledInfo(insertAt.AddRow(2), "AbsoluteUri", teamProject.ArtifactUri.AbsoluteUri);
                //XlHlp.AddTitledInfo(insertAt.AddRow(2), "ServerItem", teamProject.ServerItem);
                //XlHlp.AddTitledInfo(insertAt.AddRow(2), "VCS ServerQuid", teamProject.VersionControlServer.ServerGuid.ToString());

                insertAt.IncrementRows();
            }
            // The columns in this method need to be kept in sync with CreateTeamProjectsInfo()

            foreach (CatalogNode projectNode in projectNodes.OrderBy(tp => tp.Resource.DisplayName))
            {
                insertAt.ClearOffsets();

                try
                {
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), projectNode.Resource.DisplayName);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), projectNode.Resource.Description);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), projectNode.Resource.Identifier.ToString());
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), projectNode.Resource.Properties["ProjectId"]);
                    //XlHlp.AddContentToCell(insertAt.AddOffsetColumn(), projectNode.Resource.Properties["ProjectName"]);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), projectNode.Resource.Properties["ProjectState"]);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), projectNode.Resource.Properties["ProjectUri"]);

                    string sccType = "??";

                    if (projectNode.Resource.Properties.Keys.Contains("SourceControlCapabilityFlags"))
                    {
                        switch (int.Parse(projectNode.Resource.Properties["SourceControlCapabilityFlags"]))
                        {
                            case 0:
                                sccType = "NONE";
                                break;

                            case 1:
                                sccType = "TFS";
                                break;

                            case 2:
                                sccType = "GIT";
                                break;

                            case 3:
                                sccType = "TFS/GIT";
                                break;

                            default:
                                break;

                        }
                    }

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), sccType);
                }
                catch (Exception ex)
                {

                }


                //projectNode.FullPath
                //    projectNode.Resource.Description
                //    projectNode.Resource.Identifier


                insertAt.IncrementRows();
            }

            if (!displayDataOnly)
            {
                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblTP_{0}", tableSuffix));

                insertAt.Group(insertAt.OrientVertical, hide: true);
            }

            XlHlp.DisplayInWatchWindow(insertAt, startTicks, "End");

            return insertAt;
        }

        #endregion

    }
}

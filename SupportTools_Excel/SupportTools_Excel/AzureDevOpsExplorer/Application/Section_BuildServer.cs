using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Office.Interop.Excel;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.VersionControl.Client;
using SupportTools_Excel.AzureDevOpsExplorer.Domain;
using SupportTools_Excel.Domain;

using XlHlp = VNC.AddinHelper.Excel;

namespace SupportTools_Excel.AzureDevOpsExplorer.Application
{
    class Section_BuildServer
    {
        #region Build Server (BS)

        public delegate void ProcessAddBodyCommand_BS(
            XlHlp.XlLocation insertAt,
            IBuildServer buildServer,
            TeamProject teamProject);

        internal static XlHlp.XlLocation ProcessAddSectionCommand_BuildServer(
            XlHlp.XlLocation insertAt,
            IBuildServer buildServer,
            TeamProject teamProject,
            string sectionTitle,
            RequestHandlers.ProcessAddHeaderCommand addHeaderCommand,
            ProcessAddBodyCommand_BS addBodyCommand,
            string tablePrefix)
        {
            long startTicks = XlHlp.DisplayInWatchWindow(insertAt);

            try
            {
                // Save the location of the title so we can update later after have traversed all items.

                Range rngTitle = insertAt.GetCurrentRange();

                if (insertAt.OrientVertical)
                {
                    XlHlp.AddSectionInfo(insertAt.AddRow(), sectionTitle, "");
                }
                else
                {
                    XlHlp.AddSectionInfo(insertAt.AddRow(), sectionTitle, "",
                        orientation: XlOrientation.xlUpward);
                    insertAt.IncrementColumns();
                }

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                addHeaderCommand(insertAt);

                addBodyCommand(insertAt, buildServer, teamProject);

                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("{0}_{1}", tablePrefix, teamProject.Name));

                insertAt.Group(insertAt.OrientVertical);

                insertAt.EndSectionAndSetNextLocation(insertAt.OrientVertical);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            XlHlp.DisplayInWatchWindow("End: " + DateTime.Now);
            XlHlp.DisplayInWatchWindow(insertAt, startTicks, "End");

            return insertAt;
        }

        internal static XlHlp.XlLocation AddSections(XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            IBuildServer buildServer,
            TeamProject teamProject,
            List<string> sectionsToDisplay)
        {
            long startTicks = XlHlp.DisplayInWatchWindow(insertAt);

            if (sectionsToDisplay.Count > 0)
            {
                if (insertAt.OrientVertical)
                {
                    XlHlp.AddSectionInfo(insertAt.AddRow(), "Build Server (BS) Information", "");
                }
                else
                {
                    XlHlp.AddSectionInfo(insertAt.AddRow(), "Build Server (BS) Information", "",
                        orientation: XlOrientation.xlUpward);
                    insertAt.DecrementRows();   // AddRow bumped it.
                    insertAt.IncrementColumns();
                }

                //_buildServer.QueryBuildAgents;
                //_buildServer.QueryBuildControllers;
                //_buildServer.QueryBuildDefinitions;
                //_buildServer.QueryBuilds;
                //_buildServer.QueryBuildServiceHosts;
                //_buildServer.QueryProcessTemplates;

                insertAt = Add_Info(insertAt);

                if (sectionsToDisplay.Contains("Build Agents"))
                {
                    insertAt = Add_BuildAgents(insertAt, options, buildServer, teamProject).IncrementPosition(insertAt.OrientVertical);
                }

                if (sectionsToDisplay.Contains("Build Controllers"))
                {
                    insertAt = Add_BuildControllers(insertAt, options, buildServer, teamProject).IncrementPosition(insertAt.OrientVertical);
                }

                if (sectionsToDisplay.Contains("Build Definitions"))
                {
                    insertAt = Add_BuildDefinitions(insertAt, options, buildServer, teamProject).IncrementPosition(insertAt.OrientVertical);
                }

                if (sectionsToDisplay.Contains("Builds"))
                {
                    insertAt = Add_Builds(insertAt, options, buildServer, teamProject).IncrementPosition(insertAt.OrientVertical);
                }

                if (sectionsToDisplay.Contains("Build ServiceHosts"))
                {
                    insertAt = Add_BuildServiceHosts(insertAt, options, buildServer, teamProject).IncrementPosition(insertAt.OrientVertical);
                }

                if (sectionsToDisplay.Contains("Build ProcessTemplates"))
                {
                    insertAt = Add_BuildProcessTemplates(insertAt, options, buildServer, teamProject).IncrementPosition(insertAt.OrientVertical);
                }
            }

            XlHlp.DisplayInWatchWindow(insertAt, startTicks, "End");

            return insertAt;
        }

        internal static XlHlp.XlLocation Add_Info(XlHlp.XlLocation insertAt)
        {
            long startTicks = XlHlp.DisplayInWatchWindow(insertAt);

            try
            {
                // TODO(crhodes)
                // Add Dummy output
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            XlHlp.DisplayInWatchWindow(insertAt, startTicks, "End");

            return insertAt;
        }

        internal static XlHlp.XlLocation Add_BuildAgents(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            IBuildServer buildServer,
            TeamProject teamProject)
        {
            return ProcessAddSectionCommand_BuildServer(insertAt, 
                buildServer, teamProject,
                "Build Agents", Header_BuildServer.Add_BuildAgents, 
                (insertAt, buildServer, teamProject) => Body_BuildServer.Add_BuildAgents(insertAt, options, buildServer, teamProject), 
                "tblBA_");
        }

        internal static XlHlp.XlLocation Add_BuildControllers(XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            IBuildServer buildServer,
            TeamProject teamProject)
        {
            return ProcessAddSectionCommand_BuildServer(insertAt, 
                buildServer, teamProject,
                "Build Controllers", Header_BuildServer.Add_BuildAgents,
                (insertAt, buildServer, teamProject) => Body_BuildServer.Add_BuildAgents(insertAt, options, buildServer, teamProject),
                "tblBC_");
        }

        internal static XlHlp.XlLocation Add_BuildDefinitions(XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
                        IBuildServer buildServer,
            TeamProject teamProject)
        {
            return ProcessAddSectionCommand_BuildServer(insertAt, 
                buildServer, teamProject,
                "Build Definitions", Header_BuildServer.Add_BuildAgents,
                (insertAt, buildserver, teamProject) => Body_BuildServer.Add_BuildAgents(
                    insertAt, options, buildServer, teamProject), 
                "tblBD_");
        }

        internal static XlHlp.XlLocation Add_Builds(XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
                        IBuildServer buildServer,
            TeamProject teamProject)
        {
            return ProcessAddSectionCommand_BuildServer(insertAt, 
                buildServer, teamProject,
                "Builds", Header_BuildServer.Add_BuildAgents,
                (insertAt, buildServer, teamProject) => Body_BuildServer.Add_BuildAgents(
                    insertAt, options, buildServer, teamProject),
                "tblBlds_");
        }

        internal static XlHlp.XlLocation Add_BuildServiceHosts(XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            IBuildServer buildServer,
            TeamProject teamProject)
        {
            return ProcessAddSectionCommand_BuildServer(insertAt, 
                buildServer, teamProject,
                "Build ServiceHosts", Header_BuildServer.Add_BuildAgents, 
                (insertAt, buildServer, teamProject) => Body_BuildServer.Add_BuildAgents(
                    insertAt, options, buildServer, teamProject), 
                "tblBSH_");
        }

        internal static XlHlp.XlLocation Add_BuildProcessTemplates(XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            IBuildServer buildServer,
            TeamProject teamProject)
        {
            return ProcessAddSectionCommand_BuildServer(insertAt, 
                buildServer, teamProject,
                "Build ProcessTemplates", Header_BuildServer.Add_BuildAgents, 
                (insertAt, buildServer, teamProject) => Body_BuildServer.Add_BuildAgents(insertAt, options, buildServer, teamProject), 
                "tblBPT_");
        }

        #endregion
    }
}

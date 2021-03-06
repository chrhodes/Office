﻿using System;

using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

using SupportTools_Excel.Domain;
using SupportTools_Excel.AzureDevOpsExplorer.Domain;

using XlHlp = VNC.AddinHelper.Excel;
using VNC;

namespace SupportTools_Excel.AzureDevOpsExplorer.Application
{
    class Body_BuildServer
    {
        #region Build Server (BS)

        internal static void Add_BuildAgents(XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            IBuildServer buildServer,
            TeamProject teamProject)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            //var buildDefinitions = Server.BuildServer.QueryBuildAgents();

            //foreach (IBuildDefinition buildDef in buildDefinitions)
            //{
            //    insertAt.ClearOffsets();
            //    int count = 0;

            //    XlHlp.AddContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildDef.Name));
            //    XlHlp.AddContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildDef.Description));
            //    XlHlp.AddContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildDef.QueueStatus));

            //    insertAt.IncrementRows();
            //}

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_BuildControllers(XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            IBuildServer buildServer,
            TeamProject teamProject)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            var buildControllers = buildServer.QueryBuildControllers();

            foreach (IBuildController buildController in buildControllers)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildController.Name));
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildController.Description));
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildController.Enabled));
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildController.Agents.Count));

                insertAt.IncrementRows();
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_BuildDefinitions(XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            IBuildServer buildServer,
            TeamProject teamProject)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            try
            {
                var buildDefinitions = buildServer.QueryBuildDefinitions(teamProject.Name);

                foreach (IBuildDefinition buildDef in buildDefinitions)
                {
                    insertAt.ClearOffsets();

                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", teamProject.Name));
                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildDef.Name));
                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildDef.Description));
                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildDef.QueueStatus));

                    insertAt.IncrementRows();
                }
            }
            catch (Exception ex)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", teamProject.Name));
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", "<N/A>"));
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", "<N/A>"));
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", "<N/A>"));

                insertAt.IncrementRows();
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_BuildProcessTemplates(XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            IBuildServer buildServer,
            TeamProject teamProject)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            var processTemplates = buildServer.QueryProcessTemplates(teamProject.Name);

            foreach (IProcessTemplate processTemplate in processTemplates)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", processTemplate.Id));
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", processTemplate.Description));
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", processTemplate.TemplateType));
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", processTemplate.Version));

                insertAt.IncrementRows();
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_Builds(XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            IBuildServer buildServer,
            TeamProject teamProject)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            var builds = buildServer.QueryBuilds(teamProject.Name);

            foreach (IBuildDetail buildDetail in builds)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildDetail.BuildController.Name));
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildDetail.LabelName));
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildDetail.StartTime));
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildDetail.FinishTime));
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildDetail.BuildFinished));

                insertAt.IncrementRows();
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_BuildServiceHosts(XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            IBuildServer buildServer,
            TeamProject teamProject)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            var buildServiceHosts = buildServer.QueryBuildServiceHosts("*");

            foreach (IBuildServiceHost buildServiceHost in buildServiceHosts)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildServiceHost.Name));
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildServiceHost.Status));
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), string.Format("{0}", buildServiceHost.StatusChangedOn));

                insertAt.IncrementRows();
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        #endregion
    }
}

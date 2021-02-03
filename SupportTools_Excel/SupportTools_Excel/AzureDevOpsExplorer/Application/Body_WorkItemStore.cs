using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

using Microsoft.Office.Interop.Excel;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

using SupportTools_Excel.AzureDevOpsExplorer.Domain;

using VNC;
using VNC.AddinHelper;

using XlHlp = VNC.AddinHelper.Excel;

namespace SupportTools_Excel.AzureDevOpsExplorer.Application
{
    class Body_WorkItemStore
    {
        #region WorkItem Store (WIS)

        internal static XlHlp.XlLocation Add_TP_Areas(XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            ICommonStructureService commonStructureService,
            Project project)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            if (project.AreaRootNodes.Count == 0)
            {
                XlHlp.AddContentToCell(insertAt.AddRowX(), "None");
            }
            else
            {
                insertAt = AddChildNodes(insertAt, options, commonStructureService, project.AreaRootNodes, project.Name, 0);
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);

            return insertAt;
        }

        internal static void Add_TP_FieldMapping(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            Microsoft.TeamFoundation.WorkItemTracking.Client.Project project)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            XlHlp.DisplayInWatchWindow("Begin");

            Dictionary<WorkItemType, List<ControlFieldMap>> allMappings = new Dictionary<WorkItemType, List<ControlFieldMap>>();

            foreach (WorkItemType wit in project.WorkItemTypes.Cast<WorkItemType>().OrderBy(nnn => nnn.Name))
            {
                long loopTicks = XlHlp.DisplayInWatchWindow("WorkItemType Loop");

                try
                {
                    var mappings = GetFieldMappings(allMappings, wit);

                    foreach (var controlFieldMap in mappings)
                    {
                        insertAt.ClearOffsets();

                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{project.Name}");
                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{wit.Name}");

                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{controlFieldMap.FieldMap.Name}");
                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{controlFieldMap.FieldMap.RefName}");
                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{controlFieldMap.FieldMap.Type}");
                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{controlFieldMap.FieldMap.Required}");

                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{controlFieldMap.MapType}");

                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{controlFieldMap.ControlMap.Label}");
                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{controlFieldMap.ControlMap.Name}");
                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{controlFieldMap.ControlMap.FieldName}");
                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{controlFieldMap.ControlMap.Type}");

                        insertAt.IncrementRows();
                    }

                    AZDOHelper.ProcessItemDelay(options);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                XlHlp.DisplayInWatchWindow("Loop", startTicks);
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static XlHlp.XlLocation Add_TP_Iterations(XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            ICommonStructureService commonStructureService,
            Project project)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            if (project.IterationRootNodes.Count == 0)
            {
                XlHlp.AddContentToCell(insertAt.AddRowX(), "None");
            }
            else
            {
                insertAt = AddChildNodes(insertAt, options, commonStructureService, project.IterationRootNodes, project.Name, 0);
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);

            return insertAt;
        }

         internal static void Add_TP_WorkItem_Info(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            WorkItemStore workItemStore,
            int workItemID)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            insertAt.ClearOffsets();
            int count = 0;

            try
            {
                WorkItem wi = VNC.TFS.Helper.RetrieveWorkItem(workItemID, workItemStore);
                insertAt.ClearOffsets();

                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ wi.Project.Name }");
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ wi.Id }");
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ wi.Type.Name }");
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ wi.Title }");
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ wi.CreatedBy }");
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ wi.CreatedDate }");
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ wi.ChangedBy }");
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ wi.ChangedDate }");
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ wi.Reason }");
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ wi.State }");
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ wi.AreaPath }");
                XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ wi.IterationPath }");

                insertAt.IncrementRows();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_TP_WorkItem_Links(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            WorkItemStore workItemStore,
            int workItemID)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            insertAt.ClearOffsets();
            int count = 0;

            try
            {
                string query = String.Format(
                    "Select [Id], [Created Date], [Changed Date], [Revised Date]"
                    + " From WorkItems"
                    + " Where [System.Id] = '{0}'",
                    workItemID);

                string query2 = String.Format(
                    "Select [Id], [System.Title]"
                    + " From WorkItemLinks"
                    + " Where Source.[System.Id] = '{0}'",
                    workItemID);


                Query wiQuery = new Query(workItemStore, query2);
                WorkItemLinkInfo[] wiTrees = wiQuery.RunLinkQuery();

                //PrintTrees(wiTrees, workItemID);

                WorkItemCollection queryResults = workItemStore.Query(query);

                if (queryResults.Count > 0)
                {
                    WorkItem wi = queryResults[0];

                    // TODO(crhodes)
                    // Figure out how wi.Links and wi.WorkItemLinks Differ
                    // Ok.  Look at Class Model.  Link is base type.
                    //  There are four derived types: ExternalLink, HyperLink, RelatedLink, WorkItemLink

                    foreach (Link link in wi.Links)
                    {
                        insertAt.ClearOffsets();

                        if (link.ArtifactLinkType != null)
                        {
                            XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ link.ArtifactLinkType.Name }");
                        }
                        else
                        {
                            XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), "<null>");
                        }

                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ link.BaseType.ToString() }");

                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ link.Comment }");
                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ link.IsLocked.ToString() }");
                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ link.IsNew.ToString() }");
                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ ((RelatedLink)link).LinkTypeEnd.Id.ToString() }");
                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ ((RelatedLink)link).LinkTypeEnd.Id.ToString() }");

                        insertAt.IncrementRows();
                    }
                }
                else
                {
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ID Not Found", workItemID.ToString()); ;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_TP_WorkItem_WorkItemLinks(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            WorkItemStore workItemStore,
            WorkItem workItem,
            int recursionLevel = 0)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            long startTicks2 = 0;

            if (options.ShowIndividualItems)
            {
                startTicks2 = XlHlp.DisplayInWatchWindow($"workItem:{workItem.Id} {workItem.Type.Name}");
            }

            insertAt.ClearOffsets();
            int count = 0;

            try
            {
                string queryWIL = String.Format(
                    "Select [Id], [System.Title]"
                    + " From WorkItemLinks"
                    + " Where Source.[System.Id] = '{0}'",
                    workItem.Id);

                Query wilQuery = new Query(workItemStore, queryWIL);
                WorkItemLinkInfo[] wiLinks = wilQuery.RunLinkQuery();

                // Get list of IDs for linked work teams (our targets)

                int[] linkedIDs = wiLinks.Select(i => i.TargetId).ToArray();
                int[] linkedIDsUnique = wiLinks.Select(i => i.TargetId).Distinct().ToArray();

                string queryWILdetails = String.Format(
                    "Select [Id], [System.Title], [System.WorkItemType]"
                    + " From WorkItems");

                // Not happy if duplicates

                //Query wilDetailsQuery = new Query(Server.WorkItemStore, queryWILdetails, linkedIDs);
                Query wilDetailsQuery = new Query(workItemStore, queryWILdetails, linkedIDsUnique);

                WorkItemCollection queryResultsWIL = wilDetailsQuery.RunQuery();

                List<WorkItem> bugWI = new List<WorkItem>();
                List<WorkItem> changeRequestWI = new List<WorkItem>();
                List<WorkItem> featureWI = new List<WorkItem>();
                List<WorkItem> milestoneWI = new List<WorkItem>();
                List<WorkItem> productionIssueWI = new List<WorkItem>();
                List<WorkItem> projectRiskWI = new List<WorkItem>();
                List<WorkItem> releaseWI = new List<WorkItem>();
                List<WorkItem> requirementWI = new List<WorkItem>();
                List<WorkItem> sharedStepsWI = new List<WorkItem>();
                List<WorkItem> specificationWI = new List<WorkItem>();
                List<WorkItem> taskWI = new List<WorkItem>();
                List<WorkItem> testCaseWI = new List<WorkItem>();
                List<WorkItem> testPlanWI = new List<WorkItem>();
                List<WorkItem> testSuiteWI = new List<WorkItem>();
                List<WorkItem> userNeedsWI = new List<WorkItem>();
                List<WorkItem> userStoryWI = new List<WorkItem>();
                List<WorkItem> voiceOfCustomerWI = new List<WorkItem>();

                // This catches what we do not cover specifically yet

                List<WorkItem> otherWI = new List<WorkItem>();

                CellFormatSpecification redContent = options.FormatSpecs.RedContent;
                CellFormatSpecification dateLabel = options.FormatSpecs.DateLabel;
                CellFormatSpecification dateContent = options.FormatSpecs.DateContent;
                CellFormatSpecification witContent = options.FormatSpecs.WITContent;

                foreach (WorkItemLink workItemLink in workItem.WorkItemLinks)
                {
                    insertAt.ClearOffsets();

                    // Doing this inside foreach is SUPER SLOW

                    //WorkItem target = Server.WorkItemStore.GetWorkItem(workItemLink.TargetId);
                    //XlHlp.AddContentToCell(insertAt.AddOffsetColumn(), $"{ target.Type.Name}");

                    // Use the dictionary instead to get the Type

                    //XlHlp.AddContentToCell(insertAt.AddOffsetColumn(), linkTypes[workItemLink.TargetId]);

                    // Use the collection of workitems to get details

                    int wiIndex = queryResultsWIL.IndexOf(workItemLink.TargetId);
                    WorkItem linkedWorkItem = queryResultsWIL[wiIndex];

                    switch (linkedWorkItem.Type.Name)
                    {
                        case "Bug":
                            bugWI.Add(linkedWorkItem);
                            break;

                        case "Change Request":
                            changeRequestWI.Add(linkedWorkItem);
                            break;

                        case "Feature":
                            milestoneWI.Add(linkedWorkItem);
                            break;

                        case "Milestone":
                            milestoneWI.Add(linkedWorkItem);
                            break;

                        case "Production Issue":
                            productionIssueWI.Add(linkedWorkItem);
                            break;

                        case "Project Risk":
                            projectRiskWI.Add(linkedWorkItem);
                            break;

                        case "Release":
                            releaseWI.Add(linkedWorkItem);
                            break;

                        case "Requirement":
                            requirementWI.Add(linkedWorkItem);
                            break;

                        case "Shared Steps":
                            sharedStepsWI.Add(linkedWorkItem);
                            break;

                        case "Specification":
                            specificationWI.Add(linkedWorkItem);
                            break;

                        case "Task":
                            taskWI.Add(linkedWorkItem);
                            break;

                        case "Test Case":
                            testCaseWI.Add(linkedWorkItem);
                            break;

                        case "Test Plan":
                            testPlanWI.Add(linkedWorkItem);
                            break;

                        case "Test Suite":
                            testSuiteWI.Add(linkedWorkItem);
                            break;

                        case "User Needs":
                            userNeedsWI.Add(linkedWorkItem);
                            break;

                        case "User Story":
                            userStoryWI.Add(linkedWorkItem);
                            break;

                        case "Voice Of Customer":
                            voiceOfCustomerWI.Add(linkedWorkItem);
                            break;

                        default:
                            otherWI.Add(linkedWorkItem);
                            break;
                    }

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.Type.Name}", cellFormat: witContent);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.Id}", cellFormat: redContent);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.State}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.Title}");

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ linkedWorkItem.Type.Name}", cellFormat: witContent);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ linkedWorkItem.Id}", cellFormat: redContent);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ linkedWorkItem.State}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ linkedWorkItem.Title}");

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.SourceId}", cellFormat: redContent);
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.TargetId}", cellFormat: redContent);

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.AddedBy}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.AddedDate}", cellFormat: dateContent);

                    if (workItemLink.ArtifactLinkType != null)
                    {
                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.ArtifactLinkType.Name}");
                    }
                    else
                    {
                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), "<null>");
                    }

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.BaseType }");

                    if (workItemLink.ChangedDate != null)
                    {
                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.ChangedDate}", cellFormat: dateContent);
                    }
                    else
                    {
                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), "<null>", cellFormat: dateContent);
                    }

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.Comment}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.IsLocked}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.IsNew}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.LinkTypeEnd.Id}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ GetLastPartOfDelimitedName(workItemLink.LinkTypeEnd.ImmutableName, '.')}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.LinkTypeEnd.IsForwardLink}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ GetLastPartOfDelimitedName(workItemLink.LinkTypeEnd.LinkType.ToString(), '.')}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.LinkTypeEnd.Name}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.LinkTypeEnd.OppositeEnd.Id}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ GetLastPartOfDelimitedName(workItemLink.LinkTypeEnd.OppositeEnd.ImmutableName, '.')}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.LinkTypeEnd.OppositeEnd.IsForwardLink}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ GetLastPartOfDelimitedName(workItemLink.LinkTypeEnd.OppositeEnd.LinkType.ToString(), '.')}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.LinkTypeEnd.OppositeEnd.Name}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.RemovedBy}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItemLink.RemovedDate}", cellFormat: dateContent);

                    insertAt.IncrementRows();

                    count++;    // Helpful for debugging to see how far we have gotten
                }

                // Drill down (one level) on the WorkItems and get their links
                // This gets time consuming so only go one level down.

                if (recursionLevel < options.RecursionLevel)
                {
                    recursionLevel++;

                    // Why do we increment above but don't seem to call?
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, otherWI, "otherWI");

                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, bugWI, "bugWI");
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, changeRequestWI, "changeRequestWI");
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, featureWI, "featureWI");
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, milestoneWI, "milestoneWI");
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, productionIssueWI, "productionIssueWI");
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, projectRiskWI, "projectRiskWI");
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, releaseWI, "releaseWI");
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, requirementWI, "requirementWI");
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, sharedStepsWI, "sharedStepsWI");
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, specificationWI, "specificationWI");
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, taskWI, "taskWI");
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, testCaseWI, "testCaseWI");
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, testPlanWI, "testPlanWI");
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, testSuiteWI, "testSuiteWI");
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, userNeedsWI, "userNeedsWI");
                    ProcessOneLevelDeeper(insertAt, options, workItemStore, recursionLevel, userStoryWI, "userStoryWI");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (options.ShowIndividualItems)
            {
                XlHlp.DisplayInWatchWindow(insertAt, startTicks2, "End");
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_TP_WorkItemDetails(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            WorkItemCollection queryResults)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            int itemCount = 0;
            int totalItems = queryResults.Count;

            try
            {
                foreach (WorkItem workItem in queryResults)
                {
                    insertAt.ClearOffsets();

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.Project.Name }");

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.Id }");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.Type.Name }");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.Title }");

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.CreatedBy }");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.CreatedDate }");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.ChangedBy }");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.ChangedDate }");

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.AuthorizedDate }");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.RevisedDate }");

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.State }");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.Reason }");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.Tags }");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.AreaPath }");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.IterationPath }");

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.RelatedLinkCount }");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.ExternalLinkCount }");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.HyperLinkCount }");

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{ workItem.Rev }");

                    insertAt.IncrementRows();

                    itemCount++;

                    if (itemCount % options.LoopUpdateInterval == 0)
                    {
                        XlHlp.DisplayInWatchWindow($"Added {itemCount} out of {totalItems}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_TP_WorkItemFields(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            Project project)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            foreach (WorkItemType wit in project.WorkItemTypes.Cast<WorkItemType>().OrderBy(nnn => nnn.Name))
            {
                insertAt.ClearOffsets();

                foreach (FieldDefinition fieldDef in wit.FieldDefinitions)
                {
                    var fieldName = fieldDef.Name;
                    var fieldType = fieldDef.SystemType;

                    switch (fieldDef.SystemType.FullName)
                    {
                        case "System.DateTime":
                            break;

                        case "System.Double":
                            break;

                        case "System.String":
                            break;

                        default:
                            break;
                    }


                    //sb.AppendFormat("{0}[{1}],", fieldName, fieldType);

                    insertAt.ClearOffsets();

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", project.Name));
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", wit.Name));
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", fieldDef.Name));
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", fieldDef.FieldType));
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", fieldDef.SystemType));

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", fieldDef.Id));
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", fieldDef.IsComputed));
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", fieldDef.IsCoreField));
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", fieldDef.IsEditable));
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", fieldDef.IsIdentity));
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", fieldDef.IsIndexed));
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", fieldDef.IsQueryable));
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", fieldDef.ReferenceName));

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", fieldDef.ReportingAttributes.Name));
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", fieldDef.ReportingAttributes.ReferenceName));

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", fieldDef.Usage));

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", fieldName + "[" + fieldType + "]"));

                    if (fieldDef.AllowedValues.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (var value in fieldDef.AllowedValues)
                        {
                            if (sb.Length > 0)
                            {
                                sb.Append($";{value}");
                            }
                            else
                            {
                                sb.Append(value);
                            }
                        }

                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{sb}");
                    }

                    if (fieldDef.ProhibitedValues.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (var value in fieldDef.ProhibitedValues)
                        {
                            if (sb.Length > 0)
                            {
                                sb.Append($";{value}");
                            }
                            else
                            {
                                sb.Append(value);
                            }
                        }

                        XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{sb}");
                        //XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), string.Format("{0}", sb.ToString()));
                    }

                    insertAt.IncrementRows();
                }
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_TP_WorkItemTypes(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            WorkItemStore workItemStore,
            Project project,
            out DateTime maxLastCreatedDate,
            out DateTime maxLastChangedDate,
            out DateTime maxLastRevisedDate)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            XlHlp.DisplayInWatchWindow("Begin");

            maxLastCreatedDate = DateTime.MinValue;
            maxLastChangedDate = DateTime.MinValue;
            maxLastRevisedDate = DateTime.MinValue;

            //DateTime startingDate = (DateTime.Now - TimeSpan.FromDays(options.GoBackDays));
            //string startDate = "1/1/1900";

            //if (options.GoBackDays > 0)
            //{
            //    startDate = startingDate.ToShortDateString();
            //}

            string startDate = options.StartDate.ToShortDateString();

            Dictionary<WorkItemType, List<Transition>> allTransitions = new Dictionary<WorkItemType, List<Transition>>();

            foreach (WorkItemType wit in project.WorkItemTypes.Cast<WorkItemType>().OrderBy(nnn => nnn.Name))
            {
                long loopTicks = XlHlp.DisplayInWatchWindow("WorkItemType Loop");

                string exportXMLFilePath = "";

                if (options.ExportXMLTemplate)
                {
                    exportXMLFilePath = $@"{options.XMLTemplateFilePath}\{project.Name}";

                    Directory.CreateDirectory(exportXMLFilePath);

                    XmlDocument exportXml = wit.Export(includeGlobalListsFlag: false);
                    exportXml.Save($@"{exportXMLFilePath}\{wit.Name}.txt");

                    if (options.IncludeGlobalLists)
                    {
                        XmlDocument exportXmlGlobalLists = wit.Export(includeGlobalListsFlag: true);
                        exportXmlGlobalLists.Save($@"{exportXMLFilePath}\{wit.Name}.gl.txt");
                    }
                }

                try
                {
                    var transitions = GetTransitions(allTransitions, wit);

                    string transitionsDisplay = PrintTransitions(transitions);

                    insertAt.ClearOffsets();
                    int count = 0;

                    string lastCreateDate = "???";
                    string lastChangedDate = "???";
                    string lastRevisedDate = "???";

                    if (options.GetLastActivityDates)
                    {
                        try
                        {
                            string query = String.Format(
                                "Select [Id], [Created Date], [Changed Date], [Revised Date]"
                                + " From WorkItems"
                                + " Where [Work Item Type] = '{0}'"
                                + " and [System.TeamProject] = '{1}'"
                                + " and ([Created Date] >= '{2}' or [Changed Date] >= '{2}')",
                                wit.Name, project.Name, startDate);

                            WorkItemCollection queryResults = workItemStore.Query(query);

                            if ((count = queryResults.Count) > 0)
                            {
                                WorkItem lastCreatedItem = queryResults.Cast<WorkItem>().OrderByDescending(iii => iii.CreatedDate).First();
                                lastCreateDate = lastCreatedItem.CreatedDate.ToString();

                                if (lastCreatedItem.CreatedDate > maxLastCreatedDate)
                                {
                                    maxLastCreatedDate = lastCreatedItem.CreatedDate;
                                }

                                WorkItem lastChangedItem = queryResults.Cast<WorkItem>().OrderByDescending(iii => iii.ChangedDate).First();
                                lastChangedDate = lastChangedItem.ChangedDate.ToString();

                                if (lastChangedItem.ChangedDate > maxLastChangedDate)
                                {
                                    maxLastChangedDate = lastChangedItem.ChangedDate;
                                }

                                WorkItem lastRevisedItem = queryResults.Cast<WorkItem>().OrderByDescending(iii => iii.RevisedDate).First();
                                lastRevisedDate = lastRevisedItem.RevisedDate.ToString();

                                if (lastRevisedItem.RevisedDate > maxLastRevisedDate)
                                {
                                    maxLastRevisedDate = lastRevisedItem.RevisedDate;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }

                        if (options.SkipIfNoActivity && lastCreateDate == "???")
                        {
                            continue;
                        }
                    }

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{project.Name}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{wit.Name}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{count}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{wit.FieldDefinitions.Count}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{lastCreateDate}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{lastChangedDate}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{lastRevisedDate}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{transitionsDisplay}");

                    insertAt.IncrementRows();

                    AZDOHelper.ProcessItemDelay(options);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                XlHlp.DisplayInWatchWindow("Loop", startTicks);
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Add_TP_WorkItemActivity(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            WorkItemStore workItemStore,
            Project project,
            out DateTime maxLastCreatedDate,
            out DateTime maxLastChangedDate,
            out DateTime maxLastRevisedDate)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            XlHlp.DisplayInWatchWindow("Begin");

            maxLastCreatedDate = DateTime.MinValue;
            maxLastChangedDate = DateTime.MinValue;
            maxLastRevisedDate = DateTime.MinValue;

            //DateTime startingDate = (DateTime.Now - TimeSpan.FromDays(options.GoBackDays));
            //string startDate = "1/1/1900";

            //if (options.GoBackDays > 0)
            //{
            //    startDate = startingDate.ToShortDateString();
            //}

            string startDate = options.StartDate.ToShortDateString();

            //Dictionary<WorkItemType, List<Transition>> allTransitions = new Dictionary<WorkItemType, List<Transition>>();

            foreach (WorkItemType wit in project.WorkItemTypes.Cast<WorkItemType>().OrderBy(nnn => nnn.Name))
            {
                long loopTicks = XlHlp.DisplayInWatchWindow("WorkItemType Loop");

                string exportXMLFilePath = "";

                //if (options.ExportXMLTemplate)
                //{
                //    exportXMLFilePath = $@"{options.XMLTemplateFilePath}\{project.Name}";

                //    Directory.CreateDirectory(exportXMLFilePath);

                //    XmlDocument exportXml = wit.Export(includeGlobalListsFlag: false);
                //    exportXml.Save($@"{exportXMLFilePath}\{wit.Name}.txt");

                //    if (options.IncludeGlobalLists)
                //    {
                //        XmlDocument exportXmlGlobalLists = wit.Export(includeGlobalListsFlag: true);
                //        exportXmlGlobalLists.Save($@"{exportXMLFilePath}\{wit.Name}.gl.txt");
                //    }
                //}

                try
                {
                    //var transitions = GetTransitions(allTransitions, wit);

                    //string transitionsDisplay = PrintTransitions(transitions);

                    insertAt.ClearOffsets();
                    int count = 0;

                    string lastCreateDate = "???";
                    string lastChangedDate = "???";
                    string lastRevisedDate = "???";

                    //if (options.GetLastActivityDates)
                    //{
                        try
                        {
                            string query = String.Format(
                                "Select [Id], [Created Date], [Changed Date], [Revised Date]"
                                + " From WorkItems"
                                + " Where [Work Item Type] = '{0}'"
                                + " and [System.TeamProject] = '{1}'"
                                + " and ([Created Date] >= '{2}' or [Changed Date] >= '{2}')",
                                wit.Name, project.Name, startDate);

                            WorkItemCollection queryResults = workItemStore.Query(query);

                            if ((count = queryResults.Count) > 0)
                            {
                                WorkItem lastCreatedItem = queryResults.Cast<WorkItem>().OrderByDescending(iii => iii.CreatedDate).First();
                                lastCreateDate = lastCreatedItem.CreatedDate.ToString();

                                if (lastCreatedItem.CreatedDate > maxLastCreatedDate)
                                {
                                    maxLastCreatedDate = lastCreatedItem.CreatedDate;
                                }

                                WorkItem lastChangedItem = queryResults.Cast<WorkItem>().OrderByDescending(iii => iii.ChangedDate).First();
                                lastChangedDate = lastChangedItem.ChangedDate.ToString();

                                if (lastChangedItem.ChangedDate > maxLastChangedDate)
                                {
                                    maxLastChangedDate = lastChangedItem.ChangedDate;
                                }

                                WorkItem lastRevisedItem = queryResults.Cast<WorkItem>().OrderByDescending(iii => iii.RevisedDate).First();
                                lastRevisedDate = lastRevisedItem.RevisedDate.ToString();

                                if (lastRevisedItem.RevisedDate > maxLastRevisedDate)
                                {
                                    maxLastRevisedDate = lastRevisedItem.RevisedDate;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }

                        if (options.SkipIfNoActivity && lastCreateDate == "???")
                        {
                            continue;
                        }
                    //}

                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{project.Name}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{wit.Name}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{count}");
                    //XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{wit.FieldDefinitions.Count}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{lastCreateDate}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{lastChangedDate}");
                    XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{lastRevisedDate}");
                    //XlHlp.AddContentToCell(insertAt.AddOffsetColumnX(), $"{transitionsDisplay}");

                    insertAt.IncrementRows();

                    AZDOHelper.ProcessItemDelay(options);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                XlHlp.DisplayInWatchWindow("Loop", startTicks);
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        internal static void Get_TP_WorkItemTypesXML(
            Options_AZDO_TFS options,
            Project project)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            foreach (WorkItemType wit in project.WorkItemTypes.Cast<WorkItemType>().OrderBy(nnn => nnn.Name))
            {
                long loopTicks = XlHlp.DisplayInWatchWindow("WorkItemType Loop");

                string exportXMLFilePath = $@"{options.XMLTemplateFilePath}\{project.Name}";

                Directory.CreateDirectory(exportXMLFilePath);

                XmlDocument exportXml = wit.Export(includeGlobalListsFlag: false);
                exportXml.Save($@"{exportXMLFilePath}\{wit.Name}.txt");

                if (options.IncludeGlobalLists)
                {
                    XmlDocument exportXmlGlobalLists = wit.Export(includeGlobalListsFlag: true);
                    exportXmlGlobalLists.Save($@"{exportXMLFilePath}\{wit.Name}.gl.txt");
                }

                XlHlp.DisplayInWatchWindow("Loop", startTicks);
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        //internal static void Add_WorkItemDetails(
        //    XlHlp.XlLocation insertAt,
        //    Options_AZDO_TFS options)
        //{
        //    // TODO(crhodes)
        //    // Loop across Team Projects and get last change or maybe go back days
        //}

        internal static XlHlp.XlLocation AddChildNodes(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            ICommonStructureService commonStructureService,
            NodeCollection childNodes,
            string projectName,
            int offsetLevel)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            insertAt.UpdateOffsets();

            foreach (Node item in childNodes)
            {
                var nodeInfo = commonStructureService.GetNode(item.Uri.ToString());

                Range startofRowRange = insertAt.workSheet.Cells[insertAt.RowCurrent, 1];

                XlHlp.AddContentToCell(startofRowRange, $"{ projectName }");
                insertAt.IncrementColumns();

                if (item.IsAreaNode)
                {
                    // HACK(crhodes)
                    // Somehow this needs to use the offsetLevel to get back to the first column or just hard code it.

                    XlHlp.AddContentToCell(insertAt.AddRowX(), item.Name);

                    if (options.ShowAllNodeLevels && item.HasChildNodes)
                    {
                        //insertAt.IncrementColumns();
                        insertAt = AddChildNodes(insertAt, options, commonStructureService, item.ChildNodes, projectName, offsetLevel + 1);
                        //insertAt.DecrementColumns();
                    }
                }

                if (item.IsIterationNode)
                {
                    string startdate = nodeInfo.StartDate.HasValue ? ((DateTime)nodeInfo.StartDate).ToShortDateString() : "<null>";
                    string finishdate = nodeInfo.FinishDate.HasValue ? ((DateTime)nodeInfo.FinishDate).ToShortDateString() : "<null>";

                    string days = "??";

                    if (nodeInfo.StartDate.HasValue)
                    {
                        days = ((DateTime)nodeInfo.FinishDate).Subtract((DateTime)nodeInfo.StartDate).TotalDays.ToString();
                    }

                    string iterationinfo = $"Name: >{item.Name,30}< (id: {item.Id}) - {days,3} days ({startdate} to {finishdate})";

                    XlHlp.AddContentToCell(insertAt.AddRowX(), iterationinfo);

                    if (options.ShowAllNodeLevels && item.HasChildNodes)
                    {
                        insertAt.IncrementColumns();
                        insertAt = AddChildNodes(insertAt, options, commonStructureService, item.ChildNodes, projectName, 0);
                        insertAt.DecrementColumns();
                    }
                }

                insertAt.DecrementColumns();
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);

            return insertAt;
        }

        private static List<ControlFieldMap> GetFieldMappings(
            Dictionary<WorkItemType, List<ControlFieldMap>> allMappings,
            WorkItemType workItemType)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            List<ControlFieldMap> currentMappings;

            allMappings.TryGetValue(workItemType, out currentMappings);

            if (currentMappings != null)
            {
                return currentMappings;
            }

            var newMappings = new List<ControlFieldMap>();

            try
            {
                XmlDocument workItemTypeXml = workItemType.Export(false);
                XDocument xDocument = XDocument.Parse(workItemTypeXml.OuterXml);
                XElement xElement = XElement.Parse(workItemTypeXml.OuterXml).Element("WORKITEMTYPE");

                // N.B. FIELDS and FIELD exist in WORKITEMTYPE and WORKITEMTYPE\WORKFLOW\STATES
                // be careful to only get the ones from WORKITEMTYPE\FIELDS

                var fields = xElement.Element("FIELDS").Elements("FIELD");
                //var fields2 = xDocument.Descendants("WORKITEMTYPE").      .Element("FIELDS").Descendants("FIELD");

                // This seems to be a clean way of getting to what we want.
                var layoutControls = xDocument.Descendants("Layout").Descendants("Control");
                var webLayoutControls = xDocument.Descendants("WebLayout").Descendants("Control");

                List<ControlMap> layoutControlList = new List<ControlMap>();
                List<ControlMap> webLayoutControlList = new List<ControlMap>();

                // By default Dictionary is Case sENSITIVE
                //Dictionary<string, FieldMap> fieldDictionary = new Dictionary<string, FieldMap>();

                // Tell Dictionary to ignore case and avoid the ToLower() junk when interacting with Keys.

                Dictionary<string, FieldMap> fieldDictionary = new Dictionary<string, FieldMap>(StringComparer.OrdinalIgnoreCase);

                Dictionary<string, ControlMap> layoutControlDictionary = new Dictionary<string, ControlMap>(StringComparer.OrdinalIgnoreCase);
                Dictionary<string, ControlMap> webLayoutControlDictionary = new Dictionary<string, ControlMap>(StringComparer.OrdinalIgnoreCase);

                Hashtable controlHashtable = new Hashtable();

                // Get all the Fields

                var countFieldNodes = fields.Count();

                foreach (XElement field in fields)
                {
                    // Some fields are inconsistent between the Fields Definition and Layout Sections
                    // e.g. System.Id and System.ID.  Force to lower so we can find them later.
                    // But if we do that they show up in lower case :( system.id
                    // Better to tell dictionary to ignore case, infra :)

                    //string refName = field.Attributes["refname"].Value.ToLower();
                    string refName = field.Attribute("refname").Value;
                    string name = "";
                    string type = "";
                    bool required = false;

                    name = field.Attribute("name")?.Value ?? "";
                    type = field.Attribute("type")?.Value ?? "";

                    if (field.Descendants("REQUIRED").Any())
                    {
                        required = true;
                    }

                    // TODO(crhodes)
                    // Name and Type may not exist.  Figure out null check.

                    if (fieldDictionary.ContainsKey(refName))
                    {
                        MessageBox.Show($"refName: {refName} already exists in fieldDictionary.  ");
                    }
                    else
                    {
                        fieldDictionary.Add(refName, new FieldMap
                        {
                            Name = name,
                            RefName = refName,
                            Type = type,
                            Required = required
                        });
                    }
                }

                foreach (XElement control in layoutControls)
                {
                    ControlMap controLMap = new ControlMap
                    {
                        Name = control.Attribute("Name")?.Value ?? "",
                        FieldName = control.Attribute("FieldName")?.Value ?? "",
                        Type = control.Attribute("Type")?.Value ?? "",
                        Label = control.Attribute("Label")?.Value ?? ""
                    };

                    layoutControlList.Add(controLMap);

                    if (control.Attribute("FieldName") != null)
                    {
                        if (layoutControlDictionary.ContainsKey(control.Attribute("FieldName").Value))
                        {
                            MessageBox.Show($"WIT: {workItemType.Name}  Already found {control.Attribute("FieldName").Value} in layoutControls Label: {controLMap.Label} Name: {controLMap.Name} ");
                        }
                        else
                        {
                            layoutControlDictionary.Add(control.Attribute("FieldName").Value, controLMap);
                        }
                    }
                    else
                    {
                        var type = control.Attribute("Type").Value;

                        if ((type != "LinksControl") && (type != "AttachmentsControl") && (type != "AssociatedAutomationControl"))
                        {
                            MessageBox.Show($"No FieldName and unrecognized type: {type}");
                        }
                    }
                }

                foreach (XElement control in webLayoutControls)
                {

                    ControlMap controLMap = new ControlMap
                    {
                        Name = control.Attribute("Name")?.Value ?? "",
                        FieldName = control.Attribute("FieldName")?.Value ?? "",
                        Type = control.Attribute("Type")?.Value ?? "",
                        Label = control.Attribute("Label")?.Value ?? ""
                    };

                    webLayoutControlList.Add(controLMap);

                    if (control.Attribute("FieldName") != null)
                    {
                        if (webLayoutControlDictionary.ContainsKey(control.Attribute("FieldName").Value))
                        {
                            MessageBox.Show($"WIT: {workItemType.Name}  Already found {control.Attribute("FieldName").Value} in webLayoutControls Label: {controLMap.Label} Name: {controLMap.Name} ");
                        }
                        else
                        {
                            webLayoutControlDictionary.Add(control.Attribute("FieldName").Value, controLMap);
                        }
                    }
                    else
                    {
                        var type = control.Attribute("Type").Value;

                        if ((type != "LinksControl") && (type != "AttachmentsControl") && (type != "AssociatedAutomationControl"))
                        {
                            MessageBox.Show($"No FieldName and unrecognized type: {type}");
                        }
                    }
                }

                var countLayoutControlList = layoutControlList.Count;
                var countWebLayoutControlList = webLayoutControlList.Count;

                // TODO(crhodes)
                // Maybe we should go the other way and loop the fields and then see if any
                // Layout or WebLayout controls display the field.

                // Iterate all the Layout Controls and get the appropriate FieldMap

                foreach (var item in fieldDictionary)
                {
                    try
                    {
                        ControlFieldMap controlFieldMap = new ControlFieldMap();

                        controlFieldMap.FieldMap = fieldDictionary[item.Key];
                        string refName = controlFieldMap.FieldMap.RefName;

                        // Have to loop as field could be use in many places

                        foreach (var control in layoutControlDictionary.Values.Where(c => c.FieldName == refName))
                        {
                            controlFieldMap.MapType = "Layout";

                            controlFieldMap.ControlMap = layoutControlDictionary[refName];
                            newMappings.Add(controlFieldMap);
                        }

                        foreach (var control in webLayoutControlDictionary.Values.Where(c => c.FieldName == refName))
                        {
                            controlFieldMap.MapType = "WebLayout";

                            controlFieldMap.ControlMap = webLayoutControlDictionary[refName];
                            newMappings.Add(controlFieldMap);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

                allMappings.Add(workItemType, newMappings);

                XlHlp.DisplayInWatchWindow($"WorkItem: {workItemType.Name} FieldNodes: {countFieldNodes}  LayoutControlList: {countLayoutControlList}  WebLayoutControlList: {countWebLayoutControlList}  newMappings: {newMappings.Count}");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);

            return newMappings;
        }

        static string GetLastPartOfDelimitedName(string inputString, char delimiter)
        {
            var lio = inputString.LastIndexOf(delimiter);
            return inputString.Substring(lio + 1);
        }

        /// <summary>
        /// Get the transitions for this <see cref="WorkItemType"/>
        /// </summary>
        /// <param name="workItemType"></param>
        /// <returns></returns>
        private static List<Transition> GetTransitions(Dictionary<WorkItemType, List<Transition>> allTransitions,
            WorkItemType workItemType)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            List<Transition> currentTransitions;

            // See if this WorkItemType has already had it's transitions figured out.
            allTransitions.TryGetValue(workItemType, out currentTransitions);

            if (currentTransitions != null)
            {
                return currentTransitions;
            }

            // Create a dictionary to allow us to look up the "to" state using a "from" state.
            var newTransitions = new List<Transition>();

            // Get this worktype type as xml
            try
            {
                var foo = workItemType.Name;

                XmlDocument workItemTypeXml = workItemType.Export(false);

                // get the transitions node.
                XmlNodeList transitionsList = workItemTypeXml.GetElementsByTagName("TRANSITIONS");

                // As there is only one transitions item we can just get the first
                XmlNode transitions = transitionsList[0];

                // Iterate all the transitions
                foreach (XmlNode transition in transitions)
                {
                    StringBuilder reasons = new StringBuilder();
                    StringBuilder fields = new StringBuilder();

                    XmlNode reasonsNode = transition.SelectSingleNode("REASONS");
                    XmlNode fieldsNode = transition.SelectSingleNode("FIELDS");

                    foreach (XmlNode reason in reasonsNode)
                    {
                        if (reasons.Length != 0)
                        {
                            reasons.Append($", {reason.Attributes["value"].Value}");
                        }
                        else
                        {
                            reasons.Append(reason.Attributes["value"].Value);
                        }

                        if (reason.Name == "DEFAULTREASON")
                        {
                            reasons.Append("*");
                        }
                    }

                    // Not all REASONS have required FIELDS

                    if (fieldsNode != null)
                    {
                        foreach (XmlNode field in fieldsNode)
                        {
                            try
                            {
                                string trimedField = field.Attributes["refname"].Value.Replace("Microsoft.", "M.");

                                if (fields.Length != 0)
                                {
                                    fields.Append($", {trimedField}");
                                }
                                else
                                {
                                    fields.Append(trimedField);
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                            // TODO(crhodes)
                            // Maybe show <EMPTY />
                            //if (field.Name == "DEFAULTREASON")
                            //{
                            //    reasons.Append("*");
                            //}
                        }
                    }

                    // save off the transition 
                    newTransitions.Add(new Transition
                    {
                        From = transition.Attributes["from"].Value,
                        To = transition.Attributes["to"].Value,
                        For = transition.Attributes["for"] != null ? $"for {transition.Attributes["for"].Value}" : "",
                        Reasons = reasons.ToString(),
                        Fields = fields.ToString()
                    });
                }

                // Add transition so we don't do it again if it is needed.
                allTransitions.Add(workItemType, newTransitions);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);

            return newTransitions;
        }

        private static string PrintTransitions(List<Transition> transitions)
        {
            StringBuilder sb = new StringBuilder();
            string pad = new string(' ', 40);

            foreach (var transition in transitions.OrderBy(n => n.From))
            {
                if (sb.Length == 0)
                {
                    sb.Append($"{transition.From,17} -> {transition.To,-17} ({transition.Reasons})");
                    if (transition.For.Length > 0) sb.Append($" > {pad}{transition.For}");

                    if (transition.Fields.Length > 0) sb.Append($"\n{pad}Fields: {transition.Fields}");
                }
                else
                {
                    sb.Append($"\n{transition.From,17} -> {transition.To,-17} ({transition.Reasons})");
                    if (transition.For.Length > 0) sb.Append($"> {transition.For}");

                    if (transition.Fields.Length > 0) sb.Append($"\n{pad}Fields: {transition.Fields}");
                }
            }

            return sb.ToString();
        }

        private static void ProcessOneLevelDeeper(
            XlHlp.XlLocation insertAt,
            Options_AZDO_TFS options,
            WorkItemStore workItemStore,
            int recursionLevel,
            List<WorkItem> typeofWI, string workItemType)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_APPNAME);

            int totalItems = typeofWI.Count;

            XlHlp.DisplayInWatchWindow($"WorkItem Type: {workItemType} Count:{totalItems} RecursionLevel:{recursionLevel}");

            if (typeofWI.Count > 0)
            {
                int itemCount = 0;
                insertAt.IncrementRows();

                foreach (WorkItem wi in typeofWI)
                {
                    options.ShowIndividualItems = false;

                    Add_TP_WorkItem_WorkItemLinks(insertAt, options, workItemStore, wi, recursionLevel);
                    itemCount++;    // Useful if debugging to see how far we have progressed

                    AZDOHelper.DisplayLoopUpdates(startTicks, options, totalItems, itemCount);
                }
            }

            Log.APPLICATION("Exit", Common.LOG_APPNAME, startTicks);
        }

        //private string PrintMappings(List<FieldMap> mappings)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    string pad = new string(' ', 40);

        //    //foreach (var transition in mappings.OrderBy(n => n.From))
        //    //{
        //    //    if (sb.Length == 0)
        //    //    {
        //    //        sb.Append($"{transition.From,17} -> {transition.To,-17} ({transition.Reasons})");
        //    //        if (transition.For.Length > 0) sb.Append($" > {pad}{transition.For}");

        //    //        if (transition.Fields.Length > 0) sb.Append($"\n{pad}Fields: {transition.Fields}");
        //    //    }
        //    //    else
        //    //    {
        //    //        sb.Append($"\n{transition.From,17} -> {transition.To,-17} ({transition.Reasons})");
        //    //        if (transition.For.Length > 0) sb.Append($"> {transition.For}");

        //    //        if (transition.Fields.Length > 0) sb.Append($"\n{pad}Fields: {transition.Fields}");
        //    //    }
        //    //}

        //    return sb.ToString();
        //}

        internal struct ControlFieldMap
        {
            public ControlMap ControlMap { get; set; }
            public FieldMap FieldMap { get; set; }
            public string MapType { get; set; }
        }

        internal struct ControlMap
        {
            public string FieldName { get; set; }
            public string Label { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
        }

        internal struct FieldMap
        {
            public string Name { get; set; }
            public string RefName { get; set; }
            public string Type { get; set; }
            public bool Required { get; set; }
        }

        internal struct Transition
        {
            public string Fields { get; set; }
            public string For { get; set; }
            public string From { get; set; }
            public string Reasons { get; set; }
            public string To { get; set; }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

using SupportTools_Visio.Domain;

using VNC.Core;

using Visio = Microsoft.Office.Interop.Visio;
using VisioHelper = VNC.AddinHelper.Visio;

namespace SupportTools_Visio.Actions
{
    public partial class AZDOActions
    {
        public static VNC.WPF.Presentation.Dx.Views.DxThemedWindowHost addLinkedWorkItemsHost = null;

        private static VssCredentials _vssCredentials = null;

        public static VssCredentials GetVssCredentials()
        {
            VssCredentials credentials = null;
            string userName = default;
            string password = default;

            if (_vssCredentials is null)
            {
                // PAT
                //_vssCredentials = new VssBasicCredential(string.Empty, "bg5i7rzuveiummftdsnzuu4zs4s5vuc3gm7xl3uuyvxw2hmnbada");

                // UserName/Password
                //_vssCredentials = new VssAadCredential(userName, password);

                // Logon
                _vssCredentials = new VssClientCredentials();
            }

            return _vssCredentials;
        }

        public static async Task<IList<WorkItem>> QueryWorkItemInfo(string organization, int id)
        {
            var uri = new Uri($"https://dev.azure.com/{organization}");
            var credentials = GetVssCredentials();

            //var project = "VNC Agile";

            var wiql = new Wiql()
            {
                // NOTE: Even if other columns are specified, only the ID & URL are available in the WorkItemReference
                Query = "Select [Id] " +
                    "From WorkItems " +
                    "Where Id = " + id
            };

            using (var httpClient = new WorkItemTrackingHttpClient(uri, credentials))
            {
                // execute the query to get the list of work items in the results
                var result = await httpClient.QueryByWiqlAsync(wiql).ConfigureAwait(false);
                var ids = result.WorkItems.Select(item => item.Id).ToArray();

                // some error handling
                if (ids.Length == 0)
                {
                    return Array.Empty<WorkItem>();
                }

                string[] fields = GetFieldList();

                // Get WorkItem details (fields) for the ids found in query
                return await httpClient.GetWorkItemsAsync(ids, fields, result.AsOf).ConfigureAwait(false);
            }
        }

        public static async Task<IList<WorkItem>> QueryWorkItemLinks(string organization, int id, int relatedLinkCount)
        {
            var uri = new Uri($"https://dev.azure.com/{organization}");
            var credentials = GetVssCredentials();

            //var project = "VNC Agile";


            var wiql = new Wiql()
            {
                // NOTE: Even if other columns are specified, only the ID & URL are available in the WorkItemReference
                Query = "Select [Id] "
                    + "From WorkItemLinks "
                    + "Where Source.[System.Id] = " + id
            };


            if (relatedLinkCount > 250)
                MessageBox.Show($"Great than 250 Links ({relatedLinkCount}), removing Test Cases");
            {
                wiql.Query += " AND Target.[System.WorkItemType] <> 'Test Case'";
            }

            // NOTE(crhodes)
            // This works but still get BadRequest Exception when trying to get Test Cases back
            // from release 918783.  Maybe do multiple queries

            // " AND Target.[System.WorkItemType] = 'Test Case'"

            using (var httpClient = new WorkItemTrackingHttpClient(uri, credentials))
            {
                // execute the query to get the list of work items in the results
                var result = await httpClient.QueryByWiqlAsync(wiql).ConfigureAwait(false);
                var ids = result.WorkItemRelations.Select(item => item.Target.Id).Distinct().ToArray();

                // some error handling
                if (ids.Length == 0)
                {
                    return Array.Empty<WorkItem>();
                }

                //if (ids.Length > 250)
                //{
                //    MessageBox.Show($"Great than 250 Links ({ids.Length}), removing Test Cases");
                //}

                string[] fields = GetFieldList();

                // Get WorkItem details (fields) for the ids found in query
                return await httpClient.GetWorkItemsAsync(ids, fields, result.AsOf).ConfigureAwait(false);
                // HACK(crhodes)
                // Try taking fewer to get beyond exceptions and bad requests
                //return await httpClient.GetWorkItemsAsync(ids.Take(200), fields, result.AsOf).ConfigureAwait(false);
            }
        }

        private static string[] GetFieldList()
        {
            //build a list of the fields we want to see
            return new[]
            {
                "System.Id", "System.TeamProject"
                , "System.WorkItemType"
                , "System.Title", "System.State"
                , "System.CreatedDate", "System.CreatedBy"
                , "System.ChangedDate", "System.ChangedBy"
                , "System.RelatedLinkCount", "System.ExternalLinkCount"
                , "System.RemoteLinkCount", "System.HyperLinkCount"
            };

            //return new[]
            //{
            //    "System.Id"
            //};
        }

        internal static async void AddLinkedWorkItems(Visio.Application app, string doc, string page, string shape, string shapeu, string[] vs)
        {
            VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
                MethodBase.GetCurrentMethod().Name));

            // NOTE(crhodes)
            // Can launch a UI here.  Or earlier.

            //DxThemedWindowHost.DisplayUserControlInHost(ref addLinkedWorkItemsHost,
            //    "Edit Shape Control Points Text",
            //    Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            //    DxThemedWindowHost.ShowWindowMode.Modeless,
            //    new Presentation.Views.EditControlPoints());

            Visio.Page activePage = app.ActivePage;
            Visio.Shape activeShape = app.ActivePage.Shapes[shape];

            WorkItemInfoShape activeShapeWorkItemInfo = new WorkItemInfoShape(activeShape);

            int id;

            if (int.TryParse(activeShapeWorkItemInfo.ID, out id))
            {
            }
            else
            {
                MessageBox.Show($"Cannot parse ({activeShapeWorkItemInfo.ID}) as WorkItemID");
                return;
            }

            int relatedLinkCount;

            if (int.TryParse(activeShapeWorkItemInfo.RelatedLinkCount, out relatedLinkCount))
            {
            }
            else
            {
                MessageBox.Show($"Cannot parse ({activeShapeWorkItemInfo.RelatedLinkCount}) as RelatedLinkCount");
                return;
            }

            var result = await QueryWorkItemLinks(activeShapeWorkItemInfo.Organization, id, relatedLinkCount);

            if (result.Count > 0)
            {
                Point initialPosition = GetPosition(activeShape);
                Point insertionPoint = initialPosition;

                string stencilName = "Azure DevOps.vssx";
                string shapeName = "WI";
                //string shapeName = "WI & Info";
                Visio.Document linkStencil;
                Visio.Master linkMaster = null;

                try
                {
                    linkStencil = app.Documents[stencilName];

                    try
                    {
                        linkMaster = linkStencil.Masters[shapeName];
                    }
                    catch (Exception ex)
                    {
                        VisioHelper.DisplayInWatchWindow(string.Format("  Cannot find Master named:>{0}<", shapeName));
                    }
                }
                catch (Exception ex)
                {
                    VisioHelper.DisplayInWatchWindow(string.Format("  Cannot find open Stencil named:>{0}<", stencilName));
                }

                // TODO(crhodes)
                // Figure out how to get size of shape from master.
                // HACK(crhodes)
                // .25 is for Link counts

                WorkItemOffsets workItemOffsets = new WorkItemOffsets(initialPosition, 0.375, 0.25, 0.05);

                foreach (var linkedWorkItem in result)
                {
                    // NOTE(crhodes)
                    // This includes the current shape.  Do not add it.
                    // May always be first one.  Maybe loop counter
                    if (linkedWorkItem.Id == id)
                    {
                        continue;
                    }

                    VisioHelper.DisplayInWatchWindow($"{linkedWorkItem.Id} {linkedWorkItem.Fields["System.Title"]}");

                    insertionPoint = CalculateInsertionPoint(initialPosition, insertionPoint, linkedWorkItem, activeShapeWorkItemInfo, workItemOffsets);

                    AddNewLinkedWorkItemShape(linkMaster, activePage, linkedWorkItem, insertionPoint, activeShapeWorkItemInfo);
                }
            }

            VisioHelper.DisplayInWatchWindow($"{activeShapeWorkItemInfo}");
        }

        internal static async void GetWorkItemInfo(Visio.Application app, string doc, string page, string shape, string shapeu, string[] vs)
        {
            VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
                MethodBase.GetCurrentMethod().Name));

            Visio.Page activePage = app.ActivePage;
            Visio.Shape activeShape = app.ActivePage.Shapes[shape];

            WorkItemInfoShape workItemInfoShape = new WorkItemInfoShape(activeShape);

            int id = 0;

            if ( !int.TryParse(workItemInfoShape.ID, out id))
            {
                MessageBox.Show($"Invalid WorkItem ID: ({workItemInfoShape.ID})");
                return;
            }
            
            var result = await QueryWorkItemInfo(workItemInfoShape.Organization, int.Parse(workItemInfoShape.ID));

            var teamProject = result[0].Fields["System.TeamProject"];
            var workItemType = result[0].Fields["System.WorkItemType"];

            var title = result[0].Fields["System.Title"];
            var state = result[0].Fields["System.State"];

            var createdBy = ((IdentityRef)result[0].Fields["System.CreatedBy"]).DisplayName;
            var createdDate = result[0].Fields["System.CreatedDate"];
            var changedBy = ((IdentityRef)result[0].Fields["System.ChangedBy"]).DisplayName;
            var changedDate = result[0].Fields["System.ChangedDate"];

            var relatedLinkCount = result[0].Fields["System.RelatedLinkCount"];
            var externalLinkCount = result[0].Fields["System.ExternalLinkCount"];
            var remoteLinkCount = result[0].Fields["System.RemoteLinkCount"];
            var hyperLinkCount = result[0].Fields["System.HyperLinkCount"];

            activeShape.CellsU["Prop.TeamProject"].FormulaU = teamProject.ToString().WrapInDblQuotes();

            activeShape.CellsU["Prop.ExternalLink"].FormulaU = $"http://dev.azure.com/{workItemInfoShape.Organization}/{teamProject}/_workitems/edit/{id}/".WrapInDblQuotes();

            activeShape.CellsU["Prop.PageName"].FormulaU = workItemType.ToString().WrapInDblQuotes();

            var cleanTitle = title.ToString().Replace("\"", "\"\"").WrapInDblQuotes();

            activeShape.CellsU["Prop.Title"].FormulaU = cleanTitle;
            activeShape.CellsU["Prop.State"].FormulaU = state.ToString().WrapInDblQuotes();
            activeShape.CellsU["Prop.CreatedDate"].FormulaU = createdDate.ToString().WrapInDblQuotes();
            activeShape.CellsU["Prop.CreatedBy"].FormulaU = createdBy.WrapInDblQuotes();
            activeShape.CellsU["Prop.ChangedDate"].FormulaU = changedDate.ToString().WrapInDblQuotes();
            activeShape.CellsU["Prop.ChangedBy"].FormulaU = changedBy.WrapInDblQuotes();

            activeShape.CellsU["Prop.RelatedLinks"].FormulaU = relatedLinkCount.ToString().WrapInDblQuotes();
            activeShape.CellsU["Prop.ExternalLinks"].FormulaU = externalLinkCount.ToString().WrapInDblQuotes();
            activeShape.CellsU["Prop.RemoteLinks"].FormulaU = remoteLinkCount.ToString().WrapInDblQuotes();
            activeShape.CellsU["Prop.HyperLinks"].FormulaU = hyperLinkCount.ToString().WrapInDblQuotes();

            VisioHelper.DisplayInWatchWindow($"{workItemInfoShape}");
        }

        private static void AddNewLinkedWorkItemShape(Visio.Master linkMaster, Visio.Page page, WorkItem linkedWorkItem, Point insertionPoint, WorkItemInfoShape relatedShape)
        {
            string stencilName = "Azure DevOps.vssx";
            string shapeName = "WI";
            //string shapeName = "WI & Info";

            try
            {
                //Visio.Document linkStencil = app.Documents[stencilName];

                try
                {
                    //Visio.Master linkMaster = linkStencil.Masters[shapeName];

                    Visio.Shape newWorkItemShape = page.Drop(linkMaster, insertionPoint.X, insertionPoint.Y);

                    try
                    {
                        var id = linkedWorkItem.Fields["System.Id"];

                        newWorkItemShape.CellsU["Prop.Organization"].FormulaU = relatedShape.Organization.WrapInDblQuotes();
                        newWorkItemShape.CellsU["Prop.TeamProject"].FormulaU = relatedShape.TeamProject.WrapInDblQuotes();

                        newWorkItemShape.CellsU["Prop.ExternalLink"].FormulaU = $"http://dev.azure.com/{relatedShape.Organization}/{relatedShape.TeamProject}/_workitems/edit/{id}/".WrapInDblQuotes();

                        var teamProject = linkedWorkItem.Fields["System.TeamProject"];
                        var workItemType = linkedWorkItem.Fields["System.WorkItemType"];

                        var title = linkedWorkItem.Fields["System.Title"];
                        var state = linkedWorkItem.Fields["System.State"];

                        var createdBy = ((IdentityRef)linkedWorkItem.Fields["System.CreatedBy"]).DisplayName;
                        var createdDate = linkedWorkItem.Fields["System.CreatedDate"];
                        var changedBy = ((IdentityRef)linkedWorkItem.Fields["System.ChangedBy"]).DisplayName;
                        var changedDate = linkedWorkItem.Fields["System.ChangedDate"];

                        var relatedLinkCount = linkedWorkItem.Fields["System.RelatedLinkCount"];
                        var externalLinkCount = linkedWorkItem.Fields["System.ExternalLinkCount"];
                        var remoteLinkCount = linkedWorkItem.Fields["System.RemoteLinkCount"];
                        var hyperLinkCount = linkedWorkItem.Fields["System.HyperLinkCount"];

                        newWorkItemShape.CellsU["Prop.TeamProject"].FormulaU = teamProject.ToString().WrapInDblQuotes();
                        newWorkItemShape.CellsU["Prop.PageName"].FormulaU = workItemType.ToString().WrapInDblQuotes();
                        newWorkItemShape.CellsU["Prop.ID"].FormulaU = id.ToString().WrapInDblQuotes();

                        var cleanTitle = title.ToString().Replace("\"", "\"\"").WrapInDblQuotes();

                        newWorkItemShape.CellsU["Prop.Title"].FormulaU = cleanTitle;
                        newWorkItemShape.CellsU["Prop.State"].FormulaU = state.ToString().WrapInDblQuotes();
                        newWorkItemShape.CellsU["Prop.CreatedDate"].FormulaU = createdDate.ToString().WrapInDblQuotes();
                        newWorkItemShape.CellsU["Prop.CreatedBy"].FormulaU = createdBy.WrapInDblQuotes();
                        newWorkItemShape.CellsU["Prop.ChangedDate"].FormulaU = changedDate.ToString().WrapInDblQuotes();
                        newWorkItemShape.CellsU["Prop.ChangedBy"].FormulaU = changedBy.WrapInDblQuotes();

                        newWorkItemShape.CellsU["Prop.RelatedLinks"].FormulaU = relatedLinkCount.ToString().WrapInDblQuotes();
                        newWorkItemShape.CellsU["Prop.ExternalLinks"].FormulaU = externalLinkCount.ToString().WrapInDblQuotes();
                        newWorkItemShape.CellsU["Prop.RemoteLinks"].FormulaU = remoteLinkCount.ToString().WrapInDblQuotes();
                        newWorkItemShape.CellsU["Prop.HyperLinks"].FormulaU = hyperLinkCount.ToString().WrapInDblQuotes();
                    }
                    catch (Exception ex)
                    {
                        VisioHelper.DisplayInWatchWindow($"{linkedWorkItem.Id}");
                    }
                }
                catch (Exception ex)
                {
                    VisioHelper.DisplayInWatchWindow(string.Format("  Cannot find Master named:>{0}<", shapeName));
                }
            }
            catch (Exception ex)
            {
                VisioHelper.DisplayInWatchWindow(string.Format("  Cannot find open Stencil named:>{0}<", stencilName));
            }
        }

        private static Point CalculateInsertionPoint(Point initialPosition, Point insertionPoint,
            WorkItem linkedWorkItem, WorkItemInfoShape activeShape, WorkItemOffsets workItemOffsets)
        {
            Point newInsertionPoint = new Point();

            double height = activeShape.Height;
            double width = activeShape.Width;

            string shapeWorkItemType = activeShape.WorkItemType;

            switch (linkedWorkItem.Fields["System.WorkItemType"])
            {
                case "Bug":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            //workItemOffsets.Bug.DecrementHorizontal(width);
                            workItemOffsets.Bug.DecrementHorizontal(width, OffsetDirection.Down);
                            newInsertionPoint.X = workItemOffsets.Bug.X;
                            newInsertionPoint.Y = workItemOffsets.Bug.Y;
                            break;

                        case "Epic":
                            //workItemOffsets.Bug.DecrementHorizontal(width);
                            workItemOffsets.Bug.DecrementHorizontal(width, OffsetDirection.Down);
                            newInsertionPoint.X = workItemOffsets.Bug.X;
                            newInsertionPoint.Y = workItemOffsets.Bug.Y;
                            break;

                        case "Feature":
                            //workItemOffsets.Bug.DecrementHorizontal(width);
                            workItemOffsets.Bug.DecrementHorizontal(width, OffsetDirection.Down);
                            newInsertionPoint.X = workItemOffsets.Bug.X;
                            newInsertionPoint.Y = workItemOffsets.Bug.Y;
                            break;

                        case "Release":
                            //workItemOffsets.Bug.DecrementHorizontal(width);
                            workItemOffsets.Bug.DecrementHorizontal(width, OffsetDirection.Down);
                            newInsertionPoint.X = workItemOffsets.Bug.X;
                            newInsertionPoint.Y = workItemOffsets.Bug.Y;
                            break;

                        case "Requirement":
                            workItemOffsets.Bug.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Bug.X;
                            newInsertionPoint.Y = workItemOffsets.Bug.Y;
                            break;

                        case "Task":
                            //workItemOffsets.Bug.DecrementHorizontal(width);
                            workItemOffsets.Bug.DecrementHorizontal(width, OffsetDirection.Down);
                            newInsertionPoint.X = workItemOffsets.Bug.X;
                            newInsertionPoint.Y = workItemOffsets.Bug.Y;
                            break;

                        case "Test Case":
                            if (workItemOffsets.Release.Count > 0)
                            {
                                workItemOffsets.UserStory.DecrementHorizontal(width, OffsetDirection.Down);
                                newInsertionPoint.X = workItemOffsets.Release.X;
                                newInsertionPoint.Y = workItemOffsets.Release.Y;
                            }
                            if (workItemOffsets.UserStory.Count > 0)
                            {
                                workItemOffsets.UserStory.DecrementHorizontal(width, OffsetDirection.Down);
                                newInsertionPoint.X = workItemOffsets.UserStory.X;
                                newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            }
                            else if (workItemOffsets.Bug.Count > 0)
                            {
                                workItemOffsets.Bug.DecrementHorizontal(width, OffsetDirection.Down);
                                newInsertionPoint.X = workItemOffsets.Bug.X;
                                newInsertionPoint.Y = workItemOffsets.Bug.Y;
                            }
                            else
                            {
                                workItemOffsets.Unknown.IncrementHorizontal(width);
                                newInsertionPoint.X = workItemOffsets.Unknown.X;
                                newInsertionPoint.Y = workItemOffsets.Unknown.Y;
                            }

                            break;

                        case "User Needs":
                            workItemOffsets.Bug.DecrementHorizontal(width, OffsetDirection.Down);
                            newInsertionPoint.X = workItemOffsets.Bug.X;
                            newInsertionPoint.Y = workItemOffsets.Bug.Y;
                            break;

                        case "User Story":
                            workItemOffsets.Bug.DecrementHorizontal(width, OffsetDirection.Down);
                            newInsertionPoint.X = workItemOffsets.Bug.X;
                            newInsertionPoint.Y = workItemOffsets.Bug.Y;
                            break;

                        default:
                            // TODO(crhodes)
                            // What should this do???
                            break;
                    }

                    break;

                case "Epic":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            workItemOffsets.Epic.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Epic.X;
                            newInsertionPoint.Y = workItemOffsets.Epic.Y;
                            break;

                        case "Epic":
                            workItemOffsets.Epic.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Epic.X;
                            newInsertionPoint.Y = workItemOffsets.Epic.Y;
                            break;

                        case "Feature":
                            workItemOffsets.Epic.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Epic.X;
                            newInsertionPoint.Y = workItemOffsets.Epic.Y;
                            break;

                        case "Release":
                            workItemOffsets.Epic.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Epic.X;
                            newInsertionPoint.Y = workItemOffsets.Epic.Y;
                            break;

                        case "Requirement":
                            workItemOffsets.Epic.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Epic.X;
                            newInsertionPoint.Y = workItemOffsets.Epic.Y;
                            break;

                        case "Task":
                            workItemOffsets.Epic.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Epic.X;
                            newInsertionPoint.Y = workItemOffsets.Epic.Y;
                            break;

                        case "Test Case":
                            workItemOffsets.Epic.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Epic.X;
                            newInsertionPoint.Y = workItemOffsets.Epic.Y;
                            break;

                        case "User Needs":
                            workItemOffsets.Epic.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Epic.X;
                            newInsertionPoint.Y = workItemOffsets.Epic.Y;
                            break;

                        case "User Story":
                            workItemOffsets.Epic.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Epic.X;
                            newInsertionPoint.Y = workItemOffsets.Epic.Y;
                            break;

                        default:
                            break;
                    }

                    break;

                case "Feature":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            workItemOffsets.Feature.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Feature.X;
                            newInsertionPoint.Y = workItemOffsets.Feature.Y;
                            break;

                        case "Epic":
                            workItemOffsets.Feature.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Feature.X;
                            newInsertionPoint.Y = workItemOffsets.Feature.Y;
                            break;

                        case "Feature":
                            workItemOffsets.Feature.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Feature.X;
                            newInsertionPoint.Y = workItemOffsets.Feature.Y;
                            break;

                        case "Release":
                            //workItemOffsets.Feature.DecrementHorizontal(width);
                            //newInsertionPoint.X = workItemOffsets.Feature.X;
                            //newInsertionPoint.Y = workItemOffsets.Feature.Y;
                            if (workItemOffsets.UserStory.Count > 0)
                            {
                                workItemOffsets.UserStory.IncrementHorizontal(width, OffsetDirection.Down);
                                newInsertionPoint.X = workItemOffsets.UserStory.X;
                                newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            }
                            else
                            {
                                workItemOffsets.Feature.IncrementHorizontal(width);
                                newInsertionPoint.X = workItemOffsets.Feature.X;
                                newInsertionPoint.Y = workItemOffsets.Feature.Y;
                            }
                            break;

                        case "Requirement":
                            workItemOffsets.Feature.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Feature.X;
                            newInsertionPoint.Y = workItemOffsets.Feature.Y;
                            break;

                        case "Task":
                            workItemOffsets.Feature.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Feature.X;
                            newInsertionPoint.Y = workItemOffsets.Feature.Y;
                            break;

                        case "Test Case":
                            workItemOffsets.Feature.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Feature.X;
                            newInsertionPoint.Y = workItemOffsets.Feature.Y;
                            break;

                        case "User Needs":
                            workItemOffsets.Feature.DecrementHorizontal(width, OffsetDirection.Up);
                            newInsertionPoint.X = workItemOffsets.Feature.X;
                            newInsertionPoint.Y = workItemOffsets.Feature.Y;
                            break;

                        case "User Story":
                            workItemOffsets.Feature.DecrementHorizontal(width, OffsetDirection.Up);
                            newInsertionPoint.X = workItemOffsets.Feature.X;
                            newInsertionPoint.Y = workItemOffsets.Feature.Y;
                            break;

                        default:
                            break;
                    }

                    break;

                case "Release":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            if (workItemOffsets.UserStory.Count > 0)
                            {
                                workItemOffsets.UserStory.IncrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.UserStory.X;
                                newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            }
                            else
                            {
                                workItemOffsets.Release.IncrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.Release.X;
                                newInsertionPoint.Y = workItemOffsets.Release.Y;
                            }

                            break;

                        case "Epic":
                            workItemOffsets.Release.IncrementHorizontal(width);

                            newInsertionPoint.X = workItemOffsets.Release.X;
                            newInsertionPoint.Y = workItemOffsets.Release.Y;
                            break;

                        case "Feature":
                            workItemOffsets.Release.DecrementHorizontal(width, OffsetDirection.Up);

                            newInsertionPoint.X = workItemOffsets.Release.X;
                            newInsertionPoint.Y = workItemOffsets.Release.Y;
                            break;

                        case "Release":
                            workItemOffsets.Release.DecrementHorizontal(width);

                            newInsertionPoint.X = workItemOffsets.Release.X;
                            newInsertionPoint.Y = workItemOffsets.Release.Y;
                            break;

                        case "Requirement":
                            workItemOffsets.Release.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Release.X;
                            newInsertionPoint.Y = workItemOffsets.Release.Y;
                            break;

                        case "Task":
                            workItemOffsets.Release.IncrementHorizontal(width, OffsetDirection.Down);

                            newInsertionPoint.X = workItemOffsets.Release.X;
                            newInsertionPoint.Y = workItemOffsets.Release.Y;
                            break;

                        case "Test Case":
                            if (workItemOffsets.UserStory.Count > 0)
                            {
                                workItemOffsets.UserStory.DecrementHorizontal(width, OffsetDirection.Down);
                                newInsertionPoint.X = workItemOffsets.UserStory.X;
                                newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            }
                            else
                            {
                                workItemOffsets.Release.IncrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.Release.X;
                                newInsertionPoint.Y = workItemOffsets.Release.Y;
                            }
                            break;

                        case "User Needs":
                            workItemOffsets.Release.DecrementHorizontal(width, OffsetDirection.Up);
                            newInsertionPoint.X = workItemOffsets.Release.X;
                            newInsertionPoint.Y = workItemOffsets.Release.Y;
                            break;

                        case "User Story":
                            if (workItemOffsets.Feature.Count > 0)
                            {
                                workItemOffsets.Feature.DecrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.Feature.X;
                                newInsertionPoint.Y = workItemOffsets.Feature.Y;
                            }
                            else
                            {
                                workItemOffsets.Release.IncrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.Release.X;
                                newInsertionPoint.Y = workItemOffsets.Release.Y;
                            }

                            break;

                        default:
                            break;
                    }

                    break;

                case "Requirement":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            workItemOffsets.Requirement.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Requirement.X;
                            newInsertionPoint.Y = workItemOffsets.Requirement.Y;
                            break;

                        case "Epic":
                            workItemOffsets.Requirement.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Requirement.X;
                            newInsertionPoint.Y = workItemOffsets.Requirement.Y;
                            break;

                        case "Feature":
                            workItemOffsets.Requirement.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Requirement.X;
                            newInsertionPoint.Y = workItemOffsets.Requirement.Y;
                            break;

                        case "Release":
                            if (workItemOffsets.UserStory.Count > 0)
                            {
                                workItemOffsets.UserStory.IncrementHorizontal(width, OffsetDirection.Down);
                                newInsertionPoint.X = workItemOffsets.UserStory.X;
                                newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            }
                            else
                            {
                                workItemOffsets.Requirement.IncrementHorizontal(width);
                                newInsertionPoint.X = workItemOffsets.Requirement.X;
                                newInsertionPoint.Y = workItemOffsets.Requirement.Y;
                            }

                            break;

                        case "Requirement":
                            workItemOffsets.Requirement.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Requirement.X;
                            newInsertionPoint.Y = workItemOffsets.Requirement.Y;
                            break;

                        case "Task":
                            workItemOffsets.Requirement.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Requirement.X;
                            newInsertionPoint.Y = workItemOffsets.Requirement.Y;
                            break;

                        case "Test Case":
                            workItemOffsets.Requirement.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Requirement.X;
                            newInsertionPoint.Y = workItemOffsets.Requirement.Y;
                            break;

                        case "User Needs":
                            workItemOffsets.Requirement.IncrementHorizontal(width, OffsetDirection.Down);
                            newInsertionPoint.X = workItemOffsets.Requirement.X;
                            newInsertionPoint.Y = workItemOffsets.Requirement.Y;
                            break;

                        case "User Story":
                            workItemOffsets.Requirement.IncrementHorizontal(width, OffsetDirection.Down);
                            newInsertionPoint.X = workItemOffsets.Requirement.X;
                            newInsertionPoint.Y = workItemOffsets.Requirement.Y;
                            break;

                        default:
                            break;
                    }

                    break;

                case "Task":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            workItemOffsets.Task.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Task.X;
                            newInsertionPoint.Y = workItemOffsets.Task.Y;
                            break;

                        case "Epic":
                            workItemOffsets.Task.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Task.X;
                            newInsertionPoint.Y = workItemOffsets.Task.Y;
                            break;

                        case "Feature":
                            workItemOffsets.Task.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Task.X;
                            newInsertionPoint.Y = workItemOffsets.Task.Y;
                            break;

                        case "Release":
                            workItemOffsets.Task.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Task.X;
                            newInsertionPoint.Y = workItemOffsets.Task.Y;
                            break;

                        case "Requirement":
                            workItemOffsets.Task.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Task.X;
                            newInsertionPoint.Y = workItemOffsets.Task.Y;
                            break;

                        case "Task":
                            workItemOffsets.Task.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Task.X;
                            newInsertionPoint.Y = workItemOffsets.Task.Y;
                            break;

                        case "Test Case":
                            workItemOffsets.Task.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.Task.X;
                            newInsertionPoint.Y = workItemOffsets.Task.Y;
                            break;

                        case "User Needs":
                            workItemOffsets.Task.IncrementHorizontal(width, OffsetDirection.Down);
                            newInsertionPoint.X = workItemOffsets.Task.X;
                            newInsertionPoint.Y = workItemOffsets.Task.Y;
                            break;

                        case "User Story":
                            workItemOffsets.Task.IncrementHorizontal(width, OffsetDirection.Down);
                            newInsertionPoint.X = workItemOffsets.Task.X;
                            newInsertionPoint.Y = workItemOffsets.Task.Y;
                            break;

                        default:
                            break;
                    }

                    break;

                case "Test Case":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            if (workItemOffsets.Release.Count > 0)
                            {
                                workItemOffsets.Release.IncrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.Release.X;
                                newInsertionPoint.Y = workItemOffsets.Release.Y;
                            }
                            else if (workItemOffsets.UserStory.Count > 0)
                            {
                                workItemOffsets.UserStory.IncrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.UserStory.X;
                                newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            }
                            else if (workItemOffsets.TestCase.Count > 0)
                            {
                                workItemOffsets.TestCase.IncrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.TestCase.X;
                                newInsertionPoint.Y = workItemOffsets.TestCase.Y;
                            }
                            else
                            {
                                workItemOffsets.Unknown.IncrementHorizontal(width);
                                newInsertionPoint.X = workItemOffsets.Unknown.X;
                                newInsertionPoint.Y = workItemOffsets.Unknown.Y;
                            }
                            //workItemOffsets.TestCase.IncrementHorizontal(width, OffsetDirection.Up);
                            break;

                        case "Epic":
                            workItemOffsets.TestCase.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.TestCase.X;
                            newInsertionPoint.Y = workItemOffsets.TestCase.Y;
                            break;

                        case "Feature":
                            workItemOffsets.TestCase.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.TestCase.X;
                            newInsertionPoint.Y = workItemOffsets.TestCase.Y;
                            break;

                        case "Release":
                            workItemOffsets.TestCase.IncrementHorizontal(width, OffsetDirection.Up);
                            newInsertionPoint.X = workItemOffsets.TestCase.X;
                            newInsertionPoint.Y = workItemOffsets.TestCase.Y;
                            break;

                        case "Requirement":
                            workItemOffsets.TestCase.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.TestCase.X;
                            newInsertionPoint.Y = workItemOffsets.TestCase.Y;
                            break;

                        case "Task":
                            workItemOffsets.TestCase.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.TestCase.X;
                            newInsertionPoint.Y = workItemOffsets.TestCase.Y;
                            break;

                        case "Test Case":
                            workItemOffsets.TestCase.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.TestCase.X;
                            newInsertionPoint.Y = workItemOffsets.TestCase.Y;
                            break;

                        case "User Needs":
                            workItemOffsets.TestCase.IncrementHorizontal(width, OffsetDirection.Up);
                            newInsertionPoint.X = workItemOffsets.TestCase.X;
                            newInsertionPoint.Y = workItemOffsets.TestCase.Y;
                            break;

                        case "User Story":
                            workItemOffsets.TestCase.IncrementHorizontal(width, OffsetDirection.Up);
                            newInsertionPoint.X = workItemOffsets.TestCase.X;
                            newInsertionPoint.Y = workItemOffsets.TestCase.Y;
                            break;

                        default:
                            break;
                    }

                    break;

                case "User Needs":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            workItemOffsets.UserNeeds.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.UserNeeds.X;
                            newInsertionPoint.Y = workItemOffsets.UserNeeds.Y;
                            break;

                        case "UserNeeds":
                            workItemOffsets.UserNeeds.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.UserNeeds.X;
                            newInsertionPoint.Y = workItemOffsets.UserNeeds.Y;
                            break;

                        case "Feature":
                            //if (workItemOffsets.UserNeed.Count > 0)
                            //{
                            //    workItemOffsets.Release.DecrementHorizontal(width, OffsetDirection.Up);
                            //    newInsertionPoint.X = workItemOffsets.Release.X;
                            //    newInsertionPoint.Y = workItemOffsets.Release.Y;
                            //}
                            //else
                            //{
                                workItemOffsets.UserNeeds.IncrementHorizontal(width, OffsetDirection.Down);
                                newInsertionPoint.X = workItemOffsets.UserNeeds.X;
                                newInsertionPoint.Y = workItemOffsets.UserNeeds.Y;
                            //}

                            break;

                        case "Release":
                            //workItemOffsets.UserNeeds.DecrementHorizontal(width, OffsetDirection.Up);
                            //newInsertionPoint.X = workItemOffsets.UserNeeds.X;
                            //newInsertionPoint.Y = workItemOffsets.UserNeeds.Y;
                            if (workItemOffsets.UserStory.Count > 0)
                            {
                                workItemOffsets.UserStory.IncrementHorizontal(width, OffsetDirection.Down);
                                newInsertionPoint.X = workItemOffsets.UserStory.X;
                                newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            }
                            else
                            {
                                workItemOffsets.Feature.IncrementHorizontal(width);
                                newInsertionPoint.X = workItemOffsets.UserNeeds.X;
                                newInsertionPoint.Y = workItemOffsets.UserNeeds.Y;
                            }
                            break;

                        case "Requirement":
                            workItemOffsets.UserNeeds.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.UserNeeds.X;
                            newInsertionPoint.Y = workItemOffsets.UserNeeds.Y;
                            break;

                        case "Task":
                            workItemOffsets.UserNeeds.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.UserNeeds.X;
                            newInsertionPoint.Y = workItemOffsets.UserNeeds.Y;
                            break;

                        case "Test Case":
                            workItemOffsets.UserNeeds.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.UserNeeds.X;
                            newInsertionPoint.Y = workItemOffsets.UserNeeds.Y;
                            break;

                        case "User Needs":
                            workItemOffsets.UserNeeds.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.UserNeeds.X;
                            newInsertionPoint.Y = workItemOffsets.UserNeeds.Y;
                            break;

                        case "User Story":
                            workItemOffsets.UserNeeds.DecrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.UserNeeds.X;
                            newInsertionPoint.Y = workItemOffsets.UserNeeds.Y;
                            break;

                        default:
                            break;
                    }

                    break;
                case "User Story":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            if (workItemOffsets.TestCase.Count > 0)
                            {
                                workItemOffsets.TestCase.IncrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.TestCase.X;
                                newInsertionPoint.Y = workItemOffsets.TestCase.Y;
                            }
                            else if (workItemOffsets.UserStory.Count > 0)
                            {
                                workItemOffsets.UserStory.IncrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.UserStory.X;
                                newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            }
                            else if (workItemOffsets.Release.Count > 0)
                            {
                                workItemOffsets.Release.IncrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.Release.X;
                                newInsertionPoint.Y = workItemOffsets.Release.Y;
                            }
                            else
                            {
                                workItemOffsets.Unknown.IncrementHorizontal(width);
                                newInsertionPoint.X = workItemOffsets.Unknown.X;
                                newInsertionPoint.Y = workItemOffsets.Unknown.Y;
                            }

                            break;

                        case "Epic":
                            workItemOffsets.UserStory.IncrementHorizontal(width);
                            break;

                        case "Feature":
                            workItemOffsets.UserStory.IncrementHorizontal(width, OffsetDirection.Down);
                            newInsertionPoint.X = workItemOffsets.UserStory.X;
                            newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            break;

                        case "Release":
                            workItemOffsets.UserStory.IncrementHorizontal(width, OffsetDirection.Down);
                            newInsertionPoint.X = workItemOffsets.UserStory.X;
                            newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            break;

                        case "Requirement":
                            workItemOffsets.UserStory.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.UserStory.X;
                            newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            break;

                        case "Task":
                            workItemOffsets.UserStory.DecrementHorizontal(width, OffsetDirection.Up);
                            newInsertionPoint.X = workItemOffsets.UserStory.X;
                            newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            break;

                        case "Test Case":
                            if (workItemOffsets.Bug.Count > 0)
                            {
                                workItemOffsets.Bug.DecrementHorizontal(width, OffsetDirection.Down);
                            }
                            else
                            {
                                workItemOffsets.UserStory.DecrementHorizontal(width, OffsetDirection.Down);
                                newInsertionPoint.X = workItemOffsets.UserStory.X;
                                newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            }
                            break;

                        case "User Needs":
                            workItemOffsets.UserStory.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.UserStory.X;
                            newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            break;

                        case "User Story":
                            workItemOffsets.UserStory.IncrementHorizontal(width);
                            newInsertionPoint.X = workItemOffsets.UserStory.X;
                            newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            break;

                        default:
                            break;
                    }

                    break;

                default:
                    newInsertionPoint.X = initialPosition.X;
                    newInsertionPoint.Y = initialPosition.Y;
                    break;
            }

            return newInsertionPoint;
        }

        private static Point GetPosition(Visio.Shape activeShape)
        {
            double x = 5.5;
            double y = 2.0;

            x = activeShape.CellsU["PinX"].ResultIU;
            y = activeShape.CellsU["PinY"].ResultIU;

            Point currentPosition = new Point(x, y);

            return currentPosition;
        }
    }
}
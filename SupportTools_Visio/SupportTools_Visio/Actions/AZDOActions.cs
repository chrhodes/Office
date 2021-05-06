using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.WebApi;

using SupportTools_Visio.Domain;

using VNC.Core;
using VNC.WPF.Presentation.Dx.Views;

using Visio = Microsoft.Office.Interop.Visio;
using VisioHelper = VNC.AddinHelper.Visio;
using System.Windows;

namespace SupportTools_Visio.Actions
{
    public class AZDOActions
    {
        internal static async void GetWorkItemInfo(Visio.Application app, string doc, string page, string shape, string shapeu, string[] vs)
        {
            VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
                MethodBase.GetCurrentMethod().Name));

            Visio.Page activePage = app.ActivePage;
            Visio.Shape activeShape = app.ActivePage.Shapes[shape];

            WorkItemInfoShape workItemInfoShape = new WorkItemInfoShape(activeShape);

            var result = await QueryWorkItemInfo(workItemInfoShape.Organization, int.Parse(workItemInfoShape.ID));

            var workItemType = result[0].Fields["System.WorkItemType"];
            var title = result[0].Fields["System.Title"];
            var state = result[0].Fields["System.State"];
            var createdBy = ((IdentityRef)result[0].Fields["System.CreatedBy"]).DisplayName;
            var createdDate = result[0].Fields["System.CreatedDate"];
            var changedBy = ((IdentityRef)result[0].Fields["System.ChangedBy"]).DisplayName;
            var changedDate = result[0].Fields["System.ChangedDate"];

            activeShape.CellsU["Prop.PageName"].FormulaU = workItemType.ToString().WrapInDblQuotes();

            var cleanTitle = title.ToString().Replace("\"", "\"\"").WrapInDblQuotes();

            activeShape.CellsU["Prop.Title"].FormulaU = cleanTitle;
            activeShape.CellsU["Prop.State"].FormulaU = state.ToString().WrapInDblQuotes();
            activeShape.CellsU["Prop.CreatedDate"].FormulaU = createdDate.ToString().WrapInDblQuotes();
            activeShape.CellsU["Prop.CreatedBy"].FormulaU = createdBy.WrapInDblQuotes();
            activeShape.CellsU["Prop.ChangedDate"].FormulaU = changedDate.ToString().WrapInDblQuotes();
            activeShape.CellsU["Prop.ChangedBy"].FormulaU = changedBy.WrapInDblQuotes();

            VisioHelper.DisplayInWatchWindow($"{workItemInfoShape}");
        }

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

            //var uri = new Uri("https://dev.azure.com/BD-STS-PROD");
            var uri = new Uri($"https://dev.azure.com/{organization}");
            //var uri = new Uri($"https:////dev.azure.com//{organization}");

            var credentials = GetVssCredentials();

            var project = "VNC Agile";

            var wiql = new Wiql()
            {
                // NOTE: Even if other columns are specified, only the ID & URL are available in the WorkItemReference
                Query = "Select [Id] " +
                    "From WorkItems " +
                    "Where Id = " + id
            };

            // create instance of work item tracking http client
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

                // build a list of the fields we want to see
                var fields = new[]
                { "System.Id", "System.WorkItemType"
                    , "System.Title", "System.State"
                    , "System.CreatedDate", "System.CreatedBy"
                    , "System.ChangedDate", "System.ChangedBy"
                };

                // get work items for the ids found in query
                return await httpClient.GetWorkItemsAsync(ids, fields, result.AsOf).ConfigureAwait(false);
            }
        }

        public static VNC.WPF.Presentation.Dx.Views.DxThemedWindowHost addLinkedWorkItemsHost = null;

        private static Point GetPosition(Visio.Shape activeShape)
        {
            double x = 5.5;
            double y = 2.0;

            x = activeShape.CellsU["PinX"].ResultIU;
            y = activeShape.CellsU["PinY"].ResultIU;

            Point currentPosition = new Point(x, y);

            return currentPosition;
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

            var result = await QueryWorkItemLinks(activeShapeWorkItemInfo.Organization, id);

            if (result.Count > 0)
            {
                //Point initialInsertionPoint = new Point(5.0, 4.0);
                Point initialPosition = GetPosition(activeShape);
                Point insertionPoint = initialPosition;

                WorkItemHorizontalOffsets horizontalOffsets = new WorkItemHorizontalOffsets(initialPosition.X);

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

                    insertionPoint = CalculateInsertionPoint(initialPosition, insertionPoint, linkedWorkItem, activeShapeWorkItemInfo, horizontalOffsets);

                    AddNewLinkedWorkItemShape(app, activePage, linkedWorkItem, insertionPoint, activeShapeWorkItemInfo);
                }
            }

            VisioHelper.DisplayInWatchWindow($"{activeShapeWorkItemInfo}");
        }

        class WorkItemHorizontalOffsets
        {
            public WorkItemHorizontalOffsets(double initialOffset)
            {
                Bug = initialOffset;
                Epic = initialOffset;
                Feature = initialOffset;
                Task = initialOffset;
                TestCase = initialOffset;
                UserStory = initialOffset;
                Unknown = initialOffset;
            }

            public double Bug;
            public double Epic;
            public double Feature;
            public double Task;
            public double TestCase;
            public double UserStory;
            public double Unknown;

            public double IncrementBug(double offset)
            {
                Bug = Bug += offset;
                return Bug;
            }

            public double IncrementEpic(double offset)
            {
                return Epic = Epic += offset;
            }


            public double IncrementFeature(double offset)
            {
                return Feature = Feature += offset;
            }


            public double IncrementTask(double offset)
            {
                return Task = Task  += offset;
            }

            public double IncrementTestCase(double offset)
            {
                return TestCase = TestCase += offset;
            }

            public double IncrementUserStory(double offset)
            {
                return UserStory = UserStory += offset;
            }

            public double IncrementUnknown(double offset)
            {
                return Unknown =  Unknown  += offset;
            }
        }


        private static Point CalculateInsertionPoint(Point initialPosition, Point insertionPoint, 
            WorkItem linkedWorkItem, WorkItemInfoShape activeShape, WorkItemHorizontalOffsets horizontalOffsets)
        {
            Point newInsertionPoint = new Point();

            double height = activeShape.Height;
            double width = activeShape.Width;

            string shapeWorkItemType = activeShape.WorkItemType;

            // HACK(crhodes)
            // See if can't make this less opaque
            // Seems like work item should know where it stands in relation to other work items.
            // if same time should go left or right at same level or maybe half step.

            switch (linkedWorkItem.Fields["System.WorkItemType"])
            {
                case "Bug":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            newInsertionPoint.X = horizontalOffsets.IncrementBug(-width);
                            newInsertionPoint.Y = initialPosition.Y;
                            break;

                        case "Epic":
                            newInsertionPoint.X = horizontalOffsets.IncrementBug(-width);
                            newInsertionPoint.Y = initialPosition.Y;
                            break;

                        case "Feature":
                            newInsertionPoint.X = horizontalOffsets.IncrementBug(-width);
                            newInsertionPoint.Y = initialPosition.Y;
                            break;

                        case "Task":
                            newInsertionPoint.X = horizontalOffsets.IncrementBug(-width);
                            newInsertionPoint.Y = initialPosition.Y;
                            break;

                        case "Test Case":
                            newInsertionPoint.X = horizontalOffsets.IncrementTestCase(width);
                            newInsertionPoint.Y = initialPosition.Y;
                            break;

                        case "User Story":
                            newInsertionPoint.X = horizontalOffsets.IncrementBug(-width);
                            newInsertionPoint.Y = initialPosition.Y + 1 * height;
                            break;

                        default:
                            break;
                    }

                    break;

                case "Epic":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            newInsertionPoint.X = horizontalOffsets.IncrementEpic(width);
                            newInsertionPoint.Y = initialPosition.Y;
                            break;

                        case "Epic":
                            newInsertionPoint.X = horizontalOffsets.IncrementEpic(width);
                            newInsertionPoint.Y = initialPosition.Y;
                            break;

                        case "Feature":
                            newInsertionPoint.X = horizontalOffsets.IncrementEpic(width);
                            newInsertionPoint.Y = initialPosition.Y - 1 * height;
                            break;

                        case "Task":
                            newInsertionPoint.X = horizontalOffsets.IncrementEpic(width);
                            newInsertionPoint.Y = initialPosition.Y;
                            break;

                        case "Test Case":
                            newInsertionPoint.X = horizontalOffsets.IncrementTestCase(width);
                            newInsertionPoint.Y = initialPosition.Y;
                            break;

                        case "User Story":
                            newInsertionPoint.X = horizontalOffsets.IncrementEpic(width);
                            newInsertionPoint.Y = initialPosition.Y + 1 * height;
                            break;

                        default:
                            break;
                    }

                    break;

                case "Feature":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            newInsertionPoint.X = horizontalOffsets.IncrementFeature(-width);
                            newInsertionPoint.Y = initialPosition.Y + 1 * height;
                            break;

                        case "Epic":
                            newInsertionPoint.X = horizontalOffsets.IncrementFeature(-width);
                            newInsertionPoint.Y = initialPosition.Y - 1 * height;
                            break;

                        case "Feature":
                            newInsertionPoint.X = horizontalOffsets.IncrementFeature(-width);
                            newInsertionPoint.Y = initialPosition.Y - 1 * height;
                            break;

                        case "Task":
                            newInsertionPoint.X = horizontalOffsets.IncrementFeature(-width);
                            newInsertionPoint.Y = initialPosition.Y + 1 * height;
                            break;

                        case "Test Case":
                            newInsertionPoint.X = horizontalOffsets.IncrementTestCase(width);
                            newInsertionPoint.Y = initialPosition.Y;
                            break;

                        case "User Story":
                            newInsertionPoint.X = horizontalOffsets.IncrementFeature(-width);
                            newInsertionPoint.Y = initialPosition.Y + 1 * height;
                            break;

                        default:
                            break;
                    }

                    break;

                case "Task":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            newInsertionPoint.X = horizontalOffsets.IncrementTask(width);
                            newInsertionPoint.Y = initialPosition.Y + 1 * height;
                            break;

                        case "Epic":
                            newInsertionPoint.X = horizontalOffsets.IncrementTask(-width);
                            newInsertionPoint.Y = initialPosition.Y - 1 * height;
                            break;

                        case "Feature":
                            newInsertionPoint.X = horizontalOffsets.IncrementTask(-width);
                            newInsertionPoint.Y = initialPosition.Y - 1 * height;
                            break;

                        case "Task":
                            newInsertionPoint.X = horizontalOffsets.IncrementTask(-width);
                            newInsertionPoint.Y = initialPosition.Y + 1 * height;
                            break;

                        case "Test Case":
                            newInsertionPoint.X = horizontalOffsets.IncrementTask(width);
                            newInsertionPoint.Y = initialPosition.Y;
                            break;

                        case "User Story":
                            newInsertionPoint.X = horizontalOffsets.IncrementTask(width);
                            newInsertionPoint.Y = initialPosition.Y - 1 * height;
                            break;

                        default:
                            break;
                    }

                    break;

                case "Test Case":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            newInsertionPoint.X = horizontalOffsets.IncrementTestCase(width);
                            newInsertionPoint.Y = initialPosition.Y + 1 * height;
                            break;

                        case "Epic":
                            newInsertionPoint.X = horizontalOffsets.IncrementTestCase(width);
                            newInsertionPoint.Y = initialPosition.Y - 2 * height;
                            break;

                        case "Feature":
                            newInsertionPoint.X = horizontalOffsets.IncrementTestCase(width);
                            newInsertionPoint.Y = initialPosition.Y - 1 * height;
                            break;

                        case "Task":
                            newInsertionPoint.X = horizontalOffsets.IncrementTestCase(-width);
                            newInsertionPoint.Y = initialPosition.Y + 1 * height;
                            break;

                        case "Test Case":
                            newInsertionPoint.X = horizontalOffsets.IncrementTestCase(width);
                            newInsertionPoint.Y = initialPosition.Y;
                            break;

                        case "User Story":
                            newInsertionPoint.X = horizontalOffsets.IncrementTestCase(-width);
                            newInsertionPoint.Y = initialPosition.Y;
                            break;

                        default:
                            break;

                    }

                    break;

                case "User Story":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            newInsertionPoint.X = horizontalOffsets.IncrementUserStory(width);
                            newInsertionPoint.Y = initialPosition.Y + 1 * height;
                            break;

                        case "Epic":
                            newInsertionPoint.X = horizontalOffsets.IncrementUserStory(width);
                            newInsertionPoint.Y = initialPosition.Y - 2 * height;
                            break;

                        case "Feature":
                            newInsertionPoint.X = horizontalOffsets.IncrementUserStory(width);
                            newInsertionPoint.Y = initialPosition.Y - 1 * height;
                            break;

                        case "Task":
                            newInsertionPoint.X = horizontalOffsets.IncrementUserStory(-width);
                            newInsertionPoint.Y = initialPosition.Y + 1 * height;
                            break;

                        case "Test Case":
                            newInsertionPoint.X = horizontalOffsets.IncrementUserStory(width);
                            newInsertionPoint.Y = initialPosition.Y;
                            break;

                        case "User Story":
                            newInsertionPoint.X = horizontalOffsets.IncrementUserStory(-width);
                            newInsertionPoint.Y = initialPosition.Y;
                            break;

                        default:
                            break;

                    }

                    break;

                default:
                    newInsertionPoint.X = horizontalOffsets.IncrementUnknown(-width);
                    newInsertionPoint.Y = initialPosition.Y;
                    break;
            }

            return newInsertionPoint;
        }

        private static void AddNewLinkedWorkItemShape(Visio.Application app, Visio.Page page, WorkItem linkedWorkItem, Point insertionPoint, WorkItemInfoShape relatedShape)
        {
            string stencilName = "Azure DevOps.vssx";
            string shapeName = "WI & Info";

            try
            {
                Visio.Document linkStencil = app.Documents[stencilName];

                try
                {
                    Visio.Master linkMaster = linkStencil.Masters[shapeName];

                    Visio.Shape newWorkItemShape = page.Drop(linkMaster, insertionPoint.X, insertionPoint.Y);

                    var id = linkedWorkItem.Fields["System.Id"];
                    var workItemType = linkedWorkItem.Fields["System.WorkItemType"];
                    var title = linkedWorkItem.Fields["System.Title"];
                    var state = linkedWorkItem.Fields["System.State"];
                    var createdBy = ((IdentityRef)linkedWorkItem.Fields["System.CreatedBy"]).DisplayName;
                    var createdDate = linkedWorkItem.Fields["System.CreatedDate"];
                    var changedBy = ((IdentityRef)linkedWorkItem.Fields["System.ChangedBy"]).DisplayName;
                    var changedDate = linkedWorkItem.Fields["System.ChangedDate"];

                    newWorkItemShape.CellsU["Prop.PageName"].FormulaU = workItemType.ToString().WrapInDblQuotes();
                    newWorkItemShape.CellsU["Prop.ID"].FormulaU = id.ToString().WrapInDblQuotes();

                    newWorkItemShape.CellsU["Prop.Title"].FormulaU = title.ToString().WrapInDblQuotes();
                    newWorkItemShape.CellsU["Prop.State"].FormulaU = state.ToString().WrapInDblQuotes();
                    newWorkItemShape.CellsU["Prop.CreatedDate"].FormulaU = createdDate.ToString().WrapInDblQuotes();
                    newWorkItemShape.CellsU["Prop.CreatedBy"].FormulaU = createdBy.WrapInDblQuotes();
                    newWorkItemShape.CellsU["Prop.ChangedDate"].FormulaU = changedDate.ToString().WrapInDblQuotes();
                    newWorkItemShape.CellsU["Prop.ChangedBy"].FormulaU = changedBy.WrapInDblQuotes();

                    newWorkItemShape.CellsU["Prop.Organization"].FormulaU = relatedShape.Organization.WrapInDblQuotes();
                    newWorkItemShape.CellsU["Prop.TeamProject"].FormulaU = relatedShape.TeamProject.WrapInDblQuotes();
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

        public static async Task<IList<WorkItem>> QueryWorkItemLinks(string organization, int id)
        {
            //var uri = new Uri("https://dev.azure.com/BD-STS-PROD");
            var uri = new Uri($"https://dev.azure.com/{organization}");
            //var uri = new Uri($"https:////dev.azure.com//{organization}");

            var credentials = GetVssCredentials();

            var project = "VNC Agile";

            var wiql = new Wiql()
            {
                // NOTE: Even if other columns are specified, only the ID & URL are available in the WorkItemReference
                Query = "Select [Id] " +
                    "From WorkItemLinks " +
                    "Where Source.[System.Id] = " + id
            };

            // create instance of work item tracking http client
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

                // build a list of the fields we want to see
                var fields = new[]
                { "System.Id", "System.WorkItemType"
                    , "System.Title", "System.State"
                    , "System.CreatedDate", "System.CreatedBy"
                    , "System.ChangedDate", "System.ChangedBy"};

                // get work items for the ids found in query
                return await httpClient.GetWorkItemsAsync(ids, fields, result.AsOf).ConfigureAwait(false);
            }
        }
    }
}

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

                WorkItemOffsets workItemOffsets = new WorkItemOffsets(initialPosition, 0.375);

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


        public class WorkItemOffset
        {

            public WorkItemOffset(Point initialOffset, double overflowOffset)
            {
                _x = _xInitial = initialOffset.X;
                _y = _yInitial = initialOffset.Y;

                _rowOffset = overflowOffset;

                PadX = 0.05;
                PadY = 0.05;
            }

            public WorkItemOffset(Point initialOffset, double overflowOffset, double padX, double padY)
            {
                _x = _xInitial = initialOffset.X;
                _y = _yInitial = initialOffset.Y;

                _rowOffset = overflowOffset;
            }

            public double PadX { get; set; }
            public double PadY { get; set; }

            public double RowOffset
            {
                get => _rowOffset;
                set => _rowOffset = value;
            }
            
            private double _rowOffset;
            private double _y;
            private double _x;

            private double _yInitial;
            private double _xInitial;
            private int _count;

            public int Count
            {
                get => _count;
                set => _count = value;
            }

            public double X
            {
                get => _x;
                set => _x = value;
            }

            
            public double Y
            {
                get => _y;
                set => _y = value;
            }

            public void DecrementHorizontal(double offset)
            {
                //if (Count % 5 == 0)
                //{
                //    _y += RowOffset;
                //    _y += RowOffset > 0 ? PadY : -PadY;
                //    _x = _xInitial;
                //}

                _x -= offset;
                _x -= PadX;

                _count++;
            }

            public void IncrementHorizontal(double offset)
            {
                //if (Count % 5 == 0)
                //{
                //    _y += RowOffset;
                //    _y += RowOffset > 0 ? PadY : -PadY;
                //    _x = _xInitial;
                //}

                _x += offset;
                _x += PadX;

                _count++;
            }

            public void DecrementHorizontal(double offset, OffsetDirection offsetDirection)
            {
                if (Count % 5 == 0)
                {
                    if (offsetDirection == OffsetDirection.Up)
                    {
                        _y += RowOffset + PadY;
                    }
                    else
                    {
                        _y -= RowOffset + PadY;
                    }

                    _x = _xInitial;
                }

                _x -= offset + PadX;
                //_x -= PadX;

                _count++;
            }

            public void IncrementHorizontal(double offset, OffsetDirection offsetDirection)
            {
                if (Count % 5 == 0)
                {
                    if (offsetDirection == OffsetDirection.Up)
                    {
                        _y += RowOffset + PadY;
                    }
                    else
                    {
                        _y -= RowOffset + PadY;
                    }

                    _x = _xInitial;
                }

                _x += offset + PadX;
                //_x += PadX;

                _count++;
            }
        }

    public enum OffsetDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public class WorkItemOffsets
    {
            
        public WorkItemOffsets(Point initialOffset, double height)
        {
            Bug = new WorkItemOffset(initialOffset, height);
            Epic = new WorkItemOffset(initialOffset, height);
            Feature = new WorkItemOffset(initialOffset, height);
            Task = new WorkItemOffset(initialOffset, height);
            TestCase = new WorkItemOffset(initialOffset, height);
            UserStory = new WorkItemOffset(initialOffset, height);

            Unknown = new WorkItemOffset(initialOffset, 0.0);
        }

        public WorkItemOffset Bug;
        public WorkItemOffset Epic;
        public WorkItemOffset Feature;
        public WorkItemOffset Task;
        public WorkItemOffset TestCase;
        public WorkItemOffset UserStory;
        public WorkItemOffset Unknown;
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
                return Task = Task += offset;
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
                            workItemOffsets.Bug.DecrementHorizontal(width);
                            break;

                        case "Epic":
                            workItemOffsets.Bug.DecrementHorizontal(width);
                            break;

                        case "Feature":
                            workItemOffsets.Bug.DecrementHorizontal(width);
                            break;

                        case "Task":
                            workItemOffsets.Bug.DecrementHorizontal(width);
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
                                workItemOffsets.Bug.DecrementHorizontal(width, OffsetDirection.Down);
                                newInsertionPoint.X = workItemOffsets.Bug.X;
                                newInsertionPoint.Y = workItemOffsets.Bug.Y;
                            }
                            
                            break;

                        case "User Story":
                            workItemOffsets.Bug.DecrementHorizontal(width, OffsetDirection.Down);
                            newInsertionPoint.X = workItemOffsets.Bug.X;
                            newInsertionPoint.Y = workItemOffsets.Bug.Y;
                            break;

                        default:
                            break;
                    }

                    //newInsertionPoint.X = workItemOffsets.Bug.X;
                    //newInsertionPoint.Y = workItemOffsets.Bug.Y;

                    break;

                case "Epic":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            workItemOffsets.Epic.DecrementHorizontal(width);
                            break;

                        case "Epic":
                            workItemOffsets.Epic.DecrementHorizontal(width);
                            break;

                        case "Feature":
                            workItemOffsets.Epic.DecrementHorizontal(width);
                            break;

                        case "Task":
                            workItemOffsets.Epic.DecrementHorizontal(width);
                            break;

                        case "Test Case":
                            workItemOffsets.Epic.DecrementHorizontal(width);
                            break;

                        case "User Story":
                            workItemOffsets.Epic.DecrementHorizontal(width);
                            break;

                        default:
                            break;
                    }

                    newInsertionPoint.X = workItemOffsets.Epic.X;
                    newInsertionPoint.Y = workItemOffsets.Epic.Y;

                    break;

                case "Feature":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            workItemOffsets.Feature.DecrementHorizontal(width);
                            break;

                        case "Epic":
                            workItemOffsets.Feature.DecrementHorizontal(width);
                            break;

                        case "Feature":
                            workItemOffsets.Feature.DecrementHorizontal(width);
                            break;

                        case "Task":
                            workItemOffsets.Feature.DecrementHorizontal(width);
                            break;

                        case "Test Case":
                            workItemOffsets.Feature.DecrementHorizontal(width);
                            break;

                        case "User Story":
                            workItemOffsets.Feature.DecrementHorizontal(width, OffsetDirection.Up);
                            break;

                        default:
                            break;
                    }

                    newInsertionPoint.X = workItemOffsets.Feature.X;
                    newInsertionPoint.Y = workItemOffsets.Feature.Y;

                    break;

                case "Task":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            workItemOffsets.Task.IncrementHorizontal(width);
                            break;

                        case "Epic":
                            workItemOffsets.Task.IncrementHorizontal(width);
                            break;

                        case "Feature":
                            workItemOffsets.Task.IncrementHorizontal(width);
                            break;

                        case "Task":
                            workItemOffsets.Task.IncrementHorizontal(width);
                            break;

                        case "Test Case":
                            workItemOffsets.Task.IncrementHorizontal(width);
                            break;

                        case "User Story":
                            workItemOffsets.Task.IncrementHorizontal(width, OffsetDirection.Down);
                            break;

                        default:
                            break;
                    }

                    newInsertionPoint.X = workItemOffsets.Task.X;
                    newInsertionPoint.Y = workItemOffsets.Task.Y;

                    break;

                case "Test Case":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            workItemOffsets.TestCase.IncrementHorizontal(width, OffsetDirection.Up);
                            break;

                        case "Epic":
                            workItemOffsets.TestCase.IncrementHorizontal(width);
                            break;

                        case "Feature":
                            workItemOffsets.TestCase.IncrementHorizontal(width);
                            break;

                        case "Task":
                            workItemOffsets.TestCase.IncrementHorizontal(width);
                            break;

                        case "Test Case":
                            workItemOffsets.TestCase.IncrementHorizontal(width);
                            break;

                        case "User Story":
                            workItemOffsets.TestCase.IncrementHorizontal(width, OffsetDirection.Up);
                            break;

                        default:
                            break;

                    }

                    newInsertionPoint.X = workItemOffsets.TestCase.X;
                    newInsertionPoint.Y = workItemOffsets.TestCase.Y;

                    break;

                case "User Story":
                    switch (shapeWorkItemType)
                    {
                        case "Bug":
                            workItemOffsets.UserStory.IncrementHorizontal(width, OffsetDirection.Up);
                            break;

                        case "Epic":
                            workItemOffsets.UserStory.IncrementHorizontal(width);
                            break;

                        case "Feature":
                            workItemOffsets.UserStory.IncrementHorizontal(width, OffsetDirection.Down);
                            break;

                        case "Task":
                            workItemOffsets.UserStory.DecrementHorizontal(width, OffsetDirection.Up);
                            break;

                        case "Test Case":
                            if (workItemOffsets.Bug.Count > 0)
                            {
                                workItemOffsets.Bug.DecrementHorizontal(width, OffsetDirection.Down);
                            }
                            else
                            {
                                workItemOffsets.UserStory.DecrementHorizontal(width, OffsetDirection.Down);
                            }
                            break;

                        case "User Story":
                            workItemOffsets.UserStory.IncrementHorizontal(width);
                            break;

                        default:
                            break;

                    }

                    newInsertionPoint.X = workItemOffsets.UserStory.X;
                    newInsertionPoint.Y = workItemOffsets.UserStory.Y;

                    break;

                default:
                    newInsertionPoint.X = initialPosition.X;
                    newInsertionPoint.Y = initialPosition.Y;
                    break;
            }

            return newInsertionPoint;
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


                        var workItemType = linkedWorkItem.Fields["System.WorkItemType"];
                        var title = linkedWorkItem.Fields["System.Title"];
                        var state = linkedWorkItem.Fields["System.State"];
                        var createdBy = ((IdentityRef)linkedWorkItem.Fields["System.CreatedBy"]).DisplayName;
                        var createdDate = linkedWorkItem.Fields["System.CreatedDate"];
                        var changedBy = ((IdentityRef)linkedWorkItem.Fields["System.ChangedBy"]).DisplayName;
                        var changedDate = linkedWorkItem.Fields["System.ChangedDate"];

                        newWorkItemShape.CellsU["Prop.PageName"].FormulaU = workItemType.ToString().WrapInDblQuotes();
                        newWorkItemShape.CellsU["Prop.ID"].FormulaU = id.ToString().WrapInDblQuotes();

                        var cleanTitle = title.ToString().Replace("\"", "\"\"").WrapInDblQuotes();

                        newWorkItemShape.CellsU["Prop.Title"].FormulaU = cleanTitle;
                        newWorkItemShape.CellsU["Prop.State"].FormulaU = state.ToString().WrapInDblQuotes();
                        newWorkItemShape.CellsU["Prop.CreatedDate"].FormulaU = createdDate.ToString().WrapInDblQuotes();
                        newWorkItemShape.CellsU["Prop.CreatedBy"].FormulaU = createdBy.WrapInDblQuotes();
                        newWorkItemShape.CellsU["Prop.ChangedDate"].FormulaU = changedDate.ToString().WrapInDblQuotes();
                        newWorkItemShape.CellsU["Prop.ChangedBy"].FormulaU = changedBy.WrapInDblQuotes();
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

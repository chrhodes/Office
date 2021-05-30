﻿using System;
using System.Reflection;
using System.Windows;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi;

using SupportTools_Visio.Domain;

using VNC;
using VNC.Core;

using Visio = Microsoft.Office.Interop.Visio;
using VisioHelper = VNC.AddinHelper.Visio;

namespace SupportTools_Visio.Actions
{
    public partial class AZDOActions
    {
        public static VNC.WPF.Presentation.Dx.Views.DxThemedWindowHost addLinkedWorkItemsHost = null;

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

            WorkItemShapeInfo activeShapeWorkItemInfo = new WorkItemShapeInfo(activeShape);

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
 
            var result = await VNC.AZDO1.Helper.QueryWorkItemLinks(activeShapeWorkItemInfo.Organization, id, relatedLinkCount);

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

                    insertionPoint = CalculateInsertionPointLinkedWorkItems(initialPosition, insertionPoint, linkedWorkItem, activeShapeWorkItemInfo, workItemOffsets);

                    AddNewLinkedWorkItemShape(activePage, linkMaster, linkedWorkItem, insertionPoint, activeShapeWorkItemInfo);
                }
            }

            VisioHelper.DisplayInWatchWindow($"{activeShapeWorkItemInfo}");
        }

        internal static async void AddLinkedWorkItems2(Visio.Application app, string doc, string page, string shape, string shapeu, string[] vs)
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

            WorkItemShapeInfo activeShapeWorkItemInfo = new WorkItemShapeInfo(activeShape);

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

            var result = await VNC.AZDO1.Helper.QueryWorkItemLinks(activeShapeWorkItemInfo.Organization, id, relatedLinkCount);

            if (result.Count > 0)
            {
                Point initialPosition = GetPosition(activeShape);
                Point insertionPoint = initialPosition;

                string stencilName = "Azure DevOps.vssx";
                string shapeName = "WI 2";
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

                WorkItemOffsets workItemOffsets = new WorkItemOffsets(initialPosition, activeShapeWorkItemInfo.Height, 0.25, 0.05);

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

                    insertionPoint = CalculateInsertionPointLinkedWorkItems(initialPosition, insertionPoint, linkedWorkItem, activeShapeWorkItemInfo, workItemOffsets);

                    AddNewLinkedWorkItemShape2(linkMaster, activePage, linkedWorkItem, insertionPoint, activeShapeWorkItemInfo);
                }
            }

            VisioHelper.DisplayInWatchWindow($"{activeShapeWorkItemInfo}");
        }

        internal static async void GetWorkItemInfo1(Visio.Application app, string doc, string page, string shape, string shapeu, string[] vs)
        {
            VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
                MethodBase.GetCurrentMethod().Name));

            Visio.Page activePage = app.ActivePage;
            Visio.Shape activeShape = app.ActivePage.Shapes[shape];

            WorkItemShapeInfo workItemShapeInfo = new WorkItemShapeInfo(activeShape);

            int id = 0;

            if ( !int.TryParse(workItemShapeInfo.ID, out id))
            {
                MessageBox.Show($"Invalid WorkItem ID: ({workItemShapeInfo.ID})");
                return;
            }
            
            var result = await VNC.AZDO1.Helper.QueryWorkItemInfoById(workItemShapeInfo.Organization, id);

            if (result.Count == 0)
            {
                MessageBox.Show($"Cannot find WorkItem ID: ({workItemShapeInfo.ID})");
                return;
            }

            workItemShapeInfo.InitializeFromWorkItem(result[0]);

            workItemShapeInfo.PopulateShapeDataFromInfo(activeShape, WorkItemShapeInfo.WorkItemShapeVersion.V1);

            VisioHelper.DisplayInWatchWindow($"{workItemShapeInfo}");
        }

        internal static async void GetWorkItemInfo2(Visio.Application app, string doc, string page, string shape, string shapeu, string[] vs)
        {
            VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
                MethodBase.GetCurrentMethod().Name));

            Visio.Page activePage = app.ActivePage;
            Visio.Shape activeShape = app.ActivePage.Shapes[shape];

            WorkItemShapeInfo workItemInfoShape = new WorkItemShapeInfo(activeShape);

            int id = 0;

            if (!int.TryParse(workItemInfoShape.ID, out id))
            {
                MessageBox.Show($"Invalid WorkItem ID: ({workItemInfoShape.ID})");
                return;
            }

            var result = await VNC.AZDO1.Helper.QueryWorkItemInfoById(workItemInfoShape.Organization, id);

            if (result.Count == 0)
            {
                MessageBox.Show($"Cannot find WorkItem ID: ({workItemInfoShape.ID})");
                return;
            }

            workItemInfoShape.InitializeFromWorkItem(result[0]);

            workItemInfoShape.PopulateShapeDataFromInfo(activeShape, WorkItemShapeInfo.WorkItemShapeVersion.V2);

            VisioHelper.DisplayInWatchWindow($"{workItemInfoShape}");
        }

        private static void AddNewLinkedWorkItemShape(Visio.Page page, Visio.Master linkMaster,
            WorkItem workItem, Point insertionPoint, 
            WorkItemShapeInfo relatedShape)
        {
            try
            {
                Visio.Shape newWorkItemShape = page.Drop(linkMaster, insertionPoint.X, insertionPoint.Y);
                WorkItemShapeInfo workItemShapeInfo = new WorkItemShapeInfo(newWorkItemShape);
                workItemShapeInfo.InitializeFromWorkItem(workItem);
                workItemShapeInfo.PopulateShapeDataFromInfo(newWorkItemShape, WorkItemShapeInfo.WorkItemShapeVersion.V2);
            }
            catch (Exception ex)
            {
                VisioHelper.DisplayInWatchWindow($"{workItem.Id} - {ex}");
            }
        }

        private static void AddNewLinkedWorkItemShape2(Visio.Master linkMaster, Visio.Page page, 
            WorkItem workItem, Point insertionPoint, 
            WorkItemShapeInfo relatedShape)
        {
            try
            {
                Visio.Shape newWorkItemShape = page.Drop(linkMaster, insertionPoint.X, insertionPoint.Y);
                WorkItemShapeInfo workItemShapeInfo = new WorkItemShapeInfo(newWorkItemShape);
                workItemShapeInfo.InitializeFromWorkItem(workItem);
                workItemShapeInfo.PopulateShapeDataFromInfo(newWorkItemShape, WorkItemShapeInfo.WorkItemShapeVersion.V2);
            }
            catch (Exception ex)
            {
                VisioHelper.DisplayInWatchWindow($"{workItem.Id} - {ex}");
            }
        }

        private static Point CalculateInsertionPointLinkedWorkItems(Point initialPosition, Point insertionPoint,
            WorkItem linkedWorkItem, WorkItemShapeInfo activeShape, WorkItemOffsets workItemOffsets)
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
                            if (workItemOffsets.UserNeeds.Count > 0)
                            {
                                workItemOffsets.UserStory.IncrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.UserNeeds.X;
                                newInsertionPoint.Y = workItemOffsets.UserNeeds.Y;
                            }
                            else if (workItemOffsets.UserStory.Count > 0)
                            {
                                workItemOffsets.UserStory.IncrementHorizontal(width, OffsetDirection.Down);
                                newInsertionPoint.X = workItemOffsets.UserStory.X;
                                newInsertionPoint.Y = workItemOffsets.UserStory.Y;
                            }
                            else
                            {
                                workItemOffsets.Feature.DecrementHorizontal(width, OffsetDirection.Up);
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
                            if (workItemOffsets.UserNeeds.Count > 0)
                            {
                                workItemOffsets.UserNeeds.DecrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.UserNeeds.X;
                                newInsertionPoint.Y = workItemOffsets.UserNeeds.Y;
                            }
                            else
                            {
                                workItemOffsets.Release.DecrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.Release.X;
                                newInsertionPoint.Y = workItemOffsets.Release.Y;
                            }
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
                                workItemOffsets.Release.DecrementHorizontal(width, OffsetDirection.Up);
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
                            if (workItemOffsets.Release.Count > 0)
                            {
                                workItemOffsets.Release.DecrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.Release.X;
                                newInsertionPoint.Y = workItemOffsets.Release.Y;
                            }
                            else
                            {
                                workItemOffsets.UserNeeds.DecrementHorizontal(width, OffsetDirection.Up);
                                newInsertionPoint.X = workItemOffsets.UserNeeds.X;
                                newInsertionPoint.Y = workItemOffsets.UserNeeds.Y;
                            }

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

        private static Point GetPosition(Visio.Shape shape)
        {
            double x = 5.5;
            double y = 2.0;

            x = shape.CellsU["PinX"].ResultIU;
            y = shape.CellsU["PinY"].ResultIU;

            Point currentPosition = new Point(x, y);

            return currentPosition;
        }

        public static async void QueryWorkItems(Visio.Application app, string doc, string page, string shape, string shapeu, string[] array)
        {
            Int64 startTicks = Log.APPLICATION("Enter", Common.LOG_CATEGORY);

            Visio.Page activePage = app.ActivePage;
            Visio.Shape activeShape = app.ActivePage.Shapes[shape];

            WorkItemQueryShapeInfo shapeInfo = new WorkItemQueryShapeInfo(activeShape);

            int id = 0;

            if (!int.TryParse(shapeInfo.ID, out id))
            {
                MessageBox.Show($"Invalid WorkItem ID: ({shapeInfo.ID})");
                return;
            }

            var result = await VNC.AZDO1.Helper.QueryWorkItemInfoById(shapeInfo.Organization, int.Parse(shapeInfo.ID));

            var workItem = result[0];

            var teamProject = workItem.Fields["System.TeamProject"];
            var workItemType = workItem.Fields["System.WorkItemType"];

            var title = workItem.Fields["System.Title"];
            var state = workItem.Fields["System.State"];

            var createdBy = ((IdentityRef)workItem.Fields["System.CreatedBy"]).DisplayName;
            var createdDate = workItem.Fields["System.CreatedDate"];
            var changedBy = ((IdentityRef)workItem.Fields["System.ChangedBy"]).DisplayName;
            var changedDate = workItem.Fields["System.ChangedDate"];

            var relatedLinkCount = workItem.Fields["System.RelatedLinkCount"];
            var externalLinkCount = workItem.Fields["System.ExternalLinkCount"];
            var remoteLinkCount = workItem.Fields["System.RemoteLinkCount"];
            var hyperLinkCount = workItem.Fields["System.HyperLinkCount"];

            //// Map the properties to the corresponding Prop Data fields on the generic shape

            //activeShape.CellsU["Prop.TextUpper2"].FormulaU = createdBy.WrapInDblQuotes();
            //activeShape.CellsU["Prop.TextUpper1"].FormulaU = createdDate.ToString().WrapInDblQuotes();

            //activeShape.CellsU["Prop.TextHeader1"].FormulaU = id.ToString().WrapInDblQuotes();
            //activeShape.CellsU["Prop.TextHeader2"].FormulaU = teamProject.ToString().WrapInDblQuotes();

            //activeShape.CellsU["Prop.WorkItemType"].FormulaU = workItemType.ToString().WrapInDblQuotes();

            ////activeShape.CellsU["Prop.TextFooter2"].FormulaU = state.ToString().WrapInDblQuotes();
            //activeShape.CellsU["Prop.TextFooter1"].FormulaU = state.ToString().WrapInDblQuotes();

            //activeShape.CellsU["Prop.TextLower1"].FormulaU = changedBy.WrapInDblQuotes();
            //activeShape.CellsU["Prop.TextLower2"].FormulaU = changedDate.ToString().WrapInDblQuotes();

            //// Add the custom Prop Data fields

            //activeShape.CellsU["Prop.Title"].FormulaU = title.ToString().Replace("\"", "\"\"").WrapInDblQuotes();

            //activeShape.CellsU["Prop.ExternalLink"].FormulaU = $"http://dev.azure.com/{workItemInfoShape.Organization}/{teamProject}/_workitems/edit/{id}/".WrapInDblQuotes();

            //activeShape.CellsU["Prop.RelatedLinks"].FormulaU = relatedLinkCount.ToString().WrapInDblQuotes();
            //activeShape.CellsU["Prop.ExternalLinks"].FormulaU = externalLinkCount.ToString().WrapInDblQuotes();
            //activeShape.CellsU["Prop.RemoteLinks"].FormulaU = remoteLinkCount.ToString().WrapInDblQuotes();
            //activeShape.CellsU["Prop.HyperLinks"].FormulaU = hyperLinkCount.ToString().WrapInDblQuotes();

            //// Most likely PageName

            //activeShape.CellsU["Prop.PageName"].FormulaU = $"{workItemType} {id}".WrapInDblQuotes();

            Log.APPLICATION("Exit", Common.LOG_CATEGORY, startTicks);
        }
    }
}
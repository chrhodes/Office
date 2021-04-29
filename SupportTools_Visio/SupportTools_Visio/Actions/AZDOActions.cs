using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

using SupportTools_Visio.Domain;

using VNC.Core;

using Visio = Microsoft.Office.Interop.Visio;
using VisioHelper = VNC.AddinHelper.Visio;

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

            var result = await QueryWorkItemInfo(int.Parse(workItemInfoShape.ID));

            var workItemType = result[0].Fields["System.WorkItemType"];
            var title = result[0].Fields["System.Title"];
            var state = result[0].Fields["System.State"];
            var createdBy = ((IdentityRef)result[0].Fields["System.CreatedBy"]).DisplayName;
            var createdDate = result[0].Fields["System.CreatedDate"];
            var changedBy = ((IdentityRef)result[0].Fields["System.ChangedBy"]).DisplayName;
            var changedDate = result[0].Fields["System.ChangedDate"];

            activeShape.CellsU["Prop.PageName"].FormulaU = workItemType.ToString().WrapInDblQuotes();

            activeShape.CellsU["Prop.Title"].FormulaU = title.ToString().WrapInDblQuotes();
            activeShape.CellsU["Prop.State"].FormulaU = state.ToString().WrapInDblQuotes();
            activeShape.CellsU["Prop.CreatedDate"].FormulaU = createdDate.ToString().WrapInDblQuotes();
            activeShape.CellsU["Prop.CreatedBy"].FormulaU = createdBy.WrapInDblQuotes();
            activeShape.CellsU["Prop.ChangedDate"].FormulaU = changedDate.ToString().WrapInDblQuotes();
            activeShape.CellsU["Prop.ChangedBy"].FormulaU = changedBy.WrapInDblQuotes();

            VisioHelper.DisplayInWatchWindow($"{workItemInfoShape}");
        }

        public static async Task<IList<WorkItem>> QueryWorkItemInfo(int id)
        {

            var uri = new Uri("https://dev.azure.com/BD-STS-PROD");
            //var uri = new Uri("https://dev.azure.com/VNC-Development");

            var credentials = new VssBasicCredential(string.Empty, "ysjgjvglxsoddug3lja6r7vu4ayrraonkqiul6n4hjwylnmatila");

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

        internal static async void AddLinkedWorkItems(Visio.Application app, string doc, string page, string shape, string shapeu, string[] vs)
        {
            VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
                MethodBase.GetCurrentMethod().Name));

            Visio.Page activePage = app.ActivePage;
            Visio.Shape activeShape = app.ActivePage.Shapes[shape];

            WorkItemInfoShape workItemInfoShape = new WorkItemInfoShape(activeShape);

            var result = await QueryWorkItemLinks(int.Parse(workItemInfoShape.ID));

            foreach (var linkedWorkItem in result)
            {
                // NOTE(crhodes)
                // This includes the current shape.  Do not add it.

                VisioHelper.DisplayInWatchWindow($"{linkedWorkItem.Id} {linkedWorkItem.Fields["System.Title"]}");

                AddNewLinkedWorkItemShape(app, activePage, linkedWorkItem);
            }

            VisioHelper.DisplayInWatchWindow($"{workItemInfoShape}");
        }

        private static void AddNewLinkedWorkItemShape(Visio.Application app, Visio.Page page, WorkItem linkedWorkItem)
        {
            string stencilName = "Azure DevOps.vssx";
            string shapeName = "WI & Info";

            try
            {
                Visio.Document linkStencil = app.Documents[stencilName];

                try
                {
                    Visio.Master linkMaster = linkStencil.Masters[shapeName];

                    Visio.Shape newWorkItemShape = page.Drop(linkMaster, 4.0, 4.0);

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

        public static async Task<IList<WorkItem>> QueryWorkItemLinks(int id)
        {

            var uri = new Uri("https://dev.azure.com/BD-STS-PROD");
            //var uri = new Uri("https://dev.azure.com/VNC-Development");

            var credentials = new VssBasicCredential(string.Empty, "ysjgjvglxsoddug3lja6r7vu4ayrraonkqiul6n4hjwylnmatila");

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

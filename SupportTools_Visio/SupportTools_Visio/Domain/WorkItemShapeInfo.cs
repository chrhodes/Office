using System;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi;

using VNC.Core;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Actions
{
    public class WorkItemShapeInfo : ShapeInfo
    {
        public enum WorkItemShapeVersion
        {
            V1,
            V2
        }

        #region Constructors and Load

        public WorkItemShapeInfo(Visio.Shape shape) : base(shape)
        {
            // NOTE(crhodes)
            // These Four Properties are used by the Actions that can be performed.
            // Populate them from the activeShape which could be a WorkItemInfo or a QueryInfo shape.
            //
            // This has a little logic to handle the differences between WI 1.0 and WI 2.0

            Organization = Helper.GetShapePropertyAsString(shape, "Organization");
            TeamProject = Helper.GetShapePropertyAsString(shape, "TeamProject");

            ID = Helper.GetShapePropertyAsString(shape, "ID");

            //var exists0 = shape.CellExistsU["Prop.WorkItemType", 0];
            //var exists1 = shape.CellExistsU["Prop.WorkItemType", 1];

            //var exists0A = shape.CellExistsU["Prop.RelatedLinks", 0];
            //var exists1A = shape.CellExistsU["Prop.RelatedLinks", 1];

            if (shape.CellExistsU["Prop.WorkItemType", 0] != 0)
            {
                WorkItemType = shape.CellsU["Prop.WorkItemType"].ResultStr[Visio.VisUnitCodes.visUnitsString];

                // NB. WI 1.0 used PageName for WorkItemType.  We can remove this if we stop supported WI 1.0

                if (WorkItemType == "")
                {
                    WorkItemType = shape.CellsU["Prop.PageName"].ResultStr[Visio.VisUnitCodes.visUnitsString];
                }
            }

            // TODO(crhodes)
            // See if we really need to get this in constructor.

            if (shape.CellExistsU["Prop.RelatedLinks", 0] != 0)
            {
                RelatedLinkCount = shape.CellsU["Prop.RelatedLinks"].ResultStr[Visio.VisUnitCodes.visUnitsString];
            }

            //// NB. WI 1.0 used PageName for WorkItemType.  We can remove this if we stop supported WI 1.0

            //if (WorkItemType == "")
            //{
            //    WorkItemType = shape.CellsU["Prop.PageName"].ResultStr[Visio.VisUnitCodes.visUnitsString];
            //}

            // All the other properties are populated when getting fields from the WorkItem
            // identified by Organization and ID
        }

        public WorkItemShapeInfo(Visio.Shape shape, WorkItemShapeInfo relatedShape) : base(shape)
        {
            ID = Helper.GetShapePropertyAsString(shape, "ID");


            if (shape.CellExistsU["Prop.WorkItemType", 0] != 0)
            {
                WorkItemType = shape.CellsU["Prop.WorkItemType"].ResultStr[Visio.VisUnitCodes.visUnitsString];

                // NB. WI 1.0 used PageName for WorkItemType.  We can remove this if we stop supported WI 1.0

                if (WorkItemType == "")
                {
                    WorkItemType = shape.CellsU["Prop.PageName"].ResultStr[Visio.VisUnitCodes.visUnitsString];
                }
            }

            if (shape.CellExistsU["Prop.RelatedLinks", 0] != 0)
            {
                RelatedLinkCount = shape.CellsU["Prop.RelatedLinks"].ResultStr[Visio.VisUnitCodes.visUnitsString];
            }
        }

        #endregion Constructors and Load

        #region Enums, Fields, Properties, Structures

        public string WorkItemType { get; set; }

        public string Organization { get; set; }
        public string Url { get; set; }

        public string TeamProject { get; set; }

        public string ID { get; set; }

        public string Title { get; set; }

        public string State { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ChangedBy { get; set; }
        public string ChangedDate { get; set; }

        public string RelatedLinkCount { get; set; }
        public string ExternalLinkCount { get; set; }
        public string RemoteLinkCount { get; set; }
        public string HyperLinkCount { get; set; }

        #endregion Enums, Fields, Properties, Structures

        #region Main Methods

        public void InitializeFromWorkItem(WorkItem workItem)
        {
            Url = workItem.Url;
            Organization = VNC.AZDO1.Helper.GetOrganizationNameFromUrl(Url);

            ID = workItem.Fields["System.Id"].ToString();

            TeamProject = (string)workItem.Fields["System.TeamProject"];
            WorkItemType = (string)workItem.Fields["System.WorkItemType"];

            // NOTE(crhodes)
            // Handle special characters
            Title = workItem.Fields["System.Title"].ToString().Replace("\"", "\"\"");

            State = workItem.Fields["System.State"].ToString();

            CreatedBy = ((IdentityRef)workItem.Fields["System.CreatedBy"]).DisplayName;
            CreatedDate = workItem.Fields["System.CreatedDate"].ToString();
            ChangedBy = ((IdentityRef)workItem.Fields["System.ChangedBy"]).DisplayName;
            ChangedDate = workItem.Fields["System.ChangedDate"].ToString();

            RelatedLinkCount = workItem.Fields["System.RelatedLinkCount"].ToString();
            ExternalLinkCount = workItem.Fields["System.ExternalLinkCount"].ToString();
            RemoteLinkCount = workItem.Fields["System.RemoteLinkCount"].ToString();
            HyperLinkCount = workItem.Fields["System.HyperLinkCount"].ToString();
        }

        public void PopulateShapeDataFromInfo(Visio.Shape shape, WorkItemShapeVersion shapeVersion)
        {
            // These changed between V1 and V2

            if (shapeVersion.Equals(WorkItemShapeVersion.V1))
            {
                shape.CellsU["Prop.CreatedBy"].FormulaU = CreatedBy.WrapInDblQuotes();
                shape.CellsU["Prop.CreatedDate"].FormulaU = CreatedDate.WrapInDblQuotes();

                shape.CellsU["Prop.TeamProject"].FormulaU = TeamProject.WrapInDblQuotes();

                shape.CellsU["Prop.PageName"].FormulaU = WorkItemType.WrapInDblQuotes();

                shape.CellsU["Prop.State"].FormulaU = State.WrapInDblQuotes();

                shape.CellsU["Prop.ChangedBy"].FormulaU = ChangedBy.WrapInDblQuotes();
                shape.CellsU["Prop.ChangedDate"].FormulaU = ChangedDate.WrapInDblQuotes();
            }
            else
            {
                // Map the properties to the corresponding Prop Data fields on the generic shape

                shape.CellsU["Prop.TextUpper2"].FormulaU = CreatedBy.WrapInDblQuotes();
                shape.CellsU["Prop.TextUpper1"].FormulaU = CreatedDate.WrapInDblQuotes();

                shape.CellsU["Prop.TextHeader2"].FormulaU = TeamProject.WrapInDblQuotes();

                shape.CellsU["Prop.WorkItemType"].FormulaU = WorkItemType.WrapInDblQuotes();

                //shape.CellsU["Prop.TextFooter2"].FormulaU = state.ToString().WrapInDblQuotes();
                shape.CellsU["Prop.TextFooter1"].FormulaU = State.WrapInDblQuotes();

                shape.CellsU["Prop.TextLower1"].FormulaU = ChangedBy.WrapInDblQuotes();
                shape.CellsU["Prop.TextLower2"].FormulaU = ChangedDate.WrapInDblQuotes();

                // Most likely PageName

                shape.CellsU["Prop.PageName"].FormulaU = $"{WorkItemType} {ID}".WrapInDblQuotes();
            }

            // These didn't change

            shape.CellsU["Prop.Organization"].FormulaU = Organization.WrapInDblQuotes();
            shape.CellsU["Prop.ID"].FormulaU = ID.WrapInDblQuotes();

            shape.CellsU["Prop.Title"].FormulaU = Title.WrapInDblQuotes();

            shape.CellsU["Prop.RelatedLinks"].FormulaU = RelatedLinkCount.WrapInDblQuotes();
            shape.CellsU["Prop.ExternalLinks"].FormulaU = ExternalLinkCount.WrapInDblQuotes();
            shape.CellsU["Prop.RemoteLinks"].FormulaU = RemoteLinkCount.WrapInDblQuotes();
            shape.CellsU["Prop.HyperLinks"].FormulaU = HyperLinkCount.WrapInDblQuotes();

            shape.CellsU["Prop.ExternalLink"].FormulaU =
                $"http://dev.azure.com/{Organization}/{TeamProject}/_workitems/edit/{ID}/".WrapInDblQuotes();
        }

        public override string ToString()
        {
            return $"{ID} - {Title}";
        }

        #endregion Main Methods
    }
}
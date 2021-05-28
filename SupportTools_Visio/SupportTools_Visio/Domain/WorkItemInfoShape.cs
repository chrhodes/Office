using System;
using System.Collections.Generic;
using System.Linq;
using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Actions
{
    public class WorkItemInfoShape
    {
        #region Constructors and Load

        public WorkItemInfoShape(Visio.Shape activeShape)
        {
            // NOTE(crhodes)
            // These Four Properties are used by the Actions that can be performed.
            // Populate them from the activeShape
            //
            // This has a little logic to handle the differences between WI 1.0 and WI 2.0

            Organization = Helper.GetShapePropertyAsString(activeShape, "Organization");

            ID = Helper.GetShapePropertyAsString(activeShape, "ID");

            WorkItemType = activeShape.CellsU["Prop.WorkItemType"].ResultStr[Visio.VisUnitCodes.visUnitsString];

            RelatedLinkCount = activeShape.CellsU["Prop.RelatedLinks"].ResultStr[Visio.VisUnitCodes.visUnitsString];

            // NB. WI 1.0 used PageName for WorkItemType.  We can remove this f we stop supported WI 1.0

            if (WorkItemType == "")
            {
                WorkItemType = activeShape.CellsU["Prop.PageName"].ResultStr[Visio.VisUnitCodes.visUnitsString];
            }

            // This helps with position output relative to the activeShape

            PinX = activeShape.CellsU["PinX"].ResultIU;
            PinY = activeShape.CellsU["PinY"].ResultIU;

            Height= activeShape.CellsU["Height"].ResultIU;
            Width = activeShape.CellsU["Width"].ResultIU;

            // All the other properties are populated when getting fields from the WorkItem
            // identified by Organization and ID
        }

        #endregion

        #region Enums, Fields, Properties, Structures

        public string WorkItemType { get; set; }
        public double PinX { get; set; }
        public double PinY { get; set; }

        public double Height { get; set; }
        public double Width { get; set; }

        public string Organization { get; set; }
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

        #endregion

        #region Main Methods

        public override string ToString()
        {
            return $"{ID} - {Title}";
        }

        #endregion
    }
}

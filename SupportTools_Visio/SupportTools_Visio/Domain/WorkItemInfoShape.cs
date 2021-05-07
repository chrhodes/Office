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
            // TODO(crhodes)
            // Make this reflect on properties and loop across.

            Organization = Helper.GetShapePropertyAsString(activeShape, "Organization");
            TeamProject = Helper.GetShapePropertyAsString(activeShape, "TeamProject");
            ID = Helper.GetShapePropertyAsString(activeShape, "ID");

            PinX = activeShape.CellsU["PinX"].ResultIU;
            PinY = activeShape.CellsU["PinY"].ResultIU;

            Height= activeShape.CellsU["Height"].ResultIU;
            Width = activeShape.CellsU["Width"].ResultIU;

            // TODO(crhodes)
            // Maybe just use Helper (see infra)
            WorkItemType = activeShape.CellsU["Prop.PageName"].ResultStr[Visio.VisUnitCodes.visUnitsString];

            // // TODO(crhodes)
            // Why not get everything?

            RelatedLinkCount = activeShape.CellsU["Prop.RelatedLinks"].ResultStr[Visio.VisUnitCodes.visUnitsString];

            //Namespace = Helper.GetShapePropertyAsString(activeShape, "Namespace");
            //Version = Helper.GetShapePropertyAsString(activeShape, "Version");
            //Color = Helper.GetShapePropertyAsString(activeShape, "Color");
            //Color2 = Helper.GetShapePropertyAsString(activeShape, "Color2");
            //GroupName = Helper.GetShapePropertyAsString(activeShape, "GroupName");
            //SourceName = Helper.GetShapePropertyAsString(activeShape, "SourceName");
            //RootPath = Helper.GetShapePropertyAsString(activeShape, "RootPath");
            //AssemblyFileName = Helper.GetShapePropertyAsString(activeShape, "AssemblyFileName");
            //SourceFileName = Helper.GetShapePropertyAsString(activeShape, "SourceFileName");
            //ApplicationName = Helper.GetShapePropertyAsString(activeShape, "ApplicationName");
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

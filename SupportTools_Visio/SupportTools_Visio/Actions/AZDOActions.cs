using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using SupportTools_Visio.Domain;

using VNC.Core;

using Visio = Microsoft.Office.Interop.Visio;
using VisioHelper = VNC.AddinHelper.Visio;

namespace SupportTools_Visio.Actions
{
    public class AZDOActions
    {
        internal static void GetWorkItemInfo(Visio.Application app, string doc, string page, string shape, string shapeu, string[] vs)
        {
            VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
                MethodBase.GetCurrentMethod().Name));

            Visio.Page activePage = app.ActivePage;
            Visio.Shape activeShape = app.ActivePage.Shapes[shape];

            WorkItemInfoShape workItemInfoShape = new WorkItemInfoShape(activeShape);

            VisioHelper.DisplayInWatchWindow($"{workItemInfoShape}");
        }

        internal static void AddLinkedWorkItems(Visio.Application app, string doc, string page, string shape, string shapeu, string[] vs)
        {
            VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
                MethodBase.GetCurrentMethod().Name));

            Visio.Page activePage = app.ActivePage;
            Visio.Shape activeShape = app.ActivePage.Shapes[shape];

            WorkItemInfoShape workItemInfoShape = new WorkItemInfoShape(activeShape);

            VisioHelper.DisplayInWatchWindow($"{workItemInfoShape}");
        }
    }
}

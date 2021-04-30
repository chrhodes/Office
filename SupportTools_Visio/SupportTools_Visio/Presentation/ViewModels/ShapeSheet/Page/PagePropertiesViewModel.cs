
using System;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class PagePropertiesViewModel : PageShapeSheetSectionBase
    {
        public PagePropertiesViewModel() : base()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            // TODO(crhodes)
            // Decide if we want defaults
            //DocumentPropertiesViewModel = new DocumentPropertiesWrapper(new Domain.DocumentPropertiesViewModel());
            // For now, just display current
            OnLoadCurrentSettingsExecute();

            Log.CONSTRUCTOR("Exit", Common.LOG_APPNAME, startTicks);
        }

        public PagePropertiesWrapper PageProperties { get; set; }

        public override void OnUpdateSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!
            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdatePageProperties");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Shape shape = ((Visio.Page)app.ActivePage).PageSheet;

            Visio_Shape.Set_PageProperties_Section(shape, PageProperties.Model);

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Shape shape = ((Visio.Page)app.ActivePage).PageSheet;

            PageProperties = new PagePropertiesWrapper(Visio_Shape.Get_PageProperties(shape));
            OnPropertyChanged("PageProperties");

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }
    }
}

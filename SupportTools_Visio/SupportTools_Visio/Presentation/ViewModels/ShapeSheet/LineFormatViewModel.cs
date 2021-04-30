
using System;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class LineFormatViewModel : ShapeSheetSectionBase
    {
        public LineFormatViewModel() : base()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            UpdateButtonContent = "Update LineFormat for selected shapes";

            // TODO(crhodes)
            // Decide if we want defaults
            //LineFormatViewModel = new LineFormatWrapper(new Domain.LineFormatViewModel());

            Log.CONSTRUCTOR("Exit", Common.LOG_APPNAME, startTicks);
        }

        public LineFormatWrapper LineFormat { get; set; }

        public override void OnUpdateSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!
            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateControlRow");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_LineFormat_Section(shape, LineFormat.Model);
            }

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            // Verify only one shape, for now just grab first.

            foreach (Visio.Shape shape in selection)
            {
                LineFormat = new LineFormatWrapper(Visio_Shape.Get_LineFormat(shape));
                OnPropertyChanged("LineFormat");
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }
    }
}

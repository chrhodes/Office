
using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class LineFormatViewModel : ShapeSheetSectionBase //, ILineFormatViewModelViewModel
    {
        public LineFormatWrapper LineFormat { get; set; }

        public LineFormatViewModel() : base()
        {
            UpdateButtonContent = "Update LineFormat for selected shapes";

            // TODO(crhodes)
            // Decide if we want defaults
            //LineFormatViewModel = new LineFormatWrapper(new Domain.LineFormatViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateControlRow");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_LineFormat_Section(shape, LineFormat.Model);
            }

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            // Verify only one shape, for now just grab first.

            foreach (Visio.Shape shape in selection)
            {
                LineFormat = new LineFormatWrapper(Visio_Shape.Get_LineFormat(shape));
                OnPropertyChanged("LineFormat");
            }
        }
    }
}


using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class MiscellaneousViewModel : ShapeSheetSectionBase //, IMiscellaneousViewModelViewModel
    {
        public MiscellaneousWrapper Miscellaneous { get; set; }

        public MiscellaneousViewModel() : base()
        {
             UpdateButtonContent = "Update Miscellaneous for selected shapes";

            // TODO(crhodes)
            // Decide if we want defaults
            //MiscellaneousViewModel = new MiscellaneousWrapper(new Domain.MiscellaneousViewModel());
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
                Visio_Shape.Set_Miscellaneous_Section(shape, Miscellaneous.Model);
            }

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Miscellaneous = new MiscellaneousWrapper(Visio_Shape.Get_Miscellaneous(shape));
                OnPropertyChanged("Miscellaneous");
            }
        }
    }
}

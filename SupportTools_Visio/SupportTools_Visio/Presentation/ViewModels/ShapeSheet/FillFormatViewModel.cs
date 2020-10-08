
using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class FillFormatViewModel : ShapeSheetSectionBase //, IFillFormatViewModelViewModel
    {
        public FillFormatWrapper FillFormat { get; set; }


        public FillFormatViewModel() : base()
        {
            UpdateButtonContent = "Update FillFormat for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //FillFormatViewModel = new FillFormatWrapper(new Domain.FillFormatViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateFillFormat");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_FillFormat_Section(shape, FillFormat.Model);
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
                FillFormat = new FillFormatWrapper(Visio_Shape.Get_FillFormat(shape));
                OnPropertyChanged("FillFormat");
            }
        }
    }
}

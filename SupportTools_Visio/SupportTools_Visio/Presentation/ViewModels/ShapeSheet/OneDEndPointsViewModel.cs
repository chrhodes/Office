
using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class OneDEndPointsViewModel : ShapeSheetSectionBase //, IOneDEndPointsViewModelViewModel
    {
        public OneDEndPointsWrapper OneDEndPoints { get; set; }

        public OneDEndPointsViewModel() : base()
        {
            UpdateButtonContent = "Update 1-D Endpoints for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //OneDEndPointsViewModel = new OneDEndPointsWrapper(new Domain.OneDEndPointsViewModel());
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
                Visio_Shape.Set_OneDEndPoints_Section(shape, OneDEndPoints.Model);
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
                OneDEndPoints = new OneDEndPointsWrapper(Visio_Shape.Get_OneDEndPoints(shape));
                OnPropertyChanged("OneDEndPoints");
            }
        }
    }
}

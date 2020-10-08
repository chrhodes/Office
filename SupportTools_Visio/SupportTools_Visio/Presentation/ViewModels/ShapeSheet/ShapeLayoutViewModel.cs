
using Prism.Commands;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class ShapeLayoutViewModel : ShapeSheetSectionBase //, IShapeLayoutViewModelViewModel
    {
        public ShapeLayoutWrapper ShapeLayout { get; set; }

        public ShapeLayoutViewModel() : base()
        {
            UpdateButtonContent = "Update ShapeLayout for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //ShapeLayoutViewModel = new ShapeLayoutWrapper(new Domain.ShapeLayoutViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateShapeLayout");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_ShapeLayout_Section(shape, ShapeLayout.Model);
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
                ShapeLayout = new ShapeLayoutWrapper(Visio_Shape.Get_ShapeLayout(shape));
                OnPropertyChanged("ShapeLayout");
            }
        }
    }
}

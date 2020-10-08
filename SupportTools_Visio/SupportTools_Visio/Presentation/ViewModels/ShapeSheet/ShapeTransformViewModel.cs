
using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class ShapeTransformViewModel : ShapeSheetSectionBase //, IShapeTransformViewModelViewModel
    {
        public ShapeTransformWrapper ShapeTransform { get; set; }

        public ShapeTransformViewModel() : base()
        {
            UpdateButtonContent = "Update ShapeTransform for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //ShapeTransformViewModel = new ShapeTransformWrapper(new Domain.ShapeTransformViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateShapeTransform");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_ShapeTransform_Section(shape, ShapeTransform.Model);
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
                ShapeTransform = new ShapeTransformWrapper(Visio_Shape.Get_ShapeTransform(shape));
                OnPropertyChanged("ShapeTransform");
            }
        }
    }
}

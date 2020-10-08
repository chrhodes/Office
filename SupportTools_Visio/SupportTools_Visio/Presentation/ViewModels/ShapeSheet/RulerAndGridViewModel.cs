
using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class RulerAndGridViewModel : ShapeSheetSectionBase, IRulerAndGridViewModel
    {
        public RulerAndGridWrapper RulerAndGrid { get; set; }

        public RulerAndGridViewModel() : base()
        {
            UpdateButtonContent = "Update RulerAndGrid for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //RuleAndGridViewModel = new RuleAndGridWrapper(new Domain.RuleAndGridViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateRulerAndGrid");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_RulerAndGrid_Section(shape, RulerAndGrid.Model);
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
                RulerAndGrid = new RulerAndGridWrapper(Visio_Shape.Get_RulerAndGrid(shape));
                OnPropertyChanged("RuleAndGrid");
            }
        }
    }
}


using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class ProtectionViewModel : ShapeSheetSectionBase //, IProtectionViewModelViewModel
    {
        public ProtectionWrapper Protection { get; set; }

        public ProtectionViewModel() : base()
        {
            UpdateButtonContent = "Update Protection for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //ProtectionViewModel = new ProtectionWrapper(new Domain.ProtectionViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateProtection");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_Protection_Section(shape, Protection.Model);
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
                Protection = new ProtectionWrapper(Visio_Shape.Get_Protection(shape));
                OnPropertyChanged("Protection");
            }
        }
    }
}

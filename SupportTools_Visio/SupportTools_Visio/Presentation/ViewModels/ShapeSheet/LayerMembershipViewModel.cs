
using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class LayerMembershipViewModel : ShapeSheetSectionBase //, ILayerMembershipViewModelViewModel
    {
        public LayerMembershipWrapper LayerMembership{ get; set; }


        public LayerMembershipViewModel() : base()
        {
            UpdateButtonContent = "Update LayerMembership for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //LayerMembershipViewModel = new LayerMembershipWrapper(new Domain.LayerMembershipViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateLayerMembership");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_LayerMembership_Section(shape, LayerMembership.Model);
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
                LayerMembership = new LayerMembershipWrapper(Visio_Shape.Get_LayerMembership(shape));
                OnPropertyChanged("LayerMembership");
            }
        }
    }
}

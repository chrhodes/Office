
using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class GroupPropertiesViewModel : ShapeSheetSectionBase //, IGroupPropertiesViewModelViewModel
    {
        public GroupPropertiesWrapper GroupProperties { get; set; }


        public GroupPropertiesViewModel() : base()
        {
            UpdateButtonContent = "Update GroupProperties for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //GroupPropertiesViewModel = new GroupPropertiesWrapper(new Domain.GroupPropertiesViewModel());
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
                Visio_Shape.Set_GroupProperties_Section(shape, GroupProperties.Model);
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
                GroupProperties = new GroupPropertiesWrapper(Visio_Shape.Get_GroupProperties(shape));
                OnPropertyChanged("GroupProperties");
            }
        }
    }
}

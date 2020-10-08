
using Prism.Commands;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class BevelPropertiesViewModel : ShapeSheetSectionBase //, IBevelPropertiesViewModel
    {
        public BevelPropertiesWrapper BevelProperties { get; set; }


        public BevelPropertiesViewModel() : base()
        {
            //UpdateSettings = new DelegateCommand(OnUpdateSettingsExecute, OnUpdateSettingsCanExecute);
            //LoadCurrentSettings = new DelegateCommand(OnLoadCurrentSettingsExecute, OnLoadCurrentSettingsCanExecute);

            UpdateButtonContent = "Update BevelProperties for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //BevelPropertiesWrapperViewModel = new BevelPropertiesWrapperWrapper(new Domain.BevelPropertiesWrapperViewModel());
        }

        public void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateBevelProperties");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_BevelPropertiesWrapper_Section(shape, BevelProperties.Model);
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
                BevelProperties = new BevelPropertiesWrapper(Visio_Shape.Get_BevelProperties(shape));
                OnPropertyChanged("BevelProperties");
            }
        }
    }
}

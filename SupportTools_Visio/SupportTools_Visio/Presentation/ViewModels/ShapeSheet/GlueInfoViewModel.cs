
using Prism.Commands;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class GlueInfoViewModel : ShapeSheetSectionBase //, IGlueInfoViewModelViewModel
    {
        public GlueInfoWrapper GlueInfo { get; set; }


        public GlueInfoViewModel() : base()
        {
            //UpdateSettings = new DelegateCommand(OnUpdateSettingsExecute, OnUpdateSettingsCanExecute);
            //LoadCurrentSettings = new DelegateCommand(OnLoadCurrentSettingsExecute, OnLoadCurrentSettingsCanExecute);

            UpdateButtonContent = "Update GlueInfo for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //GlueInfoViewModel = new GlueInfoWrapper(new Domain.GlueInfoViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateGlueInfo");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_GlueInfo_Section(shape, GlueInfo.Model);
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
                GlueInfo = new GlueInfoWrapper(Visio_Shape.Get_GlueInfo(shape));
                OnPropertyChanged("GlueInfo");
            }
        }
    }
}

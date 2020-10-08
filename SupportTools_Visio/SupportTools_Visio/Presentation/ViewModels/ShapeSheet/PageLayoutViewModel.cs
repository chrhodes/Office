
using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class PageLayoutViewModel : ShapeSheetSectionBase //, IPageLayoutViewModelViewModel
    {
        public PageLayoutWrapper PageLayout { get; set; }

        public PageLayoutViewModel() : base()
        {
            UpdateButtonContent = "Update PageLayout for selected shapes";

            // TODO(crhodes)
            // Decide if we want defaults
            //PageLayoutViewModel = new PageLayoutWrapper(new Domain.PageLayoutViewModel());
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
                Visio_Shape.Set_PageLayout_Section(shape, PageLayout.Model);
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
                PageLayout = new PageLayoutWrapper(Visio_Shape.Get_PageLayout(shape));
                OnPropertyChanged("PageLayout");
            }
        }
    }
}

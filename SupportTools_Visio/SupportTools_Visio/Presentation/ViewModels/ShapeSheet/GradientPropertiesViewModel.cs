
using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class GradientPropertiesViewModel : ShapeSheetSectionBase //, IGradientPropertiesViewModelViewModel
    {
        public GradientPropertiesWrapper GradientProperties { get; set; }

        public GradientPropertiesViewModel() : base()
        {
            UpdateButtonContent = "Update GradientProperties for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //GradientPropertiesViewModel = new GradientPropertiesWrapper(new Domain.GradientPropertiesViewModel());
        }

        public void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateControlRow");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;


            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_GradientProperties_Section(shape, GradientProperties.Model);
            }

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        void OnLoadCurrentSettingsExecute()
        {
            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                GradientProperties = new GradientPropertiesWrapper(Visio_Shape.Get_GradientProperties(shape));
                OnPropertyChanged("GradientProperties");
            }
        }
    }
}

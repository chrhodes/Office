
using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class ThemePropertiesViewModel : ShapeSheetSectionBase //, IThemePropertiesViewModelViewModel
    {
        public ThemePropertiesWrapper ThemeProperties { get; set; }

        public ThemePropertiesViewModel() : base()
        {
            UpdateButtonContent = "Update ThemeProperties for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //ThemePropertiesViewModel = new ThemePropertiesWrapper(new Domain.ThemePropertiesViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateThemeProperties");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_ThemeProperties_Section(shape, ThemeProperties.Model);
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
                ThemeProperties = new ThemePropertiesWrapper(Visio_Shape.Get_ThemeProperties(shape));
                OnPropertyChanged("ThemeProperties");
            }
        }
    }
}

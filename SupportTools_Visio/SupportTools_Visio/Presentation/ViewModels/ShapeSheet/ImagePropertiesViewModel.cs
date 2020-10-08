
using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class ImagePropertiesViewModel : ShapeSheetSectionBase, IImagePropertiesViewModel
    {
        public ImagePropertiesWrapper ImageProperties { get; set; }


        public ImagePropertiesViewModel() : base()
        {
            //UpdateSettings = new DelegateCommand(OnUpdateSettingsExecute, OnUpdateSettingsCanExecute);
            //LoadCurrentSettings = new DelegateCommand(OnLoadCurrentSettingsExecute, OnLoadCurrentSettingsCanExecute);

            UpdateButtonContent = "Update ImageProperties for selected shapes";

            // TODO(crhodes)
            // Decide if we want defaults
            //ImagePropertiesViewModel = new ImagePropertiesWrapper(new Domain.ImagePropertiesViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateImageProperties");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_ImageProperties_Section(shape, ImageProperties.Model);
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
                ImageProperties = new ImagePropertiesWrapper(Visio_Shape.Get_ImageProperties(shape));
                OnPropertyChanged("ImageProperties");
            }
        }
    }
}

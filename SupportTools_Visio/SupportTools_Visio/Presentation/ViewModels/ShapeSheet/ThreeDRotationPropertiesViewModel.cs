
using System;

using Prism.Commands;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class ThreeDRotationPropertiesViewModel : ShapeSheetSectionBase
    {
        public ThreeDRotationPropertiesViewModel() : base()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            UpdateButtonContent = "Update 3-D RotationProperties for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //ThreeDRotationPropertiesViewModel = new ThreeDRotationPropertiesWrapper(new Domain.ThreeDRotationPropertiesViewModel());
            Log.CONSTRUCTOR("Exit", Common.LOG_APPNAME, startTicks);
        }

        public ThreeDRotationPropertiesWrapper ThreeDRotationProperties { get; set; }

        public override void OnUpdateSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!
            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("Update3DRotationProperties");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_ThreeDRotationProperties_Section(shape, ThreeDRotationProperties.Model);
            }

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                ThreeDRotationProperties = new ThreeDRotationPropertiesWrapper(Visio_Shape.Get_ThreeDRotationProperties(shape));
                OnPropertyChanged("ThreeDRotationProperties");
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }
    }
}

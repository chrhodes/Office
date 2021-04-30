using System;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class GradientPropertiesViewModel : ShapeSheetSectionBase
    {
        public GradientPropertiesViewModel() : base()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            UpdateButtonContent = "Update GradientProperties for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //GradientPropertiesViewModel = new GradientPropertiesWrapper(new Domain.GradientPropertiesViewModel());

            Log.CONSTRUCTOR("Exit", Common.LOG_APPNAME, startTicks);
        }

        public GradientPropertiesWrapper GradientProperties { get; set; }

        public override void OnUpdateSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!
            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateControlRow");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;


            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_GradientProperties_Section(shape, GradientProperties.Model);
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
                GradientProperties = new GradientPropertiesWrapper(Visio_Shape.Get_GradientProperties(shape));
                OnPropertyChanged("GradientProperties");
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }
    }
}

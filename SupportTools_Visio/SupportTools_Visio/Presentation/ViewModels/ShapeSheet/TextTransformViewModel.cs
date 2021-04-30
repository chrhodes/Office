using System;

using Prism.Commands;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class TextTransformViewModel : ShapeSheetSectionBase
    {
        public TextTransformViewModel() : base()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            UpdateButtonContent = "Update TextTransform for selected shapes";

            // TODO(crhodes)
            // Decide if we want defaults
            //TextTransformViewModel = new TextTransformWrapper(new Domain.TextTransformViewModel());

            Log.CONSTRUCTOR("Exit", Common.LOG_APPNAME, startTicks);
        }

        public TextTransformWrapper TextTransform { get; set; }

        public override void OnUpdateSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!
            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateTextTransform");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                // Just need to pass in the model.
                Visio_Shape.Set_TextTransform_Section(shape, TextTransform.Model);
            }

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            // TODO(crhodes)
            // Decide if should validate TextTransform secitons exists.

            foreach (Visio.Shape shape in selection)
            {
                TextTransform = new TextTransformWrapper(Visio_Shape.Get_TextTransform(shape));
                OnPropertyChanged("TextTransform");
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }
    }
}

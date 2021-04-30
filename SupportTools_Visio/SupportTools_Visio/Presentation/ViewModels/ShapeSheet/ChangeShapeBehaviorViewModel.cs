using System;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class ChangeShapeBehaviorViewModel : ShapeSheetSectionBase
    {
        public ChangeShapeBehaviorViewModel() : base()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            UpdateButtonContent = "Update ChangeShapeBehavior for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //ChangeShapeBehaviorViewModel = new ChangeShapeBehaviorWrapper(new Domain.ChangeShapeBehaviorViewModel());

            Log.CONSTRUCTOR("Exit", Common.LOG_APPNAME, startTicks);
        }

        public ChangeShapeBehaviorWrapper ChangeShapeBehavior { get; set; }

        public override void OnUpdateSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!
            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateControlRow");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_ChangeShapeBehavior_Section(shape, ChangeShapeBehavior.Model);
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
                ChangeShapeBehavior = new ChangeShapeBehaviorWrapper(Visio_Shape.Get_ChangeShapeBehavior(shape));
                OnPropertyChanged("ChangeShapeBehavior");
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }
    }
}

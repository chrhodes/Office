using System;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class UserDefinedCellRowViewModel : ShapeSheetSectionBase
    {
        //public System.Collections.ObjectModel.ObservableCollection<Domain.ControlsRow> ControlRows { get; set; }

        public UserDefinedCellRowWrapper UserDefinedCellRow { get; set; }


        public UserDefinedCellRowViewModel() : base()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            // TODO(crhodes)
            // Decide if we want defaults
            //UserDefinedCellRowViewModel = new UserDefinedCellRowWrapper(new Domain.UserDefinedCellRowViewModel());

            Log.CONSTRUCTOR("Exit", Common.LOG_APPNAME, startTicks);
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!
            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateControlRow");

            // Just need to pass in the model.

            //Visio.Application app = Globals.ThisAddIn.Application;

            //Visio.Selection selection = app.ActiveWindow.Selection;

            //// Verify only one shape, for now just grab first.

            //foreach (Visio.Shape shape in selection)
            //{
            //    //Visio_Shape.Set_UserDefinedCellRowViewModel_Section(shape, UserDefinedCellRowViewModel.Model);
            //}

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            // Verify only one shape, for now just grab first.

            foreach (Visio.Shape shape in selection)
            {
                UserDefinedCellRow = new UserDefinedCellRowWrapper(Visio_Shape.Get_UserDefinedCellRow(shape));
                OnPropertyChanged("UserDefinedCellRow");
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }

    }
}

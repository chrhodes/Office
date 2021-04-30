using System;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Domain;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class ActionsViewModel : ShapeSheetSectionBase
    { 


        public ActionsViewModel()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            UpdateButtonContent = "Update Actions for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //XXX = new XXXWrapper(new Domain.XXX());

            Log.CONSTRUCTOR("Exit", Common.LOG_APPNAME, startTicks);
        }

        public System.Collections.ObjectModel.ObservableCollection<ActionRowWrapper> Actions { get; set; }

        ActionRowWrapper _selectedItem;
        public ActionRowWrapper SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!
            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateActions");

            // Just need to pass in the model.

            //Visio.Application app = Globals.ThisAddIn.Application;

            //Visio.Selection selection = app.ActiveWindow.Selection;

            //// Verify only one shape, for now just grab first.

            //foreach (Visio.Shape shape in selection)
            //{
            //    //Visio_Shape.Set_XXX_Section(shape, XXX.Model);
            //}

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            Actions = new System.Collections.ObjectModel.ObservableCollection<ActionRowWrapper>();

            foreach (Visio.Shape shape in selection)
            {
                foreach (ActionRow row in Visio_Shape.Get_ActionsRows(shape))
                {
                    Actions.Add(new ActionRowWrapper(row));
                }
            }

            OnPropertyChanged("Actions");

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }
    }
}

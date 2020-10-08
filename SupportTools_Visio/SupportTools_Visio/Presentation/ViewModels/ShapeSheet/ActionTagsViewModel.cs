using System;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Domain;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class ActionTagsViewModel : ShapeSheetSectionBase //, IActionTagRowViewModel
    {
        public System.Collections.ObjectModel.ObservableCollection<ActionTagRowWrapper> ActionTags { get; set; }

        ActionTagRowWrapper _selectedItem;
        public ActionTagRowWrapper SelectedItem
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

        public ActionTagsViewModel()
        {
            UpdateButtonContent = "Update ActionTags for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //ActionTagRowViewModel = new ActionTagRowWrapper(new Domain.ActionTagRowViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateActionTags");

            // Just need to pass in the model.

            //Visio.Application app = Globals.ThisAddIn.Application;

            //Visio.Selection selection = app.ActiveWindow.Selection;

            //// Verify only one shape, for now just grab first.

            //foreach (Visio.Shape shape in selection)
            //{
            //    //Visio_Shape.Set_ActionTagRowViewModel_Section(shape, ActionTagRowViewModel.Model);
            //}

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        public override Boolean OnUpdateSettingsCanExecute()
        {
            // TODO(crhodes)
            // Validate we have new settings

            return true;
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            ActionTags = new System.Collections.ObjectModel.ObservableCollection<ActionTagRowWrapper>();

            foreach (Visio.Shape shape in selection)
            {
                foreach (ActionTagRow row in Visio_Shape.Get_ActionTagRows(shape))
                {
                    ActionTags.Add(new ActionTagRowWrapper(row));
                }
            }

            OnPropertyChanged("ActionTags");
        }

        public override bool OnLoadCurrentSettingsCanExecute()
        {
            // TODO(crhodes)
            // Anything else we need to do?

            //return true;
            return base.OnLoadCurrentSettingsCanExecute();
        }
    }
}

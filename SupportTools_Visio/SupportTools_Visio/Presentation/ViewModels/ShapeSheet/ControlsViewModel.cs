using System;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Domain;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class ControlsViewModel : ShapeSheetSectionBase //, IControlsRowViewModelViewModel
    {
        public System.Collections.ObjectModel.ObservableCollection<ControlsRowWrapper> Controls { get; set; }

        ControlsRowWrapper _selectedItem;
        public ControlsRowWrapper SelectedItem
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

        public ControlsViewModel()
        {
            UpdateButtonContent = "Update Controls for selected shapes";

            // TODO(crhodes)
            // Decide if we want defaults
            //ControlsRowViewModel = new ControlsRowWrapper(new Domain.ControlsRowViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateControlRow");

            // Just need to pass in the model.

            //Visio.Application app = Globals.ThisAddIn.Application;

            //Visio.Selection selection = app.ActiveWindow.Selection;

            //// Verify only one shape, for now just grab first.

            //foreach (Visio.Shape shape in selection)
            //{
            //    //Visio_Shape.Set_ControlsRowViewModel_Section(shape, ControlsRowViewModel.Model);
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

            Controls = new System.Collections.ObjectModel.ObservableCollection<ControlsRowWrapper>();

            foreach (Visio.Shape shape in selection)
            {
                foreach (ControlsRow row in Visio_Shape.Get_ControlsRows(shape))
                {
                    Controls.Add(new ControlsRowWrapper(row));
                }
            }

            OnPropertyChanged("Controls");
        }

        //public override bool OnLoadCurrentSettingsCanExecute()
        //{
        //    // TODO(crhodes)
        //    // Check if shape selected

        //    return true;
        //}
    }
}

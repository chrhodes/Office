using System;
using DevExpress.XtraRichEdit.Model;
using Prism.Commands;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Domain;
using SupportTools_Visio.Presentation.ModelWrappers;
using VNC;
using VNC.Core.Mvvm;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class ConnectionPointsViewModel : ShapeSheetSectionBase //, IConnectionPointRowViewModelViewModel
    {
        public System.Collections.ObjectModel.ObservableCollection<ConnectionPointRowWrapper> ConnectionPoints { get; set; }


        ConnectionPointRowWrapper _selectedItem;
        public ConnectionPointRowWrapper SelectedItem
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

        public ConnectionPointsViewModel()
        {
            UpdateButtonContent = "Update ConnectionPoints for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //ConnectionPointRowViewModel = new ConnectionPointRowWrapper(new Domain.ConnectionPointRowViewModel());
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
            //    //Visio_Shape.Set_ConnectionPointRowViewModel_Section(shape, ConnectionPointRowViewModel.Model);
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

            ConnectionPoints = new System.Collections.ObjectModel.ObservableCollection<ConnectionPointRowWrapper>();

            foreach (Visio.Shape shape in selection)
            {
                foreach (ConnectionPointRow row in Visio_Shape.Get_ConnectionPointRows(shape))
                {
                    ConnectionPoints.Add(new ConnectionPointRowWrapper(row));
                }
            }

            OnPropertyChanged("ConnectionPoints");
        }

        //public override bool OnLoadCurrentSettingsCanExecute()
        //{
        //    // TODO(crhodes)
        //    // Check if shape selected

        //    return true;
        //}
    }
}

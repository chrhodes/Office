using System;

using Prism.Commands;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Domain;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class ScratchViewModel : ShapeSheetSectionBase
    {
        public ScratchViewModel()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            OnLoadCurrentSettingsExecute();
            // TODO(crhodes)
            // Decide if we want defaults
            //ScratchRowViewModel = new ScratchRowWrapper(new Domain.ScratchRowViewModel());

            Log.CONSTRUCTOR("Exit", Common.LOG_APPNAME, startTicks);
        }

        public System.Collections.ObjectModel.ObservableCollection<ScratchRowWrapper> ScratchRows { get; set; }

        ScratchRowWrapper _selectedItem;
        public ScratchRowWrapper SelectedItem
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
            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateControlRow");

            // Just need to pass in the model.

            //Visio.Application app = Globals.ThisAddIn.Application;

            //Visio.Selection selection = app.ActiveWindow.Selection;

            //// Verify only one shape, for now just grab first.

            //foreach (Visio.Shape shape in selection)
            //{
            //    //Visio_Shape.Set_ScratchRowViewModel_Section(shape, ScratchRowViewModel.Model);
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

            ScratchRows = new System.Collections.ObjectModel.ObservableCollection<ScratchRowWrapper>();

            foreach (Visio.Shape shape in selection)
            {
                foreach (ScratchRow row in Visio_Shape.Get_ScratchRows(shape))
                {
                    ScratchRows.Add(new ScratchRowWrapper(row));
                }
            }

            OnPropertyChanged("ScratchRows");

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }
    }
}

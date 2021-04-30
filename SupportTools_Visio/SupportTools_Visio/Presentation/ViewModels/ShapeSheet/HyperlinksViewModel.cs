using System;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Domain;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class HyperlinksViewModel : ShapeSheetSectionBase
    {
        public HyperlinksViewModel() : base()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            UpdateButtonContent = "Update Hyperlinks for selected shapes";
            OnLoadCurrentSettingsCanExecute();

            // TODO(crhodes)
            // Decide if we want defaults
            //HyperlinkRowViewModel = new HyperlinkRowWrapper(new Domain.HyperlinkRowViewModel());

            Log.CONSTRUCTOR("Exit", Common.LOG_APPNAME, startTicks);
        }

        public System.Collections.ObjectModel.ObservableCollection<HyperlinkRowWrapper> Hyperlinks { get; set; }

        HyperlinkRowWrapper _selectedItem;
        public HyperlinkRowWrapper SelectedItem
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
            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateHyperlinks");

            // Just need to pass in the model.

            //Visio.Application app = Globals.ThisAddIn.Application;

            //Visio.Selection selection = app.ActiveWindow.Selection;

            //// Verify only one shape, for now just grab first.

            //foreach (Visio.Shape shape in selection)
            //{
            //    //Visio_Shape.Set_HyperlinkRowViewModel_Section(shape, HyperlinkRowViewModel.Model);
            //}

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            Hyperlinks = new System.Collections.ObjectModel.ObservableCollection<HyperlinkRowWrapper>();

            foreach (Visio.Shape shape in selection)
            {
                foreach (HyperlinkRow row in Visio_Shape.Get_HyperlinksRows(shape))
                {
                    Hyperlinks.Add(new HyperlinkRowWrapper(row));
                }
            }

            OnPropertyChanged("Hyperlinks");

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }
    }
}

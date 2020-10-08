using System;

using Prism.Commands;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;
using VNC;
using VNC.Core.Mvvm;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class LineGradientStopRowViewModel : ViewModelBase //, ILineGradientStopRowViewModelViewModel
    {
        public System.Collections.ObjectModel.ObservableCollection<Domain.ControlsRow> ControlRows { get; set; }


        public DelegateCommand UpdateSettings { get; private set; }
        public DelegateCommand LoadCurrentSettings { get; private set; }

        public LineGradientStopRowWrapper LineGradientStopRow { get; set; }


        public LineGradientStopRowViewModel()
        {
            UpdateSettings = new DelegateCommand(OnUpdateSettingsExecute, OnUpdateSettingsCanExecute);
            LoadCurrentSettings = new DelegateCommand(OnLoadCurrentSettingsExecute, OnLoadCurrentSettingsCanExecute);

            // TODO(crhodes)
            // Decide if we want defaults
            //LineGradientStopRowViewModel = new LineGradientStopRowWrapper(new Domain.LineGradientStopRowViewModel());
        }

        public void OnUpdateSettingsExecute()
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
            //    //Visio_Shape.Set_LineGradientStopRowViewModel_Section(shape, LineGradientStopRowViewModel.Model);
            //}

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        public Boolean OnUpdateSettingsCanExecute()
        {
            // TODO(crhodes)
            // Validate we have new settings

            return true;
        }

        void OnLoadCurrentSettingsExecute()
        {
            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            // Verify only one shape, for now just grab first.

            foreach (Visio.Shape shape in selection)
            {
                LineGradientStopRow = new LineGradientStopRowWrapper(Visio_Shape.Get_LineGradientStopRow(shape));
                OnPropertyChanged("LineGradientStopRow");
            }
        }

        bool OnLoadCurrentSettingsCanExecute()
        {
            // TODO(crhodes)
            // Check if shape selected

            return true;
        }
    }
}

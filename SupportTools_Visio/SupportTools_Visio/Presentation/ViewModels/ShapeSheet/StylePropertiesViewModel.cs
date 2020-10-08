using System;

using Prism.Commands;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;
using VNC;
using VNC.Core.Mvvm;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class StylePropertiesViewModel : ShapeSheetSectionBase //, IStylePropertiesViewModelViewModel
    {
        public StylePropertiesWrapper StyleProperties { get; set; }

        public StylePropertiesViewModel() : base()
        {
            //UpdateSettings = new DelegateCommand(OnUpdateSettingsExecute, OnUpdateSettingsCanExecute);
            //LoadCurrentSettings = new DelegateCommand(OnLoadCurrentSettingsExecute, OnLoadCurrentSettingsCanExecute);

            UpdateButtonContent = "Update StyleProperties for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //StylePropertiesViewModel = new StylePropertiesWrapper(new Domain.StylePropertiesViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateStyleProperties");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                //Visio_Shape.Set_StylePropertiesViewModel_Section(shape, StylePropertiesViewModel.Model);
            }

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                StyleProperties = new StylePropertiesWrapper(Visio_Shape.Get_StyleProperties(shape));
                OnPropertyChanged("StyleProperties");
            }
        }
    }
}

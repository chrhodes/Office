using System;

using Prism.Commands;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;
using VNC;
using VNC.Core.Mvvm;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class PrintPropertiesViewModel : ShapeSheetSectionBase //, IPrintPropertiesViewModelViewModel
    {
        public PrintPropertiesWrapper PrintProperties { get; set; }


        public PrintPropertiesViewModel() : base()
        {
            UpdateButtonContent = "Update PrintProperties for selected shapes";

            // TODO(crhodes)
            // Decide if we want defaults
            //PrintPropertiesViewModel = new PrintPropertiesWrapper(new Domain.PrintPropertiesViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateControlRow");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_PrintProperties_Section(shape, PrintProperties.Model);
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
                PrintProperties = new PrintPropertiesWrapper(Visio_Shape.Get_PrintProperties(shape));
                OnPropertyChanged("PrintProperties");
            }
        }
    }
}

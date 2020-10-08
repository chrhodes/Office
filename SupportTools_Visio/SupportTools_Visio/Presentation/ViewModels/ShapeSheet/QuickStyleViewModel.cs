﻿
using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class QuickStyleViewModel : ShapeSheetSectionBase //, IQuickStyleViewModelViewModel
    {
        public QuickStyleWrapper QuickStyle { get; set; }

        public QuickStyleViewModel() : base()
        {
            UpdateButtonContent = "Update QuickStyle for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //QuickStyleViewModel = new QuickStyleWrapper(new Domain.QuickStyleViewModel());
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
                Visio_Shape.Set_QuickStyle_Section(shape, QuickStyle.Model);
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
                QuickStyle = new QuickStyleWrapper(Visio_Shape.Get_QuickStyle(shape));
                OnPropertyChanged("QuickStyle");
            }
        }
    }
}

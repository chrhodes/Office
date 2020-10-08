using System;

using Prism.Commands;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;
using VNC;
using VNC.Core.Mvvm;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class ChangeShapeBehaviorViewModel : ShapeSheetSectionBase //, IChangeShapeBehaviorViewModelViewModel
    {
        public ChangeShapeBehaviorWrapper ChangeShapeBehavior { get; set; }


        public ChangeShapeBehaviorViewModel() : base()
        {
            UpdateButtonContent = "Update ChangeShapeBehavior for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //ChangeShapeBehaviorViewModel = new ChangeShapeBehaviorWrapper(new Domain.ChangeShapeBehaviorViewModel());
        }

        public void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateControlRow");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_ChangeShapeBehavior_Section(shape, ChangeShapeBehavior.Model);
            }

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        void OnLoadCurrentSettingsExecute()
        {
            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                ChangeShapeBehavior = new ChangeShapeBehaviorWrapper(Visio_Shape.Get_ChangeShapeBehavior(shape));
                OnPropertyChanged("ChangeShapeBehavior");
            }
        }
    }
}

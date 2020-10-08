
using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class TextBlockFormatViewModel : ShapeSheetSectionBase //, ITextBlockFormatViewModelViewModel
    {
        public TextBlockFormatWrapper TextBlockFormat { get; set; }


        public TextBlockFormatViewModel() : base()
        {
            UpdateButtonContent = "Update TextBlock for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //TextBlockFormatViewModel = new TextBlockFormatWrapper(new Domain.TextBlockFormatViewModel());
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
                Visio_Shape.Set_TextBlockFormat_Section(shape, TextBlockFormat.Model);
            }

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            // Verify only one shape, for now just grab first.

            foreach (Visio.Shape shape in selection)
            {
                TextBlockFormat = new TextBlockFormatWrapper(Visio_Shape.Get_TextBlockFormat(shape));
                OnPropertyChanged("TextBlockFormat");
            }
        }
    }
}


using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class DocumentPropertiesViewModel : ShapeSheetSectionBase //, IDocumentPropertiesViewModelViewModel
    {
        public DocumentPropertiesWrapper DocumentProperties { get; set; }


        public DocumentPropertiesViewModel() : base()
        {
            UpdateButtonContent = "Update DocumentProperties for selected shapes";
            // TODO(crhodes)
            // Decide if we want defaults
            //DocumentPropertiesViewModel = new DocumentPropertiesWrapper(new Domain.DocumentPropertiesViewModel());
        }

        public void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateDocumentProperties");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_DocumentProperties_Section(shape, DocumentProperties.Model);
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
                DocumentProperties = new DocumentPropertiesWrapper(Visio_Shape.Get_DocumentProperties(shape));
                OnPropertyChanged("DocumentProperties");
            }
        }
    }
}

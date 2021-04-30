
using System;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class DocumentShapeDataViewModel : DocumentShapeSheetSectionBase
    {
        public DocumentShapeDataViewModel() : base()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            UpdateButtonContent = "Reload DocumentProperties";

            // TODO(crhodes)
            // Decide if we want defaults
            //DocumentPropertiesViewModel = new DocumentPropertiesWrapper(new Domain.DocumentPropertiesViewModel());
            // For now, just display current
            OnLoadCurrentSettingsExecute();

            Log.CONSTRUCTOR("Exit", Common.LOG_APPNAME, startTicks);
        }

        public DocumentPropertiesWrapper DocumentProperties { get; set; }

        public override void OnUpdateSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!
            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateDocumentProperties");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Shape shape = ((Visio.Document)app.ActiveDocument).DocumentSheet;

            Visio_Shape.Set_DocumentProperties_Section(shape, DocumentProperties.Model);

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Shape shape = ((Visio.Document)app.ActiveDocument).DocumentSheet;

            DocumentProperties = new DocumentPropertiesWrapper(Visio_Shape.Get_DocumentProperties(shape));
            OnPropertyChanged("DocumentProperties");

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }
    }
}

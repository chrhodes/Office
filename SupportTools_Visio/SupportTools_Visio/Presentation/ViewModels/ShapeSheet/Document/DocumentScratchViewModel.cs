
using System;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Domain;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class DocumentScratchViewModel : DocumentShapeSheetSectionBase
    {
        public DocumentScratchViewModel() : base()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            // TODO(crhodes)
            // Decide if we want defaults
            //DocumentPropertiesViewModel = new DocumentPropertiesWrapper(new Domain.DocumentPropertiesViewModel());
            // For now, just display current
            OnLoadCurrentSettingsExecute();

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
            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateDocumentScratch");

            //Visio.Application app = Globals.ThisAddIn.Application;

            //Visio.Shape shape = ((Visio.Document)app.ActiveDocument).DocumentSheet;

            //Visio_Shape.Set_DocumentProperties_Section(shape, DocumentProperties.Model);

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Shape shape = ((Visio.Document)app.ActiveDocument).DocumentSheet;

            ScratchRows = new System.Collections.ObjectModel.ObservableCollection<ScratchRowWrapper>();

            foreach (ScratchRow row in Visio_Shape.Get_ScratchRows(shape))
            {
                ScratchRows.Add(new ScratchRowWrapper(row));
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }
    }
}

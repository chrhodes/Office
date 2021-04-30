
using System;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Domain;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class PageHyperLinksViewModel : PageShapeSheetSectionBase
    {
        public PageHyperLinksViewModel() : base()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            //UpdateButtonContent = "Reload Page Hyperlinks";

            // TODO(crhodes)
            // Decide if we want defaults
            //DocumentPropertiesViewModel = new DocumentPropertiesWrapper(new Domain.DocumentPropertiesViewModel());
            // For now, just display current
            OnLoadCurrentSettingsExecute();

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
            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdatePageHyperlinks");

            //Visio.Application app = Globals.ThisAddIn.Application;

            //Visio.Shape shape = ((Visio.Document)app.ActiveDocument).DocumentSheet;

            //Visio_Shape.Set_DocumentProperties_Section(shape, DocumentHyperlinks.Model);

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Shape shape = ((Visio.Page)app.ActivePage).PageSheet;

            Hyperlinks = new System.Collections.ObjectModel.ObservableCollection<HyperlinkRowWrapper>();

            foreach (HyperlinkRow row in Visio_Shape.Get_HyperlinksRows(shape))
            {
                Hyperlinks.Add(new HyperlinkRowWrapper(row));
            }

            OnPropertyChanged("Hyperlinks");

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }
    }
}

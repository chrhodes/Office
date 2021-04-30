using System;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Domain;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class ShapeDataViewModel : ShapeSheetSectionBase
    {
        public ShapeDataViewModel() : base()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            // TODO(crhodes)
            // Decide if we want defaults
            //ShapeDataRowViewModel = new ShapeDataRowWrapper(new Domain.ShapeDataRowViewModel());

            Log.CONSTRUCTOR("Exit", Common.LOG_APPNAME, startTicks);
        }

        //public System.Collections.ObjectModel.ObservableCollection<Domain.ShapeDataRow> ShapeDataRows { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<ShapeDataRowWrapper> ShapeDataRowsW { get; set; }

        ShapeDataRowWrapper _currentShapeDataRowW;
        public ShapeDataRowWrapper CurrentShapeDataRowW
        {
            get
            {
                return _currentShapeDataRowW;
            }
            set
            {
                _currentShapeDataRowW = value;
                OnPropertyChanged();
            }
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!
            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateShapeData");

            // Just need to pass in the model.

            //Visio.Application app = Globals.ThisAddIn.Application;

            //Visio.Selection selection = app.ActiveWindow.Selection;

            // TODO(crhodes)
            // Figure out what do to with Shapes that have multiple rows in a section.
            //foreach (Visio.Shape shape in selection)
            //{
            //    //Visio_Shape.Set_ShapeDataRowViewModel_Section(shape, ShapeDataRowViewModel.Model);
            //}

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                //ShapeDataRows = Visio_Shape.Get_ShapeDataRows(shape);
                //ShapeDataRow = new ShapeDataRowWrapper(Visio_Shape.Get_ShapeDataRows(shape));

                ShapeDataRowsW = new System.Collections.ObjectModel.ObservableCollection<ShapeDataRowWrapper>();

                foreach (ShapeDataRow row in Visio_Shape.Get_ShapeDataRows(shape))
                {
                    ShapeDataRowsW.Add(new ShapeDataRowWrapper(row));
                }

                //OnPropertyChanged("ShapeDataRows");
                OnPropertyChanged("ShapeDataRowsW");
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }
    }
}

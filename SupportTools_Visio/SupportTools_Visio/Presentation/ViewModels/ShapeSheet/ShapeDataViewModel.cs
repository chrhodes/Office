using System;
using System.CodeDom;
using Prism.Commands;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Domain;
using SupportTools_Visio.Presentation.ModelWrappers;
using VNC;
using VNC.Core.Mvvm;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class ShapeDataViewModel : ShapeSheetSectionBase //, IShapeDataRowViewModelViewModel
    {
        //public System.Collections.ObjectModel.ObservableCollection<Domain.ShapeDataRow> ShapeDataRows { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<ShapeDataRowWrapper> ShapeDataRowsW { get; set; }

        ////ShapeDataRowWrapper _currentShapeDataRow;
        //Domain.ShapeDataRow _currentShapeDataRow;
        //public Domain.ShapeDataRow CurrentShapeDataRow
        ////{ 
        ////    get; 
        ////    set;
        ////}
        //{
        //    get
        //    {
        //        return _currentShapeDataRow;
        //    }
        //    set
        //    {
        //        _currentShapeDataRow = value;
        //        OnPropertyChanged();
        //    }
        //}

        ShapeDataRowWrapper _currentShapeDataRowW;
        public ShapeDataRowWrapper CurrentShapeDataRowW
        //{ 
        //    get; 
        //    set;
        //}
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

        public ShapeDataViewModel()
        {
            // TODO(crhodes)
            // Decide if we want defaults
            //ShapeDataRowViewModel = new ShapeDataRowWrapper(new Domain.ShapeDataRowViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
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
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        public void OnSelectedItemChanged()
        {

        }

        public override Boolean OnUpdateSettingsCanExecute()
        {
            // TODO(crhodes)
            // Validate we have new settings

            return true;
        }

        public override void OnLoadCurrentSettingsExecute()
        {
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
        }

        public override bool OnLoadCurrentSettingsCanExecute()
        {
            // TODO(crhodes)
            // Check if shape selected

            return true;
        }
    }
}

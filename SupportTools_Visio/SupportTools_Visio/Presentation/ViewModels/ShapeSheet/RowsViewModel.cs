using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNC;
using VNC.Core.Mvvm;
using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class RowsViewModel<TRow,TRowWrapper> : ShapeSheetSectionBase
        where TRowWrapper : ModelWrapper<TRow>, new()
    {
        public System.Collections.ObjectModel.ObservableCollection<TRowWrapper> Rows { get; set; }

        public delegate System.Collections.ObjectModel.ObservableCollection<TRow> GetRows(Visio.Shape shape);

        GetRows getRowsCommand;
        
        TRowWrapper _selectedItem;
        public TRowWrapper SelectedItem
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

        public RowsViewModel(string updateButtonMessage, GetRows getRowsMethod)
        {
            UpdateButtonContent = updateButtonMessage;
            getRowsCommand = getRowsMethod;
            // TODO(crhodes)
            // Decide if we want defaults
            //XXX = new XXXWrapper(new Domain.XXX());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateRows");

            // Just need to pass in the model.

            //Visio.Application app = Globals.ThisAddIn.Application;

            //Visio.Selection selection = app.ActiveWindow.Selection;

            //// Verify only one shape, for now just grab first.

            //foreach (Visio.Shape shape in selection)
            //{
            //    //Visio_Shape.Set_XXX_Section(shape, XXX.Model);
            //}

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            Rows = new System.Collections.ObjectModel.ObservableCollection<TRowWrapper>();

            foreach (Visio.Shape shape in selection)
            {
                foreach (TRow row in getRowsCommand(shape))
                {
                    //Rows.Add(new TRowWrapper(row));
                    Rows.Add((TRowWrapper)Activator.CreateInstance(typeof(TRowWrapper), row));
                }
            }

            OnPropertyChanged("Rows");
        }

        public override bool OnLoadCurrentSettingsCanExecute()
        {
            // TODO(crhodes)
            // Anything else we need to do?

            //return true;
            return base.OnLoadCurrentSettingsCanExecute();
        }
    }
}

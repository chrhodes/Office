using System;
using System.Windows;

using Microsoft.Office.Interop.Visio;

using SupportTools_Visio.Domain;

using VNC;
using VNC.Core.Mvvm;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class RowsViewModel<TRow, TRowWrapper> : ShapeSheetSectionBase
        where TRowWrapper : ModelWrapper<TRow>, new()
    {
        public RowsViewModel(string updateButtonMessage, GetRows getRowsMethod, ShapeType shapeType) 
            : base()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            UpdateButtonContent = updateButtonMessage;
            _getRowsCommand = getRowsMethod;
            _shapeType = shapeType;

            OnLoadCurrentSettingsExecute();
            // TODO(crhodes)
            // Decide if we want defaults
            //XXX = new XXXWrapper(new Domain.XXX());

            Log.CONSTRUCTOR("Exit", Common.LOG_APPNAME, startTicks);
        }

        public System.Collections.ObjectModel.ObservableCollection<TRowWrapper> Rows { get; set; }

        public delegate System.Collections.ObjectModel.ObservableCollection<TRow> GetRows(Visio.Shape shape);

        GetRows _getRowsCommand;

        ShapeType _shapeType;
        
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

        public override void OnUpdateSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

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

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }

        public override bool OnUpdateSettingsCanExecute()
        {
            Visio.Application app = Globals.ThisAddIn.Application;

            switch (_shapeType)
            {
                case ShapeType.Document:
                    return app.ActiveDocument != null ? true : false;

                case ShapeType.Page:
                    return app.ActivePage != null ? true : false;

                case ShapeType.Shape:
                    return base.OnLoadCurrentSettingsCanExecute();

                default:
                    MessageBox.Show($"Unexpected _shapeType({_shapeType.GetType()}");
                    break;
            }

            return false;
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            Visio.Application app = Globals.ThisAddIn.Application;

            Rows = new System.Collections.ObjectModel.ObservableCollection<TRowWrapper>();

            switch (_shapeType)
            {
                case ShapeType.Document:
                    GetRowsFromShape(((Visio.Document)app.ActiveDocument).DocumentSheet);
                    break;

                case ShapeType.Page:
                    GetRowsFromShape(((Visio.Page)app.ActivePage).PageSheet);
                    break;

                case ShapeType.Shape:
                    Visio.Selection selection = app.ActiveWindow.Selection;

                    foreach (Visio.Shape shape in selection)
                    {
                        GetRowsFromShape(shape);
                    }
                    break;

                default:
                    MessageBox.Show($"Unexpected _shapeType({_shapeType.GetType()}");
                    break;
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME);
        }

        public override bool OnLoadCurrentSettingsCanExecute()
        {
            Visio.Application app = Globals.ThisAddIn.Application;

            switch (_shapeType)
            {
                case ShapeType.Document:
                    return app.ActiveDocument != null ? true : false;

                case ShapeType.Page:
                    return app.ActivePage != null ? true : false;

                case ShapeType.Shape:
                    return base.OnLoadCurrentSettingsCanExecute();

                default:
                    MessageBox.Show($"Unexpected _shapeType({_shapeType.GetType()}");
                    break;
            }

            return false;
        }

        private void GetRowsFromShape(Shape shape)
        {
            foreach (TRow row in _getRowsCommand(shape))
            {
                //Rows.Add(new TRowWrapper(row));
                Rows.Add((TRowWrapper)Activator.CreateInstance(typeof(TRowWrapper), row));
            }

            OnPropertyChanged("Rows");
        }
    }
}

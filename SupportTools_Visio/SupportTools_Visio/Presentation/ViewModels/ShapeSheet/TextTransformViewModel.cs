using System;

using Prism.Commands;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class TextTransformViewModel : ShapeSheetSectionBase //: ViewModelBase //, ITextTransformViewModelViewModel
    {
        //public System.Collections.ObjectModel.ObservableCollection<Domain.ControlsRow> ControlRows { get; set; }

        //public DelegateCommand UpdateSettings { get; private set; }
        //public DelegateCommand LoadCurrentSettings { get; private set; }

        public TextTransformWrapper TextTransform { get; set; }

        //int _selectedShapeCount;
        //public int SelectedShapeCount
        //{
        //    get { return _selectedShapeCount; }
        //    set
        //    {
        //        if (_selectedShapeCount == value)
        //            return;
        //        _selectedShapeCount = value;
        //        OnPropertyChanged();
        //        //LoadCurrentSettings.RaiseCanExecuteChanged();
        //    }
        //}

        //string _message;
        //public string Message
        //{
        //    get
        //    {
        //        return _message;
        //    }
        //    set
        //    {
        //        _message = value;
        //        OnPropertyChanged();
        //    }
        //}


        public TextTransformViewModel() : base()
        {
            //UpdateSettings = new DelegateCommand(OnUpdateSettingsExecute, OnUpdateSettingsCanExecute);
            //LoadCurrentSettings = new DelegateCommand(OnLoadCurrentSettingsExecute, OnLoadCurrentSettingsCanExecute);

            // Visio will publish Selection Changed event.
            //Common.EventAggregator.GetEvent<SelectionChangedEvent>().Subscribe(OnRefresh);

            UpdateButtonContent = "Update TextTransform for selected shapes";

            // TODO(crhodes)
            // Decide if we want defaults
            //TextTransformViewModel = new TextTransformWrapper(new Domain.TextTransformViewModel());
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateTextTransform");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                // Just need to pass in the model.
                Visio_Shape.Set_TextTransform_Section(shape, TextTransform.Model);
            }

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);
            Log.Trace("Exit", Common.PROJECT_NAME);
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

            // TODO(crhodes)
            // Decide if should validate TextTransform secitons exists.

            foreach (Visio.Shape shape in selection)
            {
                TextTransform = new TextTransformWrapper(Visio_Shape.Get_TextTransform(shape));
                OnPropertyChanged("TextTransform");
            }
        }

        //public override Boolean OnLoadCurrentSettingsCanExecute()
        //{
        //    //return true;
        //    Visio.Application app = Globals.ThisAddIn.Application;

        //    Visio.Selection selection = app.ActiveWindow.Selection;

        //    var containingMaster = selection.ContainingMaster;
        //    var containingMasterID = selection.ContainingMasterID;
        //    var containingPage = selection.ContainingPage;
        //    var containingPageID = selection.ContainingPageID;
        //    var containingShape = selection.ContainingShape;
        //    var primaryItem = selection.PrimaryItem;

        //    //var itemStatus = selection.ItemStatus[0];

        //    var count = selection.Count;
        //    SelectedShapeCount = count;

        //    //var whatAreYou = selection[0];

        //    //if (Visio_Shape.HasTextTransformSection(shape))
        //    //{
        //    //    return true;
        //    //}
        //    //else
        //    //{
        //    //    return false;
        //    //}
        //    //// TODO(crhodes)
        //    //// Check if shape selected


        //    if (count > 0)
        //    {
        //        if (count != 1)
        //        {
        //            LoadButtonContent = "Must select single shape to load settings";

        //            return false;
        //        }

        //        LoadButtonContent = "Load from Current Shape";

        //        return true;
        //    }
        //    else
        //    {
        //        LoadButtonContent = "No shape selected";

        //        return false;
        //    }
        //}
    }
}

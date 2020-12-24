using Prism.Commands;

using SupportTools_Excel.Infrastructure.Presentation.ViewModels;
using SupportTools_Excel.Presentation.ModelWrappers;
using SupportTools_Excel.Presentation.Views;

using VNC;
using VNC.Core.Mvvm;

namespace SupportTools_Excel.Presentation.ViewModels
{
    public class CatViewModel : ViewModelBase, ICatViewModel
    {
        #region Constructors and Load

        // View First

        public CatViewModel()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);

            DoSomethingCommand = new DelegateCommand(OnDoSomethingExecute, OnDoSomethingCanExecute);
            DoSomethingContent = "Update Actions for selected shapes";
            DoSomethingToolTip = "ToolTip for DoSomething Button";
            // TODO(crhodes)
            // Decide if we want defaults
            //Cat = new CatWrapper(new Domain.Cat());

            InitializeRows();

            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        // ViewModel First
        // Calling base(view) wires this ViewModel into the View

        public CatViewModel(Cat view) : base(view)
        {
            Log.Trace("Enter", Common.PROJECT_NAME);

            DoSomethingCommand = new DelegateCommand(OnDoSomethingExecute, OnDoSomethingCanExecute);
            DoSomethingContent = "Update Actions for selected shapes";
            DoSomethingToolTip = "ToolTip for DoSomething Button";

            InitializeRows();

            // Save the View (in ViewModelBase)
            View = view;

            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        string _message = "Click Button to do something";
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message == value) return;

                _message = value;
                OnPropertyChanged();
            }
        }

        // TODO(crhodes)
        // This is for a Grid or List

        public System.Collections.ObjectModel.ObservableCollection<CatWrapper> Rows { get; set; }

        // and the SelectedItem in the Grid or List

        CatWrapper _selectedItem;
        public CatWrapper SelectedItem
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

        // Don't forget to uncomment InitializeRows in Constructors

        void InitializeRows()
        {
            Rows = new System.Collections.ObjectModel.ObservableCollection<CatWrapper>();
            Rows.Add(new CatWrapper(new Domain.Cat() { StringProperty = "Red", IntProperty = 1 }));
            Rows.Add(new CatWrapper(new Domain.Cat() { StringProperty = "Green", IntProperty = 2 }));
            Rows.Add(new CatWrapper(new Domain.Cat() { StringProperty = "Blue", IntProperty = 3 }));

            OnPropertyChanged("Rows");
        }

        #endregion

        #region Commands

        #region DoSomething Command

        public DelegateCommand DoSomethingCommand { get; set; }
        public string DoSomethingContent { get; set; }
        public string DoSomethingToolTip { get; set; }

        public void OnDoSomethingExecute()
        {
            // TODO(crhodes)
            // Do something amazing.

            Message = "Cool, you did something!";
        }

        public bool OnDoSomethingCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            return true;
        }

        #endregion

        #endregion Commands

    }
}

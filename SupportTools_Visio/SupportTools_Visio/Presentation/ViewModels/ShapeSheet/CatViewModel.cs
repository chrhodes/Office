using Prism.Commands;

using VNC;
using VNC.Core.Mvvm;

using SupportTools_Visio.Presentation.ModelWrappers;
using SupportTools_Visio.Presentation.Views;
using SupportTools_Visio.Presentation.Presentation.ModelWrappers;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class CatViewModel : ViewModelBase, ICatViewModel
    {
        #region Constructors and Load

        // View First
        // View creates new ViewModel in code or Xaml
        // or ViewModel passed into View constructor

        public CatViewModel()
        {
            long startTicks = Log.CONSTRUCTOR("Enter", Common.PROJECT_NAME);

            // TODO(crhodes)
            // Decide if we want defaults
            //Cat = new CatWrapper(new Domain.Cat());

            InitializeViewModel();

            Log.CONSTRUCTOR("Exit", Common.PROJECT_NAME, startTicks);
        }

        // ViewModel First
        // Calling base(view) wires this ViewModel into the View

        public CatViewModel(Cat view) : base(view)
        {
            long startTicks = Log.Trace("Enter", Common.PROJECT_NAME);

            InitializeViewModel();

            Log.Trace("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void InitializeViewModel()
        {
            // TODO(crhodes)
            // Initialize any controls and/or properties that need to be

            DoSomethingCommand = new DelegateCommand(OnDoSomethingExecute, OnDoSomethingCanExecute);
            DoSomethingContent = "Update Actions for selected shapes";
            DoSomethingToolTip = "ToolTip for DoSomething Button";

            Message_DoubleClick_Command = new DelegateCommand(Message_DoubleClick);

            InitializeRows();
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
                _message = value;
                OnPropertyChanged();
            }
        }

        // TODO(crhodes)
        // This is for a Grid or List

        public System.Collections.ObjectModel.ObservableCollection<string> SelectedColors { get; set; }

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

        //Don't forget to uncomment InitializeRows in InitializeViewModel()

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

        #region Control Commands (Not Buttons)

        public DelegateCommand Message_DoubleClick_Command { get; set; }

        public void Message_DoubleClick()
        {
            Message = "Message DoubleClicked!";
        }

        #endregion

        #endregion Commands

    }
}

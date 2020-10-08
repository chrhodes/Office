using System.Windows.Controls;

using VNC;
using VNC.Core.Mvvm;
using SupportTools_Excel.ActiveDirectoryExplorer.Presentation.ViewModels;

namespace SupportTools_Excel.ActiveDirectoryExplorer.Presentation.Views
{
    public partial class ADPicker : UserControl, IView
    {
        #region Constructors and Load

        // View First.  
        // View is passed ViewModel through Injection
        // or can declare ViewModel in Xaml or Code

        // ViewModel First.  
        // ViewModel creates View 
        // and sets DataContext by setting ViewModel property

        public ADPicker()
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            InitializeComponent();

            // If View First with ViewModel in Xaml
            // Expose ViewModel
            // ViewModel = (IADPickerViewModel)DataContext;

            // Can create directly
            // ViewModel = new ADPickerViewModel();

            InitializeView();

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        public ADPicker(IADPickerViewModel viewModel)
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            InitializeComponent();
            ViewModel = viewModel;

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        private void InitializeView()
        {
            // TODO(crhodes)
            // Perform any initialization or configuration of View

            lgMain.IsCollapsed = true;
        }

        #endregion

        #region Properties

        private IViewModel _viewModel;

        public IViewModel ViewModel
        {
            get { return _viewModel; }

            set
            {
                _viewModel = value;
                DataContext = _viewModel;
            }
        }

        #endregion
    }
}

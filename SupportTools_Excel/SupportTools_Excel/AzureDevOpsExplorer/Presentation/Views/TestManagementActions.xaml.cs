using System.Windows.Controls;

using SupportTools_Excel.Infrastructure.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;

namespace SupportTools_Excel.AzureDevOpsExplorer.Presentation.Views
{
    public partial class TestManagementActions : UserControl, IView
    {

        #region Constructors and Load

        // ViewModel First.  ViewModel creates View 
        // and sets DataContext by setting ViewModel property

        public TestManagementActions()
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            InitializeComponent();

            // If View First with ViewModel in Xaml
            // Expose ViewModel
            // ViewModel = (ICatViewModel)DataContext;

            // Can create directly
            // ViewModel = CatViewModel();

            InitializeView();

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        // View First.  View is passed ViewModel through Injection
        // or can declare ViewModel in Xaml

        public TestManagementActions(IAZDOTestManagementActionsViewModel viewModel)
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            InitializeComponent();

            ViewModel = viewModel;

            InitializeView();

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        private void InitializeView()
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            // TODO(crhodes)
            // Perform any initialization or configuration of View

            lgMain.IsCollapsed = true;

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
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

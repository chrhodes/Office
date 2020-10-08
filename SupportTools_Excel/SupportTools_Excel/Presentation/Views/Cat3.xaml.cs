using System.Windows.Controls;

using VNC;
using VNC.Core.Mvvm;
using SupportTools_Excel.Presentation.ViewModels;
using SupportTools_Excel.Infrastructure.Presentation.ViewModels;

namespace SupportTools_Excel.Presentation.Views
{
    public partial class Cat3 : UserControl, IView
    {
        #region Constructors and Load

        // View First.  
        // View is passed ViewModel through Injection
        // or can declare ViewModel in Xaml or Code

        // ViewModel First.  
        // ViewModel creates View 
        // and sets DataContext by setting ViewModel property

        //public Cat3()
        //{
        //    long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

        //    InitializeComponent();

        //    // If View First with ViewModel in Xaml
        //    // Expose ViewModel
        //    // ViewModel = (ICat2ViewModel)DataContext;

        //    // Can create directly
        //    // ViewModel = new Cat2ViewModel();

        //    InitializeView();

        //    Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        //}

        public Cat3(ICat3ViewModel viewModel)
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            InitializeComponent();

            ViewModel = viewModel;

            InitializeView();

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        private void InitializeView()
        {
            // TODO(crhodes)
            // Perform any initialization or configuration of View

            //lgMain.IsCollapsed = true;
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

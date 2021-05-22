﻿using System.Windows.Controls;

using SupportTools_Excel.Core.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;

namespace SupportTools_Excel.AzureDevOpsExplorer.Presentation.Views
{
    public partial class Misc : UserControl, IView
    {
        #region Constructors and Load

        // ViewModel First.  ViewModel creates View 
        // and sets DataContext by setting ViewModel property

        public Misc()
        {
            long startTicks = Log.CONSTRUCTOR("Enter", Common.PROJECT_NAME);

            InitializeComponent();

            // If View First with ViewModel in Xaml
            // Expose ViewModel
            // ViewModel = (ICatViewModel)DataContext;

            // Can create directly
            // ViewModel = CatViewModel();

            InitializeView();

            Log.CONSTRUCTOR("Exit", Common.PROJECT_NAME, startTicks);
        }

        // View First.  View is passed ViewModel through Injection
        // or can declare ViewModel in Xaml

        public Misc(IAZDOMiscViewModel viewModel)
        {
            long startTicks = Log.CONSTRUCTOR("Enter", Common.PROJECT_NAME);

            InitializeComponent();
            ViewModel = viewModel;

            InitializeView();

            Log.CONSTRUCTOR("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void InitializeView()
        {
            long startTicks = Log.VIEW("Enter", Common.PROJECT_NAME);

            // TODO(crhodes)
            // Perform any initialization or configuration of View

            lgMain.IsCollapsed = true;

            Log.VIEW("Exit", Common.PROJECT_NAME, startTicks);
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

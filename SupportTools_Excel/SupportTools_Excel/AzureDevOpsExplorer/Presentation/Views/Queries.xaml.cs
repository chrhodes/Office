﻿using System.Windows.Controls;

using SupportTools_Excel.Infrastructure.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;

namespace SupportTools_Excel.AzureDevOpsExplorer.Presentation.Views
{

    public partial class Queries : UserControl, IView
    {
        #region Constructors and Load

        // ViewModel First.  ViewModel creates View 
        // and sets DataContext by setting ViewModel property
        // or View is created in code or Xaml.

        public Queries()
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            InitializeComponent();

            // If View First with ViewModel in Xaml
            // Expose ViewModel
            ViewModel = (IAZDOQueriesViewModel)DataContext;

            // Can create directly
            // ViewModel = CatViewModel();

            InitializeView();

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        // View First.  View is passed ViewModel through Injection
        // or can declare ViewModel in Xaml

        public Queries(IAZDOQueriesViewModel viewModel)
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

            //queryPicker.PopulateControlFromFile(Common.cCONFIG_FILE);
            //queryPicker2.PopulateControlFromFile(Common.cCONFIG_FILE);

            //PopulateControlFromFile();
            lgMain.IsCollapsed = true;

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        //private void PopulateControlFromFile()
        //{
        //    dataProvider.Source = null;
        //    dataProvider.Source = new Uri(Common.cCONFIG_FILE);
        //    dataProvider.Refresh();
        //}

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

        //private void queryPicker_ControlChanged()
        //{
        //    ((QueriesViewModel)ViewModel).SelectedQuery = new WorkItemQueryWrapper(
        //        new WorkItemQuery() { Name = queryPicker.Name, Query = queryPicker.Query });
        //}
    }
}

﻿using System;
using System.Windows;
using System.Windows.Controls;
using SupportTools_Excel.ActiveDirectoryExplorer.Presentation.ViewModels;
using SupportTools_Excel.Infrastructure.Presentation.ViewModels;
using SupportTools_Excel.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;

namespace SupportTools_Excel.ActiveDirectoryExplorer.Presentation.Views
{
    public partial class ActiveDirectoryExplorer : UserControl, IView
    {
        #region Constructors and Load

        // View First.  
        // View is passed ViewModel through Injection
        // or can declare ViewModel in Xaml or Code

        // ViewModel First.  
        // ViewModel creates View 
        // and sets DataContext by setting ViewModel property

        public ActiveDirectoryExplorer()
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            InitializeComponent();

            // If View First with ViewModel in Xaml
            // Expose ViewModel
            // ViewModel = (I$customTYPE$ViewModel)DataContext;

            // Can create directly
            ViewModel = new ActiveDirectoryExplorerViewModel();
            ViewModel.View = this;

            InitializeView();

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        public ActiveDirectoryExplorer(IADMainViewModel viewModel)
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
            LoadControlContents();
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

        private void LoadControlContents()
        {
            try
            {
                wucActiveDirectory_Picker.PopulateControlFromFile(Common.cCONFIG_FILE);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

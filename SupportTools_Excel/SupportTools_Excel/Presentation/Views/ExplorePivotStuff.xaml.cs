using System;
using System.Windows;

using SupportTools_Excel.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;

namespace SupportTools_Excel.Presentation.Views
{
    public partial class ExplorePivotStuff : ViewBase, IExplorePivotStuff, IInstanceCountV
    {

        public ExplorePivotStuff()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            // If View First with ViewModel in Xaml
            // Expose ViewModel
            ViewModel = (IExplorePivotStuffViewModel)DataContext;

            // Can create directly
            // ViewModel = CatViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public ExplorePivotStuff(IExplorePivotStuffViewModel viewModel)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()}", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            ViewModel = viewModel;

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #region IInstanceCount

        private static int _instanceCountV;

        public int InstanceCountV
        {
            get => _instanceCountV;
            set => _instanceCountV = value;
        }

        #endregion

    }
}

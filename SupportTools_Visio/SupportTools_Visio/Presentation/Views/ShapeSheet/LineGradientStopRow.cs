using System.Windows.Controls;
using SupportTools_Visio.Presentation.ViewModels;
using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class LineGradientStopRow : UserControl
    {
        private readonly LineGradientStopRowViewModel _viewModel;

        #region Constructors and Load

        public LineGradientStopRow(LineGradientStopRowViewModel viewModel)
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        #endregion
    }
}

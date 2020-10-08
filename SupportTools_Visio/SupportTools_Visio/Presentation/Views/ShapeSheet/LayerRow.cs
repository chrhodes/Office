using System.Windows.Controls;
using SupportTools_Visio.Presentation.ViewModels;
using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class LayerRow : UserControl
    {
        private readonly LayerRowViewModel _viewModel;

        #region Constructors and Load

        public LayerRow(LayerRowViewModel viewModel)
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

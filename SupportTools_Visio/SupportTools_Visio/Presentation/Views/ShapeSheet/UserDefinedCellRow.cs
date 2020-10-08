using System.Windows.Controls;
using SupportTools_Visio.Presentation.ViewModels;
using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class UserDefinedCellRow : UserControl
    {
        private readonly UserDefinedCellRowViewModel _viewModel;

        #region Constructors and Load

        public UserDefinedCellRow(UserDefinedCellRowViewModel viewModel)
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

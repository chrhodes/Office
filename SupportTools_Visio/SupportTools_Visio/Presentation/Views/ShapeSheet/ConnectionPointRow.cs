using System.Windows.Controls;
using SupportTools_Visio.Presentation.ViewModels;
using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class ConnectionPointRow : UserControl
    {
        //private readonly ConnectionPointRowViewModel _viewModel;

        #region Constructors and Load

        public ConnectionPointRow(ConnectionPointsViewModel viewModel)
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            InitializeComponent();
            //_viewModel = viewModel;
            //DataContext = _viewModel;
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        #endregion
    }
}

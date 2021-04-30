using System.Windows.Controls;

using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class OneDEndPoints : UserControl
    {
        //private readonly OneDEndPointsViewModel _viewModel;

        #region Constructors and Load

        public OneDEndPoints()
        {
            Log.CONSTRUCTOR("Enter", Common.PROJECT_NAME);
            InitializeComponent();
            //_viewModel = viewModel;
            //DataContext = _viewModel;
            Log.CONSTRUCTOR("Exit", Common.PROJECT_NAME);
        }

        #endregion
    }
}

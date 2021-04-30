using System.Windows.Controls;
using SupportTools_Visio.Presentation.ViewModels;
using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class Events : UserControl
    {
        //private readonly EventsViewModel _viewModel;

        #region Constructors and Load

        public Events()
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

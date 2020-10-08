using System.Windows.Controls;
using SupportTools_Visio.Presentation.ViewModels;
using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class TextTransform : UserControl
    {
        //private readonly TextTransformViewModel _viewModel;

        #region Constructors and Load

        public TextTransform()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            //_viewModel = viewModel;
            //DataContext = _viewModel;
            InitializeComponent();
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        #endregion
    }
}

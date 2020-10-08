using System.Windows.Controls;
using SupportTools_Visio.Presentation.ViewModels;
using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class TextBlockFormat : UserControl
    {
        //private readonly TextBlockFormatViewModel _viewModel;

        #region Constructors and Load

        public TextBlockFormat()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            InitializeComponent();
            //_viewModel = viewModel;
            //DataContext = _viewModel;
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        //public TextBlockFormat(TextBlockFormatViewModel viewModel)
        //{
        //    Log.Trace("Enter", Common.PROJECT_NAME);
        //    InitializeComponent();
        //    _viewModel = viewModel;
        //    DataContext = _viewModel;
        //    Log.Trace("Exit", Common.PROJECT_NAME);
        //}

        #endregion
    }
}

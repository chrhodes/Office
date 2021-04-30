using System.Windows.Controls;

using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class AdditionalEffectProperties : UserControl
    {
        //private readonly AdditionalEffectPropertiesViewModel _viewModel;

        #region Constructors and Load

        public AdditionalEffectProperties()
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

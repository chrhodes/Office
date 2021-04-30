using System.Windows.Controls;

using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class PageProperties : UserControl
    {
        //private readonly DocumentPropertiesViewModel _viewModel;

        #region Constructors and Load

        public PageProperties()
        {
            Log.CONSTRUCTOR("Enter", Common.PROJECT_NAME);

            InitializeComponent();

            Log.CONSTRUCTOR("Exit", Common.PROJECT_NAME);
        }

        #endregion
    }
}

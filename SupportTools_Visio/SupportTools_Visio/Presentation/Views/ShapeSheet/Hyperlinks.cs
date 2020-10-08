using System.Windows.Controls;
using SupportTools_Visio.Presentation.ViewModels;
using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class Hyperlinks : UserControl
    {
        #region Constructors and Load

        public Hyperlinks()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            InitializeComponent();
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        #endregion
    }
}

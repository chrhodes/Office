using System.Windows.Controls;
using SupportTools_Visio.Presentation.ViewModels;
using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class ShapeData : UserControl
    {
        #region Constructors and Load

        public ShapeData()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            InitializeComponent();
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        #endregion
    }
}

using System;
using System.Windows.Controls;
using SupportTools_Visio.Presentation.ViewModels;
using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class PageScratchRows : UserControl
    {
        #region Constructors and Load

        public PageScratchRows()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.PROJECT_NAME);

            InitializeComponent();

            Log.CONSTRUCTOR("Exit", Common.PROJECT_NAME, startTicks);
        }

        #endregion
    }
}

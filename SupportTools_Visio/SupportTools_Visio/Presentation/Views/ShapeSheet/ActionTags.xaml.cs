using System;
using System.Windows.Controls;

using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class ActionTags : UserControl
    {
        #region Constructors and Load

        public ActionTags()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.PROJECT_NAME);

            InitializeComponent();

            Log.CONSTRUCTOR("Exit", Common.PROJECT_NAME, startTicks);
        }

        #endregion
    }
}

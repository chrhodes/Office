using System;

using Microsoft.Office.Tools.Ribbon;

using VNC;
using VNC.Core.Presentation;
using VNC.WPF.Presentation.Dx.Views;

namespace SupportTools_Visio
{
    public partial class Ribbon
    {
        #region Event Handlers

        #region WPF Events - Custom

        private void btnXMLPagesCommands_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void btnCommandCockpit_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public static DxThemedWindowHost editControlRowsHost = null;

        private void btnEditControlRows_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref editControlRowsHost,
                "Edit Control Rows",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                ShowWindowMode.Modal_ShowDialog,
                new Presentation.Views.EditControlRows(new Presentation.ViewModels.EditControlRowsViewModel()));

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public static DxThemedWindowHost editParagraphHost = null;

        //public static VNC.Core.Xaml.Presentation.WindowHost editControlPointsHost = null;
        private void btnEditParagraph_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref editParagraphHost,
                "Edit Paragraph",
                300, 600,
                ShowWindowMode.Modeless_Show,
                new Presentation.Views.EditParagraph(new Presentation.ViewModels.EditParagraphViewModel()));

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private Presentation.Views.EditControlPoints editControlPointsUC = null;

        public static VNC.WPF.Presentation.Dx.Views.DxThemedWindowHost editTextHost = null;

        // static VNC.Core.Xaml.Presentation.WindowHost editTextHost = null;
        private Presentation.Views.EditText editTextUC = null;

        public static VNC.WPF.Presentation.Dx.Views.DxThemedWindowHost editControlPointsHost = null;

        private void btnEditControlPoints_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref editControlPointsHost,
                "Edit Shape Control Points Text",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                ShowWindowMode.Modeless_Show,
                new Presentation.Views.EditControlPoints());

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void btnEditText_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref editTextHost,
                "Edit Text",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                ShowWindowMode.Modeless_Show,
                new Presentation.Views.EditText(new Presentation.ViewModels.EditTextViewModel()));

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #endregion Event Handlers
    }
}
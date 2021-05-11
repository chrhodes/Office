using Microsoft.Office.Tools.Ribbon;

using VNC.Core.Presentation;
using VNC.WPF.Presentation.Dx.Views;

namespace SupportTools_Visio
{
    public partial class Ribbon
    {
        #region Event Handlers

        #region WPF Events - Custom

        public static DxThemedWindowHost editControlRowsHost = null;

        private void btnEditControlRows_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref editControlRowsHost,
                "Edit Control Rows",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                ShowWindowMode.Modal_ShowDialog,
                new Presentation.Views.EditControlRows(new Presentation.ViewModels.EditControlRowsViewModel()));
        }

        public static DxThemedWindowHost editParagraphHost = null;

        //public static VNC.Core.Xaml.Presentation.WindowHost editControlPointsHost = null;
        private void btnEditParagraph_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref editParagraphHost,
                "Edit Paragraph",
                300, 600,
                ShowWindowMode.Modeless_Show,
                new Presentation.Views.EditParagraph(new Presentation.ViewModels.EditParagraphViewModel()));
        }

        private Presentation.Views.EditControlPoints editControlPointsUC = null;

        public static VNC.WPF.Presentation.Dx.Views.DxThemedWindowHost editTextHost = null;

        // static VNC.Core.Xaml.Presentation.WindowHost editTextHost = null;
        private Presentation.Views.EditText editTextUC = null;

        public static VNC.WPF.Presentation.Dx.Views.DxThemedWindowHost editControlPointsHost = null;

        private void btnEditControlPoints_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref editControlPointsHost,
                "Edit Shape Control Points Text",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                ShowWindowMode.Modeless_Show,
                new Presentation.Views.EditControlPoints());
        }

        private void btnEditText_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref editTextHost,
                "Edit Text",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                ShowWindowMode.Modeless_Show,
                new Presentation.Views.EditText(new Presentation.ViewModels.EditTextViewModel()));
        }

        #endregion

        #region SMARTS Events

        private void btnHilight_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmEditVisio();
            frm.Show();
        }

        private void btnNavigateDown_Click(object sender, RibbonControlEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("TODO: Navigate Down");
        }

        private void btnNavigateUp_Click(object sender, RibbonControlEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("TODO: Navigate Up");
        }

        private void btnRelatedIntfrastructure_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmRelateShape();
            frm.Show();
        }

        private void btnRelatedProcess_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmRelateShape();
            frm.Show();
        }

        private void btnRelatedSystem_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmRelateShape();
            frm.Show();
        }

        private void btnRetrive_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmRetrieveShape();
            frm.Show();
        }

        private void btnValidate_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmRetrieveShape();
            frm.Show();
        }

        private void btnWebPage_Click(object sender, RibbonControlEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("TODO: Navigate to Web Page");
        }

        #endregion SMARTS Events

        #endregion Event Handlers
    }
}
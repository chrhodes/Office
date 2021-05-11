using System;
using System.Windows;
using System.Windows.Controls;

using dxe = DevExpress.Xpf.Editors;

using VisioHelper = VNC.AddinHelper.Visio;

namespace SupportTools_Visio.User_Interface.User_Controls
{
    public partial class wucRenamePages : UserControl
    {
        #region Constructors and Load

        public wucRenamePages()
        {
            InitializeComponent();
            LoadControlContents();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            VNC.Log.Trace("", Common.LOG_CATEGORY, 0);
            VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            VNC.Log.Trace("", Common.LOG_CATEGORY, 0);
            VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));
        }

        #endregion

        #region Event Handlers

        private void btnExecuteCommand_Click(object sender, RoutedEventArgs e)
        {
            VNC.Log.Trace("", Common.LOG_CATEGORY, 0);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("ParseCommand");

            Actions.Visio_Document.RenamePages(teSearchExpression.Text, teReplacementExpression.Text);

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);
        }

        private void cbeDefaultPatterns_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            dxe.ComboBoxEdit control = (dxe.ComboBoxEdit)sender;

            dxe.ComboBoxEditItem item = (dxe.ComboBoxEditItem)control.SelectedItem;

            switch (item.Tag)
            {
                case "Front":
                    teSearchExpression.Text = "XXX(.*$)";
                    teReplacementExpression.Text = "YYY$1";
                    break;

                case "Middle":
                    teSearchExpression.Text = "(^.*)XXX(.*$)";
                    teReplacementExpression.Text = "$1YYY$2";
                    break;

                case "End":
                    teSearchExpression.Text = "(^.*)XXX$";
                    teReplacementExpression.Text = "$1YYY";
                    break;

                case "Empty":
                    teSearchExpression.Text = "";
                    teReplacementExpression.Text = "";
                    break;

                default:
                    teSearchExpression.Text = "";
                    teReplacementExpression.Text = "";
                    break;
            }
        }

        #endregion


        #region Private Methods

        private void LoadControlContents()
        {
            try
            {
                //visioCommand_Picker.PopulateControlFromFile(Common.cCONFIG_FILE);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion
    }
}

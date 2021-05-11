using System;
using System.Windows;
using System.Windows.Controls;

using dxe = DevExpress.Xpf.Editors;

using Microsoft.Office.Interop.Visio;
using VisioHelper = VNC.AddinHelper.Visio;
using VNC;

namespace SupportTools_Visio.User_Interface.User_Controls
{
    public partial class wucMovePages : UserControl
    {
        #region Constructors and Load

        public wucMovePages()
        {
            Log.Trace("Enter", Common.LOG_CATEGORY);
            InitializeComponent();
            LoadControlContents();

            Log.Trace("Exit", Common.LOG_CATEGORY);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Log.Trace("Enter", Common.LOG_CATEGORY, 0);
            VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            Documents documents = Globals.ThisAddIn.Application.Documents;
            Document currentDocument = Globals.ThisAddIn.Application.ActiveDocument;

            cbeOpenDocuments.Items.Clear();

            foreach (Document document in documents)
            {
                if (!currentDocument.Name.Equals(document.Name))
                {
                    if (document.Type.Equals(VisDocumentTypes.visTypeDrawing))
                    {
                        cbeOpenDocuments.Items.Add(document.Name);
                    }          
                }
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            VNC.Log.Trace("Enter", Common.LOG_CATEGORY, 0);
            VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            Log.Trace("Exit", Common.LOG_CATEGORY);
        }

        #endregion

        #region Event Handlers

        private void btnExecuteCommand_Click(object sender, RoutedEventArgs e)
        {
            Log.Trace("Enter", Common.LOG_CATEGORY, 0);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("ParseCommand");

            // TODO(crhodes)
            // Get this from UI

            string targetDocument = (string)cbeOpenDocuments.SelectedItemValue;
            Actions.Visio_Document.MovePages(targetDocument);

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);

            Log.Trace("Exit", Common.LOG_CATEGORY);
        }

        private void cbeDefaultPatterns_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            dxe.ComboBoxEdit control = (dxe.ComboBoxEdit)sender;

            dxe.ComboBoxEditItem item = (dxe.ComboBoxEditItem)control.SelectedItem;
        }

        private void cbeOpenDocuments_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

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

using Microsoft.Office.Tools.Ribbon;

using VNC;
using VNC.Core.Presentation;
using VNC.WPF.Presentation.Dx.Views;

namespace SupportTools_Visio
{
    public partial class Ribbon
    {
        #region Event Handlers

        #region Document Actions Events

        public static DxThemedWindowHost duplicatePage_Host = null;

        private void btnAddDefaultLayers_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.AddDefaultLayers();
        }

        private void btnAddFooter_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.AddFooter();
        }

        private void btnAddHeader_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.AddHeader();
        }

        private void btnAddNavigationLinks_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.AddNavigationLinks();
        }

        private void btnAddTableOfContents_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.CreateTableOfContents();
        }

        private void btnAllPageOff_Click(object sender, RibbonControlEventArgs e)
        {
            string layerName = cmbLayers.Text;

            if (layerName.Length > 0)
            {
                Actions.Visio_Document.DisplayLayer(layerName, false);
            }
        }

        private void btnAllPageOn_Click(object sender, RibbonControlEventArgs e)
        {
            string layerName = cmbLayers.Text;

            if (layerName.Length > 0)
            {
                Actions.Visio_Document.DisplayLayer(layerName, true);
            }
        }

        private void btnAutoSizePagesOff_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.AutoSizePagesOff();
        }

        private void btnAutoSizePagesOn_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.AutoSizePagesOn();
        }

        private void btnDeletePages_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.DeletePages();
        }

        private void btnDisplayPageNames_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.DisplayPageNames();
        }

        private void btnDuplicatePage_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref duplicatePage_Host,
            "Duplicate Page",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            ShowWindowMode.Modeless_Show,
            new Presentation.Views.DuplicatePage(new Presentation.ViewModels.DuplicatePageViewModel()));
        }

        private void btnGetApplicationInfo_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Application.DisplayInfo();
        }

        private void btnGetDocumentInfo_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.DisplayInfo();
        }

        private void btnGetStencilInfo_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Stencil.GatherInfo();
        }

        private void btnLoadLayers_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Application.LayerManager();
        }

        private void btnMovePages_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmMovePages();
            frm.Show();
        }

        private void btnPrintPages_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.PrintPages();
        }

        private void btnRemoveLayers_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.RemoveLayers();
        }

        private void btnRenamePages_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmRenamePages();
            frm.Show();
        }

        private void btnSavePages_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.SavePages();
        }

        private void btnSortAllPages_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.SortAllPages();
        }

        private void btnSyncPageNames_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.SyncPageNames();
        }

        private void btnUpdatePageNameShapes_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.UpdatePageNameShapes();
        }

        #endregion Document Actions Events

        #region Page Actions Events

        private void btnAddNavLinks_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.AddNavigationLinks(Globals.ThisAddIn.Application.ActivePage);
        }

        private void btnAutoSizePageOff_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.AutoSizePageOff();
        }

        private void btnAutoSizePageOn_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.AutoSizePageOn();
        }

        private void btnGetPageInfo_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.GatherInfo(Globals.ThisAddIn.Application.ActivePage);
        }

        private void btnPrintPage_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.PrintPage();
        }

        private void btnSavePage_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.SavePage(Globals.ThisAddIn.Application.ActivePage);
        }

        private void btnSyncPageNamesPage_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.SyncPageNames();
        }

        private void btnUpdatePageNameShapesPage_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.UpdatePageNameShapes(Globals.ThisAddIn.Application.ActivePage);
        }

        #endregion Page Actions Events

        #region Layer Actions Events

        private void btnAddDefaultLayers_Page_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.AddDefaultLayers();
        }

        private void btnLockBackground_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.LockLayer("Background");
        }

        private void btnPageOff_Click(object sender, RibbonControlEventArgs e)
        {
            string layerName = cmbLayers.Text;

            if (layerName.Length > 0)
            {
                Actions.Visio_Page.DisplayLayer(Globals.ThisAddIn.Application.ActivePage, layerName, false);
            }
        }

        private void btnPageOn_Click(object sender, RibbonControlEventArgs e)
        {
            string layerName = cmbLayers.Text;

            if (layerName.Length > 0)
            {
                Actions.Visio_Page.DisplayLayer(Globals.ThisAddIn.Application.ActivePage, layerName, true);
            }
        }

        private void btnRemoveLayers_Page_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.RemoveLayers();
        }

        private void btnUnlockBackground_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.UnlockLayer("Background");
        }

        #endregion Layer Actions Events

        #region Visio_Shape Events

        private void btn0PtMargins_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.SetMargins("0 pt");
        }

        private void btn1PtMargins_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.SetMargins("1 pt");
        }

        private void btn2PtMargins_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.SetMargins("2 pt");
        }

        private void btnAddColorSupport_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.AddColorSupportToSelection();
        }

        private void btnAddHyperLink_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.AddHyperlinkToPage_FromShapeText();
        }

        private void btnAddIDAndTextSupport_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.Add_IDandTextSupport_ToSelection();
        }

        private void btnAddIDSupport_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.Add_IDSupport_ToSelection();
        }

        private void btnAddIsPageName_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.Add_User_IsPageName();
        }

        private void btnAddTextControl_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.Add_TextControl_ToSelection();
        }

        private void btnGetShapeInfo_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.GatherInfo();
        }

        private void btnMakeLinkableMaster_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.MakeLinkableMaster();
        }

        private void btnMoveToBackgroundLayer_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.MoveToBackgroundLayer();
        }

        #endregion Visio_Shape Events

        #region Debug Events

        private void btnDebugWindow_Click(object sender, RibbonControlEventArgs e)
        {
            DisplayDebugWindow();
        }

        private void btnDeveloperMode_Click(object sender, RibbonControlEventArgs e)
        {
            ToggleDeveloperMode();
        }

        private void btnWatchWindow_Click(object sender, RibbonControlEventArgs e)
        {
            DisplayWatchWindow();
        }

        private void chkDisplayChattyEvents_Click(object sender, RibbonControlEventArgs e)
        {
            Common.DisplayChattyEvents = chkDisplayChattyEvents.Checked;
        }

        private void chkDisplayEvents_Click(object sender, RibbonControlEventArgs e)
        {
            Common.DisplayEvents = chkDisplayEvents.Checked;
        }

        private void chkEnableAppEvents_Click(object sender, RibbonControlEventArgs e)
        {
            Common.HasAppEvents = chkEnableAppEvents.Checked;

            if (Common.HasAppEvents)
            {
                if (Common.AppEvents == null)
                {
                    Common.AppEvents = new Events.VisioAppEvents();
                    Common.AppEvents.VisioApplication = Globals.ThisAddIn.Application;
                }
            }
            else
            {
                Common.AppEvents = null;
            }
        }

        #endregion Debug Events

        #region Main Function Routines

        private void btnAddInInfo_Click(object sender, RibbonControlEventArgs e)
        {
            DisplayAddInInfo();
        }

        private void DisplayAddInInfo()
        {
            long startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            VNC.AddinHelper.AddInInfo.DisplayInfo();

            Log.Trace("Exit", Common.LOG_CATEGORY);
        }

        private void DisplayDebugWindow()
        {
            long startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            VNC.AddinHelper.Common.DebugWindow.Visible = !VNC.AddinHelper.Common.DebugWindow.Visible;

            Log.Trace("Exit", Common.LOG_CATEGORY);
        }

        private void DisplayWatchWindow()
        {
            long startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            VNC.AddinHelper.Common.WatchWindow.Visible = !VNC.AddinHelper.Common.WatchWindow.Visible;

            Log.Trace("Exit", Common.LOG_CATEGORY);
        }

        private void ToggleDeveloperMode()
        {
            long startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            VNC.AddinHelper.Common.DeveloperMode = !VNC.AddinHelper.Common.DeveloperMode;
            Globals.Ribbons.Ribbon.rgDebug.Visible = VNC.AddinHelper.Common.DeveloperMode;

            Log.Trace("Exit", Common.LOG_CATEGORY);
        }

        #endregion Main Function Routines

        #endregion Event Handlers
    }
}
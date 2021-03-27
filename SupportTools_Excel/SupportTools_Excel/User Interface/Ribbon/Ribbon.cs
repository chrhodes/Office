using System;
using System.Threading;
using System.Windows;

using Microsoft.Office.Tools.Ribbon;
using Prism.Unity;
using SupportTools_Excel.Presentation.ViewModels;
using SupportTools_Excel.Presentation.Views;

using VNC;
using VNC.WPF.Presentation.Dx.Views;
using VNC.WPF.Presentation.Views;

using ExcelHlp = VNC.AddinHelper.Excel;
using VNCHlp = VNC.AddinHelper;

namespace SupportTools_Excel
{
    public partial class Ribbon
    {
        // NOTE(crhodes)
        // This was moved out of designer so we can call bootstrapper.

        public Ribbon()
           : base(Globals.Factory.GetRibbonFactory())
        {
            Log.Info("SignalR Startup Message - Sleeping for 1000ms so SignalR can load", Common.LOG_APPNAME);
            // HACK(crhodes)
            // See if this helps logging first few messages
            Thread.Sleep(250);

            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            InitializeComponent();

            // NOTE(crhodes)
            // Try moving Bootstrapper to Common so we can access UnityContainer
            Common.ApplicationBootstrapper = new Application.Bootstrapper();
            Common.ApplicationBootstrapper.Run();
            //var bootstrapper = new Application.Bootstrapper();
            //bootstrapper.Run();

            Log.CONSTRUCTOR("Exit", Common.LOG_APPNAME, startTicks);
        }

        #region Event Handlers

        private void btnExplore_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            var frm = new User_Interface.Forms.frmExploreHost();
            frm.Show();

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void btnLoadTPHost_ActiveDirectory_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            var frm = new User_Interface.Forms.frmTaskPaneHost_ActiveDirectory();
            frm.Show();

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void btnLoadSMOHost_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            var frm = new User_Interface.Forms.frmSMOHost();
            frm.Show();

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void btnSMO_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            if (Common.TaskPaneSMO == null)
            {
                Common.TaskPaneSMO = VNCHlp.TaskPaneUtil.AddTaskPane(
                    new User_Interface.Task_Panes.TaskPane_SMO(), 
                    "SMO Utilities", Globals.ThisAddIn.CustomTaskPanes);
                // This works if the minimum size for the control has been set.
                Common.TaskPaneSMO.Width = Common.TaskPaneSMO.Control.Width;
            }
            else
            {
                Common.TaskPaneSMO.Visible = !Common.TaskPaneSMO.Visible;
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        //private void btnTPDevelopment_Click(object sender, RibbonControlEventArgs e)
        //{
        //    if (Common.TaskPaneDevelopment == null)
        //    {
        //        Common.TaskPaneDevelopment = VNCHlp.TaskPaneUtil.AddTaskPane(
        //            new User_Interface.Task_Panes.TaskPane_Developer(), 
        //            "Developer Utilities", Globals.ThisAddIn.CustomTaskPanes);
        //        // This works if the minimum size for the control has been set.
        //        Common.TaskPaneDevelopment.Width = Common.TaskPaneDevelopment.Control.Width;
        //    }
        //    else
        //    {
        //        Common.TaskPaneDevelopment.Visible = !Common.TaskPaneDevelopment.Visible;
        //    }
        //}

        private void btnTPDevelopment_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            if (Common.TaskPaneDevelopment == null)
            {
                Common.TaskPaneDevelopment = VNCHlp.TaskPaneUtil.GetTaskPane(
                    () => new User_Interface.Task_Panes.TaskPane_Developer(), "Developer Utilities",
                    Globals.ThisAddIn.CustomTaskPanes, Globals.ThisAddIn.Application.Hwnd.ToString());

                // This works if the minimum size for the control has been set.
                Common.TaskPaneDevelopment.Width = Common.TaskPaneDevelopment.Control.Width;
                Common.TaskPaneDevelopment.Visible = ! Common.TaskPaneDevelopment.Visible;
            }
            else
            {
                Common.TaskPaneDevelopment.Visible = !Common.TaskPaneDevelopment.Visible;
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void ddTheme_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            // TODO(crhodes):
            // This doesn't work.  Try putting it in Support Tools
            DevExpress.Xpf.Core.ThemeManager.ApplicationThemeName = DevExpress.Xpf.Core.Theme.MetropolisLightName;

            DevExpress.Xpf.Core.ThemeManager.ApplicationThemeName = ((RibbonDropDown)sender).SelectedItem.Label;

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void btnActiveDirectory_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            if (Common.TaskPaneActiveDirectory == null)
            {
                Common.TaskPaneActiveDirectory = VNCHlp.TaskPaneUtil.AddTaskPane(
                    new User_Interface.Task_Panes.TaskPane_ActiveDirectory(), 
                    "Active Directory Utilities", Globals.ThisAddIn.CustomTaskPanes);
                // This works if the minimum size for the control has been set.
                Common.TaskPaneActiveDirectory.Width = Common.TaskPaneActiveDirectory.Control.Width;
            }
            else
            {
                Common.TaskPaneActiveDirectory.Visible = !Common.TaskPaneActiveDirectory.Visible;
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void btnAddInInfo_Click(object sender, RibbonControlEventArgs e)
        {
            DisplayAddInInfo();
        }

        private void btnDebugWindow_Click(object sender, RibbonControlEventArgs e)
        {
            DisplayDebugWindow();
        }

        private void btnDeveloperMode_Click(object sender, RibbonControlEventArgs e)
        {
            ToggleDeveloperMode();
        }

        #region TaskPane Hosts

        private void btnAppUtilities_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            if (Common.TaskPaneAppUtilities == null)
            {
                Common.TaskPaneAppUtilities = VNCHlp.TaskPaneUtil.GetTaskPane(
                    () => new User_Interface.Task_Panes.TaskPane_ExcelUtil(), "App Utilities",
                    Globals.ThisAddIn.CustomTaskPanes, Globals.ThisAddIn.Application.Hwnd.ToString());

                Common.TaskPaneAppUtilities.Width = Common.TaskPaneAppUtilities.Control.Width;
                Common.TaskPaneAppUtilities.Visible = !Common.TaskPaneAppUtilities.Visible;
            }
            else
            {
                Common.TaskPaneAppUtilities.Visible = !Common.TaskPaneAppUtilities.Visible;
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void btnExcelUtilities_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            if (Common.TaskPaneUtilities == null)
            {
                Common.TaskPaneUtilities = VNCHlp.TaskPaneUtil.GetTaskPane(
                   () => new User_Interface.Task_Panes.TaskPane_Utilities(), "Excel Utilities",
                   Globals.ThisAddIn.CustomTaskPanes, Globals.ThisAddIn.Application.Hwnd.ToString());

                // This works if the minimum size for the control has been set.
                Common.TaskPaneUtilities.Width = Common.TaskPaneUtilities.Control.Width;
                Common.TaskPaneUtilities.Visible = !Common.TaskPaneUtilities.Visible;
            }
            else
            {
                Common.TaskPaneUtilities.Visible = !Common.TaskPaneUtilities.Visible;
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void btnSharePoint_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            Common.TaskPaneSharePoint = VNCHlp.TaskPaneUtil.GetTaskPane(
                () => new User_Interface.Task_Panes.TaskPane_SharePoint(), "SharePoint Utilities",
                Globals.ThisAddIn.CustomTaskPanes, Globals.ThisAddIn.Application.Hwnd.ToString());

            // This works if the minimum size for the control has been set.
            Common.TaskPaneSharePoint.Width = Common.TaskPaneSharePoint.Control.Width;
            Common.TaskPaneSharePoint.Visible = !Common.TaskPaneSharePoint.Visible;

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void btnTFS_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            Common.TaskPaneTFS = VNCHlp.TaskPaneUtil.GetTaskPane(
                () => new User_Interface.Task_Panes.TaskPane_TFS(), "TFS Utilities",
                Globals.ThisAddIn.CustomTaskPanes, Globals.ThisAddIn.Application.Hwnd.ToString());

            // This works if the minimum size for the control has been set.
            Common.TaskPaneTFS.Width = Common.TaskPaneTFS.Control.Width;
            Common.TaskPaneTFS.Visible = !Common.TaskPaneTFS.Visible;

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        #endregion

        #region Winform Hosts

        // Load using Winform

        private void btnLoadTFSHost_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            var frm = new User_Interface.Forms.frmTFSHost();
            frm.Show();

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        #endregion

        #region WPF Hosts

        public static DxThemedWindowHost ad_Host = null;

        private void btnLoadADHost_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref ad_Host,
            "Active Directory Explorer",
            600, 900,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new User_Interface.User_Controls.wucTaskPane_ActiveDirectory());

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        public static DxThemedWindowHost ad_HostMVVM = null;

        private void btnLoadActiveDirectoryHostMVVM_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref ad_HostMVVM,
            "Active Directory Explorer (MVVM)",
            600, 900,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new ActiveDirectoryExplorer.Presentation.Views.ActiveDirectoryExplorer());

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        public static DxThemedWindowHost azdo_Host = null;

        private void btnLoadAZDOHost_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref azdo_Host,
            "wucTaskPane_TFS",
            600, 900,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new User_Interface.User_Controls.wucTaskPane_TFS());

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        #endregion


        //private void btnITRs_Click(object sender, RibbonControlEventArgs e)
        //{
        //    if(Common.TaskPaneITRs == null)
        //    {
        //        Common.TaskPaneITRs = AddinHelper.TaskPaneUtil.AddTaskPane(new User_Interface.Task_Panes.TaskPane_ITRs(), "ITRs", Globals.ThisAddIn.CustomTaskPanes);
        //                        // This works if the minimum size for the control has been set.
        //          Common.TaskPaneITRs.Width = Common.TaskPaneITRs.Control.Width;
        //    }
        //    else
        //    {
        //        Common.TaskPaneITRs.Visible = ! Common.TaskPaneITRs.Visible;
        //    }
        //}

        private void btnLogParser_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            if (Common.TaskPaneLogParser == null)
            {
                Common.TaskPaneLogParser = VNCHlp.TaskPaneUtil.GetTaskPane(
                      () => new User_Interface.Task_Panes.TaskPane_LogParser(), "Log Parser",
                      Globals.ThisAddIn.CustomTaskPanes, Globals.ThisAddIn.Application.Hwnd.ToString());

                // This works if the minimum size for the control has been set.
                Common.TaskPaneLogParser.Width = Common.TaskPaneLogParser.Control.Width;
                Common.TaskPaneLogParser.Visible = ! Common.TaskPaneLogParser.Visible;
            }
            else
            {
                Common.TaskPaneLogParser.Visible = ! Common.TaskPaneLogParser.Visible;
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void btnLTC_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            if (Common.TaskPaneLTC == null)
            {
                Common.TaskPaneLTC = VNCHlp.TaskPaneUtil.GetTaskPane(
                    () => new User_Interface.Task_Panes.TaskPane_LTC(), "LTC Utilities",
                    Globals.ThisAddIn.CustomTaskPanes, Globals.ThisAddIn.Application.Hwnd.ToString());

                // This works if the minimum size for the control has been set.
                Common.TaskPaneLTC.Width = Common.TaskPaneLTC.Control.Width;
                Common.TaskPaneLTC.Visible = ! Common.TaskPaneLTC.Visible;
            }
            else
            {
                Common.TaskPaneLTC.Visible = ! Common.TaskPaneLTC.Visible;
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void btnMTreaty_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            if (Common.TaskPaneMTreaty == null)
            {
                Common.TaskPaneMTreaty = VNCHlp.TaskPaneUtil.GetTaskPane(
                    () => new User_Interface.Task_Panes.TaskPane_MTreaty(), "MTreaty Utilities",
                    Globals.ThisAddIn.CustomTaskPanes, Globals.ThisAddIn.Application.Hwnd.ToString());

                // This works if the minimum size for the control has been set.
                Common.TaskPaneMTreaty.Width = Common.TaskPaneMTreaty.Control.Width;
                Common.TaskPaneMTreaty.Visible = !Common.TaskPaneMTreaty.Visible;
            }
            else
            {
                Common.TaskPaneMTreaty.Visible = !Common.TaskPaneMTreaty.Visible;
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void btnNetworkTraces_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            if (Common.TaskPaneNetworkTrace == null)
            {
                Common.TaskPaneNetworkTrace = VNCHlp.TaskPaneUtil.GetTaskPane(
                    () => new User_Interface.Task_Panes.TaskPane_NetworkTrace(), "Network Traces",
                    Globals.ThisAddIn.CustomTaskPanes, Globals.ThisAddIn.Application.Hwnd.ToString());

                // This works if the minimum size for the control has been set.
                Common.TaskPaneNetworkTrace.Width = Common.TaskPaneNetworkTrace.Control.Width;
                Common.TaskPaneNetworkTrace.Visible = !Common.TaskPaneNetworkTrace.Visible;
            }
            else
            {
                Common.TaskPaneNetworkTrace.Visible = !Common.TaskPaneNetworkTrace.Visible;
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void btnSQLSMO_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_APPNAME);

            if (Common.TaskPaneSQLSMO == null)
            {
                Common.TaskPaneSQLSMO = VNCHlp.TaskPaneUtil.AddTaskPane(new User_Interface.Task_Panes.TaskPane_SQLSMO(), "SQL SMO", Globals.ThisAddIn.CustomTaskPanes);
                // This throws an exception
                //Globals.ThisAddIn.Application.CommandBars["SQL SMO"].Width = Common.TaskPaneSQLSMO.Width;
                foreach (Microsoft.Office.Core.CommandBar bar in Globals.ThisAddIn.Application.CommandBars)
                {
                    string foo = bar.Name;

                    if (foo == "SQL SMO")
                    {
                        // Which is curious as the bar is found!
                        //Globals.ThisAddIn.Application.CommandBars["SQL SMO"].Width = Common.TaskPaneSQLSMO.Width;
                    }
                }

                // This works if the minimum size for the control has been set.
                Common.TaskPaneSQLSMO.Width = Common.TaskPaneSQLSMO.Control.Width;
            }
            else
            {
                Common.TaskPaneSQLSMO.Visible = !Common.TaskPaneSQLSMO.Visible;
            }

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void btnWatchWindow_Click(object sender, RibbonControlEventArgs e)
        {
            DisplayWatchWindow();
        }

        private void chkDisplayEvents_Click(object sender, RibbonControlEventArgs e)
        {
            Common.DisplayEvents = chkDisplayEvents.Checked;
        }

        private void chkEnableAppEvents_Click(object sender, RibbonControlEventArgs e)
        {
            Common.HasAppEvents = chkEnableAppEvents.Checked;

            if(Common.HasAppEvents)
            {
                if(Common.AppEvents == null)
                {
                    Common.AppEvents = new Events.ExcelAppEvents();
                    Common.AppEvents.ExcelApplication = Globals.ThisAddIn.Application;
                }
            }
            else
            {
                Common.AppEvents = null;
                Common.AppEvents.ExcelApplication = null;
            }
        }

        private void chkEnableTraceLogging_Click(object sender, RibbonControlEventArgs e)
        {
            Common.EnableLogging = chkEnableTraceLogging.Checked;
        }

        private void chkDisplayXlLocationUpdates_Click(object sender, RibbonControlEventArgs e)
        {
            Common.DisplayXlLocationUpdates = chkDisplayXlLocationUpdates.Checked;
        }

        private void chkScreenUpdates_Click(object sender, RibbonControlEventArgs e)
        {
            ExcelHlp.DisplayScreenUpdates = chkScreenUpdates.Checked;
        }

        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        #endregion

        #region Main Function Routines

        private void DisplayAddInInfo()
        {
            VNCHlp.AddInInfo.DisplayInfo();
        }

        private void DisplayDebugWindow()
        {
            if(VNCHlp.Common.DebugWindow.Visible)
            {
                VNCHlp.Common.DebugWindow.Visible = false;
            }
            else
            {
                VNCHlp.Common.DebugWindow.Visible = true;
            }
        }

        private void DisplayWatchWindow()
        {
            VNCHlp.Common.WatchWindow.Visible = !VNCHlp.Common.WatchWindow.Visible;
        }

        private void ToggleDeveloperMode()
        {
            VNCHlp.Common.DeveloperMode = !VNCHlp.Common.DeveloperMode;
            Globals.Ribbons.Ribbon.grpDebug.Visible = VNCHlp.Common.DeveloperMode;
        }

        #endregion

        #region UI Launch Events

        enum ShowWindowMode
        {
            Modeless_Show,
            Modal_ShowDialog
        }

        private DxThemedWindowHost themedWindowHost = null;

        private void btnThemedWindowHostModeless_Click(object sender, RibbonControlEventArgs e)
        {

            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref themedWindowHost,
                "ThemedWindowHost (ModeLess)",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void btnThemedWIndowHostModal_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref themedWindowHost,
                "ThemedWindowHost (Modal)",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modal);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        public static WindowHost windowHostLocal = null;

        private void btnWindowHostLocal_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            WindowHost.DisplayUserControlInHost(ref windowHostLocal,
                "WindowHost (local) Test",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                WindowHost.ShowWindowMode.Modeless_Show);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        public static VNC.Core.Xaml.Presentation.WindowHost windowHostVNC = null;

        private void btnWindowHostVNC_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            ShowEmptyHost(windowHostVNC, "WindowHost (VNC)", ShowWindowMode.Modal_ShowDialog);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private static void ShowEmptyHost(Window host, string title, ShowWindowMode mode)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            if (host is null)
            {
                host = new DxThemedWindowHost();
                host.Height = Common.DEFAULT_WINDOW_SMALL_HEIGHT;
                host.Width = Common.DEFAULT_WINDOW_SMALL_WIDTH;
                host.Title = title;
            }

            if (mode == ShowWindowMode.Modal_ShowDialog)
            {
                long endTicks2 = Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);

                host.Title = $"{host.GetType()} loadtime: {Log.GetDuration(startTicks, endTicks2)}";

                host.ShowDialog();
            }
            else
            {
                host.Show();
            }

            long endTicks = Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);

            host.Title = $"{host.GetType()} loadtime: {Log.GetDuration(startTicks, endTicks)}";
        }

        private DxDXWindowHost dxWindowHost = null;

        private void btnDxWindowHost_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxDXWindowHost.DisplayUserControlInHost(ref dxWindowHost,
                "DxWindowHost Test",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxDXWindowHost.ShowWindowMode.Modeless_Show);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        #endregion

        #region WPF UI Events

        public static WindowHost cylonHost = null;

        private void btnLaunchCylon_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            WindowHost.DisplayUserControlInHost(ref cylonHost,
                "I am a Cylon loaded by name",
                Common.DEFAULT_WINDOW_SMALL_WIDTH, Common.DEFAULT_WINDOW_SMALL_HEIGHT,
                WindowHost.ShowWindowMode.Modeless_Show,
                "VNC.WPF.Presentation.Views.CylonEyeBall, VNC.WPF.Presentation");

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        public static VNC.Core.Xaml.Presentation.WindowHost cylonHost2 = null;

        private void btnLaunchCylonn2_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            WindowHost.DisplayUserControlInHost(ref cylonHost,
                "I am a Cylon loaded by type",
                Common.DEFAULT_WINDOW_SMALL_WIDTH, Common.DEFAULT_WINDOW_SMALL_HEIGHT,
                WindowHost.ShowWindowMode.Modeless_Show,
                new CylonEyeBall());

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        PrismRegionTest _prismRegionTest;

        public PrismRegionTest PrismRegionTest
        {
            get
            {
                if (_prismRegionTest is null)
                {
                    _prismRegionTest = new PrismRegionTest();
                }

                return _prismRegionTest;
            }
            set
            {
                _prismRegionTest = value;
            }
        }


        private DxThemedWindowHost prismRegionTestHost = null;
        private void btnPrismRegionTest_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref prismRegionTestHost,
                "Prism Region Test 2", 600, 400,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                PrismRegionTest);

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private DxThemedWindowHost dxLayoutControlHost = null;

        private void btnDxLayoutControl_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref dxLayoutControlHost,
                "DxLayoutControl Test",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new DxLayoutControl());

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private DxThemedWindowHost dxDockLayoutControlHost = null;

        private void btnDxDockLayoutControl_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref dxDockLayoutControlHost,
                "DxDockLayoutControl Test",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new DxDockLayoutControl());

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        private DxThemedWindowHost dxDockLayoutManagerControlHost = null;

        private void btnDockLayoutManagerControl_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref dxDockLayoutManagerControlHost,
                "DxDocLayoutManagerControl Test",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new DxDockLayoutManagerControl());

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        #endregion

        #region MVVM Examples

        public static DxThemedWindowHost vncMVVM_V1_Host = null;

        private void btnVNC_MVVM_V1_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_V1_Host,
            "MVVM View First (View is passed ViewModel)",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Cat(new CatViewModel()));

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        public static DxThemedWindowHost vncMVVM_VM1_Host = null;

        private void btnVNC_MVVM_VM1_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_VM1_Host,
            "MVVM ViewModel First (ViewModel is passed View)",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new CatViewModel(new Cat()));

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        public static DxThemedWindowHost vncMVVM_V1_Modal_Host = null;

        private void btnVNC_MVVM_V1_Modal_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_V1_Modal_Host,
            "MVVM View First (View is passed ViewModel) Modal",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modal,
            new Cat(new CatViewModel()));

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        public static DxThemedWindowHost vncMVVM_VM1_Modal_Host = null;

        private void btnVNC_MVVM_VM1_Modal_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_VM1_Modal_Host,
            "MVVM ViewModel First (ViewModel is passed View) Modal",
            800, 600,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modal,
            new CatViewModel(new Cat()));

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        #endregion

        #region MVVM Dependency Injection Examples

        public static DxThemedWindowHost vncMVVM_V1_DI_Host = null;

        private void btnVNC_MVVM_V1_DI_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_V1_DI_Host,
                "MVVM View First (Cat) using Dependency Injection",
                800, 600,
                //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                Common.ApplicationBootstrapper.Container.TryResolve<Cat>());

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        public static DxThemedWindowHost vncMVVM_V1_DI2_Host = null;

        private void btnVNC_MVVM_V1_DI2_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_V1_DI2_Host,
                "MVVM View First (Cat2) using Dependency Injection",
                800, 600,
                //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                Common.ApplicationBootstrapper.Container.TryResolve<Cat2>());

            // NOTE(crhodes)
            // Hum.  This is interesting.  Have not registered Cat2 in Bootstrapper but still Resolved!

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        public static DxThemedWindowHost vncMVVM_V1_DI3_Host = null;

        private void btnVNC_MVVM_V1_DI3_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_V1_DI3_Host,
                "MVVM View First (Cat3) using Dependency Injection",
                800, 600,
                //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                Common.ApplicationBootstrapper.Container.TryResolve<Cat3>());

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        public static DxThemedWindowHost vncMVVM_V1_DI4_Host = null;

        private void btnVNC_MVVM_V1_DI4_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_V1_DI3_Host,
                "MVVM View First (Cat3) using Dependency Injection",
                800, 600,
                //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                Common.ApplicationBootstrapper.Container.TryResolve<Cat3>());

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        public static DxThemedWindowHost vncMVVM_VM1_DI_Host = null;

        private void btnVNC_MVVM_VM1_DI_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_VM1_DI_Host,
                "MVVM ViewModel First (CatViewModel) using Dependency Injection",
                800, 600,
                //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                Common.ApplicationBootstrapper.Container.TryResolve<CatViewModel>());

            // NOTE(crhodes)
            // Hum.  This works great but somewhat unexpectedly calls the CatViewModel(Cat view) constructor
            // Might be exactly what we want but how would you just call the CatViewModel() constructor?
            // Does it matter if we don't register the Cat View?

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        public static DxThemedWindowHost vncMVVM_V1XamlVM_DI_Host = null;

        private void btnVNC_MVVM_V1XamlVM_DI_Click(object sender, RibbonControlEventArgs e)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.PROJECT_NAME);

            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_V1XamlVM_DI_Host,
                "MVVM View First (CatXamlVM) using Dependency Injection",
                800, 600,
                //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                Common.ApplicationBootstrapper.Container.TryResolve<CatXamlVM>());

            Log.EVENT_HANDLER("Exit", Common.PROJECT_NAME, startTicks);
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using VNCHlp = VNC.AddinHelper;

namespace SupportTools_PowerPoint
{
    public partial class Ribbon
    {
        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        #region Event Handlers
        private void btnPowerPointUtil_Click(object sender, RibbonControlEventArgs e)
        {
            Common.TaskPanePowerPointUtil = VNCHlp.TaskPaneUtil.GetTaskPane(
                () => new User_Interface.Task_Panes.TaskPane_PowerPointUtil(), "Log Parser",
                Globals.ThisAddIn.CustomTaskPanes, Globals.ThisAddIn.Application.HWND.ToString());

            // This works if the minimum size for the control has been set.
            Common.TaskPanePowerPointUtil.Width = Common.TaskPanePowerPointUtil.Control.Width;
            Common.TaskPanePowerPointUtil.Visible = !Common.TaskPanePowerPointUtil.Visible;
        }

        private void btnAddInInfo_Click(object sender, RibbonControlEventArgs e)
        {
            DisplayAddInInfo();
        }

        private void btnComplianceUtilities_Click(object sender, RibbonControlEventArgs e)
        {
            Common.TaskPaneComplianceUtil = VNC.AddinHelper.TaskPaneUtil.GetTaskPane(
                () => new User_Interface.Task_Panes.TaskPane_ComplianceUtil(), "Compliance Utilities",
                Globals.ThisAddIn.CustomTaskPanes,
                Globals.ThisAddIn.Application.HWND.ToString());

            Common.TaskPaneComplianceUtil.Width = Common.TaskPaneComplianceUtil.Control.Width;
            Common.TaskPaneComplianceUtil.Visible = !Common.TaskPaneComplianceUtil.Visible;

            //if (Common.TaskPaneComplianceUtil == null)
            //{
            //    Common.TaskPaneComplianceUtil = VNC.AddinHelper.TaskPaneUtil.AddTaskPane(new User_Interface.Task_Panes.TaskPane_ComplianceUtil(), "Compliance Utilities", Globals.ThisAddIn.CustomTaskPanes);
            //    Common.TaskPaneComplianceUtil.Width = Common.TaskPaneComplianceUtil.Control.Width;
            //}
            //else
            //{
            //    Common.TaskPaneComplianceUtil.Visible = !Common.TaskPaneComplianceUtil.Visible;
            //}
        }

        private void btnDebugWindow_Click(object sender, RibbonControlEventArgs e)
        {
            DisplayDebugWindow();
        }

        private void btnDeveloperMode_Click(object sender, RibbonControlEventArgs e)
        {
            ToggleDeveloperMode();
        }

        private void btnAppUtilities_Click(object sender, RibbonControlEventArgs e)
        {
            Common.TaskPaneAppUtil = VNC.AddinHelper.TaskPaneUtil.GetTaskPane(
                () => new User_Interface.Task_Panes.TaskPane_AppUtil(), "App Utilities",
                Globals.ThisAddIn.CustomTaskPanes,
                Globals.ThisAddIn.Application.HWND.ToString());

            Common.TaskPaneAppUtil.Width = Common.TaskPaneAppUtil.Control.Width;
            Common.TaskPaneAppUtil.Visible = !Common.TaskPaneAppUtil.Visible;

            //if (Common.TaskPaneAppUtil == null)
            //{
            //    Common.TaskPaneAppUtil = VNC.AddinHelper.TaskPaneUtil.AddTaskPane(new User_Interface.Task_Panes.TaskPane_AppUtil(), "App Utilities", Globals.ThisAddIn.CustomTaskPanes);
            //    Common.TaskPaneAppUtil.Width = Common.TaskPaneAppUtil.Control.Width;
            //}
            //else
            //{
            //    Common.TaskPaneAppUtil.Visible = !Common.TaskPaneAppUtil.Visible;
            //}
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

            if (Common.HasAppEvents)
            {
                if (Common.AppEvents == null)
                {
                    Common.AppEvents = new Events.PowerPointAppEvents();
                    Common.AppEvents.PowerPointApplication = Globals.ThisAddIn.Application;
                }
            }
            else
            {
                Common.AppEvents = null;
            }
        }

        #endregion

        #region Main Function Routines


        private void DisplayAddInInfo()
        {
            VNC.AddinHelper.AddInInfo.DisplayInfo();
        }

        private void DisplayDebugWindow()
        {
            if (VNC.AddinHelper.Common.DebugWindow.Visible)
            {
                VNC.AddinHelper.Common.DebugWindow.Visible = false;
            }
            else
            {
                VNC.AddinHelper.Common.DebugWindow.Visible = true;
            }
        }

        private void DisplayWatchWindow()
        {
            VNC.AddinHelper.Common.WatchWindow.Visible = ! VNC.AddinHelper.Common.WatchWindow.Visible;
        }

        private void ToggleDeveloperMode()
        {
            VNC.AddinHelper.Common.DeveloperMode = ! VNC.AddinHelper.Common.DeveloperMode;
            Globals.Ribbons.Ribbon.grpDebug.Visible = VNC.AddinHelper.Common.DeveloperMode;
        }

        #endregion
    }
}

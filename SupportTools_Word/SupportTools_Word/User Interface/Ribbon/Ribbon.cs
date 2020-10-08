using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;

using VNC;

namespace SupportTools_Word
{
    public partial class Ribbon
    {
        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        #region Event Handlers

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

        private void btnAppUtilities_Click(object sender, RibbonControlEventArgs e)
        {
            if (Common.TaskPaneAppUtil == null)
            {
                Common.TaskPaneAppUtil = VNC.AddinHelper.TaskPaneUtil.AddTaskPane(new User_Interface.Task_Panes.TaskPane_AppUtil(), "App Utilities", Globals.ThisAddIn.CustomTaskPanes);
            }
            else
            {
                Common.TaskPaneAppUtil.Visible = !Common.TaskPaneAppUtil.Visible;
            }
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
                    Common.AppEvents = new Events.WordAppEvents();
                    Common.AppEvents.WordApplication = Globals.ThisAddIn.Application;
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

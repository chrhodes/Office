using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Office = Microsoft.Office.Core;

using VNC;

namespace SupportTools_PowerPoint
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            try
            {
                AppLog.Trace("Enter", Common.PROJECT_NAME);
                Common.DeveloperMode = true;
                Common.WriteToDebugWindow("ThisAddIn_Startup()");
                Common.DeveloperMode = false;

                Globals.Ribbons.Ribbon.chkDisplayEvents.Checked = Common.DisplayEvents;
                Globals.Ribbons.Ribbon.chkEnableAppEvents.Checked = Common.HasAppEvents;

                if (Common.HasAppEvents)
                {
                    Common.AppEvents = new Events.PowerPointAppEvents();
                    Common.AppEvents.PowerPointApplication = Globals.ThisAddIn.Application;
                }

                Common.PowerPointHelper.PowerPointApplication = Globals.ThisAddIn.Application;
                AppLog.Trace("Exit", Common.PROJECT_NAME);
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, Common.PROJECT_NAME);
                Common.DeveloperMode = true;
                Common.WriteToDebugWindow(ex.ToString());
                Common.DeveloperMode = false;
                throw (ex);
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            try
            {
                AppLog.Trace("Enter", Common.PROJECT_NAME);
                Common.DeveloperMode = true;
                Common.WriteToDebugWindow("ThisAddIn_Shutdown()");
                Common.DeveloperMode = false;

                if (Common.HasAppEvents)
                {
                    Common.AppEvents = null;
                }

                AppLog.Trace("Exit", Common.PROJECT_NAME);
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, Common.PROJECT_NAME);
                Common.DeveloperMode = true;
                Common.WriteToDebugWindow(ex.ToString());
                Common.DeveloperMode = false;
                throw (ex);
            }
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}

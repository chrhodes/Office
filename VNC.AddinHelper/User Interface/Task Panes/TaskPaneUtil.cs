using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Core;
using MOT=Microsoft.Office.Tools;

namespace VNC.AddinHelper
{
    public class TaskPaneUtil
    {
        // Starting in Office 2013 (SDI vs MDI) there is only one instance of Excel, however, each window
        // can have it's own TaskPanes.  Keep track of TaskPanes and Windows and return existing pane or 
        // create new one for each Window.  This addresses the SDI multiple Workbook problem where only first
        // window had the task pane.

        static Dictionary<string, MOT.CustomTaskPane> _CreatedPanes = new Dictionary<string, MOT.CustomTaskPane>();

        public static MOT.CustomTaskPane GetTaskPane(Func<UserControl> taskPaneCreatorFunc, string taskPaneName, MOT.CustomTaskPaneCollection customTaskPanes, string Hwnd)
        {
            MOT.CustomTaskPane ctp = default(MOT.CustomTaskPane);

            //PLLog.Trace3("Enter", Common.PROJECT_NAME);

            string key = string.Format("{0}({1})", taskPaneName, Hwnd);

            if (_CreatedPanes.ContainsKey(key))
            {
                // Return the existing TaskPane

                ctp = _CreatedPanes[key];
            }
            else
            {
                // Create a new TaskPane and add to the dictionary

                ctp = customTaskPanes.Add(taskPaneCreatorFunc(), taskPaneName);
                _CreatedPanes[key] = ctp;
                ctp.DockPosition = Microsoft.Office.Core.MsoCTPDockPosition.msoCTPDockPositionRight;
                //ctp.Visible = true;
            }

            //PLLog.Trace3("Exit", System.Data.Common.PROJECT_NAME);
            return ctp;
        }

        //public static MOT.CustomTaskPane GetTaskPane(System.Windows.Forms.UserControl taskPane, string name, MOT.CustomTaskPaneCollection customTaskPanes, string Hwnd)
        //{
        //    MOT.CustomTaskPane ctp = default(MOT.CustomTaskPane);

        //    //PLLog.Trace3("Enter", Common.PROJECT_NAME);

        //    string key = string.Format("{0}({1})", name, Hwnd);

        //    if (_CreatedPanes.ContainsKey(key))
        //    {
        //        ctp = _CreatedPanes[key];
        //    }
        //    else
        //    {
        //        ctp = customTaskPanes.Add(taskPane, name);
        //        ctp.DockPosition = Microsoft.Office.Core.MsoCTPDockPosition.msoCTPDockPositionRight;
        //        //ctp.Visible = true;
        //    }

        //    //PLLog.Trace3("Exit", System.Data.Common.PROJECT_NAME);
        //    return ctp;
        //}

        public static MOT.CustomTaskPane AddTaskPane(System.Windows.Forms.UserControl taskPane, string name, MOT.CustomTaskPaneCollection customTaskPanes)
        {
            MOT.CustomTaskPane ctp = default(MOT.CustomTaskPane);

            //PLLog.Trace3("Enter", Common.PROJECT_NAME);

            ctp = customTaskPanes.Add(taskPane, name);
            ctp.DockPosition = Microsoft.Office.Core.MsoCTPDockPosition.msoCTPDockPositionRight;
            ctp.Visible = true;

            //PLLog.Trace3("Exit", System.Data.Common.PROJECT_NAME);
	        return ctp;
        }

        public static void RemoveTaskPane(MOT.CustomTaskPane taskPane, MOT.CustomTaskPaneCollection customTaskPanes)
        {
            //PLLog.Trace3("Enter", System.Data.Common.PROJECT_NAME);

	        customTaskPanes.Remove(taskPane);

            //PLLog.Trace3("Exit", System.Data.Common.PROJECT_NAME);
        }

    }
}

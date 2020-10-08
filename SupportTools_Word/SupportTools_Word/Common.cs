using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupportTools_Word
{
    class Common : VNC.AddinHelper.Common
    {
        new public const string PROJECT_NAME = "SupportTools_Word";

        public static VNC.AddinHelper.Word WordHelper = new VNC.AddinHelper.Word();
        public static Events.WordAppEvents AppEvents;

        //public static Microsoft.Office.Tools.CustomTaskPane TaskPaneConfig;

        //public static Microsoft.Office.Tools.CustomTaskPane TaskPaneHelp;

        public static Microsoft.Office.Tools.CustomTaskPane TaskPaneAppUtil;
    }
}
